using System.Linq;
using SimpleNLG.Main.lexicon.util.lexCheck.Gram;
using CheckFormat = SimpleNLG.Main.lexicon.util.lexCheck.Lib.CheckFormat;

namespace SimpleNLG.Main.lexicon.util.lexCheck.Cat.Verb
{
    using CheckFormatEui = CheckFormatEui;
    using CheckFormat = CheckFormat;

    public class CheckFormatVerbNominalization : CheckFormat
    {
        public virtual bool IsLegalFormat(string filler)
        {
            string[] buf = filler.Split('|').ToList().Where(x => x != "").ToArray(); ;
            if (buf.Length == 0)
            {
                return false;
            }

            string @base = buf[0];
            if (buf.Length > 1)
            {
                string cat = buf[1];
                if (!cat.Equals("noun"))
                {
                    return false;
                }

                if (buf.Length > 2)
                {
                    string eui = buf[2];
                    CheckFormatEui checkFormatEui = new CheckFormatEui();
                    if (!checkFormatEui.IsLegalFormat(eui))
                    {
                        return false;
                    }

                    return true;
                }

                return true;
            }

            return true;
        }
    }
}