using SimpleNLG.Main.lexicon.util.lexCheck.Cat.Adj;
using SimpleNLG.Main.lexicon.util.lexCheck.Cat.Adv;
using SimpleNLG.Main.lexicon.util.lexCheck.Cat.Auxi;
using SimpleNLG.Main.lexicon.util.lexCheck.Cat.Det;
using SimpleNLG.Main.lexicon.util.lexCheck.Cat.Modal;
using SimpleNLG.Main.lexicon.util.lexCheck.Cat.Noun;
using SimpleNLG.Main.lexicon.util.lexCheck.Cat.Pron;
using SimpleNLG.Main.lexicon.util.lexCheck.Cat.Verb;
using SimpleNLG.Main.lexicon.util.lexCheck.Lib;

namespace SimpleNLG.Main.lexicon.util.lexCheck.Cat
{
    using CheckAdj = CheckAdj;
    using CheckAdv = CheckAdv;
    using CheckAux = CheckAux;
    using CheckDet = CheckDet;
    using CheckModal = CheckModal;
    using CheckNoun = CheckNoun;
    using CheckPron = CheckPron;
    using CheckVerb = CheckVerb;
    using CheckSt = CheckSt;
    using LexRecord = LexRecord;
    using LineObject = LineObject;


    public class CheckCatEntry

    {
        public static bool Check(LineObject lineObject, bool printFlag, CheckSt st, CheckSt catSt, LexRecord lexObj,
            int nextState, bool debugFlag)

        {
            bool flag = false;
            string category = lexObj.GetCategory();
            if (category.Equals("verb") == true)

            {
                flag = CheckVerb.Check(lineObject, printFlag, catSt, lexObj, debugFlag);
            }
            else if (category.Equals("aux") == true)

            {
                flag = CheckAux.Check(lineObject, printFlag, catSt, lexObj, debugFlag);
            }
            else if (category.Equals("modal") == true)

            {
                flag = CheckModal.Check(lineObject, printFlag, catSt, lexObj, debugFlag);
            }
            else if (category.Equals("noun") == true)

            {
                flag = CheckNoun.Check(lineObject, printFlag, catSt, lexObj, debugFlag);
            }
            else if (category.Equals("pron") == true)

            {
                flag = CheckPron.Check(lineObject, printFlag, catSt, lexObj, debugFlag);
            }
            else if (category.Equals("adj") == true)

            {
                flag = CheckAdj.Check(lineObject, printFlag, catSt, lexObj, debugFlag);
            }
            else if (category.Equals("adv") == true)

            {
                flag = CheckAdv.Check(lineObject, printFlag, catSt, lexObj, debugFlag);
            }
            else if (category.Equals("prep") == true)

            {
                flag = CheckNone(lineObject, catSt, nextState);
            }
            else if (category.Equals("conj") == true)

            {
                flag = CheckNone(lineObject, catSt, nextState);
            }
            else if (category.Equals("compl") == true)

            {
                flag = CheckNone(lineObject, catSt, nextState);
            }
            else if (category.Equals("det") == true)

            {
                flag = CheckDet.Check(lineObject, printFlag, catSt, lexObj, debugFlag);
            }

            if (catSt.GetCurState() == nextState)

            {
                st.SetCurState(nextState);
                catSt.SetCurState(40);
            }

            return flag;
        }


        private static bool CheckNone(LineObject lineObject, CheckSt catSt, int nextState)

        {
            lineObject.SetGoToNext(false);
            catSt.UpdateCurState(nextState);
            return true;
        }
    }
}