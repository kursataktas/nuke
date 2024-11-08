// Copyright 2024 Maintainers of NUKE.
// Distributed under the MIT License.
// https://github.com/nuke-build/nuke/blob/master/LICENSE

using System;
using System.Linq;
using Newtonsoft.Json;

namespace Nuke.Common.Tooling;

public static partial class OptionsExtensions
{
    internal static T Modify<T>(this T options, Action<Options> modification = null)
        where T : Options
    {
        var serializedObject = JsonConvert.SerializeObject(options);
        var copiedObject = JsonConvert.DeserializeObject<T>(serializedObject);
        modification?.Invoke(copiedObject);

        // TODO OPTIONS: HACK
        if (options is ToolOptions originalToolOptions && copiedObject is ToolOptions copiedToolOptions)
        {
            copiedToolOptions.ProcessLogger = originalToolOptions.ProcessLogger;
            // exit handler?
        }

        return copiedObject;
    }
}
