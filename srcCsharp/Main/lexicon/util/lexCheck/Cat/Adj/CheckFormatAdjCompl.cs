using System;
using System.Collections.Generic;
using SimpleNLG.Main.lexicon.util.lexCheck.Compl;
using CheckFormat = SimpleNLG.Main.lexicon.util.lexCheck.Lib.CheckFormat;

namespace SimpleNLG.Main.lexicon.util.lexCheck.Cat.Adj
{
    using CheckAsComp = CheckAsComp;
    using CheckFinComp = CheckFinComp;
    using CheckInfComp = CheckInfComp;
    using CheckPphr = CheckPphr;
    using CheckWhinfComp = CheckWhinfComp;
    using CheckFormat = CheckFormat;


    public class CheckFormatAdjCompl : CheckFormat


    {
        private const int LEGAL_FILLER_NUM = 8;

        public virtual bool IsLegalFormat(string filler)

        {
            bool flag = false;
            if (filler_.Contains(filler) == true)

            {
                flag = true;
            }
            else if (filler.StartsWith("ascomp:", StringComparison.Ordinal) == true)

            {
                flag = CheckAsComp.IsLegal(filler);
            }
            else if (filler.StartsWith("fincomp(", StringComparison.Ordinal) == true)

            {
                flag = CheckFinComp.IsLegal(filler);
            }
            else if (filler.StartsWith("infcomp:", StringComparison.Ordinal) == true)

            {
                flag = CheckInfComp.IsLegal(filler);
            }
            else if (filler.StartsWith("pphr(", StringComparison.Ordinal) == true)

            {
                flag = CheckPphr.IsLegal(filler);
            }
            else if (filler.StartsWith("binfcomp:", StringComparison.Ordinal) != true)

            {
                if (filler.StartsWith("whinfcomp:", StringComparison.Ordinal) == true)

                {
                    flag = CheckWhinfComp.IsLegal(filler);
                }
                else

                {
                    flag = false;
                }
            }

            return flag;
        }

        private static HashSet<string> filler_ = new HashSet<string>();

        static CheckFormatAdjCompl()

        {
            filler_.Add("advbl");
            filler_.Add("ascomp:");
            filler_.Add("fincomp(");
            filler_.Add("infcomp:");
            filler_.Add("pphr");
            filler_.Add("binfcomp:");
            filler_.Add("whfincomp");
            filler_.Add("whinfcomp:");
        }
    }
}