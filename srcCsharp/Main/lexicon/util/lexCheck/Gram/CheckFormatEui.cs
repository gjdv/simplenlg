using System;

namespace SimpleNLG.Main.lexicon.util.lexCheck.Gram
{
    using CheckFormat = Lib.CheckFormat;


    public class CheckFormatEui : CheckFormat


    {
        public static bool IsValidEui(string eui)

        {
            CheckFormatEui temp = new CheckFormatEui();
            if ((ReferenceEquals(eui, null)) || (eui.Length == 0))

            {
                return false;
            }

            return temp.IsLegalFormat(eui);
        }


        public virtual bool IsLegalFormat(string filler)

        {
            bool flag = true;


            flag = (filler.StartsWith("E", StringComparison.Ordinal)) && (filler.Length == 8) &&
                   (IsAllDigit(filler.Substring(1)));
            return flag;
        }

        private static bool IsAllDigit(string euiId)

        {
            for (int i = 0; i < euiId.Length; i++)

            {
                char c = euiId[i];
                if (!char.IsDigit(c))

                {
                    return false;
                }
            }

            return true;
        }
    }
}