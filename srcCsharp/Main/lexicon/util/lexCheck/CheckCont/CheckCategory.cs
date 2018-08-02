using SimpleNLG.Main.lexicon.util.lexCheck.Lib;

namespace SimpleNLG.Main.lexicon.util.lexCheck.CheckCont
{
    using LexRecord = LexRecord;


    public class CheckCategory

    {
        public static bool CheckContent(LexRecord lexRecord)

        {
            bool validFlag = CheckIllegalCat(lexRecord);
            return validFlag;
        }

        private static bool CheckIllegalCat(LexRecord lexRecord)

        {
            bool validFlag = true;
            string cat = lexRecord.GetCategory();

            if (!LexRecordUtil.IsCategory(cat))

            {
                validFlag = false;
                ErrMsgUtilLexRecord.AddContentErrMsg(3, 1, cat, lexRecord);
            }

            return validFlag;
        }
    }


}