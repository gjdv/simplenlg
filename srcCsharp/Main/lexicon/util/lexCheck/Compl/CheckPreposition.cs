using System;
using System.Collections.Generic;
using System.Text;
using SimpleNLG.Main.lexicon.util.lexCheck.Lib;

namespace SimpleNLG.Main.lexicon.util.lexCheck.Compl
{
    using GlobalVars = GlobalVars;


    public class CheckPreposition

    {
        private const int PREPOSITION_NUM = 200;

        public CheckPreposition()

        {
            string inFile = GlobalVars.GetPrepositionFile();
            Init(inFile);
        }


        public static bool IsLegal(string term)

        {
            if (preposition_.Count == 0)

            {
                string prepositionFile = GlobalVars.GetPrepositionFile();
                Init(prepositionFile);
            }

            bool flag = preposition_.Contains(term);
            return flag;
        }

        private static void Init(string inFile)

        {
            string line = null;
            int prepNo = 0;
            try

            {
                System.IO.StreamReader @in = null;

                if ((inFile.Length != 0) &&
                    (System.IO.Directory.Exists(inFile) || System.IO.File.Exists(inFile) == true))


                {
                    @in = new System.IO.StreamReader(
                        new System.IO.FileStream(inFile, System.IO.FileMode.Open, System.IO.FileAccess.Read),
                        Encoding.UTF8);
                }
                else

                {
                    inFile = "Resources/NIHLexicon/prepositions.data";

                    @in = new System.IO.StreamReader(inFile);
                }

                GlobalVars.SetPrepositionFile(inFile);

                while (!ReferenceEquals((line = @in.ReadLine()), null))

                {
                    if ((line.Length > 0) && (line[0] != '#'))

                    {
                        prepNo++;
                        preposition_.Add(line);
                    }
                }

                @in.Close();
            }
            catch (Exception e)

            {
                Console.Error.WriteLine("** Err: problem of reading file: " + inFile + " at line: " + prepNo);

                Console.Error.WriteLine("Exception: " + e.ToString());
            }
        }


        private static HashSet<string> preposition_ = new HashSet<string>();
    }
}