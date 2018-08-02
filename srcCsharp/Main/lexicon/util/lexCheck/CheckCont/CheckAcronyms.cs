using SimpleNLG.Main.lexicon.util.lexCheck.Lib;

namespace SimpleNLG.Main.lexicon.util.lexCheck.CheckCont
{
    using LexRecord = LexRecord;


    public class CheckAcronyms

    {
        public static bool CheckContent(LexRecord lexRecord)

        {
            bool spaceFlag = CheckCont.CheckContent.CheckSpacesForList(lexRecord, 8);

            bool validFlag = spaceFlag;
            return validFlag;
        }

        public static bool CheckContents(LexRecord lexRecord)

        {
            bool dupFlag = CheckCont.CheckContent.CheckDuplicatesForList(lexRecord, 8);

            bool validFlag = dupFlag;
            return validFlag;
        }
    }


}