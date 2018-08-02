using SimpleNLG.Main.lexicon.util.lexCheck.Lib;
using UpdateLex = SimpleNLG.Main.lexicon.util.lexCheck.Lib.UpdateLex;

namespace SimpleNLG.Main.lexicon.util.lexCheck.Cat.Adj
{
    using LexRecord = LexRecord;
    using UpdateLex = UpdateLex;


    public class UpdateAdjStative : UpdateLex


    {
        public virtual void Update(LexRecord lexObj, string token)

        {
            lexObj.GetCatEntry().GetAdjEntry().SetStative(token.Equals("\tstative"));
        }
    }
}