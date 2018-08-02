using SimpleNLG.Main.lexicon.util.lexCheck.Lib;
using UpdateLex = SimpleNLG.Main.lexicon.util.lexCheck.Lib.UpdateLex;

namespace SimpleNLG.Main.lexicon.util.lexCheck.Cat.Det
{
    using LexRecord = LexRecord;
    using UpdateLex = UpdateLex;


    public class UpdateDetInterrogative : UpdateLex


    {
        public virtual void Update(LexRecord lexObj, string token)

        {
            lexObj.GetCatEntry().GetDetEntry().SetInterrogative(token.Equals("\tinterrogative"));
        }
    }
}