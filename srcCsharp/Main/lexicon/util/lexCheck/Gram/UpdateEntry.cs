namespace SimpleNLG.Main.lexicon.util.lexCheck.Gram
{
    using LexRecord = Lib.LexRecord;
    using UpdateLex = Lib.UpdateLex;


    public class UpdateEntry : UpdateLex


    {
        public virtual void Update(LexRecord lexObj, string token)

        {
            lexObj.SetEui(token);
        }
    }
}