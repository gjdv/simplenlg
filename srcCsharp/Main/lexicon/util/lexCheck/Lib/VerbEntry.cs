using System.Collections.Generic;

namespace SimpleNLG.Main.lexicon.util.lexCheck.Lib
{
    public class VerbEntry
    {
        public VerbEntry()
        {
            variants_ = new List<string>();
            intran_ = new List<string>();
            tran_ = new List<string>();
            ditran_ = new List<string>();
            link_ = new List<string>();
            cplxtran_ = new List<string>();
            nominalization_ = new List<string>();
        }

        public virtual List<string> GetVariants()
        {
            return variants_;
        }

        public virtual List<string> GetIntran()
        {
            return intran_;
        }

        public virtual List<string> GetTran()
        {
            return tran_;
        }

        public virtual List<string> GetDitran()
        {
            return ditran_;
        }

        public virtual List<string> GetLink()
        {
            return link_;
        }

        public virtual List<string> GetCplxtran()
        {
            return cplxtran_;
        }

        public virtual List<string> GetNominalization()
        {
            return nominalization_;
        }

        public virtual void AddVariant(string variant)
        {
            variants_.Add(variant);
        }

        public virtual void SetVariants(List<string> variants)
        {
            variants_ = variants;
        }

        public virtual void AddIntran(string intran)
        {
            intran_.Add(intran);
        }

        public virtual void AddTran(string tran)
        {
            tran_.Add(tran);
        }

        public virtual void AddDitran(string ditran)
        {
            ditran_.Add(ditran);
        }

        public virtual void AddLink(string link)
        {
            link_.Add(link);
        }

        public virtual void AddCplxtran(string cplxtran)
        {
            cplxtran_.Add(cplxtran);
        }

        public virtual void SetIntran(List<string> intran)
        {
            intran_ = intran;
        }

        public virtual void SetTran(List<string> tran)
        {
            tran_ = tran;
        }

        public virtual void SetDitran(List<string> ditran)
        {
            ditran_ = ditran;
        }

        public virtual void SetLink(List<string> link)
        {
            link_ = link;
        }

        public virtual void SetCplxtran(List<string> cplxtran)
        {
            cplxtran_ = cplxtran;
        }

        public virtual void AddNominalization(string nominalization)
        {
            nominalization_.Add(nominalization);
        }

        public virtual void SetNominalization(List<string> nominalization)
        {
            nominalization_ = nominalization;
        }

        public virtual string GetText()
        {
            string text = "";
            text = TextLib.AddToText(text, "variants=", variants_, 1);
            text = TextLib.AddToText(text, "", intran_, 1);
            text = TextLib.AddToText(text, "tran=", tran_, 1);
            text = TextLib.AddToText(text, "ditran=", ditran_, 1);
            text = TextLib.AddToText(text, "link=", link_, 1);
            text = TextLib.AddToText(text, "cplxtran=", cplxtran_, 1);
            text = TextLib.AddToText(text, "nominalization=", nominalization_, 1);
            return text;
        }

        public virtual string GetXml()
        {
            bool convertFlag = true;
            string xml = "";
            xml = XmlLib.AddToXml(xml, "<verbEntry>", 2);
            xml = XmlLib.AddToXml(xml, "<variants>", "</variants>", variants_, 3, convertFlag);
            xml = XmlLib.AddToXml(xml, "<intran>", "</intran>", intran_, 3, convertFlag);
            xml = XmlLib.AddToXml(xml, "<tran>", "</tran>", tran_, 3, convertFlag);
            xml = XmlLib.AddToXml(xml, "<ditran>", "</ditran>", ditran_, 3, convertFlag);
            xml = XmlLib.AddToXml(xml, "<link>", "</link>", link_, 3, convertFlag);
            xml = XmlLib.AddToXml(xml, "<cplxtran>", "</cplxtran>", cplxtran_, 3, convertFlag);
            xml = XmlLib.AddToXml(xml, "<nominalization>", "</nominalization>", nominalization_, 3, convertFlag);
            xml = XmlLib.AddToXml(xml, "</verbEntry>", 2);
            return xml;
        }

        private List<string> variants_ = new List<string>();
        private List<string> intran_ = new List<string>();
        private List<string> tran_ = new List<string>();
        private List<string> ditran_ = new List<string>();
        private List<string> link_ = new List<string>();
        private List<string> cplxtran_ = new List<string>();
        private List<string> nominalization_ = new List<string>();
    }
}