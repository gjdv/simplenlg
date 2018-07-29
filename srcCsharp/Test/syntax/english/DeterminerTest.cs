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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleNLG.Main.framework;
using SimpleNLG.Main.lexicon;
using SimpleNLG.Main.phrasespec;
using SimpleNLG.Main.realiser.english;
using SimpleNLG.Main.xmlrealiser;

namespace SimpleNLG.Test.syntax.english
{
    using NLGFactory = NLGFactory;
    using Lexicon = Lexicon;
    using NIHDBLexicon = NIHDBLexicon;
    using XMLLexicon = XMLLexicon;
    using NPPhraseSpec = NPPhraseSpec;
    using SPhraseSpec = SPhraseSpec;
    using Realiser = Realiser;

    /**
     * Some determiner tests -- in particular for indefinite articles like "a" or "an".
     *
     * @author Saad Mahamood, Data2Text Limited.
     *
     */
    [TestClass]
    public class DeterminerTest
    {
        public DeterminerTest() : base()
        {
        }

        /** The realiser. */
        private Realiser realiser;

        private NLGFactory phraseFactory;

        private Lexicon lexicon;

        private readonly string DB_FILENAME = "src/test/resources/NIHLexicon/lexAccess2011.data";


        /**
         * Set up the variables we'll need for this simplenlg.test to run (Called
         * automatically by JUnit)
         */
        [TestInitialize]
        public virtual void setUp()
        {
            lexicon = new XMLLexicon(); // built in lexicon

            phraseFactory = new NLGFactory(lexicon);
            realiser = new Realiser(lexicon);
        }

        [TestCleanup]
        public virtual void tearDown()
        {
            realiser = null;

            phraseFactory = null;

            if (null != lexicon)
            {
                lexicon = null;
            }
        }

        /**
         * testLowercaseConstant - Test for when there is a lower case constant
         */
        [TestMethod]
        public virtual void testLowercaseConstant()
        {
            SPhraseSpec sentence = phraseFactory.createClause();

            NPPhraseSpec subject = phraseFactory.createNounPhrase("a", "dog");
            sentence.setSubject(subject);

            string output = realiser.realiseSentence(sentence);

            Assert.AreEqual("A dog.", output);
        }

        /**
         * testLowercaseVowel - Test for "an" as a specifier.
         */
        [TestMethod]
        public virtual void testLowercaseVowel()
        {
            SPhraseSpec sentence = phraseFactory.createClause();

            NPPhraseSpec subject = phraseFactory.createNounPhrase("a", "owl");
            sentence.setSubject(subject);

            string output = realiser.realiseSentence(sentence);

            Assert.AreEqual("An owl.", output);
        }

        /**
         * testUppercaseConstant - Test for when there is a upper case constant
         */
        [TestMethod]
        public virtual void testUppercaseConstant()
        {
            SPhraseSpec sentence = phraseFactory.createClause();

            NPPhraseSpec subject = phraseFactory.createNounPhrase("a", "Cat");
            sentence.setSubject(subject);

            string output = realiser.realiseSentence(sentence);

            Assert.AreEqual("A Cat.", output);
        }

        /**
         * testUppercaseVowel - Test for "an" as a specifier for upper subjects.
         */
        [TestMethod]
        public virtual void testUppercaseVowel()
        {
            SPhraseSpec sentence = phraseFactory.createClause();

            NPPhraseSpec subject = phraseFactory.createNounPhrase("a", "Emu");
            sentence.setSubject(subject);

            string output = realiser.realiseSentence(sentence);

            Assert.AreEqual("An Emu.", output);
        }

        /**
         * testNumericA - Test for "a" specifier with a numeric subject 
         */
        [TestMethod]
        public virtual void testNumericA()
        {
            SPhraseSpec sentence = phraseFactory.createClause();

            NPPhraseSpec subject = phraseFactory.createNounPhrase("a", "7");
            sentence.setSubject(subject);

            string output = realiser.realiseSentence(sentence);

            Assert.AreEqual("A 7.", output);
        }

        /**
         * testNumericAn - Test for "an" specifier with a numeric subject 
         */
        [TestMethod]
        public virtual void testNumericAn()
        {
            SPhraseSpec sentence = phraseFactory.createClause();

            NPPhraseSpec subject = phraseFactory.createNounPhrase("a", "11");
            sentence.setSubject(subject);

            string output = realiser.realiseSentence(sentence);

            Assert.AreEqual("An 11.", output);
        }

        /**
         * testIrregularSubjects - Test irregular subjects that don't conform to the
         * vowel vs. constant divide. 
         */
        [TestMethod]
        public virtual void testIrregularSubjects()
        {
            SPhraseSpec sentence = phraseFactory.createClause();

            NPPhraseSpec subject = phraseFactory.createNounPhrase("a", "one");
            sentence.setSubject(subject);

            string output = realiser.realiseSentence(sentence);

            Assert.AreEqual("A one.", output);
        }

        /**
         * testSingluarThisDeterminerNPObject - Test for "this" when used in the singular form as a determiner in a NP Object
         */
        [TestMethod]
        public virtual void testSingluarThisDeterminerNPObject()
        {
            SPhraseSpec sentence_1 = phraseFactory.createClause();

            NPPhraseSpec nounPhrase_1 = phraseFactory.createNounPhrase("this", "monkey");
            sentence_1.setObject(nounPhrase_1);

            Assert.AreEqual("This monkey.", realiser.realiseSentence(sentence_1));
        }

        /**
         * testPluralThisDeterminerNPObject - Test for "this" when used in the plural form as a determiner in a NP Object
         */
        [TestMethod]
        public virtual void testPluralThisDeterminerNPObject()
        {
            SPhraseSpec sentence_1 = phraseFactory.createClause();

            NPPhraseSpec nounPhrase_1 = phraseFactory.createNounPhrase("monkey");
            nounPhrase_1.Plural = true;
            nounPhrase_1.setDeterminer("this");
            sentence_1.setObject(nounPhrase_1);

            Assert.AreEqual("These monkeys.", realiser.realiseSentence(sentence_1));
        }

        /**
         * testSingluarThatDeterminerNPObject - Test for "that" when used in the singular form as a determiner in a NP Object
         */
        [TestMethod]
        public virtual void testSingluarThatDeterminerNPObject()
        {
            SPhraseSpec sentence_1 = phraseFactory.createClause();

            NPPhraseSpec nounPhrase_1 = phraseFactory.createNounPhrase("that", "monkey");
            sentence_1.setObject(nounPhrase_1);

            Assert.AreEqual("That monkey.", realiser.realiseSentence(sentence_1));
        }

        /**
         * testPluralThatDeterminerNPObject - Test for "that" when used in the plural form as a determiner in a NP Object
         */
        [TestMethod]
        public virtual void testPluralThatDeterminerNPObject()
        {
            SPhraseSpec sentence_1 = phraseFactory.createClause();

            NPPhraseSpec nounPhrase_1 = phraseFactory.createNounPhrase("monkey");
            nounPhrase_1.Plural = true;
            nounPhrase_1.setDeterminer("that");
            sentence_1.setObject(nounPhrase_1);

            Assert.AreEqual("Those monkeys.", realiser.realiseSentence(sentence_1));
        }

        /**
         * testSingularThoseDeterminerNPObject - Test for "those" when used in the singular form as a determiner in a NP Object
         */
        [TestMethod]
        public virtual void testSingularThoseDeterminerNPObject()
        {
            SPhraseSpec sentence_1 = phraseFactory.createClause();

            NPPhraseSpec nounPhrase_1 = phraseFactory.createNounPhrase("monkey");
            nounPhrase_1.Plural = false;
            nounPhrase_1.setDeterminer("those");
            sentence_1.setObject(nounPhrase_1);

            Assert.AreEqual("That monkey.", realiser.realiseSentence(sentence_1));
        }

        /**
         * testSingularTheseDeterminerNPObject - Test for "these" when used in the singular form as a determiner in a NP Object
         */
        [TestMethod]
        public virtual void testSingularTheseDeterminerNPObject()
        {
            SPhraseSpec sentence_1 = phraseFactory.createClause();

            NPPhraseSpec nounPhrase_1 = phraseFactory.createNounPhrase("monkey");
            nounPhrase_1.Plural = false;
            nounPhrase_1.setDeterminer("these");
            sentence_1.setObject(nounPhrase_1);

            Assert.AreEqual("This monkey.", realiser.realiseSentence(sentence_1));
        }

        /**
         * testPluralThoseDeterminerNPObject - Test for "those" when used in the plural form as a determiner in a NP Object
         */
        [TestMethod]
        public virtual void testPluralThoseDeterminerNPObject()
        {
            SPhraseSpec sentence_1 = phraseFactory.createClause();

            NPPhraseSpec nounPhrase_1 = phraseFactory.createNounPhrase("monkey");
            nounPhrase_1.Plural = true;
            nounPhrase_1.setDeterminer("those");
            sentence_1.setObject(nounPhrase_1);

            Assert.AreEqual("Those monkeys.", realiser.realiseSentence(sentence_1));
        }

        /**
         * testPluralTheseDeterminerNPObject - Test for "these" when used in the plural form as a determiner in a NP Object
         */
        [TestMethod]
        public virtual void testPluralTheseDeterminerNPObject()
        {
            SPhraseSpec sentence_1 = phraseFactory.createClause();

            NPPhraseSpec nounPhrase_1 = phraseFactory.createNounPhrase("monkey");
            nounPhrase_1.Plural = true;
            nounPhrase_1.setDeterminer("these");
            sentence_1.setObject(nounPhrase_1);

            Assert.AreEqual("These monkeys.", realiser.realiseSentence(sentence_1));
        }

        /**
         * testSingularTheseDeterminerNPObject - Test for "these" when used in the singular form as a determiner in a NP Object
         *                                       using the NIHDB Lexicon.
         */
        [TestMethod]
        public virtual void testSingularTheseDeterminerNPObject_NIHDBLexicon()
        {
            lexicon = new NIHDBLexicon(DB_FILENAME);
            phraseFactory = new NLGFactory(lexicon);
            realiser = new Realiser(lexicon);

            SPhraseSpec sentence_1 = phraseFactory.createClause();

            NPPhraseSpec nounPhrase_1 = phraseFactory.createNounPhrase("monkey");
            nounPhrase_1.Plural = false;
            nounPhrase_1.setDeterminer("these");
            sentence_1.setObject(nounPhrase_1);

            Assert.AreEqual("This monkey.", realiser.realiseSentence(sentence_1));
        }

        /**
         * testSingularThoseDeterminerNPObject - Test for "those" when used in the singular form as a determiner in a NP Object
         *                                       using the NIHDB Lexicon
         */
        [TestMethod]
        public virtual void testSingularThoseDeterminerNPObject_NIHDBLexicon()
        {
            lexicon = new NIHDBLexicon(DB_FILENAME);
            phraseFactory = new NLGFactory(lexicon);
            realiser = new Realiser(lexicon);

            SPhraseSpec sentence_1 = phraseFactory.createClause();

            NPPhraseSpec nounPhrase_1 = phraseFactory.createNounPhrase("monkey");
            nounPhrase_1.Plural = false;
            nounPhrase_1.setDeterminer("those");
            sentence_1.setObject(nounPhrase_1);

            Assert.AreEqual("That monkey.", realiser.realiseSentence(sentence_1));
        }

        /**
         * testPluralThatDeterminerNPObject - Test for "that" when used in the plural form as a determiner in a NP Object
         *                                    using the NIHDB Lexicon.
         */
        [TestMethod]
        public virtual void testPluralThatDeterminerNPObject_NIHDBLexicon()
        {
            lexicon = new NIHDBLexicon(DB_FILENAME);
            phraseFactory = new NLGFactory(lexicon);
            realiser = new Realiser(lexicon);

            SPhraseSpec sentence_1 = phraseFactory.createClause();

            NPPhraseSpec nounPhrase_1 = phraseFactory.createNounPhrase("monkey");
            nounPhrase_1.Plural = true;
            nounPhrase_1.setDeterminer("that");
            sentence_1.setObject(nounPhrase_1);

            Assert.AreEqual("Those monkeys.", realiser.realiseSentence(sentence_1));
        }

        /**
         * testPluralThisDeterminerNPObject - Test for "this" when used in the plural form as a determiner in a NP Object
         *                                    using the NIHDBLexicon.
         */
        [TestMethod]
        public virtual void testPluralThisDeterminerNPObject_NIHDBLexicon()
        {
            lexicon = new NIHDBLexicon(DB_FILENAME);
            phraseFactory = new NLGFactory(lexicon);
            realiser = new Realiser(lexicon);

            SPhraseSpec sentence_1 = phraseFactory.createClause();

            NPPhraseSpec nounPhrase_1 = phraseFactory.createNounPhrase("monkey");
            nounPhrase_1.Plural = true;
            nounPhrase_1.setDeterminer("this");
            sentence_1.setObject(nounPhrase_1);

            Assert.AreEqual("These monkeys.", realiser.realiseSentence(sentence_1));
        }
    }
}