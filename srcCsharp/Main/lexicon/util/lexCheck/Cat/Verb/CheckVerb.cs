using System;
using System.Collections.Generic;
using SimpleNLG.Main.lexicon.util.lexCheck.CkLib;
using SimpleNLG.Main.lexicon.util.lexCheck.Lib;

namespace SimpleNLG.Main.lexicon.util.lexCheck.Cat.Verb
{
    using CheckCode = CheckCode;
    using CheckObject = CheckObject;
    using CheckSt = CheckSt;
    using ErrMsg = ErrMsg;
    using LexRecord = LexRecord;
    using LineObject = LineObject;
    using VerbEntry = VerbEntry;


    public class CheckVerb

    {
        public static bool Check(LineObject lineObject, bool printFlag, CheckSt catSt, LexRecord lexObj, bool debugFlag)

        {
            bool flag = true;
            int curSt = catSt.GetCurState();

            if (curSt == 40)

            {
                curSt = 201;
                catSt.SetCurState(curSt);
            }

            switch (curSt)

            {
                case 201:
                    flag = CheckCode.Check(lineObject, printFlag, catSt, lexObj, checkVariants_,
                        new UpdateVerbVariants(), 4, true);


                    PrintStep(201, debugFlag, lineObject.GetLine());

                    break;
                case 202:
                    flag = CheckCode.Check(lineObject, printFlag, catSt, lexObj, checkIntran_, new UpdateVerbIntran(),
                        6, false);


                    PrintStep(202, debugFlag, lineObject.GetLine());

                    break;
                case 203:
                    flag = CheckCode.Check(lineObject, printFlag, catSt, lexObj, checkIntran2_, new UpdateVerbIntran(),
                        3, false);


                    PrintStep(203, debugFlag, lineObject.GetLine());

                    break;
                case 204:
                    flag = CheckCode.Check(lineObject, printFlag, catSt, lexObj, checkTran_, new UpdateVerbTran(), 3,
                        true);


                    PrintStep(204, debugFlag, lineObject.GetLine());

                    break;
                case 205:
                    flag = CheckCode.Check(lineObject, printFlag, catSt, lexObj, checkDitran_, new UpdateVerbDitran(),
                        3, true);


                    PrintStep(205, debugFlag, lineObject.GetLine());

                    break;
                case 206:
                    flag = CheckCode.Check(lineObject, printFlag, catSt, lexObj, checkLink_, new UpdateVerbLink(), 3,
                        true);


                    PrintStep(206, debugFlag, lineObject.GetLine());

                    break;
                case 207:
                    flag = CheckCode.Check(lineObject, printFlag, catSt, lexObj, checkCplxtran_,
                        new UpdateVerbCplxtran(), 3, true);


                    PrintStep(207, debugFlag, lineObject.GetLine());

                    break;
                case 208:
                    if (!CheckVerbComplNum(lexObj))

                    {
                        string line = lineObject.GetLine();
                        ErrMsg.PrintErrMsg(printFlag, 79, lineObject, line, 0, line.Length, true);

                        flag = false;
                    }
                    else
                    {
                        if (flag == true)

                        {
                            flag = CheckCode.Check(lineObject, printFlag, catSt, lexObj, checkNominalization_,
                                new UpdateVerbNominalization(), 3, true);
                        }

                        PrintStep(208, debugFlag, lineObject.GetLine());
                    }

                    break;
            }

            return flag;
        }

        private static bool CheckVerbComplNum(LexRecord lexObj)

        {
            bool flag = false;
            VerbEntry verbEntry = lexObj.GetCatEntry().GetVerbEntry();
            if ((verbEntry.GetIntran().Count > 0) || (verbEntry.GetTran().Count > 0) ||
                (verbEntry.GetDitran().Count > 0) || (verbEntry.GetLink().Count > 0) ||
                (verbEntry.GetCplxtran().Count > 0))


            {
                flag = true;
            }

            return flag;
        }

        private static void PrintStep(int state, bool debugFlag, string line)
        {
            if (debugFlag == true)

            {
                switch (state)

                {
                    case 201:
                        Console.WriteLine("-- Checked Verb Variants: '" + line + "'");

                        break;
                    case 202:
                        Console.WriteLine("-- Checked Verb Intransitive: '" + line + "'");

                        break;
                    case 203:
                        Console.WriteLine("-- Checked Verb Intransitive 2: '" + line + "'");

                        break;
                    case 204:
                        Console.WriteLine("-- Checked Verb Transitive: '" + line + "'");

                        break;
                    case 205:
                        Console.WriteLine("-- Checked Verb Ditransitive: '" + line + "'");

                        break;
                    case 206:
                        Console.WriteLine("-- Checked Verb Linking: '" + line + "'");

                        break;
                    case 207:
                        Console.WriteLine("-- Checked Verb Complex Transitive: '" + line + "'");

                        break;
                    case 208:
                        Console.WriteLine("-- Checked Verb Nominalization: '" + line + "'");
                        break;
                }
            }
        }

        private static CheckObject checkVariants_ = null;
        private static HashSet<string> variantsNextStartStrs_ = new HashSet<string>();

        private static CheckObject checkIntran_ = null;
        private static HashSet<string> intranNextStartStrs_ = new HashSet<string>();

        private static CheckObject checkIntran2_ = null;
        private static HashSet<string> intran2NextStartStrs_ = new HashSet<string>();

        private static CheckObject checkTran_ = null;
        private static HashSet<string> tranNextStartStrs_ = new HashSet<string>();

        private static CheckObject checkDitran_ = null;
        private static HashSet<string> ditranNextStartStrs_ = new HashSet<string>();

        private static CheckObject checkLink_ = null;
        private static HashSet<string> linkNextStartStrs_ = new HashSet<string>();

        private static CheckObject checkCplxtran_ = null;
        private static HashSet<string> cplxtranNextStartStrs_ = new HashSet<string>();


        private static HashSet<string> nominalizationNextStartStrs_ = new HashSet<string>();


        static CheckVerb()

        {
            variantsNextStartStrs_.Add("\tintran");
            variantsNextStartStrs_.Add("\ttran=");
            variantsNextStartStrs_.Add("\tditran=");
            variantsNextStartStrs_.Add("\tlink=");
            variantsNextStartStrs_.Add("\tcplxtran=");
            variantsNextStartStrs_.Add("\tnominalization=");
            variantsNextStartStrs_.Add("\tacronym_of=");
            variantsNextStartStrs_.Add("\tabbreviation_of=");
            variantsNextStartStrs_.Add("annotation=");
            variantsNextStartStrs_.Add("signature=");
            variantsNextStartStrs_.Add("}");
            checkVariants_ = new CheckObject("\tvariants=", 63, 64, 202, variantsNextStartStrs_,
                new CheckFormatVerbVariants());


            intranNextStartStrs_.Add("\tintran;part(");
            intranNextStartStrs_.Add("\ttran=");
            intranNextStartStrs_.Add("\tditran=");
            intranNextStartStrs_.Add("\tlink=");
            intranNextStartStrs_.Add("\tcplxtran=");
            intranNextStartStrs_.Add("\tnominalization=");
            intranNextStartStrs_.Add("\tacronym_of=");
            intranNextStartStrs_.Add("\tabbreviation_of=");
            intranNextStartStrs_.Add("annotation=");
            intranNextStartStrs_.Add("signature=");
            intranNextStartStrs_.Add("}");
            checkIntran_ = new CheckObject("\tintran", 65, 66, 203, intranNextStartStrs_, null);


            intran2NextStartStrs_.Add("\ttran=");
            intran2NextStartStrs_.Add("\tditran=");
            intran2NextStartStrs_.Add("\tlink=");
            intran2NextStartStrs_.Add("\tcplxtran=");
            intran2NextStartStrs_.Add("\tnominalization=");
            intran2NextStartStrs_.Add("\tacronym_of=");
            intran2NextStartStrs_.Add("\tabbreviation_of=");
            intran2NextStartStrs_.Add("annotation=");
            intran2NextStartStrs_.Add("signature=");
            intran2NextStartStrs_.Add("}");
            checkIntran2_ = new CheckObject("\tintran;part(", 67, 68, 204, intran2NextStartStrs_,
                new CheckFormatVerbIntran(), "(");


            tranNextStartStrs_.Add("\tditran=");
            tranNextStartStrs_.Add("\tlink=");
            tranNextStartStrs_.Add("\tcplxtran=");
            tranNextStartStrs_.Add("\tnominalization=");
            tranNextStartStrs_.Add("\tacronym_of=");
            tranNextStartStrs_.Add("\tabbreviation_of=");
            tranNextStartStrs_.Add("annotation=");
            tranNextStartStrs_.Add("signature=");
            tranNextStartStrs_.Add("}");
            checkTran_ = new CheckObject("\ttran=", 69, 70, 205, tranNextStartStrs_, new CheckFormatVerbTran());


            ditranNextStartStrs_.Add("\tlink=");
            ditranNextStartStrs_.Add("\tcplxtran=");
            ditranNextStartStrs_.Add("\tnominalization=");
            ditranNextStartStrs_.Add("\tacronym_of=");
            ditranNextStartStrs_.Add("\tabbreviation_of=");
            ditranNextStartStrs_.Add("annotation=");
            ditranNextStartStrs_.Add("signature=");
            ditranNextStartStrs_.Add("}");
            checkDitran_ = new CheckObject("\tditran=", 71, 72, 206, ditranNextStartStrs_, new CheckFormatVerbDitran());


            linkNextStartStrs_.Add("\tcplxtran=");
            linkNextStartStrs_.Add("\tnominalization=");
            linkNextStartStrs_.Add("\tacronym_of=");
            linkNextStartStrs_.Add("\tabbreviation_of=");
            linkNextStartStrs_.Add("annotation=");
            linkNextStartStrs_.Add("signature=");
            linkNextStartStrs_.Add("}");
            checkLink_ = new CheckObject("\tlink=", 73, 74, 207, linkNextStartStrs_, new CheckFormatVerbLink());


            cplxtranNextStartStrs_.Add("\tnominalization=");
            cplxtranNextStartStrs_.Add("\tacronym_of=");
            cplxtranNextStartStrs_.Add("\tabbreviation_of=");
            cplxtranNextStartStrs_.Add("annotation=");
            cplxtranNextStartStrs_.Add("signature=");
            cplxtranNextStartStrs_.Add("}");
            checkCplxtran_ = new CheckObject("\tcplxtran=", 75, 76, 208, cplxtranNextStartStrs_,
                new CheckFormatVerbCplxtran());


            nominalizationNextStartStrs_.Add("\tacronym_of=");
            nominalizationNextStartStrs_.Add("\tabbreviation_of=");
            nominalizationNextStartStrs_.Add("annotation=");
            nominalizationNextStartStrs_.Add("signature=");
            nominalizationNextStartStrs_.Add("}");
        }

        private static CheckObject checkNominalization_ = new CheckObject("\tnominalization=", 77, 78, 91,
            nominalizationNextStartStrs_, new CheckFormatVerbNominalization());
    }
}