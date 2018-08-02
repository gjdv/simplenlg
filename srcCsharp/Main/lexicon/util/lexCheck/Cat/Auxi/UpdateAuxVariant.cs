using SimpleNLG.Main.lexicon.util.lexCheck.Lib;
using UpdateLex = SimpleNLG.Main.lexicon.util.lexCheck.Lib.UpdateLex;

namespace SimpleNLG.Main.lexicon.util.lexCheck.Cat.Auxi
{
    using LexRecord = LexRecord;
    using UpdateLex = UpdateLex;


    public class UpdateAuxVariant : UpdateLex


    {
        public virtual void Update(LexRecord lexObj, string token)

        {
            lexObj.GetCatEntry().GetAuxEntry().AddVariant(token);
        }
    }
}