using System;
using System.Collections.Generic;
using SimpleNLG.Main.lexicon.util.lexCheck.Lib;

namespace SimpleNLG.Main.lexicon.util.lexCheck.CheckCont
{
    using LexRecord = LexRecord;


    public class CrossCheckNomEui

    {
        public static bool Check(LexRecord lexRecord)

        {
            bool validFlag = true;
            List<string> nomList = lexRecord.GetNominalizations();
            for (int i = 0; i < nomList.Count; i++)

            {
                string nom = (string) nomList[i];

                int index1 = nom.IndexOf("|", StringComparison.Ordinal);
                int index2 = nom.IndexOf("|", index1 + 1, StringComparison.Ordinal);
                string nomCit = nom.Substring(0, index1);
                string nomCat = "";
                string nomEui = "";
                if (index2 > 0)

                {
                    nomCat = nom.Substring(index1 + 1, index2 - (index1 + 1));
                    nomEui = nom.Substring(index2 + 1);
                }
                else

                {
                    nomCat = nom.Substring(index1 + 1);
                }

                string citCat = nomCit + "|" + nomCat;

                HashSet<string> euisByCit = CrossCheckDupLexRecords.GetEuisByCitCat(citCat);

                if (euisByCit == null)

                {
                    if (nomEui.Length > 0)

                    {
                        nomList[i] = citCat;

                        validFlag = false;
                        ErrMsgUtilLexicon.AddContentErrMsg(3, 3, nom + " - None", lexRecord);
                    }
                    else

                    {
                        validFlag = false;
                        ErrMsgUtilLexicon.AddContentErrMsg(3, 4, nom + " - New", lexRecord);
                    }
                }
                else if (euisByCit.Count == 1)

                {
                    List<string> euiList = new List<string>(euisByCit);
                    string newEui = (string) euiList[0];
                    if (nomEui.Length > 0)

                    {
                        if (euisByCit.Contains(nomEui) != true)

                        {
                            validFlag = false;
                            ErrMsgUtilLexicon.AddContentErrMsg(3, 6, nom + " - " + newEui, lexRecord);
                        }
                    }
                    else

                    {
                        string newNom = nom + "|" + newEui;
                        nomList[i] = newNom;

                        validFlag = false;
                        ErrMsgUtilLexicon.AddContentErrMsg(3, 5, nom + " - " + newEui, lexRecord);
                    }
                }
                else

                {
                    List<string> euiList = new List<string>(euisByCit);
                    if (nomEui.Length > 0)

                    {
                        if (euisByCit.Contains(nomEui) != true)

                        {
                            validFlag = false;
                            ErrMsgUtilLexicon.AddContentErrMsg(3, 8, nom + " - " + euiList, lexRecord);
                        }
                    }
                    else

                    {
                        validFlag = false;
                        ErrMsgUtilLexicon.AddContentErrMsg(3, 7, nom + " - " + euiList, lexRecord);
                    }
                }
            }


            return validFlag;
        }
    }


}