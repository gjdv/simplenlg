/*
 * The contents of this file are subject to the Mozilla Public License
 * Version 1.1 (the "License"); you may not use this file except in
 * compliance with the License. You may obtain a copy of the License at
 * http://www.mozilla.org/MPL/
 *
 * Software distributed under the License is distributed on an "AS IS"
 * basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
 * License for the specific language governing rights and limitations
 * under the License.
 *
 * The Original Code is "Simplenlg".
 *
 * The Initial Developer of the Original Code is Ehud Reiter, Albert Gatt and Dave Westwater.
 * Portions created by Ehud Reiter, Albert Gatt and Dave Westwater are Copyright (C) 2010-11 The University of Aberdeen. All Rights Reserved.
 *
 * Contributor(s): Ehud Reiter, Albert Gatt, Dave Wewstwater, Roman Kutlak, Margaret Mitchell, Saad Mahamood.
 *
 * Ported to C# by Gert-Jan de Vries
 */

using NUnit.Framework;
using SimpleNLG.Main.features;
using SimpleNLG.Main.framework;
using Assert = NUnit.Framework.Assert;

namespace SimpleNLG.Test.syntax.english
{
    using Feature = Feature;
    using CoordinatedPhraseElement = CoordinatedPhraseElement;

    // TODO: Auto-generated Javadoc
    /**
     * This class groups together some tests for prepositional phrases and
     * coordinate prepositional phrases.
     * @author agatt
     */
    [TestFixture]
    public class PrepositionalPhraseTest : SimpleNLG4Test
    {
        public PrepositionalPhraseTest() : this(null)
        {
        }

        /**
         * Instantiates a new pP test.
         * 
         * @param name
         *            the name
         */
        public PrepositionalPhraseTest(string name) : base(name)
        {
        }

        [OneTimeTearDown]
        public override void tearDown()
        {
            base.tearDown();
        }

        /**
         * Basic test for the pre-set PP fixtures.
         */
        [Test]
        public virtual void testBasic()
        {
            Assert.AreEqual("in the room", realiser.realise(inTheRoom).Realisation); //$NON-NLS-1$
            Assert.AreEqual("behind the curtain", realiser.realise(behindTheCurtain).Realisation); //$NON-NLS-1$
            Assert.AreEqual("on the rock", realiser.realise(onTheRock).Realisation); //$NON-NLS-1$
        }

        /**
         * Test for coordinate NP complements of PPs.
         */
        [Test]
        public virtual void testComplementation()
        {
            inTheRoom.clearComplements();
            inTheRoom.addComplement(new CoordinatedPhraseElement(phraseFactory.createNounPhrase("the", "room"),
                phraseFactory.createNounPhrase("a", "car"))); //$NON-NLS-1$//$NON-NLS-2$ - $NON-NLS-1$ //$NON-NLS-2$
            Assert.AreEqual("in the room and a car", realiser.realise(inTheRoom).Realisation); //$NON-NLS-1$
        }

        /**
         * Test for PP coordination.
         */
        [Test]
        public virtual void testCoordination()
        {
            // simple coordination

            CoordinatedPhraseElement coord1 = new CoordinatedPhraseElement(inTheRoom, behindTheCurtain);
            Assert.AreEqual("in the room and behind the curtain", realiser.realise(coord1).Realisation); //$NON-NLS-1$


            // change the conjunction
            coord1.setFeature(Feature.CONJUNCTION, "or"); //$NON-NLS-1$
            Assert.AreEqual("in the room or behind the curtain", realiser.realise(coord1).Realisation); //$NON-NLS-1$


            // new coordinate
            CoordinatedPhraseElement coord2 = new CoordinatedPhraseElement(onTheRock, underTheTable);
            coord2.setFeature(Feature.CONJUNCTION, "or"); //$NON-NLS-1$
            Assert.AreEqual("on the rock or under the table", realiser.realise(coord2).Realisation); //$NON-NLS-1$


            // coordinate two coordinates
            CoordinatedPhraseElement coord3 = new CoordinatedPhraseElement(coord1, coord2);

            string text = realiser.realise(coord3).Realisation;
            Assert.AreEqual("in the room or behind the curtain and on the rock or under the table", text); //$NON-NLS-1$
        }
    }
}