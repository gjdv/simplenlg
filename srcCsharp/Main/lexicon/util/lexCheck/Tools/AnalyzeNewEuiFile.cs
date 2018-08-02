using System;

namespace SimpleNLG.Main.lexicon.util.lexCheck.Tools
{
    public class AnalyzeNewEuiFile

    {
        private const string SP = "|";

        public static void Main(string[] args)

        {
            string topDir = "/nfsvol/lex/Lu/Development/Lexicon/data/";

            string year = "2015";
            string inFileName = "2.2.4.newEuis.acr.tagged.txt";
            if (args.Length == 2)

            {
                year = args[0];
                inFileName = args[1];
            }
            else if (args.Length == 1)

            {
                year = args[0];
            }
            else if (args.Length > 0)

            {
                Console.Error.WriteLine("Usage: java AnalyzeNewEuiFile <year> <inFile>");
                Environment.Exit(0);
            }

            string inFile = topDir + year + "/data/Tags/" + inFileName;
            AnalyzeNewEuiFromFile(inFile);
        }

        private static void AnalyzeNewEuiFromFile(string inFile)

        {
            Console.WriteLine("===== AnalyzeNewEuiFromFile =====");
            int lineNo = 0;
            string line = null;
            string yFile = inFile + ".y";
            string nFile = inFile + ".n";
            string oFile = inFile + ".o";
            string outFile = inFile + ".out";
            try

            {
                System.IO.StreamReader reader =
                    Files.newBufferedReader(Paths.get(inFile, new string[0]), Charset.forName("UTF-8"));

                System.IO.StreamWriter yWriter = Files.newBufferedWriter(Paths.get(yFile, new string[0]),
                    Charset.forName("UTF-8"), new OpenOption[0]);

                System.IO.StreamWriter nWriter = Files.newBufferedWriter(Paths.get(nFile, new string[0]),
                    Charset.forName("UTF-8"), new OpenOption[0]);

                System.IO.StreamWriter oWriter = Files.newBufferedWriter(Paths.get(oFile, new string[0]),
                    Charset.forName("UTF-8"), new OpenOption[0]);

                System.IO.StreamWriter outWriter = Files.newBufferedWriter(Paths.get(outFile, new string[0]),
                    Charset.forName("UTF-8"), new OpenOption[0]);


                int yNo = 0;
                int nNo = 0;
                int oNo = 0;

                while (!string.ReferenceEquals((line = reader.ReadLine()), null))

                {
                    lineNo++;
                    if (!line.StartsWith("#", StringComparison.Ordinal))

                    {
                        if (line.EndsWith("|Y", StringComparison.Ordinal) == true)

                        {
                            yNo++;
                            yWriter.Write(line);
                            yWriter.WriteLine();
                        }
                        else if (line.EndsWith("|N", StringComparison.Ordinal) == true)

                        {
                            nNo++;
                            nWriter.Write(line);
                            nWriter.WriteLine();

                            string outStr = GetExpansionPos(line);
                            outWriter.Write(outStr);
                            outWriter.WriteLine();
                        }
                        else

                        {
                            oNo++;
                            oWriter.Write(line);
                            oWriter.WriteLine();
                        }
                    }
                }

                reader.Close();
                yWriter.Close();
                nWriter.Close();
                oWriter.Close();
                outWriter.Close();

                Console.WriteLine("- lineNo: " + lineNo);
                Console.WriteLine("- Y tag No: " + yNo);
                Console.WriteLine("- N tag No: " + nNo);
                Console.WriteLine("- Other tag No: " + oNo + " (should be 0)");
            }
            catch (Exception x)

            {
                Console.WriteLine("** ERR@ " + lineNo + " :[" + line + "] " + x.ToString());
            }
        }

        private static string GetExpansionPos(string inStr)

        {
            string expStartStr = " - new EUI (";
            string expEndStr = " - New): @ [";
            int expStartStrSize = 12;
            int expEndStrSize = 12;
            int expStartIndex = inStr.IndexOf(expStartStr, StringComparison.Ordinal) + expStartStrSize;
            int expEndIndex = inStr.IndexOf(expEndStr, StringComparison.Ordinal);
            string expansion = inStr.Substring(expStartIndex, expEndIndex - expStartIndex);


            string posEndStr = "] => Manually add a new record to To-Do list";
            int posEndIndex = inStr.IndexOf(posEndStr, StringComparison.Ordinal);

            int posStartIndex = inStr.IndexOf("|", expEndIndex + 9 + expEndStrSize, StringComparison.Ordinal);
            string pos = inStr.Substring(posStartIndex + 1, posEndIndex - (posStartIndex + 1));
            string outStr = expansion + "|" + pos;
            return outStr;
        }
    }
}