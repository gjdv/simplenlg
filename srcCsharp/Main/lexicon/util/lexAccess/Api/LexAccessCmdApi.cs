using System;
using System.Collections.Generic;
using System.Text;
using java.sql;
using java.util;
using SimpleNLG.Main.lexicon.util.lexAccess.Db;
using SimpleNLG.Main.lexicon.util.lexAccess.Lib;
using ArrayList = System.Collections.ArrayList;
using Hashtable = System.Collections.Hashtable;

namespace SimpleNLG.Main.lexicon.util.lexAccess.Api
{
    using DbBase = DbBase;
    using Configuration = Configuration;
    using Option = gov.nih.nlm.nls.lvg.CmdLineSyntax.Option;
    using OptionItem = gov.nih.nlm.nls.lvg.CmdLineSyntax.OptionItem;
    using SystemOption = gov.nih.nlm.nls.lvg.CmdLineSyntax.SystemOption;
    using Out = gov.nih.nlm.nls.lvg.Util.Out;


    public class LexAccessCmdApi : SystemOption


    {
        public LexAccessCmdApi()

        {
            Init();
        }


        public LexAccessCmdApi(string optionStr)

        {
            this.option_ = new Option(optionStr);
            Init();
        }


        public LexAccessCmdApi(string optionStr, string configFile)

        {
            this.option_ = new Option(optionStr);
            this.configFile_ = configFile;
            Init();
        }


        public LexAccessCmdApi(Dictionary<string, string> properties)

        {
            this.properties_ = properties;
            Init();
        }


        public LexAccessCmdApi(string optionStr, Dictionary<string, string> properties)

        {
            this.option_ = new Option(optionStr);
            this.properties_ = properties;
            Init();
        }


        public LexAccessCmdApi(string optionStr, string configFile, Dictionary<string, string> properties)

        {
            this.option_ = new Option(optionStr);
            this.configFile_ = configFile;
            this.properties_ = properties;
            Init();
        }


        public virtual void SetPromptStr(string promptStr)

        {
            this.promptStr_ = promptStr;
        }


        public virtual void SetQuitStrList(List<string> quitStrList)

        {
            this.quitStrList_ = new ArrayList(quitStrList);
        }


        public virtual bool IsLegalOption()

        {
            bool isLegalOption = SystemOption.CheckSyntax(this.option_, GetOption(), false, true);
            return isLegalOption;
        }


        public virtual void SetOption(string optionStr)

        {
            this.option_ = new Option(optionStr);


            PreProcess();


            InitDb();
        }


        public virtual Configuration GetConfiguration()

        {
            return this.conf_;
        }


        public virtual Connection GetConnection()

        {
            return this.conn_;
        }


        public virtual void CleanUp()

        {
            try

            {
                Close();
            }
            catch (Exception e)

            {
                Console.Error.WriteLine("** ERR: " + e.Message);
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
            }
        }


        //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public String ProcessToString() throws java.sql.SQLException, java.io.IOException
        public virtual string ProcessToString()


        {
            this.out_.ResetOutString();
            Process(true);
            return this.out_.GetOutString();
        }


        //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public boolean Process() throws java.sql.SQLException, java.io.IOException
        public virtual bool Process()


        {
            return Process(false);
        }

        //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: private boolean Process(boolean toStringFlag) throws java.sql.SQLException, java.io.IOException
        private bool Process(bool toStringFlag)


        {
            if (!this.runFlag_)

            {
                return false;
            }

            if (this.category_ == 0L)

            {
                if (this.promptFlag_ == true)

                {
                    GetPrompt();
                }

                string line = null;
                if (this.inReader_ == null)

                {
                    this.inReader_ = new System.IO.StreamReader(System.in, Encoding.UTF8);
                }

                line = this.inReader_.ReadLine();

                if ((string.ReferenceEquals(line, null)) || (this.quitStrList_.Contains(line)))

                {
                    return false;
                }

                ProcessLine(line, toStringFlag);
                return true;
            }

            string fs = GlobalBehavior.GetInstance().GetFieldSeparator();
            string outStr = this.lexAccessApi_.GetResultStrByCategory(this.category_, this.showQuery_, this.query_,
                this.noOutputFlag_, this.noOutputMsg_, this.showTotalRecNum_, this.formatOpt_, fs);


            this.out_.Print(this.outWriter_, outStr, this.fileOutput_, toStringFlag);
            return false;
        }


        public virtual void PrintLexAccessHelp()

        {
            LexAccessHelp.LexAccessHelp(this.outWriter_, this.fileOutput_, this.out_);
        }


        protected internal virtual void ExecuteCommand(OptionItem optionItem, Option systemOption, Out @out)

        {
            OptionItem nameItem = OptionUtility.GetItemByName(optionItem, systemOption, false);
            if (CheckOption(nameItem, "-b:b") == true)

            {
                this.baseOpt_ = 1;
            }
            else if (CheckOption(nameItem, "-b:c") == true)

            {
                this.baseOpt_ = 2;
            }
            else if (CheckOption(nameItem, "-b:e") == true)

            {
                this.baseOpt_ = 3;
            }
            else if (CheckOption(nameItem, "-b:ex") == true)

            {
                this.baseOpt_ = 4;
            }
            else if (CheckOption(nameItem, "-c:a") == true)

            {
                this.category_ |= Category.ToValue("adj");
            }
            else if (CheckOption(nameItem, "-c:all") == true)

            {
                this.category_ |= 0x7FF;
            }
            else if (CheckOption(nameItem, "-c:b") == true)

            {
                this.category_ |= Category.ToValue("adv");
            }
            else if (CheckOption(nameItem, "-c:c") == true)

            {
                this.category_ |= Category.ToValue("compl");
            }
            else if (CheckOption(nameItem, "-c:d") == true)

            {
                this.category_ |= Category.ToValue("det");
            }
            else if (CheckOption(nameItem, "-c:h") == true)

            {
                LexAccessHelp.CategoryHelp(this.outWriter_, this.fileOutput_, @out);
                this.runFlag_ = false;
            }
            else if (CheckOption(nameItem, "-c:j") == true)

            {
                this.category_ |= Category.ToValue("conj");
            }
            else if (CheckOption(nameItem, "-c:m") == true)

            {
                this.category_ |= Category.ToValue("modal");
            }
            else if (CheckOption(nameItem, "-c:n") == true)

            {
                this.category_ |= Category.ToValue("noun");
            }
            else if (CheckOption(nameItem, "-c:p") == true)

            {
                this.category_ |= Category.ToValue("prep");
            }
            else if (CheckOption(nameItem, "-c:r") == true)

            {
                this.category_ |= Category.ToValue("pron");
            }
            else if (CheckOption(nameItem, "-c:v") == true)

            {
                this.category_ |= Category.ToValue("verb");
            }
            else if (CheckOption(nameItem, "-c:x") == true)

            {
                this.category_ |= Category.ToValue("aux");
            }
            else if (CheckOption(nameItem, "-ci") == true)

            {
                try

                {
                    bool useClassPath = false;
                    string configFile = this.configFile_;
                    if (string.ReferenceEquals(configFile, null))

                    {
                        useClassPath = true;
                        configFile = "data.config.lexAccess";
                    }

                    Configuration conf = new Configuration(configFile, useClassPath);


                    if (this.properties_ != null)

                    {
                        conf.OverwriteProperties(this.properties_);
                    }

                    @out.Println(this.outWriter_, conf.GetInformation(), this.fileOutput_, false);
                }
                catch (IOException)
                {
                }

                this.runFlag_ = false;
            }
            else if (CheckOption(nameItem, "-cn:LONG") == true)

            {
                long cat = long.Parse(nameItem.GetOptionArgument());
                if ((cat <= 0L) || (cat > 2047L))

                {
                    string temp = "*** Error: Illegal value for category (" + cat + ").";

                    try

                    {
                        @out.Println(this.outWriter_, temp, this.fileOutput_, false);
                    }
                    catch (IOException)
                    {
                    }

                    this.runFlag_ = false;
                }
                else

                {
                    this.category_ = long.Parse(nameItem.GetOptionArgument());
                }
            }
            else if (CheckOption(nameItem, "-cnf:LONG") == true)

            {
                this.outCat_ = long.Parse(nameItem.GetOptionArgument());
            }
            else if (CheckOption(nameItem, "-f:b") == true)

            {
                this.formatOpt_ = 3;
            }
            else if (CheckOption(nameItem, "-f:bd") == true)

            {
                this.formatOpt_ = 4;
            }
            else if (CheckOption(nameItem, "-f:h") == true)

            {
                LexAccessHelp.FormatHelp(this.outWriter_, this.fileOutput_, @out);
                this.runFlag_ = false;
            }
            else if (CheckOption(nameItem, "-f:i") == true)

            {
                this.formatOpt_ = 7;
            }
            else if (CheckOption(nameItem, "-f:id") == true)

            {
                this.formatOpt_ = 8;
            }
            else if (CheckOption(nameItem, "-f:s") == true)

            {
                this.formatOpt_ = 5;
            }
            else if (CheckOption(nameItem, "-f:sd") == true)

            {
                this.formatOpt_ = 6;
            }
            else if (CheckOption(nameItem, "-f:t") == true)

            {
                this.formatOpt_ = 0;
            }
            else if (CheckOption(nameItem, "-f:tx") == true)

            {
                this.formatOpt_ = 2;
            }
            else if (CheckOption(nameItem, "-f:x") == true)

            {
                this.formatOpt_ = 1;
            }
            else if (CheckOption(nameItem, "-h") == true)

            {
                LexAccessHelp.LexAccessHelp(this.outWriter_, this.fileOutput_, @out);
                this.runFlag_ = false;
            }
            else if (CheckOption(nameItem, "-hs") == true)

            {
                systemOption.PrintOptionHierachy();
                this.runFlag_ = false;
            }
            else if (CheckOption(nameItem, "-i:STR") == true)

            {
                string inFile = nameItem.GetOptionArgument();
                if (!string.ReferenceEquals(inFile, null))

                {
                    try

                    {
                        this.inReader_ = new System.IO.StreamReader(
                            new System.IO.FileStream(inFile, System.IO.FileMode.Open, System.IO.FileAccess.Read),
                            Encoding.UTF8);
                    }
                    catch (IOException)

                    {
                        this.runFlag_ = false;
                        Console.Error.WriteLine("**Error: problem of opening/reading file " + inFile);
                    }
                }
            }
            else if (CheckOption(nameItem, "-n") == true)

            {
                this.noOutputFlag_ = true;
            }
            else if (CheckOption(nameItem, "-o:STR") == true)

            {
                string outFile = nameItem.GetOptionArgument();
                if (!string.ReferenceEquals(outFile, null))

                {
                    try

                    {
                        this.outWriter_ = new System.IO.StreamWriter(
                            new System.IO.FileStream(outFile, System.IO.FileMode.Create, System.IO.FileAccess.Write),
                            Encoding.UTF8);

                        this.fileOutput_ = true;
                    }
                    catch (IOException)

                    {
                        this.runFlag_ = false;
                        Console.Error.WriteLine("**Error: problem of opening/writing file " + outFile);
                    }
                }
            }
            else if (CheckOption(nameItem, "-p") == true)

            {
                this.promptFlag_ = true;
            }
            else if (CheckOption(nameItem, "-q") == true)

            {
                this.query_ = this.option_.GetOptionStr();
                this.showQuery_ = true;
            }
            else if (CheckOption(nameItem, "-rv:STR") == true)

            {
                this.version_ = nameItem.GetOptionArgument();
                if (!SetVersionInConfig(this.version_))

                {
                    Console.Error.WriteLine("**Err@LexAccessCmdApi(): Illegal version setting (" + this.version_ + ")");


                    this.runFlag_ = false;
                }
            }
            else if (CheckOption(nameItem, "-s:STR") == true)

            {
                string separator = nameItem.GetOptionArgument();

                if (separator.Equals("\\t"))

                {
                    separator = (new char?('\t')).ToString();
                }

                GlobalBehavior.GetInstance().SetFieldSeparator(separator);
            }
            else if (CheckOption(nameItem, "-t") == true)

            {
                this.showTotalRecNum_ = true;
            }
            else if (CheckOption(nameItem, "-v") == true)

            {
                try

                {
                    string releaseStr = "lexAccess.2016";
                    @out.Println(this.outWriter_, releaseStr, this.fileOutput_, false);
                }
                catch (IOException)
                {
                }

                this.runFlag_ = false;
            }
            else if (CheckOption(nameItem, "-x:STR") == true)

            {
                this.configFile_ = nameItem.GetOptionArgument();
            }
        }


        protected internal virtual void DefineFlag()

        {
            string flagStr =
                "-b:b:c:e:ex -c:a:all:b:c:d:h:j:m:n:p:r:v:x -ci -cn:LONG -cnf:LONG -f:b:bd:h:i:id:s:sd:t:tx:x -h -hs -i:STR -n -o:STR -p -q -rv:STR -s:STR -t -v -x:STR";

            this.systemOption_ = new Option(flagStr);

            this.systemOption_.SetFlagFullName("-b:b", "Base_begin_with");
            this.systemOption_.SetFlagFullName("-b:c", "Base_contain");
            this.systemOption_.SetFlagFullName("-b:e", "Base_end_with");
            this.systemOption_.SetFlagFullName("-b:ex", "Base_exact_match");
            this.systemOption_.SetFlagFullName("-c:a", "Category_Adj");
            this.systemOption_.SetFlagFullName("-c:all", "Category_All");
            this.systemOption_.SetFlagFullName("-c:b", "Category_Adv");
            this.systemOption_.SetFlagFullName("-c:c", "Category_Compl");
            this.systemOption_.SetFlagFullName("-c:d", "Category_Det");
            this.systemOption_.SetFlagFullName("-c:h", "Category_Help");
            this.systemOption_.SetFlagFullName("-c:j", "Category_Conj");
            this.systemOption_.SetFlagFullName("-c:m", "Category_Modal");
            this.systemOption_.SetFlagFullName("-c:n", "Category_Noun");
            this.systemOption_.SetFlagFullName("-c:p", "Category_Prep");
            this.systemOption_.SetFlagFullName("-c:r", "Category_Pron");
            this.systemOption_.SetFlagFullName("-c:v", "Category_Verb");
            this.systemOption_.SetFlagFullName("-c:x", "Category_Aux");
            this.systemOption_.SetFlagFullName("-ci", "Show_Config_Info");
            this.systemOption_.SetFlagFullName("-cn:LONG", "Category_in_value");
            this.systemOption_.SetFlagFullName("-cnf:LONG", "Category_filter");
            this.systemOption_.SetFlagFullName("-f:b", "Base_Form");
            this.systemOption_.SetFlagFullName("-f:bd", "Base_Form_Details");
            this.systemOption_.SetFlagFullName("-f:h", "Print_Help");
            this.systemOption_.SetFlagFullName("-f:i", "Inflected_Term");
            this.systemOption_.SetFlagFullName("-f:id", "Inflected_Term_Details");
            this.systemOption_.SetFlagFullName("-f:s", "Base_Spelling_Var");
            this.systemOption_.SetFlagFullName("-f:sd", "Base_Spelling_Var_Details");
            this.systemOption_.SetFlagFullName("-f:t", "Text_Record");
            this.systemOption_.SetFlagFullName("-f:tx", "Text_XML_Record");
            this.systemOption_.SetFlagFullName("-f:x", "Xml_Record");
            this.systemOption_.SetFlagFullName("-h", "Help");
            this.systemOption_.SetFlagFullName("-hs", "Hierarchy_Struture");
            this.systemOption_.SetFlagFullName("-i", "Input_File");
            this.systemOption_.SetFlagFullName("-n", "No_Record_Found");
            this.systemOption_.SetFlagFullName("-o", "Output_File");
            this.systemOption_.SetFlagFullName("-p", "Show_Prompt");
            this.systemOption_.SetFlagFullName("-q", "Print_Query");
            this.systemOption_.SetFlagFullName("-rv:STR", "Run_Specified_version");
            this.systemOption_.SetFlagFullName("-s:STR", "Field_Separator");
            this.systemOption_.SetFlagFullName("-t", "Total_Rec_Num");
            this.systemOption_.SetFlagFullName("-v", "Version");
            this.systemOption_.SetFlagFullName("-x:STR", "Load_Configuration_file");
        }


        //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: protected void GetPrompt() throws java.io.IOException
        protected internal virtual void GetPrompt()


        {
            this.out_.Println(this.outWriter_, this.promptStr_, this.fileOutput_, false);
        }

        private void Init()

        {
            PreProcess();


            InitConfigVars();

            InitDb();

            if (this.lexAccessApi_ == null)

            {
                this.lexAccessApi_ = new LexAccessApi(this.conn_);
            }
        }

        private void InitDb()

        {
            try

            {
                if ((this.conn_ == null) && (this.runFlag_ == true))

                {
                    this.conn_ = DbBase.OpenConnection(this.conf_);
                }
            }
            catch (Exception e)

            {
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
            }
        }

        private void PreProcess()

        {
            try

            {
                this.outWriter_ = new System.IO.StreamWriter(System.out, Encoding.UTF8);
            }
            catch (IOException)

            {
                Console.Error.WriteLine("**Error: problem of opening Std-out.");
            }

            List<string> args = GetOptions(this.option_.GetOptionStr());

            for (int i = 0; i < args.Count; i++)

            {
                string temp = (string) args[i];
                Option io = new Option(temp);

                ExecuteCommands(io, GetOption(), this.out_);
            }
        }

        private static List<string> GetOptions(string inStr)
        {
            List<string> @out = new ArrayList();
            StringTokenizer buf = new StringTokenizer(inStr, " \t");

            while (buf.hasMoreTokens() == true)

            {
                @out.Add(buf.nextToken());
            }

            return @out;
        }

        private void InitConfigVars()

        {
            bool useClassPath = false;
            if (string.ReferenceEquals(this.configFile_, null))

            {
                useClassPath = true;
                this.configFile_ = "data.config.lexAccess";
            }

            this.conf_ = new Configuration(this.configFile_, useClassPath);


            if (this.properties_ != null)

            {
                this.conf_.OverwriteProperties(this.properties_);
            }

            if (string.ReferenceEquals(this.noOutputMsg_, null))

            {
                this.noOutputMsg_ = this.conf_.GetConfiguration("NO_OUTPUT_MSG");
            }

            if (Platform.IsWindow() == true)

            {
                this.promptStr_ = "- Please input a term/eui (type \"Ctl-z\" then \"Enter\" to quit) >";
            }
            else

            {
                this.promptStr_ = "- Please input a term (type \"Ctl-d\" to quit) >";
            }

            string textIndent = this.conf_.GetConfiguration("TEXT_INDENT");

            if (!string.ReferenceEquals(textIndent, null))

            {
                GlobalVars.SetTextIndent(GetStringContent(textIndent));
            }

            string xmlIndent = this.conf_.GetConfiguration("XML_INDENT");

            if (!string.ReferenceEquals(xmlIndent, null))

            {
                GlobalVars.SetXmlIndent(GetStringContent(xmlIndent));
            }

            string xmlHeader = this.conf_.GetConfiguration("XML_HEADER");

            if (!string.ReferenceEquals(xmlHeader, null))

            {
                GlobalVars.SetXmlHeader(GetStringContent(xmlHeader));
            }
        }

        private static string GetStringContent(string inStr)
        {
            int length = inStr.Length;
            if ((!string.ReferenceEquals(inStr, null)) && (inStr.StartsWith("\"", StringComparison.Ordinal) == true) &&
                (inStr.EndsWith("\"", StringComparison.Ordinal) == true))


            {
                return inStr.Substring(1, (length - 1) - 1);
            }

            return inStr;
        }

        //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: private void Close() throws java.io.IOException, java.sql.SQLException
        private void Close()
        {
            if ((this.outWriter_ != null) && (this.fileOutput_ == true))

            {
                this.outWriter_.Close();
            }

            if (this.inReader_ != null)

            {
                this.inReader_.Close();
            }

            if (this.conn_ != null)

            {
                DbBase.CloseConnection(this.conn_);
            }
        }

        //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: private void ProcessLine(String line, boolean toStringFlag) throws java.sql.SQLException, java.io.IOException
        private void ProcessLine(string line, bool toStringFlag)


        {
            string outStr = GetLexRecords(line);

            this.out_.Print(this.outWriter_, outStr, this.fileOutput_, toStringFlag);
        }

        private string GetLexRecords(string inTerm)
        {
            string resultStr = "";
            string fs = GlobalBehavior.GetInstance().GetFieldSeparator();


            if ((string.ReferenceEquals(inTerm, null)) || (inTerm.Length == 0))

            {
                return resultStr;
            }

            try

            {
                if (this.baseOpt_ == 0)

                {
                    resultStr = this.lexAccessApi_.GetResultStrByTerm(inTerm, this.outCat_, this.showQuery_,
                        this.query_, this.noOutputFlag_, this.noOutputMsg_, this.showTotalRecNum_, this.formatOpt_, fs);
                }
                else

                {
                    resultStr = this.lexAccessApi_.GetResultStrByBase(inTerm, this.baseOpt_, this.outCat_,
                        this.showQuery_, this.query_, this.noOutputFlag_, this.noOutputMsg_, this.showTotalRecNum_,
                        this.formatOpt_, fs);
                }
            }
            catch (Exception e)

            {
                Console.Error.WriteLine("** ERR: " + e.Message);
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
            }

            return resultStr;
        }

        private bool SetVersionInConfig(string version)
        {
            bool flag = true;
            if (IsLegalVersion(version) == true)

            {
                this.properties_ = new Hashtable();
                string dbDir = "HSqlDb." + version + "/";
                string dbName = "lexAccess" + version;
                this.properties_["DB_DIR"] = dbDir;
                this.properties_["DB_NAME"] = dbName;
            }
            else

            {
                flag = false;
            }

            return flag;
        }

        private bool IsLegalVersion(string version)
        {
            bool flag = false;


            try

            {
                bool useClassPath = false;
                string configFile = this.configFile_;
                if (string.ReferenceEquals(configFile, null))

                {
                    useClassPath = true;
                    configFile = "data.config.lexAccess";
                }

                Configuration conf = new Configuration(configFile, useClassPath);


                this.firstVersion_ = int.Parse(conf.GetConfiguration("FIRST_VERSION"));

                this.latestVersion_ = int.Parse(conf.GetConfiguration("LATEST_VERSION"));
            }
            catch (Exception)
            {
            }


            if (version.Length == 4)

            {
                try

                {
                    int year = int.Parse(version);
                    if ((year >= this.firstVersion_) && (year <= this.latestVersion_))

                    {
                        flag = true;
                    }
                }
                catch (Exception)
                {
                }
            }
            else if (version.Length == 9)

            {
                if (version.EndsWith("ASCII", StringComparison.Ordinal))

                {
                    try

                    {
                        int year = int.Parse(version.Substring(0, 4));
                        if ((year >= this.firstVersion_) && (year <= this.latestVersion_))

                        {
                            flag = true;
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            return flag;
        }

        protected internal List<string> quitStrList_ = new List<string>();
        protected internal System.IO.StreamReader inReader_ = null;
        protected internal System.IO.StreamWriter outWriter_ = null;
        private bool fileOutput_ = false;
        protected internal bool runFlag_ = true;
        private Option option_ = new Option("");
        private string promptStr_ = null;

        private string configFile_ = null;
        private Configuration conf_ = null;
        private Connection conn_ = null;
        private LexAccessApi lexAccessApi_ = null;
        private Dictionary<string, string> properties_ = null;
        private string version_ = "2016";

        private long category_ = 0L;
        private long outCat_ = 2047L;
        private bool showTotalRecNum_ = false;
        private bool showQuery_ = false;
        private string query_ = "";
        private bool noOutputFlag_ = false;
        private string noOutputMsg_ = "-No Record Found-";
        private bool promptFlag_ = false;
        private int baseOpt_ = 0;
        private int formatOpt_ = 0;
        private int firstVersion_ = 2003;
        private int latestVersion_ = 2053;
        private Out out_ = new Out();
    }


}