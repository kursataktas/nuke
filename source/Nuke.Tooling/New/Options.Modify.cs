// Copyright 2024 Maintainers of NUKE.
// Distributed under the MIT License.
// https://github.com/nuke-build/nuke/blob/master/LICENSE

using System;
using System.Linq;
using Newtonsoft.Json;

namespace Nuke.Tooling;

public static class OptionsExtensions
{
    internal static T Modify<T>(this T builder, Action<Options> modification = null)
        where T : Options
    {
        var serializedObject = JsonConvert.SerializeObject(builder);
        var copiedObject = JsonConvert.DeserializeObject<T>(serializedObject);
        modification?.Invoke(copiedObject);
        return copiedObject;
    }
}
