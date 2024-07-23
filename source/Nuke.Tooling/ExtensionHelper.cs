// Copyright 2023 Maintainers of NUKE.
// Distributed under the MIT License.
// https://github.com/nuke-build/nuke/blob/master/LICENSE

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Nuke.Common.Utilities;
using static Nuke.Common.Utilities.ReflectionUtility;

namespace Nuke.Common.Tooling;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public static class ExtensionHelper
{
    public static void ToggleBoolean(IDictionary dictionary, string key)
    {
        dictionary[key] = !dictionary.Contains(key) || !Convert<bool>(dictionary[key].ToString());
    }

    public static void SetCollection<T>(IDictionary dictionary, string key, IEnumerable<T> values, char separator)
    {
        var collectionAsString = CollectionToString(values, separator);
        if (string.IsNullOrWhiteSpace(collectionAsString))
            return;

        dictionary[key] = collectionAsString;
    }

    public static void AddItems<T>(IDictionary dictionary, string key, IEnumerable<T> values, char separator)
    {
        var collection = ParseCollection<T>(dictionary, key, separator);
        collection.AddRange(values);
        dictionary[key] = CollectionToString(collection, separator);
    }

    public static void RemoveItems<T>(IDictionary dictionary, string key, IEnumerable<T> values, char separator)
    {
        var valueHashSet = new HashSet<T>(values);
        var collection = ParseCollection<T>(dictionary, key, separator);
        collection.RemoveAll(x => valueHashSet.Contains(x));
        dictionary[key] = CollectionToString(collection, separator);
    }

    private static List<TValue> ParseCollection<TValue>(IDictionary dictionary, string key, char separator)
    {
        return (dictionary.Contains(key)
                ? ((string) dictionary[key]).Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries)
                : new string[0])
            .Select(Convert<TValue>).ToList();
    }

    private static string CollectionToString<T>(IEnumerable<T> collection, char separator)
    {
        return collection.Select(x => x.ToString()).Join(separator);
    }
}

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public static class ExtensionHelper2
{
    public static IDictionary<string, object> Toggle(IReadOnlyDictionary<string, object> dictionary, string key)
    {
        var newDictionary = dictionary.ToDictionary(x => x.Key, x => x.Value);
        newDictionary[key] = !dictionary.ContainsKey(key) || !Convert<bool>(dictionary[key].ToString());
        return newDictionary;
    }

    public static IDictionary<string, object> SetCollection<TValue>(IReadOnlyDictionary<string, object> dictionary, string key, IEnumerable<TValue> values, string separator)
    {
        var newDictionary = dictionary.ToDictionary(x => x.Key, x => x.Value);
        var collectionAsString = CollectionToString(values, separator);
        if (!string.IsNullOrWhiteSpace(collectionAsString))
            newDictionary[key] = collectionAsString;
        return newDictionary;
    }

    public static IDictionary<string, object> AddItems<TValue>(IReadOnlyDictionary<string, object> dictionary, string key, IEnumerable<TValue> values, string separator)
    {
        var newDictionary = dictionary.ToDictionary(x => x.Key, x => x.Value);
        var collection = ParseCollection<TValue>(dictionary, key, separator);
        collection.AddRange(values);
        newDictionary[key] = CollectionToString(collection, separator);
        return newDictionary;
    }

    public static IDictionary<string, object> RemoveItems<TValue>(IReadOnlyDictionary<string, object> dictionary, string key, IEnumerable<TValue> values, string separator)
    {
        var newDictionary = dictionary.ToDictionary(x => x.Key, x => x.Value);
        var valueHashSet = new HashSet<TValue>(values);
        var collection = ParseCollection<TValue>(dictionary, key, separator);
        collection.RemoveAll(x => valueHashSet.Contains(x));
        newDictionary[key] = CollectionToString(collection, separator);
        return newDictionary;
    }

    private static List<TValue> ParseCollection<TValue>(IReadOnlyDictionary<string, object> dictionary, string key, string separator)
    {
        return (dictionary.TryGetValue(key, out var value)
                ? ((string)value).Split([separator], StringSplitOptions.RemoveEmptyEntries)
                : new string[0])
            .Select(Convert<TValue>).ToList();
    }

    private static string CollectionToString<T>(IEnumerable<T> collection, string separator)
    {
        return collection.Select(x => x.ToString()).Join(separator);
    }
}
