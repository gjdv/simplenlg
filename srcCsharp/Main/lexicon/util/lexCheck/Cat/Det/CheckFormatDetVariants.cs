using System.Collections.Generic;
using CheckFormat = SimpleNLG.Main.lexicon.util.lexCheck.Lib.CheckFormat;

namespace SimpleNLG.Main.lexicon.util.lexCheck.Cat.Det
{
    using CheckFormat = CheckFormat;


    public class CheckFormatDetVariants : CheckFormat


    {
        private const int LEGAL_FILLER_NUM = 6;

        public virtual bool IsLegalFormat(string filler)

        {
            bool flag = filler_.Contains(filler);
            return flag;
        }

        private static HashSet<string> filler_ = new HashSet<string>();

        static CheckFormatDetVariants()

        {
            filler_.Add("sing");
            filler_.Add("plur");
            filler_.Add("uncount");
            filler_.Add("singuncount");
            filler_.Add("pluruncount");
            filler_.Add("free");
        }
    }
}