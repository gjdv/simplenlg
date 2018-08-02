using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleNLG.Main.lexicon.util.lexCheck.Gram;

namespace SimpleNLG.Main.lexicon.util.lexCheck.Api
{
    using CheckSt = Lib.CheckSt;
    using InflVar = Lib.InflVar;
    using LineObject = Lib.LineObject;


    public class ToXmlApi

    {
        [Obsolete]
        public static ApiOutput ToXmlFromFile(string inFile)


        {
            return ToXmlFromTextFile(inFile);
        }


        public static ApiOutput ToXmlFromTextFile(string inFile)

        {
            CheckSt st = new CheckSt();
            CheckSt catSt = new CheckSt(40);

            LineObject lineObject = new LineObject();
            StringBuilder xmlOut = new StringBuilder("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n");

            xmlOut.Append("<lexRecords>\n");
            int recordNum = 0;
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

                    recordNum = CheckLine(st, catSt, lineObject, xmlOut, recordNum);
                }

                inReader.Close();
            }
            catch (Exception e)

            {
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
            }

            xmlOut.Append("</lexRecords>\n");
            ApiOutput apiOutput = new ApiOutput(xmlOut.ToString(), recordNum);
            return apiOutput;
        }


        public static ApiOutput ToXmlFromText(string text)

        {
            CheckSt st = new CheckSt();
            CheckSt catSt = new CheckSt(40);

            LineObject lineObject = new LineObject();
            StringBuilder xmlOut = new StringBuilder("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n");

            xmlOut.Append("<lexRecords>\n");
            int recordNum = 0;

            string unixText = text.Replace("\r\n","\n");
            string[] buf = unixText.Split('\n').ToList().Where(x => x != "").ToArray();

            foreach (string line in buf)
            {
                if (lineObject.IsGoToNext() == true)

                {
                    lineObject.SetLine(line);
                    lineObject.IncreaseLineNum();
                }

                recordNum = CheckLine(st, catSt, lineObject, xmlOut, recordNum);
            }

            xmlOut.Append("</lexRecords>\n");
            ApiOutput apiOutput = new ApiOutput(xmlOut.ToString(), recordNum);
            return apiOutput;
        }


        [Obsolete]
        public static List<InflVar> GetInflVars(string inFile)


        {
            return ToInflVarsApi.GetInflVarsFromTextFile(inFile);
        }


        public static void Main(string[] args)

        {
            string bat = "{base=bat\nentry=E0012112\n\tcat=noun\n\tvariants=reg\nsignature=vanni\n}";

            string molt =
                "{base=molt\nspelling_variant=moult\nentry=E0040723\n\tcat=noun\n\tvariants=reg\nsignature=vanni\n}\n{base=molt\nspelling_variant=moult\nentry=E0040724\n\tcat=verb\n\tvariants=reg\n\tintran\n\ttran=np\n\tnominalization=molting|noun|E0412675\nsignature=vanni\n}";

            string inFile = "molt";


            Console.Write(ToXmlFromFile(inFile).GetXml());
            Console.WriteLine("---- Record Num: " + ToXmlFromFile(inFile).GetRecordNum());
        }

        private static int CheckLine(CheckSt st, CheckSt catSt, LineObject lineObject, StringBuilder xmlOut,
            int recordNum)

        {
            bool flag = false;
            bool printFlag = true;

            flag = CheckGrammer.Check(lineObject, printFlag, st, catSt, false);

            if ((st.GetCurState() == 10) && (flag == true))

            {
                xmlOut.Append(CheckGrammer.GetLexRecord().GetXml());
                recordNum++;
            }

            return recordNum;
        }
    }


}