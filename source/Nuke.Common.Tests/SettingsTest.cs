// Copyright 2023 Maintainers of NUKE.
// Distributed under the MIT License.
// https://github.com/nuke-build/nuke/blob/master/LICENSE

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Nuke.Common.Tools.MSBuild;
using Nuke.Common.Tools.OpenCover;
using Nuke.Common.Tools.Xunit;
using Nuke.Common.IO;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.CorFlags;
using Nuke.Common.Tools.Discord;
using Nuke.Common.Tools.Docker;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities;
using Nuke.Tooling;
using VerifyXunit;
using Xunit;

namespace Nuke.Common.Tests;

public class SettingsTest
{
    private static AbsolutePath RootDirectory => Constants.TryGetRootDirectoryFrom(Directory.GetCurrentDirectory()).NotNull();

    private static void Assert<T>(Configure<T> configurator, string arguments)
        where T : ToolSettings, new()
    {
        configurator.Invoke(new T()).GetProcessArguments().RenderForOutput().Should().Be(arguments);
    }

    private static void Assert<T>(T options, string arguments)
        where T : ToolOptions, new()
    {
        options.GetArguments().JoinSpace().Should().Be(arguments);
    }

    [Fact]
    public void TestCommon()
    {
        var logEntry = default((OutputType Type, string String));
        var settings = new DotNetRunSettings()
            .SetProcessToolPath("/path/to/dotnet")
            .SetProcessEnvironmentVariable("key", "value")
            .SetProcessExecutionTimeout(TimeSpan.FromMilliseconds(1_000));

        settings.ProcessToolPath.Should().Be("/path/to/dotnet");
        settings.ProcessEnvironmentVariables.Should().ContainSingle(x => x.Key == "key" && x.Value == "value");
        settings.ProcessExecutionTimeout.Should().Be(1_000);
    }

    [Fact]
    public void TestMSBuild()
    {
        var projectFile = RootDirectory / "source" / "Nuke.Common" / "Nuke.Common.csproj";
        var solutionFile = RootDirectory / "nuke-common.sln";

        Assert(new MSBuildSettings()
                .SetProjectFile(projectFile)
                .SetTargetPlatform(MSBuildTargetPlatform.MSIL)
                .SetConfiguration("Release")
                .DisableNodeReuse()
                .EnableNoLogo(),
            $"{projectFile.ToString().DoubleQuoteIfNeeded()} /p:Platform=AnyCPU /p:Configuration=Release /nodeReuse:false /nologo");

        Assert(new MSBuildSettings()
                .SetProjectFile(solutionFile)
                .SetTargetPlatform(MSBuildTargetPlatform.MSIL)
                .EnableNodeReuse()
                .DisableNoLogo()
                .ToggleRunCodeAnalysis(),
            $"{solutionFile.ToString().DoubleQuoteIfNeeded()} /p:Platform=\"Any CPU\" /nodeReuse:true /p:RunCodeAnalysis=true");
    }

    [Fact]
    public void TestXunit2()
    {
        Assert<Xunit2Settings>(x => x
                .AddTargetAssemblies("A.csproj")
                .AddTargetAssemblyWithConfigs("B.csproj", "D.config", "new folder\\E.config"),
            "A.csproj  B.csproj D.config B.csproj \"new folder\\E.config\"");

        Assert<Xunit2Settings>(x => x
                .AddResultReport(Xunit2ResultFormat.HTML, "new folder\\data.html")
                .AddResultReport(Xunit2ResultFormat.Xml, "new_folder\\data.xml"),
            "-HTML \"new folder\\data.html\" -Xml new_folder\\data.xml");

        Assert<Xunit2Settings>(x => x
                .AddResultReport(Xunit2ResultFormat.NUnit, "new folder\\nunit.xml")
                .EnableDiagnostics()
                .EnableFailSkips(),
            "-failskips -diagnostics -NUnit \"new folder\\nunit.xml\"");
    }

    [Fact]
    public void TestOpenCover()
    {
        var projectFile = RootDirectory / "source" / "Nuke.Common" / "Nuke.Common.csproj";

        Assert<OpenCoverSettings>(x => x
                .SetTargetPath(projectFile)
                .AddFilters("+[*]*", "-[xunit.*]*", "-[NUnit.*]*")
                .SetTargetArguments("-diagnostics -HTML \"new folder\\data.xml\""),
            $"-target:{projectFile.ToString().DoubleQuoteIfNeeded()} -targetargs:\"-diagnostics -HTML \\\"new folder\\data.xml\\\"\" -filter:\"+[*]* -[xunit.*]* -[NUnit.*]*\"");
    }

    [Fact]
    public void TestCorFlags()
    {
        Assert(new CorFlagsSettings()
                .SetAssembly("assembly")
                .EnablePrefer32Bit()
                .DisableILOnly(),
            "assembly -32BITPREF+ -ILONLY-");
    }

    [Fact]
    public void TestDotNet()
    {
        Assert(new DotNetTestSettings()
                .AddRunSetting("key", "value")
                .AddRunSetting("foo", "bar")
                .EnableNoLogo(),
            "test --nologo -- key=value foo=bar");

        Assert(new DotNetRunSettings()
                .AddApplicationArguments("arg1")
                .AddApplicationArguments("arg2")
                .SetProperty("foo", "bar"),
            "run /property:foo=bar -- arg1 arg2");
    }

    [Fact]
    public void TestDocker()
    {
        Assert(new DockerAttachSettings()
                .SetDetachKeys("detach-keys")
                .SetContainer("container")
                .SetLogLevel(DockerLogLevel.debug),
            "attach --detach-keys detach-keys container --log-level debug");
    }

    [Fact]
    public Task TestDiscord()
    {
        var result = new DiscordMessage()
            .SetNonce("nonce")
            .SetChannelId("channel-id")
            .AddEmbeds(_ => _
                .SetType(DiscordEmbedType.article)
                .SetAuthor(_ => _
                    .SetName("author-name")))
            .ToJson(Options.JsonSerializerSettings);

        return Verifier.Verify(result);
    }
}
