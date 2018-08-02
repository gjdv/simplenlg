using System;
using System.Collections.Generic;
using SimpleNLG.Main.lexicon.util.lexCheck.Lib;

namespace SimpleNLG.Main.lexicon.util.lexCheck.CheckCont
{
    using LexRecord = LexRecord;


    public class CrossCheckNomSym

    {
        public static bool Check(List<LexRecord> lexRecords)

        {
            LoadEuiNomObjTable(lexRecords);

            bool validFlag = true;
            IEnumerator<string> euis = euiNomObjTable_.Keys.GetEnumerator();
            while (euis.MoveNext() == true)

            {
                string eui = (string) euis.Current;
                LexRecordNomObj lexRecordNomObj = (LexRecordNomObj) euiNomObjTable_[eui];
                string cit = lexRecordNomObj.GetBase();
                string cat = lexRecordNomObj.GetCategory();
                string tarNom = cit + "|" + cat + "|" + eui;

                List<string> nomList = lexRecordNomObj.GetNominalizations();
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


                        LexRecordNomObj tarLexRecordNomObj = (LexRecordNomObj) euiNomObjTable_[nomEui];

                        if (tarLexRecordNomObj == null)

                        {
                            validFlag = false;
                            ErrMsgUtilLexicon.AddContentErrMsg(3, 11, tarNom + ": " + nom);
                        }
                        else

                        {
                            List<string> tarNomList = tarLexRecordNomObj.GetNominalizations();

                            if (!nomCat.Equals(tarLexRecordNomObj.GetCategory()))

                            {
                                validFlag = false;
                                ErrMsgUtilLexicon.AddContentErrMsg(3, 10, tarNom + ": " + nom);
                            }
                            else if (!nomCit.Equals(tarLexRecordNomObj.GetBase()))

                            {
                                validFlag = false;
                                ErrMsgUtilLexicon.AddContentErrMsg(3, 9, tarNom + ": " + nom);
                            }
                            else if (tarNomList.Count == 0)

                            {
                                validFlag = false;
                                ErrMsgUtilLexicon.AddContentErrMsg(3, 11, tarNom + ": " + nom);
                            }
                            else if (!tarNomList.Contains(tarNom))

                            {
                                validFlag = false;
                                ErrMsgUtilLexicon.AddContentErrMsg(3, 11, tarNom + ": " + nom);
                            }
                        }
                    }
                }
            }


            return validFlag;
        }

        private static void LoadEuiNomObjTable(List<LexRecord> lexRecords)

        {
            int recNo = 0;
            int nomNo = 0;

            if (euiNomObjTable_ == null)

            {
                euiNomObjTable_ = new Dictionary<string, LexRecordNomObj>();
                foreach (LexRecord lexRecord in lexRecords)

                {
                    string eui = lexRecord.GetEui();
                    List<string> nominalizations = lexRecord.GetNominalizations();
                    if (nominalizations.Count > 0)

                    {
                        LexRecordNomObj lexRecordNomObj = new LexRecordNomObj(lexRecord);

                        euiNomObjTable_[eui] = lexRecordNomObj;
                        nomNo++;
                    }

                    recNo++;
                }

                Console.WriteLine("=== CrossCheckNomSym.LoadEuiNomObjTable( ) ===");

                Console.WriteLine("-- Total lexRecord: " + recNo);
                Console.WriteLine("-- Total nomLists: " + nomNo);
            }
        }

        private static Dictionary<string, LexRecordNomObj> euiNomObjTable_ = null;
    }


}