using System;

namespace SimpleNLG.Main.lexicon.util.lexCheck.Gram
{
    using CheckFormat = Lib.CheckFormat;


    public class CheckFormatAbbreviation : CheckFormat


    {
        public virtual bool IsLegalFormat(string filler)

        {
            if ((ReferenceEquals(filler, null)) || (filler.Length == 0))

            {
                return false;
            }

            int index = filler.IndexOf("|", StringComparison.Ordinal);
            bool flag = true;

            if (index != filler.LastIndexOf("|", StringComparison.Ordinal))

            {
                return false;
            }

            if (index == -1)

            {
                return true;
            }

            string @base = filler.Substring(0, index);
            string eui = filler.Substring(index + 1);

            flag = (@base.Length > 0) && (checkFormatEui_.IsLegalFormat(eui));

            return flag;
        }

        private static CheckFormatEui checkFormatEui_ = new CheckFormatEui();
    }
}