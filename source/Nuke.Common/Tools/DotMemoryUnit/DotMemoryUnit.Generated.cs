// Generated from https://github.com/nuke-build/nuke/blob/master/source/Nuke.Common/Tools/DotMemoryUnit/DotMemoryUnit.json

using JetBrains.Annotations;
using Newtonsoft.Json;
using Nuke.Common;
using Nuke.Common.Tooling;
using Nuke.Common.Tools;
using Nuke.Common.Utilities.Collections;
using Nuke.Tooling;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;

namespace Nuke.Common.Tools.DotMemoryUnit;

/// <summary><p>dotMemory Unit is a unit testing framework which allows you to write tests that check your code for all kinds of memory issues. You can now extend NUnit, MSTest or another .NET unit testing framework with the functionality of a memory profiler.<para/>Perfect fit for any workflow: integrated with Visual Studio, works with stand-alone unit test runners, Continuous Integration ready. Last but not least, dotMemory Unit is free.</p><p>For more details, visit the <a href="https://www.jetbrains.com/dotmemory/unit/">official website</a>.</p></summary>
[PublicAPI]
[ExcludeFromCodeCoverage]
[NuGetPackageRequirement(PackageId)]
[NuGetTool(Id = PackageId, Executable = PackageExecutable)]
public partial class DotMemoryUnitTasks : ToolTasks, IRequireNuGetPackage
{
    public static string DotMemoryUnitPath => new DotMemoryUnitTasks().GetToolPath();
    public const string PackageId = "JetBrains.DotMemoryUnit";
    public const string PackageExecutable = "dotMemoryUnit.exe";
    /// <summary><p>dotMemory Unit is a unit testing framework which allows you to write tests that check your code for all kinds of memory issues. You can now extend NUnit, MSTest or another .NET unit testing framework with the functionality of a memory profiler.<para/>Perfect fit for any workflow: integrated with Visual Studio, works with stand-alone unit test runners, Continuous Integration ready. Last but not least, dotMemory Unit is free.</p><p>For more details, visit the <a href="https://www.jetbrains.com/dotmemory/unit/">official website</a>.</p></summary>
    public static IReadOnlyCollection<Output> DotMemoryUnit(ArgumentStringHandler arguments, string workingDirectory = null, IReadOnlyDictionary<string, string> environmentVariables = null, int? timeout = null, bool? logOutput = null, bool? logInvocation = null, Action<OutputType, string> logger = null, Func<IProcess, object> exitHandler = null) => Run<DotMemoryUnitTasks>(arguments, workingDirectory, environmentVariables, timeout, logOutput, logInvocation, logger, exitHandler);
}
