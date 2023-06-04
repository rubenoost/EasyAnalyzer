using EasyAnalyzer.Abstractions;
using EasyAnalyzer.Sandbox.Extensions;

namespace EasyAnalyzer.Sandbox;

public partial class Rules : RuleProvider
{
    private void UseFilterNull<T>()
    {
        Match(() =>
            {
                // Whatever
                Any<IEnumerable<T>>("x1").Where(x => x != null);
            })
            .Warn("Use FilterNull()")
            .CodeFix(() =>
            {
                // Whatever
                Any<IEnumerable<T>>("x1").FilterNull();
            });
    }
    
    
    // Should generate:
    
    // [RuleSource]
    public class Generated
    {
        public string Rule001 = "...";
    }
}