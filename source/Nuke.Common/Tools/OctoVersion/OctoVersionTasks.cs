// Copyright 2023 Maintainers of NUKE.
// Distributed under the MIT License.
// https://github.com/nuke-build/nuke/blob/master/LICENSE

using System;
using Newtonsoft.Json;
using Nuke.Common.IO;
using Nuke.Common.Tooling;
using Nuke.Common.Utilities;
using Nuke.Tooling;

namespace Nuke.Common.Tools.OctoVersion;

public partial class OctoVersionGetVersionSettings
{
    private string GetProcessToolPath()
    {
        return OctoVersionTasks.GetToolPath(Framework);
    }
}

public partial class OctoVersionExecuteSettings
{
    private string GetProcessToolPath()
    {
        return OctoVersionTasks.GetToolPath(Framework);
    }
}

partial class OctoVersionTasks2 : ToolTasks
{
    public const string PackageId = "foo";

    protected override string GetToolPath(ToolOptions options = null)
    {
        var framework = (object)options switch
        {
            OctoVersionGetVersionSettings settings => settings.Framework,
            OctoVersionExecuteSettings settings => settings.Framework,
        };
        return NuGetToolPathResolver.GetPackageExecutable(
            packageId: PackageId,
            packageExecutable: "OctoVersion.Tool.dll",
            framework: framework);
    }
}

public partial class OctoVersionTasks
{
    internal static string GetToolPath(string framework = null)
    {
        return NuGetToolPathResolver.GetPackageExecutable(
            packageId: "Octopus.OctoVersion.Tool",
            packageExecutable: "OctoVersion.Tool.dll",
            framework: framework);
    }

    private static OctoVersionInfo GetResult(IProcess process, OctoVersionGetVersionSettings toolSettings)
    {
        Assert.FileExists(toolSettings.OutputJsonFile);
        try
        {
            var file = (AbsolutePath) toolSettings.OutputJsonFile;
            return file.ReadJson<OctoVersionInfo>(new JsonSerializerSettings { ContractResolver = new AllWritableContractResolver() });
        }
        catch (Exception exception)
        {
            throw new Exception($"Cannot parse {nameof(OctoVersion)} output from {toolSettings.OutputJsonFile.SingleQuote()}.", exception);
        }
    }
}
