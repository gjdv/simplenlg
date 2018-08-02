using System;
using System.Collections.Generic;
using SimpleNLG.Main.lexicon.util.lexCheck.Lib;

namespace SimpleNLG.Main.lexicon.util.lexCheck.CheckCont
{
    using LexRecord = LexRecord;


    public class CheckEntry

    {
        public static bool CheckRegd(LexRecord lexRecord, string inBase, int contentType)

        {
            bool validFlag = true;
            string cat = lexRecord.GetCategory();
            List<string> variants = new List<string>();
            if (cat.Equals(LexRecordUtil.GetCategory(10)))

            {
                variants = lexRecord.GetCatEntry().GetVerbEntry().GetVariants();
            }
            else if (cat.Equals(LexRecordUtil.GetCategory(0)))

            {
                variants = lexRecord.GetCatEntry().GetAdjEntry().GetVariants();
            }

            if (variants.Count > 0)

            {
                bool hasRegd = false;

                for (int i = 0; i < variants.Count; i++)

                {
                    string variant = (string) variants[i];
                    if (variant.Equals("regd") == true)

                    {
                        hasRegd = true;
                        break;
                    }
                }

                if (hasRegd == true)

                {
                    char lastChar = InflVarsAndAgreements.GetLastChar(inBase);
                    char last2Char = InflVarsAndAgreements.GetLast2Char(inBase);
                    string lastCharStr = (new char?(lastChar)).ToString();
                    string last2CharStr = (new char?(last2Char)).ToString();


                    if ((!InflVarsAndAgreements.consonants_.Contains(lastCharStr)) ||
                        (!InflVarsAndAgreements.vowels_.Contains(last2CharStr)))


                    {
                        validFlag = false;
                    }
                }

                if (!validFlag)

                {
                    ErrMsgUtilLexRecord.AddContentErrMsg(contentType, 5, inBase, lexRecord);
                }
            }

            return validFlag;
        }

        public static bool CheckGlreg(LexRecord lexRecord, string inBase, int contentType)

        {
            bool validFlag = true;
            string cat = lexRecord.GetCategory();
            if (cat.Equals(LexRecordUtil.GetCategory(7)))

            {
                List<string> variants = lexRecord.GetCatEntry().GetNounEntry().GetVariants();

                bool hasGlreg = false;
                for (int i = 0; i < variants.Count; i++)

                {
                    string variant = (string) variants[i];
                    if ((variant.Equals("glreg")) || (variant.Equals("group(glreg)")))


                    {
                        hasGlreg = true;
                        break;
                    }
                }

                if (hasGlreg == true)

                {
                    validFlag = false;
                    for (int j = 0; j < glregEnds_.Count; j++)

                    {
                        string ending = (string) glregEnds_[j];
                        if (inBase.EndsWith(ending, StringComparison.Ordinal) == true)

                        {
                            validFlag = true;
                            break;
                        }
                    }
                }

                if (!validFlag)

                {
                    ErrMsgUtilLexRecord.AddContentErrMsg(contentType, 4, inBase, lexRecord);
                }
            }

            return validFlag;
        }

        private static List<string> glregEnds_ = new List<string>();

        static CheckEntry()
        {
            glregEnds_.Add("us");
            glregEnds_.Add("ma");
            glregEnds_.Add("a");
            glregEnds_.Add("um");
            glregEnds_.Add("on");
            glregEnds_.Add("sis");
            glregEnds_.Add("is");
            glregEnds_.Add("men");
            glregEnds_.Add("ex");
            glregEnds_.Add("x");
        }
    }


}