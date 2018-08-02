using System;
using System.Collections.Generic;

namespace SimpleNLG.Main.lexicon.util.lexCheck.Gram
{
    using CheckCatEntry = Cat.CheckCatEntry;
    using CheckFormatCat = Cat.CheckFormatCat;
    using CheckCode = CkLib.CheckCode;
    using CheckObject = CkLib.CheckObject;
    using CheckSt = Lib.CheckSt;
    using LexRecord = Lib.LexRecord;
    using LineObject = Lib.LineObject;


    public class CheckGrammer

    {
        public static bool Check(LineObject lineObject, bool printFlag, CheckSt st, CheckSt catSt, bool debugFlag)

        {
            bool flag = true;

            switch (st.GetCurState())

            {
                case 10:
                    lexRecord_ = new LexRecord();
                    flag = CheckCode.Check(lineObject, printFlag, st, lexRecord_, checkBase_, new UpdateBase(), 1,
                        true);

                    PrintStep(10, debugFlag, lineObject.GetLine());
                    break;
                case 20:
                    flag = CheckCode.Check(lineObject, printFlag, st, lexRecord_, checkSpellingVar_,
                        new UpdateSpellingVar(), 3, true);


                    PrintStep(20, debugFlag, lineObject.GetLine());

                    break;
                case 21:
                    flag = CheckCode.Check(lineObject, printFlag, st, lexRecord_, checkEntry_, new UpdateEntry(), 1,
                        true);

                    PrintStep(21, debugFlag, lineObject.GetLine());
                    break;
                case 30:
                    flag = CheckCode.Check(lineObject, printFlag, st, lexRecord_, checkCat_, new UpdateCat(), 1, true);

                    PrintStep(30, debugFlag, lineObject.GetLine());
                    break;
                case 40:
                    flag = CheckCatEntry.Check(lineObject, printFlag, st, catSt, lexRecord_, 91, debugFlag);

                    PrintStep(40, debugFlag, lineObject.GetLine());

                    break;
                case 91:
                    flag = CheckCode.Check(lineObject, printFlag, st, lexRecord_, checkAcronym_, new UpdateAcronym(), 3,
                        true);


                    PrintStep(91, debugFlag, lineObject.GetLine());

                    break;
                case 92:
                    flag = CheckCode.Check(lineObject, printFlag, st, lexRecord_, checkAbbreviation_,
                        new UpdateAbbreviation(), 3, true);


                    PrintStep(92, debugFlag, lineObject.GetLine());

                    break;
                case 95:
                    flag = CheckCode.Check(lineObject, printFlag, st, lexRecord_, checkAnnotation_,
                        new UpdateAnnotation(), 3, true);


                    PrintStep(95, debugFlag, lineObject.GetLine());

                    break;
                case 96:
                    flag = CheckCode.Check(lineObject, printFlag, st, lexRecord_, checkSignature_,
                        new UpdateSignature(), 2, true);


                    PrintStep(96, debugFlag, lineObject.GetLine());

                    break;
                case 99:
                    flag = CheckCode.Check(lineObject, printFlag, st, lexRecord_, checkEnd_, new UpdateEnd(), 5, false);


                    PrintStep(99, debugFlag, lineObject.GetLine());
                    break;
            }


            return flag;
        }


        public static LexRecord GetLexRecord()

        {
            return lexRecord_;
        }

        private static void PrintStep(int state, bool debugFlag, string line)

        {
            if (debugFlag == true)

            {
                switch (state)

                {
                    case 10:
                        Console.WriteLine("-- Checked Base: '" + line + "'");
                        break;
                    case 20:
                        Console.WriteLine("-- Checked SPELLING_VAR: '" + line + "'");

                        break;
                    case 21:
                        Console.WriteLine("-- Checked Entry: '" + line + "'");
                        break;
                    case 30:
                        Console.WriteLine("-- Checked Cat: '" + line + "'");
                        break;
                    case 40:
                        Console.WriteLine("-- Checked Cat Entry: '" + line + "'");
                        break;
                    case 91:
                        Console.WriteLine("-- Checked Acronym: '" + line + "'");
                        break;
                    case 92:
                        Console.WriteLine("-- Checked Abbreviation: '" + line + "'");

                        break;
                    case 95:
                        Console.WriteLine("-- Checked Annotation: '" + line + "'");
                        break;
                    case 96:
                        Console.WriteLine("-- Checked Signature: '" + line + "'");
                        break;
                    case 99:
                        Console.WriteLine("-- Checked End: '" + line + "'");
                        break;
                }
            }
        }

        private static LexRecord lexRecord_ = new LexRecord();
        private static CheckObject checkBase_ = null;
        private static CheckObject checkSpellingVar_ = null;
        private static HashSet<string> spellingVarNextStartStrs_ = new HashSet<string>();

        private static CheckObject checkEntry_ = null;
        private static CheckObject checkCat_ = null;
        private static CheckObject checkAcronym_ = null;
        private static HashSet<string> acronymNextStartStrs_ = new HashSet<string>();

        private static CheckObject checkAbbreviation_ = null;
        private static HashSet<string> abbreviationNextStartStrs_ = new HashSet<string>();

        private static CheckObject checkAnnotation_ = null;
        private static HashSet<string> annotationNextStartStrs_ = new HashSet<string>();


        private static HashSet<string> signatureNextStartStrs_ = new HashSet<string>();


        static CheckGrammer()

        {
            checkBase_ = new CheckObject("{base=", 3, -1, 20, null, null);


            spellingVarNextStartStrs_.Add("entry");
            checkSpellingVar_ = new CheckObject("spelling_variant=", 4, -1, 21, spellingVarNextStartStrs_, null);


            checkEntry_ = new CheckObject("entry=", 5, 6, 30, null, new CheckFormatEui());


            checkCat_ = new CheckObject("\tcat=", 8, 9, 40, null, new CheckFormatCat());


            acronymNextStartStrs_.Add("\tabbreviation_of=");
            acronymNextStartStrs_.Add("annotation=");
            acronymNextStartStrs_.Add("signature=");
            acronymNextStartStrs_.Add("}");
            checkAcronym_ = new CheckObject("\tacronym_of=", 18, 19, 92, acronymNextStartStrs_,
                new CheckFormatAcronym());


            abbreviationNextStartStrs_.Add("annotation=");
            abbreviationNextStartStrs_.Add("signature=");
            abbreviationNextStartStrs_.Add("}");
            checkAbbreviation_ = new CheckObject("\tabbreviation_of=", 16, 17, 95, abbreviationNextStartStrs_,
                new CheckFormatAbbreviation());


            annotationNextStartStrs_.Add("signature=");
            annotationNextStartStrs_.Add("}");
            checkAnnotation_ = new CheckObject("annotation=", 12, -1, 96, annotationNextStartStrs_, null);


            signatureNextStartStrs_.Add("}");
        }

        private static CheckObject checkSignature_ =
            new CheckObject("signature=", 13, -1, 99, signatureNextStartStrs_, null);


        private static CheckObject checkEnd_ = new CheckObject("}", 15, -1, 10, null, null);
    }
}