using System.Collections.Generic;
using SimpleNLG.Main.lexicon.util.lexCheck.Lib;

namespace SimpleNLG.Main.lexicon.util.lexCheck.CheckCont
{
    using LexRecord = LexRecord;


    public class LexRecordNomObj

    {
        public LexRecordNomObj(LexRecord lexRecord)

        {
            if (lexRecord != null)

            {
                base_ = lexRecord.GetBase();
                eui_ = lexRecord.GetEui();
                category_ = lexRecord.GetCategory();
                nominalizations_ = lexRecord.GetNominalizations();
            }
        }

        public LexRecordNomObj(string @base, string eui, string category, List<string> nominalizations)

        {
            base_ = @base;
            eui_ = eui;
            category_ = category;
            nominalizations_ = nominalizations;
        }

        public virtual string GetBase()

        {
            return base_;
        }

        public virtual string GetEui()
        {
            return eui_;
        }

        public virtual string GetCategory()
        {
            return category_;
        }

        public virtual List<string> GetNominalizations()
        {
            return nominalizations_;
        }

        private string base_ = null;
        private string eui_ = null;
        private string category_ = null;
        private List<string> nominalizations_ = new List<string>();
    }


}