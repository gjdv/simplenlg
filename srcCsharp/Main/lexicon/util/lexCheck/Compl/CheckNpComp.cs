using System;

namespace SimpleNLG.Main.lexicon.util.lexCheck.Compl
{
    public class CheckNpComp

    {
        private const string KEY_COMPL = "np|";


        public static bool IsLegal(string filler)

        {
            bool flag = true;

            if ((!filler.StartsWith("np|", StringComparison.Ordinal)) ||
                (!filler.EndsWith("|", StringComparison.Ordinal)))


            {
                return false;
            }

            if (filler.IndexOf("|", StringComparison.Ordinal) + 1 == filler.LastIndexOf("|", StringComparison.Ordinal))

            {
                return false;
            }

            return flag;
        }


        private static readonly int size_ = "np|".Length;
    }
}