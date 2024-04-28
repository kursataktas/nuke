// Copyright 2024 Maintainers of NUKE.
// Distributed under the MIT License.
// https://github.com/nuke-build/nuke/blob/master/LICENSE

using System.Collections.Generic;
using JetBrains.Annotations;
using Serilog.Events;

namespace Nuke.Tooling;

partial class ToolOptions
{
    public string ProcessToolPath => Get<string>(() => ProcessToolPath);
    public string ProcessWorkingDirectory => Get<string>(() => ProcessWorkingDirectory);

    public IReadOnlyDictionary<string, object> ProcessEnvironmentVariables =>
        Get<Dictionary<string, object>>(() => ProcessEnvironmentVariables);

    public int? ProcessExecutionTimeout => Get<int?>(() => ProcessExecutionTimeout);
    public LogEventLevel? ProcessOutputLogging => Get<LogEventLevel?>(() => ProcessOutputLogging);
    public LogEventLevel? ProcessInvocationLogging => Get<LogEventLevel?>(() => ProcessInvocationLogging);
}

[PublicAPI]
public static partial class ToolOptionsExtensions
{
    #region ToolOptions.ProcessToolPath

    /// <summary><p>Defines the path of the tool to be invoked. In most cases, the tool path is automatically resolved from the PATH environment variable or a NuGet package.</p></summary>
    [Builder(Type = typeof(ToolOptions), Property = nameof(ToolOptions.ProcessToolPath))]
    public static T SetProcessToolPath<T>(this T o, string value) where T : ToolOptions => o.Modify(b => b.Set(() => o.ProcessToolPath, value));

    /// <summary><p>Defines the path of the tool to be invoked. In most cases, the tool path is automatically resolved from the PATH environment variable or a NuGet package.</p></summary>
    [Builder(Type = typeof(ToolOptions), Property = nameof(ToolOptions.ProcessToolPath))]
    public static T ResetProcessToolPath<T>(this T o) where T : ToolOptions => o.Modify(b => b.Remove(() => o.ProcessToolPath));

    #endregion

    #region ToolOptions.ProcessWorkingDirectory

    /// <summary><p>Defines the working directory for the process.</p></summary>
    [Builder(Type = typeof(ToolOptions), Property = nameof(ToolOptions.ProcessWorkingDirectory))]
    public static T SetProcessWorkingDirectory<T>(this T o, string value) where T : ToolOptions => o.Modify(b => b.Set(() => o.ProcessWorkingDirectory, value));

    /// <summary><p>Defines the working directory for the process.</p></summary>
    [Builder(Type = typeof(ToolOptions), Property = nameof(ToolOptions.ProcessWorkingDirectory))]
    public static T ResetProcessWorkingDirectory<T>(this T o) where T : ToolOptions => o.Modify(b => b.Remove(() => o.ProcessWorkingDirectory));

    #endregion

    #region ToolOptions.ProcessEnvironmentVariables

    /// <summary><p>Defines the environment variables to be passed to the process. By default, the environment variables of the current process are used.</p></summary>
    [Builder(Type = typeof(ToolOptions), Property = nameof(ToolOptions.ProcessEnvironmentVariables))]
    public static T SetProcessEnvironmentVariables<T>(this T o, IReadOnlyDictionary<string, object> values) where T : ToolOptions => o.Modify(b => b.Set(() => o.ProcessEnvironmentVariables, values));

    /// <summary><p>Defines the environment variables to be passed to the process. By default, the environment variables of the current process are used.</p></summary>
    [Builder(Type = typeof(ToolOptions), Property = nameof(ToolOptions.ProcessEnvironmentVariables))]
    public static T SetProcessEnvironmentVariables<T>(this T o, IDictionary<string, object> values) where T : ToolOptions => o.Modify(b => b.Set(() => o.ProcessEnvironmentVariables, values));

    /// <summary><p>Defines the environment variables to be passed to the process. By default, the environment variables of the current process are used.</p></summary>
    [Builder(Type = typeof(ToolOptions), Property = nameof(ToolOptions.ProcessEnvironmentVariables))]
    public static T AddProcessEnvironmentVariables<T>(this T o, IReadOnlyDictionary<string, object> values) where T : ToolOptions => o.Modify(b => b.AddDictionary(() => o.ProcessEnvironmentVariables, values));

    /// <summary><p>Defines the environment variables to be passed to the process. By default, the environment variables of the current process are used.</p></summary>
    [Builder(Type = typeof(ToolOptions), Property = nameof(ToolOptions.ProcessEnvironmentVariables))]
    public static T AddProcessEnvironmentVariables<T>(this T o, IDictionary<string, object> values) where T : ToolOptions => o.Modify(b => b.AddDictionary(() => o.ProcessEnvironmentVariables, values));

    /// <summary><p>Defines the environment variables to be passed to the process. By default, the environment variables of the current process are used.</p></summary>
    [Builder(Type = typeof(ToolOptions), Property = nameof(ToolOptions.ProcessEnvironmentVariables))]
    public static T AddProcessEnvironmentVariable<T>(this T o, string key, object value) where T : ToolOptions => o.Modify(b => b.AddDictionary(() => o.ProcessEnvironmentVariables, key, value));

    /// <summary><p>Defines the environment variables to be passed to the process. By default, the environment variables of the current process are used.</p></summary>
    [Builder(Type = typeof(ToolOptions), Property = nameof(ToolOptions.ProcessEnvironmentVariables))]
    public static T SetProcessEnvironmentVariable<T>(this T o, string key, object value) where T : ToolOptions => o.Modify(b => b.SetDictionary(() => o.ProcessEnvironmentVariables, key, value));

    /// <summary><p>Defines the environment variables to be passed to the process. By default, the environment variables of the current process are used.</p></summary>
    [Builder(Type = typeof(ToolOptions), Property = nameof(ToolOptions.ProcessEnvironmentVariables))]
    public static T RemoveProcessEnvironmentVariable<T>(this T o, string key) where T : ToolOptions => o.Modify(b => b.RemoveDictionary(() => o.ProcessEnvironmentVariables, key));

    /// <summary><p>Defines the environment variables to be passed to the process. By default, the environment variables of the current process are used.</p></summary>
    [Builder(Type = typeof(ToolOptions), Property = nameof(ToolOptions.ProcessEnvironmentVariables))]
    public static T ClearProcessEnvironmentVariables<T>(this T o) where T : ToolOptions => o.Modify(b => b.ClearDictionary(() => o.ProcessEnvironmentVariables));

    /// <summary><p>Defines the environment variables to be passed to the process. By default, the environment variables of the current process are used.</p></summary>
    [Builder(Type = typeof(ToolOptions), Property = nameof(ToolOptions.ProcessEnvironmentVariables))]
    public static T ResetProcessEnvironmentVariables<T>(this T o) where T : ToolOptions => o.Modify(b => b.Remove(() => o.ProcessEnvironmentVariables));

    #endregion

    #region ToolOptions.ProcessExecutionTimeout

    /// <summary><p>Defines the execution timeout of the invoked process.</p></summary>
    [Builder(Type = typeof(ToolOptions), Property = nameof(ToolOptions.ProcessExecutionTimeout))]
    public static T SetProcessExecutionTimeout<T>(this T o, int? value) where T : ToolOptions => o.Modify(b => b.Set(() => o.ProcessExecutionTimeout, value));

    /// <summary><p>Defines the execution timeout of the invoked process.</p></summary>
    [Builder(Type = typeof(ToolOptions), Property = nameof(ToolOptions.ProcessExecutionTimeout))]
    public static T ResetProcessExecutionTimeout<T>(this T o) where T : ToolOptions => o.Modify(b => b.Remove(() => o.ProcessExecutionTimeout));

    #endregion

    #region ToolOptions.ProcessOutputLogging

    /// <summary><p>Defines the log-level for standard output.</p></summary>
    [Builder(Type = typeof(ToolOptions), Property = nameof(ToolOptions.ProcessOutputLogging))]
    public static T SetProcessOutputLogging<T>(this T o, LogEventLevel? value) where T : ToolOptions => o.Modify(b => b.Set(() => o.ProcessOutputLogging, value));

    /// <summary><p>Defines the log-level for standard output.</p></summary>
    [Builder(Type = typeof(ToolOptions), Property = nameof(ToolOptions.ProcessOutputLogging))]
    public static T ResetProcessOutputLogging<T>(this T o) where T : ToolOptions => o.Modify(b => b.Remove(() => o.ProcessOutputLogging));

    #endregion

    #region ToolOptions.ProcessInvocationLogging

    /// <summary><p>Defines the log-level for the process invocation.</p></summary>
    [Builder(Type = typeof(ToolOptions), Property = nameof(ToolOptions.ProcessInvocationLogging))]
    public static T SetProcessInvocationLogging<T>(this T o, LogEventLevel? value) where T : ToolOptions => o.Modify(b => b.Set(() => o.ProcessInvocationLogging, value));

    /// <summary><p>Defines the log-level for the process invocation.</p></summary>
    [Builder(Type = typeof(ToolOptions), Property = nameof(ToolOptions.ProcessInvocationLogging))]
    public static T ResetProcessInvocationLogging<T>(this T o) where T : ToolOptions => o.Modify(b => b.Remove(() => o.ProcessInvocationLogging));

    #endregion
}
