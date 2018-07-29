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

namespace SimpleNLG.Main.syntax.english
{

	using Feature = features.Feature;
	using CoordinatedPhraseElement = framework.CoordinatedPhraseElement;
	using DocumentElement = framework.DocumentElement;
	using ElementCategory = framework.ElementCategory;
	using InflectedWordElement = framework.InflectedWordElement;
	using LexicalCategory = framework.LexicalCategory;
	using ListElement = framework.ListElement;
	using NLGElement = framework.NLGElement;
	using NLGModule = framework.NLGModule;
	using PhraseCategory = framework.PhraseCategory;
	using PhraseElement = framework.PhraseElement;
	using WordElement = framework.WordElement;

    /**
     * <p>
     * This is the processor for handling syntax within the SimpleNLG. The processor
     * translates phrases into lists of words.
     * </p>
     * 
     * <p>
     * All processing modules perform realisation on a tree of
     * <code>NLGElement</code>s. The modules can alter the tree in whichever way
     * they wish. For example, the syntax processor replaces phrase elements with
     * list elements consisting of inflected words while the morphology processor
     * replaces inflected words with string elements.
     * </p>
     * 
     * <p>
     * <b>N.B.</b> the use of <em>module</em>, <em>processing module</em> and
     * <em>processor</em> is interchangeable. They all mean an instance of this
     * class.
     * </p>
     * 
     * @author D. Westwater, University of Aberdeen.
     * @version 4.0
     */
	public class SyntaxProcessor : NLGModule
	{

		public override void initialise()
		{
		    // Do nothing
		}

		public override NLGElement realise(NLGElement element)
		{
			NLGElement realisedElement = null;

			if (element != null && !element.getFeatureAsBoolean(Feature.ELIDED))
			{

				if (element is DocumentElement)
				{
					IList<NLGElement> children = element.Children;
					((DocumentElement) element).Components = realise(children);
					realisedElement = element;

				}
				else if (element is PhraseElement)
				{
					realisedElement = realisePhraseElement((PhraseElement) element);

				}
				else if (element is ListElement)
				{
					realisedElement = new ListElement();
					((ListElement) realisedElement).addComponents(realise(element.Children));

				}
				else if (element is InflectedWordElement)
				{
					string baseForm = ((InflectedWordElement) element).BaseForm;
					ElementCategory category = element.Category;

					if (lexicon != null && !ReferenceEquals(baseForm, null))
					{
						WordElement word = ((InflectedWordElement) element).BaseWord;

						if (word == null)
						{
							if (category is LexicalCategory)
							{
								word = lexicon.lookupWord(baseForm, (LexicalCategory) category);
							}
							else
							{
								word = lexicon.lookupWord(baseForm);
							}
						}

						if (word != null)
						{
							((InflectedWordElement) element).BaseWord = word;
						}
					}

					realisedElement = element;

				}
				else if (element is WordElement)
				{
				    // AG: need to check if it's a word element, in which case it
				    // needs to be marked for inflection
					InflectedWordElement infl = new InflectedWordElement((WordElement) element);


				    // the inflected word inherits all features from the base word
					foreach (string feature in element.AllFeatureNames)
					{
						infl.setFeature(feature, element.getFeature(feature));
					}

					realisedElement = realise(infl);

				}
				else if (element is CoordinatedPhraseElement)
				{
					realisedElement = CoordinatedPhraseHelper.realise(this, (CoordinatedPhraseElement) element);

				}
				else
				{
					realisedElement = element;
				}
			}

		    // Remove the spurious ListElements that have only one element.
			if (realisedElement is ListElement)
			{
				if (((ListElement) realisedElement).size() == 1)
				{
					realisedElement = ((ListElement) realisedElement).First;
				}
			}

			return realisedElement;
		}

		public override IList<NLGElement> realise(IList<NLGElement> elements)
		{
			IList<NLGElement> realisedList = new List<NLGElement>();
			NLGElement childRealisation = null;

			if (elements != null)
			{
				foreach (NLGElement eachElement in elements)
				{
					if (eachElement != null)
					{
						childRealisation = realise(eachElement);
						if (childRealisation != null)
						{
							if (childRealisation is ListElement)
							{
								((List<NLGElement>)realisedList).AddRange(((ListElement) childRealisation).Children);
							}
							else
							{
								realisedList.Add(childRealisation);
							}
						}
					}
				}
			}
			return realisedList;
		}

	    /**
	     * Realises a phrase element.
	     * 
	     * @param phrase
	     *            the element to be realised
	     * @return the realised element.
	     */
		private NLGElement realisePhraseElement(PhraseElement phrase)
		{
			NLGElement realisedElement = null;

			if (phrase != null)
			{
				ElementCategory category = phrase.Category;

				if (category is PhraseCategory)
				{
					switch (((PhraseCategory) category).GetPhraseCategory())
					{

				        case PhraseCategory.PhraseCategoryEnum.CLAUSE:
					        realisedElement = ClauseHelper.realise(this, phrase);
					        break;

				        case PhraseCategory.PhraseCategoryEnum.NOUN_PHRASE:
					        realisedElement = NounPhraseHelper.realise(this, phrase);
					        break;

				        case PhraseCategory.PhraseCategoryEnum.VERB_PHRASE:
					        realisedElement = VerbPhraseHelper.realise(this, phrase);
					        break;

				        case PhraseCategory.PhraseCategoryEnum.PREPOSITIONAL_PHRASE:
				        case PhraseCategory.PhraseCategoryEnum.ADJECTIVE_PHRASE:
				        case PhraseCategory.PhraseCategoryEnum.ADVERB_PHRASE:
					        realisedElement = PhraseHelper.realise(this, phrase);
					        break;

				        default:
					        realisedElement = phrase;
					        break;
					}
				}
			}

			return realisedElement;
		}
	}

}