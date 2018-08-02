using SimpleNLG.Main.lexicon.util.lexCheck.Lib;
using UpdateLex = SimpleNLG.Main.lexicon.util.lexCheck.Lib.UpdateLex;

namespace SimpleNLG.Main.lexicon.util.lexCheck.Cat.Det
{
    using LexRecord = LexRecord;
    using UpdateLex = UpdateLex;


    public class UpdateDetDemonstrative : UpdateLex


    {
        public virtual void Update(LexRecord lexObj, string token)

        {
            lexObj.GetCatEntry().GetDetEntry().SetDemonstrative(token.Equals("\tdemonstrative"));
        }
    }
}