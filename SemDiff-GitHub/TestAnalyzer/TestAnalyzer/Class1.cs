using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using System;

public interface Class1
{
    void OnSyntaxTree(SyntaxTreeAnalysisContext context);
    void OnSyntaxNode(SyntaxNodeAnalysisContext context);
    void OnSymbol(SymbolAnalysisContext context);
    void OnSemanticModel(SemanticModelAnalysisContext context);
    void OnCompilation(CompilationAnalysisContext context);
    void OnCodeBlock(CodeBlockAnalysisContext context);
    void OnCompilationStart(CompilationStartAnalysisContext context);
    void OnCodeBlockStart(CodeBlockStartAnalysisContext<SyntaxKind> context);
}
