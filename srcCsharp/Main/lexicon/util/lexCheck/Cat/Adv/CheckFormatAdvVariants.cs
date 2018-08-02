using System;
using System.Collections.Generic;
using System.Linq;
using CheckFormat = SimpleNLG.Main.lexicon.util.lexCheck.Lib.CheckFormat;

namespace SimpleNLG.Main.lexicon.util.lexCheck.Cat.Adv
{
    using CheckFormat = CheckFormat;

    public class CheckFormatAdvVariants : CheckFormat
    {
        private const int LEGAL_FILLER_NUM = 4;

        public virtual bool IsLegalFormat(string filler)
        {
            bool flag = filler_.Contains(filler);
            if ((!flag) && (filler.StartsWith("irreg|", StringComparison.Ordinal)))
            {
                string[] buf = filler.Split('|').ToList().Where(x => x != "").ToArray();
                int pipeNum = 0;
                foreach (string token in buf)
                {
                    pipeNum++;
                }

                flag = pipeNum == 4;
            }

            return flag;
        }

        private static HashSet<string> filler_ = new HashSet<string>();

        static CheckFormatAdvVariants()
        {
            filler_.Add("reg");
            filler_.Add("irreg");
            filler_.Add("inv");
            filler_.Add("inv;periph");
        }
    }
}