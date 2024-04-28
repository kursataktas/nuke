// Copyright 2024 Maintainers of NUKE.
// Distributed under the MIT License.
// https://github.com/nuke-build/nuke/blob/master/LICENSE

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json.Linq;
using Nuke.Common;
using Nuke.Common.Utilities;
using Nuke.Common.Utilities.Collections;

namespace Nuke.Tooling;

partial class ToolOptions
{
    private const string KeyPlaceholder = "{key}";
    private const string ValuePlaceholder = "{value}";

    internal partial IEnumerable<string> GetArguments()
    {
        var optionsType = GetType();
        var commandAttribute = optionsType.GetCustomAttribute<CommandAttribute>();
        var toolAttribute = commandAttribute?.Type.GetCustomAttribute<ToolAttribute>();

        if (toolAttribute?.Arguments != null)
            yield return toolAttribute.Arguments;

        if (commandAttribute?.Arguments != null)
            yield return commandAttribute.Arguments;

        var escapeMethod = CreateEscape();
        var arguments = InternalOptions.Properties()
            .Select(x => (Token: x.Value, Property: GetType().GetProperty(x.Name).NotNull()))
            .Select(x => (x.Token, x.Property, Attribute: x.Property.GetCustomAttribute<ArgumentAttribute>()))
            .Where(x => x.Attribute != null)
            .OrderByDescending(x => x.Attribute.Position.CompareTo(0))
            .ThenBy(x => x.Attribute.Position)
            .SelectMany(x => GetArgument(x.Token, x.Property, x.Attribute, escapeMethod))
            .WhereNotNull();

        foreach (var argument in arguments)
            yield return argument;

        Func<string, PropertyInfo, string> CreateEscape()
        {
            if (toolAttribute?.EscapeMethod == null)
                return (x, _) => x.DoubleQuoteIfNeeded();

            var formatterType = toolAttribute.EscapeType ?? GetType();
            var formatterMethod = formatterType.GetMethod(toolAttribute.EscapeMethod, ReflectionUtility.All);
            return (value, property) => formatterMethod.GetValue<string>(obj: this, args: [value, property]);
        }
    }

    private IEnumerable<string> GetArgument(
        JToken token,
        PropertyInfo property,
        ArgumentAttribute attribute,
        Func<string, PropertyInfo, string> escape)
    {
        var format = attribute.Format;
        var formatParts = format.SplitSpace();

        if (!property.PropertyType.IsGenericType || property.PropertyType.IsNullableType())
            return GetScalarArguments();

        if (property.PropertyType.GetGenericTypeDefinition() == typeof(IReadOnlyList<>))
            return GetListArguments();

        if (property.PropertyType.GetGenericTypeDefinition() == typeof(IReadOnlyDictionary<,>))
            return GetDictionaryArguments();

        if (property.PropertyType.GetGenericTypeDefinition() == typeof(ILookup<,>))
            return GetLookupArguments();

        return [];

        string Parse(JToken token, Type type)
        {
            string value;
            if (attribute.FormatterMethod != null)
            {
                var formatterType = attribute.FormatterType ?? GetType();
                var formatterMethod = formatterType.GetMethod(attribute.FormatterMethod, ReflectionUtility.All);
                value = formatterMethod.GetValue<string>(obj: this, args: [token.ToObject(type), property]);
            }
            else
            {
                value = token.ToObject<string>();
                if (new[] { typeof(bool), typeof(bool?) }.Contains(type))
                    value = value.ToLowerInvariant();
            }

            return escape.Invoke(value, property);
        }

        IEnumerable<string> GetScalarArguments()
        {
            if (property.PropertyType == typeof(bool?) &&
                !format.ContainsOrdinalIgnoreCase(ValuePlaceholder) &&
                !token.Value<bool>())
                yield break;

            var value = Parse(token, property.PropertyType);
            foreach (var part in formatParts)
                yield return part.Replace(ValuePlaceholder, value);
        }

        IEnumerable<string> GetListArguments()
        {
            Assert.True(formatParts.Length <= 2);

            var valueType = property.PropertyType.GetScalarType();
            var values = token.Value<JArray>().Select(x => Parse(x, valueType));

            if (attribute.Separator == null)
            {
                return from value in values
                       from part in formatParts
                       select part.Replace(ValuePlaceholder, value);
            }
            else
            {
                Assert.False(attribute.Separator.IsNullOrWhiteSpace());
                return from part in formatParts
                       select part.Replace(ValuePlaceholder, values.Join(attribute.Separator));
            }
        }

        IEnumerable<string> GetDictionaryArguments()
        {
            var valueType = property.PropertyType.GetGenericArguments().Last();
            var pairs = token.Value<JObject>().Properties().Select(x => (Key: x.Name, Value: Parse(x, valueType)));

            if (attribute.Separator == null)
            {
                return from pair in pairs
                       from part in formatParts
                       select part.Replace(KeyPlaceholder, pair.Key).Replace(ValuePlaceholder, pair.Value);
            }
            else
            {
                Assert.False(attribute.Separator.IsNullOrWhiteSpace());
                var (keyIndex, valueIndex) = (
                    format.IndexOf(KeyPlaceholder, StringComparison.OrdinalIgnoreCase),
                    format.IndexOf(ValuePlaceholder, StringComparison.OrdinalIgnoreCase)
                );

                return from part in formatParts
                       select part.Replace(ValuePlaceholder, values.Join(attribute.Separator));
            }
            //
            // var (keyIndex, valueIndex) = (
            //     format.IndexOf(KeyPlaceholder, StringComparison.OrdinalIgnoreCase),
            //     format.IndexOf(ValuePlaceholder, StringComparison.OrdinalIgnoreCase)
            // );
            // Assert.True(keyIndex > 0 && valueIndex > 0);
            // var itemSeparator = format.Substring(
            //     startIndex: keyIndex + KeyPlaceholder.Length,
            //     length: valueIndex - keyIndex - KeyPlaceholder.Length);
            //
            // foreach (var value in values.Properties())
            // {
            //     // yield return first;
            //     yield return value.Name;
            //     yield return Format(value.Value, itemType);
            // }
        }

        IEnumerable<string> GetLookupArguments()
        {
            var valueType = property.PropertyType.GetGenericArguments().Last();
            var pairs = token.Value<JObject>().Properties()
                .Select(x => (Key: x.Name, Values: x.Value<JArray>().Select(x => Parse(x, valueType))));
            return [];
        }
    }
}
