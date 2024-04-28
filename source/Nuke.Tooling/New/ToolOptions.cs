using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Nuke.Common.Tooling;

namespace Nuke.Tooling;

public interface IToolWithCustomToolPath
{
    abstract string GetToolPath(ToolOptions options);
}

public interface IToolOptionsWithCustomToolPath
{
    string GetToolPath();
}

[PublicAPI]
public partial class ToolOptions : Options
{
    internal partial string GetToolPath();
    internal partial IEnumerable<string> GetArguments();
    internal partial Action<OutputType, string> GetLogger();
}
