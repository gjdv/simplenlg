using System;
using System.Collections.Generic;
using SimpleNLG.Main.lexicon.util.lexCheck.Lib;

namespace SimpleNLG.Main.lexicon.util.lexCheck.CheckCont
{
    using LexRecord = LexRecord;


    public class CheckContent

    {
        public const int STR_ASCII_NO_PUNC = 0;
        public const int STR_ASCII_PUNC = 1;
        public const int STR_NON_ASCII_NO_PUNC = 2;
        public const int STR_NON_ASCII_PUNC = 3;

        public static bool CheckSpacesForItem(LexRecord lexRecord, int contentType)

        {
            bool validFlag = true;
            string inItem = LexRecordUtil.GetItemFromLexRecord(lexRecord, contentType);

            string newInItem = StringTrim(inItem);
            if (!newInItem.Equals(inItem))

            {
                validFlag = false;
                ErrMsgUtilLexRecord.AddContentErrMsg(contentType, 7, inItem, lexRecord);


                LexRecordUtil.SetItemInLexRecord(lexRecord, contentType, newInItem);
            }

            return validFlag;
        }


        public static bool CheckSpacesForList(LexRecord lexRecord, int contentType)

        {
            bool validFlag = true;


            List<string> inList = LexRecordUtil.GetListFromLexRecord(lexRecord, contentType);
            for (int i = 0; i < inList.Count; i++)

            {
                string inItem = (string) inList[i];
                string newInItem = StringTrim(inItem);
                if (!newInItem.Equals(inItem))

                {
                    validFlag = false;
                    ErrMsgUtilLexRecord.AddContentErrMsg(contentType, 7, inItem, lexRecord);


                    LexRecordUtil.SetItemInListInLexRecordAt(lexRecord, contentType, newInItem, i);
                }
            }

            return validFlag;
        }

        private static string StringTrim(string inStr)

        {
            string outStr = string.Join(inStr," ");
            return outStr.Trim();
        }


        public static bool CheckDuplicatesForList(LexRecord lexRecord, int contentType)

        {
            bool validFlag = true;


            List<string> inList = LexRecordUtil.GetListFromLexRecord(lexRecord, contentType);
            List<string> uList = new List<string>();

            for (int i = 0; i < inList.Count; i++)

            {
                string inItem = (string) inList[i];

                if (uList.Contains(inItem) == true)

                {
                    validFlag = false;
                    ErrMsgUtilLexRecord.AddContentErrMsg(contentType, 2, inItem, lexRecord);
                }
                else

                {
                    uList.Add(inItem);
                }
            }

            if (!validFlag)

            {
                LexRecordUtil.SetListInLexRecord(lexRecord, contentType, uList);
            }

            return validFlag;
        }

        public static bool CheckDoublePipesForList(LexRecord lexRecord, int contentType)

        {
            bool validFlag = true;


            List<string> inList = LexRecordUtil.GetListFromLexRecord(lexRecord, contentType);
            foreach (string inItem in inList)

            {
                if (inItem.IndexOf("||", StringComparison.Ordinal) >= 0)

                {
                    ErrMsgUtilLexRecord.AddContentErrMsg(contentType, 3, inItem, lexRecord);

                    validFlag = false;
                }
            }

            return validFlag;
        }
    }


}