// Copyright 2024 Maintainers of NUKE.
// Distributed under the MIT License.
// https://github.com/nuke-build/nuke/blob/master/LICENSE

using System;
using System.Linq;
using JetBrains.Annotations;
using Nuke.Common.Tooling;

namespace Nuke.Tooling;

[PublicAPI]
public abstract partial class ToolTasks
{
    public static string GetToolPath<T>() where T : ToolTasks, new() => new T().GetToolPathInternal();

    protected internal virtual partial Action<OutputType, string> GetLogger();

    protected virtual partial string GetToolPath(ToolOptions options = null);
    protected virtual partial Action<IProcess> GetExitHandler(ToolOptions options = null);

    protected virtual ToolOptions PreProcess(ToolOptions options) => options;
    protected virtual void PostProcess(ToolOptions options) { }
}
