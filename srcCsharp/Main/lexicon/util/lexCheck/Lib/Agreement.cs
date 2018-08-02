namespace SimpleNLG.Main.lexicon.util.lexCheck.Lib
{
    public class Agreement

    {
        public Agreement(string eui, string var, string cat, string agr, string @base, string cit)

        {
            eui_ = eui;
            var_ = var;
            cat_ = cat;
            agr_ = agr;
            base_ = @base;
            cit_ = cit;
        }


        public virtual string GetCat()

        {
            return cat_;
        }


        public virtual string GetCit()

        {
            return cit_;
        }


        public virtual string GetEui()

        {
            return eui_;
        }


        public virtual string GetBase()

        {
            return base_;
        }


        public virtual string GetAgreement()

        {
            return agr_;
        }


        public virtual string GetVar()

        {
            return var_;
        }


        public virtual void SetCat(string cat)

        {
            cat_ = cat;
        }


        public virtual void SetCit(string cit)

        {
            cit_ = cit;
        }


        public virtual void SetEui(string eui)

        {
            eui_ = eui;
        }


        public virtual void SetBase(string @base)

        {
            base_ = @base;
        }


        public virtual void SetAgreement(string agr)

        {
            agr_ = agr;
        }


        public virtual void SetVar(string var)

        {
            var_ = var;
        }

        private string var_ = null;
        private string cat_ = null;
        private string agr_ = null;
        private string eui_ = null;
        private string base_ = null;
        private string cit_ = null;
    }
}