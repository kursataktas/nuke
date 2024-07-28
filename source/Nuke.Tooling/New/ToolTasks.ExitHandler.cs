// Copyright 2024 Maintainers of NUKE.
// Distributed under the MIT License.
// https://github.com/nuke-build/nuke/blob/master/LICENSE

using System;
using System.Linq;
using Nuke.Common.Tooling;

namespace Nuke.Tooling;

public abstract partial class ToolTasks
{
    internal Action<IProcess> GetExitHandlerInternal(ToolOptions options = null)
    {
        return options?.ProcessExitHandling ?? true
            ? GetExitHandler(options)
            : _ => { };
    }

    protected virtual partial Action<IProcess> GetExitHandler(ToolOptions options = null)
    {
        return x => x.AssertZeroExitCode();
    }
}
