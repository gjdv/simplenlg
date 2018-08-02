using System;
using System.Collections.Generic;
using System.Text;
using SimpleNLG.Main.lexicon.util.lexCheck.Api;
using SimpleNLG.Main.lexicon.util.lexCheck.Lib;

namespace SimpleNLG.Main.lexicon.util.lexCheck.CheckCont
{
    using ToJavaObjApi = ToJavaObjApi;
    using LexRecord = LexRecord;


    public class CrossCheckLexRecords

    {
        public static void CheckFromFile(string inFile, string outFile, bool verbose,
            Dictionary<string, HashSet<string>> dupRecExpList, HashSet<string> notBaseFormSet)

        {
            try

            {
                List<LexRecord> lexRecords = ToJavaObjApi.ToJavaObjsFromTextFile(inFile);
                System.IO.StreamWriter @out = new System.IO.StreamWriter(
                    new System.IO.FileStream(outFile, System.IO.FileMode.Create, System.IO.FileAccess.Write),
                    Encoding.UTF8);

                System.IO.StreamWriter dupOut = new System.IO.StreamWriter(
                    new System.IO.FileStream(outFile + ".dupRec", System.IO.FileMode.Create,
                        System.IO.FileAccess.Write), Encoding.UTF8);


                CheckLexRecords(lexRecords, @out, dupOut, verbose, dupRecExpList, notBaseFormSet);

                @out.Close();
                dupOut.Close();
            }
            catch (Exception e)

            {
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
            }
        }


        //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public static void CheckLexRecords(java.util.Vector<gov.nih.nlm.nls.lexCheck.Lib.LexRecord> lexRecords, java.io.BufferedWriter out, java.io.BufferedWriter dupOut, boolean verbose, java.util.Hashtable<String, java.util.HashSet<String>> dupRecExpList, java.util.HashSet<String> notBaseFormSet) throws java.io.IOException
        public static void CheckLexRecords(List<LexRecord> lexRecords, System.IO.StreamWriter @out,
            System.IO.StreamWriter dupOut, bool verbose, Dictionary<string, HashSet<string>> dupRecExpList,
            HashSet<string> notBaseFormSet)


        {
            if (verbose == true)

            {
                Console.WriteLine("===== Check Lexicon Cross-Ref Contents =====");
            }

            ErrMsgUtil.ResetErrMsg();
            bool validFlag = true;

            bool dupEuiFlag = CrossCheckDupEuis.Check(lexRecords);

            bool dupRecFlag = CrossCheckDupLexRecords.Check(lexRecords, dupRecExpList, dupOut, verbose);

            validFlag = (dupEuiFlag) && (dupRecFlag);

            int recSize = lexRecords.Count;
            for (int i = 0; i < recSize; i++)

            {
                LexRecord lexRecord = (LexRecord) lexRecords[i];

                bool nomFlag = CrossCheckNomEui.Check(lexRecord);


                bool abbFlag = CrossCheckAbbEui.Check(lexRecord, notBaseFormSet);

                bool acrFlag = CrossCheckAcrEui.Check(lexRecord, notBaseFormSet);
                validFlag = (validFlag) && (nomFlag) && (abbFlag) && (acrFlag);

                if (@out != null)

                {
                    string text = lexRecord.GetText();
                    @out.Write(text);
                }
            }

            bool symFlag = CrossCheckNomSym.Check(lexRecords);
            validFlag = (symFlag) && (validFlag);


            if (!validFlag)

            {
                Console.WriteLine(ErrMsgUtil.GetErrMsg());
            }

            Console.WriteLine("----- Total lexRecords checked: " + recSize);
            Console.WriteLine("----- cross-ref content error type stats -----");
            Console.WriteLine(ErrMsgUtilLexicon.GetErrStats());
        }
    }


}