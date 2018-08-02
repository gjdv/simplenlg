using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using SimpleNLG.Main.lexicon.util.lexCheck.Gram;
using SimpleNLG.Main.lexicon.util.lexCheck.Lib;

namespace SimpleNLG.Main.lexicon.util.lexCheck.Api
{
    using Document = XmlDocument;
    using NamedNodeMap = XmlNamedNodeMap;
    using Node = XmlNode;


    public static class ToJavaObjApi

    {
        [Obsolete]
        public static List<LexRecord> ToJavaObjs(string xmlFileName)


        {
            return ToJavaObjsFromXmlFile(xmlFileName);
        }


        public static List<LexRecord> ToJavaObjsFromXmlFile(string xmlFileName)


        {
            System.IO.Stream @is =
                new System.IO.FileStream(xmlFileName, System.IO.FileMode.Open, System.IO.FileAccess.Read);

            Document doc = new Document();
            doc.Load(@is);

            List<LexRecord> lexRecords = GetLexRecords(doc);
            return lexRecords;
        }


        public static List<LexRecord> ToJavaObjsFromTextFile(string inFile)

        {
            CheckSt st = new CheckSt();
            CheckSt catSt = new CheckSt(40);

            LineObject lineObject = new LineObject();
            int recordNum = 0;
            List<LexRecord> lexRecords = new List<LexRecord>();
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

                    recordNum = CheckLine(st, catSt, lineObject, lexRecords, recordNum);
                }

                inReader.Close();
            }
            catch (Exception e)

            {
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
            }

            return lexRecords;
        }


        public static List<LexRecord> ToJavaObjsFromText(string text)

        {
            CheckSt st = new CheckSt();
            CheckSt catSt = new CheckSt(40);

            LineObject lineObject = new LineObject();

            string unixText = text.Replace("\r\n","\n").Trim();
            List<LexRecord> lexRecords = new List<LexRecord>();
            if (!string.IsNullOrEmpty(unixText))
            {
                string[] buf = unixText.Split('\n').ToList().Where(x => x != "").ToArray();

                int i = -1;
                while (lineObject != null)
                {
                    if (lineObject.IsGoToNext() == true)
                    {
                        i += 1;
                        if (i >= buf.Length)
                        {
                            break;
                        }
                        string line = buf[i];
                        lineObject.SetLine(line);
                        lineObject.IncreaseLineNum();
                    }

                    CheckLine(st, catSt, lineObject, lexRecords);
                }
            }

            return lexRecords;
        }


        public static LexRecord ToJavaObjFromText(string text)

        {
            ErrMsg.ResetErrMsg();
            CheckSt st = new CheckSt();
            CheckSt catSt = new CheckSt(40);

            LineObject lineObject = new LineObject();

            LexRecord lexRecord = new LexRecord();
            string unixText = text.Replace("\r\n", "\n");
            List<LexRecord> lexRecords = new List<LexRecord>();

            if (!string.IsNullOrEmpty(unixText))
            {
                string[] buf = unixText.Split('\n').ToList().Where(x => x != "").ToArray();

                int i = -1;
                while (lineObject != null)
                {
                    if (lineObject.IsGoToNext() == true)
                    {
                        i += 1;
                        if (i >= buf.Length)
                        {
                            break;
                        }
                        string line = buf[i];
                        lineObject.SetLine(line);
                        lineObject.IncreaseLineNum();
                    }

                    lexRecord = CheckLine(st, catSt, lineObject);
                }
            }

            return lexRecord;
        }


        public static bool CheckRecordText(string text)

        {
            ErrMsg.ResetErrMsg();
            CheckSt st = new CheckSt();
            CheckSt catSt = new CheckSt(40);

            LineObject lineObject = new LineObject();

            string unixText = text.Replace("\r\n", "\n");
            List<LexRecord> lexRecords = new List<LexRecord>();

            if (!string.IsNullOrEmpty(unixText))
            {
                string[] buf = unixText.Split('\n').ToList().Where(x => x != "").ToArray();

                int i = -1;
                while (lineObject != null)
                {
                    if (lineObject.IsGoToNext() == true)
                    {
                        i += 1;
                        if (i >= buf.Length)
                        {
                            break;
                        }
                        string line = buf[i];
                        lineObject.SetLine(line);
                        lineObject.IncreaseLineNum();
                    }
                    else if (!CheckLine2(st, catSt, lineObject))

                    {
                        return false;
                    }
                }
            }

            return true;
        }


        public static int CheckRecordsFromTextFile(string inFile)


        {
            string prepositionFile = "";
            return CheckRecordsFromTextFile(inFile, prepositionFile);
        }


        //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public static int CheckRecordsFromTextFile(String inFile, String prepositionFile) throws Exception
        public static int CheckRecordsFromTextFile(string inFile, string prepositionFile)


        {
            GlobalVars.SetPrepositionFile(prepositionFile);

            lexRecNum_ = 0;
            CheckSt st = new CheckSt();
            CheckSt catSt = new CheckSt(40);

            LineObject lineObject = new LineObject();
            System.IO.StreamReader inReader = new System.IO.StreamReader(
                new System.IO.FileStream(inFile, System.IO.FileMode.Open, System.IO.FileAccess.Read), Encoding.UTF8);

            while (lineObject != null)

            {
                if (lineObject.IsGoToNext() == true)

                {
                    string line = inReader.ReadLine();
                    if (!ReferenceEquals(line, null))

                    {
                        lineObject.SetLine(line);
                        lineObject.IncreaseLineNum();
                    }
                }
                else if (!CheckLine2(st, catSt, lineObject))

                {
                    return -1;
                }
            }

            inReader.Close();
            return lexRecNum_;
        }


        public static string GetErrMsg()

        {
            return ErrMsg.GetErrMsg();
        }


        public static void Main(string[] args)

        {
            string bat = "{base=bat\nentry=E0012112\n\tcat=noun\n\tvariants=reg\nsignature=vanni\n}";

            string molt =
                "{base=molt\nspelling_variant=moult\nentry=E0040723\n\tcat=noun\n\tvariants=reg\n\tacronym_of=test123\n\tabbreviation_of=test456\n\tacronym_of=test789\nsignature=vanni\n}\n{base=molt\nspelling_variant=moult\nentry=E0040724\n\tcat=verb\n\tvariants=reg\n\tintran\n\ttran=np\n\tnominalization=molting|noun|E0412675\nsignature=vanni\n}";

            if (args.Length == 0)

            {
                try

                {
                    LexRecord lexRecord;
                    List<LexRecord> lexRecords = ToJavaObjsFromText(molt);
                    for (int i = 0; i < lexRecords.Count; i++)

                    {
                        lexRecord = (LexRecord) lexRecords[i];
                        Console.WriteLine(lexRecord.GetText());

                        Console.WriteLine("---------------------------------");
                    }

                    lexRecord = ToJavaObjFromText(molt);
                    Console.WriteLine(lexRecord.GetText());
                }
                catch (Exception e)

                {
                    Console.WriteLine(e.ToString());
                    Console.Write(e.StackTrace);
                }
            }
            else if (args.Length == 1)

            {
                try

                {
                    List<LexRecord> lexRecords = ToJavaObjs(args[0]);
                    if (lexRecords.Count <= 0)

                    {
                        Environment.Exit(1);
                    }
                    else

                    {
                        for (int i = 0; i < lexRecords.Count; i++)

                        {
                            LexRecord lexRecord = (LexRecord) lexRecords[i];
                            Console.WriteLine(lexRecord.GetText());
                            Console.WriteLine(lexRecord.GetXml());
                            Console.WriteLine("---------------------------------");
                            InflVarsAndAgreements infls = new InflVarsAndAgreements(lexRecord);

                            List<InflVar> inflVars = infls.GetInflValues();
                            for (int j = 0; j < inflVars.Count; j++)

                            {
                                InflVar inflVar = (InflVar) inflVars[j];
                                Console.WriteLine("unInfl: " + inflVar.GetUnInfl() + "; infl: " +
                                                  inflVar.GetInflection() + "; var: " + inflVar.GetVar());
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
            else

            {
                Console.Error.WriteLine("*** Usage: java ToJavaObjApi <filename>");
                Environment.Exit(1);
            }
        }


        private static List<LexRecord> GetLexRecords(Node node)


        {
            List<LexRecord> lexRecords = new List<LexRecord>();

            LexRecord lexRecord = null;
            CatEntry catEntry = null;

            for (Node records = node.FirstChild; records != null; records = records.NextSibling)


            {
                for (Node record = records.FirstChild; record != null; record = record.NextSibling)


                {
                    if (record.NodeType == XmlNodeType.Element)

                    {
                        if (record.Name.Equals("lexRecord"))

                        {
                            lexRecord = new LexRecord();
                            GetLexRecord(record, lexRecord, catEntry);
                        }

                        lexRecords.Add(lexRecord);
                    }
                }
            }

            return lexRecords;
        }

        private static void GetLexRecord(Node node, LexRecord lexRecord, CatEntry catEntry)


        {
            if (node.NodeType == XmlNodeType.Element)

            {
                if (node.Name.Equals("base"))

                {
                    lexRecord.SetBase(getValue(node));
                }
                else if (node.Name.Equals("eui"))

                {
                    lexRecord.SetEui(getValue(node));
                }
                else if (node.Name.Equals("cat"))
                {
                    lexRecord.SetCat(getValue(node));
                }
                else if (node.Name.Equals("spellingVars"))

                {
                    lexRecord.SetSpellingVar(getValue(node));
                }
                else if (node.Name.Equals("acronyms"))

                {
                    lexRecord.GetAcronyms().Add(getValue(node));
                }
                else if (node.Name.Equals("abbreviations"))

                {
                    lexRecord.GetAbbreviations().Add(getValue(node));
                }
                else if (node.Name.Equals("annotations"))

                {
                    lexRecord.GetAnnotations().Add(getValue(node));
                }
                else if (node.Name.Equals("signature"))

                {
                    lexRecord.SetSignature(getValue(node));
                }
                else if (node.Name.Equals("verbEntry"))

                {
                    catEntry = getVerbValues(node);
                    lexRecord.SetCatEntry(catEntry);
                }
                else if (node.Name.Equals("auxEntry"))

                {
                    catEntry = getAuxValues(node);
                    lexRecord.SetCatEntry(catEntry);
                }
                else if (node.Name.Equals("modalEntry"))

                {
                    catEntry = getModalValues(node);
                    lexRecord.SetCatEntry(catEntry);
                }
                else if (node.Name.Equals("nounEntry"))

                {
                    catEntry = getNounValues(node);
                    lexRecord.SetCatEntry(catEntry);
                }
                else if (node.Name.Equals("pronEntry"))

                {
                    catEntry = getPronValues(node);
                    lexRecord.SetCatEntry(catEntry);
                }
                else if (node.Name.Equals("adjEntry"))

                {
                    catEntry = getAdjValues(node);
                    lexRecord.SetCatEntry(catEntry);
                }
                else if (node.Name.Equals("advEntry"))

                {
                    catEntry = getAdvValues(node);
                    lexRecord.SetCatEntry(catEntry);
                }
                else if (node.Name.Equals("detEntry"))

                {
                    catEntry = getDetValues(node);
                    lexRecord.SetCatEntry(catEntry);
                }
            }

            for (Node child = node.FirstChild; child != null; child = child.NextSibling)


            {
                GetLexRecord(child, lexRecord, catEntry);
            }
        }

        private static CatEntry getVerbValues(Node node)
        {
            CatEntry catEntry = new CatEntry("verb");

            for (Node ch = node.FirstChild; ch != null; ch = ch.NextSibling)

            {
                if (ch.NodeType == XmlNodeType.Element)

                {
                    if (ch.Name.Equals("variants"))

                    {
                        catEntry.GetVerbEntry().GetVariants().Add(getValue(ch));
                    }
                    else if (ch.Name.Equals("intran"))

                    {
                        catEntry.GetVerbEntry().GetIntran().Add(getValue(ch));
                    }
                    else if (ch.Name.Equals("tran"))

                    {
                        catEntry.GetVerbEntry().GetTran().Add(getValue(ch));
                    }
                    else if (ch.Name.Equals("ditran"))

                    {
                        catEntry.GetVerbEntry().GetDitran().Add(getValue(ch));
                    }
                    else if (ch.Name.Equals("link"))

                    {
                        catEntry.GetVerbEntry().GetLink().Add(getValue(ch));
                    }
                    else if (ch.Name.Equals("cplxtran"))

                    {
                        catEntry.GetVerbEntry().GetCplxtran().Add(getValue(ch));
                    }
                    else if (ch.Name.Equals("nominalization"))

                    {
                        catEntry.GetVerbEntry().GetNominalization().Add(getValue(ch));
                    }
                }
            }

            return catEntry;
        }

        private static CatEntry getAuxValues(Node node)
        {
            CatEntry catEntry = new CatEntry("aux");

            for (Node ch = node.FirstChild; ch != null; ch = ch.NextSibling)

            {
                if (ch.NodeType == XmlNodeType.Element)

                {
                    if (ch.Name.Equals("variant"))

                    {
                        catEntry.GetAuxEntry().GetVariant().Add(getValue(ch));
                    }
                }
            }

            return catEntry;
        }

        private static CatEntry getModalValues(Node node)
        {
            CatEntry catEntry = new CatEntry("modal");

            for (Node ch = node.FirstChild; ch != null; ch = ch.NextSibling)

            {
                if (ch.NodeType == XmlNodeType.Element)

                {
                    if (ch.Name.Equals("variant"))

                    {
                        catEntry.GetModalEntry().GetVariant().Add(getValue(ch));
                    }
                }
            }

            return catEntry;
        }

        private static CatEntry getNounValues(Node node)
        {
            CatEntry catEntry = new CatEntry("noun");

            for (Node ch = node.FirstChild; ch != null; ch = ch.NextSibling)

            {
                if (ch.NodeType == XmlNodeType.Element)

                {
                    if (ch.Name.Equals("variants"))

                    {
                        catEntry.GetNounEntry().GetVariants().Add(getValue(ch));
                    }
                    else if (ch.Name.Equals("compl"))

                    {
                        catEntry.GetNounEntry().GetCompl().Add(getValue(ch));
                    }
                    else if (ch.Name.Equals("nominalization"))

                    {
                        catEntry.GetNounEntry().GetNominalization().Add(getValue(ch));
                    }
                    else if (ch.Name.Equals("tradeName"))

                    {
                        catEntry.GetNounEntry().GetTradeName().Add(getValue(ch));
                    }
                    else if (ch.Name.Equals("trademark"))

                    {
                        catEntry.GetNounEntry().SetTradeMark(true);
                    }
                    else if (ch.Name.Equals("proper"))

                    {
                        catEntry.GetNounEntry().SetProper(true);
                    }
                }
            }

            return catEntry;
        }

        private static CatEntry getPronValues(Node node)
        {
            CatEntry catEntry = new CatEntry("pron");

            for (Node ch = node.FirstChild; ch != null; ch = ch.NextSibling)

            {
                if (ch.NodeType == XmlNodeType.Element)

                {
                    if (ch.Name.Equals("variants"))

                    {
                        catEntry.GetPronEntry().GetVariants().Add(getValue(ch));
                    }
                    else if (ch.Name.Equals("type"))

                    {
                        catEntry.GetPronEntry().GetType().Add(getValue(ch));
                    }
                    else if (ch.Name.Equals("gender"))

                    {
                        catEntry.GetPronEntry().SetGender(getAttribute(ch, "type"));
                    }
                    else if (ch.Name.Equals("interrogative"))

                    {
                        catEntry.GetPronEntry().SetInterrogative(true);
                    }
                }
            }

            return catEntry;
        }

        private static CatEntry getDetValues(Node node)
        {
            CatEntry catEntry = new CatEntry("det");

            for (Node ch = node.FirstChild; ch != null; ch = ch.NextSibling)

            {
                if (ch.NodeType == XmlNodeType.Element)

                {
                    if (ch.Name.Equals("variants"))

                    {
                        catEntry.GetDetEntry().SetVariants(getValue(ch));
                    }
                    else if (ch.Name.Equals("interrogative"))

                    {
                        catEntry.GetDetEntry().SetInterrogative(true);
                    }
                    else if (ch.Name.Equals("demonstrative"))

                    {
                        catEntry.GetDetEntry().SetDemonstrative(true);
                    }
                }
            }

            return catEntry;
        }

        private static CatEntry getAdvValues(Node node)
        {
            CatEntry catEntry = new CatEntry("adv");

            for (Node ch = node.FirstChild; ch != null; ch = ch.NextSibling)

            {
                if (ch.NodeType == XmlNodeType.Element)

                {
                    if (ch.Name.Equals("variants"))

                    {
                        catEntry.GetAdvEntry().GetVariants().Add(getValue(ch));
                    }
                    else if (ch.Name.Equals("modification"))

                    {
                        catEntry.GetAdvEntry().GetModification().Add(getValue(ch));
                    }
                    else if (ch.Name.Equals("negative"))

                    {
                        catEntry.GetAdvEntry().SetNegative(getAttribute(ch, "type"));
                    }
                    else if (ch.Name.Equals("interrogative"))

                    {
                        catEntry.GetAdvEntry().SetInterrogative(true);
                    }
                }
            }

            return catEntry;
        }

        private static CatEntry getAdjValues(Node node)
        {
            CatEntry catEntry = new CatEntry("adj");

            for (Node ch = node.FirstChild; ch != null; ch = ch.NextSibling)

            {
                if (ch.NodeType == XmlNodeType.Element)

                {
                    if (ch.Name.Equals("variants"))

                    {
                        catEntry.GetAdjEntry().GetVariants().Add(getValue(ch));
                    }
                    else if (ch.Name.Equals("position"))

                    {
                        catEntry.GetAdjEntry().GetPosition().Add(getAttribute(ch, "type"));
                    }
                    else if (ch.Name.Equals("compl"))

                    {
                        catEntry.GetAdjEntry().GetCompl().Add(getValue(ch));
                    }
                    else if (ch.Name.Equals("nominalization"))

                    {
                        catEntry.GetAdjEntry().GetNominalization().Add(getValue(ch));
                    }
                    else if (ch.Name.Equals("stative"))

                    {
                        catEntry.GetAdjEntry().SetStative(true);
                    }
                }
            }

            return catEntry;
        }

        private static string getValue(Node node)
        {
            string value = null;

            for (Node ch = node.FirstChild; ch != null; ch = ch.NextSibling)

            {
                if (ch.NodeType == XmlNodeType.Element || ch.NodeType == XmlNodeType.EntityReference)

                {
                    getValue(ch);
                }
                else if (ch.NodeType == XmlNodeType.Text)

                {
                    value = ch.Value;
                }
            }

            return value;
        }

        private static string getAttribute(Node node, string name)
        {
            string attribute = "";
            NamedNodeMap nnm = node.Attributes;
            if (nnm.Count == 1)

            {
                Node item = nnm.Item(0);
                if (item.Name.Equals(name))

                {
                    attribute = item.Value;
                }
            }

            return attribute;
        }

        private static List<string> getAttributes(Node node)
        {
            List<string> positions = new List<string>();
            NamedNodeMap nnm = node.Attributes;
            for (int i = 0; i < nnm.Count; i++)

            {
                Node item = nnm.Item(i);
                if (item.Name.Equals("type"))

                {
                    positions.Add(item.Value);
                }
            }

            return positions;
        }

        private static void CheckLine(CheckSt st, CheckSt catSt, LineObject lineObject, List<LexRecord> lexRecords)

        {
            bool flag = false;
            bool printFlag = true;

            flag = CheckGrammer.Check(lineObject, printFlag, st, catSt, false);

            if ((st.GetCurState() == 10) && (flag == true))

            {
                lexRecords.Add(CheckGrammer.GetLexRecord());
            }
        }

        private static LexRecord CheckLine(CheckSt st, CheckSt catSt, LineObject lineObject)

        {
            bool flag = false;
            bool printFlag = true;

            flag = CheckGrammer.Check(lineObject, printFlag, st, catSt, false);
            LexRecord lexRecord = new LexRecord();

            if ((st.GetCurState() == 10) && (flag == true))

            {
                lexRecord = CheckGrammer.GetLexRecord();
            }

            return lexRecord;
        }

        private static bool CheckLine2(CheckSt st, CheckSt catSt, LineObject lineObject)

        {
            bool flag = false;
            bool printFlag = true;

            flag = CheckGrammer.Check(lineObject, printFlag, st, catSt, false);
            if (!flag)

            {
                return flag;
            }

            if ((st.GetCurState() == 10) && (flag == true))

            {
                lexRecNum_ += 1;
            }

            return flag;
        }

        private static int CheckLine(CheckSt st, CheckSt catSt, LineObject lineObject, List<LexRecord> lexRecords,
            int recordNum)

        {
            bool flag = false;
            bool printFlag = true;

            flag = CheckGrammer.Check(lineObject, printFlag, st, catSt, false);

            if ((st.GetCurState() == 10) && (flag == true))

            {
                lexRecords.Add(CheckGrammer.GetLexRecord());
                recordNum++;
            }

            return recordNum;
        }

        private static int lexRecNum_ = 0;
    }


}