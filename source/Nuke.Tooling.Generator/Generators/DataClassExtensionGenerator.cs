// Copyright 2023 Maintainers of NUKE.
// Distributed under the MIT License.
// https://github.com/nuke-build/nuke/blob/master/LICENSE

using System;
using System.Linq;
using Nuke.CodeGeneration.Model;
using Nuke.CodeGeneration.Writers;
using Nuke.Common.Utilities;
using Serilog;

// ReSharper disable UnusedMethodReturnValue.Local

namespace Nuke.CodeGeneration.Generators;

public static class DataClassExtensionGenerator
{
    public static void Run(DataClass dataClass, ToolWriter toolWriter)
    {
        var writer = new DataClassWriter(dataClass, toolWriter);
        writer
            .WriteLine($"#region {dataClass.Name}Extensions")
            .WriteSummary(dataClass)
            .WriteLine("[PublicAPI]")
            .WriteObsoleteAttributeWhenObsolete(dataClass)
            .WriteLine("[ExcludeFromCodeCoverage]")
            .WriteLine($"public static partial class {dataClass.Name}Extensions")
            .WriteBlock(w => w.ForEach(dataClass.Properties, WriteMethods))
            .WriteLine("#endregion");
    }

    private static void WriteMethods(DataClassWriter writer, Property property)
    {
        if (property.CustomImpl)
            return;

        writer.WriteLine($"#region {property.Name}");
        var access = $"o.{property.Name}";

        if (!property.IsList() && !property.IsDictionary() && !property.IsLookupTable())
        {
            var attributes = property.Secret ?? false ? "[Secret] " : string.Empty;
            writer
                .WriteMethod(
                    property,
                    name: $"Set{property.Name}",
                    additionalParameters: [$"{attributes}{property.GetNullableType()} v"],
                    modification: $"Set(() => {access}, v)")
                .WriteMethod(
                    property,
                    name: $"Reset{property.Name}",
                    additionalParameters: [],
                    modification: $"Remove(() => {access})");
        }

        // bool
        if (property.IsBoolean())
        {
            writer
                // Enable
                .WriteMethod(
                    property,
                    name: $"Enable{property.Name}",
                    additionalParameters: [],
                    modification: $"Set(() => {access}, true)")
                // Disable
                .WriteMethod(
                    property,
                    name: $"Disable{property.Name}",
                    additionalParameters: [],
                    modification: $"Set(() => {access}, false)")
                // Toggle
                .WriteMethod(
                    property,
                    name: $"Toggle{property.Name}",
                    additionalParameters: [],
                    modification: $"Set(() => {access}, !{access})");
        }

        // List<T>
        if (property.IsList())
        {
            CheckPlural(property);
            var valueType = property.GetListValueType();
            writer
                // Set
                .WriteMethod(
                    property,
                    name: $"Set{property.Name}",
                    additionalParameters: [$"params {valueType}[] v"],
                    modification: $"Set(() => {access}, v)")
                .WriteMethod(
                    property,
                    name: $"Set{property.Name}",
                    additionalParameters: [$"IEnumerable<{valueType}> v"],
                    modification: $"Set(() => {access}, v)")
                // Add
                .WriteMethod(
                    property,
                    name: $"Add{property.Name}",
                    additionalParameters: [$"params {valueType}[] v"],
                    modification: $"AddCollection(() => {access}, v)")
                .WriteMethod(
                    property,
                    name: $"Add{property.Name}",
                    additionalParameters: [$"IEnumerable<{valueType}> v"],
                    modification: $"AddCollection(() => {access}, v)")
                .When(property.HasCustomListType(), _ => _
                    .WriteMethod(
                        property,
                        name: $"Add{property.Name}",
                        additionalParameters: [$"Configure<{valueType}> v"],
                        modification: $"AddCollection(() => {access}, v.InvokeSafe(new()))"))
                // Remove
                .WriteMethod(
                    property,
                    name: $"Remove{property.Name}",
                    additionalParameters: [$"params {valueType}[] v"],
                    modification: $"RemoveCollection(() => {access}, v)")
                .WriteMethod(
                    property,
                    name: $"Remove{property.Name}",
                    additionalParameters: [$"IEnumerable<{valueType}> v"],
                    modification: $"RemoveCollection(() => {access}, v)")
                // Clear
                .WriteMethod(
                    property,
                    name: $"Clear{property.Name}",
                    additionalParameters: [],
                    modification: $"ClearCollection(() => {access})");
        }

        // Dictionary<TKey, TValue>
        if (property.IsDictionary())
        {
            CheckPlural(property);
            if (!property.OnlyDelegates)
            {
                var (keyType, valueType) = property.GetDictionaryKeyValueTypes();

                writer
                    // Set
                    .WriteMethod(
                        property,
                        name: $"Set{property.Name}",
                        additionalParameters: [$"IDictionary<{keyType}, {valueType}> v"],
                        modification: $"Set(() => {access}, v.ToDictionary(x => x.Key, x => x.Value, {property.GetKeyComparer()}))")
                    // Set item
                    .WriteMethod(
                        property,
                        name: $"Set{property.Name.ToSingular()}",
                        additionalParameters: [$"{keyType} k", $"{valueType} v"],
                        modification: $"SetDictionary(() => {access}, k, v)")
                    // Add
                    .WriteMethod(
                        property,
                        name: $"Add{property.Name.ToSingular()}",
                        additionalParameters: [$"{keyType} k", $"{valueType} v"],
                        modification: $"AddDictionary(() => {access}, k, v)")
                    // Remove
                    .WriteMethod(
                        property,
                        name: $"Remove{property.Name.ToSingular()}",
                        additionalParameters: [$"{keyType} k"],
                        modification: $"RemoveDictionary(() => {access}, k)")
                    // Clear
                    .WriteMethod(
                        property,
                        name: $"Clear{property.Name}",
                        additionalParameters: [],
                        modification: $"ClearDictionary(() => {access})");
            }

            // dictionary["well-known-key"]
            property.Delegates.ForEach(WriteDelegateProperty);
        }

        // else if (property.IsLookupTable())
        //     WriteLookupExtensions(writer, property);

        writer.WriteLine("#endregion");
        return;

        void WriteDelegateProperty(Property delegateProperty)
        {
            writer.WriteLine($"#region {delegateProperty.Name}");
            var keyValue = delegateProperty.Name.DoubleQuote();

            if (!delegateProperty.IsList())
            {
                writer
                    .WriteMethod(
                        property,
                        name: $"Set{delegateProperty.Name}",
                        additionalParameters: [$"{delegateProperty.GetNullableType()} v"],
                        modification: $"SetDictionary(() => {access}, {keyValue}, v)",
                        delegateProperty.Help)
                    .WriteMethod(
                        property,
                        name: $"Reset{delegateProperty.Name}",
                        additionalParameters: [],
                        modification: $"RemoveDictionary(() => {access}, {keyValue})",
                        delegateProperty.Help);
            }

            // bool
            if (delegateProperty.IsBoolean())
            {
                writer
                    // Enable
                    .WriteMethod(
                        property,
                        name: $"Enable{delegateProperty.Name}",
                        additionalParameters: [],
                        modification: $"SetDictionary(() => {access}, {keyValue}, true)",
                        delegateProperty.Help)
                    // Disable
                    .WriteMethod(
                        property,
                        name: $"Disable{delegateProperty.Name}",
                        additionalParameters: [],
                        modification: $"SetDictionary(() => {access}, {keyValue}, false)",
                        delegateProperty.Help)
                    // Toggle
                    .WriteMethod(
                        property,
                        name: $"Toggle{delegateProperty.Name}",
                        additionalParameters: [],
                        modification: $"Set(() => {access}, DelegateHelper.Toggle({access}, {keyValue}))",
                        delegateProperty.Help);
            }

            // List<T>
            if (delegateProperty.IsList())
            {
                CheckPlural(delegateProperty);
                var valueType = delegateProperty.GetListValueType();
                var propertyPlural = delegateProperty.Name.ToPlural();
                var separator = delegateProperty.Separator.DoubleQuote();

                writer
                    // Set
                    .WriteMethod(
                        property,
                        name: $"Set{propertyPlural}",
                        additionalParameters: [$"params {valueType}[] v"],
                        modification: $"Set(() => {access}, DelegateHelper.SetCollection({access}, {keyValue}, v, {separator}))",
                        delegateProperty.Help)
                    .WriteMethod(
                        property,
                        name: $"Set{propertyPlural}",
                        additionalParameters: [$"IEnumerable<{valueType}> v"],
                        modification: $"Set(() => {access}, DelegateHelper.SetCollection({access}, {keyValue}, v, {separator}))",
                        delegateProperty.Help)
                    // Add
                    .WriteMethod(
                        property,
                        name: $"Add{propertyPlural}",
                        additionalParameters: [$"params {valueType}[] v"],
                        modification: $"Set(() => {access}, DelegateHelper.AddCollection({access}, {keyValue}, v, {separator}))",
                        delegateProperty.Help)
                    .WriteMethod(
                        property,
                        name: $"Add{propertyPlural}",
                        additionalParameters: [$"IEnumerable<{valueType}> v"],
                        modification: $"Set(() => {access}, DelegateHelper.AddCollection({access}, {keyValue}, v, {separator}))",
                        delegateProperty.Help)
                    // Remove
                    .WriteMethod(
                        property,
                        name: $"Remove{propertyPlural}",
                        additionalParameters: [$"params {valueType}[] v"],
                        modification: $"Set(() => {access}, DelegateHelper.RemoveCollection({access}, {keyValue}, v, {separator}))",
                        delegateProperty.Help)
                    .WriteMethod(
                        property,
                        name: $"Remove{propertyPlural}",
                        additionalParameters: [$"IEnumerable<{valueType}> v"],
                        modification: $"Set(() => {access}, DelegateHelper.RemoveCollection({access}, {keyValue}, v, {separator}))",
                        delegateProperty.Help)
                    // Clear
                    .WriteMethod(
                        property,
                        name: $"Reset{delegateProperty.Name}",
                        additionalParameters: [],
                        modification: $"RemoveDictionary(() => {access}, {keyValue})",
                        delegateProperty.Help);
            }

            writer.WriteLine("#endregion");
        }
    }

    private static void WriteLookupExtensions(DataClassWriter writer, Property property)
    {
        // var propertyInstance = property.Name.ToInstance();
        // var (keyType, valueType) = property.GetLookupTableKeyValueTypes();
        // var propertySingular = property.Name.ToSingular();
        // var propertySingularInstance = property.Name.ToSingular().ToInstance();
        // var keyInstance = $"{propertyInstance}Key";
        // var valueInstance = $"{propertyInstance}Value";
        // var valueInstances = $"{propertyInstance}Values";
        // var propertyAccess = $"toolSettings.{property.Name}Internal";
        //
        // // TODO: params
        // // TODO: remove by key
        // writer
        //     .WriteSummaryExtension($"Sets {property.GetCrefTag()} to a new lookup table", property)
        //     .WriteObsoleteAttributeWhenObsolete(property)
        //     .WriteMethod($"Set{property.Name}",
        //         $"ILookup<{keyType}, {valueType}> {propertyInstance}",
        //         $"{propertyAccess} = {propertyInstance}.ToLookupTable(StringComparer.OrdinalIgnoreCase);")
        //     .WriteSummaryExtension($"Clears {property.GetCrefTag()}", property)
        //     .WriteObsoleteAttributeWhenObsolete(property)
        //     .WriteMethod($"Clear{property.Name}",
        //         $"{propertyAccess}.Clear();")
        //     .WriteSummaryExtension($"Adds new values for the given key to {property.GetCrefTag()}", property)
        //     .WriteObsoleteAttributeWhenObsolete(property)
        //     .WriteMethod($"Add{property.Name}",
        //         new[] { $"{keyType} {keyInstance}", $"params {valueType}[] {valueInstances}" },
        //         $"{propertyAccess}.AddRange({keyInstance}, {valueInstances});")
        //     .WriteSummaryExtension($"Adds new values for the given key to {property.GetCrefTag()}", property)
        //     .WriteObsoleteAttributeWhenObsolete(property)
        //     .WriteMethod($"Add{property.Name}",
        //         new[] { $"{keyType} {keyInstance}", $"IEnumerable<{valueType}> {valueInstances}" },
        //         $"{propertyAccess}.AddRange({keyInstance}, {valueInstances});")
        //     .WriteSummaryExtension($"Removes a single {propertySingularInstance} from {property.GetCrefTag()}", property)
        //     .WriteObsoleteAttributeWhenObsolete(property)
        //     .WriteMethod($"Remove{propertySingular}",
        //         new[] { $"{keyType} {keyInstance}", $"{valueType} {valueInstance}" },
        //         $"{propertyAccess}.Remove({keyInstance}, {valueInstance});");
    }

    private static DataClassWriter WriteMethod(
        this DataClassWriter writer,
        Property property,
        string name,
        string[] additionalParameters,
        string modification,
        string help = null)
    {
        var builder = $"[Builder(Type = typeof({writer.DataClass.Name}), Property = nameof({writer.DataClass.Name}.{property.Name}))]";
        var parameters = new[] { "this T o" }.Concat(additionalParameters);
        var signature = $"public static T {name}<T>({parameters.JoinCommaSpace()}) where T : {writer.DataClass.Name}";
        var implementation = $"o.Modify(b => b.{modification})";
        return writer
            .WriteLineIfTrue(help == null, $"/// <inheritdoc cref=\"{writer.DataClass.Name}.{property.Name}\"/>")
            .WriteLineIfTrue(help != null, $"/// <summary>{help}</summary>")
            .WriteLine($"[Pure] {builder}")
            .WriteObsoleteAttributeWhenObsolete(property)
            .WriteLine($"{signature} => {implementation};");
    }

    private static void CheckPlural(Property property)
    {
        if (property.Name.ToPlural() != property.Name)
            Log.Warning("Property {Class}.{Property} should be pluralized", property.DataClass.Name, property.Name);
    }
}
