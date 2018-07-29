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

using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleNLG.Main.syntax.english
{

	using DiscourseFunction = features.DiscourseFunction;
	using Feature = features.Feature;
	using InternalFeature = features.InternalFeature;
	using LexicalFeature = features.LexicalFeature;
	using InflectedWordElement = framework.InflectedWordElement;
	using LexicalCategory = framework.LexicalCategory;
	using ListElement = framework.ListElement;
	using NLGElement = framework.NLGElement;
	using PhraseCategory = framework.PhraseCategory;
	using PhraseElement = framework.PhraseElement;

    /**
     * <p>
     * This class contains static methods to help the syntax processor realise
     * phrases.
     * </p>
     * 
     * @author E. Reiter and D. Westwater, University of Aberdeen.
     * @version 4.0
     */
	internal abstract class PhraseHelper
	{
	    /**
	     * The main method for realising phrases.
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

			if (phrase != null)
			{
				realisedElement = new ListElement();

				realiseList(parent, realisedElement, phrase.PreModifiers, DiscourseFunction.PRE_MODIFIER);

				realiseHead(parent, phrase, realisedElement);
				realiseComplements(parent, phrase, realisedElement);

				realiseList(parent, realisedElement, phrase.PostModifiers, DiscourseFunction.POST_MODIFIER);
			}

			return realisedElement;
		}

	    /**
	     * Realises the complements of the phrase adding <em>and</em> where
	     * appropriate.
	     * 
	     * @param parent
	     *            the parent <code>SyntaxProcessor</code> that will do the
	     *            realisation of the complementiser.
	     * @param phrase
	     *            the <code>PhraseElement</code> representing this noun phrase.
	     * @param realisedElement
	     *            the current realisation of the noun phrase.
	     */
		private static void realiseComplements(SyntaxProcessor parent, PhraseElement phrase, ListElement realisedElement)
		{

			bool firstProcessed = false;
			NLGElement currentElement = null;

			foreach (NLGElement complement in phrase.getFeatureAsElementList(InternalFeature.COMPLEMENTS))
			{
				currentElement = parent.realise(complement);
				if (currentElement != null)
				{
					currentElement.setFeature(InternalFeature.DISCOURSE_FUNCTION, DiscourseFunction.COMPLEMENT);
					if (firstProcessed)
					{
						realisedElement.addComponent(new InflectedWordElement("and", new LexicalCategory(LexicalCategory.LexicalCategoryEnum.CONJUNCTION))); //$NON-NLS-1$
					}
					else
					{
						firstProcessed = true;
					}
					realisedElement.addComponent(currentElement);
				}
			}
		}

	    /**
	     * Realises the head element of the phrase.
	     * 
	     * @param parent
	     *            the parent <code>SyntaxProcessor</code> that will do the
	     *            realisation of the complementiser.
	     * @param phrase
	     *            the <code>PhraseElement</code> representing this noun phrase.
	     * @param realisedElement
	     *            the current realisation of the noun phrase.
	     */
		private static void realiseHead(SyntaxProcessor parent, PhraseElement phrase, ListElement realisedElement)
		{

			NLGElement head = phrase.getHead();
			if (head != null)
			{
				if (phrase.hasFeature(Feature.IS_COMPARATIVE))
				{
					head.setFeature(Feature.IS_COMPARATIVE, phrase.getFeature(Feature.IS_COMPARATIVE));
				}
				else if (phrase.hasFeature(Feature.IS_SUPERLATIVE))
				{
					head.setFeature(Feature.IS_SUPERLATIVE, phrase.getFeature(Feature.IS_SUPERLATIVE));
				}
				head = parent.realise(head);
				head.setFeature(InternalFeature.DISCOURSE_FUNCTION, DiscourseFunction.HEAD);
				realisedElement.addComponent(head);
			}
		}

	    /**
	     * Iterates through a <code>List</code> of <code>NLGElement</code>s
	     * realisation each element and adding it to the on-going realisation of
	     * this clause.
	     * 
	     * @param parent
	     *            the parent <code>SyntaxProcessor</code> that will do the
	     *            realisation of the complementiser.
	     * @param realisedElement
	     *            the current realisation of the clause.
	     * @param elementList
	     *            the <code>List</code> of <code>NLGElement</code>s to be
	     *            realised.
	     * @param function
	     *            the <code>DiscourseFunction</code> each element in the list is
	     *            to take. If this is <code>null</code> then the function is not
	     *            set and any existing discourse function is kept.
	     */
		internal static void realiseList(SyntaxProcessor parent, ListElement realisedElement, IList<NLGElement> elementList, DiscourseFunction function)
		{


		    // AG: Change here: the original list structure is kept, i.e. rather
		    // than taking the elements of the list and putting them in the realised
		    // element, we now add the realised elements to a new list and put that
		    // in the realised element list. This preserves constituency for
		    // orthography and morphology processing later.
			ListElement realisedList = new ListElement();
			NLGElement currentElement = null;

			foreach (NLGElement eachElement in elementList)
			{
				currentElement = parent.realise(eachElement);

				if (currentElement != null)
				{
					currentElement.setFeature(InternalFeature.DISCOURSE_FUNCTION, function);

					if (eachElement.getFeatureAsBoolean(Feature.APPOSITIVE))
					{
						currentElement.setFeature(Feature.APPOSITIVE, true);
					}

				    // realisedElement.addComponent(currentElement);
					realisedList.addComponent(currentElement);
				}
			}

			if (realisedList.Children.Any())
			{
				realisedElement.addComponent(realisedList);
			}
		}

	    /**
	     * Determines if the given phrase has an expletive as a subject.
	     * 
	     * @param phrase
	     *            the <code>PhraseElement</code> to be examined.
	     * @return <code>true</code> if the phrase has an expletive subject.
	     */
		public static bool isExpletiveSubject(PhraseElement phrase)
		{
			IList<NLGElement> subjects = phrase.getFeatureAsElementList(InternalFeature.SUBJECTS);
			bool expletive = false;

			if (subjects.Count == 1)
			{
				NLGElement subjectNP = subjects[0];

				if (subjectNP.isA(new PhraseCategory(PhraseCategory.PhraseCategoryEnum.NOUN_PHRASE)))
				{
					expletive = subjectNP.getFeatureAsBoolean(LexicalFeature.EXPLETIVE_SUBJECT);
				}
				else if (subjectNP.isA(new PhraseCategory(PhraseCategory.PhraseCategoryEnum.CANNED_TEXT)))
				{
					expletive = "there".Equals(subjectNP.Realisation, StringComparison.OrdinalIgnoreCase); //$NON-NLS-1$
				}
			}
			return expletive;
		}
	}

}