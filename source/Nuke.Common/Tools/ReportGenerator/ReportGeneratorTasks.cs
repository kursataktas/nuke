// Copyright 2023 Maintainers of NUKE.
// Distributed under the MIT License.
// https://github.com/nuke-build/nuke/blob/master/LICENSE

using JetBrains.Annotations;
using Nuke.Common.Tooling;
using Nuke.Tooling;

namespace Nuke.Common.Tools.ReportGenerator;

[PublicAPI]
public class ReportGeneratorVerbosityMappingAttribute : VerbosityMappingAttribute
{
    public ReportGeneratorVerbosityMappingAttribute()
        : base(typeof(ReportGeneratorVerbosity))
    {
        Quiet = nameof(ReportGeneratorVerbosity.Off);
        Minimal = nameof(ReportGeneratorVerbosity.Warning);
        Normal = nameof(ReportGeneratorVerbosity.Info);
        Verbose = nameof(ReportGeneratorVerbosity.Verbose);
    }
}

partial class ReportGeneratorTasks
{
    protected override string GetToolPath(ToolOptions options = null)
    {
        var reportGeneratorOptions = (ReportGeneratorSettings)options;
        return NuGetToolPathResolver.GetPackageExecutable(
            packageId: PackageId,
            packageExecutable: "ReportGenerator.dll|ReportGenerator.exe",
            framework: reportGeneratorOptions?.Framework);
    }
}
