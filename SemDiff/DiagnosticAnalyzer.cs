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
        private SemDiffRepoManager RepoManager { get; set; }

        public override void Initialize(AnalysisContext context)
        {
            RepoManager = SemDiffRepoManager.GetRepo();
            context.RegisterCompilationStartAction(OnCompilationStart);
        }

        private void OnCompilationStart(CompilationStartAnalysisContext context)
        {
            RepoManager.TriggerUpdate();
            context.RegisterSyntaxTreeAction(OnSyntaxTree);
            context.RegisterSemanticModelAction(OnSemanticModel);
        }

        private void OnSemanticModel(SemanticModelAnalysisContext obj)
        {
            throw new NotImplementedException();
        }

        private void OnSyntaxTree(SyntaxTreeAnalysisContext obj)
        {
            throw new NotImplementedException();
        }
    }
}