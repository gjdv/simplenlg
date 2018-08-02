using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace SimpleNLG.Main.lexicon.util.lexCheck.CheckCont
{
    public class SetTable

    {
        public static bool InTable(string inStr, HashSet<string> setTable)

        {
            return setTable.Contains(inStr);
        }


        public static HashSet<string> GetSetTabe(string inFile, bool verbose, int lineNum, bool useDefaultFileFlag)

        {
            string line = null;
            int lineNo = 0;
            int dupNo = 0;
            HashSet<string> setTable = new HashSet<string>();
            List<string> duplicates = new List<string>();
            try

            {
                TextReader @in = null;
                
                if (useDefaultFileFlag == true)

                {
                    @in = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream(inFile));
                }
                else

                {
                    @in = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream(inFile),Encoding.UTF8);
                }

                while (!ReferenceEquals((line = @in.ReadLine()), null))

                {
                    if ((line.Length > 0) && (line[0] != '#'))

                    {
                        if (!setTable.Add(line))

                        {
                            duplicates.Add(line);
                            dupNo++;
                        }

                        lineNo++;
                    }

                    if ((verbose == true) && (lineNo % lineNum == 0) && (lineNo != 0))

                    {
                        Console.WriteLine("-- current line: " + lineNo);
                    }
                }

                if (verbose == true)

                {
                    Console.WriteLine("-----------------------------");
                    Console.WriteLine("--- Total line: " + lineNo);
                    Console.WriteLine("--- Total duplicates: " + dupNo);
                    Console.WriteLine(VectorToString(duplicates));
                }

                @in.Close();
            }
            catch (Exception e)

            {
                Console.Error.WriteLine("** ERR@SetTable.GetSetTabe (" + inFile + ")");
                Console.Error.WriteLine("** ERR@(" + lineNo + "): " + line);
                Console.Error.WriteLine("Exception: " + e.ToString());
            }

            return setTable;
        }

        public static string VectorToString(List<string> inList)

        {
            string outStr = "";
            foreach (string inItem in inList)

            {
                outStr = outStr + inItem + Lib.GlobalVars.LS_STR;
            }

            return outStr;
        }
    }


}