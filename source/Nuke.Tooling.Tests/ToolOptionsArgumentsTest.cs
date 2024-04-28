// Copyright 2024 Maintainers of NUKE.
// Distributed under the MIT License.
// https://github.com/nuke-build/nuke/blob/master/LICENSE

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using FluentAssertions;
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
        Assert<ListToolOptions>(new { FormattedList = new[] { "true", "false" } }, ["--param=TRUE", "--param=FALSE"]);
    }

    private class ListToolOptions : ToolOptions
    {
        [Argument(Format = "--param {value}")] public IReadOnlyList<string> SimpleList => Get<List<string>>(() => SimpleList);
        [Argument(Format = "--param {value}", Separator = "+")] public IReadOnlyList<string> SeparatorList => Get<List<string>>(() => SeparatorList);
        [Argument(Format = "--param {value}", Separator = " ")] public IReadOnlyList<string> WhitespaceSeparatorList => Get<List<string>>(() => SeparatorList);
        [Argument(Format = "--param={value}", FormatterMethod = nameof(Format))] public IReadOnlyList<bool> FormattedList => Get<List<bool>>(() => FormattedList);

        private string Format(bool value, PropertyInfo property) => value.ToString().ToUpperInvariant();
    }

    [Fact]
    public void TestDictionary()
    {
        var simpleDictionary = new { SimpleDictionary = new Dictionary<string, object> { ["key1"] = 1, ["key2"] = "foobar" } };
        Assert<DictionaryToolOptions>(simpleDictionary, ["--param", "key1=1", "--param", "key2=foobar"]);

        var separatorDictionary = new { SeparatorDictionary = new Dictionary<string, object> { ["key1"] = 1, ["key2"] = "foobar" } };
        Assert<DictionaryToolOptions>(separatorDictionary, ["/p:key1=1;key2=foobar"]);

        var whitespaceDictionary = new { SeparatorDictionary = new Dictionary<string, object> { ["key1"] = 1, ["key2"] = "foobar" } };
        Assert<DictionaryToolOptions>(whitespaceDictionary, ["--", "key1=1", "key2=foobar"]);
    }

    private class DictionaryToolOptions : ToolOptions
    {
        [Argument(Format = "-p {key}={value}")] public IReadOnlyDictionary<string, object> SimpleDictionary => Get<Dictionary<string, object>>(() => SimpleDictionary);
        [Argument(Format = "/p:{key}={value}", Separator = ";")] public IReadOnlyDictionary<string, object> SeparatorDictionary => Get<Dictionary<string, object>>(() => SeparatorDictionary);
        [Argument(Format = "-- {key}={value}", Separator = " ")] public IReadOnlyDictionary<string, object> WhitespaceDictionary => Get<Dictionary<string, object>>(() => WhitespaceDictionary);
    }

    [Fact]
    public void TestLookup()
    {
        // new SimpleToolOptions()
        //     .SetLookupValue(new LookupTable<string, object> { ["key"] = [1, 2] })
        //     .GetArguments().Should().Equal(["--param", "key=1,2"]);
    }

    private void Assert<T>(object obj, params string[] arguments)
        where T : ToolOptions, new()
    {
        var options = new T();
        options.InternalOptions = obj.ToJObject();
        options.GetArguments().Should().Equal(arguments);
    }
}

[NuGetTool(PackageId = "xunit.runner.console", Executable = "xunit.console.exe")]
file class SimpleTool;

[Command(Type = typeof(SimpleTool))]
file class SimpleToolOptions : ToolOptions
{
    [Argument(Format = "--param {key}={value}", Separator = ",")] public ILookup<string, object> LookupValue => Get<LookupTable<string, object>>(() => LookupValue);
}
