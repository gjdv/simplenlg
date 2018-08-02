using System;
using System.Collections.Generic;
using SimpleNLG.Main.lexicon.util.lexCheck.Compl;
using CheckFormat = SimpleNLG.Main.lexicon.util.lexCheck.Lib.CheckFormat;

namespace SimpleNLG.Main.lexicon.util.lexCheck.Cat.Verb
{
    using CheckAsComp = CheckAsComp;
    using CheckBinfComp = CheckBinfComp;
    using CheckEdComp = CheckEdComp;
    using CheckFinComp = CheckFinComp;
    using CheckInfComp = CheckInfComp;
    using CheckIngComp = CheckIngComp;
    using CheckParticle = CheckParticle;
    using CheckPphr = CheckPphr;
    using CheckFormat = CheckFormat;


    public class CheckFormatVerbCplxtran : CheckFormat


    {
        private const int LEGAL_FILLER1_NUM = 1;

        public virtual bool IsLegalFormat(string inCode)

        {
            bool flag = false;
            int partIndex = inCode.IndexOf(";part(", StringComparison.Ordinal);
            int passiveIndex = inCode.IndexOf(";nopass", StringComparison.Ordinal);
            string filler = null;
            string particle = null;
            bool passive = false;

            if (partIndex != -1)

            {
                filler = inCode.Substring(0, partIndex);

                if (passiveIndex != -1)

                {
                    particle = inCode.Substring(partIndex, passiveIndex - partIndex);
                }
                else

                {
                    particle = inCode.Substring(partIndex);
                }
            }
            else if (passiveIndex != -1)

            {
                filler = inCode.Substring(0, passiveIndex);
            }
            else

            {
                filler = inCode;
            }

            if (!ReferenceEquals(particle, null))

            {
                flag = (CheckCplxtranFiller(filler)) && (CheckParticle.IsLegal(particle));
            }
            else

            {
                flag = CheckCplxtranFiller(filler);
            }

            return flag;
        }

        private bool CheckCplxtranFiller(string filler)

        {
            bool flag = false;
            string filler1 = "";
            string filler2 = "";
            if (filler.StartsWith("pphr(", StringComparison.Ordinal) == true)

            {
                int index = filler.IndexOf("),", StringComparison.Ordinal);
                filler1 = filler.Substring(0, index + 1);
                filler2 = filler.Substring(index + 2);
            }
            else

            {
                int index = filler.IndexOf(",", StringComparison.Ordinal);
                filler1 = filler.Substring(0, index);
                filler2 = filler.Substring(index + 1);
            }

            flag = (CheckFiller1(filler1)) && (CheckFiller2(filler2));
            return flag;
        }

        private bool CheckFiller1(string filler)
        {
            bool flag = false;
            if (filler1_.Contains(filler) == true)

            {
                flag = true;
            }
            else if (filler.StartsWith("fincomp(", StringComparison.Ordinal) == true)

            {
                flag = CheckFinComp.IsLegal(filler);
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

        private bool CheckFiller2(string filler)
        {
            bool flag = false;
            if (filler2_.Contains(filler) == true)

            {
                flag = true;
            }
            else if (filler.StartsWith("ascomp:", StringComparison.Ordinal) == true)

            {
                flag = CheckAsComp.IsLegal(filler);
            }
            else if (filler.StartsWith("binfcomp:", StringComparison.Ordinal) == true)

            {
                flag = CheckBinfComp.IsLegal(filler);
            }
            else if (filler.StartsWith("edcomp:", StringComparison.Ordinal) == true)

            {
                flag = CheckEdComp.IsLegal(filler);
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

        private static HashSet<string> filler1_ = new HashSet<string>();

        private const int LEGAL_FILLER2_NUM = 2;
        private static HashSet<string> filler2_ = new HashSet<string>();

        static CheckFormatVerbCplxtran()

        {
            filler1_.Add("np");
            filler2_.Add("adj");
            filler2_.Add("advbl");
            filler2_.Add("np");
        }
    }
}