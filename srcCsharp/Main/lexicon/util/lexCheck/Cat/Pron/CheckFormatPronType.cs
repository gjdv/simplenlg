using System.Collections.Generic;
using CheckFormat = SimpleNLG.Main.lexicon.util.lexCheck.Lib.CheckFormat;

namespace SimpleNLG.Main.lexicon.util.lexCheck.Cat.Pron
{
    using CheckFormat = CheckFormat;


    public class CheckFormatPronType : CheckFormat


    {
        private const int LEGAL_FILLER_NUM = 10;

        public virtual bool IsLegalFormat(string filler)

        {
            bool flag = filler_.Contains(filler);
            return flag;
        }

        private static HashSet<string> filler_ = new HashSet<string>();

        static CheckFormatPronType()

        {
            filler_.Add("obj");
            filler_.Add("subj");
            filler_.Add("poss");
            filler_.Add("possnom");
            filler_.Add("refl");
            filler_.Add("univ");
            filler_.Add("indef(neg)");
            filler_.Add("indef(assert)");
            filler_.Add("indef(nonassert)");
            filler_.Add("dem");
        }
    }
}