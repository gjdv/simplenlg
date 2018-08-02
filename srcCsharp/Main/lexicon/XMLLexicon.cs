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
 * Contributor(s): Ehud Reiter, Albert Gatt, Dave Wewstwater, Roman Kutlak, Margaret Mitchell, Saad Mahamood.
 *
 * Ported to C# by Gert-Jan de Vries
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using SimpleNLG.Main.features;

namespace SimpleNLG.Main.lexicon
{

	using Inflection = Inflection;
	using LexicalFeature = LexicalFeature;
	using ElementCategory = framework.ElementCategory;
	using LexicalCategory = framework.LexicalCategory;
	using WordElement = framework.WordElement;

    /**
     * This class loads words from an XML lexicon. All features specified in the
     * lexicon are loaded
     * 
     * @author ereiter
     * 
     */
	public class XMLLexicon : Lexicon
	{

	    // node names in lexicon XML files
		private const string XML_BASE = "base"; // base form of Word
		private const string XML_CATEGORY = "category"; // base form of Word
		private const string XML_ID = "id"; // base form of Word
		private const string XML_WORD = "word"; // node defining a word

	    // lexicon
		private ISet<WordElement> words; // set of words
		private IDictionary<string, WordElement> indexByID; // map from ID to word
		private IDictionary<string, IList<WordElement>> indexByBase; // map from base to set
	    // of words with this baseform
		private IDictionary<string, IList<WordElement>> indexByVariant; // map from variants

	    // to set of words with this variant
            
	    /**********************************************************************/
	    // constructors
	    /**********************************************************************/

	    /**
	     * Load an XML Lexicon from a named file
	     * 
	     * @param filename
	     */
		public XMLLexicon(string filename) : base()
		{
			createLexicon(new Uri(filename, UriKind.RelativeOrAbsolute));
		}

	    /**
	     * Load an XML Lexicon from a URI
	     * 
	     * @param lexiconURI
	     */

		public XMLLexicon(Uri lexiconURI) : base()
		{
			createLexicon(lexiconURI);
		}

		public XMLLexicon()
		{
		    string baseDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
            Uri lexiconUri = new Uri(baseDir + Path.DirectorySeparatorChar + "Resources/default-lexicon.xml", UriKind.Absolute);
		    if (!File.Exists(lexiconUri.ToAbsolute().AbsolutePath))
		    {
		        throw new FileNotFoundException("Lexicon file does not exist: " + lexiconUri.ToAbsolute().AbsolutePath);
		    }

		    if (!createLexicon(lexiconUri))
		    {
		        throw new Exception("Could not succesfully create lexicon");
		    }
        }

	    /**
	     * method to actually load and index the lexicon from a URI
	     * 
	     * @param uri
	     * #returns bool representing success
	     */
		private bool createLexicon(Uri lexiconURI)
		{
		    // initialise objects
			words = new HashSet<WordElement>();
			indexByID = new Dictionary<string, WordElement>();
			indexByBase = new Dictionary<string, IList<WordElement>>();
			indexByVariant = new Dictionary<string, IList<WordElement>>();
		    bool success = false;

			try
			{
				XmlDocument doc = new XmlDocument();
			    doc.Load(lexiconURI.ToString());

				if (doc != null)
				{
					XmlElement lexRoot = doc.DocumentElement;
					XmlNodeList wordNodes = lexRoot.ChildNodes;
					for (int i = 0; i < wordNodes.Count; i++)
					{
						XmlNode wordNode = wordNodes.Item(i);
					    // ignore things that aren't elements
						if (wordNode.NodeType == XmlNodeType.Element)
						{
							WordElement word = convertNodeToWord(wordNode);
							if (word != null)
							{
								words.Add(word);
								IndexWord(word);
							}
						}
					}

				    success = true;

				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}

			addSpecialCases();
		    return success;
		}

	    /**
	     * add special cases to lexicon
	     * 
	     */
		private void addSpecialCases()
		{
		    // add variants of "be"
			WordElement be = getWord("be", new LexicalCategory(LexicalCategory.LexicalCategoryEnum.VERB));
			if (be != null)
			{
				updateIndex(be, "is", indexByVariant);
				updateIndex(be, "am", indexByVariant);
				updateIndex(be, "are", indexByVariant);
				updateIndex(be, "was", indexByVariant);
				updateIndex(be, "were", indexByVariant);
			}
		}

	    /**
	     * create a simplenlg WordElement from a Word node in a lexicon XML file
	     * 
	     * @param wordNode
	     * @return
	     * @throws XPathUtilException
	     */
		private WordElement convertNodeToWord(XmlNode wordNode)
		{
		    // if this isn't a Word node, ignore it
			if (!wordNode.Name.Equals(XML_WORD,StringComparison.CurrentCultureIgnoreCase))
			{
				return null;
			}

		    // if there is no base, flag an error and return null
		    // String base = XPathUtil.extractValue(wordNode, Constants.XML_BASE);
		    // if (base == null) {
		    // System.out.println("Error in loading XML lexicon: Word with no base");
		    // return null;
		    // }

		    // create word
			WordElement word = new WordElement();
			IList<Inflection> inflections = new List<Inflection>();

		    // now copy features
			XmlNodeList nodes = wordNode.ChildNodes;
			for (int i = 0; i < nodes.Count; i++)
			{
			    XmlNode featureNode = nodes.Item(i);

				if (featureNode.NodeType == XmlNodeType.Element)
				{
					string feature = featureNode.Name.Trim();
					string value = featureNode.InnerText;

					if (!ReferenceEquals(value, null))
					{
						value = value.Trim();
					}

					if (ReferenceEquals(feature, null))
					{
						Console.Error.WriteLine("Error in XML lexicon node for " + word.ToString());
						break;
					}

					if (feature.Equals(XML_BASE, StringComparison.OrdinalIgnoreCase))
					{
						word.BaseForm = value;
					}
					else if (feature.Equals(XML_CATEGORY, StringComparison.OrdinalIgnoreCase))
					{
					    Enum.TryParse(value.ToUpper(), out LexicalCategory.LexicalCategoryEnum lexcat);
                        word.Category = new LexicalCategory(lexcat);
					}
					else if (feature.Equals(XML_ID, StringComparison.OrdinalIgnoreCase))
					{
						word.Id = value;
					}

					else if (ReferenceEquals(value, null) || value.Equals(""))
					{
					    // if this is an infl code, add it to inflections
						Inflection? infl = Inflection.REGULAR.getInflCode(feature);

						if (infl != null)
						{
							inflections.Add((Inflection) infl);
						}
						else
						{
						    // otherwise assume it's a boolean feature
							word.setFeature(feature, true);
						}
					}
					else
					{
						word.setFeature(feature, value);
					}
				}

			}

		    // if no infl specified, assume regular
			if (inflections.Count == 0)
			{
				inflections.Add(Inflection.REGULAR);
			}

		    // default inflection code is "reg" if we have it, else random pick form infl codes available
			Inflection defaultInfl = inflections.Contains(Inflection.REGULAR) ? Inflection.REGULAR : inflections[0];

			word.setFeature(LexicalFeature.DEFAULT_INFL, defaultInfl);
			word.setDefaultInflectionalVariant(defaultInfl);

			foreach (Inflection infl in inflections)
			{
				word.addInflectionalVariant(infl);
			}

		    // done, return word
			return word;
		}

	    /**
	     * add word to internal indices
	     * 
	     * @param word
	     */
		private void IndexWord(WordElement word)
		{
		    // first index by base form
			string @base = word.BaseForm;
		    // shouldn't really need is, as all words have base forms
			if (!ReferenceEquals(@base, null))
			{
				updateIndex(word, @base, indexByBase);
			}

		    // now index by ID, which should be unique (if present)
			string id = word.Id;
			if (!ReferenceEquals(id, null))
			{
				if (indexByID.ContainsKey(id))
				{
					Console.WriteLine("Lexicon error: ID " + id + " occurs more than once");
				}
				indexByID[id] = word;
			}

		    // now index by variant
			foreach (string variant in getVariants(word))
			{
				updateIndex(word, variant, indexByVariant);
			}

		    // done
		}

	    /**
	     * convenience method to update an index
	     * 
	     * @param word
	     * @param base
	     * @param index
	     */
		private void updateIndex(WordElement word, string @base, IDictionary<string, IList<WordElement>> index)
		{
			if (!index.ContainsKey(@base))
			{
				index[@base] = new List<WordElement>();
			}
			index[@base].Add(word);
		}
	    /******************************************************************************************/
	    // main methods to get data from lexicon
	    /******************************************************************************************/

	    /*
	     * (non-Javadoc)
	     * 
	     * @see simplenlg.lexicon.Lexicon#getWords(java.lang.String,
	     * simplenlg.features.LexicalCategory)
	     */
		public override IList<WordElement> getWords(string baseForm, LexicalCategory category)
		{
			return getWordsFromIndex(baseForm, category, indexByBase);
		}

	    /**
	     * get matching keys from an index map
	     * 
	     * @param indexKey
	     * @param category
	     * @param indexMap
	     * @return
	     */
		private IList<WordElement> getWordsFromIndex(string indexKey, LexicalCategory category, IDictionary<string, IList<WordElement>> indexMap)
		{
			IList<WordElement> result = new List<WordElement>();

		    // case 1: unknown, return empty list
			if (!indexMap.ContainsKey(indexKey))
			{
				return result;
			}

		    // case 2: category is ANY, return everything
			if (category.GetLexicalCategory() == LexicalCategory.LexicalCategoryEnum.ANY)
			{
				foreach (WordElement word in indexMap[indexKey])
				{
					result.Add(new WordElement(word));
				}
				return result;
			}
			else
			{
			    // case 3: other category, search for match
				foreach (WordElement word in indexMap[indexKey])
				{
					if (word.Category == category)
					{
						result.Add(new WordElement(word));
					}
				}
			}
			return result;
		}

	    /*
	     * (non-Javadoc)
	     * 
	     * @see simplenlg.lexicon.Lexicon#getWordsByID(java.lang.String)
	     */
		public override IList<WordElement> getWordsByID(string id)
		{
			IList<WordElement> result = new List<WordElement>();
			if (indexByID.ContainsKey(id))
			{
				result.Add(new WordElement(indexByID[id]));
			}
			return result;
		}


	    /*
	     * (non-Javadoc)
	     * 
	     * @see simplenlg.lexicon.Lexicon#getWordsFromVariant(java.lang.String,
	     * simplenlg.features.LexicalCategory)
	     */
		public override IList<WordElement> getWordsFromVariant(string variant, LexicalCategory category)
		{
			return getWordsFromIndex(variant, category, indexByVariant);
		}

	    /**
	     * quick-and-dirty routine for getting morph variants should be replaced by
	     * something better!
	     * 
	     * @param word
	     * @return
	     */
		private ISet<string> getVariants(WordElement word)
		{
			ISet<string> variants = new HashSet<string>();
			variants.Add(word.BaseForm);
			ElementCategory category = word.Category;
			if (category is LexicalCategory)
			{
				switch (((LexicalCategory) category).GetLexicalCategory())
				{
				    case LexicalCategory.LexicalCategoryEnum.NOUN:
					    variants.Add(getVariant(word, LexicalFeature.PLURAL, "s"));
					    break;

				    case LexicalCategory.LexicalCategoryEnum.ADJECTIVE:
					    variants.Add(getVariant(word, LexicalFeature.COMPARATIVE, "er"));
					    variants.Add(getVariant(word, LexicalFeature.SUPERLATIVE, "est"));
					    break;

				    case LexicalCategory.LexicalCategoryEnum.VERB:
					    variants.Add(getVariant(word, LexicalFeature.PRESENT3S, "s"));
					    variants.Add(getVariant(word, LexicalFeature.PAST, "ed"));
					    variants.Add(getVariant(word, LexicalFeature.PAST_PARTICIPLE, "ed"));
					    variants.Add(getVariant(word, LexicalFeature.PRESENT_PARTICIPLE, "ing"));
					    break;

				    default:
				        // only base needed for other forms
					    break;
				}
			}
			return variants;
		}

	    /**
	     * quick-and-dirty routine for computing morph forms Should be replaced by
	     * something better!
	     * 
	     * @param word
	     * @param feature
	     * @param string
	     * @return
	     */
		private string getVariant(WordElement word, string feature, string suffix)
		{
			if (word.hasFeature(feature))
			{
				return word.getFeatureAsString(feature);
			}
			else
			{
				return getForm(word.BaseForm, suffix);
			}
		}
	    /**
	     * quick-and-dirty routine for standard orthographic changes Should be
	     * replaced by something better!
	     * 
	     * @param base
	     * @param suffix
	     * @return
	     */
		private string getForm(string @base, string suffix)
		{
		    // add a suffix to a base form, with orthographic changes

		    // rule 1 - convert final "y" to "ie" if suffix does not start with "i"
		    // eg, cry + s = cries , not crys
			if (@base.EndsWith("y", StringComparison.Ordinal) && !suffix.StartsWith("i", StringComparison.Ordinal))
			{
				@base = @base.Substring(0, @base.Length - 1) + "ie";
			}
		    // rule 2 - drop final "e" if suffix starts with "e" or "i"
		    // eg, like+ed = liked, not likeed

			if (@base.EndsWith("e", StringComparison.Ordinal) && (suffix.StartsWith("e", StringComparison.Ordinal) || suffix.StartsWith("i", StringComparison.Ordinal)))
			{
				@base = @base.Substring(0, @base.Length - 1);
			}
		    // rule 3 - insert "e" if suffix is "s" and base ends in s, x, z, ch, sh
		    // eg, watch+s -> watches, not watchs
			if (suffix.StartsWith("s", StringComparison.Ordinal) && (@base.EndsWith("s", StringComparison.Ordinal) || @base.EndsWith("x", StringComparison.Ordinal) || @base.EndsWith("z", StringComparison.Ordinal) || @base.EndsWith("ch", StringComparison.Ordinal) || @base.EndsWith("sh", StringComparison.Ordinal)))
			{
				@base = @base + "e";
			}

		    // have made changes, now append and return
			return @base + suffix; // eg, want + s = wants
		}
	}

}