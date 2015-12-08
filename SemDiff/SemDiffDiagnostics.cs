using Microsoft.CodeAnalysis;
using System.Collections.Immutable;

namespace SemDiffAnalyzer
{
    /// <summary>
    /// The Diagnostics class manages issues related to reporting diagnostics. This will include transforming data from our analysis into actual messages to the user and the enumeration of our supported diagnostics
    /// </summary>
    internal class SemDiffDiagnostics
    {
        //Shared Values
        public const string DiagnosticId = "SemDiff"; //Like the error code (just like missing semicolon is CS1002)

        private const string Category = "Conflicts"; //Not sure where this shows up yet

        private const string FalseNegativeDescription = "False-Negatives occur text based tools fail to detect a conflict that affects the semantics of the application. i.e., when the semantics of a dependant item (a called method, a base class, a variable used) has been changed in a way that changes the runtime of the application when the changes are merged.";

        private const string FalseNegativeMessageFormat = "False-Negatives for type '{0}' between '{1}' and '({2})[{3}]'";

        //False-Negative
        private const string FalseNegativeTitle = "Possible False-Negative condition detected";

        private const string FalsePositiveDescription = "False-Positives occur text based tools detect a conflict that affects the semantics of the application. i.e., when merge conflicts may occur, but there are no semantic differences between the conflicting changes.";

        private const string FalsePositiveMessageFormat = "False-Positive between '{0}' and '({1})[{2}]'";

        //False-Positive
        private const string FalsePositiveTitle = "Possible False-Positive condition detected"; //Not sure where this comes up yet

        //similar to fp
        private static DiagnosticDescriptor FalseNegative = new DiagnosticDescriptor(DiagnosticId, FalseNegativeTitle, FalseNegativeMessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: FalseNegativeDescription);

        //0 is name of file, 1 is title of pull request, 2 is url of pull request //Actual error message text (formatable string)
        //There is a option to show this in the error list for more info
        private static DiagnosticDescriptor FalsePositive = new DiagnosticDescriptor(DiagnosticId, FalsePositiveTitle, FalsePositiveMessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: FalsePositiveDescription);

        public ImmutableArray<DiagnosticDescriptor> GetSupportedDiagnostics()
        {
            return ImmutableArray.Create(FalsePositive, FalseNegative);
        }
    }
}