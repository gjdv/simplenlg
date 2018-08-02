using System;
using SimpleNLG.Main.lexicon.util.lexCheck.Api;

namespace SimpleNLG.Main.lexicon.util.lexCheck.Tools
{
    using ToJavaObjApi = ToJavaObjApi;


    public class ValidateSyntaxFromTextFile

    {
        public static void Main(string[] args)

        {
            string inFile = "LEXICON";
            string prepositionFile = "../data/Files/prepositions.data";
            bool verbose = false;
            if (args.Length == 1)

            {
                inFile = args[0];
            }
            else if (args.Length == 2)

            {
                inFile = args[0];
                if (args[1].Equals("-v") == true)

                {
                    verbose = true;
                }
            }
            else if (args.Length == 3)

            {
                inFile = args[0];
                if (args[1].Equals("-v") == true)

                {
                    verbose = true;
                }

                prepositionFile = args[2];
            }
            else if (args.Length >= 0)

            {
                Console.Error.WriteLine(
                    "** Usage: java ValidateSyntaxFromTextFile <inFile> <verbose> <prepositionFile>");
                Environment.Exit(1);
            }

            int recNum = 0;
            try

            {
                recNum = ToJavaObjApi.CheckRecordsFromTextFile(inFile, prepositionFile);
            }
            catch (Exception e)

            {
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
            }

            if (verbose == true)

            {
                Console.WriteLine("===== ValidateSyntaxFromTextFile( ) ======");
                Console.WriteLine("-- inFile: [" + inFile + "]");
                Console.WriteLine("-- prepositionsFile: [" + Lib.GlobalVars.GetPrepositionFile() + "]");
            }

            if (recNum > 0)

            {
                Console.WriteLine("-- inFile(" + inFile + "): " + recNum + " records checked. No error found.");
            }
        }
    }
    
}