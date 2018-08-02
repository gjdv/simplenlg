using System;

namespace SimpleNLG.Main.lexicon.util.lexCheck.Lib
{
    public class GlobalVars

    {
        public const string VERSION = "2016";


        public static void SetTextIndent(string textIndent)

        {
            textIndent_ = textIndent;
        }


        public static void SetXmlIndent(string xmlIndent)

        {
            xmlIndent_ = xmlIndent;
        }


        public static void SetXmlHeader(string xmlHeader)

        {
            xmlHeader_ = xmlHeader;
        }


        public static void SetPrepositionFile(string prepositionFile)

        {
            prepositionFile_ = prepositionFile;
        }


        public static void SetIrregExpFile(string irregExpFile)

        {
            irregExpFile_ = irregExpFile;
        }


        public static string GetTextIndent()

        {
            return textIndent_;
        }


        public static string GetXmlIndent()

        {
            return xmlIndent_;
        }


        public static string GetXmlHeader()

        {
            return xmlHeader_;
        }


        public static string GetPrepositionFile()

        {
            return prepositionFile_;
        }


        public static string GetIrregExpFile()

        {
            return irregExpFile_;
        }


        public static readonly string LS_STR = Environment.NewLine;
        private static string textIndent_ = "\t";
        private static string xmlIndent_ = "\t";
        private static string xmlHeader_ = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";

        private static string prepositionFile_ = "";
        private static string irregExpFile_ = "";
    }
}