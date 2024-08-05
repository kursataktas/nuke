// Copyright 2024 Maintainers of NUKE.
// Distributed under the MIT License.
// https://github.com/nuke-build/nuke/blob/master/LICENSE

using System;
using System.Collections.Generic;
using System.Linq;
using Nuke.Common.Tooling;
using Nuke.Common.Utilities;
using Nuke.Common.Utilities.Collections;

namespace Nuke.Tooling;

partial class ToolTasks
{
    protected static IReadOnlyCollection<Output> Run<T>(ToolOptions options)
        where T : ToolTasks, new()
    {
        var tool = new T();
        var secrets = options.GetSecrets();

        options = tool.PreProcess(options);
        using var process = ProcessTasks.StartProcess(
            tool.GetToolPathInternal(),
            options.GetArguments().JoinSpace(),
            options.ProcessWorkingDirectory,
            options.ProcessEnvironmentVariables.ToDictionary(x => x.Key, x => x.Value.ToString()),
            options.ProcessExecutionTimeout,
            options.ProcessOutputLogging,
            options.ProcessInvocationLogging,
            tool.GetLogger(),
            text => secrets.Aggregate(text, (str, s) => str.Replace(s, "[REDACTED]")));

        tool.GetExitHandlerInternal().Invoke(process);
        tool.PostProcess(options);

        return process.Output;
    }

    protected static (TResult Result, IReadOnlyCollection<Output> Output) Run<T, TResult>(ToolOptions options)
        where T : ToolTasks, new()
    {
        var output = Run<T>(options);
        try
        {
            var result = new T().GetResult<TResult>(options, output);
            return (Result: (TResult)result, Output: output);
        }
        catch (Exception exception)
        {
            throw new Exception($"Cannot parse {typeof(TResult).Name} from output:".Concat(output.Select(x => x.Text)).JoinNewLine(), exception);
        }
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
        Func<IProcess, object> exitHandler = null)
        where T : ToolTasks, new()
    {
        var tool = new T();
        using var process = ProcessTasks.StartProcess(
            tool.GetToolPathInternal(),
            arguments,
            workingDirectory,
            environmentVariables,
            timeout,
            logOutput,
            logInvocation,
            logger ?? tool.GetLogger());
        (exitHandler ?? tool.GetExitHandlerInternal()).Invoke(process);
        return process.Output;
    }

#endif
}
