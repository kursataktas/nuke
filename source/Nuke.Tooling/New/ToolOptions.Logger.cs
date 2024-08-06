// Copyright 2024 Maintainers of NUKE.
// Distributed under the MIT License.
// https://github.com/nuke-build/nuke/blob/master/LICENSE

using System;
using System.Reflection;
using Nuke.Common;
using Nuke.Common.Tooling;
using Nuke.Common.Utilities;

namespace Nuke.Tooling;

partial class ToolOptions
{
    internal Action<OutputType, string> ProcessLogger { get; set; }

    internal partial Action<OutputType, string> GetLogger()
    {
        var commandAttribute = GetType().GetCustomAttribute<CommandAttribute>().NotNull();
        var toolInstance = commandAttribute.Type.CreateInstance<ToolTasks>();
        return toolInstance.GetLogger(this);
    }
}
