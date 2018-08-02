using System.Collections.Generic;

namespace SimpleNLG.Main.lexicon.util.lexCheck.Lib
{
    public class AdjEntry
    {
        public AdjEntry()
        {
            variants_ = new List<string>();
            position_ = new List<string>();
            compl_ = new List<string>();
            nominalization_ = new List<string>();
            stative_ = false;
        }

        public virtual List<string> GetVariants()
        {
            return variants_;
        }

        public virtual List<string> GetPosition()
        {
            return position_;
        }

        public virtual List<string> GetCompl()
        {
            return compl_;
        }

        public virtual List<string> GetNominalization()
        {
            return nominalization_;
        }

        public virtual bool IsStative()
        {
            return stative_;
        }

        public virtual void AddVariant(string variant)
        {
            variants_.Add(variant);
        }

        public virtual void SetVariants(List<string> variants)
        {
            variants_ = variants;
        }

        public virtual void AddPosition(string position)
        {
            position_.Add(position);
        }

        public virtual void SetPosition(List<string> position)
        {
            position_ = position;
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

        public virtual void SetStative(bool stative)
        {
            stative_ = stative;
        }

        public virtual string GetText()
        {
            string text = "";
            text = TextLib.AddToText(text, "variants=", variants_, 1);
            text = TextLib.AddToText(text, "position=", position_, 1);
            text = TextLib.AddToText(text, "compl=", compl_, 1);
            text = TextLib.AddToText(text, "stative", stative_, 1);
            text = TextLib.AddToText(text, "nominalization=", nominalization_, 1);
            return text;
        }

        public virtual string GetXml()
        {
            bool convertFlag = true;
            string xml = "";
            xml = XmlLib.AddToXml(xml, "<adjEntry>", 2);
            xml = XmlLib.AddToXml(xml, "<variants>", "</variants>", variants_, 3, convertFlag);
            if (position_ != null)
            {
                for (int i = 0; i < position_.Count; i++)
                {
                    string positionTag = "<position type=\"" + (string) position_[i] + "\"/>";
                    xml = XmlLib.AddToXml(xml, positionTag, 3);
                }
            }

            xml = XmlLib.AddToXml(xml, "<compl>", "</compl>", compl_, 3, convertFlag);
            xml = XmlLib.AddToXml(xml, "<stative/>", stative_, 3);
            xml = XmlLib.AddToXml(xml, "<nominalization>", "</nominalization>", nominalization_, 3, convertFlag);
            xml = XmlLib.AddToXml(xml, "</adjEntry>", 2);
            return xml;
        }

        private List<string> variants_ = new List<string>();
        private List<string> position_ = new List<string>();
        private List<string> compl_ = new List<string>();
        private bool stative_ = false;
        private List<string> nominalization_ = new List<string>();
    }
}