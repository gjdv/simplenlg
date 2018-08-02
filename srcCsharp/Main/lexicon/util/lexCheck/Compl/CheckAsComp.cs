using System;

namespace SimpleNLG.Main.lexicon.util.lexCheck.Compl
{
    public class CheckAsComp

    {
        private const string KEY_COMPL = "ascomp:";


        public static bool IsLegal(string filler)

        {
            bool flag = true;

            if (!filler.StartsWith("ascomp:", StringComparison.Ordinal))

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


        private static readonly int size_ = "ascomp:".Length;
    }
}