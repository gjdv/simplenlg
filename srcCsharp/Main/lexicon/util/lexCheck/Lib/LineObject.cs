namespace SimpleNLG.Main.lexicon.util.lexCheck.Lib
{
    public class LineObject

    {
        public virtual void SetLine(string line)

        {
            line_ = line;
        }


        public virtual void SetLineNum(int lineNum)

        {
            lineNum_ = lineNum;
        }


        public virtual void SetGoToNext(bool goToNext)

        {
            goToNext_ = goToNext;
        }


        public virtual string GetLine()

        {
            return line_;
        }


        public virtual int GetLineNum()

        {
            return lineNum_;
        }


        public virtual bool IsGoToNext()

        {
            return goToNext_;
        }


        public virtual void IncreaseLineNum()

        {
            lineNum_ += 1;
        }

        private string line_ = null;
        private int lineNum_ = 0;
        private bool goToNext_ = true;
    }
}