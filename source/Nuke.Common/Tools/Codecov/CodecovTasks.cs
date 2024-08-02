// Copyright 2023 Maintainers of NUKE.
// Distributed under the MIT License.
// https://github.com/nuke-build/nuke/blob/master/LICENSE

using System;
using Nuke.Common.Tooling;
using Nuke.Tooling;

namespace Nuke.Common.Tools.Codecov;

partial class CodecovSettings
{
    private string GetProcessToolPath()
    {
        return CodecovTasks.GetToolPath();
    }
}

partial class CodecovTasks2 : ToolTasks
{
    public const string PackageId = "foo";

    protected override string GetToolPath(ToolOptions options = null)
    {
        return NuGetToolPathResolver.GetPackageExecutable(
            packageId: PackageId,
            packageExecutable: EnvironmentInfo.Platform switch
            {
                PlatformFamily.Windows => "codecov.exe",
                PlatformFamily.OSX => "codecov-macos",
                PlatformFamily.Linux => "codecov-linux",
                _ => throw new ArgumentOutOfRangeException()
            });
    }
}

partial class CodecovTasks
{
    internal static string GetToolPath()
    {
        return NuGetToolPathResolver.GetPackageExecutable(
            packageId: "CodecovUploader",
            packageExecutable: GetPackageExecutable());
    }

    private static string GetPackageExecutable()
    {
        return EnvironmentInfo.Platform switch
        {
            PlatformFamily.Windows => "codecov.exe",
            PlatformFamily.OSX => "codecov-macos",
            PlatformFamily.Linux => "codecov-linux",
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
