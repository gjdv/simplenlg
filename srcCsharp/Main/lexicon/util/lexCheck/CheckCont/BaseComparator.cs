using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleNLG.Main.lexicon.util.lexCheck.CheckCont
{
    public class BaseComparator<T> : Comparer<T>


    {

        public static bool IsAsciiStr(string input)
        {
            const int MaxAnsiCode = 255;

            return !input.Any(c => c > MaxAnsiCode);
        }

        public override int Compare(T o1, T o2)

        {
            string str1 = o1 as string;
            string str2 = o2 as string;

            bool asciiFlag1 = IsAsciiStr(str1);
            bool asciiFlag2 = IsAsciiStr(str2);
            if (asciiFlag1 != asciiFlag2)

            {
                if (asciiFlag1 == true)

                {
                    return -1;
                }

                if (asciiFlag2 == true)

                {
                    return 1;
                }
            }

            bool puncFlag1 = HasNoPunctuation(str1);
            bool puncFlag2 = HasNoPunctuation(str2);
            if (puncFlag1 != puncFlag2)

            {
                if (puncFlag1 == true)

                {
                    return -1;
                }

                if (puncFlag2 == true)

                {
                    return 1;
                }
            }

            int length1 = str1.Length;
            int length2 = str2.Length;
            if (length1 != length2)

            {
                return length1 - length2;
            }

            return str1.CompareTo(str2);
        }


        public static void Main(string[] args)

        {
            List<string> baseStrs = new List<string>();
            baseStrs.Add("M-bcr");
            baseStrs.Add("mbcr");
            baseStrs.Add("MBCR");
            baseStrs.Add("Mbcr");
            baseStrs.Add("M-BCR");
            baseStrs.Add("M-BĆR");
            baseStrs.Add("Mbćr");
            baseStrs.Add("Amœba");
            baseStrs.Add("Amoeba");
            baseStrs.Add("résumé");
            baseStrs.Add("resume");
            baseStrs.Add("colour");
            baseStrs.Add("color");
            baseStrs.Add("retroceecal");
            baseStrs.Add("retrocaecal");
            baseStrs.Add("retrocecal");
            baseStrs.Add("reo virus");
            baseStrs.Add("reovirus");
            PrintList(baseStrs);

            BaseComparator<string> bc = new BaseComparator<string>();
            baseStrs.Sort(bc);
            PrintList(baseStrs);
        }

        private static void PrintList(List<string> inList)

        {
            Console.WriteLine("--------------------------------");
            foreach (string inStr in inList)

            {
                Console.WriteLine(inStr);
            }
        }

        private static bool HasNoPunctuation(string inWord)

        {
            bool noPunctuation = true;
            for (int i = 0; i < inWord.Length; i++)

            {
                if (IsPunctuationChar(inWord[i]) == true)

                {
                    noPunctuation = false;
                    break;
                }
            }

            return noPunctuation;
        }

        private static bool IsPunctuationChar(char inChar)

        {
            return char.IsPunctuation(inChar);
        }
    }


}