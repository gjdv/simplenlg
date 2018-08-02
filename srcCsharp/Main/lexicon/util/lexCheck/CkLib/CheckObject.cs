using System;
using System.Collections.Generic;
using SimpleNLG.Main.lexicon.util.lexCheck.Lib;

namespace SimpleNLG.Main.lexicon.util.lexCheck.CkLib
{
    using CheckFormat = CheckFormat;


    public class CheckObject

    {
        public CheckObject(string startStr, int startErrMsg, int fillerErrMsg, int nextState,
            HashSet<string> nextStartStrs, CheckFormat fillerFormat)

        {
            startStr_ = startStr;
            delim_ = "=";
            startErrMsg_ = startErrMsg;
            fillerErrMsg_ = fillerErrMsg;
            nextState_ = nextState;
            nextStartStrs_ = nextStartStrs;
            nextLine_ = null;
            fillerFormat_ = fillerFormat;
            if (!ReferenceEquals(startStr, null))

            {
                isTab_ = startStr.StartsWith("\t", StringComparison.Ordinal);
            }
        }


        public CheckObject(string startStr, int startErrMsg, int fillerErrMsg, int nextState,
            HashSet<string> nextStartStrs, HashSet<string> nextLine, CheckFormat fillerFormat) : this(startStr,
            startErrMsg, fillerErrMsg, nextState, nextStartStrs, fillerFormat)

        {
            nextLine_ = nextLine;
        }


        public CheckObject(string startStr, int startErrMsg, int fillerErrMsg, int nextState,
            HashSet<string> nextStartStrs, CheckFormat fillerFormat, string delim) : this(startStr, startErrMsg,
            fillerErrMsg, nextState, nextStartStrs, fillerFormat)

        {
            delim_ = delim;
        }


        public CheckObject(string startStr, int startErrMsg, int fillerErrMsg, int nextState,
            HashSet<string> nextStartStrs, HashSet<string> nextLine, CheckFormat fillerFormat, string delim) : this(
            startStr, startErrMsg, fillerErrMsg, nextState, nextStartStrs, nextLine, fillerFormat)

        {
            delim_ = delim;
        }


        public virtual string GetStartStr()

        {
            return startStr_;
        }


        public virtual string GetDelim()

        {
            return delim_;
        }


        public virtual int GetStartErrMsg()

        {
            return startErrMsg_;
        }


        public virtual int GetFillerErrMsg()

        {
            return fillerErrMsg_;
        }


        public virtual int GetNextState()

        {
            return nextState_;
        }


        public virtual HashSet<string> GetNextStartStrs()

        {
            return nextStartStrs_;
        }


        public virtual HashSet<string> GetNextLine()

        {
            return nextLine_;
        }


        public virtual bool IsNextStartStr(string line)

        {
            bool flag = false;
            for (IEnumerator<string> it = nextStartStrs_.GetEnumerator(); it.MoveNext();)

            {
                string itNext = (string) it.Current;
                if (line.StartsWith(itNext, StringComparison.Ordinal) == true)

                {
                    flag = true;


                    if (nextLine_ == null)
                    {
                        break;
                    }

                    for (IEnumerator<string> it2 = nextLine_.GetEnumerator(); it2.MoveNext();)

                    {
                        string it2Next = (string) it2.Current;
                        if ((line.StartsWith(it2Next, StringComparison.Ordinal) == true) && (!line.Equals(it2Next)))


                        {
                            flag = false;
                            break;
                        }
                    }

                    break;
                }
            }


            return flag;
        }


        public virtual CheckFormat GetFillerFormat()

        {
            return fillerFormat_;
        }


        public virtual bool IsTab()

        {
            return isTab_;
        }

        private string startStr_ = null;
        private string delim_ = "=";
        private int startErrMsg_ = -1;
        private int fillerErrMsg_ = -1;
        private int nextState_ = 0;
        private HashSet<string> nextStartStrs_ = null;
        private HashSet<string> nextLine_ = null;
        private CheckFormat fillerFormat_ = null;
        private bool isTab_ = false;
    }
}