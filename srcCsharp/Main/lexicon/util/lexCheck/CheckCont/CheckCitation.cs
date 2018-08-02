using System.Collections.Generic;
using SimpleNLG.Main.lexicon.util.lexCheck.Lib;

namespace SimpleNLG.Main.lexicon.util.lexCheck.CheckCont
{
    using LexRecord = LexRecord;


    public class CheckCitation

    {
        public static bool CheckContent(LexRecord lexRecord)

        {
            bool spaceFlag = CheckCont.CheckContent.CheckSpacesForItem(lexRecord, 1);

            bool validFlag = spaceFlag;
            return validFlag;
        }

        public static bool CheckContents(LexRecord lexRecord)

        {
            bool orderFlag = CheckOrder(lexRecord);
            bool glregFlag = CheckGlreg(lexRecord);
            bool regdFlag = CheckRegd(lexRecord);
            bool validFlag = (orderFlag) && (glregFlag) && (regdFlag);
            return validFlag;
        }

        private static bool CheckRegd(LexRecord lexRecord)

        {
            string @base = lexRecord.GetBase();
            bool validFlag = CheckEntry.CheckRegd(lexRecord, @base, 1);

            return validFlag;
        }

        private static bool CheckGlreg(LexRecord lexRecord)

        {
            string @base = lexRecord.GetBase();
            bool validFlag = CheckEntry.CheckGlreg(lexRecord, @base, 1);

            return validFlag;
        }

        private static bool CheckOrder(LexRecord lexRecord)

        {
            bool validFlag = true;
            int contentType = 1;

            string citation = lexRecord.GetBase();
            List<string> spVars = lexRecord.GetSpellingVars();
            List<string> bases = new List<string>();
            bases.Add(citation);
            foreach (string spVar in spVars)

            {
                bases.Add(spVar);
            }

            BaseComparator<string> bc = new BaseComparator<string>();
            bases.Sort(bc);

            if (!citation.Equals(bases[0]))

            {
                lexRecord.SetBase((string) bases[0]);
                validFlag = false;
            }

            bases.RemoveAt(0);
            lexRecord.SetSpellingVars(bases);

            if (!validFlag)

            {
                ErrMsgUtilLexRecord.AddContentErrMsg(contentType, 6, citation, lexRecord);
            }

            return validFlag;
        }
    }


}