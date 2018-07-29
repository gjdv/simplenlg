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
using SimpleNLG.Main.framework;

namespace SimpleNLG.Main.morphology.english
{

	using DiscourseFunction = features.DiscourseFunction;
	using Feature = features.Feature;
	using InternalFeature = features.InternalFeature;

    /**
     * <p>
     * This is the processor for handling morphology within the SimpleNLG. The
     * processor inflects words form the base form depending on the features applied
     * to the word. For example, <em>kiss</em> is inflected to <em>kissed</em> for
     * past tense, <em>dog</em> is inflected to <em>dogs</em> for pluralisation.
     * </p>
     *
     * <p>
     * As a matter of course, the processor will first use any user-defined
     * inflection for the world. If no inflection is provided then the lexicon, if
     * it exists, will be examined for the correct inflection. Failing this a set of
     * very basic rules will be examined to inflect the word.
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
     *
     * @author D. Westwater, University of Aberdeen.
     * @version 4.0
     */
    public class MorphologyProcessor : NLGModule
	{

		public override void initialise()
		{
		    // Do nothing
		}

		public override NLGElement realise(NLGElement element)
		{
			NLGElement realisedElement = null;

			if (element is InflectedWordElement)
			{
				realisedElement = doMorphology((InflectedWordElement) element);

			}
			else if (element is StringElement)
			{
				realisedElement = element;

			}
			else if (element is WordElement)
			{
			    // AG: now retrieves the default spelling variant, not the baseform
			    // String baseForm = ((WordElement) element).getBaseForm();
				string defaultSpell = ((WordElement) element).DefaultSpellingVariant;

				if (!ReferenceEquals(defaultSpell, null))
				{
					realisedElement = new StringElement(defaultSpell);
				}

			}
			else if (element is DocumentElement)
			{
				IList<NLGElement> children = element.Children;
				((DocumentElement) element).Components = realise(children);
				realisedElement = element;

			}
			else if (element is ListElement)
			{
				realisedElement = new ListElement();
				((ListElement) realisedElement).addComponents(realise(element.Children));

			}
			else if (element is CoordinatedPhraseElement)
			{
				IList<NLGElement> children = element.Children;
				((CoordinatedPhraseElement) element).clearCoordinates();

				if (children != null && children.Any())
				{
					((CoordinatedPhraseElement) element).addCoordinate(realise(children[0]));

					for (int index = 1; index < children.Count; index++)
					{
						((CoordinatedPhraseElement) element).addCoordinate(realise(children[index]));
					}

					realisedElement = element;
				}

			}
			else if (element != null)
			{
				realisedElement = element;
			}

			return realisedElement;
		}

	    /**
	     * This is the main method for performing the morphology. It effectively
	     * examines the lexical category of the element and calls the relevant set
	     * of rules from <code>MorphologyRules</em>.
	     *
	     * @param element
	     *            the <code>InflectedWordElement</code>
	     * @return an <code>NLGElement</code> reflecting the correct inflection for
	     *         the word.
	     */
		private NLGElement doMorphology(InflectedWordElement element)
		{
			NLGElement realisedElement = null;
			if (element.getFeatureAsBoolean(InternalFeature.NON_MORPH))
			{
				realisedElement = new StringElement(element.BaseForm);
				realisedElement.setFeature(InternalFeature.DISCOURSE_FUNCTION, element.getFeature(InternalFeature.DISCOURSE_FUNCTION));

			}
			else
			{
				NLGElement baseWord = element.getFeatureAsElement(InternalFeature.BASE_WORD);

				if (baseWord == null && lexicon != null)
				{
					baseWord = lexicon.lookupWord(element.BaseForm);
				}

				ElementCategory category = element.Category;

				if (category is LexicalCategory)
				{
					switch (((LexicalCategory) category).GetLexicalCategory())
					{
					    case LexicalCategory.LexicalCategoryEnum.PRONOUN:
						    realisedElement = MorphologyRules.doPronounMorphology(element);
						    break;

					    case LexicalCategory.LexicalCategoryEnum.NOUN:
						    realisedElement = MorphologyRules.doNounMorphology(element, (WordElement) baseWord);
						    break;

					    case LexicalCategory.LexicalCategoryEnum.VERB:
						    realisedElement = MorphologyRules.doVerbMorphology(element, (WordElement) baseWord);
						    break;

					    case LexicalCategory.LexicalCategoryEnum.ADJECTIVE:
						    realisedElement = MorphologyRules.doAdjectiveMorphology(element, (WordElement) baseWord);
						    break;

					    case LexicalCategory.LexicalCategoryEnum.ADVERB:
						    realisedElement = MorphologyRules.doAdverbMorphology(element, (WordElement) baseWord);
						    break;

					    default:
						    realisedElement = new StringElement(element.BaseForm);
						    realisedElement.setFeature(InternalFeature.DISCOURSE_FUNCTION, element.getFeature(InternalFeature.DISCOURSE_FUNCTION));
					    break;
					}
				}
			}
			return realisedElement;
		}

		public override IList<NLGElement> realise(IList<NLGElement> elements)
		{
			IList<NLGElement> realisedElements = new List<NLGElement>();
			NLGElement currentElement = null;
			NLGElement determiner = null;
			NLGElement prevElement = null;

			if (elements != null)
			{
				foreach (NLGElement eachElement in elements)
				{
					currentElement = realise(eachElement);

					if (currentElement != null)
					{
					    //pass the discourse function and appositive features -- important for orth processor
						currentElement.setFeature(Feature.APPOSITIVE, eachElement.getFeature(Feature.APPOSITIVE));
						object function = eachElement.getFeature(InternalFeature.DISCOURSE_FUNCTION);

						if (function != null)
						{
							currentElement.setFeature(InternalFeature.DISCOURSE_FUNCTION, function);
						}

						if (prevElement != null && prevElement is StringElement && eachElement is InflectedWordElement && ((InflectedWordElement) eachElement).Category == LexicalCategory.LexicalCategoryEnum.NOUN)
						{

							string prevString = prevElement.Realisation;

						    //realisedElements.get(realisedElements.size() - 1)

							prevElement.Realisation = DeterminerAgrHelper.checkEndsWithIndefiniteArticle(prevString, currentElement.Realisation);

						}

					    // realisedElements.add(realise(currentElement));
						realisedElements.Add(currentElement);

						if (determiner == null && DiscourseFunction.SPECIFIER.Equals(currentElement.getFeature(InternalFeature.DISCOURSE_FUNCTION)))
						{
							determiner = currentElement;
							determiner.setFeature(Feature.NUMBER, eachElement.getFeature(Feature.NUMBER));
						    // MorphologyRules.doDeterminerMorphology(determiner,
						    // currentElement.getRealisation());

						}
						else if (determiner != null)
						{

							if (currentElement is ListElement)
							{
							    // list elements: ensure det matches first element
								NLGElement firstChild = ((ListElement) currentElement).Children[0];

								if (firstChild != null)
								{
								    //AG: need to check if child is a coordinate
									if (firstChild is CoordinatedPhraseElement)
									{
										MorphologyRules.doDeterminerMorphology(determiner, firstChild.Children[0].Realisation);
									}
									else
									{
										MorphologyRules.doDeterminerMorphology(determiner, firstChild.Realisation);
									}
								}

							}
							else
							{
							    // everything else: ensure det matches realisation
								MorphologyRules.doDeterminerMorphology(determiner, currentElement.Realisation);
							}

							determiner = null;
						}
					}
					prevElement = eachElement;
				}
			}

			return realisedElements;
		}

	}

}