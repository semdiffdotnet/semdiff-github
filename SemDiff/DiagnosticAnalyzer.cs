using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using System.Collections.Immutable;
using System.IO;

namespace SemDiffAnalyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class SemDiffAnalyzerAnalyzer : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => Diagnostics.GetSupportedDiagnostics();
        private SemDiffDiagnostics Diagnostics { get; } = new SemDiffDiagnostics();

        //Called once per solution, to provide us an oportunity to assign callbacks
        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxTreeAction(OnSyntaxTree);
            context.RegisterSemanticModelAction(OnSemanticModel);
        }        

        private void OnSemanticModel(SemanticModelAnalysisContext obj)
        {
            var filePath = obj.SemanticModel.SyntaxTree.FilePath;
            var repoManager = SemDiffRepoManager.GetRepoFor(filePath);
            if (repoManager == null)
                return;
        }

        private void OnSyntaxTree(SyntaxTreeAnalysisContext obj)
        {
            var filePath = obj.Tree.FilePath;
            var repoManager = SemDiffRepoManager.GetRepoFor(filePath);
            if (repoManager == null)
                return;
        }
    }
}