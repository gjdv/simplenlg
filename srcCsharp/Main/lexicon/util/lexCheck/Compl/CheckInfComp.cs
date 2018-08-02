using System;

namespace SimpleNLG.Main.lexicon.util.lexCheck.Compl
{
    public class CheckInfComp

    {
        private const string KEY_COMPL = "infcomp:";


        public static bool IsLegal(string filler)

        {
            bool flag = true;

            if (!filler.StartsWith("infcomp:", StringComparison.Ordinal))

            {
                return false;
            }

            int index = filler.IndexOf(":", StringComparison.Ordinal);
            if (index < size_ - 1)

            {
                return false;
            }

            string interpretation = filler.Substring(index + 1);
            if (!CheckInterpretation.IsLegal(interpretation))

            {
                return false;
            }

            return flag;
        }


        private static readonly int size_ = "infcomp:".Length;
    }
}