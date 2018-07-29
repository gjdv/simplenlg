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

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleNLG.Main.features;
using SimpleNLG.Main.framework;
using SimpleNLG.Main.lexicon;
using SimpleNLG.Main.phrasespec;
using SimpleNLG.Main.realiser.english;
using SimpleNLG.Main.server;
using SimpleNLG.Main.xmlrealiser;

namespace SimpleNLG.Test.lexicon.english
{
    using Feature = Feature;
    using Inflection = Inflection;
    using LexicalFeature = LexicalFeature;
    using NumberAgreement = NumberAgreement;
    using Tense = Tense;
    using InflectedWordElement = InflectedWordElement;
    using LexicalCategory = LexicalCategory;
    using NLGFactory = NLGFactory;
    using WordElement = WordElement;
    using NPPhraseSpec = NPPhraseSpec;
    using SPhraseSpec = SPhraseSpec;
    using Realiser = Realiser;

    [TestClass]
    /**
     * Tests on the use of spelling and inflectional variants, using the
     * NIHDBLexicon.
     * 
     * @author bertugatt
     * 
     */
    public class LexicalVariantsTest
    {
        // lexicon object -- an instance of Lexicon
        internal Lexicon lexicon = null;

        // factory for phrases
        internal NLGFactory factory;

        // realiser
        internal Realiser realiser;

        internal static string BASE_DIRECTORY = @"../../";

        // DB location -- change this to point to the lex access data dir
        internal static string DB_FILENAME = BASE_DIRECTORY + System.IO.Path.DirectorySeparatorChar +
                                             "Resources/NIHLexicon/lexAccess2011.data";

        [TestInitialize]
        /**
         * Sets up the accessor and runs it -- takes ca. 26 sec
         */
        public virtual void setUp()
        {
            // use property file for the lexicon

            try
            {
                Properties prop = new Properties();
                prop.load(BASE_DIRECTORY + System.IO.Path.DirectorySeparatorChar + "Resources/lexicon.properties");

                string lexiconType = prop.getProperty("LexiconType");

                // the XML lexicon is used by default
                if (ReferenceEquals(lexiconType, null))
                {
                    lexiconType = "XML";
                }

                if ("NIH".Equals(lexiconType))
                {
                    // NIH lexicon
                    lexicon = new NIHDBLexicon(BASE_DIRECTORY + System.IO.Path.DirectorySeparatorChar + prop.getProperty("DB_FILENAME"));
                }
                else
                {
                    // XML lexicon
                    lexicon = new XMLLexicon(BASE_DIRECTORY + System.IO.Path.DirectorySeparatorChar +
                                             prop.getProperty("XML_FILENAME"));
                }
            }
            catch (Exception)
            {
                lexicon = new NIHDBLexicon(DB_FILENAME);
            }

            factory = new NLGFactory(lexicon);
            realiser = new Realiser(lexicon);
        }

        /**
         * Close the lexicon
         */
        [TestCleanup]
        public virtual void tearDown()
        {
            if (lexicon != null)
            {
                lexicon.close();
            }
        }

        /**
         * check that spelling variants are properly set
         */
        [TestMethod]
        public virtual void spellingVariantsTest()
        {
            WordElement asd = lexicon.getWord("Adams-Stokes disease");
            IList<string> spellVars = asd.getFeatureAsStringList(LexicalFeature.SPELL_VARS);
            Assert.IsTrue(spellVars.Contains("Adams Stokes disease"));
            Assert.IsTrue(spellVars.Contains("Adam-Stokes disease"));
            Assert.AreEqual(2, spellVars.Count);
            Assert.AreEqual(asd.BaseForm, asd.getFeatureAsString(LexicalFeature.DEFAULT_SPELL));

            // default spell variant is baseform
            Assert.AreEqual("Adams-Stokes disease", asd.DefaultSpellingVariant);


            // default spell variant changes
            asd.DefaultSpellingVariant = "Adams Stokes disease";
            Assert.AreEqual("Adams Stokes disease", asd.DefaultSpellingVariant);
        }


        /**
         * Test spelling/orthographic variants with different inflections
         */
        [TestMethod]
        public virtual void spellingVariantWithInflectionTest()
        {
            WordElement word = lexicon.getWord("formalization");
            IList<string> spellVars = word.getFeatureAsStringList(LexicalFeature.SPELL_VARS);
            Assert.IsTrue(spellVars.Contains("formalisation"));
            Assert.AreEqual(Inflection.REGULAR, word.getDefaultInflectionalVariant());


            // create with default spelling
            NPPhraseSpec np = factory.createNounPhrase("the", word);
            np.setFeature(Feature.NUMBER, NumberAgreement.PLURAL);
            Assert.AreEqual("the formalizations", realiser.realise(np).Realisation);


            // reset spell var
            word.DefaultSpellingVariant = "formalisation";
            Assert.AreEqual("the formalisations", realiser.realise(np).Realisation);
        }

        /**
         * Test the inflectional variants for a verb.
         */
        [TestMethod]
        public virtual void verbInflectionalVariantsTest()
        {
            WordElement word = lexicon.getWord("lie", new LexicalCategory(LexicalCategory.LexicalCategoryEnum.VERB));
            Assert.AreEqual(Inflection.REGULAR, word.getDefaultInflectionalVariant());


            // default past is "lied"
            InflectedWordElement infl = new InflectedWordElement(word);
            infl.setFeature(Feature.TENSE, Tense.PAST);
            string past = realiser.realise(infl).Realisation;
            Assert.AreEqual("lied", past);

            // switch to irregular
            word.setDefaultInflectionalVariant(Inflection.IRREGULAR);
            infl = new InflectedWordElement(word);
            infl.setFeature(Feature.TENSE, Tense.PAST);
            past = realiser.realise(infl).Realisation;
            Assert.AreEqual("lay", past);

            // switch back to regular
            word.setDefaultInflectionalVariant(Inflection.REGULAR);
            Assert.AreEqual(null, word.getFeature(LexicalFeature.PAST));
            infl = new InflectedWordElement(word);
            infl.setFeature(Feature.TENSE, Tense.PAST);
            past = realiser.realise(infl).Realisation;
            Assert.AreEqual("lied", past);
        }

        /**
         * Test inflectional variants for nouns
         */
        [TestMethod]
        public virtual void nounInflectionalVariantsTest()
        {
            WordElement word =
                lexicon.getWord("sanctum", new LexicalCategory(LexicalCategory.LexicalCategoryEnum.NOUN));
            Assert.AreEqual(Inflection.REGULAR, word.getDefaultInflectionalVariant());


            // reg plural shouldn't be stored
            Assert.AreEqual(null, word.getFeature(LexicalFeature.PLURAL));
            InflectedWordElement infl = new InflectedWordElement(word);
            infl.setFeature(Feature.NUMBER, NumberAgreement.PLURAL);
            string plur = realiser.realise(infl).Realisation;
            Assert.AreEqual("sanctums", plur);

            // switch to glreg
            word.setDefaultInflectionalVariant(Inflection.GRECO_LATIN_REGULAR);
            infl = new InflectedWordElement(word);
            infl.setFeature(Feature.NUMBER, NumberAgreement.PLURAL);
            plur = realiser.realise(infl).Realisation;
            Assert.AreEqual("sancta", plur);

            // and back to reg
            word.setDefaultInflectionalVariant(Inflection.REGULAR);
            infl = new InflectedWordElement(word);
            infl.setFeature(Feature.NUMBER, NumberAgreement.PLURAL);
            plur = realiser.realise(infl).Realisation;
            Assert.AreEqual("sanctums", plur);
        }

        /**
         * Check that spelling variants are preserved during realisation of NPs
         */
        [TestMethod]
        public virtual void spellingVariantsInNPTest()
        {
            WordElement asd = lexicon.getWord("Adams-Stokes disease");
            Assert.AreEqual("Adams-Stokes disease", asd.DefaultSpellingVariant);
            NPPhraseSpec np = factory.createNounPhrase(asd);
            np.setSpecifier(lexicon.getWord("the"));
            Assert.AreEqual("the Adams-Stokes disease", realiser.realise(np).Realisation);


            // change spelling var
            asd.DefaultSpellingVariant = "Adams Stokes disease";
            Assert.AreEqual("Adams Stokes disease", asd.DefaultSpellingVariant);
            Assert.AreEqual("the Adams Stokes disease", realiser.realise(np).Realisation);


            //default infl for this word is uncount
            np.setFeature(Feature.NUMBER, NumberAgreement.PLURAL);
            Assert.AreEqual("the Adams Stokes disease", realiser.realise(np).Realisation);


            //change default infl for this word
            asd.setDefaultInflectionalVariant(Inflection.REGULAR);
            Assert.AreEqual("the Adams Stokes diseases", realiser.realise(np).Realisation);
        }

        /**
         * Check that spelling variants are preserved during realisation of VPs
         */
        [TestMethod]
        public virtual void spellingVariantsInVPTest()
        {
            WordElement eth = (WordElement) factory.createWord("etherise",
                new LexicalCategory(LexicalCategory.LexicalCategoryEnum.VERB));
            Assert.AreEqual("etherize", eth.DefaultSpellingVariant);
            eth.DefaultSpellingVariant = "etherise";
            Assert.AreEqual("etherise", eth.DefaultSpellingVariant);
            SPhraseSpec s = factory.createClause(factory.createNounPhrase("the", "doctor"), eth,
                factory.createNounPhrase("the patient"));
            Assert.AreEqual("the doctor etherises the patient", realiser.realise(s).Realisation);
        }

        /**
         * Test the difference between an uncount and a count noun
         */
        [TestMethod]
        public virtual void uncountInflectionalVariantTest()
        {
            WordElement calc = (WordElement) factory.createWord("calcification",
                new LexicalCategory(LexicalCategory.LexicalCategoryEnum.NOUN));
            NPPhraseSpec theCalc = factory.createNounPhrase("the", calc);
            theCalc.setFeature(Feature.NUMBER, NumberAgreement.PLURAL);

            string r1 = realiser.realise(theCalc).Realisation;
            Assert.AreEqual("the calcifications", r1);

            calc.setDefaultInflectionalVariant(Inflection.UNCOUNT);
            NPPhraseSpec theCalc2 = factory.createNounPhrase("the", calc);
            theCalc2.setFeature(Feature.NUMBER, NumberAgreement.PLURAL);
            string r2 = realiser.realise(theCalc2).Realisation;
            Assert.AreEqual("the calcification", r2);
        }
    }
}