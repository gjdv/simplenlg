using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleNLG.Main.lexicon.util.lexCheck.Lib
{
    public class Configuration
    {
        public const string LC_DIR = "LC_DIR";
        public const string TEXT_INDENT = "TEXT_INDENT";
        public const string XML_INDENT = "XML_INDENT";
        public const string XML_HEADER = "XML_HEADER";
        public const string PREPOSITION_FILE = "PREPOSITION_FILE";
        public const string IRREG_EXP_FILE = "IRREG_EXP_FILE";

        public Configuration(string fName, bool useClassPath)
        {
            //SetConfiguration(fName, useClassPath);
        }

        public virtual string GetConfiguration(string key)
        {
            return null;
            //string @out = this.config_.getString(key);
            //return @out;
        }

        public static string OverWriteProperty(string key, Configuration conf, Dictionary<string, string> properties)
        {
            string property = conf.GetConfiguration(key);
            string value = (string) properties[key];
            if (!ReferenceEquals(value, null))
            {
                property = value;
            }

            return property;
        }

        public virtual string GetInformation()
        {
            StringBuilder buffer = new StringBuilder();
            buffer.Append("LC_DIR: [" + GetConfiguration("LC_DIR") + "]");
            buffer.Append(LS_STR);
            return buffer.ToString();
        }

        //private void SetConfiguration(string fName, bool useClassPath)
        //{
        //    try
        //    {
        //        if (useClassPath == true)
        //        {
        //            this.config_ = ((PropertyResourceBundle) ResourceBundle.getBundle(fName));
        //        }
        //        else
        //        {
        //            System.IO.FileStream file =
        //                new System.IO.FileStream(fName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
        //            this.config_ = new PropertyResourceBundle(file);
        //            file.Close();
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Console.Error.WriteLine("** Configuration Error: " + e.Message);
        //        Console.Error.WriteLine("** Error: problem of opening/reading config file: '" + fName +
        //                                "'. Use -x option to specify the config file path.");
        //    }
        //}

        //private PropertyResourceBundle config_ = null;
        private static readonly string LS_STR = Environment.NewLine;
    }
}