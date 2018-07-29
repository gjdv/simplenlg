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
using SimpleNLG.Main.framework;
using SimpleNLG.Main.phrasespec;

namespace SimpleNLG.Test.syntax.english
{
    using DocumentElement = DocumentElement;
    using NPPhraseSpec = NPPhraseSpec;
    using SPhraseSpec = SPhraseSpec;

    /**
     * Tests for the DocumentElement class.
     * 
     * @author ereiter
     */
    [TestClass]
    public class DocumentElementTest : SimpleNLG4Test
    {
        private SPhraseSpec p1, p2, p3;

        public DocumentElementTest() : this(null)
        {
        }

        /**
         * Instantiates a new document element test.
         * 
         * @param name
         *            the name
         */
        public DocumentElementTest(string name) : base(name)
        {
        }

        [TestInitialize]
        public override void setUp()
        {
            base.setUp();
            p1 = phraseFactory.createClause("you", "be", "happy");
            p2 = phraseFactory.createClause("I", "be", "sad");
            p3 = phraseFactory.createClause("they", "be", "nervous");
        }

        [TestCleanup]
        public override void tearDown()
        {
            base.tearDown();
            p1 = null;
            p2 = null;
            p3 = null;
        }

        /**
         * Basic tests.
         */
        [TestMethod]
        public virtual void testBasics()
        {
            DocumentElement s1 = phraseFactory.createSentence(p1);
            DocumentElement s2 = phraseFactory.createSentence(p2);
            DocumentElement s3 = phraseFactory.createSentence(p3);

            DocumentElement par1 = phraseFactory.createParagraph(new List<DocumentElement> {s1, s2, s3});

            Assert.AreEqual("You are happy. I am sad. They are nervous.\n\n", realiser.realise(par1).Realisation);
        }

        /**
         * Ensure that no extra whitespace is inserted into a realisation if a
         * constituent is empty. (This is to check for a bug fix for addition of
         * spurious whitespace).
         */
        [TestMethod]
        public virtual void testExtraWhitespace()
        {
            NPPhraseSpec np1 = phraseFactory.createNounPhrase("a", "vessel");

            // empty coordinate as premod
            np1.setPreModifier(phraseFactory.createCoordinatedPhrase());
            Assert.AreEqual("a vessel", realiser.realise(np1).Realisation);


            // empty adjP as premod
            np1.setPreModifier(phraseFactory.createAdjectivePhrase());
            Assert.AreEqual("a vessel", realiser.realise(np1).Realisation);


            // empty string
            np1.setPreModifier("");
            Assert.AreEqual("a vessel", realiser.realise(np1).Realisation);
        }

        /**
         * test whether sents can be embedded in a section without intervening paras
         */
        [TestMethod]
        public virtual void testEmbedding()
        {
            DocumentElement sent = phraseFactory.createSentence("This is a test");
            DocumentElement sent2 = phraseFactory.createSentence(phraseFactory.createClause("John", "be", "missing"));
            DocumentElement section = phraseFactory.createSection("SECTION TITLE");
            section.addComponent(sent);
            section.addComponent(sent2);

            Assert.AreEqual("SECTION TITLE\nThis is a test.\n\nJohn is missing.\n\n",
                realiser.realise(section).Realisation);
        }

        [TestMethod]
        public virtual void testSections()
        {
            // doc which contains a section, and two paras
            DocumentElement doc = phraseFactory.createDocument("Test Document");

            DocumentElement section = phraseFactory.createSection("Test Section");
            doc.addComponent(section);

            DocumentElement para1 = phraseFactory.createParagraph();
            DocumentElement sent1 = phraseFactory.createSentence("This is the first test paragraph");
            para1.addComponent(sent1);
            section.addComponent(para1);

            DocumentElement para2 = phraseFactory.createParagraph();
            DocumentElement sent2 = phraseFactory.createSentence("This is the second test paragraph");
            para2.addComponent(sent2);
            section.addComponent(para2);

            Assert.AreEqual(
                "Test Document\n\nTest Section\nThis is the first test paragraph.\n\nThis is the second test paragraph.\n\n",
                realiser.realise(doc).Realisation);
            //
            // Realiser htmlRealiser = new Realiser();
            // htmlRealiser.setHTML(true);
            // Assert
            // .assertEquals(
            // "<BODY><H1>Test Document</H1>\r\n<H2>Test Section</H2>\r\n<H3>Test Subsection</H3>\r\n<UL><LI>This is the first test paragraph.</LI>\r\n<LI>This is the second test paragraph.</LI>\r\n</UL>\r\n</BODY>\r\n",
            // htmlRealiser.realise(doc));
            //
            // // now lets try a doc with a header, header-less section and
            // subsection,
            // // and 2 paras (no list)
            // doc = new TextSpec();
            // doc.setDocument();
            // doc.setHeading("Test Document2");
            //
            // section = new TextSpec();
            // section.setDocStructure(DocStructure.SECTION);
            // ;
            // doc.addSpec(section);
            //
            // subsection = new TextSpec();
            // subsection.setDocStructure(DocStructure.SUBSECTION);
            // section.addSpec(subsection);
            //
            // // use list from above, with indent
            // subsection.addChild(list);
            // list.setIndentedList(false);
            //
            // Assert
            // .assertEquals(
            // "Test Document2\r\n\r\nThis is the first test paragraph.\r\n\r\nThis is the second test paragraph.\r\n",
            // this.realiser.realise(doc));
            //
            // Assert
            // .assertEquals(
            // "<BODY><H1>Test Document2</H1>\r\n<P>This is the first test paragraph.</P>\r\n<P>This is the second test paragraph.</P>\r\n</BODY>\r\n",
            // htmlRealiser.realise(doc));
        }

        /**
         * Tests for lists and embedded lists
         */
        [TestMethod]
        public virtual void testListItems()
        {
            DocumentElement list = phraseFactory.createList();
            list.addComponent(phraseFactory.createListItem(p1));
            list.addComponent(phraseFactory.createListItem(p2));
            list.addComponent(phraseFactory.createListItem(phraseFactory.createCoordinatedPhrase(p1, p2)));
            string realisation = realiser.realise(list).Realisation;
            Assert.AreEqual("* you are happy\n* I am sad\n* you are happy and I am sad\n", realisation);
        }
    }
}