using System;

namespace SimpleNLG.Main.lexicon.util.lexCheck.Compl
{
    public class CheckParticle

    {
        private const string KEY_COMPL = ";part(";


        public static bool IsLegal(string particle)

        {
            bool flag = true;

            if ((!particle.StartsWith(";part(", StringComparison.Ordinal)) ||
                (!particle.EndsWith(")", StringComparison.Ordinal)))


            {
                return false;
            }

            if (particle.IndexOf("(", StringComparison.Ordinal) + 1 == particle.IndexOf(")", StringComparison.Ordinal))

            {
                return false;
            }

            if ((particle.IndexOf("(", StringComparison.Ordinal) !=
                 particle.LastIndexOf("(", StringComparison.Ordinal)) ||
                (particle.IndexOf(")", StringComparison.Ordinal) !=
                 particle.LastIndexOf(")", StringComparison.Ordinal)))


            {
                return false;
            }

            return flag;
        }


        private static readonly int size_ = ";part(".Length;
    }
}