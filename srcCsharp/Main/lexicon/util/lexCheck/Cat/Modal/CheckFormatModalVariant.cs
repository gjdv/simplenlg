using System;
using System.Collections.Generic;
using CheckFormat = SimpleNLG.Main.lexicon.util.lexCheck.Lib.CheckFormat;

namespace SimpleNLG.Main.lexicon.util.lexCheck.Cat.Modal
{
    using CheckFormat = CheckFormat;


    public class CheckFormatModalVariant : CheckFormat


    {
        private const int LEGAL_FILLER_NUM = 40;

        public virtual bool IsLegalFormat(string filler)

        {
            int index = filler.IndexOf(";", StringComparison.Ordinal);
            string feature = filler.Substring(index);
            bool flag = filler_.Contains(feature);
            return flag;
        }

        private static HashSet<string> filler_ = new HashSet<string>();

        static CheckFormatModalVariant()

        {
            filler_.Add(";past(fst_sing)");
            filler_.Add(";past(fst_plur)");
            filler_.Add(";past(second)");
            filler_.Add(";past(sec_sing)");
            filler_.Add(";past(sec_plur)");
            filler_.Add(";past(third)");
            filler_.Add(";past(thr_sing)");
            filler_.Add(";past(thr_plur)");
            filler_.Add(";pres(fst_sing)");
            filler_.Add(";pres(fst_plur)");
            filler_.Add(";pres(second)");
            filler_.Add(";pres(sec_sing)");
            filler_.Add(";pres(sec_plur)");
            filler_.Add(";pres(third)");
            filler_.Add(";pres(thr_sing)");
            filler_.Add(";pres(thr_plur)");
            filler_.Add(";past(fst_sing):negative");
            filler_.Add(";past(fst_plur):negative");
            filler_.Add(";past(second):negative");
            filler_.Add(";past(sec_sing):negative");
            filler_.Add(";past(sec_plur):negative");
            filler_.Add(";past(third):negative");
            filler_.Add(";past(thr_sing):negative");
            filler_.Add(";past(thr_plur):negative");
            filler_.Add(";pres(fst_sing):negative");
            filler_.Add(";pres(fst_plur):negative");
            filler_.Add(";pres(second):negative");
            filler_.Add(";pres(sec_sing):negative");
            filler_.Add(";pres(sec_plur):negative");
            filler_.Add(";pres(third):negative");
            filler_.Add(";pres(thr_sing):negative");
            filler_.Add(";pres(thr_plur):negative");
            filler_.Add(";past_part");
            filler_.Add(";pres_part");
            filler_.Add(";pres");
            filler_.Add(";past");
            filler_.Add(";past_part:negative");
            filler_.Add(";pres_part:negative");
            filler_.Add(";pres:negative");
            filler_.Add(";past:negative");
        }
    }
}