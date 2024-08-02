using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;
using Nuke.Common;
using Nuke.Common.Tooling;
using Nuke.Common.Utilities;
using Nuke.Common.Utilities.Collections;

namespace Nuke.Tooling;

// TODO: rename to TaskOptions / CommandOptions ?
[PublicAPI]
public abstract partial class ToolOptions : Options, ISettingsEntity
{
    internal static event EventHandler Created;

    protected ToolOptions()
    {
        Set(() => ProcessEnvironmentVariables, EnvironmentInfo.Variables.ToDictionary(x => x.Key, object (x) => x.Value));
        Created?.Invoke(this, EventArgs.Empty);
    }

    internal partial IEnumerable<string> GetArguments();
    internal partial IEnumerable<string> GetSecrets();
    internal partial Action<OutputType, string> GetLogger();
}

partial class ToolOptions
{
    internal partial Action<OutputType, string> GetLogger()
    {
        var commandAttribute = GetType().GetCustomAttribute<CommandAttribute>();
        var toolInstance = commandAttribute.Type.CreateInstance<ToolTasks>();
        return toolInstance.GetLogger();
    }
}
