using System;
using System.Collections.Generic;

namespace SimpleNLG.Main.lexicon.util.lvg
{
    public class Category : BitMaskBase
    {
        public const int ADJ_BIT = 0;
        public const int ADV_BIT = 1;
        public const int AUX_BIT = 2;
        public const int COMPL_BIT = 3;
        public const int CONJ_BIT = 4;
        public const int DET_BIT = 5;
        public const int MODAL_BIT = 6;
        public const int NOUN_BIT = 7;
        public const int PREP_BIT = 8;
        public const int PRON_BIT = 9;
        public const int VERB_BIT = 10;
        public const int TOTAL_BITS = 11;
        public const long ALL_BIT_VALUE = 2047L;
        public const long NO_BIT_VALUE = 0L;

        public Category() : base(2047L, BIT_STR)
        {
        }

        public Category(long value) : base(value, 2047L, BIT_STR)
        {
        }

        public static long ToValue(string valueStr)
        {
            return ToValue(valueStr, BIT_STR);
        }

        public static string ToName(long value)
        {
            return ToName(value, 2047L, BIT_STR);
        }

        public static string GetBitName(int bitValue)
        {
            return GetBitName(bitValue, 0);
        }

        public static string GetBitName(int bitValue, int index)
        {
            return GetBitName(bitValue, index, BIT_STR);
        }

        public static long Enumerate(string valueStr)
        {
            return Enumerate(valueStr, BIT_STR);
        }

        public static List<long> ToValues(long value)
        {
            return ToValues(value, 11);
        }

        public static long[] ToValuesArray(long value)
        {
            List<long> @out = ToValues(value, 11);
            return @out.ToArray();
        }

        public static void Main(string[] args)
        {
            Console.WriteLine("----- static methods -----");
            Console.WriteLine(" -  ToValue(adv+adv+noun+verb): " + ToValue("adv+adj+noun+verb"));
            Console.WriteLine(" -  ToName(1155): " + ToName(1155L));
            List<long> values = ToValues(1155L);
            for (int i = 0; i < values.Count; i++)
            {
                Console.WriteLine(" - Category.ToValues(1155): " + values[i]);
            }

            long[] valuesArray = ToValuesArray(1155L);
            for (int i = 0; i < valuesArray.Length; i++)
            {
                Console.WriteLine(" - Category.ToValues(1155): " + valuesArray[i]);
            }

            Console.WriteLine(" -  ToValue(noun): " + ToValue("noun"));
            Console.WriteLine(" -  GetBitValue(Category.NOUN_BIT): " + GetBitValue(7));
            Console.WriteLine("----- object methods -----");
            Category c1 = new Category(1155L);
            Console.WriteLine(" - c1.GetValue(): " + c1.GetValue());
            Console.WriteLine(" - c1.GetName(): " + c1.GetName());
            c1.SetValue(2047L);
            Console.WriteLine(" - c1.GetValue(): " + c1.GetValue());
            Console.WriteLine(" - c1.GetName(): " + c1.GetName());
        }

        private static readonly List<List<string>> BIT_STR = new List<List<string>>(63);

        static Category()
        {
            for (int i = 0; i < 63; i++)
            {
                BIT_STR.Insert(i, new List<string>());
            }

            BIT_STR[0].Add("adj");
            BIT_STR[0].Add("adjective");
            BIT_STR[0].Add("ADJ");
            BIT_STR[1].Add("adv");
            BIT_STR[1].Add("adverb");
            BIT_STR[1].Add("ADV");
            BIT_STR[2].Add("aux");
            BIT_STR[2].Add("auxiliary");
            BIT_STR[3].Add("compl");
            BIT_STR[3].Add("complementizer");
            BIT_STR[4].Add("conj");
            BIT_STR[4].Add("conjunction");
            BIT_STR[4].Add("CON");
            BIT_STR[4].Add("con");
            BIT_STR[5].Add("det");
            BIT_STR[5].Add("determiner");
            BIT_STR[5].Add("DET");
            BIT_STR[6].Add("modal");
            BIT_STR[7].Add("noun");
            BIT_STR[7].Add("NOM");
            BIT_STR[7].Add("NPR");
            BIT_STR[8].Add("prep");
            BIT_STR[8].Add("preposition");
            BIT_STR[8].Add("PRE");
            BIT_STR[8].Add("pre");
            BIT_STR[9].Add("pron");
            BIT_STR[9].Add("pronoun");
            BIT_STR[10].Add("verb");
            BIT_STR[10].Add("VER");
            BIT_STR[10].Add("ver");
        }
    }
}