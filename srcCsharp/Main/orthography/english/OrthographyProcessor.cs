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
using System.Text;
using System.Text.RegularExpressions;

namespace SimpleNLG.Main.orthography.english
{

	using DiscourseFunction = features.DiscourseFunction;
	using Feature = features.Feature;
	using InternalFeature = features.InternalFeature;
	using CoordinatedPhraseElement = framework.CoordinatedPhraseElement;
	using DocumentCategory = framework.DocumentCategory;
	using DocumentElement = framework.DocumentElement;
	using ElementCategory = framework.ElementCategory;
	using ListElement = framework.ListElement;
	using NLGElement = framework.NLGElement;
	using NLGModule = framework.NLGModule;
	using StringElement = framework.StringElement;

    /**
     * <p>
     * This processing module deals with punctuation when applied to
     * <code>DocumentElement</code>s. The punctuation currently handled by this
     * processor includes the following (as of version 4.0):
     * <ul>
     * <li>Capitalisation of the first letter in sentences.</li>
     * <li>Termination of sentences with a period if not interrogative.</li>
     * <li>Termination of sentences with a question mark if they are interrogative.</li>
     * <li>Replacement of multiple conjunctions with a comma. For example,
     * <em>John and Peter and Simon</em> becomes <em>John, Peter and Simon</em>.</li>
     * </ul>
     * </p>
     * 
     * 
     * @author D. Westwater, University of Aberdeen.
     * @version 4.0
     * 
     */
	public class OrthographyProcessor : NLGModule
	{

		private bool commaSepPremodifiers; // set whether to separate premodifiers using commas

		private bool commaSepCuephrase; // set whether to include a comma after a cue phrase (if marked by the CUE_PHRASE=true) feature.

		public override void initialise()
		{
			commaSepPremodifiers = true;
			commaSepCuephrase = false;
		}

	    /**
	     * Set whether to separate premodifiers using a comma. If <code>true</code>,
	     * premodifiers will be comma-separated, as in <i>the long, dark road</i>.
	     * If <code>false</code>, they won't.
	     * 
	     * @param commaSepPremodifiers
	     *            the commaSepPremodifiers to set
	     */
		public virtual bool CommaSepPremodifiers
		{
			get
			{
				return commaSepPremodifiers;
			}
			set
			{
				commaSepPremodifiers = value;
			}
		}

	    /**
	     * If set to <code>true</code>, separates a cue phrase from the matrix
	     * phrase using a comma. Cue phrases are typically at the start of a
	     * sentence (e.g. <i><u>However</u>, John left early</i>). This will only
	     * apply to phrases with the feature
	     * {@link simplenlg.features.DiscourseFunction#CUE_PHRASE} or {@link simplenlg.features.DiscourseFunction#FRONT_MODIFIER}.
	     * 
	     * @param commaSepCuephrase
	     *            whether to separate cue phrases using a comma
	     */
		public virtual bool CommaSepCuephrase
		{
			get
			{
				return commaSepCuephrase;
			}
			set
			{
				commaSepCuephrase = value;
			}
		}



		public override NLGElement realise(NLGElement element)
		{
			NLGElement realisedElement = null;
			object function = null; //the element's discourse function

		    //get the element's function first
			if (element is ListElement)
			{
				IList<NLGElement> children = element.Children;
				if (children.Any())
				{
					NLGElement firstChild = children[0];
					function = firstChild.getFeature(InternalFeature.DISCOURSE_FUNCTION);
				}
			}
			else
			{
				if (element != null)
				{
					function = element.getFeature(InternalFeature.DISCOURSE_FUNCTION);
				}
			}

			if (element != null)
			{
				ElementCategory category = element.Category;

				if (category is DocumentCategory && element is DocumentElement)
				{
					IList<NLGElement> components = ((DocumentElement) element).Components;

					switch (((DocumentCategory) category).GetDocumentCategory())
					{

					    case DocumentCategory.DocumentCategoryEnum.SENTENCE:
						    realisedElement = realiseSentence(components, element);
						    break;

					    case DocumentCategory.DocumentCategoryEnum.LIST_ITEM :
						    if (components != null && components.Any())
						    {
						    // recursively realise whatever is in the list item
						    // NB: this will realise embedded lists within list items
							    realisedElement = new ListElement(realise(components));
							    realisedElement.Parent = element.Parent;
						    }
						    break;

					    default :
						    ((DocumentElement) element).Components = realise(components);
						    realisedElement = element;
					    break;
					}

				}
				else if (element is ListElement)
				{
				    // AG: changes here: if we have a premodifier, then we ask the
				    // realiseList method to separate with a comma.
				    // if it's a postmod, we need commas at the start and end only
				    // if it's appositive
					StringBuilder buffer = new StringBuilder();

					if (DiscourseFunction.PRE_MODIFIER.Equals(function))
					{

						bool all_appositives = true;
						foreach (NLGElement child in element.Children)
						{
							all_appositives = all_appositives && child.getFeatureAsBoolean(Feature.APPOSITIVE);
						}

					    // TODO: unless this is the end of the sentence
						if (all_appositives)
						{
							buffer.Append(", ");
						}
						realiseList(buffer, element.Children, commaSepPremodifiers ? "," : "");
						if (all_appositives)
						{
							buffer.Append(", ");
						}
					}
					else if (DiscourseFunction.POST_MODIFIER.Equals(function))
					{ // && appositive)

						IList<NLGElement> postmods = element.Children;
					    // bug fix due to Owen Bennett
						int len = postmods.Count;

						for (int i = 0; i < len; i++)
						{
						    // for(NLGElement postmod: element.getChildren()) {
							NLGElement postmod = postmods[i];

						    // if the postmod is appositive, it's sandwiched in commas
							if (postmod.getFeatureAsBoolean(Feature.APPOSITIVE))
							{
								buffer.Append(", ");
								buffer.Append(realise(postmod));
								buffer.Append(", ");
							}
							else
							{
								buffer.Append(realise(postmod));
								if (postmod is ListElement || (postmod.Realisation != null && !postmod.Realisation.Equals("")))
								{
									buffer.Append(" ");
								}
							}
						}

					}
					else if ((DiscourseFunction.CUE_PHRASE.Equals(function) || DiscourseFunction.FRONT_MODIFIER.Equals(function)) && commaSepCuephrase)
					{
						realiseList(buffer, element.Children, commaSepCuephrase ? "," : "");

					}
					else
					{
						realiseList(buffer, element.Children, "");
					}

				    // realiseList(buffer, element.getChildren(), "");
					realisedElement = new StringElement(buffer.ToString());

				}
				else if (element is CoordinatedPhraseElement)
				{
					realisedElement = realiseCoordinatedPhrase(element.Children);
				}
				else
				{
					realisedElement = element;
				}

			    // make the realised element inherit the original category
			    // essential if list items are to be properly formatted later
				if (realisedElement != null)
				{
					realisedElement.Category = category;
				}

			    //check if this is a cue phrase; if param is set, postfix a comma
				if ((DiscourseFunction.CUE_PHRASE.Equals(function) || DiscourseFunction.FRONT_MODIFIER.Equals(function)) && commaSepCuephrase)
				{
					string realisation = realisedElement.Realisation;

					if (!realisation.EndsWith(",", StringComparison.Ordinal))
					{
						realisation = realisation + ",";
					}

					realisedElement.Realisation = realisation;
				}
			    if (element.Capitalized)
			    {
			        string realisation = realisedElement.Realisation;
			        realisedElement.Realisation = realisation.Substring(0, 1).ToUpper() + realisation.Substring(1);
			    }
            }
		    //remove preceding and trailing whitespace from internal punctuation
			removePunctSpace(realisedElement);
			return realisedElement;
		}

	    /**
	     * removes extra spaces preceding punctuation from a realised element
	     * 
	     * @param realisedElement
	     */
		private void removePunctSpace(NLGElement realisedElement)
		{

			if (realisedElement != null)
			{

				string realisation = realisedElement.Realisation;

				if (!ReferenceEquals(realisation, null))
				{
					realisation = realisation.Replace(" ,", ",");
					realisation = Regex.Replace(realisation,",,+", ",");
					realisedElement.Realisation = realisation;
				}

			}
		}

	    /**
	     * Performs the realisation on a sentence. This includes adding the
	     * terminator and capitalising the first letter.
	     * 
	     * @param components
	     *            the <code>List</code> of <code>NLGElement</code>s representing
	     *            the components that make up the sentence.
	     * @param element
	     *            the <code>NLGElement</code> representing the sentence.
	     * @return the realised element as an <code>NLGElement</code>.
	     */
		private NLGElement realiseSentence(IList<NLGElement> components, NLGElement element)
		{

			NLGElement realisedElement = null;
			if (components != null && components.Any())
			{
				StringBuilder realisation = new StringBuilder();
				realiseList(realisation, components, "");

				stripLeadingCommas(realisation);
				capitaliseFirstLetter(realisation);
				terminateSentence(realisation, element.getFeatureAsBoolean(InternalFeature.INTERROGATIVE));

				((DocumentElement) element).clearComponents();
			    // realisation.append(' ');
				element.Realisation = realisation.ToString();
				realisedElement = element;
			}

			return realisedElement;
		}

	    /**
	     * Adds the sentence terminator to the sentence. This is a period ('.') for
	     * normal sentences or a question mark ('?') for interrogatives.
	     * 
	     * @param realisation
	     *            the <code>StringBuffer<code> containing the current 
	     * realisation of the sentence.
	     * @param interrogative
	     *            a <code>boolean</code> flag showing <code>true</code> if the
	     *            sentence is an interrogative, <code>false</code> otherwise.
	     */
		private void terminateSentence(StringBuilder realisation, bool interrogative)
		{
			char character = realisation[realisation.Length - 1];
			if (character != '.' && character != '?')
			{
				if (interrogative)
				{
					realisation.Append('?');
				}
				else
				{
					realisation.Append('.');
				}
			}
		}

	    /**
	     * Remove recursively any leading spaces or commas at the start 
	     * of a sentence.
	     * 
	     * @param realisation
	     *            the <code>StringBuffer<code> containing the current 
	     * realisation of the sentence.
	     */
		private void stripLeadingCommas(StringBuilder realisation)
		{
			char character = realisation[0];
			if (character == ' ' || character == ',')
			{
				realisation.Remove(0, 1);
				stripLeadingCommas(realisation);
			}
		}


	    /**
	     * Capitalises the first character of a sentence if it is a lower case
	     * letter.
	     * 
	     * @param realisation
	     *            the <code>StringBuffer<code> containing the current 
	     * realisation of the sentence.
	     */
		private void capitaliseFirstLetter(StringBuilder realisation)
		{
			char character = realisation[0];
			if (character >= 'a' && character <= 'z')
			{
				character = (char)('A' + (character - 'a'));
				realisation[0] = character;
			}
		}

		public override IList<NLGElement> realise(IList<NLGElement> elements)
		{
			IList<NLGElement> realisedList = new List<NLGElement>();

			if (elements != null && elements.Any())
			{
				foreach (NLGElement eachElement in elements)
				{
					if (eachElement is DocumentElement)
					{
						realisedList.Add(realise(eachElement));
					}
					else
					{
						realisedList.Add(eachElement);
					}
				}
			}
			return realisedList;
		}

	    /**
	     * Realises a list of elements appending the result to the on-going
	     * realisation.
	     * 
	     * @param realisation
	     *            the <code>StringBuffer<code> containing the current 
	     * 			  realisation of the sentence.
	     * @param components
	     *            the <code>List</code> of <code>NLGElement</code>s representing
	     *            the components that make up the sentence.
	     * @param listSeparator
	     *            the string to use to separate elements of the list, empty if
	     *            no separator needed
	     */
		private void realiseList(StringBuilder realisation, IList<NLGElement> components, string listSeparator)
		{

			NLGElement realisedChild = null;

			for (int i = 0; i < components.Count; i++)
			{
				NLGElement thisElement = components[i];
				realisedChild = realise(thisElement);
				string childRealisation = realisedChild.Realisation;

			    // check that the child realisation is non-empty
				if (!ReferenceEquals(childRealisation, null) && childRealisation.Length > 0 && !Regex.IsMatch(childRealisation,"^[\\s\\n]+$"))
				{
					realisation.Append(realisedChild.Realisation);

					if (components.Count > 1 && i < components.Count - 1)
					{
						realisation.Append(listSeparator);
					}

					realisation.Append(' ');
				}
			}

			if (realisation.Length > 0)
			{
				realisation.Length = realisation.Length - 1;
			}
		}

	    /**
	     * Realises coordinated phrases. Where there are more than two coordinates,
	     * then a comma replaces the conjunction word between all the coordinates
	     * save the last two. For example, <em>John and Peter and Simon</em> becomes
	     * <em>John, Peter and Simon</em>.
	     * 
	     * @param components
	     *            the <code>List</code> of <code>NLGElement</code>s representing
	     *            the components that make up the sentence.
	     * @return the realised element as an <code>NLGElement</code>.
	     */
		private NLGElement realiseCoordinatedPhrase(IList<NLGElement> components)
		{
			StringBuilder realisation = new StringBuilder();
			NLGElement realisedChild = null;

			int length = components.Count;

			for (int index = 0; index < length; index++)
			{
				realisedChild = components[index];
				if (index < length - 2 && DiscourseFunction.CONJUNCTION.Equals(realisedChild.getFeature(InternalFeature.DISCOURSE_FUNCTION)))
				{

					realisation.Append(", "); //$NON-NLS-1$
				}
				else
				{
					realisedChild = realise(realisedChild);
					realisation.Append(realisedChild.Realisation).Append(' ');
				}
			}
			realisation.Length = realisation.Length - 1;
			return new StringElement(realisation.ToString().Replace(" ,", ",")); //$NON-NLS-1$ //$NON-NLS-2$
		}
	}

}