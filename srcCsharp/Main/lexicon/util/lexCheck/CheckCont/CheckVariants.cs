using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using SimpleNLG.Main.lexicon.util.lexCheck.Lib;

namespace SimpleNLG.Main.lexicon.util.lexCheck.CheckCont
{
    using LexRecord = LexRecord;


    public class CheckVariants

    {
        private const string IRREG_STR = "irreg|";

        public static bool CheckContent(LexRecord lexRecord)

        {
            bool spaceFlag = CheckCont.CheckContent.CheckSpacesForList(lexRecord, 5);

            bool validFlag = spaceFlag;
            return validFlag;
        }


        public static bool CheckContents(LexRecord lexRecord, HashSet<string> irregExpEuiList)

        {
            bool dupFlag = CheckCont.CheckContent.CheckDuplicatesForList(lexRecord, 5);

            bool irregFlag = CheckIrreg(lexRecord, irregExpEuiList);
            bool validFlag = (dupFlag) && (irregFlag);
            return validFlag;
        }


        private static bool CheckIrreg(LexRecord lexRecord, HashSet<string> irregExpEuiList)

        {
            bool validFlag = true;

            List<string> variants = lexRecord.GetVariants();

            string citation = lexRecord.GetBase();
            List<string> svList = lexRecord.GetSpellingVars();
            HashSet<string> baseList = new HashSet<string>(svList);
            baseList.Add(citation);

            HashSet<string> irregBases = new HashSet<string>();
            string variant;
            for (System.Collections.IEnumerator localIterator = variants.GetEnumerator(); localIterator.MoveNext();)
            {
                variant = (string) localIterator.Current;

                if ((variant.StartsWith("irreg|")) || (variant.StartsWith("group(irreg|")))


                {
                    string irregBase = GetIrregBase(variant);
                    if (!baseList.Contains(irregBase))

                    {
                        validFlag = false;
                        ErrMsgUtilLexRecord.AddContentErrMsg(5, 8, variant, lexRecord);
                    }
                    else

                    {
                        irregBases.Add(irregBase);
                    }
                }
            }

            if (!validFlag)

            {
                return validFlag;
            }

            if (irregBases.Count > 0)

            {
                string eui = lexRecord.GetEui();
                if ((baseList.Count != irregBases.Count) && (!irregExpEuiList.Contains(eui)))


                {
                    validFlag = false;
                    foreach (string @base in baseList)

                    {
                        if (!irregBases.Contains(@base))

                        {
                            ErrMsgUtilLexRecord.AddContentErrMsg(5, 9, @base, lexRecord);
                        }
                    }
                }
            }

            return validFlag;
        }

        private static string GetIrregBase(string variant)
        {
            string irregBase = "";
            int index1 = variant.IndexOf("irreg|", StringComparison.Ordinal);
            if (index1 < 0)

            {
                return irregBase;
            }

            int index2 = variant.IndexOf("|", index1 + 7, StringComparison.Ordinal);
            irregBase = variant.Substring(index1 + 6, index2 - (index1 + 6));
            return irregBase;
        }

        public static HashSet<string> GetIrregExpEuiListFromFile(string irregExpFile)

        {
            bool verbose = true;
            int lineNum = 100;
            return GetIrregExpEuiListFromFile(irregExpFile, verbose, lineNum);
        }

        public static HashSet<string> GetIrregExpEuiListFromFile(string irregExpFile, bool verbose, int lineNum)

        {
            HashSet<string> irregExpEuiList = null;
            bool useDefaultFileFlag = false;

            if ((ReferenceEquals(irregExpFile, null)) || (irregExpFile.Length == 0) ||
                (!System.IO.Directory.Exists(irregExpFile) || System.IO.File.Exists(irregExpFile)))


            {
                useDefaultFileFlag = true;
                string baseDir = new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase)).AbsolutePath;
                irregExpFile = baseDir + Path.DirectorySeparatorChar + "Resources/NIHLexicon/irregExceptions.data";
            }

            string defaultStr = "";
            if (useDefaultFileFlag == true)

            {
                defaultStr = " (default file in jar)";
            }

            Console.WriteLine("===== Get Irreg Exception EUIs from" + defaultStr + ": " + irregExpFile);

            irregExpEuiList = SetTable.GetSetTabe(irregExpFile, verbose, lineNum, useDefaultFileFlag);

            return irregExpEuiList;
        }
    }


}