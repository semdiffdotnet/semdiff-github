using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;

namespace SemDiff
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class SemDiffAnalyzer : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => SemDiffDiagnostics.GetSupportedDiagnostics();
        private SemDiffDiagnostics Diagnostics { get; } = new SemDiffDiagnostics();
        private SemDiffRepoManager RepoManager { get; } = new SemDiffRepoManager();

        //Called once per solution, to provide us an oportunity to assign callbacks
        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxTreeAction(OnSyntaxTree);
            //context.RegisterSemanticModelAction(OnSemanticModel);
        }

        private void OnSyntaxTree(SyntaxTreeAnalysisContext obj)
        {
            var filePath = obj.Tree.FilePath;
            var repoManager = RepoManager.GetRepoFor(filePath);
            if (repoManager == null)
                return;
        }

        private void OnSemanticModel(SemanticModelAnalysisContext obj)
        {
            var filePath = obj.SemanticModel.SyntaxTree.FilePath;
            var repoManager = RepoManager.GetRepoFor(filePath);
            if (repoManager == null)
                return;
        }
    }
}