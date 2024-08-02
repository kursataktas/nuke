// Copyright 2023 Maintainers of NUKE.
// Distributed under the MIT License.
// https://github.com/nuke-build/nuke/blob/master/LICENSE

using System;
using Nuke.Common.Tooling;
using Nuke.Tooling;

namespace Nuke.Common.Tools.Octopus;

partial class OctopusTasks
{
    protected override string GetToolPath(ToolOptions options = null)
    {
        return NuGetToolPathResolver.GetPackageExecutable(
            packageId: PackageId,
            packageExecutable: "OctoVersion.Tool.dll",
            framework: options switch
            {
                OctopusBuildInformationSettings settings => settings.Framework,
                OctopusPackSettings settings => settings.Framework,
                OctopusPushSettings settings => settings.Framework,
                OctopusCreateReleaseSettings settings => settings.Framework,
                OctopusDeployReleaseSettings settings => settings.Framework,
                _ => throw new ArgumentOutOfRangeException(nameof(options), options, null)
            });
    }
}
