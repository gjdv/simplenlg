﻿namespace SimpleNLG.Main.lexicon.util.lexCheck.Gram
{
    using LexRecord = Lib.LexRecord;
    using UpdateLex = Lib.UpdateLex;


    public class UpdateAcronym : UpdateLex


    {
        public virtual void Update(LexRecord lexObj, string token)

        {
            lexObj.SetAcronym(token);
        }
    }
}