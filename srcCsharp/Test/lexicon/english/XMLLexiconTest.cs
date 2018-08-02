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
 *
 */

using System;
using System.Diagnostics;
using NUnit.Framework;
using SimpleNLG.Main.features;
using SimpleNLG.Main.framework;
using SimpleNLG.Main.lexicon;
using SimpleNLG.Main.phrasespec;
using SimpleNLG.Main.realiser.english;
using Assert = NUnit.Framework.Assert;

namespace SimpleNLG.Test.lexicon.english
{
    using Feature = Feature;
    using NumberAgreement = NumberAgreement;
    using Tense = Tense;
    using NLGFactory = NLGFactory;
    using NPPhraseSpec = NPPhraseSpec;
    using PPPhraseSpec = PPPhraseSpec;
    using SPhraseSpec = SPhraseSpec;
    using Realiser = Realiser;

    /**
     * @author D. Westwater, Data2Text Ltd
     * 
     */
    [TestFixture]
    public class XMLLexiconTest
    {
        // lexicon object -- an instance of Lexicon

        internal XMLLexicon lexicon = null;

        /**
         * Sets up the accessor and runs it -- takes ca. 26 sec
         */
        [SetUp]
        public virtual void setUp()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            //lexicon = new XMLLexicon(XML_FILENAME); // omit, use default lexicon instead
            lexicon = new XMLLexicon();

            stopwatch.Stop();

            Console.Write("Loading XML lexicon took " + stopwatch.ElapsedMilliseconds + " ms%n");
        }

        /**
         * Close the lexicon and cleanup.
         */
        [OneTimeTearDown]
        public virtual void tearDown()
        {
            if (lexicon != null)
            {
                lexicon.close();
            }
        }

        /**
         * Runs basic Lexicon tests.
         */
        [Test]
        public virtual void basicLexiconTests()
        {
            SharedLexiconTests tests = new SharedLexiconTests();
            tests.doBasicTests(lexicon);
        }

        /**
         * Tests the immutability of the XMLLexicon by checking to make sure features 
         * are not inadvertently propagated to the canonical XMLLexicon WordElement object.
         */
        [Test]
        public virtual void xmlLexiconImmutabilityTest()
        {
            NLGFactory factory = new NLGFactory(lexicon);
            Realiser realiser = new Realiser(lexicon);

            // "wall" is singular.
            NPPhraseSpec wall = factory.createNounPhrase("the", "wall");
            Assert.AreEqual(NumberAgreement.SINGULAR, wall.getFeature(Feature.NUMBER));

            // Realise a sentence with plural form of "wall"
            wall.Plural = true;
            SPhraseSpec sentence = factory.createClause("motion", "observe");
            sentence.setFeature(Feature.TENSE, Tense.PAST);
            PPPhraseSpec pp = factory.createPrepositionPhrase("in", wall);
            sentence.addPostModifier(pp);
            realiser.realiseSentence(sentence);

            // Create a new 'the wall' NP and check to make sure that the syntax processor has
            // not propagated plurality to the canonical XMLLexicon WordElement object.
            wall = factory.createNounPhrase("the", "wall");
            Assert.AreEqual(NumberAgreement.SINGULAR, wall.getFeature(Feature.NUMBER));
        }
    }
}