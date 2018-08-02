using System;
using System.Collections.Generic;
using System.Linq;
using SimpleNLG.Main.lexicon.util.lexCheck.Lib;

namespace SimpleNLG.Main.lexicon.util.lexCheck.Cat.Auxi
{
    public class CheckFormatAuxVariant : CheckFormat

    {
        private const int LEGAL_TENSE_CODE_NUM = 5;
        private const int LEGAL_AGREEMENT_FEATURE_NUM = 8;
        private const string NEGATIVE_CODE = "negative";
        private const string TC_PAST = "past";
        private const string TC_PRES = "pres";
        private const string TC_PAST_PART = "past_part";
        private const string TC_PRES_PART = "pres_part";
        private const string TC_INFINITIVE = "infinitive";
        private const string AF_FST_SING = "fst_sing";
        private const string AF_FST_PLUR = "fst_plur";
        private const string AF_SECOND = "second";
        private const string AF_SEC_SING = "sec_sing";
        private const string AF_SEC_PLUR = "sec_plur";
        private const string AF_THIRD = "third";
        private const string AF_THR_SING = "thr_sing";
        private const string AF_THR_PLUR = "thr_plur";

        public virtual bool IsLegalFormat(string filler)
        {
            bool flag = false;

            Queue<string> buf = new Queue<string>(filler.Split(";(,):".ToCharArray()).ToList().Where(x=>x!=""));
            string inflection = "";

            if (buf.Count > 0)
            {
                inflection = buf.Dequeue();
            }
            else

            {
                return false;
            }

            string tenseCode = "";
            if (buf.Count > 0)

            {
                tenseCode = buf.Dequeue();
            }
            else

            {
                return false;
            }

            if (tenseCode_.Contains(tenseCode) == true)

            {
                if (tenseCode.StartsWith("infinitive", StringComparison.Ordinal))

                {
                    flag = tenseCode.Equals("infinitive");
                }
                else if ((tenseCode.StartsWith("past_part", StringComparison.Ordinal) == true) ||
                         (tenseCode.StartsWith("pres_part", StringComparison.Ordinal) == true))


                {
                    string negative = "";
                    if (buf.Count > 0)

                    {
                        negative = buf.Dequeue();
                        flag = negative.Equals("negative");
                    }
                    else

                    {
                        flag = true;
                    }
                }
                else if ((tenseCode.StartsWith("past", StringComparison.Ordinal) == true) ||
                         (tenseCode.StartsWith("pres", StringComparison.Ordinal) == true))


                {
                    string feature = "";
                    if (buf.Count > 0)

                    {
                        feature = buf.Dequeue();
                        if (feature.Equals("negative") == true)

                        {
                            flag = true;
                        }
                        else if (agreementFeature_.Contains(feature) == true)

                        {
                            flag = true;
                            do
                            {
                                if (buf.Count == 0)
                                {
                                    break;
                                }
                                feature = buf.Dequeue();

                                flag = (feature.Equals("negative")) || (agreementFeature_.Contains(feature));

                            } while (flag);
                        }
                        else

                        {
                            flag = false;
                        }
                    }
                    else

                    {
                        flag = true;
                    }
                }
            }
            else
            {
                return false;
            }

            return flag;
        }


        private static HashSet<string> tenseCode_ = new HashSet<string>();

        private static HashSet<string> agreementFeature_ = new HashSet<string>();

        static CheckFormatAuxVariant()

        {
            tenseCode_.Add("past");
            tenseCode_.Add("pres");
            tenseCode_.Add("past_part");
            tenseCode_.Add("pres_part");
            tenseCode_.Add("infinitive");
            agreementFeature_.Add("fst_sing");
            agreementFeature_.Add("fst_plur");
            agreementFeature_.Add("second");
            agreementFeature_.Add("sec_sing");
            agreementFeature_.Add("sec_plur");
            agreementFeature_.Add("third");
            agreementFeature_.Add("thr_sing");
            agreementFeature_.Add("thr_plur");
        }
    }
}