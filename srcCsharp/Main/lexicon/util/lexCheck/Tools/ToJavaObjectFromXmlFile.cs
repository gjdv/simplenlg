using System;
using System.Collections.Generic;
using SimpleNLG.Main.lexicon.util.lexCheck.Api;
using SimpleNLG.Main.lexicon.util.lexCheck.Lib;
using InflVar = SimpleNLG.Main.lexicon.util.lexCheck.Lib.InflVar;

namespace SimpleNLG.Main.lexicon.util.lexCheck.Tools
{
    using ToJavaObjApi = ToJavaObjApi;
    using InflVar = lexCheck.Lib.InflVar;
    using LexRecord = LexRecord;
    using Category = gov.nih.nlm.nls.lvg.Lib.Category;
    using Inflection = gov.nih.nlm.nls.lvg.Lib.Inflection;

    public class ToJavaObjectFromXmlFile

    {
        public static void Main(string[] args)

        {
            if ((args.Length == 0) || (args.Length > 2))

            {
                Console.Error.WriteLine("** Usage: java ToJavaObjectFromXmlFile <inFile(Xml)> <-i>");
                Console.Error.WriteLine("");
                Console.Error.WriteLine("Options:");
                Console.Error.WriteLine("  -i: generate inflection vars");
                Environment.Exit(1);
            }

            bool inflVarFlag = false;
            for (int i = 0; i < args.Length; i++)

            {
                if (args[i].Equals("-i"))

                {
                    inflVarFlag = true;
                    break;
                }
            }

            try

            {
                List<LexRecord> lexRecords = ToJavaObjApi.ToJavaObjsFromXmlFile(args[0]);
                if (lexRecords.Count <= 0)

                {
                    Environment.Exit(1);
                }
                else

                {
                    for (int i = 0; i < lexRecords.Count; i++)

                    {
                        LexRecord lexRecord = (LexRecord) lexRecords[i];
                        Console.Write(lexRecord.GetText());
                        if (inflVarFlag == true)

                        {
                            Console.WriteLine("---------- Inflection Vars ----------");

                            List<InflVar> inflVars = lexRecord.GetInflVarsAndAgreements().GetInflValues();
                            for (int j = 0; j < inflVars.Count; j++)

                            {
                                InflVar inflVar = (InflVar) inflVars[j];
                                Console.WriteLine(inflVar.GetVar() + "|" + Category.ToValue(inflVar.GetCat()) + "|" +
                                                  Inflection.ToValue(inflVar.GetInflection()) + "|" + inflVar.GetEui() +
                                                  "|" + inflVar.GetUnInfl() + "|" + inflVar.GetCit());
                            }
                        }
                    }
                }
            }
            catch (Exception e)

            {
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
            }
        }
    }
    
}