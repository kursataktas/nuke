// Copyright 2024 Maintainers of NUKE.
// Distributed under the MIT License.
// https://github.com/nuke-build/nuke/blob/master/LICENSE

using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using Nuke.Common;
using Nuke.Common.Tooling;
using Nuke.Common.Utilities;
using Serilog;
using Serilog.Events;

namespace Nuke.Tooling;

partial class ToolOptions
{
    internal partial Action<OutputType, string> GetLogger()
    {
        var optionsType = GetType();
        var toolType = optionsType.GetCustomAttribute<CommandAttribute>().NotNull().Type;
        var levelProvider = toolType.GetCustomAttributes<LogLevelPattern>()
            .Select(x =>
            {
                var regex = new Regex(x.Pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                return new Func<string, LogEventLevel?>(text => regex.IsMatch(text) ? x.Level : null);
            }).ToList();

        var errorAsStandard = toolType.HasCustomAttribute<LogErrorAsStandard>();
        var defaultLevel = toolType.GetCustomAttribute<DefaultLogLevel>()?.Level ?? LogEventLevel.Debug;

        return (type, text) =>
        {
            var patternLevel = levelProvider.Select(x => x.Invoke(text)).FirstOrDefault(x => x != null);
            if (patternLevel != null)
                Log.Write(patternLevel.Value, text);
            else if (type == OutputType.Err && !errorAsStandard)
                Log.Error(text);
            else
                Log.Write(defaultLevel, text);
        };
    }
}
