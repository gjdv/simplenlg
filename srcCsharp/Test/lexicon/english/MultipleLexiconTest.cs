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
using SimpleNLG.Main.features;
using SimpleNLG.Main.framework;
using SimpleNLG.Main.lexicon;
using SimpleNLG.Main.server;
using SimpleNLG.Main.xmlrealiser;
using Assert = NUnit.Framework.Assert;

namespace SimpleNLG.Test.lexicon.english
{
    using LexicalFeature = LexicalFeature;
    using WordElement = WordElement;

    /**
     * @author Dave Westwater, Data2Text Ltd
     *
     */
    [TestFixture]
    public class MultipleLexiconTest
    {
        // NIH, XML lexicon location
        internal static string DB_FILENAME = "Resources/NIHLexicon/lexAccess2011.sqlite";

        internal static XMLRealiser.LexiconType LEXICON_TYPE = XMLRealiser.LexiconType.NIHDB_SQLITE;

        internal static string XML_FILENAME = "Resources/default-lexicon.xml";

        // multi lexicon
        internal MultipleLexicon lexicon;


        [SetUp]
        public virtual void setUp()
        {
            try
            {
                Properties prop = new Properties();
                prop.load("Resources/lexicon.properties");

                string xmlFile = prop.getProperty("XML_FILENAME");
                string dbFile = prop.getProperty("DB_FILENAME");
                string lexiconTypeStr = prop.getProperty("LexiconType");
                XMLRealiser.LexiconType lexiconType = XMLRealiser.LexiconType.NIHDB_HSQL;
                if (lexiconTypeStr == "NIH_SQLITE")
                {
                    lexiconType = XMLRealiser.LexiconType.NIHDB_SQLITE;
                }

                lexicon = new MultipleLexicon(new XMLLexicon(xmlFile), new NIHDBLexicon(dbFile, lexiconType));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
                lexicon = new MultipleLexicon(new XMLLexicon(XML_FILENAME),
                    new NIHDBLexicon(DB_FILENAME, LEXICON_TYPE));
            }
        }

        [OneTimeTearDown]
        public virtual void tearDown()
        {
            lexicon.close();
        }

        [Test]
        public virtual void basicLexiconTests()
        {
            SharedLexiconTests tests = new SharedLexiconTests();
            tests.doBasicTests(lexicon);
        }

        [Test]
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