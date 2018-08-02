using SimpleNLG.Main.lexicon.util.lexCheck.Lib;
using UpdateLex = SimpleNLG.Main.lexicon.util.lexCheck.Lib.UpdateLex;

namespace SimpleNLG.Main.lexicon.util.lexCheck.Cat.Noun
{
    using LexRecord = LexRecord;
    using UpdateLex = UpdateLex;


    public class UpdateNounNominalization : UpdateLex


    {
        public virtual void Update(LexRecord lexObj, string token)

        {
            lexObj.GetCatEntry().GetNounEntry().AddNominalization(token);
        }
    }
}