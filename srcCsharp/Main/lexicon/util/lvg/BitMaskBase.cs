using System.Collections.Generic;
using System.Linq;

namespace SimpleNLG.Main.lexicon.util.lvg
{
    public class BitMaskBase
    {
        protected internal const int MAX_BIT = 63;
        private const string SP = "+";
        private const string ALL_STR = "all";

        public BitMaskBase()
        {
            value_ = 0L;
        }

        public BitMaskBase(long value)
        {
            value_ = value;
        }

        protected internal BitMaskBase(long allBitValue, List<List<string>> bitStr)
        {
            value_ = 0L;
            allBitValue_ = allBitValue;
            bitStr_ = bitStr;
        }

        protected internal BitMaskBase(long value, long allBitValue, List<List<string>> bitStr)
        {
            value_ = value;
            allBitValue_ = allBitValue;
            bitStr_ = bitStr;
        }

        public static long GetBitValue(int bitNum)
        {
            return MASK[bitNum];
        }

        public static int GetBitIndex(long value)
        {
            int bit = -1;
            for (int i = 0; i < 63; i++)
            {
                if (value == MASK[i])
                {
                    bit = i;
                    break;
                }
            }

            return bit;
        }

        public static bool Contains(long container, long value)
        {
            bool contain = (container & value) == value;
            return contain;
        }

        protected internal static List<long> ToValues(long value, int maxBitUsed)
        {
            if ((value <= 0L) && (value > MASK[maxBitUsed]))
            {
                return null;
            }

            List<long> outs = new List<long>();
            for (int i = 0; i < maxBitUsed; i++)
            {
                if ((value & MASK[i]) != 0L)
                {
                    outs.Add(MASK[i]);
                }
            }

            return outs;
        }

        public virtual void SetValue(long value)
        {
            if (value > 0L)
            {
                value_ = value;
            }
        }

        public virtual bool GetBitFlag(int maskIndex)
        {
            bool bitValue = (value_ & MASK[maskIndex]) != 0L;
            return bitValue;
        }

        public virtual void SetBitFlag(int maskIndex, bool flag)
        {
            if (flag == true)
            {
                value_ |= MASK[maskIndex];
            }
            else
            {
                value_ &= (MASK[maskIndex] ^ (long) 0xFFFFFFFFFFFFFFF);//0xFFFFFFFFFFFFFFFF
            }
        }

        public virtual long GetValue()
        {
            return value_;
        }

        public virtual string GetName()
        {
            return ToName(value_, allBitValue_, bitStr_);
        }

        public virtual bool Contains(long value)
        {
            if ((value_ & value) == value)
            {
                return true;
            }

            return false;
        }

        protected internal static long ToValue(string valueStr, List<List<string>> bitStr)
        {
            long value = 0L;
            List<string> valueStrList = GetStringList(valueStr);
            BitMaskBase bm = new BitMaskBase();
            for (int k = 0; k < valueStrList.Count; k++)
            {
                for (int i = 0; i < 63; i++)
                {
                    for (int j = 0; j < ( bitStr[i]).Count; j++)
                    {
                        string temp = (string) valueStrList[k];
                        if (temp.Equals(( bitStr[i])[j]) == true)
                        {
                            bm.SetBitFlag(i, true);
                        }
                    }
                }
            }

            value = bm.GetValue();
            return value;
        }

        protected internal static string ToName(long value, long allBitValue, List<List<string>> bitStr)
        {
            if (value == allBitValue)
            {
                return "all";
            }

            string returnBitStr = null;
            for (int i = 0; i < 63; i++)
            {
                if ((( bitStr[i]).Count >= 1) && ((value & MASK[i]) != 0L))
                {
                    if (ReferenceEquals(returnBitStr, null))
                    {
                        returnBitStr = (string) ( bitStr[i])[0] + "+";
                    }
                    else
                    {
                        returnBitStr = returnBitStr + (string) ( bitStr[i])[0] + "+";
                    }
                }
            }

            if (!ReferenceEquals(returnBitStr, null))
            {
                returnBitStr = returnBitStr.Substring(0, returnBitStr.Length - 1);
            }
            else
            {
                returnBitStr = "";
            }

            return returnBitStr;
        }

        protected internal static long Enumerate(string valueStr, List<List<string>> bitStr)
        {
            long value = 0L;
            for (int i = 0; i < bitStr.Count; i++)
            {
                for (int j = 0; j < ( bitStr[i]).Count; j++)
                {
                    if (valueStr.Equals(( bitStr[i])[j]))
                    {
                        value = MASK[i];
                        break;
                    }
                }
            }

            return value;
        }

        protected internal static string GetBitName(int bitValue, int index, List<List<string>> bitStr)
        {
            string returnBitStr = null;
            if (bitValue >= 63)
            {
                return returnBitStr;
            }

            if (index < ( bitStr[bitValue]).Count)
            {
                returnBitStr = (string) ( bitStr[bitValue])[index];
            }

            return returnBitStr;
        }

        //public static void Main(string[] args)
        //{
        //    Console.WriteLine("----- static methods -------");
        //    Console.WriteLine(" -  Gender.ToName(7): " + Gender.ToName(7L));
        //    Console.WriteLine(" -  Gender.ToName(15): " + Gender.ToName(15L));
        //    Console.WriteLine(" -  " + Gender.ToName(6L) + " contains " + Gender.ToName(2L) + ": " +
        //                      Gender.Contains(6L, 2L));
        //    Console.WriteLine(" -  " + Gender.ToName(6L) + " contains " + Gender.ToName(4L) + ": " +
        //                      Gender.Contains(6L, 4L));
        //    Console.WriteLine(" -  " + Gender.ToName(6L) + " contains " + Gender.ToName(5L) + ": " +
        //                      Gender.Contains(6L, 5L));
        //    Console.WriteLine(" -  Category.ToName(1030): " + Category.ToName(1030L));
        //    List<long> values = Category.ToValues(1030L);
        //    for (int i = 0; i < values.Count; i++)
        //    {
        //        Console.WriteLine(" - Category.ToValues(1030): " + values[i]);
        //    }

        //    Console.WriteLine(" -  Category.ToName(2047): " + Category.ToName(2047L));
        //    Console.WriteLine(" -  Inflection.ToName(2584): " + Inflection.ToName(2584L));
        //    Console.WriteLine(" -  Inflection.ToName(16777215): " + Inflection.ToName(16777215L));
        //    for (int i = 0; i < 63; i++)
        //    {
        //        Console.WriteLine(i + ": " + MASK[i]);
        //    }
        //}

        private static List<string> GetStringList(string value)
        {
            List<string> stringList = new List<string>();
            return value.Split('+').ToList();
        }

        public static readonly long[] MASK = new long[]
        {
            1L, 2L, 4L, 8L, 16L, 32L, 64L, 128L, 256L, 512L, 1024L, 2048L, 4096L, 8192L, 16384L, 32768L, 65536L,
            131072L, 262144L, 524288L, 1048576L, 2097152L, 4194304L, 8388608L, 16777216L, 33554432L, 67108864L,
            134217728L, 268435456L, 536870912L, 1073741824L, 2147483648L, 4294967296L, 8589934592L, 17179869184L,
            34359738368L, 68719476736L, 137438953472L, 274877906944L, 549755813888L, 1099511627776L, 2199023255552L,
            4398046511104L, 8796093022208L, 17592186044416L, 35184372088832L, 70368744177664L, 140737488355328L,
            281474976710656L, 562949953421312L, 1125899906842624L, 2251799813685248L, 4503599627370496L,
            9007199254740992L, 18014398509481984L, 36028797018963968L, 72057594037927936L, 144115188075855872L,
            288230376151711744L, 576460752303423488L, 1152921504606846976L, 2305843009213693952L, 4611686018427387904L
        };

        private long value_;
        private long allBitValue_ = -1L;
        private List<List<string>> bitStr_ = new List<List<string>>(63);
    }

}