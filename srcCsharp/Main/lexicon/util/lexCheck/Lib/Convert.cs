using System;
using System.Globalization;

namespace SimpleNLG.Main.lexicon.util.lexCheck.Lib
{
    public class Convert

    {
        public const string UNIX_LINE_SEP = "\n";
        public const string PC_LINE_SEP = "^M\n";

        public static void Main(string[] args)

        {
            string inStr = "ASCII: A-Z,a-z.\n&,<,>,\",  .\nSpace Test";

            if (args.Length == 1)

            {
                inStr = args[0];
            }

            Console.WriteLine("-- in: " + inStr);
            Console.WriteLine("-- Name:" + ToNamedEntity(inStr));
            Console.WriteLine("-- Numeric:" + ToNumericEntity(inStr));
            Console.WriteLine("-- ToHtml:" + AsciiToHtml(inStr));
        }


        public static string ToNumericEntity(string ascii)

        {
            int curPos = 0;
            for (int i = 0; i < ENTITY_LIST.Length; i++)

            {
                int length = ENTITY_LIST[i][2].Length;
                while ((curPos = ascii.IndexOf(ENTITY_LIST[i][2], curPos + 1, StringComparison.Ordinal)) >= 0)

                {
                    ascii = ascii.Substring(0, curPos) + ENTITY_LIST[i][0] + ascii.Substring(curPos + length);
                }
            }

            return ascii;
        }


        public static string ToNamedEntity(string ascii)

        {
            int curPos = 0;
            for (int i = 0; i < ENTITY_LIST.Length; i++)

            {
                int length = ENTITY_LIST[i][2].Length;
                while ((curPos = ascii.IndexOf(ENTITY_LIST[i][2], curPos + 1, StringComparison.Ordinal)) >= 0)

                {
                    ascii = ascii.Substring(0, curPos) + ENTITY_LIST[i][1] + ascii.Substring(curPos + length);
                }
            }

            return ascii;
        }


        public static string AsciiToHtml(string ascii)

        {
            int curPos = 0;
            for (int i = 0; i < ENTITY_LIST.Length; i++)

            {
                int length = ENTITY_LIST[i][2].Length;
                while ((curPos = ascii.IndexOf(ENTITY_LIST[i][2], curPos + 1, StringComparison.Ordinal)) >= 0)

                {
                    ascii = ascii.Substring(0, curPos) + ENTITY_LIST[i][0] + ascii.Substring(curPos + length);
                }
            }

            while ((curPos = ascii.IndexOf(" ", StringComparison.Ordinal)) >= 0)

            {
                ascii = ascii.Substring(0, curPos) + "&nbsp;" + ascii.Substring(curPos + 1);
            }

            while ((curPos = ascii.IndexOf("\n", StringComparison.Ordinal)) >= 0)

            {
                ascii = ascii.Substring(0, curPos) + "&#010;" + ascii.Substring(curPos + 1);
            }

            while ((curPos = ascii.IndexOf("\t", StringComparison.Ordinal)) >= 0)

            {
                ascii = ascii.Substring(0, curPos) + "&#009;" + ascii.Substring(curPos + 1);
            }

            return ascii;
        }


        public static string HtmlToAscii(string html)

        {
            int lastPos = 0;
            int curPos = html.IndexOf("&#", StringComparison.Ordinal);
            while (curPos >= lastPos)

            {
                int offSet = curPos + 2;
                while ((offSet < html.Length) && (char.IsDigit(html[offSet])))


                {
                    offSet++;
                }

                if ((offSet >= html.Length) || (html[offSet] != ';'))


                {
                    curPos += 2;
                }
                else

                {
                    int ascii = -1;
                    try

                    {
                        ascii = int.Parse(html.Substring(curPos + 2, offSet - (curPos + 2)));
                    }
                    catch (Exception)
                    {
                    }

                    html = html.Substring(0, curPos) + (char) ascii + html.Substring(offSet + 1);
                }

                lastPos = curPos;
                curPos += html.Substring(curPos).IndexOf("&#", StringComparison.Ordinal);
            }

            for (int i = 0; i < ENTITY_LIST.Length; i++)

            {
                int length = ENTITY_LIST[i][0].Length;
                while ((curPos = html.IndexOf(ENTITY_LIST[i][0], StringComparison.Ordinal)) >= 0)

                {
                    html = html.Substring(0, curPos) + ENTITY_LIST[i][2] + html.Substring(curPos + length);
                }
            }

            while ((curPos = html.IndexOf(" ", StringComparison.Ordinal)) >= 0)

            {
                html = html.Substring(0, curPos) + html.Substring(curPos + 1);
            }

            return html;
        }


        public static string ToUnixLineSeparator(string inStr)

        {
            string outStr = inStr;
            string src = "\n";
            string tar = "\n";
            int tarSize = tar.Length;
            int srcSize = src.Length;
            int index = outStr.IndexOf(src, StringComparison.Ordinal);
            while (index >= 0)

            {
                char mC = outStr[index - 1];
                if (char.GetUnicodeCategory(mC) == UnicodeCategory.Format) // 15

                {
                    outStr = outStr.Substring(0, index - 1) + tar + outStr.Substring(index + 1);
                    index = outStr.IndexOf(src, index, StringComparison.Ordinal);
                }
                else

                {
                    index = outStr.IndexOf(src, index + 1, StringComparison.Ordinal);
                }
            }

            return outStr;
        }


        public static string ToPcLineSeparator(string inStr)

        {
            string unixStr = ToUnixLineSeparator(inStr);

            string pcStr = Replace(unixStr, "\n", "^M\n");
            return pcStr;
        }


        public static string Replace(string inStr, string src, string tar)

        {
            string outStr = inStr;
            int tarSize = tar.Length;
            int srcSize = src.Length;
            int index = outStr.IndexOf(src, StringComparison.Ordinal);
            while (index >= 0)

            {
                outStr = outStr.Substring(0, index) + tar + outStr.Substring(index + srcSize);
                index = outStr.IndexOf(src, index + tarSize, StringComparison.Ordinal);
            }

            return outStr;
        }


        private static readonly string[][] ENTITY_LIST = new string[][]
        {
            new string[] {"&#038;", "&amp;", "&"},
            new string[] {"&#034;", "&quot;", "\""},
            new string[] {"&#060;", "&lt;", "<"},
            new string[] {"&#062;", "&gt;", ">"},
            new string[] {"&#160;", "&nbsp;", " "}
        };
    }
}