
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using Nuke.Common.Utilities;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.EventEmitters;

public static class Extensions
{
    public static ExpandoObject Combine(this ExpandoObject obj1, ExpandoObject obj2)
    {
        var dictionary1 = (IDictionary<string, object>)obj1;
        var dictionary2 = (IDictionary<string, object>)obj2;
        var pairs = dictionary1.Concat(dictionary2);

        var result = new ExpandoObject() as IDictionary<string, object>;
        foreach (var pair in pairs)
            result[pair.Key] = pair.Value;

        return (ExpandoObject)result;
    }

    public static ExpandoObject ToExpandoObject(this object obj)
    {
        IDictionary<string, object> expando = new ExpandoObject();

        foreach (System.ComponentModel.PropertyDescriptor property in TypeDescriptor.GetProperties(obj.GetType()))
            expando.Add(property.Name, property.GetValue(obj));

        return (ExpandoObject)expando;
    }
}

public class ConfigurationBuilder
{
    internal IDictionary<string, object> Object = new ExpandoObject();

    public override string ToString()
    {
        var build = new SerializerBuilder()
            .WithIndentedSequences()
            .WithDefaultScalarStyle(ScalarStyle.Any)
            .WithEventEmitter(x => new MultilineScalarFlowStyleEmitter(x))
            .Build();
        return build.Serialize(Object);
    }
}

public static class ConfigurationBuilderExtensions
{
    internal static T Clone<T>(this T builder)
        where T : ConfigurationBuilder, new()
    {
        var old = builder.ToString();
        var new2 = new T();
        new2.Object = new Deserializer().Deserialize<ExpandoObject>(old);
        return new2;
    }

    internal static T SetProperty<T>(this T builder, string name, object value)
        where T : ConfigurationBuilder, new()
    {
        var clone = builder.Clone();
        var existingProperty = builder.Object[name];
        // builder.Object[name] = existingProperty != null;
        return builder;
    }
}

public class GitHubWorkflow : ConfigurationBuilder
{
    public GitHubWorkflow OnPush(params string[] branches)
    {
        return this.SetProperty("on", new { push = new { branches } });
    }

    public GitHubWorkflow OnPushIgnore(params string[] branches)
    {
        return this.SetProperty("on", new { push = new { branches_ignore = branches } });
    }
}


public class MultilineScalarFlowStyleEmitter(IEventEmitter nextEmitter)
    : ChainedEventEmitter(nextEmitter)
{
    public override void Emit(ScalarEventInfo eventInfo, IEmitter emitter)
    {
        if (typeof(string).IsAssignableFrom(eventInfo.Source.Type))
        {
            var value = eventInfo.Source.Value as string;
            if (!value.IsNullOrEmpty() && value.IndexOfAny(['\r', '\n', '\x85', '\x2028', '\x2029']) >= 0)
            {
                eventInfo = new ScalarEventInfo(eventInfo.Source) { Style = ScalarStyle.Literal };
            }
        }

        nextEmitter.Emit(eventInfo, emitter);
    }
}
