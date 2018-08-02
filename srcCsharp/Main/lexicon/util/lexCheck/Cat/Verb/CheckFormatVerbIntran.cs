using System;
using SimpleNLG.Main.lexicon.util.lexCheck.Compl;
using CheckFormat = SimpleNLG.Main.lexicon.util.lexCheck.Lib.CheckFormat;

namespace SimpleNLG.Main.lexicon.util.lexCheck.Cat.Verb
{
    using CheckParticle = CheckParticle;
    using CheckFormat = CheckFormat;


    public class CheckFormatVerbIntran : CheckFormat


    {
        public virtual bool IsLegalFormat(string inCode)

        {
            bool flag = false;
            int partIndex = inCode.IndexOf(";part(", StringComparison.Ordinal);
            string filler = null;
            string particle = null;

            if (partIndex != -1)

            {
                filler = inCode.Substring(0, partIndex + 1);
                particle = inCode.Substring(partIndex);
            }
            else

            {
                filler = inCode;
            }

            if (!ReferenceEquals(particle, null))

            {
                flag = CheckParticle.IsLegal(particle);
            }

            return flag;
        }
    }
}