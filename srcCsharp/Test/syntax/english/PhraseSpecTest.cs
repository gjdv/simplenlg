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
using SimpleNLG.Main.phrasespec;
using Assert = NUnit.Framework.Assert;

namespace SimpleNLG.Test.syntax.english
{
    using Feature = Feature;
    using Tense = Tense;
    using InflectedWordElement = InflectedWordElement;
    using NLGElement = NLGElement;
    using PhraseElement = PhraseElement;
    using StringElement = StringElement;
    using WordElement = WordElement;
    using SPhraseSpec = SPhraseSpec;

    /**
     * test suite for simple XXXPhraseSpec classes
     * @author ereiter
     * 
     */
    [TestFixture]
    public class PhraseSpecTest : SimpleNLG4Test
    {
        public PhraseSpecTest() : this(null)
        {
        }

        public PhraseSpecTest(string name) : base(name)
        {
        }


        [OneTimeTearDown]
        public override void tearDown()
        {
            base.tearDown();
        }

        /**
         * Check that empty phrases are not realised as "null"
         */
        [Test]
        public virtual void emptyPhraseRealisationTest()
        {
            SPhraseSpec emptyClause = phraseFactory.createClause();
            Assert.AreEqual("", realiser.realise(emptyClause).Realisation);
        }


        /**
         * Test SPhraseSpec
         */
        [Test]
        public virtual void testSPhraseSpec()
        {
            // simple test of methods
            SPhraseSpec c1 = (SPhraseSpec) phraseFactory.createClause();
            c1.setVerb("give");
            c1.setSubject("John");
            c1.setObject("an apple");
            c1.setIndirectObject("Mary");
            c1.setFeature(Feature.TENSE, Tense.PAST);
            c1.setFeature(Feature.NEGATED, true);

            // check getXXX methods
            Assert.AreEqual("give", getBaseForm(c1.getVerb()));
            Assert.AreEqual("John", getBaseForm(c1.getSubject()));
            Assert.AreEqual("an apple", getBaseForm(c1.getObject()));
            Assert.AreEqual("Mary", getBaseForm(c1.getIndirectObject()));

            Assert.AreEqual("John did not give Mary an apple", realiser.realise(c1).Realisation); //$NON-NLS-1$


            // test modifier placement
            SPhraseSpec c2 = (SPhraseSpec) phraseFactory.createClause();
            c2.setVerb("see");
            c2.setSubject("the man");
            c2.setObject("me");
            c2.addModifier("fortunately");
            c2.addModifier("quickly");
            c2.addModifier("in the park");
            // try setting tense directly as a feature
            c2.setFeature(Feature.TENSE, Tense.PAST);
            Assert.AreEqual("fortunately the man quickly saw me in the park",
                realiser.realise(c2).Realisation); //$NON-NLS-1$
        }

        // get string for head of constituent
        private string getBaseForm(NLGElement constituent)
        {
            if (constituent == null)
            {
                return null;
            }
            else if (constituent is StringElement)
            {
                return constituent.Realisation;
            }
            else if (constituent is WordElement)
            {
                return ((WordElement) constituent).BaseForm;
            }
            else if (constituent is InflectedWordElement)
            {
                return getBaseForm(((InflectedWordElement) constituent).BaseWord);
            }
            else if (constituent is PhraseElement)
            {
                return getBaseForm(((PhraseElement) constituent).getHead());
            }
            else
            {
                return null;
            }
        }
    }
}