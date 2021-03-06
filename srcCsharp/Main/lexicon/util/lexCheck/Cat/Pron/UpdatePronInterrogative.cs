﻿using SimpleNLG.Main.lexicon.util.lexCheck.Lib;
using UpdateLex = SimpleNLG.Main.lexicon.util.lexCheck.Lib.UpdateLex;

namespace SimpleNLG.Main.lexicon.util.lexCheck.Cat.Pron
{
    using LexRecord = LexRecord;
    using UpdateLex = UpdateLex;


    public class UpdatePronInterrogative : UpdateLex


    {
        public virtual void Update(LexRecord lexObj, string token)

        {
            lexObj.GetCatEntry().GetPronEntry().SetInterrogative(token.Equals("\tinterrogative"));
        }
    }
}