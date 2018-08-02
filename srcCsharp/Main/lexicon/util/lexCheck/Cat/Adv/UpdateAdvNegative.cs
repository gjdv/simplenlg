using System;
using SimpleNLG.Main.lexicon.util.lexCheck.Lib;
using UpdateLex = SimpleNLG.Main.lexicon.util.lexCheck.Lib.UpdateLex;

namespace SimpleNLG.Main.lexicon.util.lexCheck.Cat.Adv
{
    using LexRecord = LexRecord;
    using UpdateLex = UpdateLex;


    public class UpdateAdvNegative : UpdateLex


    {
        public virtual void Update(LexRecord lexObj, string token)

        {
            int index = token.IndexOf("\t", StringComparison.Ordinal);
            string negative = token.Substring(index + 1);
            lexObj.GetCatEntry().GetAdvEntry().SetNegative(negative);
        }
    }
}