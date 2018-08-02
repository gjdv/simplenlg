using SimpleNLG.Main.lexicon.util.lexCheck.Lib;

namespace SimpleNLG.Main.lexicon.util.lexCheck.CheckCont
{
    using LexRecord = LexRecord;


    public class ErrMsgUtil

    {
        public static void ResetErrMsg()

        {
            errMsg_ = "";
        }


        public static void SetErrMsg(string errMsg)

        {
            errMsg_ = errMsg;
        }


        public static void AppendErrMsg(string errMsg)

        {
            errMsg_ += errMsg;
        }


        public static void AddContentErrMsg(int contentType, int errType, string msg, string[][] errTypeStrs,
            int[] errTypeNos, bool crossRefFlag)

        {
            string contentTypeStr = LexRecordUtil.GetContentTypeStr(contentType);
            if (crossRefFlag == true)

            {
                contentTypeStr = LexRecordUtil.GetCrossRefTypeStr(contentType);
            }

            string errTypeStr = GetErrTypeStr(errType, errTypeStrs);
            string errFixStr = GetErrFixStr(errType, errTypeStrs);

            string errMsg = "** Content Err in " + contentTypeStr + " - " + errTypeStr + " @ [" + msg + "]" +
                            errFixStr + GlobalVars.LS_STR;


            errMsg_ += errMsg;

            AddErrTypeNo(errType, errTypeNos);
        }


        public static void AddContentErrMsg(int contentType, int errType, string curItem, LexRecord lexRecord,
            string[][] errTypeStrs, int[] errTypeNos, bool crossRefFlag)

        {
            string contentTypeStr = LexRecordUtil.GetContentTypeStr(contentType);
            if (crossRefFlag == true)

            {
                contentTypeStr = LexRecordUtil.GetCrossRefTypeStr(contentType);
            }

            string errTypeStr = GetErrTypeStr(errType, errTypeStrs);
            string errFixStr = GetErrFixStr(errType, errTypeStrs);


            string errMsg = "** Content Err in " + contentTypeStr + " - " + errTypeStr + " (" + curItem + "): @ [" +
                            LexRecordUtil.GetLexRecordInfo(lexRecord) + "]" + errFixStr + GlobalVars.LS_STR;


            if (errType == 9)

            {
                errMsg = errMsg + "=> Add EUI (" + lexRecord.GetEui() +
                         ") to irregExcetions.data if this Err is an OK exception." + GlobalVars.LS_STR;
            }


            errMsg_ += errMsg;

            AddErrTypeNo(errType, errTypeNos);
        }


        public static string GetErrMsg()

        {
            return errMsg_;
        }


        public static string GetErrStats(string[][] errTypeStrs, int[] errTypeNos)

        {
            string errStatsStr = "";
            for (int i = 1; i < errTypeNos.Length; i++)

            {
                errStatsStr = errStatsStr + i + ". " + errTypeStrs[i][0] + ": " + errTypeNos[i] + GlobalVars.LS_STR;
            }

            errStatsStr = errStatsStr + "---------------------------" + GlobalVars.LS_STR;
            errStatsStr = errStatsStr + errTypeStrs[0][0] + ": " + errTypeNos[0] + GlobalVars.LS_STR;

            return errStatsStr;
        }

        private static void AddErrTypeNo(int errType, int[] errTypeNos)

        {
            if ((errType > 0) && (errType < errTypeNos.Length))

            {
                errTypeNos[errType] += 1;
                errTypeNos[0] += 1;
            }
        }

        protected internal static string GetErrTypeStr(int errType, string[][] errTypeStrs)
        {
            string errTypeStr = errTypeStrs[0][0];
            if ((errType >= 0) && (errType < errTypeStrs.Length))

            {
                errTypeStr = errTypeStrs[errType][0];
            }

            return errTypeStr;
        }

        protected internal static string GetErrFixStr(int errType, string[][] errTypeStrs)
        {
            string errTypeStr = errTypeStrs[0][1];
            if ((errType >= 0) && (errType < errTypeStrs.Length))

            {
                errTypeStr = errTypeStrs[errType][1];
            }

            return errTypeStr;
        }

        protected internal static string errMsg_ = "";
        private const int ERR = 0;
        private const int FIX = 1;
        public const int ERR_NONE = 0;
        public const int ERR_TOTAL = 0;
    }


}