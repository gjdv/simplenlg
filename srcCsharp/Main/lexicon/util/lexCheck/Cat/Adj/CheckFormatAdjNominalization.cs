﻿using System.Linq;
using SimpleNLG.Main.lexicon.util.lexCheck.Gram;
using SimpleNLG.Main.lexicon.util.lexCheck.Lib;

namespace SimpleNLG.Main.lexicon.util.lexCheck.Cat.Adj
{
    using CheckFormatEui = CheckFormatEui;
    using CheckFormat = CheckFormat;


    public class CheckFormatAdjNominalization : CheckFormat
    {
        public virtual bool IsLegalFormat(string filler)
        {
            var buf = filler.Split('|').ToList().Where(x => x != "").ToArray();

            if (buf.Length == 0) return false;

            string @base = buf[0];

            if (buf.Length > 1)
            {
                string cat = buf[1];
                if (!cat.Equals("noun")) return false;


                if (buf.Length > 2)
                {
                    string eui = buf[2];
                    CheckFormatEui checkFormatEui = new CheckFormatEui();
                    if (!checkFormatEui.IsLegalFormat(eui)) return false;

                    return true;
                }


                return true;
            }


            return true;
        }
    }
}