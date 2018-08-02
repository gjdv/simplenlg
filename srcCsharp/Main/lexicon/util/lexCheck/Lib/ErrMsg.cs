using System;
using System.Text;

namespace SimpleNLG.Main.lexicon.util.lexCheck.Lib
{
    public class ErrMsg

    {
        public static void PrintErrMsg(bool printFlag, int index, LineObject lineObject, string token, int beginIndex,
            int endIndex, bool isTab)

        {
            if ((printFlag == true) && (index != -1))

            {
                curErrMsg_ = GetMsg(index, lineObject, token, beginIndex, endIndex, isTab);
                Console.WriteLine(curErrMsg_);
            }
        }


        public static string GetErrMsg()

        {
            return curErrMsg_;
        }


        public static void SetErrMsg(string errMsg)

        {
            curErrMsg_ = errMsg;
        }


        public static void ResetErrMsg()

        {
            curErrMsg_ = "";
        }

        private static string GetMsg(int index, LineObject lineObject, string token, int beginIndex, int endIndex,
            bool isTab)

        {
            string line = lineObject.GetLine();
            int lineNum = lineObject.GetLineNum();
            string msg = "";
            if (ReferenceEquals(token, null))

            {
                msg = "Syntax Error: at the beginning of line " + lineNum + LS + " Rule-" + (index + 1) + ": " +
                      errMsg_[index] + LS + line + LS + GetErrorPositionString(beginIndex, endIndex, isTab);
            }
            else

            {
                msg = "Syntax Error: at token '" + token + "' on line " + lineNum + LS + "  Rule-" + (index + 1) +
                      ": " + errMsg_[index] + LS + line + LS + GetErrorPositionString(beginIndex, endIndex, isTab);
            }

            return msg;
        }


        private static string GetErrorPositionString(int beginIndex, int endIndex, bool isTab)

        {
            if (beginIndex > endIndex)

            {
                int tempIndex = beginIndex;
                beginIndex = endIndex;
                endIndex = tempIndex;
            }

            if (beginIndex < 0)

            {
                beginIndex = 0;
            }

            if (endIndex < 0)

            {
                endIndex = 0;
            }

            StringBuilder positionStr = new StringBuilder();
            int startIndex = 0;
            if (isTab == true)

            {
                positionStr.Append('\t');
                startIndex = 1;
            }

            for (int i = startIndex; i < endIndex + 1; i++)

            {
                positionStr.Append(' ');
            }

            positionStr[beginIndex] = '^';
            positionStr[endIndex] = '^';
            return positionStr.ToString();
        }

        public static readonly string LS = Environment.NewLine;

        public const int NONE = -1;

        public const int FILLER_FORMAT_ERR = 0;

        public const int FILLER_BLANK_ERR = 1;

        public const int FILLER_START_CHAR_ERR = 2;

        public const int BASE_SLOT_ERR = 3;

        public const int SPELLING_VAR_SLOT_ERR = 4;

        public const int ENTRY_SLOT_ERR = 5;

        public const int ENTRY_FILLER_ERR = 6;

        public const int TAB_ERR = 7;

        public const int CAT_SLOT_ERR = 8;

        public const int CAT_FILLER_ERR = 9;

        public const int VARIANTS_ERR = 10;

        public const int VARIANTS_FORMAT_ERR = 11;

        public const int ANNOTATION_SLOT_ERR = 12;

        public const int SIGNATURE_SLOT_ERR = 13;

        public const int END_SLOT_ERR = 14;

        public const int END_CODE_ERR = 15;

        public const int ABBREVIATION_SLOT_ERR = 16;

        public const int ABBREVIATION_FILLER_ERR = 17;

        public const int ACRONYM_SLOT_ERR = 18;

        public const int ACRONYM_FILLER_ERR = 19;

        public const int DUPLICATE_SLOT_ERR = 20;

        public const int DET_VAR_SLOT_ERR = 21;

        public const int DET_VAR_FILLER_ERR = 22;

        public const int DET_INTERROGATIVE_SLOT_ERR = 23;

        public const int DET_DEMONSTRATIVE_SLOT_ERR = 24;

        public const int PRON_VAR_SLOT_ERR = 25;

        public const int PRON_VAR_FILLER_ERR = 26;

        public const int PRON_GENDER_SLOT_ERR = 27;

        public const int PRON_GENDER_FILLER_ERR = 28;

        public const int PRON_INTERROGATIVE_SLOT_ERR = 29;

        public const int PRON_TYPE_SLOT_ERR = 30;

        public const int PRON_TYPE_FILLER_ERR = 31;

        public const int MODAL_VAR_SLOT_ERR = 32;

        public const int MODAL_VAR_FILLER_ERR = 33;

        public const int AUX_VAR_SLOT_ERR = 34;

        public const int AUX_VAR_FILLER_ERR = 35;

        public const int ADV_VAR_SLOT_ERR = 36;

        public const int ADV_VAR_FILLER_ERR = 37;

        public const int ADV_INTERROGATIVE_SLOT_ERR = 38;

        public const int ADV_MODIFICATION_SLOT_ERR = 39;

        public const int ADV_MODIFICATION_FILLER_ERR = 40;

        public const int ADV_NEGATIVE_SLOT_ERR = 41;

        public const int ADV_BROAD_NEGATIVE_SLOT_ERR = 42;

        public const int ADJ_VAR_SLOT_ERR = 43;

        public const int ADJ_VAR_FILLER_ERR = 44;

        public const int ADJ_POSITION_SLOT_ERR = 45;

        public const int ADJ_POSITION_FILLER_ERR = 46;

        public const int ADJ_COMPL_SLOT_ERR = 47;

        public const int ADJ_COMPL_FILLER_ERR = 48;

        public const int ADJ_STATIVE_SLOT_ERR = 49;

        public const int ADJ_NOMINALIZATION_SLOT_ERR = 50;

        public const int ADJ_NOMINALIZATION_FILLER_ERR = 51;

        public const int NOUN_VAR_SLOT_ERR = 52;

        public const int NOUN_VAR_FILLER_ERR = 53;

        public const int NOUN_COMPL_SLOT_ERR = 54;

        public const int NOUN_COMPL_FILLER_ERR = 55;

        public const int NOUN_NOMINALIZATION_SLOT_ERR = 56;

        public const int NOUN_NOMINALIZATION_FILLER_ERR = 57;

        public const int NOUN_TRADENAME_SLOT_ERR = 58;

        public const int NOUN_TRADENAME_FILLER_ERR = 59;

        public const int NOUN_TRADEMARK_SLOT_ERR = 60;

        public const int NOUN_TRADEMARK_FILLER_ERR = 61;

        public const int NOUN_PROPER_SLOT_ERR = 62;

        public const int VERB_VAR_SLOT_ERR = 63;

        public const int VERB_VAR_FILLER_ERR = 64;

        public const int VERB_INTRAN_SLOT_ERR = 65;

        public const int VERB_INTRAN_FILLER_ERR = 66;

        public const int VERB_INTRAN2_SLOT_ERR = 67;

        public const int VERB_INTRAN2_FILLER_ERR = 68;

        public const int VERB_TRAN_SLOT_ERR = 69;

        public const int VERB_TRAN_FILLER_ERR = 70;

        public const int VERB_DITRAN_SLOT_ERR = 71;

        public const int VERB_DITRAN_FILLER_ERR = 72;

        public const int VERB_LINK_SLOT_ERR = 73;

        public const int VERB_LINK_FILLER_ERR = 74;

        public const int VERB_CPLXTRAN_SLOT_ERR = 75;

        public const int VERB_CPLXTRAN_FILLER_ERR = 76;

        public const int VERB_NOMINALIZATION_SLOT_ERR = 77;

        public const int VERB_NOMINALIZATION_FILLER_ERR = 78;

        public const int VERB_COMPL_NUM_ERR = 79;

        public const int SLOT_FILLER_ERR = 80;

        public const int SLOT_FILLER_NUMBER_ERR = 81;

        public const int SLOT_NUMBER_ERR = 82;

        private static string[] errMsg_ = new string[]
        {
            "The filler has an illegal format", "A blank filler on the right side of slot= is not allowed",
            "The first character of the filler is illegal", "A lexical record begins with '{base='",
            "A line begins with 'spelling_variant=' is expected", "A line begins with 'entry=' is expected",
            "The Eui is a seven number preceded by the letter \"E\"", "A line begins with tab is expected",
            "A line begins with '\tcat=' is expected", "The category is illegal",
            "'\tvariants=' slot is missing or has illegal format", "The filler of 'variants=' is illegal",
            "A line begins with 'annotation=' is expected", "A line begins with 'signature=' is expected",
            "A line begins with '}' is expected", "A lexical record ends with '}'",
            "A line begins with '\tabbreviation_of=' is expected", "The format of abbreviation is illegal",
            "A line begins with '\tacronym_of=' is expected", "The format of acronym is illegal",
            "The slot= is duplicated", "'\tvariants=' slot is missing or has illegal format",
            "The filler of 'variants=' is illegal", "A line begins with '\tinterrogative' is expected",
            "A line begins with '\tdemonstrative' is expected", "'\tvariants=' slot is missing or has illegal format",
            "The filler of 'variants=' is illegal", "'\tgender=' slot is missing or has illegal format",
            "The filler of 'gender=' is illegal", "A line begins with '\tinterrogative' is expected",
            "'\ttype=' slot is missing or has illegal format", "The filler of 'type=' is illegal",
            "'\tvariant=' slot is missing or has illegal format", "The filler of 'variant=' is illegal",
            "'\tvariant=' slot is missing or has illegal format", "The filler of 'variant=' is illegal",
            "'\tvariants=' slot is missing or has illegal format", "The filler of '\tvariants=' is illegal",
            "'\tinterrogative' slot is missing or has illegal format",
            "'\tmodification_type=' slot is missing or has illegal format",
            "The filler of '\tmodification_type=' is illegal", "'\tnegative' slot is missing or has illegal format",
            "'\tbroad_negative' slot is missing or has illegal format",
            "'\tvariants=' slot is missing or has illegal format", "The filler of '\tvariants=' is illegal",
            "'\tposition=' slot is missing or has illegal format", "The filler of '\tposition=' is illegal",
            "'\tcompl=' slot is missing or has illegal format", "The filler of '\tcompl=' is illegal",
            "'\tstative=' slot is missing or has illegal format",
            "'\tnominalization=' slot is missing or has illegal format", "The filler of '\tnominalization=' is illegal",
            "'\tvariants=' slot is missing or has illegal format", "The filler of '\tvariants=' is illegal",
            "'\tcompl=' slot is missing or has illegal format", "The filler of '\tcompl=' is illegal",
            "'\tnominalization_of=' slot is missing or has illegal format",
            "The filler of '\tnominalization_of=' is illegal", "'\ttrademark=' slot is missing or has illegal format",
            "The filler of '\ttrademark=' is illegal", "'\ttrademark' slot is missing or has illegal format",
            "The filler of '\ttrademark' is illegal", "'\tproper=' slot is missing or has illegal format",
            "'\tvariants=' slot is missing or has illegal format", "The filler of '\tvariants=' is illegal",
            "'\tintran' slot is missing or has illegal format", "The filler of '\tintran' is illegal",
            "'\tintran;part( )' slot is missing or has illegal format", "The filler of '\tintran;part( )' is illegal",
            "'\ttran=' slot is missing or has illegal format", "The filler of '\ttran=' is illegal",
            "'\tditran=' slot is missing or has illegal format", "The filler of '\tditran=' is illegal",
            "'\tlink=' slot is missing or has illegal format", "The filler of '\tlink=' is illegal",
            "'\tcplxtran=' slot is missing or has illegal format", "The filler of '\tcplxtran=' is illegal",
            "'\tnominalization=' slot is missing or has illegal format", "The filler of '\tnominalization=' is illegal",
            "The number of verb complement is 0 ( at least 1)", "Can't find delimiter in CheckSlotFiller()",
            "More than one slot=filler exists", "More than one slot exists"
        };


        private static string curErrMsg_ = "";
    }
}