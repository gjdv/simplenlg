using System;
using System.Collections.Generic;
using java.nio.charset;
using SimpleNLG.Main.features;
using SimpleNLG.Main.lexicon.util.lexCheck.Api;
using SimpleNLG.Main.lexicon.util.lexCheck.Lib;

namespace SimpleNLG.Main.lexicon.util.lexCheck.Tools
{
    using ToInflVarsApi = ToInflVarsApi;
    using InflVar = lexCheck.Lib.InflVar;
    using InflVarComparator = lexCheck.Lib.InflVarComparator<InflVar>;


    public class GenerateInflVars

    {
        public static void Main(string[] args)

        {
            string inFile = "LEXICON";
            string outFile = "inflVars.data";

            if (args.Length == 2)

            {
                inFile = args[0];
                outFile = args[1];
            }
            else if (args.Length > 0)

            {
                Console.WriteLine("** Usage: java GenerateInflVars <inFile> <outFile>");
                Environment.Exit(0);
            }

            Console.WriteLine("-- inFile: [" + inFile + "]");
            Console.WriteLine("-- outFile: [" + outFile + "]");


            try

            {
                List<InflVar> inflVars = ToInflVarsApi.GetInflVarsFromTextFile(inFile);
                InflVarComparator<InflVar> comp = new InflVarComparator();
                inflVars.Sort(comp);

                System.IO.StreamWriter outWriter = Files.newBufferedWriter(Paths.get(outFile, new string[0]),
                    Charset.forName("UTF-8"), new OpenOption[0]);


                for (int i = 0; i < inflVars.Count; i++)

                {
                    InflVar inflVar = (InflVar) inflVars[i];
                    if (inflVar.GetUnique() == true)

                    {
                        string outStr = inflVar.GetVar() + "|" + Category.ToValue(inflVar.GetCat()) + "|" +
                                        Inflection.ToValue(inflVar.GetInflection()) + "|" + inflVar.GetEui() + "|" +
                                        inflVar.GetUnInfl() + "|" + inflVar.GetCit();
                        outWriter.Write(outStr);
                        outWriter.WriteLine();
                    }
                }

                outWriter.Close();
            }
            catch (Exception x)

            {
                Console.WriteLine("** ERR@GenerateInflVars( ): " + x.ToString());
            }
        }
    }

}