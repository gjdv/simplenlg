using System.Collections.Generic;
using SimpleNLG.Main.lexicon.util.lexCheck.Lib;

namespace SimpleNLG.Main.lexicon.util.lexCheck.CheckCont
{
    using LexRecord = LexRecord;


    public class CrossCheckDupEuis

    {
        //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public static boolean Check(java.util.Vector<gov.nih.nlm.nls.lexCheck.Lib.LexRecord> lexRecords) throws java.io.IOException
        public static bool Check(List<LexRecord> lexRecords)


        {
            bool validFlag = true;
            HashSet<string> euiList = new HashSet<string>();

            int recSize = lexRecords.Count;
            for (int i = 0; i < recSize; i++)

            {
                LexRecord lexRecord = (LexRecord) lexRecords[i];
                string eui = lexRecord.GetEui();
                if (!euiList.Add(eui))

                {
                    validFlag = false;
                    ErrMsgUtilLexicon.AddContentErrMsg(1, 1, eui, lexRecord);
                }
            }

            return validFlag;
        }
    }


}