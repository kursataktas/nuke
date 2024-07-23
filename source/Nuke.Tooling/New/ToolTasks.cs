// Copyright 2024 Maintainers of NUKE.
// Distributed under the MIT License.
// https://github.com/nuke-build/nuke/blob/master/LICENSE

using System;
using System.Linq;
using Nuke.Common.Tooling;

namespace Nuke.Tooling;

public abstract partial class ToolTasks
{
    protected internal virtual partial string GetToolPath(ToolOptions options = null);
    protected internal virtual partial Action<OutputType, string> GetLogger();
    protected internal virtual partial Action<IProcess> GetExitHandler(ToolOptions options = null);
}
