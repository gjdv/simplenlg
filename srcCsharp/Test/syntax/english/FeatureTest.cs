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
 * Contributor(s): Ehud Reiter, Albert Gatt, Dave Westwater, Roman Kutlak, Margaret Mitchell, Saad Mahamood.
 *
 * Ported to C# by Gert-Jan de Vries
 */

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleNLG.Main.features;
using SimpleNLG.Main.framework;
using SimpleNLG.Main.phrasespec;

namespace SimpleNLG.Test.syntax.english
{
    using Feature = Feature;
    using Tense = Tense;
    using CoordinatedPhraseElement = CoordinatedPhraseElement;
    using DocumentElement = DocumentElement;
    using LexicalCategory = LexicalCategory;
    using NLGElement = NLGElement;
    using NLGFactory = NLGFactory;
    using PhraseElement = PhraseElement;
    using AdvPhraseSpec = AdvPhraseSpec;
    using NPPhraseSpec = NPPhraseSpec;
    using SPhraseSpec = SPhraseSpec;
    using VPPhraseSpec = VPPhraseSpec;

    /**
     * Tests that check that realization of different Features against NLGElements.
     * 
     * @author François Portet
     */
    [TestClass]
    public class FeatureTest : SimpleNLG4Test
    {
        private bool InstanceFieldsInitialized = false;

        private void InitializeInstanceFields()
        {
            docFactory = new NLGFactory(lexicon);
        }


        internal NLGFactory docFactory;

        public FeatureTest() : this(null)
        {
        }

        /**
         * Instantiates a new text spec test.
         * 
         * @param name
         *            the name
         */
        public FeatureTest(string name) : base(name)
        {
            if (!InstanceFieldsInitialized)
            {
                InitializeInstanceFields();
                InstanceFieldsInitialized = true;
            }
        }

        [TestCleanup]
        public override void tearDown()
        {
            base.tearDown();
            docFactory = null;
        }

        /**
         * Tests use of the Possessive Feature.
         */
        [TestMethod]
        public virtual void testPossessiveFeature_PastTense()
        {
            phraseFactory.Lexicon = lexicon;
            realiser.Lexicon = lexicon;

            // Create the pronoun 'she'
            NLGElement she =
                phraseFactory.createWord("she", new LexicalCategory(LexicalCategory.LexicalCategoryEnum.PRONOUN));

            // Set possessive on the pronoun to make it 'her'
            she.setFeature(Feature.POSSESSIVE, true);

            // Create a noun phrase with the subject lover and the determiner as she
            PhraseElement herLover = phraseFactory.createNounPhrase(she, "lover");

            // Create a clause to say 'he be her lover'
            PhraseElement clause = phraseFactory.createClause("he", "be", herLover);

            // Add the cue phrase need the comma as orthography
            // currently doesn't handle this.
            // This could be expanded to be a noun phrase with determiner
            // 'two' and noun 'week', set to plural and with a premodifier of
            // 'after'
            clause.setFeature(Feature.CUE_PHRASE, "after two weeks,");

            // Add the 'for a fortnight' as a post modifier. Alternatively
            // this could be added as a prepositional phrase 'for' with a
            // complement of a noun phrase ('a' 'fortnight')
            clause.addPostModifier("for a fortnight");

            // Set 'be' to 'was' as past tense
            clause.setFeature(Feature.TENSE, Tense.PAST);

            // Add the clause to a sentence.
            DocumentElement sentence1 = docFactory.createSentence(clause);

            // Realise the sentence
            NLGElement realised = realiser.realise(sentence1);

            Assert.AreEqual("After two weeks, he was her lover for a fortnight.", realised.Realisation);
        }

        /**
         * Basic tests.
         */
        [TestMethod]
        public virtual void testTwoPossessiveFeature_PastTense()
        {
            phraseFactory.Lexicon = lexicon;

            // Create the pronoun 'she'
            NLGElement she =
                phraseFactory.createWord("she", new LexicalCategory(LexicalCategory.LexicalCategoryEnum.PRONOUN));

            // Set possessive on the pronoun to make it 'her'
            she.setFeature(Feature.POSSESSIVE, true);

            // Create a noun phrase with the subject lover and the determiner as she
            PhraseElement herLover = phraseFactory.createNounPhrase(she, "lover");
            herLover.Plural = true;

            // Create the pronoun 'he'
            NLGElement he =
                phraseFactory.createNounPhrase(new LexicalCategory(LexicalCategory.LexicalCategoryEnum.PRONOUN), "he");
            he.Plural = true;

            // Create a clause to say 'they be her lovers'
            PhraseElement clause = phraseFactory.createClause(he, "be", herLover);
            clause.setFeature(Feature.POSSESSIVE, true);

            // Add the cue phrase need the comma as orthography
            // currently doesn't handle this.
            // This could be expanded to be a noun phrase with determiner
            // 'two' and noun 'week', set to plural and with a premodifier of
            // 'after'
            clause.setFeature(Feature.CUE_PHRASE, "after two weeks,");

            // Add the 'for a fortnight' as a post modifier. Alternatively
            // this could be added as a prepositional phrase 'for' with a
            // complement of a noun phrase ('a' 'fortnight')
            clause.addPostModifier("for a fortnight");

            // Set 'be' to 'was' as past tense
            clause.setFeature(Feature.TENSE, Tense.PAST);

            // Add the clause to a sentence.
            DocumentElement sentence1 = docFactory.createSentence(clause);

            // Realise the sentence
            NLGElement realised = realiser.realise(sentence1);

            Assert.AreEqual("After two weeks, they were her lovers for a fortnight.",
                realised.Realisation); //$NON-NLS-1$
        }

        /**
         * Test use of the Complementiser feature by combining two S's using cue phrase and gerund.
         */
        [TestMethod]
        public virtual void testComplementiserFeature_PastTense()
        {
            phraseFactory.Lexicon = lexicon;

            PhraseElement born = phraseFactory.createClause("Dave Bus", "be", "born");
            born.setFeature(Feature.TENSE, Tense.PAST);
            born.addPostModifier("in");
            born.setFeature(Feature.COMPLEMENTISER, "which");

            PhraseElement theHouse = phraseFactory.createNounPhrase("the", "house");
            theHouse.addComplement(born);

            PhraseElement clause = phraseFactory.createClause(theHouse, "be",
                phraseFactory.createPrepositionPhrase("in", "Edinburgh"));
            DocumentElement sentence = docFactory.createSentence(clause);
            NLGElement realised = realiser.realise(sentence);

            // Retrieve the realisation and dump it to the console
            Assert.AreEqual("The house which Dave Bus was born in is in Edinburgh.", realised.Realisation);
        }

        /**
         * Test use of the Complementiser feature in a {@link CoordinatedPhraseElement} by combine two S's using cue phrase and gerund.
         */
        [TestMethod]
        public virtual void testComplementiserFeatureInACoordinatePhrase_PastTense()
        {
            phraseFactory.Lexicon = lexicon;

            NLGElement dave = phraseFactory.createWord("Dave Bus",
                new LexicalCategory(LexicalCategory.LexicalCategoryEnum.NOUN));
            NLGElement albert =
                phraseFactory.createWord("Albert", new LexicalCategory(LexicalCategory.LexicalCategoryEnum.NOUN));

            CoordinatedPhraseElement coord1 = new CoordinatedPhraseElement(dave, albert);

            PhraseElement born = phraseFactory.createClause(coord1, "be", "born");
            born.setFeature(Feature.TENSE, Tense.PAST);
            born.addPostModifier("in");
            born.setFeature(Feature.COMPLEMENTISER, "which");

            PhraseElement theHouse = phraseFactory.createNounPhrase("the", "house");
            theHouse.addComplement(born);

            PhraseElement clause = phraseFactory.createClause(theHouse, "be",
                phraseFactory.createPrepositionPhrase("in", "Edinburgh"));
            DocumentElement sentence = docFactory.createSentence(clause);

            NLGElement realised = realiser.realise(sentence);

            // Retrieve the realisation and dump it to the console
            Assert.AreEqual("The house which Dave Bus and Albert were born in is in Edinburgh.", realised.Realisation);
        }

        /**
         * Test the use of the Progressive and Complementiser Features in future tense.
         */
        [TestMethod]
        public virtual void testProgressiveAndComplementiserFeatures_FutureTense()
        {
            phraseFactory.Lexicon = lexicon;

            // Inner clause is 'I' 'make' 'sentence' 'for'.
            PhraseElement inner = phraseFactory.createClause("I", "make", "sentence for");
            // Inner clause set to progressive.
            inner.setFeature(Feature.PROGRESSIVE, true);

            //Complementiser on inner clause is 'whom'
            inner.setFeature(Feature.COMPLEMENTISER, "whom");

            // create the engineer and add the inner clause as post modifier 
            PhraseElement engineer = phraseFactory.createNounPhrase("the engineer");
            engineer.addComplement(inner);

            // Outer clause is: 'the engineer' 'go' (preposition 'to' 'holidays')
            PhraseElement outer = phraseFactory.createClause(engineer, "go",
                phraseFactory.createPrepositionPhrase("to", "holidays"));

            // Outer clause tense is Future.
            outer.setFeature(Feature.TENSE, Tense.FUTURE);

            // Possibly progressive as well not sure.
            outer.setFeature(Feature.PROGRESSIVE, true);

            //Outer clause postmodifier would be 'tomorrow'
            outer.addPostModifier("tomorrow");
            DocumentElement sentence = docFactory.createSentence(outer);
            NLGElement realised = realiser.realise(sentence);

            // Retrieve the realisation and dump it to the console
            Assert.AreEqual("The engineer whom I am making sentence for will be going to holidays tomorrow.",
                realised.Realisation);
        }


        /**
         * Tests the use of the Complementiser, Passive, Perfect features in past tense.
         */
        [TestMethod]
        public virtual void testComplementiserPassivePerfectFeatures_PastTense()
        {
            setUp();
            realiser.Lexicon = lexicon;

            PhraseElement inner = phraseFactory.createClause("I", "play", "poker");
            inner.setFeature(Feature.TENSE, Tense.PAST);
            inner.setFeature(Feature.COMPLEMENTISER, "where");

            PhraseElement house = phraseFactory.createNounPhrase("the", "house");
            house.addComplement(inner);

            SPhraseSpec outer = phraseFactory.createClause(null, "abandon", house);

            outer.addPostModifier("since 1986");

            outer.setFeature(Feature.PASSIVE, true);
            outer.setFeature(Feature.PERFECT, true);

            DocumentElement sentence = docFactory.createSentence(outer);
            NLGElement realised = realiser.realise(sentence);

            // Retrieve the realisation and dump it to the console
            Assert.AreEqual("The house where I played poker has been abandoned since 1986.", realised.Realisation);
        }

        /**
         * Tests the user of the progressive and complementiser featuers in past tense.
         */
        [TestMethod]
        public virtual void testProgressiveComplementiserFeatures_PastTense()
        {
            phraseFactory.Lexicon = lexicon;

            NLGElement sandwich =
                phraseFactory.createNounPhrase(new LexicalCategory(LexicalCategory.LexicalCategoryEnum.NOUN),
                    "sandwich");
            sandwich.Plural = true;

            PhraseElement first = phraseFactory.createClause("I", "make", sandwich);
            first.setFeature(Feature.TENSE, Tense.PAST);
            first.setFeature(Feature.PROGRESSIVE, true);
            first.Plural = false;

            PhraseElement second = phraseFactory.createClause("the mayonnaise", "run out");
            second.setFeature(Feature.TENSE, Tense.PAST);

            second.setFeature(Feature.COMPLEMENTISER, "when");

            first.addComplement(second);

            DocumentElement sentence = docFactory.createSentence(first);
            NLGElement realised = realiser.realise(sentence);

            // Retrieve the realisation and dump it to the console
            Assert.AreEqual("I was making sandwiches when the mayonnaise ran out.", realised.Realisation);
        }

        /**
         * Test the use of Passive in creating a Passive sentence structure: <Object> + [be] + <verb> + [by] + [Subject].
         */
        [TestMethod]
        public virtual void testPassiveFeature()
        {
            realiser.Lexicon = lexicon;

            PhraseElement phrase = phraseFactory.createClause("recession", "affect", "value");
            phrase.setFeature(Feature.PASSIVE, true);
            DocumentElement sentence = docFactory.createSentence(phrase);
            NLGElement realised = realiser.realise(sentence);

            Assert.AreEqual("Value is affected by recession.", realised.Realisation);
        }


        /**
         * Test for repetition of the future auxiliary "will", courtesy of Luxor
         * Vlonjati
         */
        [TestMethod]
        public virtual void testFutureTense()
        {
            SPhraseSpec test = phraseFactory.createClause();

            NPPhraseSpec subj = phraseFactory.createNounPhrase("I");

            VPPhraseSpec verb = phraseFactory.createVerbPhrase("go");

            AdvPhraseSpec adverb = phraseFactory.createAdverbPhrase("tomorrow");

            test.setSubject(subj);
            test.VerbPhrase = verb;
            test.setFeature(Feature.TENSE, Tense.FUTURE);
            test.addPostModifier(adverb);
            string sentence = realiser.realiseSentence(test);
            Assert.AreEqual("I will go tomorrow.", sentence);

            SPhraseSpec test2 = phraseFactory.createClause();
            NLGElement vb =
                phraseFactory.createWord("go", new LexicalCategory(LexicalCategory.LexicalCategoryEnum.VERB));
            test2.setSubject(subj);
            test2.setVerb(vb);
            test2.setFeature(Feature.TENSE, Tense.FUTURE);
            test2.addPostModifier(adverb);
            string sentence2 = realiser.realiseSentence(test);
            Assert.AreEqual("I will go tomorrow.", sentence2);
        }
    }
}