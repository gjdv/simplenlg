using SimpleNLG.Main.lexicon.util.lexCheck.Lib;
using UpdateLex = SimpleNLG.Main.lexicon.util.lexCheck.Lib.UpdateLex;

namespace SimpleNLG.Main.lexicon.util.lexCheck.Cat.Verb
{
    using LexRecord = LexRecord;
    using UpdateLex = UpdateLex;


    public class UpdateVerbDitran : UpdateLex


    {
        public virtual void Update(LexRecord lexObj, string token)

        {
            lexObj.GetCatEntry().GetVerbEntry().AddDitran(token);
        }
    }
}