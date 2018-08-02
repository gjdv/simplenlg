/*
 * Ported to C# by Gert-Jan de Vries
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SimpleNLG.Main.framework;

namespace SimpleNLG.Main
{
    public static class ICollectionExtensions
    {
        public static String ToStringNLG<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("{");
            stringBuilder.Append(string.Join(", ", dictionary.Keys.OrderBy(x => x).Select(
                                                     key => key + "=" +
                                                        (dictionary[key] is IList<NLGElement> ? 
                                                            ((IList<NLGElement>)dictionary[key]).ToStringNLG() :
                                                            (dictionary[key] is IDictionary<string, object> ? 
                                                                ((IDictionary<string, object>)dictionary[key]).ToStringNLG() : 
                                                                dictionary[key].ToString()
                                                            )
                                                        )
                                                    )
                                            ));
            stringBuilder.Append("}");
            return stringBuilder.ToString();
        }

        public static String ToStringNLG<T>(this IList<T> list)

        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("[");
            stringBuilder.Append(string.Join(", ", list.Select(x =>
                (x is IList<NLGElement>
                    ? ((IList<NLGElement>) x).ToStringNLG()
                    : (x is IDictionary<string, object>
                        ? ((IDictionary<string, object>) x).ToStringNLG()
                        : x.ToString())
                )
            )));
            stringBuilder.Append("]");
            return stringBuilder.ToString();
        }
    }

    public static class URIExtensions
    {
        public static Uri ToAbsolute(this Uri fileUri)
        {
            if (fileUri.IsAbsoluteUri)
            {
                return fileUri;
            }
            else
            {
                var baseUri = new Uri(Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar);

                return new Uri(baseUri, fileUri);
            }
        }
    }

    public class EqualElmDictionary<TKey, TValue> : Dictionary<TKey, TValue>
    {
        public override bool Equals (object obj2)
        {
            if (!(obj2 is IDictionary)) return false;
            IDictionary<TKey, TValue> dict2 = (IDictionary<TKey, TValue>) obj2;
            if (this == dict2) return true;
            if (Count != dict2.Count) return false;

            EqualityComparer<TValue> valueComparer = EqualityComparer<TValue>.Default;

            foreach (KeyValuePair<TKey, TValue> kvp in this)
            {
                TValue value2;
                if (!dict2.TryGetValue(kvp.Key, out value2)) return false;
                if (!valueComparer.Equals(kvp.Value, value2)) return false;
            }

            return true;
        }

    }

    public class EqualElmList<T> : List<T>
    { 

        public override bool Equals(object obj2)
        {
            if (!(obj2 is IList)) return false;
            IList<T> list2 = (IList<T>) obj2;
            if (this == list2) return true;
            if (Count != list2.Count) return false;

            EqualityComparer<T> valueComparer = EqualityComparer<T>.Default;
            for (int i = 0; i < Count; i++)
            {
                if (!valueComparer.Equals(this[i], list2[i])) return false;
            }

            return true;
        }
    }

    public static class StringExtensions
    {

        public static sbyte[] GetBytes(this string self, Encoding encoding)
        {
            //get the byte array
            byte[] bytes = encoding.GetBytes(self);
            //convert it to sbyte array
            sbyte[] sbytes = new sbyte[bytes.Length];
            for (int i = 0; i < bytes.Length; i++)
                sbytes[i] = (sbyte)bytes[i];
            return sbytes;
        }

        public static string SubstringSpecial(this string self, int start, int end)
        {
            return self.Substring(start, end - start);
        }

    }

}
