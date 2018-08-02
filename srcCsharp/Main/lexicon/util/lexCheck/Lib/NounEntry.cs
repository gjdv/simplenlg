using System.Collections.Generic;

namespace SimpleNLG.Main.lexicon.util.lexCheck.Lib
{
    public class NounEntry
    {
        public NounEntry()
        {
            variants_ = new List<string>();
            compl_ = new List<string>();
            nominalization_ = new List<string>();
            tradeName_ = new List<string>();
            tradeMark_ = false;
            proper_ = false;
        }

        public virtual List<string> GetVariants()
        {
            return variants_;
        }

        public virtual List<string> GetCompl()
        {
            return compl_;
        }

        public virtual List<string> GetNominalization()
        {
            return nominalization_;
        }

        public virtual List<string> GetTradeName()
        {
            return tradeName_;
        }

        public virtual bool IsTradeMark()
        {
            return tradeMark_;
        }

        public virtual bool IsProper()
        {
            return proper_;
        }

        public virtual void AddVariant(string variant)
        {
            variants_.Add(variant);
        }

        public virtual void SetVariants(List<string> variants)
        {
            variants_ = variants;
        }

        public virtual void AddCompl(string compl)
        {
            compl_.Add(compl);
        }

        public virtual void SetCompl(List<string> compl)
        {
            compl_ = compl;
        }

        public virtual void AddNominalization(string nominalization)
        {
            nominalization_.Add(nominalization);
        }

        public virtual void SetNominalization(List<string> nominalization)
        {
            nominalization_ = nominalization;
        }

        public virtual void AddTradeName(string tradeName)
        {
            tradeName_.Add(tradeName);
        }

        public virtual void SetTradeName(List<string> tradeName)
        {
            tradeName_ = tradeName;
        }

        public virtual void SetTradeMark(bool tradeMark)
        {
            tradeMark_ = tradeMark;
        }

        public virtual void SetProper(bool proper)
        {
            proper_ = proper;
        }

        public virtual string GetText()
        {
            string text = "";
            text = TextLib.AddToText(text, "variants=", variants_, 1);
            text = TextLib.AddToText(text, "compl=", compl_, 1);
            text = TextLib.AddToText(text, "nominalization_of=", nominalization_, 1);
            text = TextLib.AddToText(text, "proper", proper_, 1);
            text = TextLib.AddToText(text, "trademark=", tradeName_, 1);
            text = TextLib.AddToText(text, "trademark", tradeMark_, 1);
            return text;
        }

        public virtual string GetXml()
        {
            bool convertFlag = true;
            string xml = "";
            xml = XmlLib.AddToXml(xml, "<nounEntry>", 2);
            xml = XmlLib.AddToXml(xml, "<variants>", "</variants>", variants_, 3, convertFlag);
            xml = XmlLib.AddToXml(xml, "<compl>", "</compl>", compl_, 3, convertFlag);
            xml = XmlLib.AddToXml(xml, "<nominalization>", "</nominalization>", nominalization_, 3, convertFlag);
            xml = XmlLib.AddToXml(xml, "<tradeName>", "</tradeName>", tradeName_, 3, convertFlag);
            xml = XmlLib.AddToXml(xml, "<trademark/>", tradeMark_, 3);
            xml = XmlLib.AddToXml(xml, "<proper/>", proper_, 3);
            xml = XmlLib.AddToXml(xml, "</nounEntry>", 2);
            return xml;
        }

        private List<string> variants_ = new List<string>();
        private List<string> compl_ = new List<string>();
        private List<string> nominalization_ = new List<string>();
        private List<string> tradeName_ = new List<string>();
        private bool tradeMark_ = false;
        private bool proper_ = false;
    }
}