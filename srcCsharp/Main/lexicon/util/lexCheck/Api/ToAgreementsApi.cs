using System;
using System.Collections.Generic;
using System.Text;
using SimpleNLG.Main.lexicon.util.lexCheck.Gram;

namespace SimpleNLG.Main.lexicon.util.lexCheck.Api
{
    using Agreement = Lib.Agreement;
    using CheckSt = Lib.CheckSt;
    using InflVarsAndAgreements = Lib.InflVarsAndAgreements;
    using LineObject = Lib.LineObject;


    public class ToAgreementsApi

    {
        public static List<Agreement> GetAgreementsFromTextFile(string inFile)

        {
            CheckSt st = new CheckSt();
            CheckSt catSt = new CheckSt(40);

            LineObject lineObject = new LineObject();
            List<Agreement> agreements = new List<Agreement>();
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
                        InflVarsAndAgreements agrs = new InflVarsAndAgreements(CheckGrammer.GetLexRecord());
                        agreements.AddRange(agrs.GetAgreementValues());
                    }
                }

                inReader.Close();
            }
            catch (Exception e)

            {
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
            }

            return agreements;
        }


        public static void Main(string[] args)

        {
            string inFile = "molt";


            List<Agreement> temp = GetAgreementsFromTextFile(inFile);
        }
    }


}