using System.Collections.Generic;

namespace SimpleNLG.Main.lexicon.util.lexCheck.Lib
{
    public class InflVarComparator<T> : Comparer<T>
    {
        public override int Compare(T o1, T o2)

        {
            if (o1 != null && o2 != null)
            {
                InflVar var1 = o1 as InflVar;
                InflVar var2 = o2 as InflVar;

                int @out = var1.GetEui().CompareTo(var2.GetEui());

                if (@out == 0)

                {
                    @out = var1.GetInflection().Length - var2.GetInflection().Length;
                    if (@out == 0)

                    {
                        @out = var1.GetInflection().CompareTo(var2.GetInflection());
                    }
                }

                if (@out == 0)

                {
                    @out = var1.GetVar().CompareTo(var2.GetVar());
                }

                return @out;
            }

            return -1;
        }
    }
}