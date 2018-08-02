using System;
using System.Collections.Generic;
using SimpleNLG.Main.lexicon.util.lexCheck.Compl;
using CheckFormat = SimpleNLG.Main.lexicon.util.lexCheck.Lib.CheckFormat;

namespace SimpleNLG.Main.lexicon.util.lexCheck.Cat.Verb
{
    using CheckEdComp = CheckEdComp;
    using CheckFinComp = CheckFinComp;
    using CheckInfComp = CheckInfComp;
    using CheckIngComp = CheckIngComp;
    using CheckParticle = CheckParticle;
    using CheckPphr = CheckPphr;
    using CheckFormat = CheckFormat;


    public class CheckFormatVerbLink : CheckFormat


    {
        private const int LEGAL_FILLER_NUM = 3;

        public virtual bool IsLegalFormat(string inCode)

        {
            bool flag = false;
            int partIndex = inCode.IndexOf(";part(", StringComparison.Ordinal);
            string filler = null;
            string particle = null;

            if (partIndex != -1)

            {
                filler = inCode.Substring(0, partIndex);
                particle = inCode.Substring(partIndex);
            }
            else

            {
                filler = inCode;
            }

            if (!ReferenceEquals(particle, null))

            {
                flag = (CheckLinkFiller(filler)) && (CheckParticle.IsLegal(particle));
            }
            else

            {
                flag = CheckLinkFiller(filler);
            }

            return flag;
        }

        private bool CheckLinkFiller(string filler)

        {
            bool flag = false;
            if (filler_.Contains(filler) == true)

            {
                flag = true;
            }
            else if (filler.StartsWith("edcomp:", StringComparison.Ordinal) == true)

            {
                flag = CheckEdComp.IsLegal(filler);
            }
            else if (filler.StartsWith("fincomp(", StringComparison.Ordinal) == true)

            {
                flag = CheckFinComp.IsLegal(filler);
            }
            else if (filler.StartsWith("infcomp:", StringComparison.Ordinal) == true)

            {
                flag = CheckInfComp.IsLegal(filler);
            }
            else if (filler.StartsWith("ingcomp:", StringComparison.Ordinal) == true)

            {
                flag = CheckIngComp.IsLegal(filler);
            }
            else if (filler.StartsWith("pphr(", StringComparison.Ordinal) == true)

            {
                flag = CheckPphr.IsLegal(filler);
            }

            return flag;
        }

        private static HashSet<string> filler_ = new HashSet<string>();

        static CheckFormatVerbLink()

        {
            filler_.Add("adj");
            filler_.Add("advbl");
            filler_.Add("np");
        }
    }
}