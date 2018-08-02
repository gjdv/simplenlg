using System;
using System.Collections.Generic;
using SimpleNLG.Main.lexicon.util.lexCheck.Lib;

namespace SimpleNLG.Main.lexicon.util.lexCheck.CheckCont
{
    using LexRecord = LexRecord;


    public class CrossCheckDupLexRecords

    {
        //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public static boolean Check(java.util.Vector<gov.nih.nlm.nls.lexCheck.Lib.LexRecord> lexRecords, java.util.Hashtable<String, java.util.HashSet<String>> dupRecExpList, java.io.BufferedWriter out, boolean verbose) throws java.io.IOException
        public static bool Check(List<LexRecord> lexRecords, Dictionary<string, HashSet<string>> dupRecExpList,
            System.IO.StreamWriter @out, bool verbose)


        {
            bool validFlag = true;

            int recSize = lexRecords.Count;
            for (int i = 0; i < recSize; i++)

            {
                LexRecord lexRecord = (LexRecord) lexRecords[i];
                string cit = lexRecord.GetBase();
                string cat = lexRecord.GetCategory();
                string eui = lexRecord.GetEui();

                string citCat = cit + "|" + cat;
                bool dupFlag = AddToCitCatEuisTable(citCat, eui);
                validFlag = (validFlag) && (dupFlag);


                if (verbose == true)

                {
                    if ((i % 100000 == 0) && (i > 0))

                    {
                        Console.WriteLine("- Loaded " + i + " lexRecords to table");
                    }
                }
            }

            if (verbose == true)

            {
                Console.WriteLine("- Complete loaded " + recSize + " lexRecords to hash table for dupRec check");
            }

            if (!validFlag)

            {
                IEnumerator<string> keys = citCatEuisTable_.Keys.GetEnumerator();
                while (keys.MoveNext() == true)

                {
                    string key = (string) keys.Current;
                    HashSet<string> euis = (HashSet<string>) citCatEuisTable_[key];
                    bool dupFlag = false;

                    if (euis.Count > 1)

                    {
                        IEnumerator<string> it = euis.GetEnumerator();
                        while (it.MoveNext() == true)

                        {
                            string eui = (string) it.Current;
                            HashSet<string> dupRecExpEuis = (HashSet<string>) dupRecExpList[key];
                            if ((dupRecExpEuis == null) || (!dupRecExpEuis.Contains(eui)))


                            {
                                dupFlag = true;
                            }
                        }

                        if (dupFlag == true)

                        {
                            string errMsg = key;
                            IEnumerator<string> it2 = euis.GetEnumerator();
                            while (it2.MoveNext() == true)

                            {
                                errMsg = errMsg + "|" + (string) it2.Current;
                            }

                            @out.Write(errMsg + "|");
                            @out.WriteLine();
                            ErrMsgUtilLexicon.AddContentErrMsg(2, 2, errMsg);
                        }
                    }
                }
            }


            return validFlag;
        }

        public static HashSet<string> GetEuisByCitCat(string citCat)
        {
            return citCatEuisTable_[citCat];
        }

        public static HashSet<string> GetEuisByBaseCat(string baseCat)
        {
            return baseCatEuisTable_[baseCat];
        }

        public static string GetCitByEui(string citation)
        {
            return (string) euiCitTable_[citation];
        }

        private static bool AddToCitCatEuisTable(string key, string value)

        {
            bool validFlag = true;

            if (!citCatEuisTable_.ContainsKey(key))

            {
                HashSet<string> values = new HashSet<string>();
                values.Add(value);
                citCatEuisTable_[key] = values;
            }
            else

            {
                if (citCatEuisTable_[key].Add(value) != true)

                {
                    Console.Error.WriteLine("** Err@CrossCheckDupLexRecords.AddToCitCatEuisTable[" + key + ", " +
                                            value + "]");
                }


                validFlag = false;
            }

            return validFlag;
        }

        private static void AddToBaseCatEuisTable(string key, string value)

        {
            if (!baseCatEuisTable_.ContainsKey(key))

            {
                HashSet<string> values = new HashSet<string>();
                values.Add(value);
                baseCatEuisTable_[key] = values;
            }
            else if (baseCatEuisTable_[key].Add(value) != true)

            {
                Console.Error.WriteLine("** Err@CrossCheckDupLexRecords.AddToBaseCatEuisTable[" + key + ", " + value +
                                        "]");
            }
        }


        private static void AddToEuiCitTable(string key, string value)

        {
            if (!euiCitTable_.ContainsKey(key))

            {
                euiCitTable_[key] = value;
            }
            else

            {
                Console.Error.WriteLine("** Err@CrossCheckDupLexRecords.AddToEuiCitTable[" + key + ", " + value + "]");
            }
        }


        private static Dictionary<string, HashSet<string>> citCatEuisTable_ = new Dictionary<string, HashSet<string>>();


        private static Dictionary<string, HashSet<string>> baseCatEuisTable_ = new Dictionary<string, HashSet<string>>();


        private static Dictionary<string, string> euiCitTable_ = new Dictionary<string, string>();
    }


}