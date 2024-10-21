// Copyright 2023 Maintainers of NUKE.
// Distributed under the MIT License.
// https://github.com/nuke-build/nuke/blob/master/LICENSE

using System;
using System.Linq;
using JetBrains.Annotations;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tools.GitHub;
using Nuke.Common.Utilities;
using Nuke.Common.Utilities.Collections;
using static Nuke.CodeGeneration.CodeGenerator;
using static Nuke.CodeGeneration.ReferenceUpdater;
using static Nuke.Common.Tools.Git.GitTasks;

partial class Build
{
    AbsolutePath SpecificationsDirectory => RootDirectory / "source" / "Nuke.Common" / "Tools";
    AbsolutePath ReferencesDirectory => BuildProjectDirectory / "references";

    Target References => _ => _
        .Requires(() => GitHasCleanWorkingCopy())
        .Executes(() =>
        {
            ReferencesDirectory.CreateOrCleanDirectory();

            UpdateReferences(SpecificationsDirectory, ReferencesDirectory);
        });

    [UsedImplicitly]
    Target GenerateTools => _ => _
        .Executes(() =>
        {
            SpecificationsDirectory.GlobFiles("*/*.json").Where(x => x.Name.EqualsAnyOrdinalIgnoreCase([
                "AzureSignTool.json",
                "BenchmarkDotNet.json",
                "Boots.json",
                "Chocolatey.json",
                // "CloudFoundry.json",
                "CodeMetrics.json",
                "Codecov.json",
                "CorFlags.json",
                "CoverallsNet.json",
                "Coverlet.json",
                "Discord.json",
                "DocFX.json",
                "Docker.json",
                "DotCover.json",
                "DotMemoryUnit.json",
                "DotNet.json",
                "DotnetPackaging.json",
                "EntityFramework.json",
                "Fixie.json",
                "Git.json",
                "GitLink.json",
                "GitReleaseManager.json",
                "GitVersion.json",
                "Helm.json",
                "ILRepack.json",
                "InnoSetup.json",
                "Kubernetes.json",
                "MSBuild.json",
                "MSpec.json",
                "MakeNSIS.json",
                "Mastodon.json",
                "MauiCheck.json",
                "MinVer.json",
                "NSwag.json",
                "NUnit.json",
                "NerdbankGitVersioning.json",
                "Netlify.json",
                "Npm.json",
                "NuGet.json",
                "OctoVersion.json",
                "Octopus.json",
                "OpenCover.json",
                "Paket.json",
                "PowerShell.json",
                "Pulumi.json",
                "Pwsh.json",
                "ReSharper.json",
                "ReportGenerator.json",
                "SignClient.json",
                "SignTool.json",
                "Slack.json",
                "SonarScanner.json",
                "SpecFlow.json",
                "Squirrel.json",
                "StaticWebApps.json",
                "Teams.json",
                "TestCloud.json",
                // "Unity.json",
                "VSTest.json",
                "VSWhere.json",
                "WebConfigTransformRunner.json",
                "Xunit.json",
            ])).ForEach(x =>
                GenerateCode(
                    x,
                    namespaceProvider: x => $"Nuke.Common.Tools.{x.Name}",
                    sourceFileProvider: x => GitRepository.SetBranch(MasterBranch).GetGitHubBrowseUrl(x.SpecificationFile)));
        });
}
