/*
 * Ported to C# by Gert-Jan de Vries
 */

using NUnit.Framework;
using SimpleNLG.Main.format.english;

namespace SimpleNLG.Test.format.english
{
    [TestFixture]
    public class NumberedPrefixTest
    {
        [Test]
        public virtual void testNewInstancePrefixIsZero()
        {
            NumberedPrefix prefix = new NumberedPrefix();
            Assert.AreEqual("0", prefix.Prefix);
        }

        [Test]
        public virtual void testIncrementFromNewInstanceIsOne()
        {
            NumberedPrefix prefix = new NumberedPrefix();
            prefix.increment();
            Assert.AreEqual("1", prefix.Prefix);
        }

        [Test]
        public virtual void testIncrementForTwoPointTwoIsTwoPointThree()
        {
            NumberedPrefix prefix = new NumberedPrefix();
            prefix.Prefix = "2.2";
            prefix.increment();
            Assert.AreEqual("2.3", prefix.Prefix);
        }

        [Test]
        public virtual void testIncrementForThreePointFourPointThreeIsThreePointFourPointFour()
        {
            NumberedPrefix prefix = new NumberedPrefix();
            prefix.Prefix = "3.4.3";
            prefix.increment();
            Assert.AreEqual("3.4.4", prefix.Prefix);
        }

        [Test]
        public virtual void testUpALevelForNewInstanceIsOne()
        {
            NumberedPrefix prefix = new NumberedPrefix();
            prefix.upALevel();
            Assert.AreEqual("1", prefix.Prefix);
        }

        [Test]
        public virtual void testDownALevelForNewInstanceIsZero()
        {
            NumberedPrefix prefix = new NumberedPrefix();
            prefix.downALevel();
            Assert.AreEqual("0", prefix.Prefix);
        }

        [Test]
        public virtual void testDownALevelForSevenIsZero()
        {
            NumberedPrefix prefix = new NumberedPrefix();
            prefix.Prefix = "7";
            prefix.downALevel();
            Assert.AreEqual("0", prefix.Prefix);
        }

        [Test]
        public virtual void testDownALevelForTwoPointSevenIsTwo()
        {
            NumberedPrefix prefix = new NumberedPrefix();
            prefix.Prefix = "2.7";
            prefix.downALevel();
            Assert.AreEqual("2", prefix.Prefix);
        }

        [Test]
        public virtual void testDownALevelForThreePointFourPointThreeIsThreePointFour()
        {
            NumberedPrefix prefix = new NumberedPrefix();
            prefix.Prefix = "3.4.3";
            prefix.downALevel();
            Assert.AreEqual("3.4", prefix.Prefix);
        }
    }
}