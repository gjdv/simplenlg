using SimpleNLG.Main.lexicon.util.lexCheck.Lib;

namespace SimpleNLG.Main.lexicon.util.lexCheck.CheckCont
{
    using LexRecord = LexRecord;


    public class ErrMsgUtilLexicon : ErrMsgUtil


    {
        public const int ERR_DUP_EUI = 1;
        public const int ERR_DUP_REC = 2;
        public const int ERR_NO_EUI = 3;
        public const int ERR_NEW_EUI = 4;
        public const int ERR_MISS_EUI = 5;
        public const int ERR_WRONG_EUI = 6;
        public const int ERR_MISS_EUIS = 7;
        public const int ERR_WRONG_EUIS = 8;
        public const int ERR_SYM_CIT = 9;
        public const int ERR_SYM_CAT = 10;
        public const int ERR_SYM_NONE = 11;

        public static void AddContentErrMsg(int contentType, int errType, string curItem, LexRecord lexRecord)

        {
            bool crossRefFlag = true;
            AddContentErrMsg(contentType, errType, curItem, lexRecord, errTypeStrs_, errTypeNos_, crossRefFlag);
        }

        public static void AddContentErrMsg(int contentType, int errType, string errMsg)

        {
            bool crossRefFlag = true;
            AddContentErrMsg(contentType, errType, errMsg, errTypeStrs_, errTypeNos_, crossRefFlag);
        }

        public static string GetErrStats()

        {
            return GetErrStats(errTypeStrs_, errTypeNos_);
        }

        public static string GetErrTypeStr(int errType)
        {
            return GetErrTypeStr(errType, errTypeStrs_);
        }

        public static string GetErrFixStr(int errType)
        {
            return GetErrFixStr(errType, errTypeStrs_);
        }


        private static string[][] errTypeStrs_ = new string[][]
        {
            new string[] {"Total", ""},
            new string[] {"dup EUI", " => requires manully fix"},
            new string[] {"dup LexRecord", " => requires manually check"},
            new string[] {"no EUI", " => remove EUI, change citation, etc."},
            new string[] {"new EUI", " => Manually add a new record to To-Do list"},
            new string[] {"missing EUI", " => add EUI"},
            new string[] {"wrong EUI", " => requires manually check"},
            new string[] {"missing EUIs", " => requires manually check"},
            new string[] {"wrong EUIs", " => requires manually check"},
            new string[] {"symmetric citation", " => requires manually check"},
            new string[] {"symmetric catogory", " => requires manually check"},
            new string[] {"symmetric none", " => requires manually check"}
        };


        private static int[] errTypeNos_ = new int[] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
    }


}