using System.Collections.Generic;
using CheckFormat = SimpleNLG.Main.lexicon.util.lexCheck.Lib.CheckFormat;

namespace SimpleNLG.Main.lexicon.util.lexCheck.Cat
{
    using CheckFormat = CheckFormat;


    public class CheckFormatCat : CheckFormat


    {
        private const int CAT_NUM = 11;

        public virtual bool IsLegalFormat(string filler)

        {
            bool flag = legalCat_.Contains(filler);
            return flag;
        }

        private static HashSet<string> legalCat_ = new HashSet<string>();
        public const string VERB = "verb";
        public const string AUX = "aux";
        public const string MODAL = "modal";
        public const string NOUN = "noun";
        public const string PRON = "pron";
        public const string ADJ = "adj";
        public const string ADV = "adv";
        public const string PREP = "prep";
        public const string CONJ = "conj";
        public const string COMPL = "compl";
        public const string DET = "det";

        static CheckFormatCat()

        {
            legalCat_.Add("verb");
            legalCat_.Add("aux");
            legalCat_.Add("modal");
            legalCat_.Add("noun");
            legalCat_.Add("pron");
            legalCat_.Add("adj");
            legalCat_.Add("adv");
            legalCat_.Add("prep");
            legalCat_.Add("conj");
            legalCat_.Add("compl");
            legalCat_.Add("det");
        }
    }
}