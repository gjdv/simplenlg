using System.Collections.Generic;

namespace SimpleNLG.Main.lexicon.util.lexCheck.Compl
{
    public class CheckFinComp

    {
        public static bool IsLegal(string filler)

        {
            bool flag = finComp_.Contains(filler);
            return flag;
        }

        private static HashSet<string> finComp_ = new HashSet<string>();

        static CheckFinComp()
        {
            finComp_.Add("fincomp(o)");
            finComp_.Add("fincomp(t)");
            finComp_.Add("fincomp(p)");
            finComp_.Add("fincomp(s)");
            finComp_.Add("fincomp(ts)");
            finComp_.Add("fincomp(tp)");
            finComp_.Add("fincomp(sp)");
            finComp_.Add("fincomp(tsp)");
            finComp_.Add("fincomp(o):subj");
            finComp_.Add("fincomp(t):subj");
            finComp_.Add("fincomp(p):subj");
            finComp_.Add("fincomp(s):subj");
            finComp_.Add("fincomp(ts):subj");
            finComp_.Add("fincomp(tp):subj");
            finComp_.Add("fincomp(sp):subj");
            finComp_.Add("fincomp(tsp):subj");
        }
    }
}