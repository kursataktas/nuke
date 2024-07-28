﻿// Copyright 2024 Maintainers of NUKE.
// Distributed under the MIT License.
// https://github.com/nuke-build/nuke/blob/master/LICENSE

using System;
// ReSharper disable ArrangeMethodOrOperatorBody

namespace Nuke.Tooling;

partial class ToolOptionsExtensions
{
    /// <inheritdoc cref="ToolOptions.ProcessOutputLogging"/>
    [Obsolete($"Use {nameof(DisableProcessOutputLogging)} instead")]
    [Builder(Type = typeof(ToolOptions), Property = nameof(ToolOptions.ProcessOutputLogging))]
    public static T DisableProcessLogOutput<T>(this T o) where T : ToolOptions => o.DisableProcessOutputLogging();

    /// <inheritdoc cref="ToolOptions.ProcessInvocationLogging"/>
    [Obsolete($"Use {nameof(DisableProcessInvocationLogging)} instead")]
    [Builder(Type = typeof(ToolOptions), Property = nameof(ToolOptions.ProcessInvocationLogging))]
    public static T DisableProcessLogInvocation<T>(this T o) where T : ToolOptions => o.DisableProcessInvocationLogging();

}
