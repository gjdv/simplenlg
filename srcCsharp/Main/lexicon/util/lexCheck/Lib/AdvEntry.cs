using System.Collections.Generic;

namespace SimpleNLG.Main.lexicon.util.lexCheck.Lib
{
    public class AdvEntry
    {
        public AdvEntry()
        {
            variants_ = new List<string>();
            interrogative_ = false;
            modification_ = new List<string>();
            negative_ = null;
        }

        public virtual List<string> GetVariants()
        {
            return variants_;
        }

        public virtual bool IsInterrogative()
        {
            return interrogative_;
        }

        public virtual List<string> GetModification()
        {
            return modification_;
        }

        public virtual string GetNegative()
        {
            return negative_;
        }

        public virtual void AddVariant(string variant)
        {
            variants_.Add(variant);
        }

        public virtual void SetVariants(List<string> variants)
        {
            variants_ = variants;
        }

        public virtual void SetInterrogative(bool interrogative)
        {
            interrogative_ = interrogative;
        }

        public virtual void AddModification(string modification)
        {
            modification_.Add(modification);
        }

        public virtual void SetModification(List<string> modification)
        {
            modification_ = modification;
        }

        public virtual void SetNegative(string negative)
        {
            negative_ = negative;
        }

        public virtual string GetText()
        {
            string text = "";
            text = TextLib.AddToText(text, "variants=", variants_, 1);
            text = TextLib.AddToText(text, "interrogative", interrogative_, 1);
            text = TextLib.AddToText(text, "modification_type=", modification_, 1);
            text = TextLib.AddToText(text, "", negative_, 1);
            return text;
        }

        public virtual string GetXml()
        {
            bool convertFlag = true;
            string xml = "";
            xml = XmlLib.AddToXml(xml, "<advEntry>", 2);
            xml = XmlLib.AddToXml(xml, "<variants>", "</variants>", variants_, 3, convertFlag);
            xml = XmlLib.AddToXml(xml, "<modification>", "</modification>", modification_, 3, convertFlag);
            if (!ReferenceEquals(negative_, null))
            {
                string negativeTag = "<negative type=\"" + negative_ + "\"/>";
                xml = XmlLib.AddToXml(xml, negativeTag, 3);
            }

            xml = XmlLib.AddToXml(xml, "<interrogative/>", interrogative_, 3);
            xml = XmlLib.AddToXml(xml, "</advEntry>", 2);
            return xml;
        }

        private List<string> variants_ = new List<string>();
        private bool interrogative_ = false;
        private List<string> modification_ = new List<string>();
        private string negative_ = null;
    }
}