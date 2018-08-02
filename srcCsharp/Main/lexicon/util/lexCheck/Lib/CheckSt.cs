namespace SimpleNLG.Main.lexicon.util.lexCheck.Lib
{
    public class CheckSt

    {
        public const int NO_CHANGE = 0;

        public const int INCREASE_ST = 1;

        public const int CHECK_BASE = 10;

        public const int CHECK_SPELLING_VAR = 20;
        public const int CHECK_ENTRY = 21;
        public const int CHECK_CAT = 30;
        public const int CHECK_CAT_ENTRY = 40;
        public const int CHECK_AGREEMENT = 41;
        public const int CHECK_POSITION = 42;
        public const int CHECK_COMPLEMENT = 43;
        public const int CHECK_STATIVE = 44;
        public const int CHECK_NOMINALIZATION = 45;
        public const int CHECK_PROPER_NOUN = 46;
        public const int CHECK_GENDER = 47;
        public const int CHECK_INTEROATIVE = 48;
        public const int CHECK_TYPE = 49;
        public const int CHECK_MODIFICATION_TYPE = 50;

        public CheckSt()
        {
        }

        public CheckSt(int state)

        {
            curSt_ = state;
            lastSt_ = state;
        }


        public virtual int GetCurState()

        {
            return curSt_;
        }


        public virtual int GetLastState()

        {
            return lastSt_;
        }


        public virtual void SetCurState(int curSt)

        {
            curSt_ = curSt;
        }


        public virtual void SetLastState(int lastSt)

        {
            lastSt_ = lastSt;
        }


        public virtual void IncreaseState()

        {
            curSt_ += 1;
        }


        public virtual void UpdateLastState()

        {
            lastSt_ = curSt_;
        }


        public virtual void UpdateCurState(int newSt)

        {
            if (newSt != 0)

            {
                if (newSt == 1)

                {
                    curSt_ += 1;
                }
                else

                {
                    curSt_ = newSt;
                }
            }
        }


        public const int CHECK_NEGATIVE = 51;


        public const int CHECK_DEICTIC = 52;


        public const int CHECK_ACRONYM = 91;


        public const int CHECK_ABBREVIATION = 92;


        public const int CHECK_ANNOTATION = 95;


        public const int CHECK_SIGNATURE = 96;


        public const int CHECK_END = 99;


        public const int CHECK_ADJ = 100;


        public const int CHECK_ADJ_VARIANTS = 101;


        public const int CHECK_ADJ_POSITION = 102;


        public const int CHECK_ADJ_COMPL = 103;


        public const int CHECK_ADJ_STATIVE = 104;


        public const int CHECK_ADJ_NOMINALIZATION = 105;


        public const int CHECK_ADV = 110;


        public const int CHECK_ADV_VARIANTS = 111;


        public const int CHECK_ADV_INTERROGATIVE = 112;


        public const int CHECK_ADV_MODIFICATION = 113;


        public const int CHECK_ADV_NEGATIVE = 114;


        public const int CHECK_ADV_BROAD_NEGATIVE = 115;


        public const int CHECK_AUX = 120;


        public const int CHECK_AUX_VARIANT = 121;


        public const int CHECK_COMPL = 130;


        public const int CHECK_CONJ = 140;


        public const int CHECK_DET = 150;


        public const int CHECK_DET_VARIANTS = 151;


        public const int CHECK_DET_INTERROGATIVE = 152;


        public const int CHECK_DET_DEMONSTRATIVE = 153;


        public const int CHECK_MODAL = 160;


        public const int CHECK_MODAL_VARIANT = 161;


        public const int CHECK_NOUN = 170;


        public const int CHECK_NOUN_VARIANTS = 171;


        public const int CHECK_NOUN_COMPL = 172;


        public const int CHECK_NOUN_NOMINALIZATION = 173;

        public const int CHECK_NOUN_PROPER = 174;

        public const int CHECK_NOUN_TRADENAME = 175;

        public const int CHECK_NOUN_TRADEMARK = 176;

        public const int CHECK_PREP = 180;

        public const int CHECK_PRON = 190;

        public const int CHECK_PRON_VARIANTS = 191;

        public const int CHECK_PRON_GENDER = 192;

        public const int CHECK_PRON_INTERROGATIVE = 193;

        public const int CHECK_PRON_TYPE = 194;

        public const int CHECK_VERB = 200;

        public const int CHECK_VERB_VARIANTS = 201;

        public const int CHECK_VERB_INTRAN = 202;

        public const int CHECK_VERB_INTRAN2 = 203;

        public const int CHECK_VERB_TRAN = 204;

        public const int CHECK_VERB_DITRAN = 205;

        public const int CHECK_VERB_LINK = 206;

        public const int CHECK_VERB_CPLXTRAN = 207;

        public const int CHECK_VERB_NOMINALIZATION = 208;

        private int curSt_ = 10;
        private int lastSt_ = 10;
    }
}