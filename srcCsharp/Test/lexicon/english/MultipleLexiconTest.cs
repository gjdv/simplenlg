﻿/*
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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleNLG.Main.features;
using SimpleNLG.Main.framework;
using SimpleNLG.Main.lexicon;
using SimpleNLG.Main.server;
using SimpleNLG.Main.xmlrealiser;

namespace SimpleNLG.Test.lexicon.english
{
    using LexicalFeature = LexicalFeature;
    using WordElement = WordElement;

    /**
     * @author Dave Westwater, Data2Text Ltd
     *
     */
    [TestClass]
    public class MultipleLexiconTest
    {
        internal static string BASE_DIRECTORY = @"../../";

        // NIH, XML lexicon location
        internal static string DB_FILENAME = BASE_DIRECTORY + System.IO.Path.DirectorySeparatorChar +
                                             "Resources/NIHLexicon/lexAccess2011.data";

        internal static string XML_FILENAME =
            BASE_DIRECTORY + System.IO.Path.DirectorySeparatorChar + "Resources/default-lexicon.xml";

        // multi lexicon
        internal MultipleLexicon lexicon;


        [TestInitialize]
        public virtual void setUp()
        {
            try
            {
                Properties prop = new Properties();
                prop.load("Resources/lexicon.properties");

                string xmlFile = BASE_DIRECTORY + System.IO.Path.DirectorySeparatorChar +
                                 prop.getProperty("XML_FILENAME");
                string dbFile = BASE_DIRECTORY + System.IO.Path.DirectorySeparatorChar +
                                prop.getProperty("DB_FILENAME");
                lexicon = new MultipleLexicon(new XMLLexicon(xmlFile), new NIHDBLexicon(dbFile));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
                lexicon = new MultipleLexicon(new XMLLexicon(XML_FILENAME),
                    new NIHDBLexicon(DB_FILENAME));
            }
        }

        [TestCleanup]
        public virtual void tearDown()
        {
            lexicon.close();
        }

        [TestMethod]
        public virtual void basicLexiconTests()
        {
            SharedLexiconTests tests = new SharedLexiconTests();
            tests.doBasicTests(lexicon);
        }

        [TestMethod]
        public virtual void multipleSpecificsTests()
        {
            // try to get word which is only in NIH lexicon
            WordElement UK = lexicon.getWord("UK");

            Assert.AreEqual(true, UK.getFeatureAsString(LexicalFeature.ACRONYM_OF).Contains("United Kingdom"));

            // test alwaysSearchAll flag
            bool alwaysSearchAll = lexicon.AlwaysSearchAll;

            // tree as noun exists in both, but as verb only in NIH
            lexicon.AlwaysSearchAll = true;
            Assert.AreEqual(3, lexicon.getWords("tree").Count); // 3 = once in XML plus twice in NIH

            lexicon.AlwaysSearchAll = false;
            Assert.AreEqual(1, lexicon.getWords("tree").Count);

            // restore flag to original state
            lexicon.AlwaysSearchAll = alwaysSearchAll;
        }
    }
}