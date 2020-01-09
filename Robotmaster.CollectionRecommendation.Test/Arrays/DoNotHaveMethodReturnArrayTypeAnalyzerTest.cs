using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Robotmaster.CollectionRecommendation.Arrays;
using Robotmaster.CollectionRecommendation.Test.Verifiers;

namespace Robotmaster.CollectionRecommendation.Test.Arrays
{
    [TestClass]
    public class DoNotHaveMethodReturnArrayTypeAnalyzerTest : DiagnosticVerifier
    {
        //No diagnostics expected to show up
        [TestMethod]
        public void TestEmptyInput()
        {
            var test = @"";

            VerifyCSharpDiagnostic(test);
        }

        //Diagnostic triggered and checked for
        [TestMethod]
        public void TestMatchingCase()
        {
            var test = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {
        class TypeName
        {
            object[] MethodName()
            {
            }
        }
    }";
            var expected = new DiagnosticResult
            {
                Id = DoNotHaveMethodReturnArrayTypeAnalyzer.DiagnosticId,
                Message = String.Format(DoNotHaveMethodReturnArrayTypeAnalyzer.MessageFormat, "MethodName"),
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[]
                    {
                        new DiagnosticResultLocation("Test0.cs", 13, 22)
                    }
            };

            VerifyCSharpDiagnostic(test, expected);
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new DoNotHaveMethodReturnArrayTypeAnalyzer();
        }
    }
}