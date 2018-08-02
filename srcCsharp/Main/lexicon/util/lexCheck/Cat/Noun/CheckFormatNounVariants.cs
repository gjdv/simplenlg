using System;
using System.Collections.Generic;
using System.Linq;
using CheckFormat = SimpleNLG.Main.lexicon.util.lexCheck.Lib.CheckFormat;

namespace SimpleNLG.Main.lexicon.util.lexCheck.Cat.Noun
{
    using CheckFormat = CheckFormat;

    public class CheckFormatNounVariants : CheckFormat
    {
        private const int LEGAL_ARGUMENT_NUM = 5;

        public virtual bool IsLegalFormat(string filler)
        {
            bool flag = filler_.Contains(filler);
            if (!flag)
            {
                if ((filler.StartsWith("irreg|", StringComparison.Ordinal) == true) &&
                    (filler.EndsWith("|", StringComparison.Ordinal) == true))
                {
                    string[] buf = filler.Split('|').ToList().Where(x => x != "").ToArray();
                    int pipeNum = 0;
                    foreach (string token in buf)
                    {
                        pipeNum++;
                    }

                    flag = pipeNum == 3;
                }
                else if (filler.StartsWith("group(", StringComparison.Ordinal) == true)
                {
                    flag = CheckNounGroupArgument(filler);
                }
            }

            return flag;
        }

        private bool CheckNounGroupArgument(string argument)
        {
            bool flag = groupNounArgument_.Contains(argument);
            if (!flag)
            {
                if ((argument.StartsWith("group(irreg|", StringComparison.Ordinal) == true) &&
                    (argument.EndsWith("|)", StringComparison.Ordinal) == true))
                {
                    string[] buf = argument.Split('|').ToList().Where(x => x != "").ToArray();
                    int pipeNum = 0;
                    foreach (string token in buf)
                    {
                        pipeNum++;
                    }

                    flag = pipeNum == 4;
                }
            }

            return flag;
        }

        private static HashSet<string> groupNounArgument_ = new HashSet<string>();
        private const int LEGAL_FILLER_NUM = 11;
        private static HashSet<string> filler_ = new HashSet<string>();

        static CheckFormatNounVariants()
        {
            groupNounArgument_.Add("group(reg)");
            groupNounArgument_.Add("group(irreg)");
            groupNounArgument_.Add("group(glreg)");
            groupNounArgument_.Add("group(metareg)");
            groupNounArgument_.Add("group(sing)");
            filler_.Add("reg");
            filler_.Add("glreg");
            filler_.Add("metareg");
            filler_.Add("irreg|");
            filler_.Add("sing");
            filler_.Add("plur");
            filler_.Add("inv");
            filler_.Add("group()");
            filler_.Add("uncount");
            filler_.Add("groupuncount");
        }
    }
}