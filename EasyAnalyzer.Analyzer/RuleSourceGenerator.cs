using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace EasyAnalyzer.Analyzer;

[Generator(LanguageNames.CSharp)]
public class RuleSourceGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var rules = context.SyntaxProvider
            .CreateSyntaxProvider(IsInterestingSyntax, static (ctx, ct) => ctx.Node as ClassDeclarationSyntax)
            .SelectMany(CollectMethods)
            .Collect()
            .Combine(context.CompilationProvider);

        context.RegisterSourceOutput(rules, MapOutput);
    }

    private static bool IsInterestingSyntax(SyntaxNode node, CancellationToken ct)
    {
        if (node is not ClassDeclarationSyntax cds) return false;
        if (cds.BaseList == null || cds.BaseList.Types.Count == 0) return false;
        return true;
    }

    private static IEnumerable<MethodDeclarationSyntax> CollectMethods(ClassDeclarationSyntax? syntax, CancellationToken ct)
    {
        if (syntax == null) yield break;
        foreach (var member in syntax.Members)
        {
            if (member is MethodDeclarationSyntax mds) yield return mds;
        }
    }

    private static void MapOutput(SourceProductionContext ctx, (ImmutableArray<MethodDeclarationSyntax> methods, Compilation compilation) input)
    {
        var (methods, compilation) = input;
        foreach (var groupedMethods in methods.GroupBy(m => m.SyntaxTree))
        {
            var semanticModel = compilation.GetSemanticModel(groupedMethods.Key);
            var o = new IndentedStringBuilder();
            o.AppendLine("#if DEBUG");
            o.AppendLine("[global::EasyAnalyzer.Abstractions.RuleDefinitionAttribute]");
            o.AppendLine("public class ABC");
            using (o.IndentBraces)
            {
                // Loop through all methods
                foreach (var method in groupedMethods)
                {
                    var methodSymbol = semanticModel.GetDeclaredSymbol(method);
                    o.AppendLine($"// {methodSymbol?.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)}");
                }
            }

            o.AppendLine("#endif");

            ctx.AddSource("ABC.cs", o.ToString());
        }
    }
}