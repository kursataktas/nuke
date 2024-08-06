// Copyright 2023 Maintainers of NUKE.
// Distributed under the MIT License.
// https://github.com/nuke-build/nuke/blob/master/LICENSE

using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Nuke.Common.Tooling;
using Nuke.Tooling;

namespace Nuke.Common.Tools.MinVer;

partial class MinVerTasks
{
    protected override string GetToolPath(ToolOptions options = null)
    {
        return NuGetToolPathResolver.GetPackageExecutable(
            packageId: PackageId,
            packageExecutable: "minver-cli.dll",
            framework: (options as MinVerSettings)?.Framework);
    }

    protected override object GetResult<T>(ToolOptions options, IReadOnlyCollection<Output> output)
    {
        var versionString = output.Select(x => x.Text).Single(x => !x.StartsWith("MinVer:"));
        return new MinVer(versionString);
    }
}

[PublicAPI]
public record MinVer(string MinVerVersion)
{
    public string MinVerMajor => MinVerVersion.Split('.')[0];
    public string MinVerMinor => MinVerVersion.Split('.')[1];
    public string MinVerPatch => MinVerVersion.Split('.')[2].Split('-')[0].Split('+')[0];
    public string MinVerPreRelease => MinVerVersion.Split('+')[0].Contains('-') ? MinVerVersion.Split('+')[0].Split(['-',], count: 2)[1] : null;
    public string MinVerBuildMetadata => MinVerVersion.Contains('+') ? MinVerVersion.Split(['+',], count: 2)[1] : null;
    public string AssemblyVersion => $"{MinVerMajor}.0.0.0";
    public string FileVersion => $"{MinVerMajor}.{MinVerMinor}.{MinVerPatch}.0";
    public string PackageVersion => MinVerVersion;
    public string Version => MinVerVersion;
}
