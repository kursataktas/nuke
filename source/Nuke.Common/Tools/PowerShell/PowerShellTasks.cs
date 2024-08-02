// Copyright 2023 Maintainers of NUKE.
// Distributed under the MIT License.
// https://github.com/nuke-build/nuke/blob/master/LICENSE

using System;
using Nuke.Common.Tooling;
using Nuke.Tooling;

namespace Nuke.Common.Tools.PowerShell;

partial class PowerShellTasks
{
    protected override string GetToolPath(ToolOptions options = null)
    {
        return ToolPathResolver.GetPathExecutable(EnvironmentInfo.IsWin ? "powershell" : "pwsh");
    }
}
