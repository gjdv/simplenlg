using System;

namespace SimpleNLG.Main.lexicon.util.lexCheck.Compl
{
    public class CheckIngComp

    {
        private const string KEY_COMPL = "ingcomp:";


        public static bool IsLegal(string filler)

        {
            bool flag = true;

            if (!filler.StartsWith("ingcomp:", StringComparison.Ordinal))

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


        private static readonly int size_ = "ingcomp:".Length;
    }
}