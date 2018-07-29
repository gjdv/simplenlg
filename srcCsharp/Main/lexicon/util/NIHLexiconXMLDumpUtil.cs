/*
 * The contents of this file are subject to the Mozilla Public License
 * Version 1.1 (the "License"); you may not use this file except in
 * compliance with the License. You may obtain a copy of the License at
 * http://www.mozilla.org/MPL/
 *
 * Software distributed under the License is distributed on an "AS IS"
 * basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
 * License for the specific language governing rights and limitations
 * under the License.
 *
 * The Original Code is "Simplenlg".
 *
 * The Initial Developer of the Original Code is Ehud Reiter, Albert Gatt and Dave Westwater.
 * Portions created by Ehud Reiter, Albert Gatt and Dave Westwater are Copyright (C) 2010-11 The University of Aberdeen. All Rights Reserved.
 *
 * Contributor(s): Ehud Reiter, Albert Gatt, Dave Westwater, Roman Kutlak, Margaret Mitchell, and Saad Mahamood.
 *
 * Ported to C# by Gert-Jan de Vries
 */

using System;
using SimpleNLG.Main.framework;

namespace SimpleNLG.Main.lexicon.util
{

	using LexicalCategory = LexicalCategory;
	using WordElement = WordElement;



    /**
     * <p>This class reads in a CSV word list, looks up the words in the NIH lexicon, 
     * and writes the XML words into an output file. This XML file can then be used as the XML Lexicon source for SimpleNLG.</p>
     * 
     * @author Ehud Reiter
     */
	public class NIHLexiconXMLDumpUtil
	{
	    // filenames

		private static string DB_FILENAME; // DB location
		private static string WORDLIST_FILENAME; // word list
		private static string XML_FILENAME; // word list

	    /**
	     * This main method reads a list of CSV words and POS tags and looks up against 
	     * the NIHDB Lexicon for a corresponding entry. If found the baseform is written out into a XML 
	     * file, which can be used in SimpleNLG or elsewhere. 
	     * 
	     * @param args : List of Arguments that this command line application must be provided with in order:
	     * <ol>
	     * 		<li>The full path to the NIHDB Lexicon database file e.g. C:\\NIHDB\\lexAccess2009</li>
	     * 		<li>The full path to the list of baseforms and POS tags to include in the written out XML Lexicon file</li>
	     * 		<li>The full path to the XML file that the XML Lexicon will be written out to.</li>
	     * </ol>
	     * 
	     *<p>Example usage: 
	     *   java simplenlg.lexicon.util.NIHLexiconXMLDumpUtil C:\\NIHDB\\lexAccess2009 C:\\NIHDB\\wordlist.csv C:\\NIHDB\\default-lexicon.xml
	     *   
	     *   You will need to have the HSQLDB driver (org.hsqldb.jdbc.JDBCDriver) on your Java classpath before running this application.
	     *</p>
	     */
		public static void Main(string[] args)
		{
			Lexicon lex = null;

			if (args.Length == 3)
			{

				DB_FILENAME = args[0];
				WORDLIST_FILENAME = args[1];
				XML_FILENAME = args[2];

        	    // Check to see if the HSQLDB driver is available on the classpath:
				bool dbDriverAvaliable = false;
				try
				{
//					Type driverClass = Type.GetType("org.hsqldb.jdbc.JDBCDriver", false, typeof(NIHLexiconXMLDumpUtil).ClassLoader); //OUTCOMMENTED UNTIL SOLUTION FOUND FOR HSQLDB CONNECTION
				    Type driverClass = null;

                    if (null != driverClass)
					{
						dbDriverAvaliable = true;
					}
				}
				catch (Exception)
				{
					Console.Error.WriteLine("*** Please add the HSQLDB JDBCDriver to your Java classpath and try again.");
				}

				if ((null != DB_FILENAME && DB_FILENAME.Length > 0) && (null != WORDLIST_FILENAME && WORDLIST_FILENAME.Length > 0) && (null != XML_FILENAME && XML_FILENAME.Length > 0) && dbDriverAvaliable)
				{
					lex = new NIHDBLexicon(DB_FILENAME);

					try
					{
						LineNumberReader wordListFile = new LineNumberReader(new System.IO.StreamReader(WORDLIST_FILENAME));
						System.IO.StreamWriter xmlFile = new System.IO.StreamWriter(XML_FILENAME);
						xmlFile.BaseStream.WriteByte(Convert.ToByte(string.Format("<lexicon>%n")));
						string line = wordListFile.ReadLine();
						while (!ReferenceEquals(line, null))
						{
							string[] cols = line.Split(',');
							string @base = cols[0];
							string cat = cols[1];
							WordElement word = null;
							if (cat.Equals("noun", StringComparison.OrdinalIgnoreCase))
							{
								word = lex.getWord(@base, new LexicalCategory(LexicalCategory.LexicalCategoryEnum.NOUN));
							}
							else if (cat.Equals("verb", StringComparison.OrdinalIgnoreCase))
							{
								word = lex.getWord(@base, new LexicalCategory(LexicalCategory.LexicalCategoryEnum.VERB));
							}
							else if (cat.Equals("adv", StringComparison.OrdinalIgnoreCase))
							{
								word = lex.getWord(@base, new LexicalCategory(LexicalCategory.LexicalCategoryEnum.ADVERB));
							}
							else if (cat.Equals("adj", StringComparison.OrdinalIgnoreCase))
							{
								word = lex.getWord(@base, new LexicalCategory(LexicalCategory.LexicalCategoryEnum.ADJECTIVE));
							}
							else if (cat.Equals("det", StringComparison.OrdinalIgnoreCase))
							{
								word = lex.getWord(@base, new LexicalCategory(LexicalCategory.LexicalCategoryEnum.DETERMINER));
							}
							else if (cat.Equals("prep", StringComparison.OrdinalIgnoreCase))
							{
								word = lex.getWord(@base, new LexicalCategory(LexicalCategory.LexicalCategoryEnum.PREPOSITION));
							}
							else if (cat.Equals("pron", StringComparison.OrdinalIgnoreCase))
							{
								word = lex.getWord(@base, new LexicalCategory(LexicalCategory.LexicalCategoryEnum.PRONOUN));
							}
							else if (cat.Equals("conj", StringComparison.OrdinalIgnoreCase))
							{
								word = lex.getWord(@base, new LexicalCategory(LexicalCategory.LexicalCategoryEnum.CONJUNCTION));
							}
							else if (cat.Equals("modal", StringComparison.OrdinalIgnoreCase))
							{
								word = lex.getWord(@base, new LexicalCategory(LexicalCategory.LexicalCategoryEnum.MODAL));
							}
							else if (cat.Equals("interjection", StringComparison.OrdinalIgnoreCase))
							{
								word = lex.getWord(@base, new LexicalCategory(LexicalCategory.LexicalCategoryEnum.NOUN)); // Kilgarriff;s interjections are mostly nouns in the lexicon
							}

							if (word == null)
							{
								Console.WriteLine("*** The following baseform and POS tag is not found: " + @base + ":" + cat);
							}
							else
							{
								xmlFile.BaseStream.WriteByte(Convert.ToByte(word.toXML()));
							}
							line = wordListFile.ReadLine();
						}
						xmlFile.BaseStream.WriteByte(Convert.ToByte(string.Format("</lexicon>%n")));
						wordListFile.Close();
						xmlFile.Close();

						lex.close();

						Console.WriteLine("*** XML Lexicon Export Completed.");

					}
					catch (Exception e)
					{
						Console.Error.WriteLine("*** An Error occured during the export. The Exception message is below: ");
						Console.Error.WriteLine(e.Message);
						Console.Error.WriteLine("************************");
						Console.Error.WriteLine("Please make sure you have the correct application arguments: ");
						printArgumentsMessage();
					}
				}
				else
				{
					printErrorArgumentMessage();
				}
			}
			else
			{
				printErrorArgumentMessage();
			}
		}

	    /**
	     * Prints Arguments Error Messages if incorrect or not enough parameters have been supplied. 
	     */
		private static void printErrorArgumentMessage()
		{
			Console.Error.WriteLine("Insuffient number of arguments supplied. Please supply the following Arguments: \n");
			printArgumentsMessage();
		}

	    /**
	     * Prints this utility applications arguments requirements. 
	     */
		private static void printArgumentsMessage()
		{
			Console.Error.WriteLine("\t\t 1. The full path to the NIHDB Lexicon database file e.g. C:\\NIHDB\\lexAccess2009 ");
			Console.Error.WriteLine("\t\t 2. The full path to the list of baseforms and POS tags to include in the written out XML Lexicon file");
			Console.Error.WriteLine("\t\t 3. The full path to the XML file that the XML Lexicon will be written out to.");
		}

	}

}