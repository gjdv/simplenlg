/*
 * Ported to C# by Gert-Jan de Vries
 */
 
using System;
using System.IO;

namespace SimpleNLG.Main.server
{
    /**
     * Class to read a properties file (e.g., lexicon.properties)
     * and retrieve its values given keys
     */
    public class Properties
    {
        private EqualElmDictionary<string, string> _properties;

        public Properties()
        {
            _properties = new EqualElmDictionary<string, string>();
        }

        /**
         * Get a property given its name
         * @param name The property name
         * @return The value, or null if not found
         */
        public string getProperty(string name)
        {
            if (_properties.TryGetValue(name, out string value))
            {
                return value;
            }
            return null;
        }

        /**
         * Load a property file given its file name
         * @param filename The filename
         */
        public void load(string filename)
        {
            try
            {
                StreamReader file = new StreamReader(filename);
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    line = line.Trim();
                    if (line.StartsWith("#") || string.IsNullOrEmpty(line))
                    { // comment or empty line
                        continue;
                    }

                    string[] parts = line.Split('=');
                    if (parts.Length != 2)
                    {
                        throw new Exception("Invalid line format encountered: " + line);
                    }

                    _properties.Add(parts[0], parts[1]);
                }

                file.Close();
            }
            catch (Exception e)
            {
                throw new Exception("Could not read properties file: " + filename + "\n" + e.Message);
            }
        }
    }
}