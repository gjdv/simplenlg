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
using System.Text;

namespace SimpleNLG.Main.framework
{

	using InternalFeature = features.InternalFeature;
	using LexicalFeature = features.LexicalFeature;

    /**
     * <p>
     * This class defines the <code>NLGElement</code> that is used to represent an
     * word that requires inflection by the morphology. It has convenience methods
     * for retrieving the base form of the word (for example, <em>kiss</em>,
     * <em>eat</em>) and for setting and retrieving the base word. The base word is
     * a <code>WordElement</code> constructed by the lexicon.
     * </p>
     * 
     * @author D. Westwater, University of Aberdeen.
     * @version 4.0
     * 
     */
	public class InflectedWordElement : NLGElement
	{
	    /**
	     * Constructs a new inflected word using the giving word as the base form.
	     * Constructing the word also requires a lexical category (such as noun,
	     * verb).
	     * 
	     * @param word
	     *            the base form for this inflected word.
	     * @param category
	     *            the lexical category for the word.
	     */
		public InflectedWordElement(string word, LexicalCategory category) : base()
		{
			setFeature(LexicalFeature.BASE_FORM, word);
			Category = category;
		}

	    /**
	     * Constructs a new inflected word from a WordElement
	     * 
	     * @param word
	     *            underlying wordelement
	     */
		public InflectedWordElement(WordElement word) : base()
		{
			setFeature(InternalFeature.BASE_WORD, word);
		    // AG: changed to use the default spelling variant
		    // setFeature(LexicalFeature.BASE_FORM, word.getBaseForm());
			string defaultSpelling = word.DefaultSpellingVariant;
			setFeature(LexicalFeature.BASE_FORM, defaultSpelling);
			Category = word.Category;
		}

	    /**
	     * This method returns null as the inflected word has no child components.
	     */
		public override IList<NLGElement> Children
		{
			get
			{
				return null;
			}
		}

		public override string ToString()
		{
			return "InflectedWordElement[" + BaseForm + ':' + Category.ToString() + ']'; //$NON-NLS-1$
		}

		public override string printTree(string indent)
		{
			StringBuilder print = new StringBuilder();
			print.Append("InflectedWordElement: base=").Append(BaseForm).Append(", category=").Append(Category.ToString()).Append(", ").Append(base.ToString()).Append('\n'); //$NON-NLS-1$ - $NON-NLS-1$ - $NON-NLS-1$
			return print.ToString();
		}

	    /**
	     * Retrieves the base form for this element. The base form is the originally
	     * supplied word.
	     * 
	     * @return a <code>String</code> forming the base form of the element.
	     */
		public virtual string BaseForm
		{
			get
			{
				return getFeatureAsString(LexicalFeature.BASE_FORM);
			}
		}
	    /**
	     * Set / get the base word for this element.
	     * 
	     * @param word
	     *            the <code>WordElement</code> representing the base word as
	     *            read from the lexicon.
	     */
		public virtual WordElement BaseWord
		{
			set
			{
				setFeature(InternalFeature.BASE_WORD, value);
			}
			get
			{
				NLGElement baseWord = getFeatureAsElement(InternalFeature.BASE_WORD);
				return baseWord is WordElement ? (WordElement) baseWord : null;
			}
		}


	}

}