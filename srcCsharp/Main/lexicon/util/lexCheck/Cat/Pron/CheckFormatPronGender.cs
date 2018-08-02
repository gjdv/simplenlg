using System.Collections.Generic;
using CheckFormat = SimpleNLG.Main.lexicon.util.lexCheck.Lib.CheckFormat;

namespace SimpleNLG.Main.lexicon.util.lexCheck.Cat.Pron
{
    using CheckFormat = CheckFormat;


    public class CheckFormatPronGender : CheckFormat


    {
        private const int LEGAL_FILLER_NUM = 4;

        public virtual bool IsLegalFormat(string filler)

        {
            bool flag = filler_.Contains(filler);
            return flag;
        }

        private static HashSet<string> filler_ = new HashSet<string>();

        static CheckFormatPronGender()

        {
            filler_.Add("pers(masc)");
            filler_.Add("pers(fem)");
            filler_.Add("pers");
            filler_.Add("neut");
        }
    }
}