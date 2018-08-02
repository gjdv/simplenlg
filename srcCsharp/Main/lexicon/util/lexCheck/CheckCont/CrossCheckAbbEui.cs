using System;
using System.Collections.Generic;
using SimpleNLG.Main.lexicon.util.lexCheck.Lib;

namespace SimpleNLG.Main.lexicon.util.lexCheck.CheckCont
{
    using LexRecord = LexRecord;


    public class CrossCheckAbbEui

    {
        public static bool Check(LexRecord lexRecord, HashSet<string> notBaseFormSet)

        {
            bool validFlag = true;
            List<string> abbList = lexRecord.GetAbbreviations();
            string abbCat = lexRecord.GetCategory();
            for (int i = 0; i < abbList.Count; i++)

            {
                string abb = (string) abbList[i];

                int index1 = abb.IndexOf("|", StringComparison.Ordinal);
                string abbCit = "";
                string abbEui = "";
                if (index1 > 0)

                {
                    abbCit = abb.Substring(0, index1);
                    abbEui = abb.Substring(index1 + 1);
                }
                else

                {
                    abbCit = abb;
                }

                string citCat = abbCit + "|" + abbCat;

                HashSet<string> euisByCitCat = CrossCheckDupLexRecords.GetEuisByCitCat(citCat);

                if (euisByCitCat == null)

                {
                    if (abbEui.Length > 0)

                    {
                        abbList[i] = abbCit;

                        validFlag = false;
                        ErrMsgUtilLexicon.AddContentErrMsg(4, 3, abb + " - None", lexRecord);
                    }
                    else

                    {
                        validFlag = false;

                        if (!notBaseFormSet.Contains(citCat))

                        {
                            ErrMsgUtilLexicon.AddContentErrMsg(4, 4, abb + " - New", lexRecord);
                        }
                    }
                }
                else if (euisByCitCat.Count == 1)

                {
                    List<string> euiList = new List<string>(euisByCitCat);
                    string newEui = (string) euiList[0];
                    if (abbEui.Length > 0)

                    {
                        if (euisByCitCat.Contains(abbEui) != true)

                        {
                            validFlag = false;
                            ErrMsgUtilLexicon.AddContentErrMsg(4, 6, abb + " - " + newEui, lexRecord);
                        }
                    }
                    else

                    {
                        string newAbb = abb + "|" + newEui;
                        abbList[i] = newAbb;

                        validFlag = false;
                        ErrMsgUtilLexicon.AddContentErrMsg(4, 5, abb + " - " + newEui, lexRecord);
                    }
                }
                else

                {
                    List<string> euiList = new List<string>(euisByCitCat);
                    if (abbEui.Length > 0)

                    {
                        if (euisByCitCat.Contains(abbEui) != true)

                        {
                            validFlag = false;
                            ErrMsgUtilLexicon.AddContentErrMsg(4, 8, abb + " - " + euiList, lexRecord);
                        }
                    }
                    else

                    {
                        validFlag = false;
                        ErrMsgUtilLexicon.AddContentErrMsg(4, 7, abb + " - " + euiList, lexRecord);
                    }
                }
            }


            return validFlag;
        }
    }


}