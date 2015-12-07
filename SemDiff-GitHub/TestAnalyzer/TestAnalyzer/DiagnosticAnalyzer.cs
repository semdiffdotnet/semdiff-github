using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.IO;

namespace TestAnalyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class TestAnalyzerAnalyzer : DiagnosticAnalyzer
    {
        private static readonly DiagnosticDescriptor testError = new DiagnosticDescriptor(id: "SA 1001", title: "", messageFormat: "", category: "Semantics", defaultSeverity: DiagnosticSeverity.Info, isEnabledByDefault: true);
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create(testError);

        public override void Initialize(AnalysisContext context)
        {
            //SyntaxTreeAction -> SemanticModelAction
            //ColdBlockStart -> Code Block
            //X Symbol -> XX Node
            context.RegisterCodeBlockStartAction<SyntaxKind>(cbsa =>
            {
                return; //Hit after SyntaxNode and SymbolAction
            });
            context.RegisterCompilationStartAction(csa => //First to be hit!
            {
                return;
            });
            context.RegisterCodeBlockAction(cpl =>
            {
                return; //Hit after SyntaxNode and SymbolAction
            });
            context.RegisterCompilationAction(ca =>
            {
                return; //Doesn't seem to ever get hit!
            });
            context.RegisterSemanticModelAction(sm => //Hit after RegisterSyntaxTree
            {
                return;
            });
            context.RegisterSymbolAction(sm => //Hit after SemanticModel
            {
                return;
            }, SymbolKind.Event, SymbolKind.Field, SymbolKind.Method, SymbolKind.NamedType, SymbolKind.Namespace, SymbolKind.Property);
            context.RegisterSyntaxNodeAction(sna => //hit after SymbolAction
            {
                return; //This is hit a lot
                #region Blah
            }, SyntaxKind.List, SyntaxKind.TildeToken, SyntaxKind.ExclamationToken, SyntaxKind.DollarToken, SyntaxKind.PercentToken, SyntaxKind.CaretToken, SyntaxKind.AmpersandToken, SyntaxKind.AsteriskToken, SyntaxKind.OpenParenToken, SyntaxKind.CloseParenToken, SyntaxKind.MinusToken, SyntaxKind.PlusToken, SyntaxKind.EqualsToken, SyntaxKind.OpenBraceToken, SyntaxKind.CloseBraceToken, SyntaxKind.OpenBracketToken, SyntaxKind.CloseBracketToken, SyntaxKind.BarToken, SyntaxKind.BackslashToken, SyntaxKind.ColonToken, SyntaxKind.SemicolonToken, SyntaxKind.DoubleQuoteToken, SyntaxKind.SingleQuoteToken, SyntaxKind.LessThanToken, SyntaxKind.CommaToken, SyntaxKind.GreaterThanToken, SyntaxKind.DotToken, SyntaxKind.QuestionToken, SyntaxKind.HashToken, SyntaxKind.SlashToken, SyntaxKind.SlashGreaterThanToken, SyntaxKind.LessThanSlashToken, SyntaxKind.XmlCommentStartToken, SyntaxKind.XmlCommentEndToken, SyntaxKind.XmlCDataStartToken, SyntaxKind.XmlCDataEndToken, SyntaxKind.XmlProcessingInstructionStartToken, SyntaxKind.XmlProcessingInstructionEndToken, SyntaxKind.BarBarToken, SyntaxKind.AmpersandAmpersandToken, SyntaxKind.MinusMinusToken, SyntaxKind.PlusPlusToken, SyntaxKind.ColonColonToken, SyntaxKind.QuestionQuestionToken, SyntaxKind.MinusGreaterThanToken, SyntaxKind.ExclamationEqualsToken, SyntaxKind.EqualsEqualsToken, SyntaxKind.EqualsGreaterThanToken, SyntaxKind.LessThanEqualsToken, SyntaxKind.LessThanLessThanToken, SyntaxKind.LessThanLessThanEqualsToken, SyntaxKind.GreaterThanEqualsToken, SyntaxKind.GreaterThanGreaterThanToken, SyntaxKind.GreaterThanGreaterThanEqualsToken, SyntaxKind.SlashEqualsToken, SyntaxKind.AsteriskEqualsToken, SyntaxKind.BarEqualsToken
, SyntaxKind.AmpersandEqualsToken, SyntaxKind.PlusEqualsToken, SyntaxKind.MinusEqualsToken, SyntaxKind.CaretEqualsToken, SyntaxKind.PercentEqualsToken, SyntaxKind.BoolKeyword, SyntaxKind.ByteKeyword, SyntaxKind.SByteKeyword, SyntaxKind.ShortKeyword, SyntaxKind.UShortKeyword, SyntaxKind.IntKeyword, SyntaxKind.UIntKeyword, SyntaxKind.LongKeyword, SyntaxKind.ULongKeyword, SyntaxKind.DoubleKeyword, SyntaxKind.FloatKeyword, SyntaxKind.DecimalKeyword, SyntaxKind.StringKeyword, SyntaxKind.CharKeyword, SyntaxKind.VoidKeyword, SyntaxKind.ObjectKeyword, SyntaxKind.TypeOfKeyword, SyntaxKind.SizeOfKeyword, SyntaxKind.NullKeyword, SyntaxKind.TrueKeyword, SyntaxKind.FalseKeyword, SyntaxKind.IfKeyword, SyntaxKind.ElseKeyword, SyntaxKind.WhileKeyword, SyntaxKind.ForKeyword, SyntaxKind.ForEachKeyword, SyntaxKind.DoKeyword, SyntaxKind.SwitchKeyword
, SyntaxKind.CaseKeyword, SyntaxKind.DefaultKeyword, SyntaxKind.TryKeyword, SyntaxKind.CatchKeyword, SyntaxKind.FinallyKeyword, SyntaxKind.LockKeyword, SyntaxKind.GotoKeyword, SyntaxKind.BreakKeyword, SyntaxKind.ContinueKeyword, SyntaxKind.ReturnKeyword, SyntaxKind.ThrowKeyword, SyntaxKind.PublicKeyword, SyntaxKind.PrivateKeyword, SyntaxKind.InternalKeyword, SyntaxKind.ProtectedKeyword, SyntaxKind.StaticKeyword, SyntaxKind.ReadOnlyKeyword, SyntaxKind.SealedKeyword, SyntaxKind.ConstKeyword, SyntaxKind.FixedKeyword, SyntaxKind.StackAllocKeyword, SyntaxKind.VolatileKeyword, SyntaxKind.NewKeyword, SyntaxKind.OverrideKeyword
, SyntaxKind.AbstractKeyword
, SyntaxKind.VirtualKeyword
, SyntaxKind.EventKeyword
, SyntaxKind.ExternKeyword
, SyntaxKind.RefKeyword
, SyntaxKind.OutKeyword
, SyntaxKind.InKeyword
, SyntaxKind.IsKeyword
, SyntaxKind.AsKeyword
, SyntaxKind.ParamsKeyword
, SyntaxKind.ArgListKeyword
, SyntaxKind.MakeRefKeyword
, SyntaxKind.RefTypeKeyword
, SyntaxKind.RefValueKeyword
, SyntaxKind.ThisKeyword
, SyntaxKind.BaseKeyword
, SyntaxKind.NamespaceKeyword
, SyntaxKind.UsingKeyword
, SyntaxKind.ClassKeyword
, SyntaxKind.StructKeyword
, SyntaxKind.InterfaceKeyword
, SyntaxKind.EnumKeyword
, SyntaxKind.DelegateKeyword
, SyntaxKind.CheckedKeyword
, SyntaxKind.UncheckedKeyword
, SyntaxKind.UnsafeKeyword
, SyntaxKind.OperatorKeyword
, SyntaxKind.ExplicitKeyword
, SyntaxKind.ImplicitKeyword
, SyntaxKind.YieldKeyword
, SyntaxKind.PartialKeyword
, SyntaxKind.AliasKeyword
, SyntaxKind.GlobalKeyword
, SyntaxKind.AssemblyKeyword
, SyntaxKind.ModuleKeyword
, SyntaxKind.TypeKeyword
, SyntaxKind.FieldKeyword
, SyntaxKind.MethodKeyword
, SyntaxKind.ParamKeyword
, SyntaxKind.PropertyKeyword
, SyntaxKind.TypeVarKeyword
, SyntaxKind.GetKeyword
, SyntaxKind.SetKeyword
, SyntaxKind.AddKeyword
, SyntaxKind.RemoveKeyword
, SyntaxKind.WhereKeyword
, SyntaxKind.FromKeyword
, SyntaxKind.GroupKeyword
, SyntaxKind.JoinKeyword
, SyntaxKind.IntoKeyword
, SyntaxKind.LetKeyword
, SyntaxKind.ByKeyword
, SyntaxKind.SelectKeyword
, SyntaxKind.OrderByKeyword
, SyntaxKind.OnKeyword
, SyntaxKind.EqualsKeyword
, SyntaxKind.AscendingKeyword
, SyntaxKind.DescendingKeyword
, SyntaxKind.NameOfKeyword
, SyntaxKind.AsyncKeyword
, SyntaxKind.AwaitKeyword
, SyntaxKind.WhenKeyword
, SyntaxKind.ElifKeyword
, SyntaxKind.EndIfKeyword
, SyntaxKind.RegionKeyword
, SyntaxKind.EndRegionKeyword
, SyntaxKind.DefineKeyword
, SyntaxKind.UndefKeyword
, SyntaxKind.WarningKeyword
, SyntaxKind.ErrorKeyword
, SyntaxKind.LineKeyword
, SyntaxKind.PragmaKeyword
, SyntaxKind.HiddenKeyword
, SyntaxKind.ChecksumKeyword
, SyntaxKind.DisableKeyword
, SyntaxKind.RestoreKeyword
, SyntaxKind.ReferenceKeyword
, SyntaxKind.InterpolatedStringStartToken
, SyntaxKind.InterpolatedStringEndToken
, SyntaxKind.InterpolatedVerbatimStringStartToken
, SyntaxKind.OmittedTypeArgumentToken
, SyntaxKind.OmittedArraySizeExpressionToken
, SyntaxKind.EndOfDirectiveToken
, SyntaxKind.EndOfDocumentationCommentToken
, SyntaxKind.EndOfFileToken
, SyntaxKind.BadToken
, SyntaxKind.IdentifierToken
, SyntaxKind.NumericLiteralToken
, SyntaxKind.CharacterLiteralToken
, SyntaxKind.StringLiteralToken
, SyntaxKind.XmlEntityLiteralToken
, SyntaxKind.XmlTextLiteralToken
, SyntaxKind.XmlTextLiteralNewLineToken
, SyntaxKind.InterpolatedStringToken
, SyntaxKind.InterpolatedStringTextToken
, SyntaxKind.EndOfLineTrivia
, SyntaxKind.WhitespaceTrivia
, SyntaxKind.SingleLineCommentTrivia
, SyntaxKind.MultiLineCommentTrivia
, SyntaxKind.DocumentationCommentExteriorTrivia
, SyntaxKind.SingleLineDocumentationCommentTrivia
, SyntaxKind.MultiLineDocumentationCommentTrivia
, SyntaxKind.DisabledTextTrivia
, SyntaxKind.PreprocessingMessageTrivia
, SyntaxKind.IfDirectiveTrivia
, SyntaxKind.ElifDirectiveTrivia
, SyntaxKind.ElseDirectiveTrivia
, SyntaxKind.EndIfDirectiveTrivia
, SyntaxKind.RegionDirectiveTrivia
, SyntaxKind.EndRegionDirectiveTrivia
, SyntaxKind.DefineDirectiveTrivia
, SyntaxKind.UndefDirectiveTrivia
, SyntaxKind.ErrorDirectiveTrivia
, SyntaxKind.WarningDirectiveTrivia
, SyntaxKind.LineDirectiveTrivia
, SyntaxKind.PragmaWarningDirectiveTrivia
, SyntaxKind.PragmaChecksumDirectiveTrivia
, SyntaxKind.ReferenceDirectiveTrivia
, SyntaxKind.BadDirectiveTrivia
, SyntaxKind.SkippedTokensTrivia
, SyntaxKind.XmlElement
, SyntaxKind.XmlElementStartTag
, SyntaxKind.XmlElementEndTag
, SyntaxKind.XmlEmptyElement
, SyntaxKind.XmlTextAttribute
, SyntaxKind.XmlCrefAttribute
, SyntaxKind.XmlNameAttribute
, SyntaxKind.XmlName
, SyntaxKind.XmlPrefix
, SyntaxKind.XmlText
, SyntaxKind.XmlCDataSection
, SyntaxKind.XmlComment
, SyntaxKind.XmlProcessingInstruction
, SyntaxKind.TypeCref
, SyntaxKind.QualifiedCref
, SyntaxKind.NameMemberCref
, SyntaxKind.IndexerMemberCref
, SyntaxKind.OperatorMemberCref
, SyntaxKind.ConversionOperatorMemberCref
, SyntaxKind.CrefParameterList
, SyntaxKind.CrefBracketedParameterList
, SyntaxKind.CrefParameter
, SyntaxKind.IdentifierName
, SyntaxKind.QualifiedName
, SyntaxKind.GenericName
, SyntaxKind.TypeArgumentList
, SyntaxKind.AliasQualifiedName
, SyntaxKind.PredefinedType
, SyntaxKind.ArrayType
, SyntaxKind.ArrayRankSpecifier
, SyntaxKind.PointerType
, SyntaxKind.NullableType
, SyntaxKind.OmittedTypeArgument
, SyntaxKind.ParenthesizedExpression
, SyntaxKind.ConditionalExpression
, SyntaxKind.InvocationExpression
, SyntaxKind.ElementAccessExpression
, SyntaxKind.ArgumentList
, SyntaxKind.BracketedArgumentList
, SyntaxKind.Argument
, SyntaxKind.NameColon
, SyntaxKind.CastExpression
, SyntaxKind.AnonymousMethodExpression
, SyntaxKind.SimpleLambdaExpression
, SyntaxKind.ParenthesizedLambdaExpression
, SyntaxKind.ObjectInitializerExpression
, SyntaxKind.CollectionInitializerExpression
, SyntaxKind.ArrayInitializerExpression
, SyntaxKind.AnonymousObjectMemberDeclarator
, SyntaxKind.ComplexElementInitializerExpression
, SyntaxKind.ObjectCreationExpression
, SyntaxKind.AnonymousObjectCreationExpression
, SyntaxKind.ArrayCreationExpression
, SyntaxKind.ImplicitArrayCreationExpression
, SyntaxKind.StackAllocArrayCreationExpression
, SyntaxKind.OmittedArraySizeExpression
, SyntaxKind.InterpolatedStringExpression
, SyntaxKind.ImplicitElementAccess
, SyntaxKind.AddExpression
, SyntaxKind.SubtractExpression
, SyntaxKind.MultiplyExpression
, SyntaxKind.DivideExpression
, SyntaxKind.ModuloExpression
, SyntaxKind.LeftShiftExpression
, SyntaxKind.RightShiftExpression
, SyntaxKind.LogicalOrExpression
, SyntaxKind.LogicalAndExpression
, SyntaxKind.BitwiseOrExpression
, SyntaxKind.BitwiseAndExpression
, SyntaxKind.ExclusiveOrExpression
, SyntaxKind.EqualsExpression
, SyntaxKind.NotEqualsExpression
, SyntaxKind.LessThanExpression
, SyntaxKind.LessThanOrEqualExpression
, SyntaxKind.GreaterThanExpression
, SyntaxKind.GreaterThanOrEqualExpression
, SyntaxKind.IsExpression
, SyntaxKind.AsExpression
, SyntaxKind.CoalesceExpression
, SyntaxKind.SimpleMemberAccessExpression
, SyntaxKind.PointerMemberAccessExpression
, SyntaxKind.ConditionalAccessExpression
, SyntaxKind.MemberBindingExpression
, SyntaxKind.ElementBindingExpression
, SyntaxKind.SimpleAssignmentExpression
, SyntaxKind.AddAssignmentExpression
, SyntaxKind.SubtractAssignmentExpression
, SyntaxKind.MultiplyAssignmentExpression
, SyntaxKind.DivideAssignmentExpression
, SyntaxKind.ModuloAssignmentExpression
, SyntaxKind.AndAssignmentExpression
, SyntaxKind.ExclusiveOrAssignmentExpression
, SyntaxKind.OrAssignmentExpression
, SyntaxKind.LeftShiftAssignmentExpression
, SyntaxKind.RightShiftAssignmentExpression
, SyntaxKind.UnaryPlusExpression
, SyntaxKind.UnaryMinusExpression
, SyntaxKind.BitwiseNotExpression
, SyntaxKind.LogicalNotExpression
, SyntaxKind.PreIncrementExpression
, SyntaxKind.PreDecrementExpression
, SyntaxKind.PointerIndirectionExpression
, SyntaxKind.AddressOfExpression
, SyntaxKind.PostIncrementExpression
, SyntaxKind.PostDecrementExpression
, SyntaxKind.AwaitExpression
, SyntaxKind.ThisExpression
, SyntaxKind.BaseExpression
, SyntaxKind.ArgListExpression
, SyntaxKind.NumericLiteralExpression
, SyntaxKind.StringLiteralExpression
, SyntaxKind.CharacterLiteralExpression
, SyntaxKind.TrueLiteralExpression
, SyntaxKind.FalseLiteralExpression
, SyntaxKind.NullLiteralExpression
, SyntaxKind.TypeOfExpression
, SyntaxKind.SizeOfExpression
, SyntaxKind.CheckedExpression
, SyntaxKind.UncheckedExpression
, SyntaxKind.DefaultExpression
, SyntaxKind.MakeRefExpression
, SyntaxKind.RefValueExpression
, SyntaxKind.RefTypeExpression
, SyntaxKind.QueryExpression
, SyntaxKind.QueryBody
, SyntaxKind.FromClause
, SyntaxKind.LetClause
, SyntaxKind.JoinClause
, SyntaxKind.JoinIntoClause
, SyntaxKind.WhereClause
, SyntaxKind.OrderByClause
, SyntaxKind.AscendingOrdering
, SyntaxKind.DescendingOrdering
, SyntaxKind.SelectClause
, SyntaxKind.GroupClause
, SyntaxKind.QueryContinuation
, SyntaxKind.Block
, SyntaxKind.LocalDeclarationStatement
, SyntaxKind.VariableDeclaration
, SyntaxKind.VariableDeclarator
, SyntaxKind.EqualsValueClause
, SyntaxKind.ExpressionStatement
, SyntaxKind.EmptyStatement
, SyntaxKind.LabeledStatement
, SyntaxKind.GotoStatement
, SyntaxKind.GotoCaseStatement
, SyntaxKind.GotoDefaultStatement
, SyntaxKind.BreakStatement
, SyntaxKind.ContinueStatement
, SyntaxKind.ReturnStatement
, SyntaxKind.YieldReturnStatement
, SyntaxKind.YieldBreakStatement
, SyntaxKind.ThrowStatement
, SyntaxKind.WhileStatement
, SyntaxKind.DoStatement
, SyntaxKind.ForStatement
, SyntaxKind.ForEachStatement
, SyntaxKind.UsingStatement
, SyntaxKind.FixedStatement
, SyntaxKind.CheckedStatement
, SyntaxKind.UncheckedStatement
, SyntaxKind.UnsafeStatement
, SyntaxKind.LockStatement
, SyntaxKind.IfStatement
, SyntaxKind.ElseClause
, SyntaxKind.SwitchStatement
, SyntaxKind.SwitchSection
, SyntaxKind.CaseSwitchLabel
, SyntaxKind.DefaultSwitchLabel
, SyntaxKind.TryStatement
, SyntaxKind.CatchClause
, SyntaxKind.CatchDeclaration
, SyntaxKind.CatchFilterClause
, SyntaxKind.FinallyClause
, SyntaxKind.CompilationUnit
, SyntaxKind.GlobalStatement
, SyntaxKind.NamespaceDeclaration
, SyntaxKind.UsingDirective
, SyntaxKind.ExternAliasDirective
, SyntaxKind.AttributeList
, SyntaxKind.AttributeTargetSpecifier
, SyntaxKind.Attribute
, SyntaxKind.AttributeArgumentList
, SyntaxKind.AttributeArgument
, SyntaxKind.NameEquals
, SyntaxKind.ClassDeclaration
, SyntaxKind.StructDeclaration
, SyntaxKind.InterfaceDeclaration
, SyntaxKind.EnumDeclaration
, SyntaxKind.DelegateDeclaration
, SyntaxKind.BaseList
, SyntaxKind.SimpleBaseType
, SyntaxKind.TypeParameterConstraintClause
, SyntaxKind.ConstructorConstraint
, SyntaxKind.ClassConstraint
, SyntaxKind.StructConstraint
, SyntaxKind.TypeConstraint
, SyntaxKind.ExplicitInterfaceSpecifier
, SyntaxKind.EnumMemberDeclaration
, SyntaxKind.FieldDeclaration
, SyntaxKind.EventFieldDeclaration
, SyntaxKind.MethodDeclaration
, SyntaxKind.OperatorDeclaration
, SyntaxKind.ConversionOperatorDeclaration
, SyntaxKind.ConstructorDeclaration
, SyntaxKind.BaseConstructorInitializer
, SyntaxKind.ThisConstructorInitializer
, SyntaxKind.DestructorDeclaration
, SyntaxKind.PropertyDeclaration
, SyntaxKind.EventDeclaration
, SyntaxKind.IndexerDeclaration
, SyntaxKind.AccessorList
, SyntaxKind.GetAccessorDeclaration
, SyntaxKind.SetAccessorDeclaration
, SyntaxKind.AddAccessorDeclaration
, SyntaxKind.RemoveAccessorDeclaration
, SyntaxKind.UnknownAccessorDeclaration
, SyntaxKind.ParameterList
, SyntaxKind.BracketedParameterList
, SyntaxKind.Parameter
, SyntaxKind.TypeParameterList
, SyntaxKind.TypeParameter
, SyntaxKind.IncompleteMember
, SyntaxKind.ArrowExpressionClause
, SyntaxKind.Interpolation
, SyntaxKind.InterpolatedStringText
, SyntaxKind.InterpolationAlignmentClause
, SyntaxKind.InterpolationFormatClause);
            #endregion
            context.RegisterSyntaxTreeAction(sta => //Second to be hit
            {
                return;
            });
            //our chance to spin up some stuff
        }
    }
}
