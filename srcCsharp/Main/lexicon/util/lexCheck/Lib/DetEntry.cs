 
namespace SimpleNLG.Main.lexicon.util.lexCheck.Lib
{
    public class DetEntry
    {
        public DetEntry()
        {
            variants_ = null;
            interrogative_ = false;
            demonstrative_ = false;
        }

        public virtual string GetVariants()
        {
            return variants_;
        }

        public virtual bool IsInterrogative()
        {
            return interrogative_;
        }

        public virtual bool IsDemonstrative()
        {
            return demonstrative_;
        }

        public virtual void SetVariants(string variant)
        {
            variants_ = variant;
        }

        public virtual void SetInterrogative(bool interrogative)
        {
            interrogative_ = interrogative;
        }

        public virtual void SetDemonstrative(bool demonstrative)
        {
            demonstrative_ = demonstrative;
        }

        public virtual string GetText()
        {
            string text = "";
            text = TextLib.AddToText(text, "variants=", variants_, 1);
            text = TextLib.AddToText(text, "interrogative", interrogative_, 1);
            text = TextLib.AddToText(text, "demonstrative", demonstrative_, 1);
            return text;
        }

        public virtual string GetXml()
        {
            bool convertFlag = true;
            string xml = "";
            xml = XmlLib.AddToXml(xml, "<detEntry>", 2);
            xml = XmlLib.AddToXml(xml, "<variants>", "</variants>", variants_, 3, convertFlag);
            xml = XmlLib.AddToXml(xml, "<interrogative/>", interrogative_, 3);
            xml = XmlLib.AddToXml(xml, "<demonstrative/>", demonstrative_, 3);
            xml = XmlLib.AddToXml(xml, "</detEntry>", 2);
            return xml;
        }

        private string variants_ = null;
        private bool interrogative_ = false;
        private bool demonstrative_ = false;
    }
}