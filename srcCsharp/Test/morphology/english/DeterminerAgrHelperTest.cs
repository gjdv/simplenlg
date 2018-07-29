/*
 * Ported to C# by Gert-Jan de Vries
 */
 
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleNLG.Main.morphology.english;

namespace SimpleNLG.Test.morphology.english
{
    [TestClass]
    public class DeterminerAgrHelperTest
    {
        [TestMethod]
        public virtual void testRequiresAn()
        {
            Assert.IsTrue(DeterminerAgrHelper.requiresAn("elephant"));

            Assert.IsFalse(DeterminerAgrHelper.requiresAn("cow"));

            // Does not hand phonetics
            Assert.IsFalse(DeterminerAgrHelper.requiresAn("hour"));

            // But does have exceptions for some numerals
            Assert.IsFalse(DeterminerAgrHelper.requiresAn("one"));

            Assert.IsFalse(DeterminerAgrHelper.requiresAn("100"));
        }

        [TestMethod]
        public virtual void testCheckEndsWithIndefiniteArticle1()
        {
            string cannedText = "I see a";

            string np = "elephant";

            string expected = "I see an";

            string actual = DeterminerAgrHelper.checkEndsWithIndefiniteArticle(cannedText, np);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public virtual void testCheckEndsWithIndefiniteArticle2()
        {
            string cannedText = "I see a";

            string np = "cow";

            string expected = "I see a";

            string actual = DeterminerAgrHelper.checkEndsWithIndefiniteArticle(cannedText, np);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public virtual void testCheckEndsWithIndefiniteArticle3()
        {
            string cannedText = "I see an";

            string np = "cow";

            // Does not handle "an" -> "a"
            string expected = "I see an";

            string actual = DeterminerAgrHelper.checkEndsWithIndefiniteArticle(cannedText, np);

            Assert.AreEqual(expected, actual);
        }
    }
}