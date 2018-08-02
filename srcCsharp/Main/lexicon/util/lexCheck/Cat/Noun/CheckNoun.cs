using System;
using System.Collections.Generic;
using SimpleNLG.Main.lexicon.util.lexCheck.CkLib;
using SimpleNLG.Main.lexicon.util.lexCheck.Lib;

namespace SimpleNLG.Main.lexicon.util.lexCheck.Cat.Noun
{
    using CheckCode = CheckCode;
    using CheckObject = CheckObject;
    using CheckSt = CheckSt;
    using LexRecord = LexRecord;
    using LineObject = LineObject;


    public class CheckNoun

    {
        public static bool Check(LineObject lineObject, bool printFlag, CheckSt catSt, LexRecord lexObj, bool debugFlag)

        {
            bool flag = true;
            int curSt = catSt.GetCurState();

            if (curSt == 40)

            {
                curSt = 171;
                catSt.SetCurState(curSt);
            }

            switch (curSt)

            {
                case 171:
                    flag = CheckCode.Check(lineObject, printFlag, catSt, lexObj, checkVariants_,
                        new UpdateNounVariants(), 4, true);


                    PrintStep(171, debugFlag, lineObject.GetLine());

                    break;
                case 172:
                    flag = CheckCode.Check(lineObject, printFlag, catSt, lexObj, checkCompl_, new UpdateNounCompl(), 3,
                        true);


                    PrintStep(172, debugFlag, lineObject.GetLine());

                    break;
                case 173:
                    flag = CheckCode.Check(lineObject, printFlag, catSt, lexObj, checkNominalization_,
                        new UpdateNounNominalization(), 3, true);


                    PrintStep(173, debugFlag, lineObject.GetLine());

                    break;
                case 174:
                    flag = CheckCode.Check(lineObject, printFlag, catSt, lexObj, checkProper_, new UpdateNounProper(),
                        6, false);


                    PrintStep(174, debugFlag, lineObject.GetLine());

                    break;
                case 175:
                    flag = CheckCode.Check(lineObject, printFlag, catSt, lexObj, checkTradeName_,
                        new UpdateNounTradeName(), 3, true);


                    PrintStep(175, debugFlag, lineObject.GetLine());

                    break;
                case 176:
                    flag = CheckCode.Check(lineObject, printFlag, catSt, lexObj, checkTradeMark_,
                        new UpdateNounTradeMark(), 6, false);


                    PrintStep(176, debugFlag, lineObject.GetLine());

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
                    case 171:
                        Console.WriteLine("-- Checked Noun Variants: '" + line + "'");

                        break;
                    case 172:
                        Console.WriteLine("-- Checked Noun Complement: '" + line + "'");

                        break;
                    case 173:
                        Console.WriteLine("-- Checked Noun Nominalization: '" + line + "'");

                        break;
                    case 174:
                        Console.WriteLine("-- Checked Noun Proper: '" + line + "'");

                        break;
                    case 175:
                        Console.WriteLine("-- Checked Noun Trade Name: '" + line + "'");

                        break;
                    case 176:
                        Console.WriteLine("-- Checked Noun Trade Mark: '" + line + "'");
                        break;
                }
            }
        }

        private static CheckObject checkVariants_ = null;
        private static HashSet<string> variantsNextStartStrs_ = new HashSet<string>();

        private static CheckObject checkCompl_ = null;
        private static HashSet<string> complNextStartStrs_ = new HashSet<string>();

        private static CheckObject checkNominalization_ = null;
        private static HashSet<string> nominalizationNextStartStrs_ = new HashSet<string>();

        private static CheckObject checkProper_ = null;
        private static HashSet<string> properNextStartStrs_ = new HashSet<string>();

        private static CheckObject checkTradeName_ = null;
        private static HashSet<string> tradeNameNextStartStrs_ = new HashSet<string>();

        private static HashSet<string> tradeNameNextLine_ = new HashSet<string>();


        private static HashSet<string> tradeMarkNextStartStrs_ = new HashSet<string>();


        static CheckNoun()

        {
            variantsNextStartStrs_.Add("\tcompl=");
            variantsNextStartStrs_.Add("\tnominalization_of=");
            variantsNextStartStrs_.Add("\ttrademark=");
            variantsNextStartStrs_.Add("\ttrademark");
            variantsNextStartStrs_.Add("\tproper");
            variantsNextStartStrs_.Add("\tacronym_of=");
            variantsNextStartStrs_.Add("\tabbreviation_of=");
            variantsNextStartStrs_.Add("annotation=");
            variantsNextStartStrs_.Add("signature=");
            variantsNextStartStrs_.Add("}");
            checkVariants_ = new CheckObject("\tvariants=", 52, 53, 172, variantsNextStartStrs_,
                new CheckFormatNounVariants());


            complNextStartStrs_.Add("\tnominalization_of=");
            complNextStartStrs_.Add("\ttrademark=");
            complNextStartStrs_.Add("\ttrademark");
            complNextStartStrs_.Add("\tproper");
            complNextStartStrs_.Add("\tacronym_of=");
            complNextStartStrs_.Add("\tabbreviation_of=");
            complNextStartStrs_.Add("annotation=");
            complNextStartStrs_.Add("signature=");
            complNextStartStrs_.Add("}");
            checkCompl_ = new CheckObject("\tcompl=", 54, 55, 173, complNextStartStrs_, new CheckFormatNounCompl());


            nominalizationNextStartStrs_.Add("\tproper");
            nominalizationNextStartStrs_.Add("\ttrademark=");
            nominalizationNextStartStrs_.Add("\ttrademark");
            nominalizationNextStartStrs_.Add("\tacronym_of=");
            nominalizationNextStartStrs_.Add("\tabbreviation_of=");
            nominalizationNextStartStrs_.Add("annotation=");
            nominalizationNextStartStrs_.Add("signature=");
            nominalizationNextStartStrs_.Add("}");
            checkNominalization_ = new CheckObject("\tnominalization_of=", 56, 57, 174, nominalizationNextStartStrs_,
                new CheckFormatNounNominalization());


            properNextStartStrs_.Add("\ttrademark=");
            properNextStartStrs_.Add("\ttrademark");
            properNextStartStrs_.Add("\tacronym_of=");
            properNextStartStrs_.Add("\tabbreviation_of=");
            properNextStartStrs_.Add("annotation=");
            properNextStartStrs_.Add("signature=");
            properNextStartStrs_.Add("}");
            checkProper_ = new CheckObject("\tproper", 62, -1, 175, properNextStartStrs_, null);


            tradeNameNextStartStrs_.Add("\ttrademark");
            tradeNameNextStartStrs_.Add("\tacronym_of=");
            tradeNameNextStartStrs_.Add("\tabbreviation_of=");
            tradeNameNextStartStrs_.Add("annotation=");
            tradeNameNextStartStrs_.Add("signature=");
            tradeNameNextStartStrs_.Add("}");
            tradeNameNextLine_.Add("\ttrademark");
            checkTradeName_ = new CheckObject("\ttrademark=", 58, 59, 176, tradeNameNextStartStrs_, tradeNameNextLine_,
                null);


            tradeMarkNextStartStrs_.Add("\tacronym_of=");
            tradeMarkNextStartStrs_.Add("\tabbreviation_of=");
            tradeMarkNextStartStrs_.Add("annotation=");
            tradeMarkNextStartStrs_.Add("signature=");
            tradeMarkNextStartStrs_.Add("}");
        }

        private static CheckObject checkTradeMark_ =
            new CheckObject("\ttrademark", 60, -1, 91, tradeMarkNextStartStrs_, null);
    }
}