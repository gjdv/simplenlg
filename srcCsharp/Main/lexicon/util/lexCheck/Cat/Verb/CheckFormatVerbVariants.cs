using System;
using System.Collections.Generic;
using System.Linq;
using CheckFormat = SimpleNLG.Main.lexicon.util.lexCheck.Lib.CheckFormat;

namespace SimpleNLG.Main.lexicon.util.lexCheck.Cat.Verb
{
    using CheckFormat = CheckFormat;

    public class CheckFormatVerbVariants : CheckFormat
    {
        private const int LEGAL_FILLER_NUM = 3;

        public virtual bool IsLegalFormat(string filler)
        {
            bool flag = filler_.Contains(filler);
            if ((!flag) && (filler.StartsWith("irreg|", StringComparison.Ordinal)))
            {
                int pipeNum = filler.Count(x => x == '|');
                
                flag = pipeNum == 6;
            }

            return flag;
        }

        private static HashSet<string> filler_ = new HashSet<string>();

        static CheckFormatVerbVariants()
        {
            filler_.Add("reg");
            filler_.Add("regd");
            filler_.Add("irreg");
        }
    }
}