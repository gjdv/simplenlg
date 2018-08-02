using System;
using System.Collections.Generic;
using SimpleNLG.Main.lexicon.util.lexCheck.Compl;
using CheckFormat = SimpleNLG.Main.lexicon.util.lexCheck.Lib.CheckFormat;

namespace SimpleNLG.Main.lexicon.util.lexCheck.Cat.Verb
{
    using CheckAsComp = CheckAsComp;
    using CheckBinfComp = CheckBinfComp;
    using CheckFinComp = CheckFinComp;
    using CheckInfComp = CheckInfComp;
    using CheckIngComp = CheckIngComp;
    using CheckNpComp = CheckNpComp;
    using CheckParticle = CheckParticle;
    using CheckPphr = CheckPphr;
    using CheckWhinfComp = CheckWhinfComp;
    using CheckFormat = CheckFormat;


    public class CheckFormatVerbTran : CheckFormat


    {
        private const int LEGAL_FILLER_NUM = 2;

        public virtual bool IsLegalFormat(string inCode)

        {
            bool flag = false;
            int partIndex = inCode.IndexOf(";part(", StringComparison.Ordinal);
            int passiveIndex = inCode.IndexOf(";nopass", StringComparison.Ordinal);
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
                flag = (CheckTranFiller(filler)) && (CheckParticle.IsLegal(particle));
            }
            else

            {
                flag = CheckTranFiller(filler);
            }

            return flag;
        }

        private bool CheckTranFiller(string filler)

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
            else if (filler.StartsWith("binfcomp:", StringComparison.Ordinal) == true)

            {
                flag = CheckBinfComp.IsLegal(filler);
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
            else if (filler.StartsWith("np|", StringComparison.Ordinal) == true)

            {
                flag = CheckNpComp.IsLegal(filler);
            }
            else if (filler.StartsWith("pphr(", StringComparison.Ordinal) == true)

            {
                flag = CheckPphr.IsLegal(filler);
            }
            else if (filler.StartsWith("whinfcomp:", StringComparison.Ordinal) == true)

            {
                flag = CheckWhinfComp.IsLegal(filler);
            }
            else

            {
                flag = false;
            }

            return flag;
        }

        private static HashSet<string> filler_ = new HashSet<string>();

        static CheckFormatVerbTran()

        {
            filler_.Add("np");
            filler_.Add("whfincomp");
        }
    }
}