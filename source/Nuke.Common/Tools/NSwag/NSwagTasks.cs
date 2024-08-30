// Copyright 2023 Maintainers of NUKE.
// Distributed under the MIT License.
// https://github.com/nuke-build/nuke/blob/master/LICENSE

using System;
using System.Linq;
using Nuke.Common.Tooling;
using Nuke.Tooling;

namespace Nuke.Common.Tools.NSwag;

partial class NSwagTasks
{
    protected override string GetToolPath(ToolOptions options = null)
    {
        return NuGetToolPathResolver.GetPackageExecutable(
            packageId: PackageId,
            packageExecutable: "dotnet-nswag.dll|NSwag.exe",
            framework: (options as NSwagOptionsBase)?.Framework);
    }
}
