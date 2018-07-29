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

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleNLG.Main.features;
using SimpleNLG.Main.framework;
using SimpleNLG.Main.phrasespec;

namespace SimpleNLG.Test.syntax.english
{
    using Feature = Feature;
    using Tense = Tense;
    using CoordinatedPhraseElement = CoordinatedPhraseElement;
    using LexicalCategory = LexicalCategory;
    using NPPhraseSpec = NPPhraseSpec;
    using PPPhraseSpec = PPPhraseSpec;
    using SPhraseSpec = SPhraseSpec;
    using VPPhraseSpec = VPPhraseSpec;

    /**
     * Some tests for coordination, especially of coordinated VPs with modifiers.
     * 
     * @author Albert Gatt
     * 
     */
    [TestClass]
    public class CoordinationTest : SimpleNLG4Test
    {
        public CoordinationTest() : this(null)
        {
        }

        public CoordinationTest(string name) : base(name)
        {
        }


        /**
         * Check that empty coordinate phrases are not realised as "null"
         */
        [TestMethod]
        public virtual void emptyCoordinationTest()
        {
            // first a simple phrase with no coordinates
            CoordinatedPhraseElement coord = phraseFactory.createCoordinatedPhrase();
            Assert.AreEqual("", realiser.realise(coord).Realisation);

            // now one with a premodifier and nothing else
            coord.addPreModifier(phraseFactory.createAdjectivePhrase("nice"));
            Assert.AreEqual("nice", realiser.realise(coord).Realisation);
        }

        /**
         * Test pre and post-modification of coordinate VPs inside a sentence.
         */
        [TestMethod]
        public virtual void testModifiedCoordVP()
        {
            CoordinatedPhraseElement coord = phraseFactory.createCoordinatedPhrase(getUp, fallDown);
            coord.setFeature(Feature.TENSE, Tense.PAST);
            Assert.AreEqual("got up and fell down", realiser.realise(coord).Realisation);


            // add a premodifier
            coord.addPreModifier("slowly");
            Assert.AreEqual("slowly got up and fell down", realiser.realise(coord).Realisation);


            // adda postmodifier
            coord.addPostModifier(behindTheCurtain);
            Assert.AreEqual("slowly got up and fell down behind the curtain", realiser.realise(coord).Realisation);


            // put within the context of a sentence
            SPhraseSpec s = phraseFactory.createClause("Jake", coord);
            s.setFeature(Feature.TENSE, Tense.PAST);
            Assert.AreEqual("Jake slowly got up and fell down behind the curtain", realiser.realise(s).Realisation);


            // add premod to the sentence
            s.addPreModifier(
                lexicon.getWord("however", new LexicalCategory(LexicalCategory.LexicalCategoryEnum.ADVERB)));
            Assert.AreEqual("Jake however slowly got up and fell down behind the curtain",
                realiser.realise(s).Realisation);


            // add postmod to the sentence
            s.addPostModifier(inTheRoom);
            Assert.AreEqual("Jake however slowly got up and fell down behind the curtain in the room",
                realiser.realise(s).Realisation);
        }

        /**
         * Test due to Chris Howell -- create a complex sentence with front modifier
         * and coordinateVP. This is a version in which we create the coordinate
         * phrase directly.
         */
        [TestMethod]
        public virtual void testCoordinateVPComplexSubject()
        {
            // "As a result of the procedure the patient had an adverse contrast media reaction and went into cardiogenic shock."
            SPhraseSpec s = phraseFactory.createClause();

            s.setSubject(phraseFactory.createNounPhrase("the", "patient"));

            // first VP
            VPPhraseSpec vp1 = phraseFactory.createVerbPhrase(lexicon.getWord("have",
                new LexicalCategory(LexicalCategory.LexicalCategoryEnum.VERB)));
            NPPhraseSpec np1 = phraseFactory.createNounPhrase("a",
                lexicon.getWord("contrast media reaction",
                    new LexicalCategory(LexicalCategory.LexicalCategoryEnum.NOUN)));
            np1.addPreModifier(lexicon.getWord("adverse",
                new LexicalCategory(LexicalCategory.LexicalCategoryEnum.ADJECTIVE)));
            vp1.addComplement(np1);

            // second VP
            VPPhraseSpec vp2 = phraseFactory.createVerbPhrase(lexicon.getWord("go",
                new LexicalCategory(LexicalCategory.LexicalCategoryEnum.VERB)));
            PPPhraseSpec pp = phraseFactory.createPrepositionPhrase("into",
                lexicon.getWord("cardiogenic shock", new LexicalCategory(LexicalCategory.LexicalCategoryEnum.NOUN)));
            vp2.addComplement(pp);

            // coordinate
            CoordinatedPhraseElement coord = phraseFactory.createCoordinatedPhrase(vp1, vp2);
            coord.setFeature(Feature.TENSE, Tense.PAST);
            Assert.AreEqual("had an adverse contrast media reaction and went into cardiogenic shock",
                realiser.realise(coord).Realisation);


            // now put this in the sentence
            s.VerbPhrase = coord;
            s.addFrontModifier("As a result of the procedure");
            Assert.AreEqual(
                "As a result of the procedure the patient had an adverse contrast media reaction and went into cardiogenic shock",
                realiser.realise(s).Realisation);
        }

        /**
         * Test setting a conjunction to null
         */
        [TestMethod]
        public virtual void testNullConjunction()
        {
            SPhraseSpec p = phraseFactory.createClause("I", "be", "happy");
            SPhraseSpec q = phraseFactory.createClause("I", "eat", "fish");
            CoordinatedPhraseElement pq = phraseFactory.createCoordinatedPhrase();
            pq.addCoordinate(p);
            pq.addCoordinate(q);
            pq.setFeature(Feature.CONJUNCTION, "");

            // should come out without conjunction
            Assert.AreEqual("I am happy I eat fish", realiser.realise(pq).Realisation);


            // should come out without conjunction
            pq.setFeature(Feature.CONJUNCTION, null);
            Assert.AreEqual("I am happy I eat fish", realiser.realise(pq).Realisation);
        }

        /**
         * Check that the negation feature on a child of a coordinate phrase remains
         * as set, unless explicitly set otherwise at the parent level.
         */
        [TestMethod]
        public virtual void testNegationFeature()
        {
            SPhraseSpec s1 = phraseFactory.createClause("he", "have", "asthma");
            SPhraseSpec s2 = phraseFactory.createClause("he", "have", "diabetes");
            s1.setFeature(Feature.NEGATED, true);
            CoordinatedPhraseElement coord = phraseFactory.createCoordinatedPhrase(s1, s2);
            string realisation = realiser.realise(coord).Realisation;
            Console.WriteLine(realisation);
            Assert.AreEqual("he does not have asthma and he has diabetes", realisation);
        }
    }
}