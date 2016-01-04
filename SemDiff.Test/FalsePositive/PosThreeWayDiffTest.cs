using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;

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

            var cons = FP.ThreeWayDiff(a, l, r);
        }

        [TestMethod]
        public void PosThreeWayDiffSameSizeIntersectingSpansTest()
        {
            var a = "return abcd;".WrapWithMethod().Parse();
            var l = "return efgh;".WrapWithMethod().Parse();
            var r = "return ijkl;".WrapWithMethod().Parse();

            var cons = FP.ThreeWayDiff(a, l, r);
            var con_strs = cons.Select(c => c.ToString()).ToList();
            Assert.AreEqual("<<<<<<<efgh|||||||abcd=======ijkl>>>>>>>", con_strs.Single());
        }

        [TestMethod]
        public void PosThreeWayDiffLeftLargeIntersectingSpansTest()
        {
            var a = "return abcd;".WrapWithMethod().Parse();
            var l = "return efghijkl;".WrapWithMethod().Parse();
            var r = "return mnop;".WrapWithMethod().Parse();

            var cons = FP.ThreeWayDiff(a, l, r);
            var con_strs = cons.Select(c => c.ToString()).ToList();
            Assert.AreEqual("<<<<<<<efghijkl|||||||abcd=======mnop>>>>>>>", con_strs.Single());
        }

        [TestMethod]
        public void PosThreeWayDiffLeftDeleteIntersectingSpansTest()
        {
            var a = "return abcd;".WrapWithMethod().Parse();
            var l = "return ;".WrapWithMethod().Parse();
            var r = "return mnop;".WrapWithMethod().Parse();

            var cons = FP.ThreeWayDiff(a, l, r);
            var con_strs = cons.Select(c => c.ToString()).ToList();
            Assert.AreEqual("<<<<<<<|||||||abcd=======mnop>>>>>>>", con_strs.Single());
        }
        [TestMethod]
        public void PosThreeWayDiffCloseCloseNonIntersectingChangesSpansTest()
        {
            var a = "return abcd;".WrapWithMethod().Parse();
            var l = "return aabcd;".WrapWithMethod().Parse();
            var r = "return mnop;".WrapWithMethod().Parse();

            var cons = FP.ThreeWayDiff(a, l, r);
            var con_strs = cons.Select(c => c.ToString()).ToList();
            Assert.AreEqual("<<<<<<<aabcd|||||||abcd=======mnop>>>>>>>", con_strs.Single());
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

            var cons = FP.ThreeWayDiff(a, l, r);
            var con_strs = cons.Select(c => c.ToString()).ToList();
        }

        [TestMethod]
        public void PosThreeWayDiffCommentAddedTest()
        {
            var a = @"
                     var x = 2*4;
                     return x;
                     ".WrapWithMethod().Parse();
            var l = @"
                     var x = 2*4; //Added a Comment
                     return x;
                     ".WrapWithMethod().Parse();
            var r = @"
                     var x = 2*4; //Comment Added
                     return x;
                     ".WrapWithMethod().Parse();

            var cons = FP.ThreeWayDiff(a, l, r);
            var con_strs = cons.Select(c => c.ToString()).ToList();
        }

        [TestMethod]
        public void PosThreeWayDiffCommentConflictTest()
        {
            var a = @"
                     var x = 2*4; //Comment Removed
                     return x;
                     ".WrapWithMethod().Parse();
            var l = @"
                     var x = 2*4; //Comment Left
                     return x;
                     ".WrapWithMethod().Parse();
            var r = @"
                     var x = 2*4; //Comment Right
                     return x;
                     ".WrapWithMethod().Parse();

            var cons = FP.ThreeWayDiff(a, l, r);
            var c = cons.SingleOrDefault();
            var con_strs = cons.Select(c => c.ToString()).ToList();
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

        [TestMethod]
        public void PosThreeWayDiffCurlyBrocTest()
        {
            var a = File.ReadAllText("AnalyseUser.orig.cs.txt").Parse();
            var l = File.ReadAllText("AnalyseUser.cs.left.txt").Parse();
            var r = File.ReadAllText("AnalyseUser.right.cs.txt").Parse();

            var cons = FP.ThreeWayDiff(a, l, r);
            var con_strs = cons.Select(c => c.ToString()).ToList();
            Assert.IsTrue(con_strs.Any());
        }
    }
}