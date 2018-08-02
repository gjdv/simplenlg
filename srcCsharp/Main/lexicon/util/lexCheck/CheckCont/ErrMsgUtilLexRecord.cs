using SimpleNLG.Main.lexicon.util.lexCheck.Lib;

namespace SimpleNLG.Main.lexicon.util.lexCheck.CheckCont
{
    using LexRecord = LexRecord;


    public class ErrMsgUtilLexRecord : ErrMsgUtil


    {
        public const int ERR_ILLEGAL = 1;
        public const int ERR_DUPLICATE = 2;
        public const int ERR_DOUBLE_PIPES = 3;
        public const int ERR_GLREG_ENDING = 4;
        public const int ERR_REGD_ENDING = 5;
        public const int ERR_ORDER = 6;
        public const int ERR_SPACES = 7;
        public const int ERR_IRREG_BASE = 8;
        public const int ERR_IRREG_MISSING = 9;
        public const int ERR_ILLEGAL_NOM_CAT = 10;

        public static void AddContentErrMsg(int contentType, int errType, string curItem, LexRecord lexRecord)

        {
            bool crossRefFlag = false;
            AddContentErrMsg(contentType, errType, curItem, lexRecord, errTypeStrs_, errTypeNos_, crossRefFlag);
        }

        public static string GetErrStats()

        {
            return GetErrStats(errTypeStrs_, errTypeNos_);
        }


        private static string[][] errTypeStrs_ = new string[][]
        {
            new string[] {"Total errors", ""},
            new string[] {"illegal value", ""},
            new string[] {"duplicates", " => fixed (remove duplicates)"},
            new string[] {"double pipes ||", ""},
            new string[] {"illegal glreg ending", ""},
            new string[] {"illegal regd ending", ""},
            new string[] {"order", " => fixed (swap citation & spVar)"},
            new string[] {"extra spaces", " => fixed (trim)"},
            new string[] {"illegal irreg base", ""},
            new string[] {"missing in irreg", ""},
            new string[] {"Illegal cat in nom", " => requires manually fix"}
        };


        private static int[] errTypeNos_ = new int[] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
    }


}