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

namespace SimpleNLG.Test.syntax.english
{
    using Feature = Feature;
    using CoordinatedPhraseElement = CoordinatedPhraseElement;
    using LexicalCategory = LexicalCategory;
    using PhraseElement = PhraseElement;
    using StringElement = StringElement;

    /**
     * This class incorporates a few tests for adjectival phrases. Also tests for
     * adverbial phrase specs, which are very similar
     * 
     * @author agatt
     */
    [TestFixture]
    public class AdjectivePhraseTest : SimpleNLG4Test
    {
        /**
         * Instantiates a new adj p test.
         * 
         * @param name
         *            the name
         */
        public AdjectivePhraseTest() : this(null)
        {
        }

        public AdjectivePhraseTest(string name) : base(name)
        {
        }

        [SetUp]
        public override void setUp()
        {
            base.setUp();
        }

        /**
         * Test premodification & coordination of Adjective Phrases (Not much else
         * to simplenlg.test)
         */
        [Test]
        public virtual void testAdj()
        {
            // form the adjphrase "incredibly salacious"

            salacious.addPreModifier(phraseFactory.createAdverbPhrase("incredibly")); //$NON-NLS-1$
            Assert.AreEqual("incredibly salacious", realiser.realise(salacious).Realisation); //$NON-NLS-1$


            // form the adjphrase "incredibly beautiful"
            beautiful.addPreModifier("amazingly"); //$NON-NLS-1$
            Assert.AreEqual("amazingly beautiful", realiser.realise(beautiful).Realisation); //$NON-NLS-1$


            // coordinate the two aps
            CoordinatedPhraseElement coordap = new CoordinatedPhraseElement(salacious, beautiful);
            Assert.AreEqual("incredibly salacious and amazingly beautiful",
                realiser.realise(coordap).Realisation); //$NON-NLS-1$


            // changing the inner conjunction
            coordap.setFeature(Feature.CONJUNCTION, "or"); //$NON-NLS-1$
            Assert.AreEqual("incredibly salacious or amazingly beautiful",
                realiser.realise(coordap).Realisation); //$NON-NLS-1$


            // coordinate this with a new AdjPhraseSpec
            CoordinatedPhraseElement coord2 = new CoordinatedPhraseElement(coordap, stunning);
            Assert.AreEqual("incredibly salacious or amazingly beautiful and stunning",
                realiser.realise(coord2).Realisation); //$NON-NLS-1$


            // add a premodifier the coordinate phrase, yielding
            // "seriously and undeniably incredibly salacious or amazingly beautiful
            // and stunning"
            CoordinatedPhraseElement preMod =
                new CoordinatedPhraseElement(new StringElement("seriously"),
                    new StringElement("undeniably")); //$NON-NLS-1$//$NON-NLS-2$

            coord2.addPreModifier(preMod);
            Assert.AreEqual("seriously and undeniably incredibly salacious or amazingly beautiful and stunning",
                realiser.realise(coord2).Realisation); //$NON-NLS-1$


            // adding a coordinate rather than coordinating should give a different result
            coordap.addCoordinate(stunning);
            Assert.AreEqual("incredibly salacious, amazingly beautiful or stunning",
                realiser.realise(coordap).Realisation); //$NON-NLS-1$
        }

        /**
         * Simple test of adverbials
         */
        [Test]
        public virtual void testAdv()
        {
            PhraseElement sent = phraseFactory.createClause("John", "eat"); //$NON-NLS-1$ //$NON-NLS-2$

            PhraseElement adv = phraseFactory.createAdverbPhrase("quickly"); //$NON-NLS-1$

            sent.addPreModifier(adv);

            Assert.AreEqual("John quickly eats", realiser.realise(sent).Realisation); //$NON-NLS-1$

            adv.addPreModifier("very"); //$NON-NLS-1$

            Assert.AreEqual("John very quickly eats", realiser.realise(sent).Realisation); //$NON-NLS-1$
        }

        /**
         * Test participles as adjectives
         */
        [Test]
        public virtual void testParticipleAdj()
        {
            PhraseElement ap = phraseFactory.createAdjectivePhrase(lexicon.getWord("associated",
                new LexicalCategory(LexicalCategory.LexicalCategoryEnum.ADJECTIVE)));
            Assert.AreEqual("associated", realiser.realise(ap).Realisation);
        }

        /**
         * Test for multiple adjective modifiers with comma-separation. Example courtesy of William Bradshaw (Data2Text Ltd).
         */
        [Test]
        public virtual void testMultipleModifiers()
        {
            PhraseElement np = phraseFactory.createNounPhrase(lexicon.getWord("message",
                new LexicalCategory(LexicalCategory.LexicalCategoryEnum.NOUN)));
            np.addPreModifier(lexicon.getWord("active",
                new LexicalCategory(LexicalCategory.LexicalCategoryEnum.ADJECTIVE)));
            np.addPreModifier(lexicon.getWord("temperature",
                new LexicalCategory(LexicalCategory.LexicalCategoryEnum.ADJECTIVE)));
            Assert.AreEqual("active, temperature message", realiser.realise(np).Realisation);

            //now we set the realiser not to separate using commas
            realiser.CommaSepPremodifiers = false;
            Assert.AreEqual("active temperature message", realiser.realise(np).Realisation);
        }
    }
}