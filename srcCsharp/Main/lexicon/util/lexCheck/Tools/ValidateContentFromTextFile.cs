using System;
using System.Collections.Generic;
using SimpleNLG.Main.lexicon.util.lexCheck.CheckCont;

namespace SimpleNLG.Main.lexicon.util.lexCheck.Tools
{
    using CheckLexRecord = CheckLexRecord;
    using CheckVariants = CheckVariants;


    public class ValidateContentFromTextFile

    {
        public static void Main(string[] args)

        {
            string inFile = "LEXICON";
            string outFile = "LEXICON.fix";
            string prepositionFile = "../data/Files/prepositions.data";
            string irregExpFile = "../data/Files/irregExceptions.data";
            bool verbose = false;
            if (args.Length == 1)

            {
                inFile = args[0];
                outFile = inFile + ".fix";
            }
            else if (args.Length == 2)

            {
                inFile = args[0];
                outFile = args[1];
            }
            else if (args.Length == 3)

            {
                inFile = args[0];
                outFile = args[1];
                if (args[2].Equals("-v") == true)

                {
                    verbose = true;
                }
            }
            else if (args.Length == 5)

            {
                inFile = args[0];
                outFile = args[1];
                if (args[2].Equals("-v") == true)

                {
                    verbose = true;
                }

                prepositionFile = args[3];
                irregExpFile = args[4];
            }
            else if (args.Length >= 0)

            {
                Console.Error.WriteLine(
                    "** Usage: java ValidateContentFromTextFile <inFile(Text)> <outFile> <-v> <prepositionFile> <irregExpFile>");
                Console.Error.WriteLine("");
                Console.Error.WriteLine("Options:");
                Console.Error.WriteLine("  inFile: LEXICON in text format");
                Console.Error.WriteLine("  outFile: auto-fixed LEXICON in text format");
                Console.Error.WriteLine("  -v: verbose");
                Console.Error.WriteLine("  prepositionFile: preposition file");
                Console.Error.WriteLine("  irregExpFile: irreg exception EUI file");
                Environment.Exit(1);
            }

            if (verbose == true)

            {
                Console.WriteLine("===== ValidateContentFromTextFile( ) =====");
                Console.WriteLine("-- inFile: " + inFile);
                Console.WriteLine("-- prepositionFile: " + prepositionFile);
                Console.WriteLine("-- irregExpFile: " + irregExpFile);
            }

            Lib.GlobalVars.SetPrepositionFile(prepositionFile);


            int lineNum = 100;

            HashSet<string> irregExpEuiList = CheckVariants.GetIrregExpEuiListFromFile(irregExpFile, verbose, lineNum);


            CheckLexRecord.CheckLexRecordsFromFile(inFile, outFile, false, irregExpEuiList);
        }
    }
    
}