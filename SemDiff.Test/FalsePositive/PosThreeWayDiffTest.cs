using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SemDiff.Test.FalsePositive
{
    [TestClass]
    public class PosThreeWayDiffTest
    {
        protected static FalsePositiveAnalysis FP;

        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            FP = new FalsePositiveAnalysis();
        }

        [TestMethod]
        public void PosThreeWayDiffGetIntersectingSpansTest()
        {
            var a = "return 2*4;".WrapWithMethod().Parse();
            var l = "return 2 * 4;".WrapWithMethod().Parse();
            var r = "return 2+4;".WrapWithMethod().Parse();

            FP.ThreeWayDiff(a, l, r);
        }

        [TestMethod]
        public void PosThreeWayDiffThreewayOverlappingSpansTest()
        {
            var a = @"
                     var x = 2*4;
                     var y = 2*4;
                     var z = 2*4;
                     var w = 2*4;
                     return x*y/(z*w);
                     ".WrapWithMethod().Parse();
            var l = @"
                     return 1;
                     ".WrapWithMethod().Parse();
            var r = @"
                     var x = 8;
                     var y = 2*4;
                     var z = 2*4;
                     var w = 8;
                     return x*y/(z*w);
                     ".WrapWithMethod().Parse();

            FP.ThreeWayDiff(a, l, r);
        }

        [TestMethod]
        public void PosThreeWayDiffNonIntersectingSpansTest()
        {
            var a = @"
                     var x = 2*4;
                     return x;
                     ".WrapWithMethod().Parse();
            var l = @"
                     var x = 2*4;
                     return 8;
                     ".WrapWithMethod().Parse();
            var r = @"
                     var x = 8;
                     return x;
                     ".WrapWithMethod().Parse();

            FP.ThreeWayDiff(a, l, r);
        }
    }
}