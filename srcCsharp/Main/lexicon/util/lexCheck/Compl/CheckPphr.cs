using System;
using System.Collections.Generic;

namespace SimpleNLG.Main.lexicon.util.lexCheck.Compl
{
    public class CheckPphr

    {
        public static bool IsLegal(string filler)

        {
            bool flag = true;

            if ((!filler.StartsWith("pphr(", StringComparison.Ordinal)) ||
                (!filler.EndsWith(")", StringComparison.Ordinal)))


            {
                return false;
            }

            int index = filler.IndexOf(",", StringComparison.Ordinal);
            if (index <= 5)

            {
                return false;
            }

            string firstPreposition = filler.Substring(5, index - 5);
            if (!CheckPreposition.IsLegal(firstPreposition))

            {
                return false;
            }

            int endIndex = filler.LastIndexOf(")", StringComparison.Ordinal);
            int comma2Index = filler.IndexOf(",pphr", index + 1, StringComparison.Ordinal);

            if (comma2Index == -1)

            {
                string argument = filler.Substring(index + 1, endIndex - (index + 1));
                flag = CheckArgument(argument);
            }
            else

            {
                string pphr2 = filler.Substring(comma2Index + 1, endIndex - (comma2Index + 1));
                flag = IsLegal(pphr2);
            }

            return flag;
        }

        private static bool CheckArgument(string argument)

        {
            bool flag = false;

            if (argument_.Contains(argument) == true)

            {
                flag = true;
            }
            else if ((argument.StartsWith("binfcomp:", StringComparison.Ordinal) == true) ||
                     (argument.StartsWith("edcomp:", StringComparison.Ordinal) == true) ||
                     (argument.StartsWith("infcomp:", StringComparison.Ordinal) == true) ||
                     (argument.StartsWith("ingcomp:", StringComparison.Ordinal) == true) ||
                     (argument.StartsWith("whinfcomp:", StringComparison.Ordinal) == true))


            {
                int index = argument.IndexOf(":", StringComparison.Ordinal);
                string interpretation = argument.Substring(index + 1);
                flag = CheckInterpretation.IsLegal(interpretation);
            }
            else if ((argument.StartsWith("np|", StringComparison.Ordinal) == true) &&
                     (argument.EndsWith("|", StringComparison.Ordinal) == true) && (!argument.Equals("np|")))


            {
                flag = argument.IndexOf("|", StringComparison.Ordinal) + 1 !=
                       argument.LastIndexOf("|", StringComparison.Ordinal);
            }

            return flag;
        }

        private static HashSet<string> argument_ = new HashSet<string>();

        static CheckPphr()
        {
            argument_.Add("adj");
            argument_.Add("advbl");
            argument_.Add("np");
            argument_.Add("whfincomp");
        }
    }
}