using System.Collections.Generic;
using CheckFormat = SimpleNLG.Main.lexicon.util.lexCheck.Lib.CheckFormat;

namespace SimpleNLG.Main.lexicon.util.lexCheck.Cat.Adv
{
    using CheckFormat = CheckFormat;


    public class CheckFormatAdvModification : CheckFormat


    {
        private const int LEGAL_FILLER_NUM = 9;

        public virtual bool IsLegalFormat(string filler)

        {
            bool flag = filler_.Contains(filler);
            return flag;
        }

        private static HashSet<string> filler_ = new HashSet<string>();

        static CheckFormatAdvModification()

        {
            filler_.Add("sentence_modifier;manner");
            filler_.Add("sentence_modifier;temporal");
            filler_.Add("sentence_modifier;locative");
            filler_.Add("verb_modifier;manner");
            filler_.Add("verb_modifier;temporal");
            filler_.Add("verb_modifier;locative");
            filler_.Add("particle");
            filler_.Add("intensifier");
        }
    }
}