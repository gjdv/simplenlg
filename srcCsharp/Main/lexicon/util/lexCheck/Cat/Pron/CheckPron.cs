using System;
using System.Collections.Generic;
using SimpleNLG.Main.lexicon.util.lexCheck.CkLib;
using SimpleNLG.Main.lexicon.util.lexCheck.Lib;

namespace SimpleNLG.Main.lexicon.util.lexCheck.Cat.Pron
{
    using CheckCode = CheckCode;
    using CheckObject = CheckObject;
    using CheckSt = CheckSt;
    using LexRecord = LexRecord;
    using LineObject = LineObject;


    public class CheckPron

    {
        public static bool Check(LineObject lineObject, bool printFlag, CheckSt catSt, LexRecord lexObj, bool debugFlag)

        {
            bool flag = true;
            int curSt = catSt.GetCurState();

            if (curSt == 40)

            {
                curSt = 191;
                catSt.SetCurState(curSt);
            }

            switch (curSt)

            {
                case 191:
                    flag = CheckCode.Check(lineObject, printFlag, catSt, lexObj, checkVariants_,
                        new UpdatePronVariants(), 4, true);


                    PrintStep(191, debugFlag, lineObject.GetLine());

                    break;
                case 192:
                    flag = CheckCode.Check(lineObject, printFlag, catSt, lexObj, checkGender_, new UpdatePronGender(),
                        2, true);


                    PrintStep(192, debugFlag, lineObject.GetLine());

                    break;
                case 193:
                    flag = CheckCode.Check(lineObject, printFlag, catSt, lexObj, checkInterrogative_,
                        new UpdatePronInterrogative(), 6, false);


                    PrintStep(193, debugFlag, lineObject.GetLine());

                    break;
                case 194:
                    flag = CheckCode.Check(lineObject, printFlag, catSt, lexObj, checkType_, new UpdatePronType(), 3,
                        false);


                    PrintStep(194, debugFlag, lineObject.GetLine());

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
                    case 191:
                        Console.WriteLine("-- Checked Pron Variants: '" + line + "'");

                        break;
                    case 192:
                        Console.WriteLine("-- Checked Pron Gender: '" + line + "'");

                        break;
                    case 193:
                        Console.WriteLine("-- Checked Pron Interrogative: '" + line + "'");

                        break;
                    case 194:
                        Console.WriteLine("-- Checked Pron Type: '" + line + "'");
                        break;
                }
            }
        }

        private static CheckObject checkVariants_ = null;
        private static HashSet<string> variantsNextStartStrs_ = new HashSet<string>();

        private static CheckObject checkGender_ = null;
        private static HashSet<string> genderNextStartStrs_ = new HashSet<string>();

        private static CheckObject checkInterrogative_ = null;
        private static HashSet<string> interrogativeNextStartStrs_ = new HashSet<string>();


        private static HashSet<string> typeNextStartStrs_ = new HashSet<string>();

        static CheckPron()

        {
            variantsNextStartStrs_.Add("\tgender=");
            variantsNextStartStrs_.Add("\tinterrogative");
            variantsNextStartStrs_.Add("\ttype=");
            variantsNextStartStrs_.Add("\tacronym_of=");
            variantsNextStartStrs_.Add("\tabbreviation_of=");
            variantsNextStartStrs_.Add("annotation=");
            variantsNextStartStrs_.Add("signature=");
            variantsNextStartStrs_.Add("}");
            checkVariants_ = new CheckObject("\tvariants=", 25, 26, 192, variantsNextStartStrs_,
                new CheckFormatPronVariants());


            genderNextStartStrs_.Add("\tinterrogative");
            genderNextStartStrs_.Add("\ttype=");
            genderNextStartStrs_.Add("\tacronym_of=");
            genderNextStartStrs_.Add("\tabbreviation_of=");
            genderNextStartStrs_.Add("annotation=");
            genderNextStartStrs_.Add("signature=");
            genderNextStartStrs_.Add("}");
            checkGender_ = new CheckObject("\tgender=", 27, 28, 193, genderNextStartStrs_, new CheckFormatPronGender());


            interrogativeNextStartStrs_.Add("\ttype=");
            interrogativeNextStartStrs_.Add("\tacronym_of=");
            interrogativeNextStartStrs_.Add("\tabbreviation_of=");
            interrogativeNextStartStrs_.Add("annotation=");
            interrogativeNextStartStrs_.Add("signature=");
            interrogativeNextStartStrs_.Add("}");
            checkInterrogative_ = new CheckObject("\tinterrogative", 29, -1, 194, interrogativeNextStartStrs_, null);


            typeNextStartStrs_.Add("\tabbreviation_of=");
            typeNextStartStrs_.Add("\tacronym_of=");
            typeNextStartStrs_.Add("annotation=");
            typeNextStartStrs_.Add("signature=");
            typeNextStartStrs_.Add("}");
        }

        private static CheckObject checkType_ =
            new CheckObject("\ttype=", 30, 31, 91, typeNextStartStrs_, new CheckFormatPronType());
    }
}