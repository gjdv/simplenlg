using System;
using System.Collections.Generic;
using System.Text;
using SimpleNLG.Main.lexicon.util.lexCheck.Gram;
using SimpleNLG.Main.lexicon.util.lexCheck.Lib;

namespace SimpleNLG.Main.lexicon.util.lexCheck.Tools
{
    using CheckSt = lexCheck.Lib.CheckSt;
    using Configuration = lexCheck.Lib.Configuration;
    using LineObject = lexCheck.Lib.LineObject;
    using Option = gov.nih.nlm.nls.lvg.CmdLineSyntax.Option;
    using OptionItem = gov.nih.nlm.nls.lvg.CmdLineSyntax.OptionItem;
    using SystemOption = gov.nih.nlm.nls.lvg.CmdLineSyntax.SystemOption;
    using Out = gov.nih.nlm.nls.lvg.Util.Out;


    public class LexCheck : SystemOption


    {
        public static void Main(string[] args)

        {
            Out @out = new Out();
            string optionStr = "";
            Option inOption = new Option(optionStr);


            if (args.Length > 0)

            {
                optionStr = "";
                for (int i = 0; i < args.Length; i++)

                {
                    if (i == 0)

                    {
                        optionStr = args[i];
                    }
                    else

                    {
                        optionStr = optionStr + " " + args[i];
                    }
                }

                inOption = new Option(optionStr);
            }

            LexCheck lexCheck = new LexCheck();
            List<OptionItem> optionItems = inOption.GetOptionItems();

            if (SystemOption.CheckSyntax(inOption, lexCheck.GetOption(), false, true) == true)

            {
                lexCheck.ExecuteCommands(inOption, lexCheck.GetOption(), @out);

                if ((runFlag_ == true) && (inReader_ == null))

                {
                    try

                    {
                        helpMenu(@out);
                    }
                    catch (Exception)
                    {
                    }
                }
                else if (runFlag_ == true)

                {
                    bool useClassPath = false;
                    if (string.ReferenceEquals(configFile_, null))

                    {
                        useClassPath = true;
                        configFile_ = "data.config.lexCheck";
                    }

                    if (conf_ == null)

                    {
                        conf_ = new Configuration(configFile_, useClassPath);
                    }

                    RunProgram(@out);
                }
            }
            else

            {
                helpMenu(@out);
            }

            try

            {
                Close();
            }
            catch (Exception)
            {
            }
        }


        private static void RunProgram(Out @out)

        {
            string ti = conf_.GetConfiguration("TEXT_INDENT");
            string xi = conf_.GetConfiguration("XML_INDENT");
            string xmlHeader = conf_.GetConfiguration("XML_HEADER");


            string prepositionFile = conf_.GetConfiguration("LC_DIR") + conf_.GetConfiguration("PREPOSITION_FILE");


            string irregExpFile = conf_.GetConfiguration("LC_DIR") + conf_.GetConfiguration("IRREG_EXP_FILE");
            GlobalVars.SetTextIndent(GetStringContent(ti));
            GlobalVars.SetXmlIndent(GetStringContent(xi));
            GlobalVars.SetXmlHeader(GetStringContent(xmlHeader));
            GlobalVars.SetPrepositionFile(GetStringContent(prepositionFile));
            GlobalVars.SetIrregExpFile(GetStringContent(irregExpFile));
            CheckRecord(@out);
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

        protected internal virtual void ExecuteCommand(OptionItem optionItem, Option systemOption, Out @out)

        {
            OptionItem nameItem = OptionUtility.GetItemByName(optionItem, systemOption, false);

            if (CheckOption(nameItem, "-d") == true)

            {
                debugFlag_ = true;
            }
            else if (CheckOption(nameItem, "-f:r") == true)

            {
                format_ = 4;
            }
            else if (CheckOption(nameItem, "-f:t") == true)

            {
                format_ = 0;
            }
            else if (CheckOption(nameItem, "-f:tx") == true)

            {
                format_ = 3;
            }
            else if (CheckOption(nameItem, "-f:x") == true)

            {
                format_ = 1;
            }
            else if (CheckOption(nameItem, "-f:nxh") == true)

            {
                printXmlHeader_ = false;
            }
            else if (CheckOption(nameItem, "-h") == true)

            {
                helpMenu(@out);
                runFlag_ = false;
            }
            else if (CheckOption(nameItem, "-hs") == true)

            {
                systemOption.PrintOptionHierachy();
                runFlag_ = false;
            }
            else if (CheckOption(nameItem, "-i:STR") == true)

            {
                string inFile = nameItem.GetOptionArgument();
                if (!string.ReferenceEquals(inFile, null))

                {
                    try

                    {
                        inReader_ = new System.IO.StreamReader(
                            new System.IO.FileStream(inFile, System.IO.FileMode.Open, System.IO.FileAccess.Read),
                            Encoding.UTF8);
                    }
                    catch (IOException)

                    {
                        runFlag_ = false;
                        Console.Error.WriteLine("**Error: problem of opening/reading file " + inFile);
                    }
                }
            }
            else if (CheckOption(nameItem, "-o:STR") == true)

            {
                string outFile = nameItem.GetOptionArgument();
                try

                {
                    outWriter_ = new System.IO.StreamWriter(
                        new System.IO.FileStream(outFile, System.IO.FileMode.Create, System.IO.FileAccess.Write),
                        Encoding.UTF8);

                    fileOutput_ = true;
                }
                catch (Exception)

                {
                    runFlag_ = false;
                    Console.Error.WriteLine("**Error: problem of opening/writing file " + outFile);
                }
            }
            else if (CheckOption(nameItem, "-v") == true)

            {
                try

                {
                    string releaseStr = "lexCheck.2016";
                    @out.Println(outWriter_, releaseStr, fileOutput_, false);
                }
                catch (Exception)
                {
                }

                runFlag_ = false;
            }
            else if (CheckOption(nameItem, "-x:STR") == true)

            {
                configFile_ = nameItem.GetOptionArgument();
            }
        }

        private static void helpMenu(Out @out)

        {
            MenuPrint(@out, "");
            MenuPrint(@out, "Synopsis:");
            MenuPrint(@out, "  LexCheck [options]");
            MenuPrint(@out, "");
            MenuPrint(@out, "Description:");
            MenuPrint(@out,
                "  Validates text syntax in LexRecords and convert them among text, Xml, and Java objects from an input file.");
            MenuPrint(@out, "");
            MenuPrint(@out, "Options:");
            MenuPrint(@out, "  -d:     Print syntax validation debug message");
            MenuPrint(@out, "  -f:r    Print lexical records in release format");
            MenuPrint(@out, "  -f:t    Print lexical records in text format");
            MenuPrint(@out, "  -f:tx   Print lexical records in text and Xml format");
            MenuPrint(@out, "  -f:x    Print lexical records in Xml format");
            MenuPrint(@out, "  -f:nxh  No Xml header with version information");
            MenuPrint(@out, "  -h      Print program help information (this is it).");
            MenuPrint(@out, "  -hs     Print option's hierarchy structure.");
            MenuPrint(@out, "  -i:STR  Define input file");
            MenuPrint(@out, "  -o:STR  Define output file");
            MenuPrint(@out, "  -v:     Return the current verion identification of LexCheck");
            MenuPrint(@out, "  -x:STR  Loading an alternative configuration file.");
        }

        private static void MenuPrint(Out @out, string text)

        {
            try
            {
                @out.Println(outWriter_, text, fileOutput_, false);
            }
            catch (Exception)
            {
            }
        }


        protected internal virtual void DefineFlag()

        {
            string flagStr = "-d -f:r:t:tx:x:nxh -h -hs -i:STR -o:STR -v -x:STR";

            this.systemOption_ = new Option(flagStr);

            this.systemOption_.SetFlagFullName("-d", "Print_Error_Msg");
            this.systemOption_.SetFlagFullName("-f:r", "Release_Format");
            this.systemOption_.SetFlagFullName("-f:t", "Text_Format");
            this.systemOption_.SetFlagFullName("-f:tx", "Text_Xml_Format");
            this.systemOption_.SetFlagFullName("-f:x", "Xml_Format");
            this.systemOption_.SetFlagFullName("-f:nxh", "No_Xml_Header");
            this.systemOption_.SetFlagFullName("-h", "Help");
            this.systemOption_.SetFlagFullName("-hs", "Hierarchy_Struture");
            this.systemOption_.SetFlagFullName("-i", "Input_File");
            this.systemOption_.SetFlagFullName("-o", "Output_File");
            this.systemOption_.SetFlagFullName("-v", "Version");
            this.systemOption_.SetFlagFullName("-x", "Load_Configuration_file");
        }

        //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: private static void Close() throws java.io.IOException
        private static void Close()
        {
            if (outWriter_ != null)

            {
                outWriter_.Close();
            }

            if (inReader_ != null)

            {
                inReader_.Close();
            }
        }

        private static void CheckRecord(Out @out)

        {
            try

            {
                if (format_ == 1)

                {
                    if (printXmlHeader_ == true)

                    {
                        @out.Println(outWriter_, LexRecord.GetXmlHeader(), fileOutput_, false);
                    }

                    @out.Println(outWriter_, LexRecord.GetXmlRootBeginTag(), fileOutput_, false);
                }

                for (;;)

                {
                    if (lineObject_.IsGoToNext() == true)

                    {
                        lineObject_.SetLine(inReader_.ReadLine());
                        lineObject_.IncreaseLineNum();
                    }

                    if (!CheckLine(@out))
                    {
                        break;
                    }
                }

                if (format_ == 1)

                {
                    @out.Println(outWriter_, LexRecord.GetXmlRootEndTag(), fileOutput_, false);
                }

                inReader_.Close();
            }
            catch (Exception e)

            {
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
            }
        }

        private static bool CheckLine(Out @out)

        {
            bool flag = false;

            if ((lineObject_ == null) || (lineObject_.GetLine() == null))

            {
                return false;
            }

            flag = CheckGrammer.Check(lineObject_, printFlag_, st_, catSt_, debugFlag_);


            if ((st_.GetCurState() == 10) && (flag == true))

            {
                try

                {
                    switch (format_)

                    {
                        case 4:
                            @out.Print(outWriter_, CheckGrammer.GetLexRecord().GetReleaseFormatText(), fileOutput_,
                                false);


                            break;
                        case 0:
                            @out.Print(outWriter_, CheckGrammer.GetLexRecord().GetText(), fileOutput_, false);


                            break;
                        case 3:
                            @out.Println(outWriter_, CheckGrammer.GetLexRecord().GetText(), fileOutput_, false);


                            @out.Println(outWriter_, CheckGrammer.GetLexRecord().GetXml(0), fileOutput_, false);


                            break;
                        case 1:
                            @out.Println(outWriter_, CheckGrammer.GetLexRecord().GetXml(1), fileOutput_, false);

                            break;
                    }
                }
                catch (Exception)
                {
                }
            }

            return flag;
        }

        private static System.IO.StreamReader inReader_ = null;
        private static System.IO.StreamWriter outWriter_ = null;
        private static bool fileOutput_ = false;
        private static bool runFlag_ = true;
        private static bool debugFlag_ = false;
        private static bool printFlag_ = true;
        private static bool printXmlHeader_ = true;
        private const int TEXT_FORMAT = 0;
        private const int XML_FORMAT = 1;
        private const int TEXT_XML_FORMAT = 3;
        private const int RELEASE_FORMAT = 4;
        private static int format_ = 0;
        private static string cDate = "";
        private static string configFile_ = null;
        private static Configuration conf_ = null;
        private static CheckSt st_ = new CheckSt();
        private static CheckSt catSt_ = new CheckSt(40);

        private static LineObject lineObject_ = new LineObject();

        static LexCheck()

        {
            try
            {
                outWriter_ = new System.IO.StreamWriter(System.out, Encoding.UTF8);
            }
            catch (Exception e)

            {
                Console.WriteLine("** Error: File Exception: " + e.ToString());
            }
        }
    }

}