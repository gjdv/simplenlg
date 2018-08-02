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
using NUnit.Framework;
using SimpleNLG.Main.framework;
using SimpleNLG.Main.lexicon;
using SimpleNLG.Main.phrasespec;
using SimpleNLG.Main.realiser.english;
using Assert = NUnit.Framework.Assert;

namespace SimpleNLG.Test.syntax.english
{
    using CoordinatedPhraseElement = CoordinatedPhraseElement;
    using DocumentElement = DocumentElement;
    using LexicalCategory = LexicalCategory;
    using NLGElement = NLGElement;
    using NLGFactory = NLGFactory;
    using StringElement = StringElement;
    using Lexicon = Lexicon;
    using NPPhraseSpec = NPPhraseSpec;
    using SPhraseSpec = SPhraseSpec;
    using Realiser = Realiser;

    /**
     * Tests for string elements as parts of larger phrases
     * 
     * @author bertugatt
     * 
     */
    [TestFixture]
    public class StringElementTest
    {
        private Lexicon lexicon = null;
        private NLGFactory phraseFactory = null;
        private Realiser realiser = null;

        [SetUp]
        public virtual void setUp()
        {
            lexicon = Lexicon.DefaultLexicon;
            phraseFactory = new NLGFactory(lexicon);
            realiser = new Realiser(lexicon);
        }

        [OneTimeTearDown]
        public virtual void tearDown()
        {
            lexicon = null;
            phraseFactory = null;
            realiser = null;
        }

        /**
         * Test that string elements can be used as heads of NP
         */
        [Test]
        public virtual void stringElementAsHeadTest()
        {
            NPPhraseSpec np = phraseFactory.createNounPhrase();
            np.setHead(phraseFactory.createStringElement("dogs and cats"));
            np.setSpecifier(phraseFactory.createWord("the",
                new LexicalCategory(LexicalCategory.LexicalCategoryEnum.DETERMINER)));
            Assert.AreEqual("the dogs and cats", realiser.realise(np).Realisation);
        }

        /**
         * Sentences whose VP is a canned string
         */
        [Test]
        public virtual void stringElementAsVPTest()
        {
            SPhraseSpec s = phraseFactory.createClause();
            s.VerbPhrase = phraseFactory.createStringElement("eats and drinks");
            s.setSubject(phraseFactory.createStringElement("the big fat man"));
            Assert.AreEqual("the big fat man eats and drinks", realiser.realise(s).Realisation);
        }

        /**
         * Test for when the SPhraseSpec has a NPSpec added directly after it:
         * "Mary loves NP[the cow]."
         */
        [Test]
        public virtual void tailNPStringElementTest()
        {
            SPhraseSpec senSpec = phraseFactory.createClause();
            senSpec.addComplement((phraseFactory.createStringElement("mary loves")));
            NPPhraseSpec np = phraseFactory.createNounPhrase();
            np.setHead("cow");
            np.setDeterminer("the");
            senSpec.addComplement(np);
            DocumentElement completeSen = phraseFactory.createSentence();
            completeSen.addComponent(senSpec);
            Assert.AreEqual("Mary loves the cow.", realiser.realise(completeSen).Realisation);
        }

        /**
         * Test for a NP followed by a canned text: "NP[A cat] loves a dog".
         */
        [Test]
        public virtual void frontNPStringElementTest()
        {
            SPhraseSpec senSpec = phraseFactory.createClause();
            NPPhraseSpec np = phraseFactory.createNounPhrase();
            np.setHead("cat");
            np.setDeterminer("the");
            senSpec.addComplement(np);
            senSpec.addComplement(phraseFactory.createStringElement("loves a dog"));
            DocumentElement completeSen = phraseFactory.createSentence();
            completeSen.addComponent(senSpec);
            Assert.AreEqual("The cat loves a dog.", realiser.realise(completeSen).Realisation);
        }


        /**
         * Test for a StringElement followed by a NP followed by a StringElement
         * "The world loves NP[ABBA] but not a sore loser."
         */
        [Test]
        public virtual void mulitpleStringElementsTest()
        {
            SPhraseSpec senSpec = phraseFactory.createClause();
            senSpec.addComplement(phraseFactory.createStringElement("the world loves"));
            NPPhraseSpec np = phraseFactory.createNounPhrase();
            np.setHead("ABBA");
            senSpec.addComplement(np);
            senSpec.addComplement(phraseFactory.createStringElement("but not a sore loser"));
            DocumentElement completeSen = phraseFactory.createSentence();
            completeSen.addComponent(senSpec);
            Assert.AreEqual("The world loves ABBA but not a sore loser.", realiser.realise(completeSen).Realisation);
        }

        /**
         * Test for multiple NP phrases with a single StringElement phrase:
         * "NP[John is] a trier NP[for cheese]."
         */
        [Test]
        public virtual void mulitpleNPElementsTest()
        {
            SPhraseSpec senSpec = phraseFactory.createClause();
            NPPhraseSpec frontNoun = phraseFactory.createNounPhrase();
            frontNoun.setHead("john");
            senSpec.addComplement(frontNoun);
            senSpec.addComplement(phraseFactory.createStringElement("is a trier"));
            NPPhraseSpec backNoun = phraseFactory.createNounPhrase();
            backNoun.setDeterminer("for");
            backNoun.setNoun("cheese");
            senSpec.addComplement(backNoun);
            DocumentElement completeSen = phraseFactory.createSentence();
            completeSen.addComponent(senSpec);
            Assert.AreEqual("John is a trier for cheese.", realiser.realise(completeSen).Realisation);
        }


        /**
         * White space check: Test to see how SNLG deals with additional whitespaces: 
         * 
         * NP[The Nasdaq] rose steadily during NP[early trading], however it plummeted due to NP[a shock] after NP[IBM] announced poor 
         * NP[first quarter results].
         */
        [Test]
        public virtual void whiteSpaceNPTest()
        {
            SPhraseSpec senSpec = phraseFactory.createClause();
            NPPhraseSpec firstNoun = phraseFactory.createNounPhrase();
            firstNoun.setDeterminer("the");
            firstNoun.setNoun("Nasdaq");
            senSpec.addComplement(firstNoun);
            senSpec.addComplement(phraseFactory.createStringElement(" rose steadily during "));
            NPPhraseSpec secondNoun = phraseFactory.createNounPhrase();
            secondNoun.setSpecifier("early");
            secondNoun.setNoun("trading");
            senSpec.addComplement(secondNoun);
            senSpec.addComplement(phraseFactory.createStringElement(" , however it plummeted due to"));
            NPPhraseSpec thirdNoun = phraseFactory.createNounPhrase();
            thirdNoun.setSpecifier("a");
            thirdNoun.setNoun("shock");
            senSpec.addComplement(thirdNoun);
            senSpec.addComplement(phraseFactory.createStringElement(" after "));
            NPPhraseSpec fourthNoun = phraseFactory.createNounPhrase();
            fourthNoun.setNoun("IBM");
            senSpec.addComplement(fourthNoun);
            senSpec.addComplement(phraseFactory.createStringElement("announced poor    "));
            NPPhraseSpec fifthNoun = phraseFactory.createNounPhrase();
            fifthNoun.setSpecifier("first quarter");
            fifthNoun.setNoun("results");
            fifthNoun.Plural = true;
            senSpec.addComplement(fifthNoun);
            DocumentElement completeSen = phraseFactory.createSentence();
            completeSen.addComponent(senSpec);
            Assert.AreEqual(
                "The Nasdaq rose steadily during early trading, however it plummeted due to a shock after IBM announced poor first quarter results.",
                realiser.realise(completeSen).Realisation);
        }

        /**
         * Point absorption test: Check to see if SNLG respects abbreviations at the end of a sentence.
         * "NP[Yahya] was sleeping his own and dreaming etc."
         */
        [Test]
        public virtual void pointAbsorptionTest()
        {
            SPhraseSpec senSpec = phraseFactory.createClause();
            NPPhraseSpec firstNoun = phraseFactory.createNounPhrase();
            firstNoun.setNoun("yaha");
            senSpec.addComplement(firstNoun);
            senSpec.addComplement("was sleeping on his own and dreaming etc.");
            DocumentElement completeSen = phraseFactory.createSentence();
            completeSen.addComponent(senSpec);
            Assert.AreEqual("Yaha was sleeping on his own and dreaming etc.",
                realiser.realise(completeSen).Realisation);
        }

        /**
         * Point absorption test: As above, but with trailing white space.
         * "NP[Yaha] was sleeping his own and dreaming etc.      "
         */
        [Test]
        public virtual void pointAbsorptionTrailingWhiteSpaceTest()
        {
            SPhraseSpec senSpec = phraseFactory.createClause();
            NPPhraseSpec firstNoun = phraseFactory.createNounPhrase();
            firstNoun.setNoun("yaha");
            senSpec.addComplement(firstNoun);
            senSpec.addComplement("was sleeping on his own and dreaming etc.      ");
            DocumentElement completeSen = phraseFactory.createSentence();
            completeSen.addComponent(senSpec);
            Assert.AreEqual("Yaha was sleeping on his own and dreaming etc.",
                realiser.realise(completeSen).Realisation);
        }

        /**
         * Abbreviation test: Check to see how SNLG deals with abbreviations in the middle of a sentence.
         * 
         * "NP[Yahya] and friends etc. went to NP[the park] to play."
         */
        [Test]
        public virtual void middleAbbreviationTest()
        {
            SPhraseSpec senSpec = phraseFactory.createClause();
            NPPhraseSpec firstNoun = phraseFactory.createNounPhrase();
            firstNoun.setNoun("yahya");
            senSpec.addComplement(firstNoun);
            senSpec.addComplement(phraseFactory.createStringElement("and friends etc. went to"));
            NPPhraseSpec secondNoun = phraseFactory.createNounPhrase();
            secondNoun.setDeterminer("the");
            secondNoun.setNoun("park");
            senSpec.addComplement(secondNoun);
            senSpec.addComplement("to play");
            DocumentElement completeSen = phraseFactory.createSentence();
            completeSen.addComponent(senSpec);
            Assert.AreEqual("Yahya and friends etc. went to the park to play.",
                realiser.realise(completeSen).Realisation);
        }


        /**
         * Indefinite Article Inflection: StringElement to test how SNLG handles a/an situations.
         * "I see an NP[elephant]" 
         */
        [Test]
        public virtual void stringIndefiniteArticleInflectionVowelTest()
        {
            SPhraseSpec senSpec = phraseFactory.createClause();
            senSpec.addComplement(phraseFactory.createStringElement("I see a"));
            NPPhraseSpec firstNoun = phraseFactory.createNounPhrase("elephant");
            senSpec.addComplement(firstNoun);
            DocumentElement completeSen = phraseFactory.createSentence();
            completeSen.addComponent(senSpec);
            Assert.AreEqual("I see an elephant.", realiser.realise(completeSen).Realisation);
        }

        /**
         * Indefinite Article Inflection: StringElement to test how SNLG handles a/an situations.
         * "I see NP[a elephant]" --> 
         */
        [Test]
        public virtual void NPIndefiniteArticleInflectionVowelTest()
        {
            SPhraseSpec senSpec = phraseFactory.createClause();
            senSpec.addComplement(phraseFactory.createStringElement("I see"));
            NPPhraseSpec firstNoun = phraseFactory.createNounPhrase("elephant");
            firstNoun.setDeterminer("a");
            senSpec.addComplement(firstNoun);
            DocumentElement completeSen = phraseFactory.createSentence();
            completeSen.addComponent(senSpec);
            Assert.AreEqual("I see an elephant.", realiser.realise(completeSen).Realisation);
        }


        /**
         * Indefinite Article Inflection: StringElement to test how SNLG handles a/an situations.
         * "I see an NP[cow]" 
         */
        [Test]
        public virtual void stringIndefiniteArticleInflectionConsonantTest()
        {
            SPhraseSpec senSpec = phraseFactory.createClause();
            senSpec.addComplement(phraseFactory.createStringElement("I see an"));
            NPPhraseSpec firstNoun = phraseFactory.createNounPhrase("cow");
            senSpec.addComplement(firstNoun);
            DocumentElement completeSen = phraseFactory.createSentence();
            completeSen.addComponent(senSpec);
            // Do not attempt "an" -> "a"
            Assert.AreNotSame("I see an cow.", realiser.realise(completeSen).Realisation);
        }

        /**
         * Indefinite Article Inflection: StringElement to test how SNLG handles a/an situations.
         * "I see NP[an cow]" --> 
         */
        [Test]
        public virtual void NPIndefiniteArticleInflectionConsonantTest()
        {
            SPhraseSpec senSpec = phraseFactory.createClause();
            senSpec.addComplement(phraseFactory.createStringElement("I see"));
            NPPhraseSpec firstNoun = phraseFactory.createNounPhrase("cow");
            firstNoun.setDeterminer("an");
            senSpec.addComplement(firstNoun);
            DocumentElement completeSen = phraseFactory.createSentence();
            completeSen.addComponent(senSpec);
            // Do not attempt "an" -> "a"
            Assert.AreEqual("I see an cow.", realiser.realise(completeSen).Realisation);
        }


        /**
         * aggregationStringElementTest: Test to see if we can aggregate two StringElements in a CoordinatedPhraseElement.
         */
        [Test]
        public virtual void aggregationStringElementTest()
        {
            CoordinatedPhraseElement coordinate = phraseFactory.createCoordinatedPhrase(
                new StringElement("John is going to Tesco"), new StringElement("Mary is going to Sainsburys"));
            SPhraseSpec sentence = phraseFactory.createClause();
            sentence.addComplement(coordinate);

            Assert.AreEqual("John is going to Tesco and Mary is going to Sainsburys.",
                realiser.realiseSentence(sentence));
        }


        /**
         * Tests that no empty space is added when a StringElement is instantiated with an empty string
         * or null object.
         */
        [Test]
        public virtual void nullAndEmptyStringElementTest()
        {
            NLGElement nullStringElement = phraseFactory.createStringElement(null);
            NLGElement emptyStringElement = phraseFactory.createStringElement("");
            NLGElement beautiful = phraseFactory.createStringElement("beautiful");
            NLGElement horseLike = phraseFactory.createStringElement("horse-like");
            NLGElement creature = phraseFactory.createStringElement("creature");

            // Test1: null or empty at beginning
            SPhraseSpec test1 = phraseFactory.createClause("a unicorn", "be", "regarded as a");
            test1.addPostModifier(emptyStringElement);
            test1.addPostModifier(beautiful);
            test1.addPostModifier(horseLike);
            test1.addPostModifier(creature);
            Console.WriteLine(realiser.realiseSentence(test1));
            Assert.AreEqual("A unicorn is regarded as a beautiful horse-like creature.",
                realiser.realiseSentence(test1));


            // Test2: empty or null at end
            SPhraseSpec test2 = phraseFactory.createClause("a unicorn", "be", "regarded as a");
            test2.addPostModifier(beautiful);
            test2.addPostModifier(horseLike);
            test2.addPostModifier(creature);
            test2.addPostModifier(nullStringElement);
            Console.WriteLine(realiser.realiseSentence(test2));
            Assert.AreEqual("A unicorn is regarded as a beautiful horse-like creature.",
                realiser.realiseSentence(test2));


            // Test3: empty or null in the middle
            SPhraseSpec test3 = phraseFactory.createClause("a unicorn", "be", "regarded as a");
            test3.addPostModifier("beautiful");
            test3.addPostModifier("horse-like");
            test3.addPostModifier("");
            test3.addPostModifier("creature");
            Console.WriteLine(realiser.realiseSentence(test3));
            Assert.AreEqual("A unicorn is regarded as a beautiful horse-like creature.",
                realiser.realiseSentence(test3));


            // Test4: empty or null in the middle with empty or null at beginning
            SPhraseSpec test4 = phraseFactory.createClause("a unicorn", "be", "regarded as a");
            test4.addPostModifier("");
            test4.addPostModifier("beautiful");
            test4.addPostModifier("horse-like");
            test4.addPostModifier(nullStringElement);
            test4.addPostModifier("creature");
            Console.WriteLine(realiser.realiseSentence(test4));
            Assert.AreEqual("A unicorn is regarded as a beautiful horse-like creature.",
                realiser.realiseSentence(test4));
        }
    }
}