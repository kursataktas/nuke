// Copyright 2024 Maintainers of NUKE.
// Distributed under the MIT License.
// https://github.com/nuke-build/nuke/blob/master/LICENSE

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Nuke.Common.Tooling;
using Nuke.Common.Utilities;
using Nuke.Common.Utilities.Collections;
using Nuke.Tooling;
using VerifyXunit;
using Xunit;

namespace Nuke.Common.Tests;

public class OptionsTest
{
    // ReSharper disable UnassignedGetOnlyAutoProperty
    private string ScalarValue { get; }
    private List<int> ListValue { get; }
    private ReadOnlyDictionary<string, string> DictionaryValue { get; }
    private LookupTable<string, string> LookupValue { get; }
    private Options NestedValue { get; }
    // ReSharper restore UnassignedGetOnlyAutoProperty

    [Fact]
    public void TestScalar()
    {
        var options = new Options();

        options.Set(() => ScalarValue, 1);
        options.Modify().Get<int>(() => ScalarValue).Should().Be(1);

        options.Set(() => ScalarValue, "string");
        options.Modify().Get<string>(() => ScalarValue).Should().Be("string");

        options.Set(() => ScalarValue, true);
        options.Modify().Get<bool>(() => ScalarValue).Should().Be(true);

        options.Set(() => ScalarValue, OutputType.Err);
        options.Modify().Get<OutputType>(() => ScalarValue).Should().Be(OutputType.Err);

        options.Set(() => ScalarValue, null);
        options.Modify().Get<OutputType?>(() => ScalarValue).Should().BeNull();

        options.Set(() => ScalarValue, CustomEnumeration.Quiet);
        options.Modify().Get<CustomEnumeration>(() => ScalarValue).Should().Be(CustomEnumeration.Quiet);
    }

    [Fact]
    public void TestList()
    {
        var options = new Options();

        options.Set(() => ListValue, new[] { 1, 2, 3 });
        options.Modify().Get<List<int>>(() => ListValue).Should().Equal(1, 2, 3);

        options.AddCollection(() => ListValue, [4, 5]);
        options.Modify().Get<List<int>>(() => ListValue).Should().EndWith(new[] { 4, 5 });

        options.RemoveCollection(() => ListValue, [2, 4]);
        options.Modify().Get<List<int>>(() => ListValue).Should().NotContain(new[] { 2, 4 });

        options.ClearCollection(() => ListValue);
        options.Modify().Get<List<int>>(() => ListValue).Should().BeEmpty();
    }

    [Fact]
    public void TestDictionary()
    {
        var options = new Options();

        options.SetDictionary(() => DictionaryValue, "key", "value");
        options.Modify().Get<Dictionary<string, string>>(() => DictionaryValue).Should().ContainKey("key").WhoseValue.Should().Be("value");

        options.AddDictionary(() => DictionaryValue, "key2", "value");
        options.Modify().Get<Dictionary<string, string>>(() => DictionaryValue).Should().HaveCount(2);

        options.RemoveDictionary(() => DictionaryValue, "key");
        options.Modify().Get<Dictionary<string, string>>(() => DictionaryValue).Should().ContainKey("key2");

        options.ClearDictionary(() => DictionaryValue);
        options.Modify().Get<Dictionary<string, string>>(() => DictionaryValue).Should().BeEmpty();
    }

    [Fact]
    public void TestLookup()
    {
        var options = new Options();

        options.SetLookup(() => LookupValue, "key", "value1", "value2");
        options.Modify().Get<LookupTable<string, string>>(() => LookupValue).Should()
            .Contain(x => x.Key == "key" && x.SequenceEqual(new[] { "value1", "value2" }));

        options.AddLookup(() => LookupValue, "key", "value3");
        options.AddLookup(() => LookupValue, "key2", "value1");
        options.Modify().Get<LookupTable<string, string>>(() => LookupValue).Should().Contain(x => x.Key == "key" && x.Last() == "value3");
        options.Modify().Get<LookupTable<string, string>>(() => LookupValue).Should().Contain(x => x.Key == "key2");

        options.RemoveLookup(() => LookupValue, "key", "value2");
        options.RemoveLookup(() => LookupValue, "key2");
        options.Modify().Get<LookupTable<string, string>>(() => LookupValue).Should().Contain(x => x.Key == "key" && x.Count() == 2);
        options.Modify().Get<LookupTable<string, string>>(() => LookupValue).Should().HaveCount(1);

        options.ClearLookup(() => LookupValue);
        options.Modify().Get<LookupTable<string, string>>(() => LookupValue).Should().BeEmpty();
    }

    [Fact]
    public void TestNested()
    {
        var options = new Options()
            .Set(() => NestedValue, new Options()
                .Set(() => ScalarValue, "scalar-value")
                .Set(() => ListValue, new[] { 1, 2, 3 })
                .Set(() => DictionaryValue, new Dictionary<string, object> { ["key"] = "value" })
                .Set(() => LookupValue, new LookupTable<string, int> { ["key"] = new[] { 1, 2, 3 } }));

        var nestedOptions = options.Get<Options>(() => NestedValue);
        nestedOptions.Get<string>(() => ScalarValue).Should().Be("scalar-value");
    }

    [Fact]
    public Task TestSerialization()
    {
        var options = new Options()
            .Set(() => ScalarValue, "scalar-value")
            .Set(() => ListValue, new[] { 1, 2, 3 })
            .Set(() => DictionaryValue, new Dictionary<string, object> { ["key"] = "value" })
            .Set(() => LookupValue, new LookupTable<string, int> { ["key"] = new[] { 1, 2, 3 } });
        options.Set(() => NestedValue, options);

        return Verifier.Verify(options.ToJson(Options.JsonSerializerSettings));
    }
}

[TypeConverter(typeof(TypeConverter<CustomEnumeration>))]
public class CustomEnumeration : Enumeration
{
    public static CustomEnumeration Quiet = "quiet";
    public static implicit operator CustomEnumeration(string value)
    {
        return new CustomEnumeration { Value = value };
    }
}
