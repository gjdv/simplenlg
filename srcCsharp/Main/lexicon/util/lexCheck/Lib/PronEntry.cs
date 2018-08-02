using System.Collections.Generic;

namespace SimpleNLG.Main.lexicon.util.lexCheck.Lib
{
    public class PronEntry
    {
        public PronEntry()
        {
            variants_ = new List<string>();
            gender_ = null;
            interrogative_ = false;
            type_ = new List<string>();
        }

        public virtual List<string> GetVariants()
        {
            return variants_;
        }

        public virtual string GetGender()
        {
            return gender_;
        }

        public virtual bool IsInterrogative()
        {
            return interrogative_;
        }

        public virtual List<string> GetType()
        {
            return type_;
        }

        public virtual void AddVariant(string variant)
        {
            variants_.Add(variant);
        }

        public virtual void SetVariants(List<string> variants)
        {
            variants_ = variants;
        }

        public virtual void SetGender(string gender)
        {
            gender_ = gender;
        }

        public virtual void SetInterrogative(bool interrogative)
        {
            interrogative_ = interrogative;
        }

        public virtual void AddType(string type)
        {
            type_.Add(type);
        }

        public virtual void SetType(List<string> type)
        {
            type_ = type;
        }

        public virtual string GetText()
        {
            string text = "";
            text = TextLib.AddToText(text, "variants=", variants_, 1);
            text = TextLib.AddToText(text, "gender=", gender_, 1);
            text = TextLib.AddToText(text, "interrogative", interrogative_, 1);
            text = TextLib.AddToText(text, "type=", type_, 1);
            return text;
        }

        public virtual string GetXml()
        {
            bool convertFlag = true;
            string xml = "";
            xml = XmlLib.AddToXml(xml, "<pronEntry>", 2);
            xml = XmlLib.AddToXml(xml, "<variants>", "</variants>", variants_, 3, convertFlag);
            xml = XmlLib.AddToXml(xml, "<type>", "</type>", type_, 3, convertFlag);
            if (!ReferenceEquals(gender_, null))
            {
                string genderTag = "<gender type=\"" + gender_ + "\"/>";
                xml = XmlLib.AddToXml(xml, genderTag, 3);
            }

            xml = XmlLib.AddToXml(xml, "<interrogative/>", interrogative_, 3);
            xml = XmlLib.AddToXml(xml, "</pronEntry>", 2);
            return xml;
        }

        private List<string> variants_ = new List<string>();
        private string gender_ = null;
        private bool interrogative_ = false;
        private List<string> type_ = new List<string>();
    }
}