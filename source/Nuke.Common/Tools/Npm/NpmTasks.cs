// Copyright 2023 Maintainers of NUKE.
// Distributed under the MIT License.
// https://github.com/nuke-build/nuke/blob/master/LICENSE

using System;
using Nuke.Common.Tooling;
using Nuke.Tooling;
using Serilog;
using Serilog.Events;

namespace Nuke.Common.Tools.Npm;

[LogLevelPattern(LogEventLevel.Warning, "^(npmWARN|npm WARN)")]
[LogLevelPattern(LogEventLevel.Debug, "^(npm notice)")]
partial class NpmTasks2 : ToolTasks
{
    protected internal override Action<OutputType, string> GetLogger()
    {
        return (type, text) =>
        {
            switch (type)
            {
                case OutputType.Std:
                    Log.Debug(text);
                    break;
                case OutputType.Err:
                {
                    if (text.StartsWith("npmWARN") || text.StartsWith("npm WARN"))
                        Log.Warning(text);
                    else if(text.StartsWith("npm notice"))
                        Log.Debug(text);
                    else
                        Log.Error(text);

                    break;
                }
            }
        };
    }
}

partial class NpmTasks
{
    public static void CustomLogger(OutputType type, string output)
    {
        switch (type)
        {
            case OutputType.Std:
                Log.Debug(output);
                break;
            case OutputType.Err:
            {
                if (output.StartsWith("npmWARN") || output.StartsWith("npm WARN"))
                    Log.Warning(output);
                else if(output.StartsWith("npm notice"))
                    Log.Debug(output);
                else
                    Log.Error(output);

                break;
            }
        }
    }
}
