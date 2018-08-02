using System;
using System.Collections.Generic;
using SimpleNLG.Main.lexicon.util.lexCheck.Compl;
using CheckFormat = SimpleNLG.Main.lexicon.util.lexCheck.Lib.CheckFormat;

namespace SimpleNLG.Main.lexicon.util.lexCheck.Cat.Verb
{
    using CheckFinComp = CheckFinComp;
    using CheckIngComp = CheckIngComp;
    using CheckNpComp = CheckNpComp;
    using CheckParticle = CheckParticle;
    using CheckPphr = CheckPphr;
    using CheckWhfinComp = CheckWhfinComp;
    using CheckWhinfComp = CheckWhinfComp;
    using CheckFormat = CheckFormat;


    public class CheckFormatVerbDitran : CheckFormat


    {
        private const int LEGAL_FILLER1_NUM = 1;

        public virtual bool IsLegalFormat(string inCode)

        {
            bool flag = false;
            int partIndex = inCode.IndexOf(";part(", StringComparison.Ordinal);
            int passiveIndex = inCode.IndexOf(";nopass", StringComparison.Ordinal);
            int datmvtIndex = inCode.IndexOf(";datmvt", StringComparison.Ordinal);
            int noPrtmvtIndex = inCode.IndexOf(";noprtmvt", StringComparison.Ordinal);
            string filler = null;
            string particle = null;
            bool passive = false;
            bool noPrtmvt = false;

            if (partIndex != -1)

            {
                filler = inCode.Substring(0, partIndex);

                if (passiveIndex != -1)

                {
                    particle = inCode.Substring(partIndex, passiveIndex - partIndex);
                }
                else if (datmvtIndex != -1)

                {
                    particle = inCode.Substring(partIndex, datmvtIndex - partIndex);
                }
                else if (noPrtmvtIndex != -1)

                {
                    particle = inCode.Substring(partIndex, noPrtmvtIndex - partIndex);
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
            else if (datmvtIndex != -1)

            {
                filler = inCode.Substring(0, datmvtIndex);
            }
            else if (noPrtmvtIndex != -1)

            {
                filler = inCode.Substring(0, noPrtmvtIndex);
            }
            else

            {
                filler = inCode;
            }

            if (!ReferenceEquals(particle, null))

            {
                flag = (CheckDitranFiller(filler)) && (CheckParticle.IsLegal(particle));
            }
            else

            {
                flag = CheckDitranFiller(filler);
            }

            return flag;
        }

        private bool CheckDitranFiller(string filler)

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
            else if (filler.StartsWith("np|", StringComparison.Ordinal) == true)

            {
                flag = CheckNpComp.IsLegal(filler);
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
            else if (filler.StartsWith("fincomp(", StringComparison.Ordinal) == true)

            {
                flag = CheckFinComp.IsLegal(filler);
            }
            else if (filler.StartsWith("np|", StringComparison.Ordinal) == true)

            {
                flag = CheckNpComp.IsLegal(filler);
            }
            else if (filler.StartsWith("pphr(", StringComparison.Ordinal) == true)

            {
                flag = CheckPphr.IsLegal(filler);
            }
            else if (filler.StartsWith("whfincomp:", StringComparison.Ordinal) == true)

            {
                flag = CheckWhfinComp.IsLegal(filler);
            }
            else if (filler.StartsWith("whinfcomp:", StringComparison.Ordinal) == true)

            {
                flag = CheckWhinfComp.IsLegal(filler);
            }

            return flag;
        }

        private static HashSet<string> filler1_ = new HashSet<string>();

        private const int LEGAL_FILLER2_NUM = 2;
        private static HashSet<string> filler2_ = new HashSet<string>();

        static CheckFormatVerbDitran()

        {
            filler1_.Add("np");
            filler2_.Add("np");
            filler2_.Add("whfincomp");
        }
    }
}