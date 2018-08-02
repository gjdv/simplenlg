namespace SimpleNLG.Main.lexicon.util.lexCheck.Api
{
    public class ApiOutput

    {
        public ApiOutput(string xml, int recordNum)

        {
            xml_ = xml;
            recordNum_ = recordNum;
        }


        public virtual void SetXml(string xml)

        {
            xml_ = xml;
        }


        public virtual void SetRecordNum(int recordNum)

        {
            recordNum_ = recordNum;
        }


        public virtual string GetXml()

        {
            return xml_;
        }


        public virtual int GetRecordNum()

        {
            return recordNum_;
        }

        private string xml_ = null;
        private int recordNum_ = 0;
    }


}