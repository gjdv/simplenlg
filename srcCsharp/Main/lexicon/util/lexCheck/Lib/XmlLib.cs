using System.Collections.Generic;

namespace SimpleNLG.Main.lexicon.util.lexCheck.Lib
{
    public class XmlLib

    {
        public static string AddToXml(string xml, string tag, int numIndent)

        {
            xml = xml + GetIndent(numIndent) + tag + LS;
            return xml;
        }


        public static string AddToXml(string xml, string tag, bool flag, int numIndent)

        {
            if (flag == true)

            {
                xml = xml + GetIndent(numIndent) + tag + LS;
            }

            return xml;
        }


        public static string AddToXml(string xml, string startTag, string endTag, string value, int numIndent,
            bool convertFlag)

        {
            if (!ReferenceEquals(value, null))

            {
                string tempValue = value.Trim();
                if (convertFlag == true)

                {
                    tempValue = Convert.ToNumericEntity(value);
                }

                xml = xml + GetIndent(numIndent) + startTag + tempValue + endTag + LS;
            }

            return xml;
        }


        public static string AddToXml(string xml, string startTag, string endTag, List<string> values, int numIndent,
            bool convertFlag)

        {
            if (values == null)

            {
                return xml;
            }

            for (int i = 0; i < values.Count; i++)

            {
                string value = ((string) values[i]).Trim();
                if (convertFlag == true)

                {
                    value = Convert.ToNumericEntity(value);
                }

                xml = xml + GetIndent(numIndent) + startTag + value + endTag + LS;
            }

            return xml;
        }

        private static string GetIndent(int numIndent)
        {
            string indentStr = "";
            for (int i = 0; i < numIndent; i++)

            {
                indentStr = indentStr + GlobalVars.GetXmlIndent();
            }

            return indentStr;
        }

        private static readonly string LS = GlobalVars.LS_STR;
    }
}