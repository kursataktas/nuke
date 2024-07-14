using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Nuke.Common.Utilities;
using VerifyXunit;
using Xunit;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.EventEmitters;

public class MyClass
{
    // name: ubuntu-latest
    //
    // on:
    //   push:
    //     branches-ignore:
    //       - master
    //       - 'release/*'
    //   pull_request:
    //     branches:
    //       - develop
    //
    // jobs:
    //   ubuntu-latest:
    //     name: ubuntu-latest
    //     runs-on: ubuntu-latest
    //     steps:
    //       - uses: actions/checkout@v3
    //         with:
    //           fetch-depth: 0
    //       - name: 'Cache: .nuke/temp, ~/.nuget/packages'
    //         uses: actions/cache@v4
    //         with:
    //           path: |
    //             .nuke/temp
    //             ~/.nuget/packages
    //           key: ${{ runner.os }}-${{ hashFiles('**/global.json', '**/*.csproj', '**/Directory.Packages.props') }}
    //       - name: 'Run: Test, Pack'
    //         run: ./build.cmd Test Pack

    [Fact]
    public async Task Foo()
    {
        var workflow = new GitHubWorkflow();
        workflow.OnPush("foobar");
        workflow.Clone().OnPushIgnore("foobar");

        // dynamic obj = new ExpandoObject();
        // dynamic on = new ExpandoObject();
        // IDictionary<string, object> jobs = new ExpandoObject();
        // obj.on = on;
        // on.push = true;
        // obj.jobs = jobs;
        // jobs["ubuntu-latest"] = "bar";
        //
        // var jobs2 = obj.jobs as IDictionary<string, object>;
        // jobs2["foo"] = "bar";
        // jobs2["props"] = new[]
        //                  {
        //                      on, on
        //                  };
        //
        // var step = """
        //            name: 'Cache: .nuke/temp, ~/.nuget/packages'
        //            uses: actions/cache@v4
        //            with:
        //              path: |
        //                .nuke/temp
        //                ~/.nuget/packages
        //              key: ${{ runner.os }}-${{ hashFiles('**/global.json', '**/*.csproj', '**/Directory.Packages.props') }}
        //            """;
        // var deserializer = new Deserializer();
        // var expandoObject = deserializer.Deserialize<ExpandoObject>(step);
        // var exapndoObject2 = expandoObject.Combine(new { whatever = new { moep = "yes"} }.ToExpandoObject());
        // jobs2["tasks"] = new[]{exapndoObject2};
        //
        // var build = new SerializerBuilder()
        //     .WithIndentedSequences()
        //     .WithDefaultScalarStyle(ScalarStyle.Any)
        //     .WithEventEmitter(x => new MultilineScalarFlowStyleEmitter(x))
        //     .Build();
        // var yaml = build.Serialize((object)obj);

        await Verifier.Verify(workflow.ToString());
    }

}
