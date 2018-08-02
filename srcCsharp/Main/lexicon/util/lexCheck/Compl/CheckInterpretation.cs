using System.Collections.Generic;

namespace SimpleNLG.Main.lexicon.util.lexCheck.Compl
{
    public class CheckInterpretation

    {
        private const int INTERPRETATION_NUM = 7;

        public static bool IsLegal(string term)

        {
            bool flag = interpretation_.Contains(term);
            return flag;
        }

        private static HashSet<string> interpretation_ = new HashSet<string>();

        static CheckInterpretation()

        {
            interpretation_.Add("objc");
            interpretation_.Add("objr");
            interpretation_.Add("subjc");
            interpretation_.Add("subjr");
            interpretation_.Add("arbc");
            interpretation_.Add("nsc");
            interpretation_.Add("nsr");
        }
    }
}