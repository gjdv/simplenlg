using System;
using System.Collections.Generic;
using System.Text;
using SimpleNLG.Main.lexicon.util.lexCheck.Api;
using SimpleNLG.Main.lexicon.util.lexCheck.Lib;

namespace SimpleNLG.Main.lexicon.util.lexCheck.CheckCont
{
    using ToJavaObjApi = ToJavaObjApi;
    using LexRecord = LexRecord;


    public static class CheckLexRecord

    {
        public static void CheckLexRecordsFromFile(string inFile, string outFile, bool verbose,
            HashSet<string> irregExpEuiList)

        {
            try

            {
                List<LexRecord> lexRecords = ToJavaObjApi.ToJavaObjsFromTextFile(inFile);
                System.IO.StreamWriter @out = new System.IO.StreamWriter(
                    new System.IO.FileStream(outFile, System.IO.FileMode.Create, System.IO.FileAccess.Write),
                    Encoding.UTF8);


                CheckLexRecords(lexRecords, @out, verbose, irregExpEuiList);
                @out.Close();
            }
            catch (Exception e)

            {
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
            }
        }

        //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public static void CheckLexRecords(java.util.Vector<gov.nih.nlm.nls.lexCheck.Lib.LexRecord> lexRecords, boolean verbose, java.util.HashSet<String> irregExpEuiList) throws Exception
        public static void CheckLexRecords(List<LexRecord> lexRecords, bool verbose, HashSet<string> irregExpEuiList)


        {
            CheckLexRecords(lexRecords, null, verbose, irregExpEuiList);
        }

        //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public static void CheckLexRecords(java.util.Vector<gov.nih.nlm.nls.lexCheck.Lib.LexRecord> lexRecords, java.io.BufferedWriter out, boolean verbose, java.util.HashSet<String> irregExpEuiList) throws Exception
        public static void CheckLexRecords(List<LexRecord> lexRecords, System.IO.StreamWriter @out, bool verbose,
            HashSet<string> irregExpEuiList)


        {
            int errRecordNo = 0;
            int okRecordNo = 0;
            int recSize = lexRecords.Count;
            if (verbose == true)

            {
                Console.WriteLine("===== Check LexRecord Contents =====");
            }

            if (lexRecords.Count > 0)

            {
                for (int i = 0; i < recSize; i++)

                {
                    LexRecord lexRecord = (LexRecord) lexRecords[i];
                    if (verbose == true)

                    {
                        Console.WriteLine("--- Checking: " + lexRecord.GetEui() + " ---");
                    }

                    if (!StaticCheckLexRecord(lexRecord, irregExpEuiList))

                    {
                        Console.WriteLine(ErrMsgUtil.GetErrMsg());
                        errRecordNo++;
                    }
                    else

                    {
                        okRecordNo++;
                    }

                    if (@out != null)

                    {
                        string text = lexRecord.GetText();
                        @out.Write(text);
                    }
                }
            }

            Console.WriteLine("----- Total lexRecords checked: " + recSize);

            Console.WriteLine("--- lexRecord has no error: " + okRecordNo);
            Console.WriteLine("--- lexRecord has error(s): " + errRecordNo);
            Console.WriteLine("----- content error type stats -----");
            Console.WriteLine(ErrMsgUtilLexRecord.GetErrStats());
        }


        public static bool StaticCheckLexRecord(LexRecord lexRecord, HashSet<string> irregExpEuiList)

        {
            return StaticCheckLexRecord(lexRecord, irregExpEuiList, true);
        }


        public static bool StaticCheckLexRecord(LexRecord lexRecord, HashSet<string> irregExpEuiList, bool checkEuiFlag)

        {
            ErrMsgUtil.ResetErrMsg();

            bool euiFlag = true;
            if (checkEuiFlag == true)

            {
                euiFlag = CheckEui.CheckContent(lexRecord);
            }

            bool citFlag = CheckCitation.CheckContent(lexRecord);
            bool svFlag = CheckSpellingVars.CheckContent(lexRecord);
            bool catFlag = CheckCategory.CheckContent(lexRecord);
            bool varFlag = CheckVariants.CheckContent(lexRecord);
            bool nomFlag = CheckNominalizations.CheckContent(lexRecord);
            bool abbFlag = CheckAbbreviations.CheckContent(lexRecord);
            bool acrFlag = CheckAcronyms.CheckContent(lexRecord);

            bool citFlag2 = CheckCitation.CheckContents(lexRecord);
            bool svFlag2 = CheckSpellingVars.CheckContents(lexRecord);
            bool varFlag2 = CheckVariants.CheckContents(lexRecord, irregExpEuiList);

            bool nomFlag2 = CheckNominalizations.CheckContents(lexRecord);
            bool abbFlag2 = CheckAbbreviations.CheckContents(lexRecord);
            bool acrFlag2 = CheckAcronyms.CheckContents(lexRecord);
            bool validFlag = (euiFlag) && (citFlag) && (svFlag) && (catFlag) && (varFlag) && (nomFlag) && (abbFlag) &&
                             (acrFlag) && (citFlag2) && (svFlag2) && (varFlag2) && (nomFlag2) && (abbFlag2) &&
                             (acrFlag2);


            return validFlag;
        }
    }


}