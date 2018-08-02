using SimpleNLG.Main.lexicon.util.lexCheck.Lib;
using UpdateLex = SimpleNLG.Main.lexicon.util.lexCheck.Lib.UpdateLex;

namespace SimpleNLG.Main.lexicon.util.lexCheck.Cat.Verb
{
    using LexRecord = LexRecord;
    using UpdateLex = UpdateLex;


    public class UpdateVerbLink : UpdateLex


    {
        public virtual void Update(LexRecord lexObj, string token)

        {
            lexObj.GetCatEntry().GetVerbEntry().AddLink(token);
        }
    }
}