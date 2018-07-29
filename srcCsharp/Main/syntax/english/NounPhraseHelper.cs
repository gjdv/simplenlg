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

	using DiscourseFunction = features.DiscourseFunction;
	using Feature = features.Feature;
	using Gender = features.Gender;
	using InternalFeature = features.InternalFeature;
	using LexicalFeature = features.LexicalFeature;
	using Person = features.Person;
	using InflectedWordElement = framework.InflectedWordElement;
	using LexicalCategory = framework.LexicalCategory;
	using ListElement = framework.ListElement;
	using NLGElement = framework.NLGElement;
	using PhraseCategory = framework.PhraseCategory;
	using PhraseElement = framework.PhraseElement;
	using NLGFactory = framework.NLGFactory;
	using WordElement = framework.WordElement;

    /**
     * <p>
     * This class contains static methods to help the syntax processor realise noun
     * phrases.
     * </p>
     * 
     * @author E. Reiter and D. Westwater, University of Aberdeen.
     * @version 4.0
     */
	internal abstract class NounPhraseHelper
	{

	    /** The qualitative position for ordering premodifiers. */
		private const int QUALITATIVE_POSITION = 1;

	    /** The colour position for ordering premodifiers. */
		private const int COLOUR_POSITION = 2;

	    /** The classifying position for ordering premodifiers. */
		private const int CLASSIFYING_POSITION = 3;

	    /** The noun position for ordering premodifiers. */
		private const int NOUN_POSITION = 4;

	    /**
	     * The main method for realising noun phrases.
	     * 
	     * @param parent
	     *            the <code>SyntaxProcessor</code> that called this method.
	     * @param phrase
	     *            the <code>PhraseElement</code> to be realised.
	     * @return the realised <code>NLGElement</code>.
	     */
		internal static NLGElement realise(SyntaxProcessor parent, PhraseElement phrase)
		{
			ListElement realisedElement = null;

			if (phrase != null && !phrase.getFeatureAsBoolean(Feature.ELIDED))
			{
				realisedElement = new ListElement();

				if (phrase.getFeatureAsBoolean(Feature.PRONOMINAL))
				{
					realisedElement.addComponent(createPronoun(parent, phrase));

				}
				else
				{
					realiseSpecifier(phrase, parent, realisedElement);
					realisePreModifiers(phrase, parent, realisedElement);
					realiseHeadNoun(phrase, parent, realisedElement);
					PhraseHelper.realiseList(parent, realisedElement, phrase.getFeatureAsElementList(InternalFeature.COMPLEMENTS), DiscourseFunction.COMPLEMENT);

					PhraseHelper.realiseList(parent, realisedElement, phrase.PostModifiers, DiscourseFunction.POST_MODIFIER);
				}
			}

			return realisedElement;
		}

	    /**
	     * Realises the head noun of the noun phrase.
	     * 
	     * @param phrase
	     *            the <code>PhraseElement</code> representing this noun phrase.
	     * @param parent
	     *            the parent <code>SyntaxProcessor</code> that will do the
	     *            realisation of the complementiser.
	     * @param realisedElement
	     *            the current realisation of the noun phrase.
	     */
		private static void realiseHeadNoun(PhraseElement phrase, SyntaxProcessor parent, ListElement realisedElement)
		{
			NLGElement headElement = phrase.getHead();

			if (headElement != null)
			{
				headElement.setFeature(Feature.ELIDED, phrase.getFeature(Feature.ELIDED));
				headElement.setFeature(LexicalFeature.GENDER, phrase.getFeature(LexicalFeature.GENDER));
				headElement.setFeature(InternalFeature.ACRONYM, phrase.getFeature(InternalFeature.ACRONYM));
				headElement.setFeature(Feature.NUMBER, phrase.getFeature(Feature.NUMBER));
				headElement.setFeature(Feature.PERSON, phrase.getFeature(Feature.PERSON));
				headElement.setFeature(Feature.POSSESSIVE, phrase.getFeature(Feature.POSSESSIVE));
				headElement.setFeature(Feature.PASSIVE, phrase.getFeature(Feature.PASSIVE));
				headElement.setFeature(Feature.IS_CAPITALIZED, phrase.getFeature(Feature.IS_CAPITALIZED));
				NLGElement currentElement = parent.realise(headElement);
				currentElement.setFeature(InternalFeature.DISCOURSE_FUNCTION, DiscourseFunction.SUBJECT);
				realisedElement.addComponent(currentElement);
			}
		}

	    /**
	     * Realises the pre-modifiers of the noun phrase. Before being realised,
	     * pre-modifiers undergo some basic sorting based on adjective ordering.
	     * 
	     * @param phrase
	     *            the <code>PhraseElement</code> representing this noun phrase.
	     * @param parent
	     *            the parent <code>SyntaxProcessor</code> that will do the
	     *            realisation of the complementiser.
	     * @param realisedElement
	     *            the current realisation of the noun phrase.
	     */
		private static void realisePreModifiers(PhraseElement phrase, SyntaxProcessor parent, ListElement realisedElement)
		{

			IList<NLGElement> preModifiers = phrase.PreModifiers;
			if (phrase.getFeatureAsBoolean(Feature.ADJECTIVE_ORDERING))
			{
				preModifiers = sortNPPreModifiers(preModifiers);
			}
			PhraseHelper.realiseList(parent, realisedElement, preModifiers, DiscourseFunction.PRE_MODIFIER);
		}

	    /**
	     * Realises the specifier of the noun phrase.
	     * 
	     * @param phrase
	     *            the <code>PhraseElement</code> representing this noun phrase.
	     * @param parent
	     *            the parent <code>SyntaxProcessor</code> that will do the
	     *            realisation of the complementiser.
	     * @param realisedElement
	     *            the current realisation of the noun phrase.
	     */
		private static void realiseSpecifier(PhraseElement phrase, SyntaxProcessor parent, ListElement realisedElement)
		{
			NLGElement specifierElement = phrase.getFeatureAsElement(InternalFeature.SPECIFIER);

			if (specifierElement != null && !phrase.getFeatureAsBoolean(InternalFeature.RAISED) && !phrase.getFeatureAsBoolean(Feature.ELIDED))
			{
				if (!specifierElement.isA(new LexicalCategory(LexicalCategory.LexicalCategoryEnum.PRONOUN)) && specifierElement.Category != PhraseCategory.PhraseCategoryEnum.NOUN_PHRASE)
				{
					specifierElement.setFeature(Feature.NUMBER, phrase.getFeature(Feature.NUMBER));
				}

				NLGElement currentElement = parent.realise(specifierElement);

				if (currentElement != null)
				{
					currentElement.setFeature(InternalFeature.DISCOURSE_FUNCTION, DiscourseFunction.SPECIFIER);
					realisedElement.addComponent(currentElement);
				}
			}
		}

	    /**
	     * Sort the list of premodifiers for this noun phrase using adjective
	     * ordering (ie, "big" comes before "red")
	     * 
	     * @param originalModifiers
	     *            the original listing of the premodifiers.
	     * @return the sorted <code>List</code> of premodifiers.
	     */
		private static IList<NLGElement> sortNPPreModifiers(IList<NLGElement> originalModifiers)
		{

			IList<NLGElement> orderedModifiers = null;

			if (originalModifiers == null || originalModifiers.Count <= 1)
			{
				orderedModifiers = originalModifiers;
			}
			else
			{
				orderedModifiers = new List<NLGElement>(originalModifiers);
				bool changesMade = false;
				do
				{
					changesMade = false;
					for (int i = 0; i < orderedModifiers.Count - 1; i++)
					{
						if (getMinPos(orderedModifiers[i]) > getMaxPos(orderedModifiers[i + 1]))
						{
							NLGElement temp = orderedModifiers[i];
							orderedModifiers[i] = orderedModifiers[i + 1];
							orderedModifiers[i + 1] = temp;
							changesMade = true;
						}
					}
				} while (changesMade == true);
			}
			return orderedModifiers;
		}

	    /**
	     * Determines the minimim position at which this modifier can occur.
	     * 
	     * @param modifier
	     *            the modifier to be checked.
	     * @return the minimum position for this modifier.
	     */
		private static int getMinPos(NLGElement modifier)
		{
			int position = QUALITATIVE_POSITION;

			if (modifier.isA(new LexicalCategory(LexicalCategory.LexicalCategoryEnum.NOUN)) || modifier.isA(new PhraseCategory(PhraseCategory.PhraseCategoryEnum.NOUN_PHRASE)))
			{

				position = NOUN_POSITION;
			}
			else if (modifier.isA(new LexicalCategory(LexicalCategory.LexicalCategoryEnum.ADJECTIVE)) || modifier.isA(new PhraseCategory(PhraseCategory.PhraseCategoryEnum.ADJECTIVE_PHRASE)))
			{
				WordElement adjective = getHeadWordElement(modifier);

				if (adjective.getFeatureAsBoolean(LexicalFeature.QUALITATIVE))
				{
					position = QUALITATIVE_POSITION;
				}
				else if (adjective.getFeatureAsBoolean(LexicalFeature.COLOUR))
				{
					position = COLOUR_POSITION;
				}
				else if (adjective.getFeatureAsBoolean(LexicalFeature.CLASSIFYING))
				{
					position = CLASSIFYING_POSITION;
				}
			}
			return position;
		}

	    /**
	     * Determines the maximim position at which this modifier can occur.
	     * 
	     * @param modifier
	     *            the modifier to be checked.
	     * @return the maximum position for this modifier.
	     */
		private static int getMaxPos(NLGElement modifier)
		{
			int position = NOUN_POSITION;

			if (modifier.isA(new LexicalCategory(LexicalCategory.LexicalCategoryEnum.ADJECTIVE)) || modifier.isA(new PhraseCategory(PhraseCategory.PhraseCategoryEnum.ADJECTIVE_PHRASE)))
			{
				WordElement adjective = getHeadWordElement(modifier);

				if (adjective.getFeatureAsBoolean(LexicalFeature.CLASSIFYING))
				{
					position = CLASSIFYING_POSITION;
				}
				else if (adjective.getFeatureAsBoolean(LexicalFeature.COLOUR))
				{
					position = COLOUR_POSITION;
				}
				else if (adjective.getFeatureAsBoolean(LexicalFeature.QUALITATIVE))
				{
					position = QUALITATIVE_POSITION;
				}
				else
				{
					position = CLASSIFYING_POSITION;
				}
			}
			return position;
		}

	    /**
	     * Retrieves the correct representation of the word from the element. This
	     * method will find the <code>WordElement</code>, if it exists, for the
	     * given phrase or inflected word.
	     * 
	     * @param element
	     *            the <code>NLGElement</code> from which the head is required.
	     * @return the <code>WordElement</code>
	     */
		private static WordElement getHeadWordElement(NLGElement element)
		{
			WordElement head = null;

			if (element is WordElement)
			{
				head = (WordElement) element;
			}
			else if (element is InflectedWordElement)
			{
				head = (WordElement) element.getFeature(InternalFeature.BASE_WORD);
			}
			else if (element is PhraseElement)
			{
				head = getHeadWordElement(((PhraseElement) element).getHead());
			}

			return head;
		}

	    /**
	     * Creates the appropriate pronoun if the subject of the noun phrase is
	     * pronominal.
	     * 
	     * @param parent
	     *            the parent <code>SyntaxProcessor</code> that will do the
	     *            realisation of the complementiser.
	     * @param phrase
	     *            the <code>PhraseElement</code> representing this noun phrase.
	     * @return the <code>NLGElement</code> representing the pronominal.
	     */
		private static NLGElement createPronoun(SyntaxProcessor parent, PhraseElement phrase)
		{

			string pronoun = "it"; //$NON-NLS-1$
			NLGFactory phraseFactory = phrase.Factory;
			object personValue = phrase.getFeature(Feature.PERSON);

			if (Person.FIRST.Equals(personValue))
			{
				pronoun = "I"; //$NON-NLS-1$
			}
			else if (Person.SECOND.Equals(personValue))
			{
				pronoun = "you"; //$NON-NLS-1$
			}
			else
			{
				object genderValue = phrase.getFeature(LexicalFeature.GENDER);
				if (Gender.FEMININE.Equals(genderValue))
				{
					pronoun = "she"; //$NON-NLS-1$
				}
				else if (Gender.MASCULINE.Equals(genderValue))
				{
					pronoun = "he"; //$NON-NLS-1$
				}
			}
		    // AG: createWord now returns WordElement; so we embed it in an
		    // inflected word element here
			NLGElement element;
			NLGElement proElement = phraseFactory.createWord(pronoun, new LexicalCategory(LexicalCategory.LexicalCategoryEnum.PRONOUN));

			if (proElement is WordElement)
			{
				element = new InflectedWordElement((WordElement) proElement);
				element.setFeature(LexicalFeature.GENDER, ((WordElement) proElement).getFeature(LexicalFeature.GENDER));
			    // Ehud - also copy over person
				element.setFeature(Feature.PERSON, ((WordElement) proElement).getFeature(Feature.PERSON));
			}
			else
			{
				element = proElement;
			}

			element.setFeature(InternalFeature.DISCOURSE_FUNCTION, DiscourseFunction.SPECIFIER);
			element.setFeature(Feature.POSSESSIVE, phrase.getFeature(Feature.POSSESSIVE));
			element.setFeature(Feature.NUMBER, phrase.getFeature(Feature.NUMBER));


			if (phrase.hasFeature(InternalFeature.DISCOURSE_FUNCTION))
			{
				element.setFeature(InternalFeature.DISCOURSE_FUNCTION, phrase.getFeature(InternalFeature.DISCOURSE_FUNCTION));
			}

			return element;
		}
	}

}