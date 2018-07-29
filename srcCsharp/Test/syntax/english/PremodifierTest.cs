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
 * Contributor(s): Ehud Reiter, Albert Gatt, Dave Westwater, Roman Kutlak, Margaret Mitchell, and Saad Mahamood.
 *
 * Ported to C# by Gert-Jan de Vries
 */

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleNLG.Main.features;
using SimpleNLG.Main.framework;
using SimpleNLG.Main.lexicon;
using SimpleNLG.Main.phrasespec;
using SimpleNLG.Main.realiser.english;

namespace SimpleNLG.Test.syntax.english
{
    using Feature = Feature;
    using Tense = Tense;
    using NLGFactory = NLGFactory;
    using Lexicon = Lexicon;
    using AdvPhraseSpec = AdvPhraseSpec;
    using NPPhraseSpec = NPPhraseSpec;
    using SPhraseSpec = SPhraseSpec;
    using VPPhraseSpec = VPPhraseSpec;
    using Realiser = Realiser;

    /**
     * {@link PremodifierTest} contains a series of JUnit test cases for Premodifiers.
     * 
     * @author Saad Mahamood
     */
    [TestClass]
    public class PremodifierTest
    {
        private Lexicon lexicon = null;
        private NLGFactory phraseFactory = null;
        private Realiser realiser = null;

        [TestInitialize]
        public virtual void setUp()
        {
            lexicon = Lexicon.DefaultLexicon;
            phraseFactory = new NLGFactory(lexicon);
            realiser = new Realiser(lexicon);
        }


        /**
         * Test change from "a" to "an" in the presence of a premodifier with a
         * vowel
         */
        [TestMethod]
        public virtual void indefiniteWithPremodifierTest()
        {
            SPhraseSpec s = phraseFactory.createClause("there", "be");
            s.setFeature(Feature.TENSE, Tense.PRESENT);
            NPPhraseSpec np = phraseFactory.createNounPhrase("a", "stenosis");
            s.setObject(np);

            // check without modifiers -- article should be "a"
            Assert.AreEqual("there is a stenosis", realiser.realise(s).Realisation);


            // add a single modifier -- should turn article to "an"
            np.addPreModifier(phraseFactory.createAdjectivePhrase("eccentric"));
            Assert.AreEqual("there is an eccentric stenosis", realiser.realise(s).Realisation);
        }

        /**
         * Test for comma separation between premodifers
         */
        [TestMethod]
        public virtual void multipleAdjPremodifiersTest()
        {
            NPPhraseSpec np = phraseFactory.createNounPhrase("a", "stenosis");
            np.addPreModifier(phraseFactory.createAdjectivePhrase("eccentric"));
            np.addPreModifier(phraseFactory.createAdjectivePhrase("discrete"));
            Assert.AreEqual("an eccentric, discrete stenosis", realiser.realise(np).Realisation);
        }

        /**
         * Test for comma separation between verb premodifiers
         */
        [TestMethod]
        public virtual void multipleAdvPremodifiersTest()
        {
            AdvPhraseSpec adv1 = phraseFactory.createAdverbPhrase("slowly");
            AdvPhraseSpec adv2 = phraseFactory.createAdverbPhrase("discretely");


            // case 1: concatenated premods: should have comma
            VPPhraseSpec vp = phraseFactory.createVerbPhrase("run");
            vp.addPreModifier(adv1);
            vp.addPreModifier(adv2);
            Assert.AreEqual("slowly, discretely runs", realiser.realise(vp).Realisation);


            // case 2: coordinated premods: no comma
            VPPhraseSpec vp2 = phraseFactory.createVerbPhrase("eat");
            vp2.addPreModifier(phraseFactory.createCoordinatedPhrase(adv1, adv2));
            Assert.AreEqual("slowly and discretely eats", realiser.realise(vp2).Realisation);
        }
    }
}