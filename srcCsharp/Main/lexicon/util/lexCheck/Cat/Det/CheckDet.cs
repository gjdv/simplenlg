using System;
using System.Collections.Generic;
using SimpleNLG.Main.lexicon.util.lexCheck.CkLib;
using SimpleNLG.Main.lexicon.util.lexCheck.Lib;

namespace SimpleNLG.Main.lexicon.util.lexCheck.Cat.Det
{
    using CheckCode = CheckCode;
    using CheckObject = CheckObject;
    using CheckSt = CheckSt;
    using LexRecord = LexRecord;
    using LineObject = LineObject;


    public class CheckDet

    {
        public static bool Check(LineObject lineObject, bool printFlag, CheckSt catSt, LexRecord lexObj, bool debugFlag)

        {
            bool flag = true;
            int curSt = catSt.GetCurState();

            if (curSt == 40)

            {
                curSt = 151;
                catSt.SetCurState(curSt);
            }

            switch (curSt)

            {
                case 151:
                    flag = CheckCode.Check(lineObject, printFlag, catSt, lexObj, checkVariants_,
                        new UpdateDetVariants(), 1, true);


                    PrintStep(151, debugFlag, lineObject.GetLine());

                    break;
                case 152:
                    flag = CheckCode.Check(lineObject, printFlag, catSt, lexObj, checkInterrogative_,
                        new UpdateDetInterrogative(), 6, false);


                    PrintStep(152, debugFlag, lineObject.GetLine());

                    break;
                case 153:
                    flag = CheckCode.Check(lineObject, printFlag, catSt, lexObj, checkDemonstrative_,
                        new UpdateDetDemonstrative(), 6, false);


                    PrintStep(152, debugFlag, lineObject.GetLine());

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
                    case 151:
                        Console.WriteLine("-- Checked Det Variants: '" + line + "'");

                        break;
                    case 152:
                        Console.WriteLine("-- Checked Det Interrogative: '" + line + "'");

                        break;
                    case 153:
                        Console.WriteLine("-- Checked Det Demonstrative: '" + line + "'");
                        break;
                }
            }
        }

        private static CheckObject checkVariants_ = null;
        private static CheckObject checkInterrogative_ = null;
        private static HashSet<string> interrogativeNextStartStrs_ = new HashSet<string>();


        private static HashSet<string> demonstrativeNextStartStrs_ = new HashSet<string>();


        static CheckDet()

        {
            checkVariants_ = new CheckObject("\tvariants=", 21, 22, 152, null, new CheckFormatDetVariants());


            interrogativeNextStartStrs_.Add("\tdemonstrative");
            interrogativeNextStartStrs_.Add("\tacronym_of=");
            interrogativeNextStartStrs_.Add("\tabbreviation_of=");
            interrogativeNextStartStrs_.Add("annotation=");
            interrogativeNextStartStrs_.Add("signature=");
            interrogativeNextStartStrs_.Add("}");
            checkInterrogative_ = new CheckObject("\tinterrogative", 23, -1, 153, interrogativeNextStartStrs_, null);


            demonstrativeNextStartStrs_.Add("\tacronym_of=");
            demonstrativeNextStartStrs_.Add("\tabbreviation_of=");
            demonstrativeNextStartStrs_.Add("annotation=");
            demonstrativeNextStartStrs_.Add("signature=");
            demonstrativeNextStartStrs_.Add("}");
        }

        private static CheckObject checkDemonstrative_ =
            new CheckObject("\tdemonstrative", 24, -1, 91, demonstrativeNextStartStrs_, null);
    }
}