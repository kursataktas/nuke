using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Nuke.Common;

namespace Nuke.Tooling;

// TODO: rename to TaskOptions
[PublicAPI]
public abstract partial class ToolOptions : Options
{
    internal static event EventHandler Created;

    protected ToolOptions()
    {
        this.SetProcessEnvironmentVariables(EnvironmentInfo.Variables.ToDictionary(x => x.Key, object (x) => x.Value));
        Created?.Invoke(this, EventArgs.Empty);
    }

    internal partial IEnumerable<string> GetArguments();
}
