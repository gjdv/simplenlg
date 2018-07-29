/*
 * Ported to C# by Gert-Jan de Vries
 */
 
using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleNLG.Main.features;
using SimpleNLG.Main.framework;
using SimpleNLG.Main.lexicon;
using SimpleNLG.Main.realiser.english;
using SimpleNLG.Main.server;
using SimpleNLG.Main.xmlrealiser;

namespace SimpleNLG.Test.lexicon.english
{
    using Feature = Feature;
    using LexicalFeature = LexicalFeature;
    using NumberAgreement = NumberAgreement;
    using Person = Person;
    using Tense = Tense;
    using InflectedWordElement = InflectedWordElement;
    using LexicalCategory = LexicalCategory;
    using NLGElement = NLGElement;
    using WordElement = WordElement;
    using Realiser = Realiser;

    [TestClass]
    public class NIHDBLexiconTest
    {
        internal NIHDBLexicon lexicon = null;

        internal static string BASE_DIRECTORY = @"../../";

        // DB location -- change this to point to the lex access data dir
        internal static string DB_FILENAME = BASE_DIRECTORY + System.IO.Path.DirectorySeparatorChar +
                                             "Resources/NIHLexicon/lexAccess2011.data";

        [TestInitialize]
        public virtual void setUp()
        {
            // use property file for the lexicon

            try
            {
                Properties prop = new Properties();
                prop.load("Resources/lexicon.properties");

                string lexiconPath = prop.getProperty("DB_FILENAME");

                if (null != lexiconPath)
                {
                    lexicon = new NIHDBLexicon(BASE_DIRECTORY + System.IO.Path.DirectorySeparatorChar + lexiconPath);
                }
            }
            catch (Exception)
            {
                lexicon = new NIHDBLexicon(DB_FILENAME);
            }
        }


        [TestCleanup]
        public virtual void tearDown()
        {
            if (lexicon != null)
            {
                lexicon.close();
            }
        }


        [TestMethod]
        public virtual void basicLexiconTests()
        {
            SharedLexiconTests tests = new SharedLexiconTests();
            tests.doBasicTests(lexicon);
        }

        [TestMethod]
        public virtual void beInflectionTest()
        {
            Realiser r = new Realiser();
            WordElement word = lexicon.getWord("be", new LexicalCategory(LexicalCategory.LexicalCategoryEnum.VERB));
            InflectedWordElement inflWord = new InflectedWordElement(word);


            inflWord.setFeature(Feature.PERSON, Person.FIRST);
            inflWord.setFeature(Feature.TENSE, Tense.PAST);
            Assert.AreEqual("was", r.realise(inflWord).ToString());


            inflWord.setFeature(Feature.PERSON, Person.SECOND);
            inflWord.setFeature(Feature.TENSE, Tense.PAST);
            Assert.AreEqual("were", r.realise(inflWord).ToString());


            inflWord.setFeature(Feature.PERSON, Person.THIRD);
            inflWord.setFeature(Feature.TENSE, Tense.PAST);
            Assert.AreEqual("was", r.realise(inflWord).ToString());


            inflWord.setFeature(Feature.PERSON, Person.FIRST);
            inflWord.setFeature(Feature.TENSE, Tense.PRESENT);
            Assert.AreEqual("am", r.realise(inflWord).ToString());


            inflWord.setFeature(Feature.PERSON, Person.SECOND);
            inflWord.setFeature(Feature.TENSE, Tense.PRESENT);
            Assert.AreEqual("are", r.realise(inflWord).ToString());


            inflWord.setFeature(Feature.PERSON, Person.THIRD);
            inflWord.setFeature(Feature.TENSE, Tense.PRESENT);
            Assert.AreEqual("is", r.realise(inflWord).ToString());

            inflWord.setFeature(Feature.NUMBER, NumberAgreement.PLURAL);


            inflWord.setFeature(Feature.PERSON, Person.FIRST);
            inflWord.setFeature(Feature.TENSE, Tense.PAST);
            Assert.AreEqual("were", r.realise(inflWord).ToString());


            inflWord.setFeature(Feature.PERSON, Person.SECOND);
            inflWord.setFeature(Feature.TENSE, Tense.PAST);
            Assert.AreEqual("were", r.realise(inflWord).ToString());


            inflWord.setFeature(Feature.PERSON, Person.THIRD);
            inflWord.setFeature(Feature.TENSE, Tense.PAST);
            Assert.AreEqual("were", r.realise(inflWord).ToString());


            inflWord.setFeature(Feature.PERSON, Person.FIRST);
            inflWord.setFeature(Feature.TENSE, Tense.PRESENT);
            Assert.AreEqual("are", r.realise(inflWord).ToString());


            inflWord.setFeature(Feature.PERSON, Person.SECOND);
            inflWord.setFeature(Feature.TENSE, Tense.PRESENT);
            Assert.AreEqual("are", r.realise(inflWord).ToString());


            inflWord.setFeature(Feature.PERSON, Person.THIRD);
            inflWord.setFeature(Feature.TENSE, Tense.PRESENT);
            Assert.AreEqual("are", r.realise(inflWord).ToString());
        }


        [TestMethod]
        public virtual void acronymsTests()
        {
            WordElement uk = lexicon.getWord("UK");
            WordElement unitedKingdom = lexicon.getWord("United Kingdom");
            IList<NLGElement> fullForms = uk.getFeatureAsElementList(LexicalFeature.ACRONYM_OF);


            Assert.AreEqual(3, fullForms.Count);
            Assert.IsTrue(fullForms.Contains(unitedKingdom));
        }


        [TestMethod]
        public virtual void standardInflectionsTest()
        {
            bool keepInflectionsFlag = lexicon.KeepStandardInflections;

            lexicon.KeepStandardInflections = true;
            WordElement dog = lexicon.getWord("dog", new LexicalCategory(LexicalCategory.LexicalCategoryEnum.NOUN));
            Assert.AreEqual("dogs", dog.getFeatureAsString(LexicalFeature.PLURAL));

            lexicon.KeepStandardInflections = false;
            WordElement cat = lexicon.getWord("cat", new LexicalCategory(LexicalCategory.LexicalCategoryEnum.NOUN));
            Assert.AreEqual(null, cat.getFeatureAsString(LexicalFeature.PLURAL));


            lexicon.KeepStandardInflections = keepInflectionsFlag;
        }


        [TestMethod]
        public virtual void multiThreadAppAccessTest()
        {
            LexThread runner1 = new LexThread(this, "lie");
            LexThread runner2 = new LexThread(this, "bark");

            Thread thread1 = new Thread(new ThreadStart(runner1.Run));
            Thread thread2 = new Thread(new ThreadStart(runner2.Run));

            try
            {
                Thread.Sleep(500);
            }
            catch (Exception)
            {
                ; // do nothing
            }


            Assert.AreEqual("lie", runner1.word.BaseForm);
            Assert.AreEqual("bark", runner2.word.BaseForm);
        }


        private class LexThread
        {
            private readonly NIHDBLexiconTest outerInstance;

            internal WordElement word;
            internal string @base;

            public LexThread(NIHDBLexiconTest outerInstance, string @base)
            {
                this.outerInstance = outerInstance;
                this.@base = @base;
            }

            public virtual void Run()
            {
                word = outerInstance.lexicon.getWord(@base,
                    new LexicalCategory(LexicalCategory.LexicalCategoryEnum.VERB));
            }
        }
    }
}