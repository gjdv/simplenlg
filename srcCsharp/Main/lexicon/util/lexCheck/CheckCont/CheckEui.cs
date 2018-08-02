using SimpleNLG.Main.lexicon.util.lexCheck.Lib;

namespace SimpleNLG.Main.lexicon.util.lexCheck.CheckCont
{
    using LexRecord = LexRecord;


    public class CheckEui

    {
        public static bool CheckContent(LexRecord lexRecord)

        {
            bool validFlag = CheckIllegalEui(lexRecord);
            return validFlag;
        }

        private static bool CheckIllegalEui(LexRecord lexRecord)

        {
            bool validFlag = true;
            string eui = lexRecord.GetEui();

            if (eui.Equals("E0000000") == true)

            {
                validFlag = false;
                ErrMsgUtilLexRecord.AddContentErrMsg(4, 1, eui, lexRecord);
            }

            return validFlag;
        }
    }


}