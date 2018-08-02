namespace SimpleNLG.Main.lexicon.util.lexCheck.Lib
{
    public class CheckFillerFormat

    {
        public static bool IsLegalFormat(bool printFlag, LineObject lineObject, string filler, int beginIndex,
            bool isTab, bool checkLength)

        {
            bool flag = true;

            if (filler.Length <= 0)

            {
                if (checkLength == true)

                {
                    ErrMsg.PrintErrMsg(printFlag, 1, lineObject, filler, beginIndex, beginIndex, isTab);

                    flag = false;
                }
            }
            else if ((filler[0] == '=') || (filler[0] == ' ') || (filler[0] == '|'))


            {
                ErrMsg.PrintErrMsg(printFlag, 2, lineObject, filler, beginIndex + 1, beginIndex + 1, isTab);

                flag = false;
            }

            return flag;
        }
    }
}