using SimpleNLG.Main.lexicon.util.lexCheck.Lib;

namespace SimpleNLG.Main.lexicon.util.lexCheck.CkLib
{
    using CheckSt = CheckSt;
    using ErrMsg = ErrMsg;
    using LexRecord = LexRecord;
    using LineObject = LineObject;
    using TokenObject = TokenObject;
    using UpdateLex = UpdateLex;


    public class CheckCode

    {
        public const int FQ_ONE = 1;
        public const int FQ_ZERO_ONE = 2;
        public const int FQ_ZERO_MANY = 3;
        public const int FQ_ONE_MANY = 4;
        public const int FQ_ONE_WHOLE_LINE = 5;
        public const int FQ_ZERO_ONE_WHOLE_LINE = 6;

        public static bool Check(LineObject lineObject, bool printFlag, CheckSt st, LexRecord lexObj,
            CheckObject checkObject, UpdateLex updateLex, int frequency, bool checkLength)

        {
            bool flag = false;
            switch (frequency)

            {
                case 1:
                    flag = CheckOne(lineObject, printFlag, st, lexObj, checkObject, updateLex, checkLength, true);

                    break;
                case 2:
                    flag = CheckZeroOne(lineObject, printFlag, st, lexObj, checkObject, updateLex, checkLength);

                    break;
                case 3:
                    flag = CheckZeroMany(lineObject, printFlag, st, lexObj, checkObject, updateLex, checkLength);

                    break;
                case 4:
                    flag = CheckOneMany(lineObject, printFlag, st, lexObj, checkObject, updateLex, checkLength);

                    break;
                case 5:
                    flag = CheckOneWholeLine(lineObject, printFlag, st, lexObj, checkObject, updateLex);

                    break;
                case 6:
                    flag = CheckZeroOneWholeLine(lineObject, printFlag, st, lexObj, checkObject, updateLex);
                    break;
            }


            return flag;
        }


        private static bool CheckOne(LineObject lineObject, bool printFlag, CheckSt st, LexRecord lexObj,
            CheckObject checkObject, UpdateLex updateLex, bool checkLength, bool updateState)

        {
            lineObject.SetGoToNext(true);

            bool flag = LineCheck.CheckStartStr(lineObject, printFlag, checkObject);

            st.UpdateLastState();

            if (flag == true)

            {
                TokenObject tokenObject = new TokenObject();

                flag = LineCheck.CheckSlotFiller(lineObject, printFlag, checkObject, tokenObject, checkLength);


                if (flag == true)

                {
                    if (updateState == true)

                    {
                        st.UpdateCurState(checkObject.GetNextState());
                    }

                    if (updateLex != null)

                    {
                        updateLex.Update(lexObj, tokenObject.GetToken());
                    }
                }
            }

            return flag;
        }


        private static bool CheckZeroOne(LineObject lineObject, bool printFlag, CheckSt st, LexRecord lexObj,
            CheckObject checkObject, UpdateLex updateLex, bool checkLength)

        {
            string line = lineObject.GetLine();
            bool flag = checkObject.IsNextStartStr(line);

            if (flag == true)

            {
                st.UpdateCurState(checkObject.GetNextState());
                lineObject.SetGoToNext(false);
                return flag;
            }

            if (st.GetCurState() != st.GetLastState())

            {
                st.UpdateLastState();
                flag = CheckOne(lineObject, printFlag, st, lexObj, checkObject, updateLex, checkLength, false);
            }
            else

            {
                ErrMsg.PrintErrMsg(printFlag, 82, lineObject, line, 0, line.Length, checkObject.IsTab());

                st.UpdateLastState();
                flag = false;
            }

            return flag;
        }

        private static bool CheckZeroMany(LineObject lineObject, bool printFlag, CheckSt st, LexRecord lexObj,
            CheckObject checkObject, UpdateLex updateLex, bool checkLength)

        {
            lineObject.SetGoToNext(true);
            st.UpdateLastState();

            bool flag = checkObject.IsNextStartStr(lineObject.GetLine());
            if (flag == true)

            {
                st.UpdateCurState(checkObject.GetNextState());
                lineObject.SetGoToNext(false);
                return flag;
            }

            flag = LineCheck.CheckStartStr(lineObject, printFlag, checkObject);
            if (flag == true)

            {
                TokenObject tokenObject = new TokenObject();

                flag = LineCheck.CheckSlotFiller(lineObject, printFlag, checkObject, tokenObject, checkLength);


                if (flag == true)

                {
                    updateLex.Update(lexObj, tokenObject.GetToken());
                }
            }

            return flag;
        }

        private static bool CheckOneMany(LineObject lineObject, bool printFlag, CheckSt st, LexRecord lexObj,
            CheckObject checkObject, UpdateLex updateLex, bool checkLength)

        {
            bool flag = false;

            if (st.GetCurState() != st.GetLastState())

            {
                st.UpdateLastState();
                flag = CheckOne(lineObject, printFlag, st, lexObj, checkObject, updateLex, checkLength, false);
            }
            else

            {
                st.UpdateLastState();
                flag = CheckZeroMany(lineObject, printFlag, st, lexObj, checkObject, updateLex, checkLength);
            }

            return flag;
        }

        private static bool CheckOneWholeLine(LineObject lineObject, bool printFlag, CheckSt st, LexRecord lexObj,
            CheckObject checkObject, UpdateLex updateLex)

        {
            lineObject.SetGoToNext(true);

            bool flag = LineCheck.CheckWholeLine(lineObject, printFlag, checkObject);


            if (flag == true)

            {
                st.UpdateLastState();
                st.UpdateCurState(checkObject.GetNextState());

                if (updateLex != null)

                {
                    updateLex.Update(lexObj, lineObject.GetLine());
                }
            }

            return flag;
        }

        private static bool CheckZeroOneWholeLine(LineObject lineObject, bool printFlag, CheckSt st, LexRecord lexObj,
            CheckObject checkObject, UpdateLex updateLex)

        {
            bool flag = checkObject.IsNextStartStr(lineObject.GetLine());

            if (flag == true)

            {
                st.UpdateCurState(checkObject.GetNextState());
                lineObject.SetGoToNext(false);
                return flag;
            }

            if (st.GetCurState() != st.GetLastState())

            {
                st.UpdateLastState();
                flag = CheckOneWholeLine(lineObject, printFlag, st, lexObj, checkObject, updateLex);
            }
            else

            {
                string line = lineObject.GetLine();
                st.UpdateLastState();
                ErrMsg.PrintErrMsg(printFlag, 82, lineObject, line, 0, line.Length, checkObject.IsTab());
            }

            return flag;
        }
    }
}