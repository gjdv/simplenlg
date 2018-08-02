using System;
using System.Collections.Generic;
using SimpleNLG.Main.lexicon.util.lexCheck.Lib;

namespace SimpleNLG.Main.lexicon.util.lexCheck.CheckCont
{
    using LexRecord = LexRecord;


    public class CrossCheckAcrEui

    {
        public static bool Check(LexRecord lexRecord, HashSet<string> notBaseFormSet)

        {
            bool validFlag = true;
            List<string> acrList = lexRecord.GetAcronyms();
            string acrCat = lexRecord.GetCategory();
            for (int i = 0; i < acrList.Count; i++)

            {
                string acr = (string) acrList[i];

                int index1 = acr.IndexOf("|", StringComparison.Ordinal);
                string acrCit = "";
                string acrEui = "";
                if (index1 > 0)

                {
                    acrCit = acr.Substring(0, index1);
                    acrEui = acr.Substring(index1 + 1);
                }
                else

                {
                    acrCit = acr;
                }

                string citCat = acrCit + "|" + acrCat;

                HashSet<string> euisByCit = CrossCheckDupLexRecords.GetEuisByCitCat(citCat);

                if (euisByCit == null)

                {
                    if (acrEui.Length > 0)

                    {
                        acrList[i] = acrCit;

                        validFlag = false;
                        ErrMsgUtilLexicon.AddContentErrMsg(5, 3, acr + " - None", lexRecord);
                    }
                    else

                    {
                        validFlag = false;

                        if (!notBaseFormSet.Contains(citCat))

                        {
                            ErrMsgUtilLexicon.AddContentErrMsg(5, 4, acr + " - New", lexRecord);
                        }
                    }
                }
                else if (euisByCit.Count == 1)

                {
                    List<string> euiList = new List<string>(euisByCit);
                    string newEui = (string) euiList[0];
                    if (acrEui.Length > 0)

                    {
                        if (euisByCit.Contains(acrEui) != true)

                        {
                            validFlag = false;
                            ErrMsgUtilLexicon.AddContentErrMsg(5, 6, acr + " - " + newEui, lexRecord);
                        }
                    }
                    else

                    {
                        string newAcr = acr + "|" + newEui;
                        acrList[i] = newAcr;

                        validFlag = false;
                        ErrMsgUtilLexicon.AddContentErrMsg(5, 5, acr + " - " + newEui, lexRecord);
                    }
                }
                else

                {
                    List<string> euiList = new List<string>(euisByCit);
                    if (acrEui.Length > 0)

                    {
                        if (euisByCit.Contains(acrEui) != true)

                        {
                            validFlag = false;
                            ErrMsgUtilLexicon.AddContentErrMsg(5, 8, acr + " - " + euiList, lexRecord);
                        }
                    }
                    else

                    {
                        validFlag = false;
                        ErrMsgUtilLexicon.AddContentErrMsg(5, 7, acr + " - " + euiList, lexRecord);
                    }
                }
            }


            return validFlag;
        }
    }


}