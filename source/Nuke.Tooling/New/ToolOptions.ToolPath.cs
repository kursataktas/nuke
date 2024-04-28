// Copyright 2024 Maintainers of NUKE.
// Distributed under the MIT License.
// https://github.com/nuke-build/nuke/blob/master/LICENSE

using System.Reflection;
using Nuke.Common;
using Nuke.Common.Tooling;
using Nuke.Common.Utilities;

namespace Nuke.Tooling;

partial class ToolOptions
{
    internal partial string GetToolPath()
    {
        if (ProcessToolPath != null)
            return ProcessToolPath;

        var optionsType = GetType();
        var toolType = optionsType.GetCustomAttribute<CommandAttribute>().NotNull().Type;

        var environmentVariable = toolType.Name.TrimEnd("Tasks").ToUpperInvariant() + "_EXE";
        if (ToolPathResolver.TryGetEnvironmentExecutable(environmentVariable) is { } environmentExecutable)
            return environmentExecutable;

        // TODO: refactor to (abstract) interfaces
        if (this is IToolOptionsWithCustomToolPath optionsCustomProvider)
            return optionsCustomProvider.GetToolPath();

        if (toolType.CreateInstance() is IToolWithCustomToolPath toolCustomProvider)
            return toolCustomProvider.GetToolPath(this);

        return toolType.GetCustomAttribute<ToolAttribute>().NotNull().GetToolPath(this);
    }
}
