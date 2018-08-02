using System;
using System.Collections.Generic;
using System.Text;
using SimpleNLG.Main.lexicon.util.lexCheck.Gram;

namespace SimpleNLG.Main.lexicon.util.lexCheck.Api
{
    using CheckSt = Lib.CheckSt;
    using InflVar = Lib.InflVar;
    using InflVarsAndAgreements = Lib.InflVarsAndAgreements;
    using LineObject = Lib.LineObject;


    public class ToInflVarsApi

    {
        public static List<InflVar> GetInflVarsFromTextFile(string inFile)

        {
            CheckSt st = new CheckSt();
            CheckSt catSt = new CheckSt(40);

            LineObject lineObject = new LineObject();
            List<InflVar> inflVars = new List<InflVar>();
            try

            {
                System.IO.StreamReader inReader = new System.IO.StreamReader(
                    new System.IO.FileStream(inFile, System.IO.FileMode.Open, System.IO.FileAccess.Read),
                    Encoding.UTF8);

                while (lineObject != null)

                {
                    if (lineObject.IsGoToNext() == true)

                    {
                        lineObject.SetLine(inReader.ReadLine());
                        lineObject.IncreaseLineNum();
                    }

                    if (lineObject.GetLine() == null)
                    {
                        break;
                    }


                    bool flag = CheckGrammer.Check(lineObject, true, st, catSt, false);

                    if ((st.GetCurState() == 10) && (flag == true))

                    {
                        InflVarsAndAgreements infls = new InflVarsAndAgreements(CheckGrammer.GetLexRecord());
                        inflVars.AddRange(infls.GetInflValues());
                    }
                }

                inReader.Close();
            }
            catch (Exception e)

            {
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
            }

            return inflVars;
        }


        public static void Main(string[] args)

        {
            string inFile = "molt";

            List<InflVar> temp = GetInflVarsFromTextFile(inFile);
        }
    }


}