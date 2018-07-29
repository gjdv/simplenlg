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
 * Contributor(s): Ehud Reiter, Albert Gatt, Dave Wewstwater, Roman Kutlak, Margaret Mitchell.
 *
 * Ported to C# by Gert-Jan de Vries
 */

using System.Collections.Generic;
using System.Linq;

namespace SimpleNLG.Main.lexicon
{

	using LexicalCategory = framework.LexicalCategory;
	using WordElement = framework.WordElement;

    /** This class contains a set of lexicons, which are searched in
     * order for the specified word
     * 
     * @author ereiter
     *
     */
	public class MultipleLexicon : Lexicon
	{
	    /* if this flag is true, all lexicons are searched for
	     * this word, even after a match is found
	     * it is false by default
	     */
		private bool alwaysSearchAll = false;

	    /* list of lexicons, in order in which they are searched */
		    private IList<Lexicon> lexiconList = null;

	    /**********************************************************************/
	    // constructors
	    /**********************************************************************/
	    
	    /**
	     * create an empty multi lexicon
	     */
		public MultipleLexicon() : base()
		{
			lexiconList = new List<Lexicon>();
			alwaysSearchAll = false;
		}

	    /** create a multi lexicon with the specified lexicons
	     * @param lexicons
	     */
		public MultipleLexicon(params Lexicon[] lexicons) : this()
		{
			foreach (Lexicon lex in lexicons)
			{
				lexiconList.Add(lex);
			}
		}
	    /**********************************************************************/
	    // routines to add more lexicons, change flags
	    /**********************************************************************/

	    /** add lexicon at beginning of list (is searched first)
	     * @param lex
	     */
		public virtual void addInitialLexicon(Lexicon lex)
		{
			lexiconList.Insert(0, lex);
		}

	    /** add lexicon at end of list (is searched last)
	     * @param lex
	     */
		public virtual void addFinalLexicon(Lexicon lex)
		{
			lexiconList.Insert(0, lex);
		}

	    /**
	     * @return the alwaysSearchAll
	     */
		public virtual bool AlwaysSearchAll
		{
			get
			{
				return alwaysSearchAll;
			}
			set
			{
				alwaysSearchAll = value;
			}
		}
		
	    /**********************************************************************/
	    // main methods
	    /**********************************************************************/

	    /* (non-Javadoc)
	     * @see simplenlg.lexicon.Lexicon#getWords(java.lang.String, simplenlg.features.LexicalCategory)
	     */
		public override IList<WordElement> getWords(string baseForm, LexicalCategory category)
		{
			IList<WordElement> result = new List<WordElement>();
			foreach (Lexicon lex in lexiconList)
			{
				IList<WordElement> lexResult = lex.getWords(baseForm, category);
				if (lexResult != null && lexResult.Any())
				{
					((List<WordElement>)result).AddRange(lexResult);
					if (!alwaysSearchAll)
					{
						return result;
					}
				}
			}
			return result;
		}

	    /* (non-Javadoc)
	     * @see simplenlg.lexicon.Lexicon#getWordsByID(java.lang.String)
	     */
		public override IList<WordElement> getWordsByID(string id)
		{
			IList<WordElement> result = new List<WordElement>();
			foreach (Lexicon lex in lexiconList)
			{
				IList<WordElement> lexResult = lex.getWordsByID(id);
				if (lexResult != null && lexResult.Any())
				{
					((List<WordElement>)result).AddRange(lexResult);
					if (!alwaysSearchAll)
					{
						return result;
					}
				}
			}
			return result;
		}

	    /* (non-Javadoc)
	     * @see simplenlg.lexicon.Lexicon#getWordsFromVariant(java.lang.String, simplenlg.features.LexicalCategory)
	     */
		public override IList<WordElement> getWordsFromVariant(string variant, LexicalCategory category)
		{
			IList<WordElement> result = new List<WordElement>();
			foreach (Lexicon lex in lexiconList)
			{
				IList<WordElement> lexResult = lex.getWordsFromVariant(variant, category);
				if (lexResult != null && lexResult.Any())
				{
					((List<WordElement>)result).AddRange(lexResult);
					if (!alwaysSearchAll)
					{
						return result;
					}
				}
			}
			return result;
		}


	    /**********************************************************************/
	    // other methods
	    /**********************************************************************/

	    /* (non-Javadoc)
	     * @see simplenlg.lexicon.Lexicon#close()
	     */
		public override void close()
		{
		    // close component lexicons
			foreach (Lexicon lex in lexiconList)
			{
				lex.close();
			}
		}

	}

}