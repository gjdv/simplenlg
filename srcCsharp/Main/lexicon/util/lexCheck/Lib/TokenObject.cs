namespace SimpleNLG.Main.lexicon.util.lexCheck.Lib
{
    public class TokenObject
    {
        //public virtual StringTokenizer GetBuffer()
        //{
        //    return this.buf_;
        //}

        public virtual string GetToken()
        {
            return token_;
        }

        //public virtual void SetBuffer(StringTokenizer buf)
        //{
        //    this.buf_ = buf;
        //}

        public virtual void SetToken(string token)
        {
            token_ = token;
        }

        //private StringTokenizer buf_ = null;
        private string token_ = null;
    }
}