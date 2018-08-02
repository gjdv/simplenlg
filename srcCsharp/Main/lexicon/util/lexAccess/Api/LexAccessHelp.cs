namespace SimpleNLG.Main.lexicon.util.lexAccess.Api
{
    using HelpMenu = gov.nih.nlm.nls.lvg.Tools.CmdLineTools.HelpMenu;
    using Out = gov.nih.nlm.nls.lvg.Util.Out;


    public class LexAccessHelp

    {
        //JAVA TO C# CONVERTER NOTE: Members cannot have the same name as their enclosing type:
        public static void LexAccessHelp_Renamed(System.IO.StreamWriter bw, bool fileOutput, Out @out)

        {
            HelpMenu helpMenu = new HelpMenu(bw, fileOutput, @out);


            helpMenu.Println("");
            helpMenu.Println("Synopsis:");
            helpMenu.Println("  lexAccess [options]");
            helpMenu.Println("");
            helpMenu.Println("Description:");
            helpMenu.Println("  LexAccess is a program to retrieve data from the SPECIALIST lexicon");
            helpMenu.Println("");
            helpMenu.Println("Options:");
            helpMenu.Println("  -b:b      Base begins with input term");
            helpMenu.Println("  -b:c      Base contains input term");
            helpMenu.Println("  -b:e      Base ends with input term");
            helpMenu.Println("  -b:ex     Base exact match with input term");
            helpMenu.Println("  -c:<cat>  Specify category");
            helpMenu.Println("  -c:h      Help information for specifying category");
            helpMenu.Println("  -ci       Show configuration information");
            helpMenu.Println("  -cn:LONG  Specify category in value");
            helpMenu.Println("  -cnf:LONG Category filter in value");
            helpMenu.Println("  -f:h      Help information for output format");
            helpMenu.Println("  -h        Print program help information (this is it)");
            helpMenu.Println("  -i:STR    Specify input file");
            helpMenu.Println("  -n        Print -No Records Found-");
            helpMenu.Println("  -o:STR    Specify output file");
            helpMenu.Println("  -p        Show prompt");
            helpMenu.Println("  -q        Print query");
            helpMenu.Println("  -rv:STR   Run a specified version (must include data set)");
            helpMenu.Println("  -s:STR    Specify a field separator");
            helpMenu.Println("  -t        Print total number of retrieved records");
            helpMenu.Println("  -v        Print the current version of LexAccess");
            helpMenu.Println("  -x:STR    Specify an alternate configuration file");
        }


        public static void CategoryHelp(System.IO.StreamWriter bw, bool fileOutput, Out @out)

        {
            HelpMenu helpMenu = new HelpMenu(bw, fileOutput, @out);

            helpMenu.Println("  -c:a     Specify category to Adj (1)");
            helpMenu.Println("  -c:all   Specify category to all (2047)");
            helpMenu.Println("  -c:b     Specify category to Adv (2)");
            helpMenu.Println("  -c:c     Specify category to Compl (8)");
            helpMenu.Println("  -c:d     Specify category to Det (32)");
            helpMenu.Println("  -c:h     Help menu for specifying category");
            helpMenu.Println("  -c:j     Specify category to Conj (16)");
            helpMenu.Println("  -c:m     Specify category to Modal (64)");
            helpMenu.Println("  -c:n     Specify category to Noun (128)");
            helpMenu.Println("  -c:p     Specify category to Prep (256)");
            helpMenu.Println("  -c:r     Specify category to Pron (512)");
            helpMenu.Println("  -c:v     Specify category to Verb (1024)");
            helpMenu.Println("  -c:x     Specify category to Aux (4)");
        }


        public static void FormatHelp(System.IO.StreamWriter bw, bool fileOutput, Out @out)

        {
            HelpMenu helpMenu = new HelpMenu(bw, fileOutput, @out);

            helpMenu.Println("  -f:b     Print base form");
            helpMenu.Println("  -f:bd    Print base form with details information");
            helpMenu.Println("  -f:h     Help menu for print options");
            helpMenu.Println("  -f:i     Print inflected terms");
            helpMenu.Println("  -f:id    Print inflected terms with details information");
            helpMenu.Println("  -f:s     Print base form and spelling variants");
            helpMenu.Println("  -f:sd    Print base form and spelling variants details");
            helpMenu.Println("  -f:t     Print lexical records in text format (default)");
            helpMenu.Println("  -f:tx    Print lexical records in xml & text format");
            helpMenu.Println("  -f:x     Print lexical records in xml format");
        }
    }


}