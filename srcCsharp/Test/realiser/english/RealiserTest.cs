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

using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleNLG.Main.features;
using SimpleNLG.Main.framework;
using SimpleNLG.Main.lexicon;
using SimpleNLG.Main.phrasespec;
using SimpleNLG.Main.realiser.english;

namespace SimpleNLG.Test.realiser.english
{
    using Feature = Feature;
    using Form = Form;
    using LexicalFeature = LexicalFeature;
    using Gender = Gender;
    using DocumentElement = DocumentElement;
    using NLGElement = NLGElement;
    using NLGFactory = NLGFactory;
    using LexicalCategory = LexicalCategory;
    using Lexicon = Lexicon;
    using NPPhraseSpec = NPPhraseSpec;
    using PPPhraseSpec = PPPhraseSpec;
    using SPhraseSpec = SPhraseSpec;
    using VPPhraseSpec = VPPhraseSpec;

    /**
     * JUnit test class for the {@link Realiser} class.
     * 
     * @author Saad Mahamood
     */
    [TestClass]
    public class RealiserTest
    {
        private Lexicon lexicon;
        private NLGFactory nlgFactory;
        private Realiser realiser = null;


        [TestInitialize]
        public virtual void setUp()
        {
            lexicon = Lexicon.DefaultLexicon;
            nlgFactory = new NLGFactory(lexicon);
            realiser = new Realiser(lexicon);
        }


        /**
         * Test the realization of List of NLGElements that is null
         */
        [TestMethod]
        public virtual void emptyNLGElementRealiserTest()
        {
            List<NLGElement> elements = new List<NLGElement>();
            IList<NLGElement> realisedElements = realiser.realise(elements);
            // Expect emtpy listed returned:
            Assert.IsNotNull(realisedElements);
            Assert.AreEqual(0, realisedElements.Count);
        }


        /**
         * Test the realization of List of NLGElements that is null
         */
        [TestMethod]
        public virtual void nullNLGElementRealiserTest()
        {
            List<NLGElement> elements = null;
            IList<NLGElement> realisedElements = realiser.realise(elements);
            // Expect emtpy listed returned:
            Assert.IsNotNull(realisedElements);
            Assert.AreEqual(0, realisedElements.Count);
        }

        /**
         * Tests the realization of multiple NLGElements in a list.
         */
        [TestMethod]
        public virtual void multipleNLGElementListRealiserTest()
        {
            List<NLGElement> elements = new List<NLGElement>();
            // Create test NLGElements to realize:

            // "The cat jumping on the counter."
            DocumentElement sentence1 = nlgFactory.createSentence();
            NPPhraseSpec subject_1 = nlgFactory.createNounPhrase("the", "cat");
            VPPhraseSpec verb_1 = nlgFactory.createVerbPhrase("jump");
            verb_1.setFeature(Feature.FORM, Form.PRESENT_PARTICIPLE);
            PPPhraseSpec prep_1 = nlgFactory.createPrepositionPhrase();
            NPPhraseSpec object_1 = nlgFactory.createNounPhrase();
            object_1.setDeterminer("the");
            object_1.setNoun("counter");
            prep_1.addComplement(object_1);
            prep_1.setPreposition("on");
            SPhraseSpec clause_1 = nlgFactory.createClause();
            clause_1.setSubject(subject_1);
            clause_1.VerbPhrase = verb_1;
            clause_1.setObject(prep_1);
            sentence1.addComponent(clause_1);

            // "The dog running on the counter."
            DocumentElement sentence2 = nlgFactory.createSentence();
            NPPhraseSpec subject_2 = nlgFactory.createNounPhrase("the", "dog");
            VPPhraseSpec verb_2 = nlgFactory.createVerbPhrase("run");
            verb_2.setFeature(Feature.FORM, Form.PRESENT_PARTICIPLE);
            PPPhraseSpec prep_2 = nlgFactory.createPrepositionPhrase();
            NPPhraseSpec object_2 = nlgFactory.createNounPhrase();
            object_2.setDeterminer("the");
            object_2.setNoun("counter");
            prep_2.addComplement(object_2);
            prep_2.setPreposition("on");
            SPhraseSpec clause_2 = nlgFactory.createClause();
            clause_2.setSubject(subject_2);
            clause_2.VerbPhrase = verb_2;
            clause_2.setObject(prep_2);
            sentence2.addComponent(clause_2);


            elements.Add(sentence1);
            elements.Add(sentence2);

            IList<NLGElement> realisedElements = realiser.realise(elements);

            Assert.IsNotNull(realisedElements);
            Assert.AreEqual(2, realisedElements.Count);
            Assert.AreEqual("The cat jumping on the counter.", realisedElements[0].Realisation);
            Assert.AreEqual("The dog running on the counter.", realisedElements[1].Realisation);
        }

        /**
         * Tests the correct pluralization with possessives (GitHub issue #9)
         */
        [TestMethod]
        public virtual void correctPluralizationWithPossessives()
        {
            NPPhraseSpec sisterNP = nlgFactory.createNounPhrase("sister");
            NLGElement word = nlgFactory.createInflectedWord("Albert Einstein",
                new LexicalCategory(LexicalCategory.LexicalCategoryEnum.NOUN));
            word.setFeature(LexicalFeature.PROPER, true);
            NPPhraseSpec possNP = nlgFactory.createNounPhrase(word);
            possNP.setFeature(Feature.POSSESSIVE, true);
            sisterNP.setSpecifier(possNP);
            Assert.AreEqual("Albert Einstein's sister", realiser.realise(sisterNP).Realisation);
            sisterNP.Plural = true;
            Assert.AreEqual("Albert Einstein's sisters", realiser.realise(sisterNP).Realisation);
            sisterNP.Plural = false;
            possNP.setFeature(LexicalFeature.GENDER, Gender.MASCULINE);
            possNP.setFeature(Feature.PRONOMINAL, true);
            Assert.AreEqual("his sister", realiser.realise(sisterNP).Realisation);
            sisterNP.Plural = true;
            Assert.AreEqual("his sisters", realiser.realise(sisterNP).Realisation);
        }
    }
}