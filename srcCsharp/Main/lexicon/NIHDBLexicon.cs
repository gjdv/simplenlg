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

// Note: if you make a project that uses this class, make sure to also reference the System.Data.SQLite NuGet package from that project
//    see https://stackoverflow.com/questions/13028069/unable-to-load-dll-sqlite-interop-dll

using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using SimpleNLG.Main.framework;
//using java.sql;
using SimpleNLG.Main.features;
using SimpleNLG.Main.lexicon.util.lexAccess;
using SimpleNLG.Main.lexicon.util.lexAccess.Api;
using SimpleNLG.Main.lexicon.util.lexCheck.Lib;
using SimpleNLG.Main.xmlrealiser;


namespace SimpleNLG.Main.lexicon
{
    //DISABLED UNTIL SOLUTION FOUND FOR LexAccess

    //using LexAccessApi = gov.nih.nlm.nls.lexAccess.Api.LexAccessApi;
    //using LexAccessApiResult = gov.nih.nlm.nls.lexAccess.Api.LexAccessApiResult;
    //using AdjEntry = gov.nih.nlm.nls.lexCheck.Lib.AdjEntry;
    //using AdvEntry = gov.nih.nlm.nls.lexCheck.Lib.AdvEntry;
    //using InflVar = gov.nih.nlm.nls.lexCheck.Lib.InflVar;
    //using LexRecord = gov.nih.nlm.nls.lexCheck.Lib.LexRecord;
    //using NounEntry = gov.nih.nlm.nls.lexCheck.Lib.NounEntry;
    //using VerbEntry = gov.nih.nlm.nls.lexCheck.Lib.VerbEntry;


    using LexicalCategory = LexicalCategory;
    using NLGElement = NLGElement;
    using WordElement = WordElement;

    /**
     * This class gets Words from the NIH Specialist Lexicon
     * 
     * @author ereiter
     * 
     */
    public class NIHDBLexicon : Lexicon
    {

        // default DB parameters
        private static string DB_HSQL_DRIVER = "org.hsqldb.jdbc.JDBCDriver"; // DB driver
        private static string DB_HQSL_JDBC = "jdbc:hsqldb:file:"; // JDBC specifier for HSQL
        private static string DB_DEFAULT_USERNAME = "sa"; // DB username
        private static string DB_DEFAULT_PASSWORD = ""; // DB password
        private static string DB_HSQL_EXTENSION = ".data"; // filename extension for HSQL DB

        private XMLRealiser.LexiconType _lexiconType = XMLRealiser.LexiconType.NIHDB_SQLITE;

        // class variables
        private object conn = null; // DB connection
        private LexAccessApi lexdb = null; // Lexicon access object

        // if false, don't keep standard inflections in the Word object
        private bool keepStandardInflections = false;

        /****************************************************************************/
        // constructors
        /****************************************************************************/

        /**
         * set up lexicon using file which contains downloaded lexAccess HSQL DB and
         * default passwords
         * 
         * @param filename
         *            of HSQL DB
         */
        public NIHDBLexicon(string filename, XMLRealiser.LexiconType lexiconType) : base()
        {
            _lexiconType = lexiconType;
            string url;
            switch (lexiconType)
            {
                case XMLRealiser.LexiconType.NIHDB_HSQL:
                    throw new NotImplementedException("HSQLdb disabled");
                    //// get rid of .data at end of filename if necessary
                    //string dbfilename = filename;
                    //if (dbfilename.EndsWith(DB_HSQL_EXTENSION, StringComparison.Ordinal))
                    //{
                    //    dbfilename = dbfilename.Substring(0, dbfilename.Length - DB_HSQL_EXTENSION.Length);
                    //}

                    //url = DB_HQSL_JDBC + dbfilename + ";default_schema=true";

                    //SetUpConnection(new org.hsqldb.jdbcDriver(), url, DB_DEFAULT_USERNAME, DB_DEFAULT_PASSWORD);
                    //break;
                case XMLRealiser.LexiconType.NIHDB_SQLITE:
                    url = "Data Source=" + filename;
                    SetUpConnection(url);
                    break;
                default:
                    throw new Exception("Not implemented for lexiconType: " + lexiconType);
            }
            
        }

        /**
         * destructor
         */
        ~NIHDBLexicon()
        {
            if (_lexiconType == XMLRealiser.LexiconType.NIHDB_SQLITE)
            {
                ((SQLiteConnection) conn).Dispose();
            }
        }

        /**
         * set up lexicon using general DB parameters; DB must be NIH specialist
         * lexicon from lexAccess
         * 
         * @param driver
         * @param url
         * @param username
         * @param password
         */
        //public NIHDBLexicon(Driver driver, string url, string username, string password) : base()
        //{
        //    SetUpConnection(driver,url,username,password);
        //}

        private void SetUpConnection(string url)
        { //SQLITE
            try
            {
                conn = new SQLiteConnection(url);
               lexdb = new LexAccessApi((SQLiteConnection) conn);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Cannot open lexical db: " + ex.ToString());
                throw;
            }
        }

        //private void SetUpConnection(Driver driver, string url, string username, string password)
        //{ // HSQL
        //    // try to open DB and set up lexicon
        //    try
        //    {
        //        DriverManager.registerDriver(driver);
        //        conn = DriverManager.getConnection(url, username, password);

        //        // now set up lexical access object
        //        lexdb = new LexAccessApi((Connection) conn);

        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Cannot open lexical db: " + ex.ToString());
        //        throw;
        //    }

        //}

        // need more constructors for general case...

        /***************** methods to set global parameters ****************************/

        /**
         * reports whether Words include standard (derivable) inflections
         * 
         * @return true if standard inflections are kept
         */
        public virtual bool KeepStandardInflections
        {
            get
            {
                return keepStandardInflections;
            }
            set
            {
                keepStandardInflections = value;
            }
        }


        /****************************************************************************/
        // core methods to retrieve words from DB
        /****************************************************************************/

        /*
         * (non-Javadoc)
         * 
         * @see simplenlg.lexicon.Lexicon#getWords(java.lang.String,
         * simplenlg.features.LexicalCategory)
         */
        public override IList<WordElement> getWords(string baseForm, LexicalCategory category)
        {
            lock (this)
            {
                // get words from DB
                try
                {
                    LexAccessApiResult lexResult = lexdb.GetLexRecordsByBase(baseForm, LexAccessApi.B_EXACT);
                    return getWordsFromLexResult(category, lexResult);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Lexical DB error: " + ex.ToString());
                    // probably should thrown an exception
                }
                return null;
            }
        }
        /*
         * (non-Javadoc)
         * 
         * @see simplenlg.lexicon.Lexicon#getWordsByID(java.lang.String)
         */
        public override IList<WordElement> getWordsByID(string id)
        {
            lock (this)
            {
                // get words from DB
                try
                {
                    LexAccessApiResult lexResult = lexdb.GetLexRecords(id);
                    return getWordsFromLexResult(new LexicalCategory(LexicalCategory.LexicalCategoryEnum.ANY), lexResult);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Lexical DB error: " + ex.ToString());
                    // probably should thrown an exception
                }
                return null;
            }
        }


        /*
         * (non-Javadoc)
         * 
         * @see simplenlg.lexicon.Lexicon#getWordsFromVariant(java.lang.String,
         * simplenlg.features.LexicalCategory)
         */
        public override IList<WordElement> getWordsFromVariant(string variant, LexicalCategory category)
        {
            lock (this)
            {

                // get words from DB
                try
                {
                    LexAccessApiResult lexResult = lexdb.GetLexRecords(variant);
                    return getWordsFromLexResult(category, lexResult);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Lexical DB error: " + ex.ToString());
                    // probably should thrown an exception
                }
                return null;
            }
        }

        /****************************************************************************/
        // other methods
        /****************************************************************************/

        /*
         * (non-Javadoc)
         * 
         * @see simplenlg.lexicon.Lexicon#close()
         */
        public override void close()
        {
            if (lexdb != null)
            {
                lexdb.Close();
            }
        }
        /**
         * make a WordElement from a lexical record. Currently just specifies basic
         * params and inflections Should do more in the future!
         * 
         * @param record
         * @return
         */
        private WordElement makeWord(LexRecord record) // LexRecord
        {
            // get basic data
            String baseForm = record.GetBase();
            LexicalCategory category = record.GetSimpleNLGCategory(record);
            String id = record.GetEui();

            // create word class
            WordElement wordElement = new WordElement(baseForm, category, id);

            // now add type information
            switch (category.GetLexicalCategory())
            {
                case LexicalCategory.LexicalCategoryEnum.ADJECTIVE:
                    addAdjectiveInfo(wordElement, record.GetCatEntry().GetAdjEntry());
                    break;
                case LexicalCategory.LexicalCategoryEnum.ADVERB:
                    addAdverbInfo(wordElement, record.GetCatEntry().GetAdvEntry());
                    break;
                case LexicalCategory.LexicalCategoryEnum.NOUN:
                    addNounInfo(wordElement, record.GetCatEntry().GetNounEntry());
                    break;
                case LexicalCategory.LexicalCategoryEnum.VERB:
                    addVerbInfo(wordElement, record.GetCatEntry().GetVerbEntry());
                    break;
                    // ignore closed class words
            }

            Inflection? defaultInfl = wordElement.getDefaultInflectionalVariant();

            // now add inflected forms
            // if (keepStandardInflections || !standardInflections(record,
            // category)) {
            foreach (InflVar inflection in record.GetInflVarsAndAgreements().GetInflValues())
            {
                String simplenlgInflection = getSimplenlgInflection(inflection
                        .GetInflection());

                if (simplenlgInflection != null)
                {
                    String inflectedForm = inflection.GetVar();
                    Inflection? inflType = Inflection.REGULAR.getInflCode(inflection.GetType());

                    // store all inflectional variants, except for regular ones
                    // unless explicitly set
                    if (inflType != null
                            && !(Inflection.REGULAR == inflType && !keepStandardInflections))
                    {
                        wordElement.addInflectionalVariant((Inflection) inflType,
                                simplenlgInflection, inflectedForm);
                    }

                    // if the infl variant is the default, also set this feature on
                    // the word
                    if (defaultInfl == null
                            || (defaultInfl.Equals(inflType) && !(Inflection.REGULAR.Equals(inflType) && !keepStandardInflections)))
                    {
                        wordElement.setFeature(simplenlgInflection, inflectedForm);
                    }

                    // wordElement
                    // .setFeature(simplenlgInflection, inflection.GetVar());
                }
            }
            // }

            // add acronym info
            addAcronymInfo(wordElement, record);

            // now add spelling variants
            addSpellingVariants(wordElement, record);

            return wordElement;
        }

        /**
         * return list of WordElement from LexAccessApiResult
         * 
         * @param category
         *            - desired category (eg, NOUN) (this filters list)
         * @param lexResult
         *            - the LexAccessApiResult
         * @return list of WordElement
         */
        private IList<WordElement> getWordsFromLexResult(LexicalCategory category, LexAccessApiResult lexResult)
        {
            List<LexRecord> recordList = lexResult.GetJavaObjs();
            // set up array of words to return
            IList<WordElement> wordElements = new List<WordElement>();

            // iterate through result records, adding to words as appropriate
            foreach (LexRecord record in recordList)
            {

                if (category.GetLexicalCategory() == LexicalCategory.LexicalCategoryEnum.ANY || category == getSimplenlgCategory(record))
                {
                    wordElements.Add(makeWord(record));
                }
            }
            return wordElements;
        }

        /**
         * check if this record has a standard (regular) inflection
         * 
         * @param record
         * @param simplenlg
         *            syntactic category
         * @return true if standard (regular) inflection
         */
        private bool standardInflections(LexRecord record, LexicalCategory category)
        {
            List<string> variants = null;
            switch (category.GetLexicalCategory())
            {
                case LexicalCategory.LexicalCategoryEnum.NOUN:
                    variants = record.GetCatEntry().GetNounEntry().GetVariants();
                    break;
                case LexicalCategory.LexicalCategoryEnum.ADJECTIVE:
                    variants = record.GetCatEntry().GetAdjEntry().GetVariants();
                    break;
                case LexicalCategory.LexicalCategoryEnum.ADVERB:
                    variants = record.GetCatEntry().GetAdvEntry().GetVariants();
                    break;
                case LexicalCategory.LexicalCategoryEnum.MODAL:
                    variants = record.GetCatEntry().GetModalEntry().GetVariant();
                    break;
                case LexicalCategory.LexicalCategoryEnum.VERB:
                    if (record.GetCatEntry().GetVerbEntry() != null) // aux verbs (eg  be) won't  have verb entries
                    {

                        variants = record.GetCatEntry().GetVerbEntry().GetVariants();
                    }
                    break;
            }

            return variants != null && variants.Contains("reg");
        }

        /***********************************************************************************/
        // The following methods map codes in the NIH Specialist Lexicon
        // into the codes used in simplenlg
        /***********************************************************************************/

        /**
         * get the simplenlg LexicalCategory of a record
         * 
         * @param cat
         * @return
         */
        private LexicalCategory getSimplenlgCategory(LexRecord record)
        {
            string cat = record.GetCategory();
            if (cat == null)
            {
                return new LexicalCategory(LexicalCategory.LexicalCategoryEnum.ANY);
            }
            else if (cat.Equals("noun", StringComparison.OrdinalIgnoreCase))
            {
                return new LexicalCategory(LexicalCategory.LexicalCategoryEnum.NOUN);
            }
            else if (cat.Equals("verb", StringComparison.OrdinalIgnoreCase))
            {
                return new LexicalCategory(LexicalCategory.LexicalCategoryEnum.VERB);
            }
            else if (cat.Equals("aux", StringComparison.OrdinalIgnoreCase) && string.Equals(record.GetBase(), "be", StringComparison.CurrentCultureIgnoreCase))
            { // return aux "be" as a VERB

                // not needed for other aux "have" and "do", they have a verb entry
                return new LexicalCategory(LexicalCategory.LexicalCategoryEnum.VERB);
            }
            else if (cat.Equals("adj", StringComparison.OrdinalIgnoreCase))
            {
                return new LexicalCategory(LexicalCategory.LexicalCategoryEnum.ADJECTIVE);
            }
            else if (cat.Equals("adv", StringComparison.OrdinalIgnoreCase))
            {
                return new LexicalCategory(LexicalCategory.LexicalCategoryEnum.ADVERB);
            }
            else if (cat.Equals("pron", StringComparison.OrdinalIgnoreCase))
            {
                return new LexicalCategory(LexicalCategory.LexicalCategoryEnum.PRONOUN);
            }
            else if (cat.Equals("det", StringComparison.OrdinalIgnoreCase))
            {
                return new LexicalCategory(LexicalCategory.LexicalCategoryEnum.DETERMINER);
            }
            else if (cat.Equals("prep", StringComparison.OrdinalIgnoreCase))
            {
                return new LexicalCategory(LexicalCategory.LexicalCategoryEnum.PREPOSITION);
            }
            else if (cat.Equals("conj", StringComparison.OrdinalIgnoreCase))
            {
                return new LexicalCategory(LexicalCategory.LexicalCategoryEnum.CONJUNCTION);
            }
            else if (cat.Equals("compl", StringComparison.OrdinalIgnoreCase))
            {
                return new LexicalCategory(LexicalCategory.LexicalCategoryEnum.COMPLEMENTISER);
            }
            else if (cat.Equals("modal", StringComparison.OrdinalIgnoreCase))
            {
                return new LexicalCategory(LexicalCategory.LexicalCategoryEnum.MODAL);
            }
            else
            { // return ANY for other cats
                return new LexicalCategory(LexicalCategory.LexicalCategoryEnum.ANY);
            }
        }

        /**
         * convert an inflection type in NIH lexicon into one used by simplenlg
         * return null if no simplenlg equivalent to NIH inflection type
         * 
         * @param NIHInflection
         *            - inflection type in NIH lexicon
         * @return inflection type in simplenlg
         */
        private string getSimplenlgInflection(string NIHInflection)
        {
            if (ReferenceEquals(NIHInflection, null))
            {
                return null;
            }
            else if (NIHInflection.Equals("comparative", StringComparison.OrdinalIgnoreCase))
            {
                return LexicalFeature.COMPARATIVE;
            }
            else if (NIHInflection.Equals("superlative", StringComparison.OrdinalIgnoreCase))
            {
                return LexicalFeature.SUPERLATIVE;
            }
            else if (NIHInflection.Equals("plural", StringComparison.OrdinalIgnoreCase))
            {
                return LexicalFeature.PLURAL;
            }
            else if (NIHInflection.Equals("pres3s", StringComparison.OrdinalIgnoreCase))
            {
                return LexicalFeature.PRESENT3S;
            }
            else if (NIHInflection.Equals("past", StringComparison.OrdinalIgnoreCase))
            {
                return LexicalFeature.PAST;
            }
            else if (NIHInflection.Equals("pastPart", StringComparison.OrdinalIgnoreCase))
            {
                return LexicalFeature.PAST_PARTICIPLE;
            }
            else if (NIHInflection.Equals("presPart", StringComparison.OrdinalIgnoreCase))
            {
                return LexicalFeature.PRESENT_PARTICIPLE;
            }
            else
            {
                // no equvalent in simplenlg, eg clitic or negative
                return null;
            }
        }
        /**
         * extract adj information from NIH AdjEntry record, and add to a simplenlg
         * WordElement For now just extract position info
         * 
         * @param wordElement
         * @param AdjEntry
         */
        private void addAdjectiveInfo(WordElement wordElement, AdjEntry adjEntry)
        {
            bool qualitativeAdj = false;
            bool colourAdj = false;
            bool classifyingAdj = false;
            bool predicativeAdj = false;
            List<string> positions = adjEntry.GetPosition();
            foreach (string position in positions)
            {
                if (position.StartsWith("attrib(1)", StringComparison.Ordinal))
                {
                    qualitativeAdj = true;
                }
                else if (position.StartsWith("attrib(2)", StringComparison.Ordinal))
                {
                    colourAdj = true;
                }
                else if (position.StartsWith("attrib(3)", StringComparison.Ordinal))
                {
                    classifyingAdj = true;
                }
                else if (position.StartsWith("pred", StringComparison.Ordinal))
                {
                    predicativeAdj = true;
                }
                // ignore other positions
            }
            // ignore (for now) other info in record
            wordElement.setFeature(LexicalFeature.QUALITATIVE, qualitativeAdj);
            wordElement.setFeature(LexicalFeature.COLOUR, colourAdj);
            wordElement.setFeature(LexicalFeature.CLASSIFYING, classifyingAdj);
            wordElement.setFeature(LexicalFeature.PREDICATIVE, predicativeAdj);
        }

        /**
         * extract adv information from NIH AdvEntry record, and add to a simplenlg
         * WordElement For now just extract modifier type
         * 
         * @param wordElement
         * @param AdvEntry
         */
        private void addAdverbInfo(WordElement wordElement, AdvEntry advEntry)
        {
            bool verbModifier = false;
            bool sentenceModifier = false;
            bool intensifier = false;

            List<string> modifications = advEntry.GetModification();
            foreach (string modification in modifications)
            {
                if (modification.StartsWith("verb_modifier", StringComparison.Ordinal))
                {
                    verbModifier = true;
                }
                else if (modification.StartsWith("sentence_modifier", StringComparison.Ordinal))
                {
                    sentenceModifier = true;
                }
                else if (modification.StartsWith("intensifier", StringComparison.Ordinal))
                {
                    intensifier = true;
                }
                // ignore other modification types
            }
            // ignore (for now) other info in record
            wordElement.setFeature(LexicalFeature.VERB_MODIFIER, verbModifier);
            wordElement.setFeature(LexicalFeature.SENTENCE_MODIFIER, sentenceModifier);
            wordElement.setFeature(LexicalFeature.INTENSIFIER, intensifier);
        }

        /**
         * extract noun information from NIH NounEntry record, and add to a
         * simplenlg WordElement For now just extract whether count/non-count and
         * whether proper or not
         * 
         * @param wordElement
         * @param nounEntry
         */
        private void addNounInfo(WordElement wordElement, NounEntry nounEntry)
        {
            bool proper = nounEntry.IsProper();
            // bool nonCountVariant = false;
            // bool regVariant = false;

            // add the inflectional variants
            List<string> variants = nounEntry.GetVariants();

            if (variants.Count > 0)
            {
                IList<Inflection> wordVariants = new List<Inflection>();

                foreach (string v in variants)
                {
                    int index = v.IndexOf("|", StringComparison.Ordinal);
                    string code;

                    if (index > -1)
                    {
                        code = v.Substring(0, index).ToLower().Trim();

                    }
                    else
                    {
                        code = v.ToLower().Trim();
                    }

                    Inflection? infl = Inflection.REGULAR.getInflCode(code);

                    if (infl != null)
                    {
                        wordVariants.Add((Inflection)infl);
                        wordElement.addInflectionalVariant((Inflection)infl);
                    }
                }

                // if the variants include "reg", this is the default, otherwise just a random pick
                Inflection defaultVariant = wordVariants.Contains(Inflection.REGULAR) || wordVariants.Count == 0 ? Inflection.REGULAR : wordVariants[0];
                wordElement.setFeature(LexicalFeature.DEFAULT_INFL, defaultVariant);
                wordElement.setDefaultInflectionalVariant(defaultVariant);
            }

            // for (String variant : variants) {
            // if (variant.startsWith("uncount")
            // || variant.startsWith("groupuncount"))
            // nonCountVariant = true;
            //
            // if (variant.startsWith("reg"))
            // regVariant = true;
            // // ignore other variant info
            // }

            // lots of words have both "reg" and "unCount", indicating they
            // can be used in either way. Regard such words as normal,
            // only flag as nonCount if unambiguous
            // wordElement.setFeature(LexicalFeature.NON_COUNT, nonCountVariant && !regVariant);
            wordElement.setFeature(LexicalFeature.PROPER, proper);
            // ignore (for now) other info in record
        }

        /**
         * extract verb information from NIH VerbEntry record, and add to a
         * simplenlg WordElement For now just extract transitive, instransitive,
         * and/or ditransitive
         * 
         * @param wordElement
         * @param verbEntry
         */
        private void addVerbInfo(WordElement wordElement, VerbEntry verbEntry)
        {
            if (verbEntry == null)
            { // should only happen for aux verbs, which have
              // auxEntry instead of verbEntry in NIH Lex
              // just flag as transitive and return
                wordElement.setFeature(LexicalFeature.INTRANSITIVE, false);
                wordElement.setFeature(LexicalFeature.TRANSITIVE, true);
                wordElement.setFeature(LexicalFeature.DITRANSITIVE, false);
                return;
            }

            bool intransitiveVerb = verbEntry.GetIntran().Any();
            bool transitiveVerb = verbEntry.GetTran().Any() || verbEntry.GetCplxtran().Any();
            bool ditransitiveVerb = verbEntry.GetDitran().Any();

            wordElement.setFeature(LexicalFeature.INTRANSITIVE, intransitiveVerb);
            wordElement.setFeature(LexicalFeature.TRANSITIVE, transitiveVerb);
            wordElement.setFeature(LexicalFeature.DITRANSITIVE, ditransitiveVerb);

            // add the inflectional variants
            List<string> variants = verbEntry.GetVariants();

            if (variants.Count > 0)
            {
                IList<Inflection> wordVariants = new List<Inflection>();

                foreach (string v in variants)
                {
                    int index = v.IndexOf("|", StringComparison.Ordinal);
                    string code;
                    Inflection? infl;

                    if (index > -1)
                    {
                        code = v.Substring(0, index).ToLower().Trim();
                        infl = Inflection.REGULAR.getInflCode(code);

                    }
                    else
                    {
                        infl = Inflection.REGULAR.getInflCode(v.ToLower().Trim());
                    }

                    if (infl != null)
                    {
                        wordElement.addInflectionalVariant((Inflection)infl);
                        wordVariants.Add((Inflection)infl);
                    }
                }

                // if the variants include "reg", this is the default, otherwise
                // just a random pick
                Inflection defaultVariant = wordVariants.Contains(Inflection.REGULAR) || wordVariants.Count == 0 ? Inflection.REGULAR : wordVariants[0];
                //			wordElement.setFeature(LexicalFeature.INFLECTIONS, wordVariants);
                //			wordElement.setFeature(LexicalFeature.DEFAULT_INFL, defaultVariant);
                wordElement.setDefaultInflectionalVariant(defaultVariant);
            }

            // ignore (for now) other info in record
        }

        ///**
        // * convenience method to test that a list is not null and not empty
        // * 
        // * @param list
        // * @return
        // */
        //private bool notEmpty<T1>(IList<T1> list)
        //{
        //    return list != null && list.Count > 0;
        //}

        /**
         * extract information about acronyms from NIH record, and add to a
         * simplenlg WordElement.
         * 
         * <P>
         * Acronyms are represented as lists of word elements. Any acronym will have
         * a list of full form word elements, retrievable via
         * {@link LexicalFeature#ACRONYM_OF}
         * 
         * @param wordElement
         * @param record
         */
        private void addAcronymInfo(WordElement wordElement, LexRecord record)
        {
            // NB: the acronyms are actually the full forms of which the word is an
            // acronym
            List<string> acronyms = record.GetAcronyms();

            if (acronyms.Count > 0)
            {
                // the list of full forms of which this word is an acronym
                List<NLGElement> acronymOf = wordElement.getFeatureAsElementList(LexicalFeature.ACRONYM_OF);

                // keep all acronym full forms and set them up as wordElements
                foreach (string fullForm in acronyms)
                {
                    if (fullForm.Contains("|"))
                    {
                        // get the acronym id
                        string acronymID = fullForm.SubstringSpecial(fullForm.IndexOf("|", StringComparison.Ordinal) + 1, fullForm.Length);
                        // create the full form element
                        WordElement fullFormWE = getWordByID(acronymID);

                        if (fullForm != null)
                        {
                            // add as full form of this acronym
                            acronymOf.Add(fullFormWE);

                            // List<NLGElement> fullFormAcronyms = fullFormWE
                            // .getFeatureAsElementList(LexicalFeature.ACRONYMS);
                            // fullFormAcronyms.add(wordElement);
                            // fullFormWE.setFeature(LexicalFeature.ACRONYMS,
                            // fullFormAcronyms);
                        }
                    }
                }

                // set all the full forms for this acronym
                wordElement.setFeature(LexicalFeature.ACRONYM_OF, acronymOf);
            }

            // if (!acronyms.isEmpty()) {
            //
            // String acronym = acronyms.get(0);
            // // remove anything after a |, this will be an NIH ID
            // if (acronym.contains("|"))
            // acronym = acronym.substring(0, acronym.indexOf("|"));
            // wordElement.setFeature(LexicalFeature.ACRONYM_OF, acronym);
            // }
        }

        ///**
        // * Extract info about the spelling variants of a word from an NIH record,
        // * and add to the simplenlg Woordelement.
        // * 
        // * <P>
        // * Spelling variants are represented as lists of strings, retrievable via
        // * {@link LexicalFeature#SPELL_VARS}
        // * 
        // * @param wordElement
        // * @param record
        // */
        private void addSpellingVariants(WordElement wordElement, LexRecord record)
        {
            List<string> vars = record.GetSpellingVars();

            if (vars != null && vars.Count > 0)
            {
                wordElement.setFeature(LexicalFeature.SPELL_VARS, vars);
            }

            // we set the default spelling var as the baseForm
            wordElement.setFeature(LexicalFeature.DEFAULT_SPELL, wordElement.BaseForm);
        }

    }

}