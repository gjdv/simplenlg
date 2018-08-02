using System;
using SimpleNLG.Main.lexicon.util.lexCheck.Api;

namespace SimpleNLG.Main.lexicon.util.lexCheck.Tools
{
    using ToXmlApi = ToXmlApi;


    public class ToXmlFromTextFile

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
                Console.Error.WriteLine("** Usage: java ToXmlFromTextFile <inFile> <verbose> <prepositionFile>");
                Environment.Exit(1);
            }

            Lib.GlobalVars.SetPrepositionFile(prepositionFile);


            if (verbose == true)

            {
                Console.WriteLine("===== ToXmlFromTextFile.( ) ======");
                Console.WriteLine("-- inFile: [" + inFile + "]");
                Console.WriteLine("-- prepositionsFile: [" + Lib.GlobalVars.GetPrepositionFile() + "]");
            }

            Console.Write(ToXmlApi.ToXmlFromTextFile(inFile).GetXml());
        }
    }
    
}