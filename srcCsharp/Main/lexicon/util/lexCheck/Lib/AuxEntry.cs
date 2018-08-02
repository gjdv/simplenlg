using System.Collections.Generic;

namespace SimpleNLG.Main.lexicon.util.lexCheck.Lib
{
    public class AuxEntry
    {
        public AuxEntry()
        {
            variant_ = new List<string>();
        }

        public virtual List<string> GetVariant()
        {
            return variant_;
        }

        public virtual void AddVariant(string variant)
        {
            variant_.Add(variant);
        }

        public virtual void SetVariant(List<string> variant)
        {
            variant_ = variant;
        }

        public virtual string GetText()
        {
            string text = "";
            text = TextLib.AddToText(text, "variant=", variant_, 1);
            return text;
        }

        public virtual string GetXml()
        {
            bool convertFlag = true;
            string xml = "";
            xml = XmlLib.AddToXml(xml, "<auxEntry>", 2);
            xml = XmlLib.AddToXml(xml, "<variant>", "</variant>", variant_, 3, convertFlag);
            xml = XmlLib.AddToXml(xml, "</auxEntry>", 2);
            return xml;
        }

        private List<string> variant_ = new List<string>();
    }
}