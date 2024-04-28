// Copyright 2024 Maintainers of NUKE.
// Distributed under the MIT License.
// https://github.com/nuke-build/nuke/blob/master/LICENSE

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Nuke.Common.Tooling;
using Nuke.Tooling;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using VerifyXunit;
using Xunit;

namespace Nuke.Common.Tests;

public class ToolOptionsLoggerTest
{
    private readonly List<LogEvent> _logEvents = new();

    public ToolOptionsLoggerTest()
    {
        var memorySink = new InMemorySink();
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.Sink(memorySink, LogEventLevel.Verbose)
            .CreateLogger();

        _logEvents = memorySink.LogEvents;
    }

    private class SimpleTool;

    [Command(Type = typeof(SimpleTool))]
    private class SimpleToolOptions : ToolOptions;

    [Fact]
    public void TestSimpleLogging()
    {
        var logger = new SimpleToolOptions().GetLogger();

        logger.Invoke(OutputType.Std, "debug");
        logger.Invoke(OutputType.Err, "warning: some text");

        _logEvents.Should().BeEquivalentTo(
        [
            new { Level = LogEventLevel.Debug, MessageTemplate = new { Text = "debug" } },
            new { Level = LogEventLevel.Error, MessageTemplate = new { Text = "warning: some text" } },
        ]);
    }

    [LogLevelPattern(LogEventLevel.Warning, "^warning:")]
    [LogLevelPattern(LogEventLevel.Error, "^\\S* error:")]
    private class RegexPatternsTool;

    [Command(Type = typeof(RegexPatternsTool))]
    private class RegexPatternsToolOptions : ToolOptions;

    [Fact]
    public void TestRegexLogging()
    {
        var logger = new RegexPatternsToolOptions().GetLogger();

        logger.Invoke(OutputType.Std, "debug");
        logger.Invoke(OutputType.Err, "warning: some text");
        logger.Invoke(OutputType.Err, "SomeFile.cs error: more info");
        logger.Invoke(OutputType.Std, "AnotherFile.cs error: more info");

        _logEvents.Should().BeEquivalentTo(
        [
            new { Level = LogEventLevel.Debug, MessageTemplate = new { Text = "debug" } },
            new { Level = LogEventLevel.Warning, MessageTemplate = new { Text = "warning: some text" } },
            new { Level = LogEventLevel.Error, MessageTemplate = new { Text = "SomeFile.cs error: more info" } },
            new { Level = LogEventLevel.Error, MessageTemplate = new { Text = "AnotherFile.cs error: more info" } },
        ]);
    }

    [LogErrorAsStandard]
    [LogLevelPattern(LogEventLevel.Warning, "^warning:")]
    private class ErrorAsStandardTool;

    [Command(Type = typeof(ErrorAsStandardTool))]
    private class ErrorAsStandardToolOptions : ToolOptions;

    [Fact]
    public void TestErrorAsStandardLogging()
    {
        var logger = new ErrorAsStandardToolOptions().GetLogger();

        logger.Invoke(OutputType.Err, "debug");
        logger.Invoke(OutputType.Err, "warning: some text");

        _logEvents.Should().BeEquivalentTo(
        [
            new { Level = LogEventLevel.Debug, MessageTemplate = new { Text = "debug" } },
            new { Level = LogEventLevel.Warning, MessageTemplate = new { Text = "warning: some text" } },
        ]);
    }

    [LogErrorAsStandard]
    [DefaultLogLevel(LogEventLevel.Debug)]
    private class DefaultLevelTool;

    [Command(Type = typeof(DefaultLevelTool))]
    private class DefaultLevelToolOptions : ToolOptions;

    [Fact]
    public void TestDefaultLevelLogging()
    {
        var logger = new DefaultLevelToolOptions().GetLogger();

        logger.Invoke(OutputType.Err, "debug");

        _logEvents.Should().BeEquivalentTo(
        [
            new { Level = LogEventLevel.Debug, MessageTemplate = new { Text = "debug" } },
        ]);
    }
}

file class InMemorySink : ILogEventSink, IDisposable
{
    public readonly List<LogEvent> LogEvents = new();
    public void Emit(LogEvent logEvent) => LogEvents.Add(logEvent);
    public void Dispose() => LogEvents.Clear();
}
