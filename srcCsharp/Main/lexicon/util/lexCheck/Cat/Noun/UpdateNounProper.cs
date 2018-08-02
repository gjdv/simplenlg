using SimpleNLG.Main.lexicon.util.lexCheck.Lib;
using UpdateLex = SimpleNLG.Main.lexicon.util.lexCheck.Lib.UpdateLex;

namespace SimpleNLG.Main.lexicon.util.lexCheck.Cat.Noun
{
    using LexRecord = LexRecord;
    using UpdateLex = UpdateLex;


    public class UpdateNounProper : UpdateLex


    {
        public virtual void Update(LexRecord lexObj, string token)

        {
            lexObj.GetCatEntry().GetNounEntry().SetProper(token.Equals("\tproper"));
        }
    }
}