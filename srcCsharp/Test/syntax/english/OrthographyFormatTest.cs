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
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleNLG.Main.features;
using SimpleNLG.Main.format.english;
using SimpleNLG.Main.framework;
using SimpleNLG.Main.phrasespec;

namespace SimpleNLG.Test.syntax.english
{
    using TextFormatter = TextFormatter;
    using DocumentElement = DocumentElement;
    using NLGElement = NLGElement;
    using NPPhraseSpec = NPPhraseSpec;
    using PPPhraseSpec = PPPhraseSpec;
    using SPhraseSpec = SPhraseSpec;
    using Feature = Feature;

    [TestClass]
    public class OrthographyFormatTest : SimpleNLG4Test
    {
        private bool InstanceFieldsInitialized = false;

        private void InitializeInstanceFields()
        {
            list2Realisation = (new StringBuilder("* on the rock")).Append("\n* ").Append(list1Realisation).Append("\n")
                .ToString();
        }


        private DocumentElement list1, list2;
        private DocumentElement listItem1, listItem2, listItem3;

        private string list1Realisation =
            new StringBuilder("* in the room").Append("\n* behind the curtain").Append("\n").ToString();

        private string list2Realisation;


        public OrthographyFormatTest() : this(null)
        {
        }

        public OrthographyFormatTest(string name) : base(name)
        {
            if (!InstanceFieldsInitialized)
            {
                InitializeInstanceFields();
                InstanceFieldsInitialized = true;
            }
        }

        [TestInitialize]
        public override void setUp()
        {
            base.setUp();

            // need to set formatter for realiser (set to null in the test superclass)
            realiser.Formatter = new TextFormatter();

            // a couple phrases as list items
            listItem1 = phraseFactory.createListItem(inTheRoom);
            listItem2 = phraseFactory.createListItem(behindTheCurtain);
            listItem3 = phraseFactory.createListItem(onTheRock);

            // a simple depth-1 list of phrases
            list1 = phraseFactory.createList(new List<DocumentElement> {listItem1, listItem2});


            // a list consisting of one phrase (depth-1) + a list )(depth-2)
            list2 = phraseFactory.createList(new List<DocumentElement>
            {
                listItem3,
                phraseFactory.createListItem(list1)
            });
        }

        [TestCleanup]
        public override void tearDown()
        {
            base.tearDown();
            list1 = null;
            list2 = null;
            listItem1 = null;
            listItem2 = null;
            listItem3 = null;
            list1Realisation = null;
            list2Realisation = null;
        }

        /**
         * Test the realisation of a simple list
         */
        [TestMethod]
        public virtual void testSimpleListOrthography()
        {
            NLGElement realised = realiser.realise(list1);
            Assert.AreEqual(list1Realisation, realised.Realisation);
        }

        /**
         * Test the realisation of a list with an embedded list
         */
        [TestMethod]
        public virtual void testEmbeddedListOrthography()
        {
            NLGElement realised = realiser.realise(list2);
            Assert.AreEqual(list2Realisation, realised.Realisation);
        }

        /**
         * Test the realisation of appositive pre-modifiers with commas around them.
         */
        [TestMethod]
        public virtual void testAppositivePreModifiers()
        {
            NPPhraseSpec subject = phraseFactory.createNounPhrase("I");
            NPPhraseSpec @object = phraseFactory.createNounPhrase("a bag");

            SPhraseSpec _s1 = phraseFactory.createClause(subject, "carry", @object);


            // add a PP complement
            PPPhraseSpec pp =
                phraseFactory.createPrepositionPhrase("on", phraseFactory.createNounPhrase("most", "Tuesdays"));
            _s1.addPreModifier(pp);

            //without appositive feature on pp
            Assert.AreEqual("I on most Tuesdays carry a bag", realiser.realise(_s1).Realisation);


            //with appositive feature
            pp.setFeature(Feature.APPOSITIVE, true);
            Assert.AreEqual("I, on most Tuesdays, carry a bag", realiser.realise(_s1).Realisation);
        }


        /**
         * Test the realisation of appositive pre-modifiers with commas around them.
         */
        [TestMethod]
        public virtual void testCommaSeparatedFrontModifiers()
        {
            NPPhraseSpec subject = phraseFactory.createNounPhrase("I");
            NPPhraseSpec @object = phraseFactory.createNounPhrase("a bag");

            SPhraseSpec _s1 = phraseFactory.createClause(subject, "carry", @object);


            // add a PP complement
            PPPhraseSpec pp1 =
                phraseFactory.createPrepositionPhrase("on", phraseFactory.createNounPhrase("most", "Tuesdays"));
            _s1.addFrontModifier(pp1);

            PPPhraseSpec pp2 = phraseFactory.createPrepositionPhrase("since", phraseFactory.createNounPhrase("1991"));
            _s1.addFrontModifier(pp2);
            pp1.setFeature(Feature.APPOSITIVE, true);
            pp2.setFeature(Feature.APPOSITIVE, true);

            //without setCommaSepCuephrase
            Assert.AreEqual("on most Tuesdays since 1991 I carry a bag", realiser.realise(_s1).Realisation);


            //with setCommaSepCuephrase
            realiser.CommaSepCuephrase = true;
            Assert.AreEqual("on most Tuesdays, since 1991, I carry a bag", realiser.realise(_s1).Realisation);
        }

        /**
         * Ensure we don't end up with doubled commas.
         */
        [TestMethod]
        public virtual void testNoDoubledCommas()
        {
            NPPhraseSpec subject = phraseFactory.createNounPhrase("I");
            NPPhraseSpec @object = phraseFactory.createNounPhrase("a bag");

            SPhraseSpec _s1 = phraseFactory.createClause(subject, "carry", @object);

            PPPhraseSpec pp1 =
                phraseFactory.createPrepositionPhrase("on", phraseFactory.createNounPhrase("most", "Tuesdays"));
            _s1.addFrontModifier(pp1);

            PPPhraseSpec pp2 = phraseFactory.createPrepositionPhrase("since", phraseFactory.createNounPhrase("1991"));
            PPPhraseSpec pp3 =
                phraseFactory.createPrepositionPhrase("except", phraseFactory.createNounPhrase("yesterday"));

            pp2.setFeature(Feature.APPOSITIVE, true);
            pp3.setFeature(Feature.APPOSITIVE, true);

            pp1.addPostModifier(pp2);
            pp1.addPostModifier(pp3);

            realiser.CommaSepCuephrase = true;

            Assert.AreEqual("on most Tuesdays, since 1991, except yesterday, I carry a bag",
                realiser.realise(_s1).Realisation);

            // without my fix (that we're testing here), you'd end up with 
            // "on most Tuesdays, since 1991,, except yesterday, I carry a bag"
        }

        // <[on most Tuesdays, since 1991, except yesterday, ]I carry a bag> but was:<[]I carry a bag>
    }
}