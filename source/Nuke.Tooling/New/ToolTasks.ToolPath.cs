// Copyright 2024 Maintainers of NUKE.
// Distributed under the MIT License.
// https://github.com/nuke-build/nuke/blob/master/LICENSE

using System;
using System.Reflection;
using Nuke.Common;
using Nuke.Common.Tooling;
using Nuke.Common.Utilities;

namespace Nuke.Tooling;

/// <summary>Marks a class as CLI tool wrapper.</summary>
[AttributeUsage(AttributeTargets.Class)]
public abstract class ToolAttribute : Attribute
{
    internal abstract string GetToolPath(ToolOptions options);
}

public abstract partial class ToolTasks
{
    internal string GetToolPathInternal(ToolOptions options = null)
    {
        if (options?.ProcessToolPath != null)
            return options.ProcessToolPath;

        var toolType = GetType();
        var environmentVariable = toolType.Name.TrimEnd("Tasks").ToUpperInvariant() + "_EXE";
        if (ToolPathResolver.TryGetEnvironmentExecutable(environmentVariable) is { } environmentExecutable)
            return environmentExecutable;

        return GetToolPath();
    }

    protected virtual partial string GetToolPath(ToolOptions options)
    {
        var toolType = GetType();
        return toolType.GetCustomAttribute<ToolAttribute>().NotNull().GetToolPath(options);
    }
}

public class PathToolAttribute : ToolAttribute
{
    public string Executable { get; set; }

    internal override string GetToolPath(ToolOptions options)
    {
        return ToolPathResolver.GetPathExecutable(Executable);
    }
}

public class NuGetToolAttribute : ToolAttribute
{
    public string Id { get; set; }
    public string Executable { get; set; }
    public string FrameworkProperty { get; set; }

    internal override string GetToolPath(ToolOptions options)
    {
        var framework = FrameworkProperty != null
            ? options.GetType().GetProperty(FrameworkProperty)?.GetValue<string>(options)
            : null;
        return NuGetToolPathResolver.GetPackageExecutable(Id, Executable, framework);
    }
}
