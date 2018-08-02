using System;
using System.Linq;
using SimpleNLG.Main.lexicon.util.lexCheck.Lib;

namespace SimpleNLG.Main.lexicon.util.lexCheck.CkLib
{
    using CheckFillerFormat = CheckFillerFormat;
    using CheckFormat = CheckFormat;
    using ErrMsg = ErrMsg;
    using LineObject = LineObject;
    using TokenObject = TokenObject;

    public class LineCheck
    {
        public static bool CheckStartStr(LineObject lineObject, bool printFlag, CheckObject checkObject)
        {
            string line = lineObject.GetLine();
            string startStr = checkObject.GetStartStr();
            bool flag = line.StartsWith(startStr, StringComparison.Ordinal);
            bool isTab = checkObject.IsTab();
            int beginIndex = 0;
            int endIndex = startStr.Length - 1;
            if (isTab == true)
            {
                beginIndex = 1;
            }

            if (!flag)
            {
                ErrMsg.PrintErrMsg(printFlag, checkObject.GetStartErrMsg(), lineObject, null, beginIndex, endIndex,
                    isTab);
            }

            return flag;
        }

        public static bool CheckWholeLine(LineObject lineObject, bool printFlag, CheckObject checkObject)
        {
            string line = lineObject.GetLine();
            string startStr = checkObject.GetStartStr();
            bool flag = line.Equals(startStr);
            bool isTab = checkObject.IsTab();
            int beginIndex = 0;
            int endIndex = startStr.Length;
            if (isTab == true)
            {
                beginIndex = 1;
            }

            if (!flag)
            {
                ErrMsg.PrintErrMsg(printFlag, checkObject.GetStartErrMsg(), lineObject, null, beginIndex, endIndex,
                    isTab);
            }

            return flag;
        }

        public static string GetStartStr(LineObject lineObject, string delim)
        {
            string line = lineObject.GetLine();
            string[] buf = line.Split(delim.ToCharArray()).ToList().Where(x => x != "").ToArray();
            string startStr = buf.Length > 0 ? buf[buf.Length - 1] : "";

            return startStr;
        }

        public static bool CheckSlotFiller(LineObject lineObject, bool printFlag, CheckObject checkObject,
            TokenObject tokenObject, bool checkLength)
        {
            string line = lineObject.GetLine();
            bool isTab = checkObject.IsTab();
            string slot = null;
            string filler = null;
            string delim = checkObject.GetDelim();
            int index = line.IndexOf(delim, StringComparison.Ordinal);
            if (index != -1)
            {
                if (ReferenceEquals(delim, "="))
                {
                    slot = line.Substring(0, index + 1);
                    filler = line.Substring(index + 1);
                }
                else
                {
                    slot = line.Substring(0, index + 1);
                    filler = line;
                }
            }
            else
            {
                ErrMsg.PrintErrMsg(printFlag, 80, lineObject, line, 0, line.Length, isTab);
                return false;
            }

            bool flag = false;
            string startStr = checkObject.GetStartStr();
            if (!ReferenceEquals(startStr, null))
            {
                flag = slot.Equals(startStr);
            }

            if (!flag)
            {
                ErrMsg.PrintErrMsg(printFlag, checkObject.GetStartErrMsg(), lineObject, slot, 0, index, isTab);
                return flag;
            }

            flag = CheckFillerFormat.IsLegalFormat(printFlag, lineObject, filler, index, isTab, checkLength);
            if (!flag)
            {
                return false;
            }

            CheckFormat fillerFormat = checkObject.GetFillerFormat();
            if (fillerFormat != null)
            {
                flag = fillerFormat.IsLegalFormat(filler);
                if (!flag)
                {
                    ErrMsg.PrintErrMsg(printFlag, checkObject.GetFillerErrMsg(), lineObject, filler, index + 1,
                        index + filler.Length, isTab);
                    return flag;
                }
            }

            if ((flag == true) && (tokenObject != null))
            {
                tokenObject.SetToken(filler);
            }

            return flag;
        }
    }
}