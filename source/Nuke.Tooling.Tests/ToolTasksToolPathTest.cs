// Copyright 2024 Maintainers of NUKE.
// Distributed under the MIT License.
// https://github.com/nuke-build/nuke/blob/master/LICENSE

using System;
using System.Linq;
using FluentAssertions;
using Nuke.Common.Tooling;
using Nuke.Tooling;
using Xunit;

namespace Nuke.Common.Tests;

public class ToolTasksToolPathTest
{
    public ToolTasksToolPathTest()
    {
        var rootDirectory = Constants.TryGetRootDirectoryFrom(EnvironmentInfo.WorkingDirectory);
        NuGetToolPathResolver.NuGetPackagesConfigFile = rootDirectory / "build" / "_build.csproj";
    }

    [Fact]
    public void TestFromAttribute()
    {
        new SimpleTool()
            .GetToolPathInternal()
            .Should().EndWith("xunit.console.exe");
    }

    [Fact]
    public void TestFromOptions()
    {
        new SimpleTool()
            .GetToolPathInternal(new SimpleToolPathToolOptions()
                .SetProcessToolPath("/some/path"))
            .Should().EndWith("/some/path");
    }

    [Fact]
    public void TestFromOverride()
    {
        new CustomToolPathTool()
            .GetToolPathInternal()
            .Should().Be(nameof(CustomToolPathTool));
    }
}

[NuGetTool(Id = "xunit.runner.console", Executable = "xunit.console.exe")]
file class SimpleTool : ToolTasks;

[Command(Type = typeof(SimpleTool))]
file class SimpleToolPathToolOptions : ToolOptions;

[NuGetTool(Id = "xunit.runner.console", Executable = "xunit.console.exe")]
file class CustomToolPathTool : ToolTasks
{
    protected internal override string GetToolPath(ToolOptions options = null) => nameof(CustomToolPathTool);
}
