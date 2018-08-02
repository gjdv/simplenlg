using System;
using System.Collections.Generic;
using SimpleNLG.Main.lexicon.util.lexCheck.CheckCont;

namespace SimpleNLG.Main.lexicon.util.lexCheck.Tools
{
    using CrossCheckLexRecords = CrossCheckLexRecords;
    using KeyValuesTable = KeyValuesTable;
    using SetTable = SetTable;


    public class LexCrossCheck

    {
        public static void Main(string[] args)

        {
            string inFile = "LEXICON";
            string autoFixFile = "LEXICON.fix";
            bool verbose = false;
            string prepositionFile = "../data/Files/prepositions.data";
            string dupRecExpFile = "../data/Files/dupRecExceptions.data";
            string notBaseFormFile = "../data/Files/notBaseForm.data";

            if (args.Length == 6)

            {
                inFile = args[0];
                autoFixFile = args[1];
                if (args[2].Equals("-v") == true)

                {
                    verbose = true;
                }

                prepositionFile = args[3];
                dupRecExpFile = args[4];
                notBaseFormFile = args[5];
            }
            else if (args.Length == 3)

            {
                inFile = args[0];
                autoFixFile = args[1];
                if (args[2].Equals("-v") == true)

                {
                    verbose = true;
                }
            }
            else if (args.Length == 2)

            {
                inFile = args[0];
                autoFixFile = args[1];
            }
            else if (args.Length == 1)

            {
                inFile = args[0];
                autoFixFile = inFile + ".fix";
            }
            else if (args.Length >= 0)

            {
                Console.Error.WriteLine(
                    "** Usage: java LexCrossCheck <inFile> <outFile> <-v> <prepositionFile> <dupRecExpFile> <notBaseFormFile>");
                Console.Error.WriteLine("");
                Console.Error.WriteLine("Options:");
                Console.Error.WriteLine("  inFile: LEXICON in text format");
                Console.Error.WriteLine("  outFile: auto-fixed LEXICON in text format");
                Console.Error.WriteLine("  -v: verbose");
                Console.Error.WriteLine("  prepositionFile: preposition file");
                Console.Error.WriteLine("  dupRecExpFile: dup record exception file");
                Console.Error.WriteLine("  notBaseFormFile: not base form file");
                Environment.Exit(1);
            }

            if (verbose == true)

            {
                Console.WriteLine("===== LexCrossCheck( ) =====");
                Console.WriteLine("-- inFile: " + inFile);
                Console.WriteLine("-- autoFixFile: " + autoFixFile);
                Console.WriteLine("-- prepositionFile: " + prepositionFile);
                Console.WriteLine("-- dupRecExpFile: " + dupRecExpFile);
                Console.WriteLine("-- notBaseFormFile: " + notBaseFormFile);
            }

            Lib.GlobalVars.SetPrepositionFile(prepositionFile);

            int lineNum = 500;

            Dictionary<string, HashSet<string>> dupRecExpList =
                KeyValuesTable.GetKey2ValuesTable(dupRecExpFile, verbose, lineNum);


            bool useDefaultFileFlag = false;

            HashSet<string> notBaseFormSet = SetTable.GetSetTabe(notBaseFormFile, verbose, lineNum, useDefaultFileFlag);


            CrossCheckLexRecords.CheckFromFile(inFile, autoFixFile, verbose, dupRecExpList, notBaseFormSet);
        }
    }

}