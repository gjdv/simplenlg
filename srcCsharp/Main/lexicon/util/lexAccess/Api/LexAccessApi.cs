using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text;
//using java.sql;
using SimpleNLG.Main.lexicon.util.lexAccess.Db;
using SimpleNLG.Main.lexicon.util.lexAccess.Lib;
using SimpleNLG.Main.lexicon.util.lexCheck.Gram;
using SimpleNLG.Main.lexicon.util.lvg;
using SimpleNLG.Main.xmlrealiser;

namespace SimpleNLG.Main.lexicon.util.lexAccess.Api
{
    using DbInflVars = DbInflVars;
    using DbLexRecord = DbLexRecord;
    using Configuration = Configuration;
    using GlobalVars = GlobalVars;
    using LexRecord = lexCheck.Lib.LexRecord;
    using BitMaskBase = BitMaskBase;
    using Category = Category;

    public class LexAccessApi

    {

        private XMLRealiser.LexiconType lexiconType;

        //public LexAccessApi(Connection conn)
        //{
        //    conn_ = conn;
        //    lexiconType = XMLRealiser.LexiconType.NIHDB_HSQL;
        //}

        public LexAccessApi(SQLiteConnection conn)
        {
            conn_ = conn;
            lexiconType = XMLRealiser.LexiconType.NIHDB_SQLITE;
        }

        public virtual string GetResultStrByTerm(string inputTerm, long category, bool showQuery, string query,
            bool noOutputFlag, string noOutputMsg, bool showTotalRecNum, int lexRecordFormat, string fieldSep)
        {
            LexAccessApiResult lexAccessApiResult = null;

            if ((!ReferenceEquals(inputTerm, null)) && (inputTerm.Length > 0))

            {
                lexAccessApiResult = GetLexRecords(inputTerm);
            }

            List<LexRecord> newLexRecordObjs = new List<LexRecord>();
            if (category < 2047L)

            {
                List<LexRecord> lexRecordObjs = lexAccessApiResult.GetJavaObjs();
                for (int i = 0; i < lexRecordObjs.Count; i++)

                {
                    LexRecord temp = (LexRecord) lexRecordObjs[i];
                    long catValue = Category.ToValue(temp.GetCategory());
                    if (BitMaskBase.Contains(category, catValue) == true)

                    {
                        newLexRecordObjs.Add(temp);
                    }
                }

                lexAccessApiResult.SetJavaObjs(newLexRecordObjs);
            }

            return FormatResultToStr(lexAccessApiResult, inputTerm, showQuery, query, noOutputFlag, noOutputMsg,
                showTotalRecNum, lexRecordFormat, fieldSep);
        }


        public virtual string GetResultStrByBase(string @base, int baseBy, long category, bool showQuery, string query,
            bool noOutputFlag, string noOutputMsg, bool showTotalRecNum, int lexRecordFormat, string fieldSep)
        {
            LexAccessApiResult lexAccessApiResult = null;

            if ((!ReferenceEquals(@base, null)) && (@base.Length > 0))

            {
                lexAccessApiResult = GetLexRecordsByBase(@base, baseBy);
            }

            List<LexRecord> newLexRecordObjs = new List<LexRecord>();
            if (category < 2047L)

            {
                List<LexRecord> lexRecordObjs = lexAccessApiResult.GetJavaObjs();
                for (int i = 0; i < lexRecordObjs.Count; i++)

                {
                    LexRecord temp = (LexRecord) lexRecordObjs[i];
                    long catValue = Category.ToValue(temp.GetCategory());
                    if (BitMaskBase.Contains(category, catValue) == true)

                    {
                        newLexRecordObjs.Add(temp);
                    }
                }

                lexAccessApiResult.SetJavaObjs(newLexRecordObjs);
            }

            return FormatResultToStr(lexAccessApiResult, @base, showQuery, query, noOutputFlag, noOutputMsg,
                showTotalRecNum, lexRecordFormat, fieldSep);
        }


        public virtual string GetResultStrByCategory(long category, bool showQuery, string query, bool noOutputFlag,
            string noOutputMsg, bool showTotalRecNum, int lexRecordFormat, string fieldSep)
        {
            LexAccessApiResult lexAccessApiResult = null;

            lexAccessApiResult = GetLexRecordsByCat(category);
            return FormatResultToStr(lexAccessApiResult, "", showQuery, query, noOutputFlag, noOutputMsg,
                showTotalRecNum, lexRecordFormat, fieldSep);
        }

        public virtual string FormatResultToStr(LexAccessApiResult lexAccessApiResult, string inputTerm, bool showQuery,
            string query, bool noOutputFlag, string noOutputMsg, bool showTotalRecNum, int lexRecordFormat,
            string fieldSep)


        {
            StringBuilder buffer = new StringBuilder();
            if (showQuery == true)

            {
                buffer.Append("Query: '" + query.Trim() + "'");
                buffer.Append(LS_STR);
            }

            if ((noOutputFlag == true) && (lexAccessApiResult.GetTotalRecordNumber() == 0))


            {
                buffer.Append(inputTerm + fieldSep + noOutputMsg);
                buffer.Append(LS_STR);
            }

            if (showTotalRecNum == true)

            {
                buffer.Append("--- Total Records Number: " + lexAccessApiResult.GetTotalRecordNumber());

                buffer.Append(LS_STR);
            }

            switch (lexRecordFormat)

            {
                case 0:
                    buffer.Append(lexAccessApiResult.GetText());
                    break;
                case 1:
                    buffer.Append(lexAccessApiResult.GetXml());
                    break;
                case 2:
                    buffer.Append(lexAccessApiResult.GetText());
                    buffer.Append(lexAccessApiResult.GetXml());
                    break;
                case 3:
                    List<string> bases = lexAccessApiResult.GetBases();
                    for (int i = 0; i < bases.Count; i++)

                    {
                        buffer.Append((string) bases[i]);
                        buffer.Append(LS_STR);
                    }

                    break;
                case 4:
                    List<string> basesD = lexAccessApiResult.GetBases(fieldSep);
                    for (int i = 0; i < basesD.Count; i++)

                    {
                        buffer.Append((string) basesD[i]);
                        buffer.Append(LS_STR);
                    }

                    break;

                case 5:
                    List<string> spellingVars = lexAccessApiResult.GetSpellingVars();
                    for (int i = 0; i < spellingVars.Count; i++)

                    {
                        buffer.Append((string) spellingVars[i]);
                        buffer.Append(LS_STR);
                    }

                    break;

                case 6:
                    List<string> spellingVarsD = lexAccessApiResult.GetSpellingVars(fieldSep);
                    for (int i = 0; i < spellingVarsD.Count; i++)

                    {
                        buffer.Append((string) spellingVarsD[i]);
                        buffer.Append(LS_STR);
                    }

                    break;
                case 7:
                    List<string> inflVars = lexAccessApiResult.GetInflVars();
                    for (int i = 0; i < inflVars.Count; i++)

                    {
                        buffer.Append((string) inflVars[i]);
                        buffer.Append(LS_STR);
                    }

                    break;

                case 8:
                    List<string> inflVarsD = lexAccessApiResult.GetInflVars(fieldSep);
                    for (int i = 0; i < inflVarsD.Count; i++)

                    {
                        buffer.Append((string) inflVarsD[i]);
                        buffer.Append(LS_STR);
                    }

                    break;
            }

            return buffer.ToString();
        }


        public virtual LexAccessApiResult GetLexRecords(string input)
        {
            LexAccessApiResult lexAccessApiResult = new LexAccessApiResult();
            if ((ReferenceEquals(input, null)) || (input.Length == 0))

            {
                return lexAccessApiResult;
            }

            bool isEui = CheckFormatEui.IsValidEui(input);
            string text = "";
            if (isEui == true)

            {
                string lexRecord = DbLexRecord.GetRecordByEui(conn_, lexiconType, input);
                text = lexRecord;
            }
            else

            {
                List<string> euis = DbInflVars.GetUniqueEuisByInflVar(conn_, lexiconType, input);

                List<string> lexRecords = new List<string>();
                foreach (string eui in euis)
                {
                    lexRecords.Add(DbLexRecord.GetRecordByEui(conn_, lexiconType, eui));
                }

                for (int i = 0; i < lexRecords.Count; i++)

                {
                    text = text + lexRecords[i];
                }
            }

            lexAccessApiResult.SetText(text);
            return lexAccessApiResult;
        }


        public virtual LexAccessApiResult GetLexRecordsByBase(string input, int baseBy)
        {
            LexAccessApiResult lexAccessApiResult = new LexAccessApiResult();
            if ((ReferenceEquals(input, null)) || (input.Length == 0))

            {
                return lexAccessApiResult;
            }

            string text = "";


            List<string> lexRecords = DbLexRecord.GetRecordsByBase(conn_, lexiconType, input, baseBy);
            for (int i = 0; i < lexRecords.Count; i++)

            {
                text = text + (string) lexRecords[i];
            }

            lexAccessApiResult.SetText(text);
            return lexAccessApiResult;
        }


        public virtual LexAccessApiResult GetLexRecordsByCat(long category)
        {
            LexAccessApiResult lexAccessApiResult = new LexAccessApiResult();
            StringBuilder text = new StringBuilder();

            List<string> lexRecords = DbLexRecord.GetRecordsByCat(conn_, lexiconType, category);

            for (int i = 0; i < lexRecords.Count; i++)

            {
                string lexRecord = (string) lexRecords[i];
                text.Append(lexRecord);
            }

            lexAccessApiResult.SetText(text.ToString());
            return lexAccessApiResult;
        }


        public virtual List<string> GetBasesByEui(string eui)
        {
            List<string> bases = DbInflVars.GetBasesByEui(conn_, lexiconType, eui);
            return bases;
        }

        
        public void Close()
        {
            switch (lexiconType)
            {
                case XMLRealiser.LexiconType.NIHDB_HSQL:
                    throw new NotImplementedException("HSQLdb disabled");
                    //((Connection)conn_).close();
                    break;
                case XMLRealiser.LexiconType.NIHDB_SQLITE:
                    ((SQLiteConnection)conn_).Dispose();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static readonly string LS_STR = GlobalVars.LS_STR;


        public const int TEXT = 0;


        public const int XML = 1;


        public const int TEXT_XML = 2;

        public const int BASE = 3;

        public const int BASE_DETAILS = 4;

        public const int SPELL_VAR = 5;

        public const int SPELL_VAR_DETAILS = 6;

        public const int INFL_VAR = 7;

        public const int INFL_VAR_DETAILS = 8;

        public const int B_NONE = 0;

        public const int B_BEGIN = 1;

        public const int B_CONTAIN = 2;

        public const int B_END = 3;

        public const int B_EXACT = 4;

        private string configFile_ = null;
        private Configuration conf_ = null;
        private string noOutputMsg_ = "-No Record Found-";
        private object conn_ = null;
    }


}