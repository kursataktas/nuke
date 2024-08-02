// Generated from https://github.com/nuke-build/nuke/blob/master/source/Nuke.Common/Tools/MinVer/MinVer.json
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
namespace Nuke.Common.Tools.MinVer;
/// <summary><p>Minimalistic versioning using Git tags.</p><p>For more details, visit the <a href="https://github.com/adamralph/minver">official website</a>.</p></summary>
[PublicAPI]
[ExcludeFromCodeCoverage]
[NuGetPackageRequirement(PackageId)]
[NuGetTool(Id = PackageId)]
public partial class MinVerTasks : ToolTasks
{
    public static string MinVerPath => new MinVerTasks().GetToolPath();
    public const string PackageId = "minver-cli";
    /// <summary><p>Minimalistic versioning using Git tags.</p><p>For more details, visit the <a href="https://github.com/adamralph/minver">official website</a>.</p></summary>
    public static IReadOnlyCollection<Output> MinVer(ArgumentStringHandler arguments, string workingDirectory = null, IReadOnlyDictionary<string, string> environmentVariables = null, int? timeout = null, bool? logOutput = null, bool? logInvocation = null, Action<OutputType, string> logger = null, Func<IProcess, object> exitHandler = null) => Run<MinVerTasks>(arguments, workingDirectory, environmentVariables, timeout, logOutput, logInvocation, logger, exitHandler);
    /// <summary><p>Minimalistic versioning using Git tags.</p><p>For more details, visit the <a href="https://github.com/adamralph/minver">official website</a>.</p></summary>
    /// <remarks><p>This is a <a href="http://www.nuke.build/docs/authoring-builds/cli-tools.html#fluent-apis">CLI wrapper with fluent API</a> that allows to modify the following arguments:</p><ul>  <li><c>--auto-increment</c> via <see cref="MinVerSettings.AutoIncrement"/></li>  <li><c>--build-metadata</c> via <see cref="MinVerSettings.BuildMetadata"/></li>  <li><c>--default-pre-release-phase</c> via <see cref="MinVerSettings.DefaultPreReleasePhase"/></li>  <li><c>--minimum-major-minor</c> via <see cref="MinVerSettings.MinimumMajorMinor"/></li>  <li><c>--tag-prefix</c> via <see cref="MinVerSettings.TagPrefix"/></li>  <li><c>--verbosity</c> via <see cref="MinVerSettings.Verbosity"/></li></ul></remarks>
    public static (MinVer Result, IReadOnlyCollection<Output> Output) MinVer(MinVerSettings options = null) => Run<MinVerTasks, MinVer>(options);
    /// <summary><p>Minimalistic versioning using Git tags.</p><p>For more details, visit the <a href="https://github.com/adamralph/minver">official website</a>.</p></summary>
    /// <remarks><p>This is a <a href="http://www.nuke.build/docs/authoring-builds/cli-tools.html#fluent-apis">CLI wrapper with fluent API</a> that allows to modify the following arguments:</p><ul>  <li><c>--auto-increment</c> via <see cref="MinVerSettings.AutoIncrement"/></li>  <li><c>--build-metadata</c> via <see cref="MinVerSettings.BuildMetadata"/></li>  <li><c>--default-pre-release-phase</c> via <see cref="MinVerSettings.DefaultPreReleasePhase"/></li>  <li><c>--minimum-major-minor</c> via <see cref="MinVerSettings.MinimumMajorMinor"/></li>  <li><c>--tag-prefix</c> via <see cref="MinVerSettings.TagPrefix"/></li>  <li><c>--verbosity</c> via <see cref="MinVerSettings.Verbosity"/></li></ul></remarks>
    public static (MinVer Result, IReadOnlyCollection<Output> Output) MinVer(Configure<MinVerSettings> configurator) => Run<MinVerTasks, MinVer>(configurator.Invoke(new MinVerSettings()));
    /// <summary><p>Minimalistic versioning using Git tags.</p><p>For more details, visit the <a href="https://github.com/adamralph/minver">official website</a>.</p></summary>
    /// <remarks><p>This is a <a href="http://www.nuke.build/docs/authoring-builds/cli-tools.html#fluent-apis">CLI wrapper with fluent API</a> that allows to modify the following arguments:</p><ul>  <li><c>--auto-increment</c> via <see cref="MinVerSettings.AutoIncrement"/></li>  <li><c>--build-metadata</c> via <see cref="MinVerSettings.BuildMetadata"/></li>  <li><c>--default-pre-release-phase</c> via <see cref="MinVerSettings.DefaultPreReleasePhase"/></li>  <li><c>--minimum-major-minor</c> via <see cref="MinVerSettings.MinimumMajorMinor"/></li>  <li><c>--tag-prefix</c> via <see cref="MinVerSettings.TagPrefix"/></li>  <li><c>--verbosity</c> via <see cref="MinVerSettings.Verbosity"/></li></ul></remarks>
    public static IEnumerable<(MinVerSettings Settings, MinVer Result, IReadOnlyCollection<Output> Output)> MinVer(CombinatorialConfigure<MinVerSettings> configurator, int degreeOfParallelism = 1, bool completeOnFailure = false) => configurator.Invoke2(MinVer, degreeOfParallelism, completeOnFailure);
}
#region MinVerSettings
/// <summary>Used within <see cref="MinVerTasks"/>.</summary>
[PublicAPI]
[ExcludeFromCodeCoverage]
[Serializable]
[Command(Type = typeof(MinVerTasks), Command = nameof(MinVerTasks.MinVer))]
public partial class MinVerSettings : ToolOptions
{
    /// <summary></summary>
    [Argument(Format = "--auto-increment {value}")] public MinVerVersionPart AutoIncrement => Get<MinVerVersionPart>(() => AutoIncrement);
    /// <summary></summary>
    [Argument(Format = "--build-metadata {value}")] public string BuildMetadata => Get<string>(() => BuildMetadata);
    /// <summary></summary>
    [Argument(Format = "--default-pre-release-phase {value}")] public string DefaultPreReleasePhase => Get<string>(() => DefaultPreReleasePhase);
    /// <summary></summary>
    [Argument(Format = "--minimum-major-minor {value}")] public string MinimumMajorMinor => Get<string>(() => MinimumMajorMinor);
    /// <summary></summary>
    [Argument(Format = "--tag-prefix {value}")] public string TagPrefix => Get<string>(() => TagPrefix);
    /// <summary></summary>
    [Argument(Format = "--verbosity {value}")] public MinVerVerbosity Verbosity => Get<MinVerVerbosity>(() => Verbosity);
    /// <summary></summary>
    [Argument()] public string Framework => Get<string>(() => Framework);
}
#endregion
#region MinVer
/// <summary>Used within <see cref="MinVerTasks"/>.</summary>
[PublicAPI]
[ExcludeFromCodeCoverage]
[Serializable]
public partial class MinVer : Options
{
    /// <summary></summary>
    [Argument()] public string MinVerVersion => Get<string>(() => MinVerVersion);
    /// <summary></summary>
    [Argument()] public string MinVerMajor => Get<string>(() => MinVerMajor);
    /// <summary></summary>
    [Argument()] public string MinVerMinor => Get<string>(() => MinVerMinor);
    /// <summary></summary>
    [Argument()] public string MinVerPatch => Get<string>(() => MinVerPatch);
    /// <summary></summary>
    [Argument()] public string MinVerPreRelease => Get<string>(() => MinVerPreRelease);
    /// <summary></summary>
    [Argument()] public string MinVerBuildMetadata => Get<string>(() => MinVerBuildMetadata);
    /// <summary></summary>
    [Argument()] public string AssemblyVersion => Get<string>(() => AssemblyVersion);
    /// <summary></summary>
    [Argument()] public string FileVersion => Get<string>(() => FileVersion);
    /// <summary></summary>
    [Argument()] public string PackageVersion => Get<string>(() => PackageVersion);
    /// <summary></summary>
    [Argument()] public string Version => Get<string>(() => Version);
}
#endregion
#region MinVerSettingsExtensions
/// <summary>Used within <see cref="MinVerTasks"/>.</summary>
[PublicAPI]
[ExcludeFromCodeCoverage]
public static partial class MinVerSettingsExtensions
{
    #region AutoIncrement
    /// <inheritdoc cref="MinVerSettings.AutoIncrement"/>
    [Pure] [Builder(Type = typeof(MinVerSettings), Property = nameof(MinVerSettings.AutoIncrement))]
    public static T SetAutoIncrement<T>(this T o, MinVerVersionPart v) where T : MinVerSettings => o.Modify(b => b.Set(() => o.AutoIncrement, v));
    /// <inheritdoc cref="MinVerSettings.AutoIncrement"/>
    [Pure] [Builder(Type = typeof(MinVerSettings), Property = nameof(MinVerSettings.AutoIncrement))]
    public static T ResetAutoIncrement<T>(this T o) where T : MinVerSettings => o.Modify(b => b.Remove(() => o.AutoIncrement));
    #endregion
    #region BuildMetadata
    /// <inheritdoc cref="MinVerSettings.BuildMetadata"/>
    [Pure] [Builder(Type = typeof(MinVerSettings), Property = nameof(MinVerSettings.BuildMetadata))]
    public static T SetBuildMetadata<T>(this T o, string v) where T : MinVerSettings => o.Modify(b => b.Set(() => o.BuildMetadata, v));
    /// <inheritdoc cref="MinVerSettings.BuildMetadata"/>
    [Pure] [Builder(Type = typeof(MinVerSettings), Property = nameof(MinVerSettings.BuildMetadata))]
    public static T ResetBuildMetadata<T>(this T o) where T : MinVerSettings => o.Modify(b => b.Remove(() => o.BuildMetadata));
    #endregion
    #region DefaultPreReleasePhase
    /// <inheritdoc cref="MinVerSettings.DefaultPreReleasePhase"/>
    [Pure] [Builder(Type = typeof(MinVerSettings), Property = nameof(MinVerSettings.DefaultPreReleasePhase))]
    public static T SetDefaultPreReleasePhase<T>(this T o, string v) where T : MinVerSettings => o.Modify(b => b.Set(() => o.DefaultPreReleasePhase, v));
    /// <inheritdoc cref="MinVerSettings.DefaultPreReleasePhase"/>
    [Pure] [Builder(Type = typeof(MinVerSettings), Property = nameof(MinVerSettings.DefaultPreReleasePhase))]
    public static T ResetDefaultPreReleasePhase<T>(this T o) where T : MinVerSettings => o.Modify(b => b.Remove(() => o.DefaultPreReleasePhase));
    #endregion
    #region MinimumMajorMinor
    /// <inheritdoc cref="MinVerSettings.MinimumMajorMinor"/>
    [Pure] [Builder(Type = typeof(MinVerSettings), Property = nameof(MinVerSettings.MinimumMajorMinor))]
    public static T SetMinimumMajorMinor<T>(this T o, string v) where T : MinVerSettings => o.Modify(b => b.Set(() => o.MinimumMajorMinor, v));
    /// <inheritdoc cref="MinVerSettings.MinimumMajorMinor"/>
    [Pure] [Builder(Type = typeof(MinVerSettings), Property = nameof(MinVerSettings.MinimumMajorMinor))]
    public static T ResetMinimumMajorMinor<T>(this T o) where T : MinVerSettings => o.Modify(b => b.Remove(() => o.MinimumMajorMinor));
    #endregion
    #region TagPrefix
    /// <inheritdoc cref="MinVerSettings.TagPrefix"/>
    [Pure] [Builder(Type = typeof(MinVerSettings), Property = nameof(MinVerSettings.TagPrefix))]
    public static T SetTagPrefix<T>(this T o, string v) where T : MinVerSettings => o.Modify(b => b.Set(() => o.TagPrefix, v));
    /// <inheritdoc cref="MinVerSettings.TagPrefix"/>
    [Pure] [Builder(Type = typeof(MinVerSettings), Property = nameof(MinVerSettings.TagPrefix))]
    public static T ResetTagPrefix<T>(this T o) where T : MinVerSettings => o.Modify(b => b.Remove(() => o.TagPrefix));
    #endregion
    #region Verbosity
    /// <inheritdoc cref="MinVerSettings.Verbosity"/>
    [Pure] [Builder(Type = typeof(MinVerSettings), Property = nameof(MinVerSettings.Verbosity))]
    public static T SetVerbosity<T>(this T o, MinVerVerbosity v) where T : MinVerSettings => o.Modify(b => b.Set(() => o.Verbosity, v));
    /// <inheritdoc cref="MinVerSettings.Verbosity"/>
    [Pure] [Builder(Type = typeof(MinVerSettings), Property = nameof(MinVerSettings.Verbosity))]
    public static T ResetVerbosity<T>(this T o) where T : MinVerSettings => o.Modify(b => b.Remove(() => o.Verbosity));
    #endregion
    #region Framework
    /// <inheritdoc cref="MinVerSettings.Framework"/>
    [Pure] [Builder(Type = typeof(MinVerSettings), Property = nameof(MinVerSettings.Framework))]
    public static T SetFramework<T>(this T o, string v) where T : MinVerSettings => o.Modify(b => b.Set(() => o.Framework, v));
    /// <inheritdoc cref="MinVerSettings.Framework"/>
    [Pure] [Builder(Type = typeof(MinVerSettings), Property = nameof(MinVerSettings.Framework))]
    public static T ResetFramework<T>(this T o) where T : MinVerSettings => o.Modify(b => b.Remove(() => o.Framework));
    #endregion
}
#endregion
#region MinVerVerbosity
/// <summary>Used within <see cref="MinVerTasks"/>.</summary>
[PublicAPI]
[Serializable]
[ExcludeFromCodeCoverage]
[TypeConverter(typeof(TypeConverter<MinVerVerbosity>))]
public partial class MinVerVerbosity : Enumeration
{
    public static MinVerVerbosity Error = (MinVerVerbosity) "Error";
    public static MinVerVerbosity Warn = (MinVerVerbosity) "Warn";
    public static MinVerVerbosity Info = (MinVerVerbosity) "Info";
    public static MinVerVerbosity Debug = (MinVerVerbosity) "Debug";
    public static MinVerVerbosity Trace = (MinVerVerbosity) "Trace";
    public static implicit operator MinVerVerbosity(string value)
    {
        return new MinVerVerbosity { Value = value };
    }
}
#endregion
#region MinVerVersionPart
/// <summary>Used within <see cref="MinVerTasks"/>.</summary>
[PublicAPI]
[Serializable]
[ExcludeFromCodeCoverage]
[TypeConverter(typeof(TypeConverter<MinVerVersionPart>))]
public partial class MinVerVersionPart : Enumeration
{
    public static MinVerVersionPart Major = (MinVerVersionPart) "Major";
    public static MinVerVersionPart Minor = (MinVerVersionPart) "Minor";
    public static MinVerVersionPart Patch = (MinVerVersionPart) "Patch";
    public static implicit operator MinVerVersionPart(string value)
    {
        return new MinVerVersionPart { Value = value };
    }
}
#endregion
