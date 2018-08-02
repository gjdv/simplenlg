using System.Collections.Generic;
using SimpleNLG.Main.lexicon.util.lexCheck.Lib;

namespace SimpleNLG.Main.lexicon.util.lexCheck.CheckCont
{
    using LexRecord = LexRecord;


    public class CheckSpellingVars

    {
        public static bool CheckContent(LexRecord lexRecord)

        {
            bool spaceFlag = CheckCont.CheckContent.CheckSpacesForList(lexRecord, 2);

            bool validFlag = spaceFlag;
            return validFlag;
        }

        public static bool CheckContents(LexRecord lexRecord)

        {
            bool dupFlag = CheckDuplicates(lexRecord);
            bool glregFlag = CheckGlreg(lexRecord);
            bool regdFlag = CheckRegd(lexRecord);
            bool validFlag = (dupFlag) && (glregFlag) && (regdFlag);
            return validFlag;
        }

        private static bool CheckRegd(LexRecord lexRecord)

        {
            List<string> spList = lexRecord.GetSpellingVars();
            bool validFlag = true;
            foreach (string spItem in spList)

            {
                bool regdFlag = CheckEntry.CheckRegd(lexRecord, spItem, 2);

                validFlag = (regdFlag) && (validFlag);
            }

            return validFlag;
        }

        private static bool CheckGlreg(LexRecord lexRecord)

        {
            List<string> spList = lexRecord.GetSpellingVars();
            bool validFlag = true;
            foreach (string spItem in spList)

            {
                bool glregFlag = CheckEntry.CheckGlreg(lexRecord, spItem, 2);

                validFlag = (glregFlag) && (validFlag);
            }

            return validFlag;
        }

        private static bool CheckDuplicates(LexRecord lexRecord)

        {
            bool validFlag = true;
            int contentType = 2;
            string @base = lexRecord.GetBase();
            List<string> svList = lexRecord.GetSpellingVars();
            List<string> uSvList = new List<string>();

            for (int i = 0; i < svList.Count; i++)

            {
                string sv = (string) svList[i];

                if ((sv.Equals(@base) == true) || (uSvList.Contains(sv) == true))


                {
                    validFlag = false;
                    ErrMsgUtilLexRecord.AddContentErrMsg(contentType, 2, sv, lexRecord);
                }
                else

                {
                    uSvList.Add(sv);
                }
            }

            if (!validFlag)

            {
                lexRecord.SetSpellingVars(uSvList);
            }

            return validFlag;
        }
    }


}