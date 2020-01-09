namespace Robotmaster.CollectionRecommendation.Arrays
{
    using System.Collections.Immutable;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Diagnostics;
    using Robotmaster.CollectionRecommendation.Helpers;

    /// <summary>
    ///     This is used to analyze and detect for situations where fields are arrays instead of <see cref="IReadOnlyList{T}"/>.
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class DoNotHaveFieldOfArrayTypeAnalyzer : DiagnosticAnalyzer
    {
        /// <summary>
        ///     This is the complete diagnostic ID for this analyzer.
        /// </summary>
        public static readonly string DiagnosticId = AnalyzerHelper.GetCompleteAnalyzerId(IdNumber);

        /// <summary>
        ///     The message format to use for diagnostics generated by this analyzer.
        /// </summary>
        internal const string MessageFormat = "The {0} field has a type of an Array instead of an IReadOnlyList.";

        /// <summary>
        ///     The description to use for diagnostics generated by this analyzer.
        /// </summary>
        private const string Description = "This is when a field has a type of an Array instead of an IReadOnlyList.";

        /// <summary>
        ///     The category to use for diagnostics generated by this analyzer.
        /// </summary>
        private const string Category = "Arrays";

        /// <summary>
        ///     The number portion of the analyzer's <see cref="DiagnosticId"/>.
        /// </summary>
        private const int IdNumber = 2;

#pragma warning disable RS1017 // DiagnosticId for analyzers must be a non-null constant.
        internal static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, AnalyzerHelper.AnalyzerTitle, MessageFormat, Category, DiagnosticSeverity.Warning, true, Description);
#pragma warning restore RS1017 // DiagnosticId for analyzers must be a non-null constant.

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
            context.EnableConcurrentExecution();
            context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.Field);
        }

        private static void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            // Cast down to a IFieldSymbol.
            IFieldSymbol fieldSymbol = (IFieldSymbol)context.Symbol;

            // If the type of the field symbol is an array.
            if (fieldSymbol.Type is IArrayTypeSymbol arrayTypeSymbol)
            {
                // For every location where the field is defined.
                foreach (var location in fieldSymbol.Locations)
                {
                    // Report a diagnostic that IReadOnlyList should be the type instead.
                    context.ReportDiagnostic(Diagnostic.Create(Rule, location, fieldSymbol.Name));
                }
            }
        }
    }
}
