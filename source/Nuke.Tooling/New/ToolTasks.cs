// Copyright 2024 Maintainers of NUKE.
// Distributed under the MIT License.
// https://github.com/nuke-build/nuke/blob/master/LICENSE

using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Nuke.Common.Tooling;
using Nuke.Common.Utilities;

namespace Nuke.Tooling;

[PublicAPI]
public abstract partial class ToolTasks
{
    public static string GetToolPath<T>() where T : ToolTasks, new() => new T().GetToolPath();

    protected internal virtual partial string GetToolPath(ToolOptions options = null);
    protected internal virtual partial Action<OutputType, string> GetLogger();
    protected internal virtual partial Action<IProcess> GetExitHandler(ToolOptions options = null);

    protected virtual ToolOptions PreProcess(ToolOptions options) => options;
    protected virtual void PostProcess(ToolOptions options) { }
}

partial class ToolTasks
{
    protected static IReadOnlyCollection<Output> Run<T>(ToolOptions options)
        where T : ToolTasks, new()
    {
        var tool = new T();
        var secrets = options.GetSecrets();

        options = tool.PreProcess(options);
        using var process = ProcessTasks.StartProcess(
            tool.GetToolPath(),
            options.GetArguments().JoinSpace(),
            options.ProcessWorkingDirectory,
            options.ProcessEnvironmentVariables.ToDictionary(x => x.Key, x => x.Value.ToString()),
            options.ProcessExecutionTimeout,
            options.ProcessOutputLogging,
            options.ProcessInvocationLogging,
            options.GetLogger(),
            text => secrets.Aggregate(text, (str, s) => str.Replace(s, "[REDACTED]")));
        tool.PostProcess(options);

        tool.GetExitHandler().Invoke(process);
        return process.Output;
    }

    protected static (T Result, IReadOnlyCollection<Output> Output) Run<T, TResult>(ToolOptions options)
        where T : ToolTasks, new()
    {
        var output = Run<T>(options);
        return (Result: default, Output: output);
    }

#if NET6_0_OR_GREATER

    protected static IReadOnlyCollection<Output> Run<T>(
        ArgumentStringHandler arguments,
        string workingDirectory = null,
        IReadOnlyDictionary<string, string> environmentVariables = null,
        int? timeout = null,
        bool? logOutput = null,
        bool? logInvocation = null,
        Action<OutputType, string> logger = null,
        Action<IProcess> exitHandler = null)
        where T : ToolTasks, new()
    {
        var tool = new T();
        using var process = ProcessTasks.StartProcess(
            tool.GetToolPath(),
            arguments,
            workingDirectory,
            environmentVariables,
            timeout,
            logOutput,
            logInvocation,
            logger ?? tool.GetLogger());
        (exitHandler ?? tool.GetExitHandler()).Invoke(process);
        return process.Output;
    }

#endif
}
