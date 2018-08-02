using System;
using System.Collections.Generic;
using SimpleNLG.Main.lexicon.util.lexCheck.CkLib;
using SimpleNLG.Main.lexicon.util.lexCheck.Lib;

namespace SimpleNLG.Main.lexicon.util.lexCheck.Cat.Adj
{
    using CheckCode = CheckCode;
    using CheckObject = CheckObject;
    using CheckSt = CheckSt;
    using LexRecord = LexRecord;
    using LineObject = LineObject;


    public class CheckAdj

    {
        public static bool Check(LineObject lineObject, bool printFlag, CheckSt catSt, LexRecord lexObj, bool debugFlag)

        {
            bool flag = true;
            int curSt = catSt.GetCurState();

            if (curSt == 40)

            {
                curSt = 101;
                catSt.SetCurState(curSt);
            }

            switch (curSt)

            {
                case 101:
                    flag = CheckCode.Check(lineObject, printFlag, catSt, lexObj, checkVariants_,
                        new UpdateAdjVariants(), 4, true);


                    PrintStep(101, debugFlag, lineObject.GetLine());

                    break;
                case 102:
                    flag = CheckCode.Check(lineObject, printFlag, catSt, lexObj, checkPosition_,
                        new UpdateAdjPosition(), 4, true);


                    PrintStep(102, debugFlag, lineObject.GetLine());

                    break;
                case 103:
                    flag = CheckCode.Check(lineObject, printFlag, catSt, lexObj, checkCompl_, new UpdateAdjCompl(), 3,
                        true);


                    PrintStep(103, debugFlag, lineObject.GetLine());

                    break;
                case 104:
                    flag = CheckCode.Check(lineObject, printFlag, catSt, lexObj, checkStative_, new UpdateAdjStative(),
                        6, false);


                    PrintStep(104, debugFlag, lineObject.GetLine());

                    break;
                case 105:
                    flag = CheckCode.Check(lineObject, printFlag, catSt, lexObj, checkNominalization_,
                        new UpdateAdjNominalization(), 3, true);


                    PrintStep(105, debugFlag, lineObject.GetLine());

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
                    case 101:
                        Console.WriteLine("-- Checked Adj Variants: '" + line + "'");

                        break;
                    case 102:
                        Console.WriteLine("-- Checked Adj Position: '" + line + "'");

                        break;
                    case 103:
                        Console.WriteLine("-- Checked Adj Complement: '" + line + "'");

                        break;
                    case 104:
                        Console.WriteLine("-- Checked Adj Stative: '" + line + "'");

                        break;
                    case 105:
                        Console.WriteLine("-- Checked Adj Nominalization: '" + line + "'");
                        break;
                }
            }
        }

        private static CheckObject checkVariants_ = null;
        private static HashSet<string> variantsNextStartStrs_ = new HashSet<string>();

        private static CheckObject checkPosition_ = null;
        private static HashSet<string> positionNextStartStrs_ = new HashSet<string>();

        private static CheckObject checkCompl_ = null;
        private static HashSet<string> complNextStartStrs_ = new HashSet<string>();

        private static CheckObject checkStative_ = null;
        private static HashSet<string> stativeNextStartStrs_ = new HashSet<string>();


        private static HashSet<string> nominalizationNextStartStrs_ = new HashSet<string>();


        static CheckAdj()

        {
            variantsNextStartStrs_.Add("\tposition=");
            variantsNextStartStrs_.Add("\tcompl=");
            variantsNextStartStrs_.Add("\tstative");
            variantsNextStartStrs_.Add("\tnominalization=");
            variantsNextStartStrs_.Add("\tacronym_of=");
            variantsNextStartStrs_.Add("\tabbreviation_of=");
            variantsNextStartStrs_.Add("annotation=");
            variantsNextStartStrs_.Add("signature=");
            variantsNextStartStrs_.Add("}");
            checkVariants_ = new CheckObject("\tvariants=", 43, 44, 102, variantsNextStartStrs_,
                new CheckFormatAdjVariants());


            positionNextStartStrs_.Add("\tcompl=");
            positionNextStartStrs_.Add("\tstative");
            positionNextStartStrs_.Add("\tnominalization=");
            positionNextStartStrs_.Add("\tacronym_of=");
            positionNextStartStrs_.Add("\tabbreviation_of=");
            positionNextStartStrs_.Add("annotation=");
            positionNextStartStrs_.Add("signature=");
            positionNextStartStrs_.Add("}");
            checkPosition_ = new CheckObject("\tposition=", 45, 46, 103, positionNextStartStrs_,
                new CheckFormatAdjPosition());


            complNextStartStrs_.Add("\tstative");
            complNextStartStrs_.Add("\tnominalization=");
            complNextStartStrs_.Add("\tacronym_of=");
            complNextStartStrs_.Add("\tabbreviation_of=");
            complNextStartStrs_.Add("annotation=");
            complNextStartStrs_.Add("signature=");
            complNextStartStrs_.Add("}");
            checkCompl_ = new CheckObject("\tcompl=", 47, 48, 104, complNextStartStrs_, new CheckFormatAdjCompl());


            stativeNextStartStrs_.Add("\tnominalization=");
            stativeNextStartStrs_.Add("\tacronym_of=");
            stativeNextStartStrs_.Add("\tabbreviation_of=");
            stativeNextStartStrs_.Add("annotation=");
            stativeNextStartStrs_.Add("signature=");
            stativeNextStartStrs_.Add("}");
            checkStative_ = new CheckObject("\tstative", 49, -1, 105, stativeNextStartStrs_, null);


            nominalizationNextStartStrs_.Add("\tacronym_of=");
            nominalizationNextStartStrs_.Add("\tabbreviation_of=");
            nominalizationNextStartStrs_.Add("annotation=");
            nominalizationNextStartStrs_.Add("signature=");
            nominalizationNextStartStrs_.Add("}");
        }

        private static CheckObject checkNominalization_ = new CheckObject("\tnominalization=", 50, 51, 91,
            nominalizationNextStartStrs_, new CheckFormatAdjNominalization());
    }
}