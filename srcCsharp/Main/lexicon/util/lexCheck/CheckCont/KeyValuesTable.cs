using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleNLG.Main.lexicon.util.lexCheck.CheckCont
{
    public class KeyValuesTable

    {
        public static List<string> GetValues(string key, Dictionary<string, HashSet<string>> keyValuesTable)

        {
            HashSet<string> values = keyValuesTable[key];
            if (values == null)

            {
                values = new HashSet<string>();
            }

            return new List<string>(values);
        }


        public static List<string> GetSortedValues(string key, Dictionary<string, HashSet<string>> keyValuesTable)

        {
            List<string> sortedValues = GetValues(key, keyValuesTable);
            sortedValues.Sort();
            return sortedValues;
        }


        public static Dictionary<string, HashSet<string>> GetKey2ValuesTable(string inFile, bool verbose, int lineNum)

        {
            string line = null;
            int lineNo = 0;
            int keyNo = 0;
            int valueNo = 0;
            Dictionary<string, HashSet<string>> keyValues = new Dictionary<string, HashSet<string>>();

            try

            {
                System.IO.StreamReader @in = new System.IO.StreamReader(
                    new System.IO.FileStream(inFile, System.IO.FileMode.Open, System.IO.FileAccess.Read),
                    Encoding.UTF8);


                while (!ReferenceEquals((line = @in.ReadLine()), null))

                {
                    lineNo++;

                    if ((line.Length > 0) && (line[0] != '#'))

                    {
                        string[] buf = line.Split('|').ToList().Where(x => x != "").ToArray();
                        string key1 = buf[0];
                        string key2 = buf[1];
                        string key = key1 + "|" + key2;
                        for (int i = 2; i < buf.Length; i++)
                        {
                            
                            string value = buf[i];


                            if (!keyValues.ContainsKey(key))

                            {
                                HashSet<string> values = new HashSet<string>();
                                values.Add(value);
                                keyValues[key] = values;
                                keyNo++;
                                valueNo++;
                            }
                            else

                            {
                                keyValues[key].Add(value);
                                valueNo++;
                            }
                        }

                        if ((verbose == true) && (lineNo % lineNum == 0))

                        {
                            Console.WriteLine("-- current line: " + lineNo);
                        }
                    }
                }

                if (verbose == true)

                {
                    Console.WriteLine("-----------------------------");
                    Console.WriteLine("--- Total line: " + lineNo);
                    Console.WriteLine("--- Total key: " + keyNo);
                    Console.WriteLine("--- Total value: " + valueNo);
                }

                @in.Close();
            }
            catch (Exception e)

            {
                Console.Error.WriteLine("** ERR@KeyValuesTable.GetKeyValuesTable (" + lineNo + "): " + line);

                Console.Error.WriteLine("Exception: " + e.ToString());
            }

            return keyValues;
        }
    }


}