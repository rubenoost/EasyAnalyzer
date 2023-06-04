namespace EasyAnalyzer.Abstractions;

public class RuleProvider
{
    protected RuleConfiguration Match(Action a) => default;
    protected T Any<T>(string s) => default;
}

public class RuleConfiguration
{
    public RuleConfiguration CodeFix(Action a) => default;
    public RuleConfiguration Warn(string s) => default;
}

[AttributeUsage(AttributeTargets.Class)]
public class RuleDefinitionAttribute : Attribute { }