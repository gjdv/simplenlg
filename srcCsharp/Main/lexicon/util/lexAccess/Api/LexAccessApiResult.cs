using System;
using System.Collections.Generic;
using System.Text;
using SimpleNLG.Main.lexicon.util.lexCheck.Api;
using SimpleNLG.Main.lexicon.util.lexCheck.Lib;
using GlobalVars = SimpleNLG.Main.lexicon.util.lexAccess.Lib.GlobalVars;

namespace SimpleNLG.Main.lexicon.util.lexAccess.Api
{

    public class LexAccessApiResult

    {
        public virtual void SetJavaObjs(List<LexRecord> lexReocrdObjs)

        {
            lexRecordObjs_ = lexReocrdObjs;

            text_ = "";

            if (lexRecordObjs_ != null)

            {
                for (int i = 0; i < lexRecordObjs_.Count; i++)

                {
                    LexRecord temp = (LexRecord) lexRecordObjs_[i];
                    text_ += temp.GetText();
                }
            }
        }


        public virtual void SetText(string text)

        {
            text_ = text;

            try

            {
                lexRecordObjs_ = ToJavaObjApi.ToJavaObjsFromText(text);
            }
            catch (Exception e)

            {
                Console.WriteLine("** Error: " + e.Message);
            }
        }


        public virtual string GetText()

        {
            return text_;
        }


        public virtual List<LexRecord> GetJavaObjs()

        {
            return lexRecordObjs_;
        }


        public virtual string GetXml()

        {
            StringBuilder xmlOut = new StringBuilder();

            if (lexRecordObjs_ != null)

            {
                xmlOut.Append(LexRecord.GetXmlHeader());
                xmlOut.Append(GlobalVars.LS_STR);
                xmlOut.Append(LexRecord.GetXmlRootBeginTag());
                xmlOut.Append(GlobalVars.LS_STR);
                for (int i = 0; i < lexRecordObjs_.Count; i++)

                {
                    LexRecord temp = (LexRecord) lexRecordObjs_[i];
                    xmlOut.Append(temp.GetXml(1));
                }

                xmlOut.Append(LexRecord.GetXmlRootEndTag());
                xmlOut.Append(GlobalVars.LS_STR);
            }

            return xmlOut.ToString();
        }


        public virtual List<string> GetBases()

        {
            List<string> bases = new List<string>();
            if (lexRecordObjs_ != null)

            {
                for (int i = 0; i < lexRecordObjs_.Count; i++)

                {
                    LexRecord temp = (LexRecord) lexRecordObjs_[i];
                    bases.Add(temp.GetBase());
                }
            }

            return bases;
        }


        public virtual List<string> GetBases(string separator)

        {
            List<string> bases = new List<string>();
            if (lexRecordObjs_ != null)

            {
                for (int i = 0; i < lexRecordObjs_.Count; i++)

                {
                    LexRecord temp = (LexRecord) lexRecordObjs_[i];
                    bases.Add(temp.GetBase() + separator + temp.GetEui() + separator + temp.GetCategory());
                }
            }

            return bases;
        }


        public virtual List<string> GetSpellingVars()

        {
            List<string> spellingVars = new List<string>();
            if (lexRecordObjs_ != null)

            {
                for (int i = 0; i < lexRecordObjs_.Count; i++)

                {
                    LexRecord temp = (LexRecord) lexRecordObjs_[i];
                    spellingVars.Add(temp.GetBase());
                    spellingVars.AddRange(temp.GetSpellingVars());
                }
            }

            return spellingVars;
        }


        public virtual List<string> GetSpellingVars(string separator)

        {
            List<string> spellingVars = new List<string>();
            if (lexRecordObjs_ != null)

            {
                for (int i = 0; i < lexRecordObjs_.Count; i++)

                {
                    LexRecord temp = (LexRecord) lexRecordObjs_[i];
                    string eui = temp.GetEui();
                    string category = temp.GetCategory();
                    spellingVars.Add(temp.GetBase() + separator + eui + separator + category);

                    List<string> tempSpellVars = temp.GetSpellingVars();
                    for (int j = 0; j < tempSpellVars.Count; j++)

                    {
                        string tempSpellVar = (string) tempSpellVars[j];
                        spellingVars.Add(tempSpellVar + separator + eui + separator + category);
                    }
                }
            }

            return spellingVars;
        }


        public virtual List<string> GetInflVars()

        {
            List<string> inflVars = new List<string>();
            if (lexRecordObjs_ != null)

            {
                for (int i = 0; i < lexRecordObjs_.Count; i++)

                {
                    LexRecord temp = (LexRecord) lexRecordObjs_[i];

                    List<InflVar> inflValues = temp.GetInflVarsAndAgreements().GetInflValues();
                    for (int j = 0; j < inflValues.Count; j++)

                    {
                        InflVar inflVar = (InflVar) inflValues[j];
                        inflVars.Add(inflVar.GetVar());
                    }
                }
            }

            return inflVars;
        }


        public virtual List<string> GetInflVars(string separator)

        {
            List<string> inflVars = new List<string>();
            if (lexRecordObjs_ != null)

            {
                for (int i = 0; i < lexRecordObjs_.Count; i++)

                {
                    LexRecord temp = (LexRecord) lexRecordObjs_[i];

                    List<InflVar> inflValues = temp.GetInflVarsAndAgreements().GetInflValues();
                    for (int j = 0; j < inflValues.Count; j++)

                    {
                        InflVar inflVar = (InflVar) inflValues[j];


                        string inflVarDetail = inflVar.GetVar() + separator + inflVar.GetCat() + separator +
                                               inflVar.GetInflection() + separator + inflVar.GetEui() + separator +
                                               inflVar.GetUnInfl() + separator + inflVar.GetCit() + separator +
                                               inflVar.GetType();
                        inflVars.Add(inflVarDetail);
                    }
                }
            }

            return inflVars;
        }


        public virtual int GetTotalRecordNumber()

        {
            return lexRecordObjs_.Count;
        }

        private string text_ = "";
        private List<LexRecord> lexRecordObjs_ = new List<LexRecord>();
    }


}