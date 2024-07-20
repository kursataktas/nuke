// Copyright 2024 Maintainers of NUKE.
// Distributed under the MIT License.
// https://github.com/nuke-build/nuke/blob/master/LICENSE

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Newtonsoft.Json;
using Nuke.Common.Utilities.Collections;
using Nuke.Tooling;
using Xunit;

namespace Nuke.Common.Tests;

public class ToolOptionsArgumentsTest
{
    [Fact]
    public void TestBool()
    {
        Assert<BoolToolOptions>(new { Bool = true }, ["/bool:true",]);
        Assert<BoolToolOptions>(new { Flag = true }, ["/flag"]);
        Assert<BoolToolOptions>(new { Flag = false }, []);
    }

    private class BoolToolOptions : ToolOptions
    {
        [Argument(Format = "/bool:{value}")] public bool Bool => Get<bool>(() => Bool);
        [Argument(Format = "/flag")] public bool? Flag => Get<bool>(() => Flag);
    }

    [Fact]
    public void TestString()
    {
        Assert<StringToolOptions>(new { String = "value" }, ["--string", "value",]);
    }

    private class StringToolOptions : ToolOptions
    {
        [Argument(Format = "--string {value}")] public string String => Get<string>(() => String);
    }

    [Fact]
    public void TestImplicit()
    {
        Assert<ImplicitToolOptions>(new { String = "value" }, ["first", "second", "--string", "value"]);
    }

    [NuGetTool(Arguments = "first")]
    private class ImplicitTool;

    [Command(Type = typeof(ImplicitTool), Arguments = "second")]
    private class ImplicitToolOptions : ToolOptions
    {
        [Argument(Format = "--string {value}")] public string String => Get<string>(() => String);
    }

    [Fact]
    public void TestOrder()
    {
        // ReSharper disable SimilarAnonymousTypeNearby
        Assert<OrderToolOptions>(new { Flag1 = true, Flag2 = true, }, ["/flag1", "/flag2"]);
        Assert<OrderToolOptions>(new { Flag2 = true, Flag1 = true, }, ["/flag2", "/flag1"]);
        // ReSharper restore SimilarAnonymousTypeNearby
    }

    private class OrderToolOptions : ToolOptions
    {
        [Argument(Format = "/flag1")] public bool Flag1 => Get<bool>(() => Flag1);
        [Argument(Format = "/flag2")] public bool Flag2 => Get<bool>(() => Flag2);
    }

    [Fact]
    public void TestPosition()
    {
        Assert<PositionToolOptions>(
            new
            {
                SecondToLast = "second-last",
                Second = "second",
                Middle = "middle",
                First = "first",
                Last = "last"
            },
            arguments: ["first", "second", "middle", "second-last", "last"]);
    }

    private class PositionToolOptions : ToolOptions
    {
        [Argument(Format = "{value}")] public string Middle => Get<string>(() => Middle);
        [Argument(Format = "{value}", Position = -1)] public string Last => Get<string>(() => Last);
        [Argument(Format = "{value}", Position = 1)] public string First => Get<string>(() => First);
        [Argument(Format = "{value}", Position = 2)] public string Second => Get<string>(() => Second);
        [Argument(Format = "{value}", Position = -2)] public string SecondToLast => Get<string>(() => SecondToLast);
    }

    [Fact]
    public void TestFormatter()
    {
        Assert<FormatToolOptions>(new { Time = DateTime.UnixEpoch.AddHours(1).AddMinutes(15) }, ["01:15"]);
        Assert<FormatToolOptions>(new { Date = DateTime.UnixEpoch }, ["01/01/1970"]);
        Assert<FormatToolOptions>(new { Minutes = TimeSpan.FromMinutes(10) }, ["10"]);
    }

    private class FormatToolOptions : ToolOptions
    {
        [Argument(Format = "{value}", FormatterMethod = nameof(FormatTime))]
        public DateTime Time => Get<DateTime>(() => Time);

        [Argument(Format = "{value}", FormatterMethod = nameof(FormatDate))]
        public DateTime Date => Get<DateTime>(() => Date);

        [Argument(Format = "{value}", FormatterType = typeof(Formatter), FormatterMethod = nameof(Formatter.FormatMinutes))]
        public TimeSpan Minutes => Get<TimeSpan>(() => Minutes);

        private string FormatTime(DateTime datetime, PropertyInfo property) => datetime.ToString("t", CultureInfo.InvariantCulture);
        private string FormatDate(DateTime datetime, PropertyInfo property) => datetime.ToString("d", CultureInfo.InvariantCulture);
    }

    private static class Formatter
    {
        public static string FormatMinutes(TimeSpan timespan, PropertyInfo _) => timespan.TotalMinutes.ToString(CultureInfo.InvariantCulture);
    }

    [Fact]
    public void TestList()
    {
        Assert<ListToolOptions>(new { SimpleList = new[] { "a", "b" } }, ["--param", "a", "--param", "b"]);
        Assert<ListToolOptions>(new { SeparatorList = new[] { "a", "b" } }, ["--param", "a+b"]);
        Assert<ListToolOptions>(new { WhitespaceList = new[] { "a", "b" } }, ["--param", "a", "b"]);
        Assert<ListToolOptions>(new { FormattedList = new[] { "true", "false" } }, ["--param=TRUE", "--param=FALSE"]);
    }

    private class ListToolOptions : ToolOptions
    {
        [Argument(Format = "--param {value}")] public IReadOnlyList<string> SimpleList => Get<List<string>>(() => SimpleList);
        [Argument(Format = "--param {value}", ListSeparator = "+")] public IReadOnlyList<string> SeparatorList => Get<List<string>>(() => SeparatorList);
        [Argument(Format = "--param {value}", ListSeparator = " ")] public IReadOnlyList<string> WhitespaceList => Get<List<string>>(() => SeparatorList);
        [Argument(Format = "--param={value}", FormatterMethod = nameof(Format))] public IReadOnlyList<bool> FormattedList => Get<List<bool>>(() => FormattedList);

        private string Format(bool value, PropertyInfo property) => value.ToString().ToUpperInvariant();
    }

    [Fact]
    public void TestDictionary()
    {
        var dictionary = new Dictionary<string, object> { ["key1"] = 1, ["key2"] = "foobar" };
        Assert<DictionaryToolOptions>(new { SimpleDictionary = dictionary }, ["-p", "key1=1", "-p", "key2=foobar"]);
        Assert<DictionaryToolOptions>(new { Simple2Dictionary = dictionary }, ["-p", "key1", "1", "-p", "key2", "foobar"]);
        Assert<DictionaryToolOptions>(new { SeparatorDictionary = dictionary }, ["/p:key1=1;key2=foobar"]);
        Assert<DictionaryToolOptions>(new { WhitespaceDictionary = dictionary }, ["--", "key1=1", "key2=foobar"]);

        var boolDictionary = new Dictionary<string, bool> { ["key1"] = true, ["key2"] = false };
        Assert<DictionaryToolOptions>(new { FormattedDictionary = boolDictionary }, ["/p:key1=TRUE", "/p:key2=FALSE"]);
    }

    private class DictionaryToolOptions : ToolOptions
    {
        [Argument(Format = "-p {key}={value}")] public IReadOnlyDictionary<string, object> SimpleDictionary => Get<Dictionary<string, object>>(() => SimpleDictionary);
        [Argument(Format = "-p {key} {value}")] public IReadOnlyDictionary<string, object> Simple2Dictionary => Get<Dictionary<string, object>>(() => Simple2Dictionary);
        [Argument(Format = "/p:{key}={value}", PairSeparator = ";")] public IReadOnlyDictionary<string, object> SeparatorDictionary => Get<Dictionary<string, object>>(() => SeparatorDictionary);
        [Argument(Format = "-- {key}={value}", PairSeparator = " ")] public IReadOnlyDictionary<string, object> WhitespaceDictionary => Get<Dictionary<string, object>>(() => WhitespaceDictionary);
        [Argument(Format = "/p:{key}={value}", FormatterMethod = nameof(Format))] public IReadOnlyDictionary<string, bool> FormattedDictionary => Get<Dictionary<string, bool>>(() => FormattedDictionary);

        private string Format(bool value, PropertyInfo property) => value.ToString().ToUpperInvariant();
    }

    private class LookupToolOptions : ToolOptions
    {
        [Argument(Format = "--param {key}={value}", ListSeparator = ",", PairSeparator = " ")] public ILookup<string, object> SimpleLookup => Get<LookupTable<string, object>>(() => SimpleLookup);
        [Argument(Format = "--param {key} {value}", ListSeparator = ",", PairSeparator = " ")] public ILookup<string, object> Simple2Lookup => Get<LookupTable<string, object>>(() => Simple2Lookup);
    }

    [Fact]
    public void TestLookup()
    {
        var lookup = new LookupTable<string, object> { ["key1"] = new object[] { 1, 2 }, ["key2"] = new object[] { true, false } };
        Assert<LookupToolOptions>(new { SimpleLookup = lookup }, ["--param", "key1=1,2", "key2=true,false"]);
        Assert<LookupToolOptions>(new { Simple2Lookup = lookup }, ["--param", "key1", "1,2", "key2", "true,false"]);
    }

    private void Assert<T>(object obj, params string[] arguments)
        where T : ToolOptions, new()
    {
        var options = new T();
        options.InternalOptions = obj.ToJObject(Options.JsonSerializer);
        options.GetArguments().Should().Equal(arguments);
    }
}
