using SimpleNLG.Main.lexicon.util.lexCheck.Lib;
using UpdateLex = SimpleNLG.Main.lexicon.util.lexCheck.Lib.UpdateLex;

namespace SimpleNLG.Main.lexicon.util.lexCheck.Cat.Adv
{
    using LexRecord = LexRecord;
    using UpdateLex = UpdateLex;


    public class UpdateAdvModification : UpdateLex


    {
        public virtual void Update(LexRecord lexObj, string token)

        {
            lexObj.GetCatEntry().GetAdvEntry().AddModification(token);
        }
    }
}