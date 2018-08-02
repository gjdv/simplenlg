using System.Collections.Generic;
using CheckFormat = SimpleNLG.Main.lexicon.util.lexCheck.Lib.CheckFormat;

namespace SimpleNLG.Main.lexicon.util.lexCheck.Cat.Pron
{
    using CheckFormat = CheckFormat;


    public class CheckFormatPronVariants : CheckFormat


    {
        private const int LEGAL_FILLER_NUM = 9;

        public virtual bool IsLegalFormat(string filler)

        {
            bool flag = filler_.Contains(filler);
            return flag;
        }

        private static HashSet<string> filler_ = new HashSet<string>();

        static CheckFormatPronVariants()

        {
            filler_.Add("fst_plur");
            filler_.Add("fst_sing");
            filler_.Add("sec_plur");
            filler_.Add("sec_sing");
            filler_.Add("second");
            filler_.Add("third");
            filler_.Add("thr_plur");
            filler_.Add("thr_sing");
            filler_.Add("free");
        }
    }
}