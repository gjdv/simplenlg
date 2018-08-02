﻿using System;

namespace SimpleNLG.Main.lexicon.util.lexCheck.Compl
{
    public class CheckWhinfComp

    {
        private const string KEY_COMPL = "whinfcomp:";


        public static bool IsLegal(string filler)

        {
            bool flag = true;

            if (!filler.StartsWith("whinfcomp:", StringComparison.Ordinal))

            {
                return false;
            }

            int index = filler.IndexOf(":", StringComparison.Ordinal);
            if (index < size_ - 1)

            {
                return false;
            }

            string interpretation = filler.Substring(index + 1);
            if (!CheckInterpretation.IsLegal(interpretation))

            {
                return false;
            }

            return flag;
        }


        private static readonly int size_ = "whinfcomp:".Length;
    }
}