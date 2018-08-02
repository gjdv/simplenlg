namespace SimpleNLG.Main.lexicon.util.lexCheck.Lib
{
    public class InflVar

    {
        public InflVar(string eui, string unInfl, string infl, string var, string cat, string cit)

        {
            eui_ = eui;
            unInfl_ = unInfl;
            type_ = "basic";
            infl_ = infl;
            var_ = var;
            cat_ = cat;
            cit_ = cit;
        }


        public InflVar(string eui, string unInfl, string type, string infl, string var, string cat, string cit)

        {
            eui_ = eui;
            unInfl_ = unInfl;
            type_ = type;
            infl_ = infl;
            var_ = var;
            cat_ = cat;
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


        public virtual string GetUnInfl()

        {
            return unInfl_;
        }


        public virtual string GetType()

        {
            return type_;
        }


        public virtual string GetInflection()

        {
            return infl_;
        }


        public virtual string GetVar()

        {
            return var_;
        }


        public virtual bool GetUnique()

        {
            return unique_;
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


        public virtual void SetUnInfl(string unInfl)

        {
            unInfl_ = unInfl;
        }


        public virtual void SetType(string type)

        {
            type_ = type;
        }


        public virtual void SetInflection(string infl)

        {
            infl_ = infl;
        }


        public virtual void SetVar(string var)

        {
            var_ = var;
        }


        public virtual void SetUnique(bool unique)

        {
            unique_ = unique;
        }

        private string var_ = null;
        private string cat_ = null;
        private string infl_ = null;
        private string eui_ = null;
        private string unInfl_ = null;
        private string cit_ = null;
        private string type_ = null;
        private bool unique_ = true;
    }
}