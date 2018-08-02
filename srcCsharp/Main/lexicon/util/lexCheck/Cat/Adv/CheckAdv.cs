using System;
using System.Collections.Generic;
using SimpleNLG.Main.lexicon.util.lexCheck.CkLib;
using SimpleNLG.Main.lexicon.util.lexCheck.Lib;

namespace SimpleNLG.Main.lexicon.util.lexCheck.Cat.Adv
{
    using CheckCode = CheckCode;
    using CheckObject = CheckObject;
    using CheckSt = CheckSt;
    using LexRecord = LexRecord;
    using LineObject = LineObject;


    public class CheckAdv

    {
        public static bool Check(LineObject lineObject, bool printFlag, CheckSt catSt, LexRecord lexObj, bool debugFlag)

        {
            bool flag = true;
            int curSt = catSt.GetCurState();

            if (curSt == 40)

            {
                curSt = 111;
                catSt.SetCurState(curSt);
            }

            switch (curSt)

            {
                case 111:
                    flag = CheckCode.Check(lineObject, printFlag, catSt, lexObj, checkVariants_,
                        new UpdateAdvVariants(), 4, true);


                    PrintStep(111, debugFlag, lineObject.GetLine());

                    break;
                case 112:
                    flag = CheckCode.Check(lineObject, printFlag, catSt, lexObj, checkInterrogative_,
                        new UpdateAdvInterrogative(), 6, false);


                    PrintStep(192, debugFlag, lineObject.GetLine());

                    break;
                case 113:
                    flag = CheckCode.Check(lineObject, printFlag, catSt, lexObj, checkModification_,
                        new UpdateAdvModification(), 4, true);


                    PrintStep(113, debugFlag, lineObject.GetLine());

                    break;
                case 114:
                    flag = CheckCode.Check(lineObject, printFlag, catSt, lexObj, checkNegative_,
                        new UpdateAdvNegative(), 6, false);


                    if (!flag)

                    {
                        catSt.SetCurState(114);
                        catSt.SetLastState(113);
                        flag = CheckCode.Check(lineObject, printFlag, catSt, lexObj, checkBroadNegative_,
                            new UpdateAdvNegative(), 6, false);
                    }

                    PrintStep(114, debugFlag, lineObject.GetLine());

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
                    case 111:
                        Console.WriteLine("-- Checked Adv Variants: '" + line + "'");

                        break;
                    case 112:
                        Console.WriteLine("-- Checked Adv Interrogative: '" + line + "'");

                        break;
                    case 113:
                        Console.WriteLine("-- Checked Adv Modification: '" + line + "'");

                        break;
                    case 114:
                        Console.WriteLine("-- Checked Adv Negative: '" + line + "'");
                        break;
                }
            }
        }

        private static CheckObject checkVariants_ = null;
        private static HashSet<string> variantsNextStartStrs_ = new HashSet<string>();

        private static CheckObject checkInterrogative_ = null;
        private static HashSet<string> interrogativeNextStartStrs_ = new HashSet<string>();

        private static CheckObject checkModification_ = null;
        private static HashSet<string> modificationNextStartStrs_ = new HashSet<string>();


        private static HashSet<string> negativeNextStartStrs_ = new HashSet<string>();


        static CheckAdv()

        {
            variantsNextStartStrs_.Add("\tinterrogative");
            variantsNextStartStrs_.Add("\tmodification_type=");
            variantsNextStartStrs_.Add("\tnegative");
            variantsNextStartStrs_.Add("\tbroad_negative");
            variantsNextStartStrs_.Add("\tacronym_of=");
            variantsNextStartStrs_.Add("\tabbreviation_of=");
            variantsNextStartStrs_.Add("annotation=");
            variantsNextStartStrs_.Add("signature=");
            variantsNextStartStrs_.Add("}");
            checkVariants_ = new CheckObject("\tvariants=", 36, 37, 112, variantsNextStartStrs_,
                new CheckFormatAdvVariants());


            interrogativeNextStartStrs_.Add("\tmodification_type=");
            interrogativeNextStartStrs_.Add("\tnegative");
            interrogativeNextStartStrs_.Add("\tbroad_negative");
            interrogativeNextStartStrs_.Add("\tacronym_of=");
            interrogativeNextStartStrs_.Add("\tabbreviation_of=");
            interrogativeNextStartStrs_.Add("annotation=");
            interrogativeNextStartStrs_.Add("signature=");
            interrogativeNextStartStrs_.Add("}");
            checkInterrogative_ = new CheckObject("\tinterrogative", 38, -1, 113, interrogativeNextStartStrs_, null);


            modificationNextStartStrs_.Add("\tnegative");
            modificationNextStartStrs_.Add("\tbroad_negative");
            modificationNextStartStrs_.Add("\tacronym_of=");
            modificationNextStartStrs_.Add("\tabbreviation_of=");
            modificationNextStartStrs_.Add("annotation=");
            modificationNextStartStrs_.Add("signature=");
            modificationNextStartStrs_.Add("}");
            checkModification_ = new CheckObject("\tmodification_type=", 39, 40, 114, modificationNextStartStrs_,
                new CheckFormatAdvModification());


            negativeNextStartStrs_.Add("\tacronym_of=");
            negativeNextStartStrs_.Add("\tabbreviation_of=");
            negativeNextStartStrs_.Add("annotation=");
            negativeNextStartStrs_.Add("signature=");
            negativeNextStartStrs_.Add("}");
        }

        private static CheckObject checkNegative_ =
            new CheckObject("\tnegative", -1, -1, 91, negativeNextStartStrs_, null);


        private static CheckObject checkBroadNegative_ =
            new CheckObject("\tbroad_negative", 42, -1, 91, negativeNextStartStrs_, null);
    }
}