using System;
using System.Collections.Generic;
using SimpleNLG.Main.lexicon.util.lexCheck.Lib;

namespace SimpleNLG.Main.lexicon.util.lexCheck.CheckCont
{
    using LexRecord = LexRecord;


    public class CheckNominalizations

    {
        public static bool CheckContent(LexRecord lexRecord)

        {
            bool spaceFlag = CheckCont.CheckContent.CheckSpacesForList(lexRecord, 6);


            bool dpFlag = CheckCont.CheckContent.CheckDoublePipesForList(lexRecord, 6);


            bool catFlag = CheckNomCat(lexRecord);
            bool validFlag = (spaceFlag) && (dpFlag) && (catFlag);
            return validFlag;
        }

        public static bool CheckContents(LexRecord lexRecord)

        {
            bool dupFlag = CheckCont.CheckContent.CheckDuplicatesForList(lexRecord, 6);

            bool validFlag = dupFlag;
            return validFlag;
        }

        private static bool CheckNomCat(LexRecord lexRecord)
        {
            bool validFlag = true;
            string cat = lexRecord.GetCategory();
            List<string> nomList = lexRecord.GetNominalizations();


            foreach (string nom in nomList)

            {
                if (!IsLegalCat(nom, cat))

                {
                    validFlag = false;
                    ErrMsgUtilLexRecord.AddContentErrMsg(6, 10, nom, lexRecord);
                }
            }

            return validFlag;
        }

        private static bool IsLegalCat(string nom, string cat)
        {
            bool validFlag = true;

            int index1 = nom.IndexOf("|", StringComparison.Ordinal);
            int index2 = nom.IndexOf("|", index1 + 1, StringComparison.Ordinal);
            int index3 = nom.IndexOf("|", index2 + 1, StringComparison.Ordinal);
            string nomCat = null;
            if (index1 == -1)

            {
                validFlag = false;
            }
            else if (index2 == -1)

            {
                nomCat = nom.Substring(index1 + 1);
            }
            else if (index3 == -1)

            {
                nomCat = nom.Substring(index1 + 1, index2 - (index1 + 1));
            }
            else

            {
                validFlag = false;
            }

            string noun = LexRecordUtil.GetCategory(7);
            string adj = LexRecordUtil.GetCategory(0);
            string verb = LexRecordUtil.GetCategory(10);
            if (!ReferenceEquals(nomCat, null))

            {
                if (cat.Equals(noun))

                {
                    if ((!nomCat.Equals(adj)) && (!nomCat.Equals(verb)))


                    {
                        validFlag = false;
                    }
                }
                else if (cat.Equals(adj) == true)

                {
                    if (!nomCat.Equals(noun))

                    {
                        validFlag = false;
                    }
                }
                else if (cat.Equals(verb) == true)

                {
                    if (!nomCat.Equals(noun))

                    {
                        validFlag = false;
                    }
                }
            }

            return validFlag;
        }
    }


}