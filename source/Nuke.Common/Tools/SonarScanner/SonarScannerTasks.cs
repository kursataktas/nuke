// Copyright 2023 Maintainers of NUKE.
// Distributed under the MIT License.
// https://github.com/nuke-build/nuke/blob/master/LICENSE

using System;
using Nuke.Common.Tooling;
using Nuke.Tooling;

namespace Nuke.Common.Tools.SonarScanner;

partial class SonarScannerTasks
{
    protected override string GetToolPath(ToolOptions options = null)
    {
        return NuGetToolPathResolver.GetPackageExecutable(
            packageId: PackageId,
            packageExecutable: "SonarScanner.MSBuild.dll|SonarScanner.MSBuild.exe",
            framework: options switch
            {
                SonarScannerBeginSettings settings => settings.Framework,
                SonarScannerEndSettings settings => settings.Framework,
                _ => throw new ArgumentOutOfRangeException(nameof(options), options, null)
            });
    }
}
