namespace SimpleNLG.Main.lexicon.util.lexCheck.Lib
{
    public class CatEntry
    {
        public CatEntry(string cat)
        {
            if (cat.Equals("verb"))
            {
                verbEntry_ = new VerbEntry();
            }
            else if (cat.Equals("noun"))
            {
                nounEntry_ = new NounEntry();
            }
            else if (cat.Equals("adj"))
            {
                adjEntry_ = new AdjEntry();
            }
            else if (cat.Equals("adv"))
            {
                advEntry_ = new AdvEntry();
            }
            else if (cat.Equals("aux"))
            {
                auxEntry_ = new AuxEntry();
            }
            else if (cat.Equals("modal"))
            {
                modalEntry_ = new ModalEntry();
            }
            else if (cat.Equals("pron"))
            {
                pronEntry_ = new PronEntry();
            }
            else if (cat.Equals("det"))
            {
                detEntry_ = new DetEntry();
            }
        }

        public virtual string GetText(string cat)
        {
            string text = "";
            if (cat.Equals("verb"))
            {
                text = verbEntry_.GetText();
            }
            else if (cat.Equals("noun"))
            {
                text = nounEntry_.GetText();
            }
            else if (cat.Equals("adj"))
            {
                text = adjEntry_.GetText();
            }
            else if (cat.Equals("adv"))
            {
                text = advEntry_.GetText();
            }
            else if (cat.Equals("aux"))
            {
                text = auxEntry_.GetText();
            }
            else if (cat.Equals("modal"))
            {
                text = modalEntry_.GetText();
            }
            else if (cat.Equals("pron"))
            {
                text = pronEntry_.GetText();
            }
            else if (cat.Equals("det"))
            {
                text = detEntry_.GetText();
            }

            return text;
        }

        public virtual string GetXml(string cat)
        {
            string xml = "";
            if (cat.Equals("verb"))
            {
                xml = verbEntry_.GetXml();
            }
            else if (cat.Equals("noun"))
            {
                xml = nounEntry_.GetXml();
            }
            else if (cat.Equals("adj"))
            {
                xml = adjEntry_.GetXml();
            }
            else if (cat.Equals("adv"))
            {
                xml = advEntry_.GetXml();
            }
            else if (cat.Equals("aux"))
            {
                xml = auxEntry_.GetXml();
            }
            else if (cat.Equals("modal"))
            {
                xml = modalEntry_.GetXml();
            }
            else if (cat.Equals("pron"))
            {
                xml = pronEntry_.GetXml();
            }
            else if (cat.Equals("det"))
            {
                xml = detEntry_.GetXml();
            }

            return xml;
        }

        public virtual VerbEntry GetVerbEntry()
        {
            return verbEntry_;
        }

        public virtual NounEntry GetNounEntry()
        {
            return nounEntry_;
        }

        public virtual AdjEntry GetAdjEntry()
        {
            return adjEntry_;
        }

        public virtual AdvEntry GetAdvEntry()
        {
            return advEntry_;
        }

        public virtual AuxEntry GetAuxEntry()
        {
            return auxEntry_;
        }

        public virtual ModalEntry GetModalEntry()
        {
            return modalEntry_;
        }

        public virtual PronEntry GetPronEntry()
        {
            return pronEntry_;
        }

        public virtual DetEntry GetDetEntry()
        {
            return detEntry_;
        }

        private VerbEntry verbEntry_ = null;
        private NounEntry nounEntry_ = null;
        private AdjEntry adjEntry_ = null;
        private AdvEntry advEntry_ = null;
        private AuxEntry auxEntry_ = null;
        private ModalEntry modalEntry_ = null;
        private PronEntry pronEntry_ = null;
        private DetEntry detEntry_ = null;
    }
}