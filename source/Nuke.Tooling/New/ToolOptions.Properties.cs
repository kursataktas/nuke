// Copyright 2024 Maintainers of NUKE.
// Distributed under the MIT License.
// https://github.com/nuke-build/nuke/blob/master/LICENSE

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using JetBrains.Annotations;
using Serilog.Events;
// ReSharper disable ArrangeMethodOrOperatorBody

namespace Nuke.Tooling;

partial class ToolOptions
{
    /// <summary><p>Defines the path of the tool to be invoked. In most cases, the tool path is automatically resolved from the PATH environment variable or a NuGet package.</p></summary>
    public string ProcessToolPath => Get<string>(() => ProcessToolPath);

    /// <summary><p>Defines the working directory for the process.</p></summary>
    public string ProcessWorkingDirectory => Get<string>(() => ProcessWorkingDirectory);

    /// <summary><p>Defines the environment variables to be passed to the process. By default, the environment variables of the current process are used.</p></summary>
    public IReadOnlyDictionary<string, object> ProcessEnvironmentVariables => Get<Dictionary<string, object>>(() => ProcessEnvironmentVariables);

    /// <summary><p>Defines the execution timeout of the invoked process.</p></summary>
    public int? ProcessExecutionTimeout => Get<int?>(() => ProcessExecutionTimeout);

    /// <summary><p>Defines whether to log output.</p></summary>
    public bool? ProcessOutputLogging => Get<bool?>(() => ProcessOutputLogging);

    /// <summary><p>Defines whether to log the invocation.</p></summary>
    public bool? ProcessInvocationLogging => Get<bool?>(() => ProcessInvocationLogging);

    /// <summary><p>Defines whether to handle the process on exit.</p></summary>
    public bool? ProcessExitHandling => Get<bool?>(() => ProcessExitHandling);

    public IReadOnlyList<string> ProcessRedactedSecrets => Get<List<string>>(() => ProcessRedactedSecrets);
}

[PublicAPI]
public static partial class ToolOptionsExtensions
{
    #region ToolOptions.ProcessToolPath

    /// <inheritdoc cref="ToolOptions.ProcessToolPath"/>
    [Builder(Type = typeof(ToolOptions), Property = nameof(ToolOptions.ProcessToolPath))]
    public static T SetProcessToolPath<T>(this T o, string value) where T : ToolOptions => o.Modify(b => b.Set(() => o.ProcessToolPath, value));

    /// <inheritdoc cref="ToolOptions.ProcessToolPath"/>
    [Builder(Type = typeof(ToolOptions), Property = nameof(ToolOptions.ProcessToolPath))]
    public static T ResetProcessToolPath<T>(this T o) where T : ToolOptions => o.Modify(b => b.Remove(() => o.ProcessToolPath));

    #endregion

    #region ToolOptions.ProcessWorkingDirectory

    /// <inheritdoc cref="ToolOptions.ProcessWorkingDirectory"/>
    [Builder(Type = typeof(ToolOptions), Property = nameof(ToolOptions.ProcessWorkingDirectory))]
    public static T SetProcessWorkingDirectory<T>(this T o, string value) where T : ToolOptions => o.Modify(b => b.Set(() => o.ProcessWorkingDirectory, value));

    /// <inheritdoc cref="ToolOptions.ProcessWorkingDirectory"/>
    [Builder(Type = typeof(ToolOptions), Property = nameof(ToolOptions.ProcessWorkingDirectory))]
    public static T ResetProcessWorkingDirectory<T>(this T o) where T : ToolOptions => o.Modify(b => b.Remove(() => o.ProcessWorkingDirectory));

    #endregion

    #region ToolOptions.ProcessEnvironmentVariables

    /// <inheritdoc cref="ToolOptions.ProcessEnvironmentVariables"/>
    [Builder(Type = typeof(ToolOptions), Property = nameof(ToolOptions.ProcessEnvironmentVariables))]
    public static T SetProcessEnvironmentVariables<T>(this T o, ReadOnlyDictionary<string, object> values) where T : ToolOptions => o.Modify(b => b.Set(() => o.ProcessEnvironmentVariables, values));

    /// <inheritdoc cref="ToolOptions.ProcessEnvironmentVariables"/>
    [Builder(Type = typeof(ToolOptions), Property = nameof(ToolOptions.ProcessEnvironmentVariables))]
    public static T SetProcessEnvironmentVariables<T>(this T o, Dictionary<string, object> values) where T : ToolOptions => o.Modify(b => b.Set(() => o.ProcessEnvironmentVariables, values));

    /// <inheritdoc cref="ToolOptions.ProcessEnvironmentVariables"/>
    [Builder(Type = typeof(ToolOptions), Property = nameof(ToolOptions.ProcessEnvironmentVariables))]
    public static T AddProcessEnvironmentVariables<T>(this T o, ReadOnlyDictionary<string, object> values) where T : ToolOptions => o.Modify(b => b.AddDictionary(() => o.ProcessEnvironmentVariables, values));

    /// <inheritdoc cref="ToolOptions.ProcessEnvironmentVariables"/>
    [Builder(Type = typeof(ToolOptions), Property = nameof(ToolOptions.ProcessEnvironmentVariables))]
    public static T AddProcessEnvironmentVariables<T>(this T o, Dictionary<string, object> values) where T : ToolOptions => o.Modify(b => b.AddDictionary(() => o.ProcessEnvironmentVariables, values));

    /// <inheritdoc cref="ToolOptions.ProcessEnvironmentVariables"/>
    [Builder(Type = typeof(ToolOptions), Property = nameof(ToolOptions.ProcessEnvironmentVariables))]
    public static T AddProcessEnvironmentVariable<T>(this T o, string key, object value) where T : ToolOptions => o.Modify(b => b.AddDictionary(() => o.ProcessEnvironmentVariables, key, value));

    /// <inheritdoc cref="ToolOptions.ProcessEnvironmentVariables"/>
    [Builder(Type = typeof(ToolOptions), Property = nameof(ToolOptions.ProcessEnvironmentVariables))]
    public static T SetProcessEnvironmentVariable<T>(this T o, string key, object value) where T : ToolOptions => o.Modify(b => b.SetDictionary(() => o.ProcessEnvironmentVariables, key, value));

    /// <inheritdoc cref="ToolOptions.ProcessEnvironmentVariables"/>
    [Builder(Type = typeof(ToolOptions), Property = nameof(ToolOptions.ProcessEnvironmentVariables))]
    public static T RemoveProcessEnvironmentVariable<T>(this T o, string key) where T : ToolOptions => o.Modify(b => b.RemoveDictionary(() => o.ProcessEnvironmentVariables, key));

    /// <inheritdoc cref="ToolOptions.ProcessEnvironmentVariables"/>
    [Builder(Type = typeof(ToolOptions), Property = nameof(ToolOptions.ProcessEnvironmentVariables))]
    public static T ClearProcessEnvironmentVariables<T>(this T o) where T : ToolOptions => o.Modify(b => b.ClearDictionary(() => o.ProcessEnvironmentVariables));

    /// <inheritdoc cref="ToolOptions.ProcessEnvironmentVariables"/>
    [Builder(Type = typeof(ToolOptions), Property = nameof(ToolOptions.ProcessEnvironmentVariables))]
    public static T ResetProcessEnvironmentVariables<T>(this T o) where T : ToolOptions => o.Modify(b => b.Remove(() => o.ProcessEnvironmentVariables));

    #endregion

    #region ToolOptions.ProcessExecutionTimeout

    /// <inheritdoc cref="ToolOptions.ProcessExecutionTimeout"/>
    [Builder(Type = typeof(ToolOptions), Property = nameof(ToolOptions.ProcessExecutionTimeout))]
    public static T SetProcessExecutionTimeout<T>(this T o, int? value) where T : ToolOptions => o.Modify(b => b.Set(() => o.ProcessExecutionTimeout, value));

    /// <inheritdoc cref="ToolOptions.ProcessExecutionTimeout"/>
    [Builder(Type = typeof(ToolOptions), Property = nameof(ToolOptions.ProcessExecutionTimeout))]
    public static T SetProcessExecutionTimeout<T>(this T o, TimeSpan value) where T : ToolOptions => o.Modify(b => b.Set(() => o.ProcessExecutionTimeout, value.TotalMilliseconds));

    /// <inheritdoc cref="ToolOptions.ProcessExecutionTimeout"/>
    [Builder(Type = typeof(ToolOptions), Property = nameof(ToolOptions.ProcessExecutionTimeout))]
    public static T ResetProcessExecutionTimeout<T>(this T o) where T : ToolOptions => o.Modify(b => b.Remove(() => o.ProcessExecutionTimeout));

    #endregion

    #region ToolOptions.ProcessOutputLogging

    /// <inheritdoc cref="ToolOptions.ProcessOutputLogging"/>
    [Builder(Type = typeof(ToolOptions), Property = nameof(ToolOptions.ProcessOutputLogging))]
    public static T SetProcessOutputLogging<T>(this T o, bool? value) where T : ToolOptions => o.Modify(b => b.Set(() => o.ProcessOutputLogging, value));

    /// <inheritdoc cref="ToolOptions.ProcessOutputLogging"/>
    [Builder(Type = typeof(ToolOptions), Property = nameof(ToolOptions.ProcessOutputLogging))]
    public static T EnableProcessOutputLogging<T>(this T o) where T : ToolOptions => o.Modify(b => b.Set(() => o.ProcessOutputLogging, value: true));

    /// <inheritdoc cref="ToolOptions.ProcessOutputLogging"/>
    [Builder(Type = typeof(ToolOptions), Property = nameof(ToolOptions.ProcessOutputLogging))]
    public static T DisableProcessOutputLogging<T>(this T o) where T : ToolOptions => o.Modify(b => b.Set(() => o.ProcessOutputLogging, value: false));

    /// <inheritdoc cref="ToolOptions.ProcessOutputLogging"/>
    [Builder(Type = typeof(ToolOptions), Property = nameof(ToolOptions.ProcessOutputLogging))]
    public static T ResetProcessOutputLogging<T>(this T o) where T : ToolOptions => o.Modify(b => b.Remove(() => o.ProcessOutputLogging));

    #endregion

    #region ToolOptions.ProcessInvocationLogging

    /// <inheritdoc cref="ToolOptions.ProcessInvocationLogging"/>
    [Builder(Type = typeof(ToolOptions), Property = nameof(ToolOptions.ProcessInvocationLogging))]
    public static T SetProcessInvocationLogging<T>(this T o, bool? value) where T : ToolOptions => o.Modify(b => b.Set(() => o.ProcessInvocationLogging, value));

    /// <inheritdoc cref="ToolOptions.ProcessInvocationLogging"/>
    [Builder(Type = typeof(ToolOptions), Property = nameof(ToolOptions.ProcessInvocationLogging))]
    public static T EnableProcessInvocationLogging<T>(this T o) where T : ToolOptions => o.Modify(b => b.Set(() => o.ProcessInvocationLogging, value: true));

    /// <inheritdoc cref="ToolOptions.ProcessInvocationLogging"/>
    [Builder(Type = typeof(ToolOptions), Property = nameof(ToolOptions.ProcessInvocationLogging))]
    public static T DisableProcessInvocationLogging<T>(this T o) where T : ToolOptions => o.Modify(b => b.Set(() => o.ProcessInvocationLogging, value: false));

    /// <inheritdoc cref="ToolOptions.ProcessInvocationLogging"/>
    [Builder(Type = typeof(ToolOptions), Property = nameof(ToolOptions.ProcessInvocationLogging))]
    public static T ResetProcessInvocationLogging<T>(this T o) where T : ToolOptions => o.Modify(b => b.Remove(() => o.ProcessInvocationLogging));

    #endregion

    #region ToolOptions.ProcessExitHandling

    /// <inheritdoc cref="ToolOptions.ProcessExitHandling"/>
    [Builder(Type = typeof(ToolOptions), Property = nameof(ToolOptions.ProcessExitHandling))]
    public static T SetProcessExitHandling<T>(this T o, LogEventLevel? value) where T : ToolOptions => o.Modify(b => b.Set(() => o.ProcessExitHandling, value));

    /// <inheritdoc cref="ToolOptions.ProcessExitHandling"/>
    [Builder(Type = typeof(ToolOptions), Property = nameof(ToolOptions.ProcessExitHandling))]
    public static T EnableProcessExitHandling<T>(this T o) where T : ToolOptions => o.Modify(b => b.Set(() => o.ProcessExitHandling, value: true));

    /// <inheritdoc cref="ToolOptions.ProcessExitHandling"/>
    [Builder(Type = typeof(ToolOptions), Property = nameof(ToolOptions.ProcessExitHandling))]
    public static T DisableProcessExitHandling<T>(this T o) where T : ToolOptions => o.Modify(b => b.Set(() => o.ProcessExitHandling, value: false));

    /// <inheritdoc cref="ToolOptions.ProcessExitHandling"/>
    [Builder(Type = typeof(ToolOptions), Property = nameof(ToolOptions.ProcessExitHandling))]
    public static T ToggleProcessExitHandling<T>(this T o) where T : ToolOptions => o.Modify(b => b.Set(() => o.ProcessExitHandling, !o.ProcessExitHandling));

    /// <inheritdoc cref="ToolOptions.ProcessExitHandling"/>
    [Builder(Type = typeof(ToolOptions), Property = nameof(ToolOptions.ProcessExitHandling))]
    public static T ResetProcessExitHandling<T>(this T o) where T : ToolOptions => o.Modify(b => b.Remove(() => o.ProcessExitHandling));

    #endregion
}
