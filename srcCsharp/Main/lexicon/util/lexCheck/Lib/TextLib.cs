using System.Collections.Generic;

namespace SimpleNLG.Main.lexicon.util.lexCheck.Lib
{
    public class TextLib

    {
        public static string AddToText(string orgText, string slot, List<string> fillers, int numIndent)

        {
            if (fillers == null)

            {
                return orgText;
            }

            string @out = orgText;
            for (int i = 0; i < fillers.Count; i++)

            {
                @out = @out + GetIndent(numIndent) + slot + (string) fillers[i] + LS;
            }

            return @out;
        }


        public static string AddToText(string orgText, string slot, string filler, int numIndent)

        {
            string @out = orgText;
            if (!ReferenceEquals(filler, null))

            {
                @out = @out + GetIndent(numIndent) + ConvertNullToNewStr(slot) + ConvertNullToNewStr(filler) + LS;
            }

            return @out;
        }


        public static string AddToText(string orgText, string slot, bool flag, int numIndent)

        {
            string @out = orgText;
            if (flag == true)

            {
                @out = @out + GetIndent(numIndent) + ConvertNullToNewStr(slot) + LS;
            }

            return @out;
        }

        private static string ConvertNullToNewStr(string inStr)

        {
            if (!ReferenceEquals(inStr, null))

            {
                return inStr;
            }

            return "";
        }

        private static string GetIndent(int numIndent)
        {
            string indentStr = "";
            for (int i = 0; i < numIndent; i++)

            {
                indentStr = indentStr + GlobalVars.GetTextIndent();
            }

            return indentStr;
        }

        private static readonly string LS = GlobalVars.LS_STR;
    }
}