using System.Collections.Generic;
using SimpleNLG.Main.lexicon.util.lexCheck.Lib;

namespace SimpleNLG.Main.lexicon.util.lexCheck.CheckCont
{
    using LexRecord = LexRecord;

    public class LexRecordUtil

    {
        public const string FS_STR = "|";
        public const int CONTENT_NONE = 0;
        public const int CONTENT_CIT = 1;
        public const int CONTENT_SV = 2;
        public const int CONTENT_CAT = 3;
        public const int CONTENT_EUI = 4;
        public const int CONTENT_VAR = 5;
        public const int CONTENT_NOM = 6;
        public const int CONTENT_ABB = 7;
        public const int CONTENT_ACR = 8;

        public static string GetItemFromLexRecord(LexRecord lexRecord, int contentType)
        {
            string outItem = "";
            switch (contentType)

            {
                case 1:
                    outItem = lexRecord.GetBase();
                    break;
            }

            return outItem;
        }

        public static List<string> GetListFromLexRecord(LexRecord lexRecord, int contentType)

        {
            List<string> outList = new List<string>();
            switch (contentType)

            {
                case 2:
                    outList = lexRecord.GetSpellingVars();
                    break;
                case 5:
                    outList = lexRecord.GetVariants();
                    break;
                case 6:
                    outList = lexRecord.GetNominalizations();
                    break;
                case 7:
                    outList = lexRecord.GetAbbreviations();
                    break;
                case 8:
                    outList = lexRecord.GetAcronyms();
                    break;
            }

            return outList;
        }

        public static void SetItemInLexRecord(LexRecord lexRecord, int contentType, string item)

        {
            switch (contentType)

            {
                case 1:
                    lexRecord.SetBase(item);
                    break;
            }
        }


        public static void SetListInLexRecord(LexRecord lexRecord, int contentType, List<string> inList)

        {
            switch (contentType)

            {
                case 2:
                    lexRecord.SetSpellingVars(inList);
                    break;
                case 5:
                    lexRecord.SetVariants(inList);
                    break;
                case 6:
                    lexRecord.SetNominalizations(inList);
                    break;
                case 7:
                    lexRecord.SetAbbreviations(inList);
                    break;
                case 8:
                    lexRecord.SetAcronyms(inList);
                    break;
            }
        }

        public static void SetItemInListInLexRecordAt(LexRecord lexRecord, int contentType, string item, int index)

        {
            switch (contentType)

            {
                case 2:
                    lexRecord.GetSpellingVars()[index] = item;
                    break;
                case 5:
                    lexRecord.GetVariants()[index] = item;
                    break;
                case 6:
                    lexRecord.GetNominalizations()[index] = item;
                    break;
                case 7:
                    lexRecord.GetAbbreviations()[index] = item;
                    break;
                case 8:
                    lexRecord.GetAcronyms()[index] = item;
                    break;
            }
        }

        public static string GetContentTypeStr(int contentType)

        {
            string contentTypeStr = contentTypeStrs_[0];
            if ((contentType > 0) && (contentType <= contentTypeStrs_.Length))

            {
                contentTypeStr = contentTypeStrs_[contentType];
            }

            return contentTypeStr;
        }

        public static string GetCrossRefTypeStr(int crossRefType)

        {
            string crossRefTypeStr = crossRefTypeStrs_[0];
            if ((crossRefType > 0) && (crossRefType <= crossRefTypeStrs_.Length))

            {
                crossRefTypeStr = crossRefTypeStrs_[crossRefType];
            }

            return crossRefTypeStr;
        }

        public static bool IsCategory(string inCat)

        {
            bool validFlag = catStrs_.Contains(inCat);
            return validFlag;
        }

        public static string GetCategory(int catIndex)

        {
            string catStr = "";
            if ((catIndex >= 0) && (catIndex < catStrs_.Count))

            {
                catStr = (string) catStrs_[catIndex];
            }

            return catStr;
        }

        public static string GetLexRecordInfo(LexRecord lexRecord)

        {
            string eui = lexRecord.GetEui();
            string cat = lexRecord.GetCategory();
            string @base = lexRecord.GetBase();
            string info = eui + "|" + @base + "|" + cat;
            return info;
        }


        private static string[] contentTypeStrs_ = new string[]
        {
            "None", "citation", "spelling variants", "category", "EUI", "variants", "nominalizations", "abbreviations",
            "acronyms"
        };


        public const int CROSS_CHK_NONE = 0;


        public const int CROSS_CHK_DUP_EUI = 1;


        public const int CROSS_CHK_DUP_REC = 2;


        public const int CROSS_CHK_NOM = 3;


        public const int CROSS_CHK_ABB = 4;


        public const int CROSS_CHK_ACR = 5;


        private static string[] crossRefTypeStrs_ = new string[]
            {"None", "duplicate EUIs", "duplicate LexRecords", "nominalizations", "abbreviations", "acronyms"};

        public const int CAT_ADJ = 0;

        public const int CAT_ADV = 1;

        public const int CAT_AUX = 2;

        public const int CAT_COMPL = 3;

        public const int CAT_CONJ = 4;

        public const int CAT_DET = 5;

        public const int CAT_MODAL = 6;

        public const int CAT_NOUN = 7;

        public const int CAT_PREP = 8;
        public const int CAT_PRON = 9;
        public const int CAT_VERB = 10;
        private static List<string> catStrs_ = new List<string>();

        static LexRecordUtil()
        {
            catStrs_.Add("adj");
            catStrs_.Add("adv");
            catStrs_.Add("aux");
            catStrs_.Add("compl");
            catStrs_.Add("conj");
            catStrs_.Add("det");
            catStrs_.Add("modal");
            catStrs_.Add("noun");
            catStrs_.Add("prep");
            catStrs_.Add("pron");
            catStrs_.Add("verb");
        }
    }


}