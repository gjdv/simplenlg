using System.Collections.Generic;
using CheckFormat = SimpleNLG.Main.lexicon.util.lexCheck.Lib.CheckFormat;

namespace SimpleNLG.Main.lexicon.util.lexCheck.Cat.Adj
{
    using CheckFormat = CheckFormat;


    public class CheckFormatAdjPosition : CheckFormat


    {
        private const int LEGAL_FILLER_NUM = 6;

        public virtual bool IsLegalFormat(string filler)

        {
            bool flag = filler_.Contains(filler);
            return flag;
        }

        private static HashSet<string> filler_ = new HashSet<string>();

        static CheckFormatAdjPosition()

        {
            filler_.Add("attrib(1)");
            filler_.Add("attrib(2)");
            filler_.Add("attrib(3)");
            filler_.Add("attribc");
            filler_.Add("pred");
            filler_.Add("post");
        }
    }
}