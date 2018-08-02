using System;
using System.Collections.Generic;
using SimpleNLG.Main.lexicon.util.lexCheck.CkLib;
using SimpleNLG.Main.lexicon.util.lexCheck.Lib;

namespace SimpleNLG.Main.lexicon.util.lexCheck.Cat.Auxi
{
    using CheckCode = CheckCode;
    using CheckObject = CheckObject;
    using CheckSt = CheckSt;
    using LexRecord = LexRecord;
    using LineObject = LineObject;


    public class CheckAux

    {
        public static bool Check(LineObject lineObject, bool printFlag, CheckSt catSt, LexRecord lexObj, bool debugFlag)

        {
            bool flag = true;
            int curSt = catSt.GetCurState();

            if (curSt == 40)

            {
                curSt = 121;
                catSt.SetCurState(curSt);
            }

            switch (curSt)

            {
                case 121:
                    flag = CheckCode.Check(lineObject, printFlag, catSt, lexObj, checkVariant_, new UpdateAuxVariant(),
                        4, true);


                    PrintStep(121, debugFlag, lineObject.GetLine());

                    break;
            }

            return flag;
        }

        private static void PrintStep(int state, bool debugFlag, string line)

        {
            if (debugFlag == true)

            {
                switch (state)

                {
                    case 121:
                        Console.WriteLine("-- Checked Aux Variant: '" + line + "'");
                        break;
                }
            }
        }


        private static HashSet<string> variantNextStartStrs_ = new HashSet<string>();


        static CheckAux()

        {
            variantNextStartStrs_.Add("\tacronym_of=");
            variantNextStartStrs_.Add("\tabbreviation_of=");
            variantNextStartStrs_.Add("annotation=");
            variantNextStartStrs_.Add("signature=");
            variantNextStartStrs_.Add("}");
        }

        private static CheckObject checkVariant_ =
            new CheckObject("\tvariant=", 34, 35, 91, variantNextStartStrs_, new CheckFormatAuxVariant());
    }
}