using SimpleNLG.Main.lexicon.util.lexCheck.Lib;

namespace SimpleNLG.Main.lexicon.util.lexCheck.CheckCont
{
    using LexRecord = LexRecord;


    public class CheckAbbreviations

    {
        public static bool CheckContent(LexRecord lexRecord)

        {
            bool spaceFlag = CheckCont.CheckContent.CheckSpacesForList(lexRecord, 7);

            bool validFlag = spaceFlag;
            return validFlag;
        }

        public static bool CheckContents(LexRecord lexRecord)

        {
            bool validFlag = CheckCont.CheckContent.CheckDuplicatesForList(lexRecord, 7);

            return validFlag;
        }
    }


}