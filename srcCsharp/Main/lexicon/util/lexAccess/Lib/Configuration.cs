using System.Collections.Generic;
using System.Text;

namespace SimpleNLG.Main.lexicon.util.lexAccess.Lib
{
    public class Configuration

    {
        public const string LA_DIR = "LA_DIR";
        public const string AUTO_MODE = "AUTO_MODE";
        public const string FIRST_VERSION = "FIRST_VERSION";
        public const string LATEST_VERSION = "LATEST_VERSION";
        public const string DB_DIR = "DB_DIR";
        public const string DB_TYPE = "DB_TYPE";
        public const string DB_DRIVER = "DB_DRIVER";
        public const string JDBC_URL = "JDBC_URL";
        public const string DB_HOST = "DB_HOST";
        public const string DB_PORT_NUM = "DB_PORT_NUM";
        public const string DB_NAME = "DB_NAME";
        public const string DB_USERNAME = "DB_USERNAME";
        public const string DB_PASSWORD = "DB_PASSWORD";
        public const string NO_OUTPUT_MSG = "NO_OUTPUT_MSG";
        public const string TEXT_INDENT = "TEXT_INDENT";
        public const string XML_INDENT = "XML_INDENT";
        public const string XML_HEADER = "XML_HEADER";

        public Configuration(string fName, bool useClassPath)

        {
            SetConfiguration(fName, useClassPath);
        }


        public virtual string GetConfiguration(string key)

        {
            string @out = (string) config_[key];
            return @out;
        }


        public virtual void OverwriteProperties(Dictionary<string, string> properties)

        {
            for (IEnumerator<string> e = properties.Keys.GetEnumerator(); e.MoveNext();)

            {
                string key = (string) e.Current;
                string value = (string) properties[key];
                config_[key] = value;
            }
        }


        public virtual string GetInformation()

        {
            StringBuilder buffer = new StringBuilder();

            buffer.Append("LA_DIR: [" + GetConfiguration("LA_DIR") + "]");
            buffer.Append(GlobalVars.LS_STR);
            buffer.Append("DB_DIR: [" + GetConfiguration("DB_DIR") + "]");
            buffer.Append(GlobalVars.LS_STR);
            buffer.Append("DB_TYPE: [" + GetConfiguration("DB_TYPE") + "]");
            buffer.Append(GlobalVars.LS_STR);
            buffer.Append("DB_NAME: [" + GetConfiguration("DB_NAME") + "]");

            return buffer.ToString();
        }

        private void SetConfiguration(string fName, bool useClassPath)

        {
            //try

            //{
            //    if (useClassPath == true)

            //    {
            //        this.configSrc_ = ((PropertyResourceBundle) java.util.ResourceBundle.getBundle(fName));
            //    }
            //    else

            //    {
            //        System.IO.FileStream file =
            //            new System.IO.FileStream(fName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            //        this.configSrc_ = new PropertyResourceBundle(file);
            //        file.Close();
            //    }
            //}
            //catch (Exception e)

            //{
            //    Console.WriteLine("** Configuration Error: " + e.Message);
            //    Console.WriteLine("** Error: problem of opening/reading config file: '" + fName +
            //                      "'. Use -x option to specify the config file path.");
            //}


            //for (IEnumerator<string> e = this.configSrc_.Keys; e.hasMoreElements();)

            //{
            //    string key = (string) e.nextElement();
            //    string value = this.configSrc_.getString(key);
            //    this.config_[key] = value;
            //}

            //if (GetConfiguration("LA_DIR").Equals("AUTO_MODE") == true)

            //{
            //    File file = new File(System.getProperty("user.dir"));

            //    string curDir = file.AbsolutePath + System.getProperty("file.separator");
            //    this.config_["LA_DIR"] = curDir;
            //}
        }


        //private PropertyResourceBundle configSrc_ = null;
        private Dictionary<string, string> config_ = new Dictionary<string, string>();
    }


}