using System;
using System.Collections.Generic;

namespace SimpleNLG.Main.lexicon.util.lexAccess.Lib
{
    public class GlobalVars

    {
        public const string YEAR = "2016";

        public static GlobalVars GetInstance()

        {
            lock (typeof(GlobalVars))
            {
                if (instance_ == null)

                {
                    instance_ = new GlobalVars();
                    instance_.Init();
                }

                return instance_;
            }
        }


        public virtual void SetFieldSeparator(string value)

        {
            fieldSeparator_ = value;
        }


        public virtual string GetFieldSeparator()

        {
            return fieldSeparator_;
        }


        public virtual Dictionary<char, string> GetEscapeCharacters()

        {
            return escapeCharacters_;
        }


        public virtual string GetDefaultFieldSeparator()

        {
            return "|";
        }


        private void Init()

        {
            if (escapeCharacters_ == null)

            {
                escapeCharacters_ = new Dictionary<char, string>();

                char c = '\r';
                escapeCharacters_[c] = "\\\r";
                c = '"';
                escapeCharacters_[c] = "\\\"";
                c = '\'';
                escapeCharacters_[c] = "\\'";
                c = '\t';
                escapeCharacters_[c] = "\t";
                c = '\\';
                escapeCharacters_[c] = "\\\\";
            }
        }


        public static readonly string LS_STR = Environment.NewLine;
        private const string FS_STR = "|";
        private string fieldSeparator_ = "|";
        private Dictionary<char, string> escapeCharacters_ = null;
        private static GlobalVars instance_;
    }


}