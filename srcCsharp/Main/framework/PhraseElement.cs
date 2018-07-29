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

namespace SimpleNLG.Main.framework
{

	using ClauseStatus = features.ClauseStatus;
	using DiscourseFunction = features.DiscourseFunction;
	using Feature = features.Feature;
	using InternalFeature = features.InternalFeature;

    /**
     * <p>
     * This class defines a phrase. It covers the expected phrase types: noun
     * phrases, verb phrases, adjective phrases, adverb phrases and prepositional
     * phrases as well as also covering clauses. Phrases can be constructed from
     * scratch by setting the correct features of the phrase elements. However, it
     * is strongly recommended that the <code>PhraseFactory</code> is used to
     * construct phrases.
     * </p>
     * 
     * 
     * @author D. Westwater, University of Aberdeen.
     * @version 4.0
     * 
     */
	public class PhraseElement : NLGElement
	{
	    /**
	     * Creates a new phrase of the given type.
	     * 
	     * @param newCategory
	     *            the <code>PhraseCategory</code> type for this phrase.
	     */
		public PhraseElement(PhraseCategory newCategory)
		{
			Category = newCategory;

		    // set default feature value
			setFeature(Feature.ELIDED, false);
		}

	    /**
	     * <p>
	     * This method retrieves the child components of this phrase. The list
	     * returned will depend on the category of the element.<br>
	     * <ul>
	     * <li>Clauses consist of cue phrases, front modifiers, pre-modifiers,
	     * subjects, verb phrases and complements.</li>
	     * <li>Noun phrases consist of the specifier, pre-modifiers, the noun
	     * subjects, complements and post-modifiers.</li>
	     * <li>Verb phrases consist of pre-modifiers, the verb group, complements
	     * and post-modifiers.</li>
	     * <li>Canned text phrases have no children thus an empty list will be
	     * returned.</li>
	     * <li>All the other phrases consist of pre-modifiers, the main phrase
	     * element, complements and post-modifiers.</li>
	     * </ul>
	     * </p>
	     * 
	     * @return a <code>List</code> of <code>NLGElement</code>s representing the
	     *         child elements of this phrase.
	     */
		public override IList<NLGElement> Children
		{
			get
			{
				IList<NLGElement> children = new List<NLGElement>();
				ElementCategory category = Category;
				NLGElement currentElement = null;
    
				if (category is PhraseCategory)
				{
					switch (((PhraseCategory) category).GetPhraseCategory())
					{
					    case PhraseCategory.PhraseCategoryEnum.CLAUSE:
						    currentElement = getFeatureAsElement(Feature.CUE_PHRASE);
						    if (currentElement != null)
						    {
							    children.Add(currentElement);
						    }
						    ((List<NLGElement>)children).AddRange(getFeatureAsElementList(InternalFeature.FRONT_MODIFIERS));
						    ((List<NLGElement>)children).AddRange(getFeatureAsElementList(InternalFeature.PREMODIFIERS));
						    ((List<NLGElement>)children).AddRange(getFeatureAsElementList(InternalFeature.SUBJECTS));
						    ((List<NLGElement>)children).AddRange(getFeatureAsElementList(InternalFeature.VERB_PHRASE));
						    ((List<NLGElement>)children).AddRange(getFeatureAsElementList(InternalFeature.COMPLEMENTS));
						    break;
        
					    case PhraseCategory.PhraseCategoryEnum.NOUN_PHRASE:
						    currentElement = getFeatureAsElement(InternalFeature.SPECIFIER);
						    if (currentElement != null)
						    {
							    children.Add(currentElement);
						    }
						    ((List<NLGElement>)children).AddRange(getFeatureAsElementList(InternalFeature.PREMODIFIERS));
						    currentElement = getHead();
						    if (currentElement != null)
						    {
							    children.Add(currentElement);
						    }
						    ((List<NLGElement>)children).AddRange(getFeatureAsElementList(InternalFeature.COMPLEMENTS));
						    ((List<NLGElement>)children).AddRange(getFeatureAsElementList(InternalFeature.POSTMODIFIERS));
						    break;
        
					    case PhraseCategory.PhraseCategoryEnum.VERB_PHRASE:
						    ((List<NLGElement>)children).AddRange(getFeatureAsElementList(InternalFeature.PREMODIFIERS));
						    currentElement = getHead();
						    if (currentElement != null)
						    {
							    children.Add(currentElement);
						    }
						    ((List<NLGElement>)children).AddRange(getFeatureAsElementList(InternalFeature.COMPLEMENTS));
						    ((List<NLGElement>)children).AddRange(getFeatureAsElementList(InternalFeature.POSTMODIFIERS));
						    break;
        
					    case PhraseCategory.PhraseCategoryEnum.CANNED_TEXT:
				            // Do nothing
						    break;
        
					    default:
						    ((List<NLGElement>)children).AddRange(getFeatureAsElementList(InternalFeature.PREMODIFIERS));
						    currentElement = getHead();
						    if (currentElement != null)
						    {
							    children.Add(currentElement);
						    }
						    ((List<NLGElement>)children).AddRange(getFeatureAsElementList(InternalFeature.COMPLEMENTS));
						    ((List<NLGElement>)children).AddRange(getFeatureAsElementList(InternalFeature.POSTMODIFIERS));
						    break;
					}
				}
				return children;
			}
		}
	    /**
	     * Sets the head, or main component, of this current phrase. For example,
	     * the head for a verb phrase should be a verb while the head of a noun
	     * phrase should be a noun. <code>String</code>s are converted to
	     * <code>StringElement</code>s. If <code>null</code> is passed in as the new
	     * head then the head feature is removed.
	     * 
	     * @param newHead
	     *            the new value for the head of this phrase.
	     */
		public virtual void setHead(object newHead)
		{
			if (newHead == null)
			{
				removeFeature(InternalFeature.HEAD);
				return;
			}
			NLGElement headElement;
			if (newHead is NLGElement)
			{
				headElement = (NLGElement) newHead;
			}
			else
			{
				headElement = new StringElement(newHead.ToString());
			}

			setFeature(InternalFeature.HEAD, headElement);

		}

	    /**
	     * Retrieves the current head of this phrase.
	     * 
	     * @return the <code>NLGElement</code> representing the head.
	     */
		public virtual NLGElement getHead()
		{
			return getFeatureAsElement(InternalFeature.HEAD);
		}

	    /**
	     * <p>
	     * Adds a new complement to the phrase element. Complements will be realised
	     * in the syntax after the head element of the phrase. Complements differ
	     * from post-modifiers in that complements are crucial to the understanding
	     * of a phrase whereas post-modifiers are optional.
	     * </p>
	     * 
	     * <p>
	     * If the new complement being added is a <em>clause</em> or a
	     * <code>CoordinatedPhraseElement</code> then its clause status feature is
	     * set to <code>ClauseStatus.SUBORDINATE</code> and it's discourse function
	     * is set to <code>DiscourseFunction.OBJECT</code> by default unless an
	     * existing discourse function exists on the complement.
	     * </p>
	     * 
	     * <p>
	     * Complements can have different functions. For example, the phrase <I>John
	     * gave Mary a flower</I> has two complements, one a direct object and one
	     * indirect. If a complement is not specified for its discourse function,
	     * then this is automatically set to <code>DiscourseFunction.OBJECT</code>.
	     * </p>
	     * 
	     * @param newComplement
	     *            the new complement as an <code>NLGElement</code>.
	     */
		public virtual void addComplement(NLGElement newComplement)
		{
			IList<NLGElement> complements = getFeatureAsElementList(InternalFeature.COMPLEMENTS);
			if (complements == null)
			{
				complements = new List<NLGElement>();
			}

		    // check if the new complement has a discourse function; if not, assume object
			if (!newComplement.hasFeature(InternalFeature.DISCOURSE_FUNCTION))
			{
				newComplement.setFeature(InternalFeature.DISCOURSE_FUNCTION, DiscourseFunction.OBJECT);
			}

			complements.Add(newComplement);
			setFeature(InternalFeature.COMPLEMENTS, complements);
			if (newComplement.isA(new PhraseCategory(PhraseCategory.PhraseCategoryEnum.CLAUSE)) || newComplement is CoordinatedPhraseElement)
			{
				newComplement.setFeature(InternalFeature.CLAUSE_STATUS, ClauseStatus.SUBORDINATE);

				if (!newComplement.hasFeature(InternalFeature.DISCOURSE_FUNCTION))
				{
					newComplement.setFeature(InternalFeature.DISCOURSE_FUNCTION, DiscourseFunction.OBJECT);
				}
			}
		}


	    /**
	     * <p>
	     * Sets a complement of the phrase element. If a complement already exists
	     * of the same DISCOURSE_FUNCTION, it is removed. This replaces complements
	     * set earlier via {@link #addComplement(NLGElement)}
	     * </p>
	     * 
	     * @param newComplement
	     *            the new complement as an <code>NLGElement</code>.
	     */
		public virtual NLGElement Complement
		{
			set
			{
				DiscourseFunction? function = (DiscourseFunction?) value.getFeature(InternalFeature.DISCOURSE_FUNCTION);
				removeComplements(function);
				addComplement(value);
			}
		}
	    /**
	     * remove complements of the specified DiscourseFunction
	     * 
	     * @param function
	     */
		private void removeComplements(DiscourseFunction? function)
		{
			IList<NLGElement> complements = getFeatureAsElementList(InternalFeature.COMPLEMENTS);
			if (function == null || complements == null)
			{
				return;
			}
			IList<NLGElement> complementsToRemove = new List<NLGElement>();
			foreach (NLGElement complement in complements)
			{
				if (function == (DiscourseFunction) complement.getFeature(InternalFeature.DISCOURSE_FUNCTION))
				{
					complementsToRemove.Add(complement);
				}
			}

			if (complementsToRemove.Any())
			{
			    foreach (NLGElement item in complementsToRemove) { complements.Remove(item); }
				setFeature(InternalFeature.COMPLEMENTS, complements);
			}

			return;
		}

	    /**
	     * <p>
	     * Adds a new complement to the phrase element. Complements will be realised
	     * in the syntax after the head element of the phrase. Complements differ
	     * from post-modifiers in that complements are crucial to the understanding
	     * of a phrase whereas post-modifiers are optional.
	     * </p>
	     * 
	     * @param newComplement
	     *            the new complement as a <code>String</code>. It is used to
	     *            create a <code>StringElement</code>.
	     */
		public virtual void addComplement(string newComplement)
		{
			StringElement newElement = new StringElement(newComplement);
			IList<NLGElement> complements = getFeatureAsElementList(InternalFeature.COMPLEMENTS);
			if (complements == null)
			{
				complements = new List<NLGElement>();
			}
			complements.Add(newElement);
			setFeature(InternalFeature.COMPLEMENTS, complements);
		}

	    /**
	     * <p>
	     * Sets the complement to the phrase element. This replaces any complements
	     * set earlier.
	     * </p>
	     * 
	     * @param newComplement
	     *            the new complement as a <code>String</code>. It is used to
	     *            create a <code>StringElement</code>.
	     */
		public virtual void setComplement(string value)
		{
			setFeature(InternalFeature.COMPLEMENTS, null);
			addComplement(value);
		}

	    /**
	     * Adds a new post-modifier to the phrase element. Post-modifiers will be
	     * realised in the syntax after the complements.
	     * 
	     * @param newPostModifier
	     *            the new post-modifier as an <code>NLGElement</code>.
	     */
		public virtual void addPostModifier(NLGElement newPostModifier)
		{
			IList<NLGElement> postModifiers = getFeatureAsElementList(InternalFeature.POSTMODIFIERS);
			if (postModifiers == null)
			{
				postModifiers = new List<NLGElement>();
			}
			newPostModifier.setFeature(InternalFeature.DISCOURSE_FUNCTION, DiscourseFunction.POST_MODIFIER);
			postModifiers.Add(newPostModifier);
			setFeature(InternalFeature.POSTMODIFIERS, postModifiers);
		}

	    /**
	     * Adds a new post-modifier to the phrase element. Post-modifiers will be
	     * realised in the syntax after the complements.
	     * 
	     * @param newPostModifier
	     *            the new post-modifier as a <code>String</code>. It is used to
	     *            create a <code>StringElement</code>.
	     */
		public virtual void addPostModifier(string newPostModifier)
		{
			IList<NLGElement> postModifiers = getFeatureAsElementList(InternalFeature.POSTMODIFIERS);
			if (postModifiers == null)
			{
				postModifiers = new List<NLGElement>();
			}
			postModifiers.Add(new StringElement(newPostModifier));
			setFeature(InternalFeature.POSTMODIFIERS, postModifiers);
		}

	    /**
	     * Set the postmodifier for this phrase. This resets all previous
	     * postmodifiers to <code>null</code> and replaces them with the given
	     * string.
	     * 
	     * @param newPostModifier
	     *            the postmodifier
	     */
		public virtual void setPostModifier(string value)
		{
		
			setFeature(InternalFeature.POSTMODIFIERS, null);
			addPostModifier(value);
		}

	    /**
	     * Set the postmodifier for this phrase. This resets all previous
	     * postmodifiers to <code>null</code> and replaces them with the given
	     * string.
	     * 
	     * @param newPostModifier
	     *            the postmodifier
	     */
		public virtual void setPostModifier(NLGElement value)
		{
			setFeature(InternalFeature.POSTMODIFIERS, null);
			addPostModifier(value);
		}

	    /**
	     * Adds a new front modifier to the phrase element.
	     * 
	     * @param newFrontModifier
	     *            the new front modifier as an <code>NLGElement</code>.
	     */
		public virtual void addFrontModifier(NLGElement newFrontModifier)
		{
			IList<NLGElement> frontModifiers = getFeatureAsElementList(InternalFeature.FRONT_MODIFIERS);
			if (frontModifiers == null)
			{
				frontModifiers = new List<NLGElement>();
			}
			frontModifiers.Add(newFrontModifier);
			setFeature(InternalFeature.FRONT_MODIFIERS, frontModifiers);
		}

	    /**
	     * Adds a new front modifier to the phrase element.
	     * 
	     * @param newFrontModifier
	     *            the new front modifier as a <code>String</code>. It is used to
	     *            create a <code>StringElement</code>.
	     */
		public virtual void addFrontModifier(string newFrontModifier)
		{
			IList<NLGElement> frontModifiers = getFeatureAsElementList(InternalFeature.FRONT_MODIFIERS);

			if (frontModifiers == null)
			{
				frontModifiers = new List<NLGElement>();
			}

			frontModifiers.Add(new StringElement(newFrontModifier));
			setFeature(InternalFeature.FRONT_MODIFIERS, frontModifiers);
		}

	    /**
	     * Set the frontmodifier for this phrase. This resets all previous front
	     * modifiers to <code>null</code> and replaces them with the given string.
	     * 
	     * @param newFrontModifier
	     *            the front modifier
	     */
		public virtual void setFrontModifier(string value)
		{
			setFeature(InternalFeature.FRONT_MODIFIERS, null);
			addFrontModifier(value);
		}

	    /**
	     * Set the front modifier for this phrase. This resets all previous front
	     * modifiers to <code>null</code> and replaces them with the given string.
	     * 
	     * @param newFrontModifier
	     *            the front modifier
	     */
		public virtual void setFrontModifier(NLGElement value)
		{
			setFeature(InternalFeature.FRONT_MODIFIERS, null);
			addFrontModifier(value);
		}

	    /**
	     * Adds a new pre-modifier to the phrase element.
	     * 
	     * @param newPreModifier
	     *            the new pre-modifier as an <code>NLGElement</code>.
	     */
		public virtual void addPreModifier(NLGElement newPreModifier)
		{
			IList<NLGElement> preModifiers = getFeatureAsElementList(InternalFeature.PREMODIFIERS);
			if (preModifiers == null)
			{
				preModifiers = new List<NLGElement>();
			}
			preModifiers.Add(newPreModifier);
			setFeature(InternalFeature.PREMODIFIERS, preModifiers);
		}

	    /**
	     * Adds a new pre-modifier to the phrase element.
	     * 
	     * @param newPreModifier
	     *            the new pre-modifier as a <code>String</code>. It is used to
	     *            create a <code>StringElement</code>.
	     */
		public virtual void addPreModifier(string newPreModifier)
		{
			addPreModifier(new StringElement(newPreModifier));
		}

	    /**
	     * Set the premodifier for this phrase. This resets all previous
	     * premodifiers to <code>null</code> and replaces them with the given
	     * string.
	     * 
	     * @param newPreModifier
	     *            the premodifier
	     */
		public virtual void setPreModifier(string value)
		{
			setFeature(InternalFeature.PREMODIFIERS, null);
			addPreModifier(value);
		}

	    /**
	     * Set the premodifier for this phrase. This resets all previous
	     * premodifiers to <code>null</code> and replaces them with the given
	     * string.
	     * 
	     * @param newPreModifier
	     *            the premodifier
	     */
		public virtual void setPreModifier(NLGElement value)
		{
			setFeature(InternalFeature.PREMODIFIERS, null);
			addPreModifier(value);
		}

	    /**
	     * Add a modifier to a phrase Use heuristics to decide where it goes
	     * 
	     * @param modifier
	     */
		public virtual void addModifier(object modifier)
		{
		    // default addModifier - always make modifier a preModifier
			if (modifier == null)
			{
				return;
			}

		    // assume is preModifier, add in appropriate form
			if (modifier is NLGElement)
			{
				addPreModifier((NLGElement) modifier);
			}
			else
			{
				addPreModifier((string) modifier);
			}
			return;
		}

	    /**
	     * Retrieves the current list of pre-modifiers for the phrase.
	     * 
	     * @return a <code>List</code> of <code>NLGElement</code>s.
	     */
		public virtual IList<NLGElement> PreModifiers
		{
			get
			{
				return getFeatureAsElementList(InternalFeature.PREMODIFIERS);
			}
		}
	    /**
	     * Retrieves the current list of post modifiers for the phrase.
	     * 
	     * @return a <code>List</code> of <code>NLGElement</code>s.
	     */
		public virtual IList<NLGElement> PostModifiers
		{
			get
			{
				return getFeatureAsElementList(InternalFeature.POSTMODIFIERS);
			}
		}
	    /**
	     * Retrieves the current list of frony modifiers for the phrase.
	     * 
	     * @return a <code>List</code> of <code>NLGElement</code>s.
	     */
		public virtual IList<NLGElement> FrontModifiers
		{
			get
			{
				return getFeatureAsElementList(InternalFeature.FRONT_MODIFIERS);
			}
		}

		public override string printTree(string indent)
		{
			string thisIndent = ReferenceEquals(indent, null) ? " |-" : indent + " |-"; //$NON-NLS-1$ //$NON-NLS-2$
			string childIndent = ReferenceEquals(indent, null) ? " | " : indent + " | "; //$NON-NLS-1$ //$NON-NLS-2$
			string lastIndent = ReferenceEquals(indent, null) ? " \\-" : indent + " \\-"; //$NON-NLS-1$ //$NON-NLS-2$
			string lastChildIndent = ReferenceEquals(indent, null) ? "   " : indent + "   "; //$NON-NLS-1$ //$NON-NLS-2$
			StringBuilder print = new StringBuilder();
			print.Append("PhraseElement: category=").Append(Category.ToString()).Append(", features="); //$NON-NLS-1$ - $NON-NLS-1$

			print.Append(AllFeatures.ToStringNLG()).Append("\n");
			IList<NLGElement> children = Children;
			int length = children.Count - 1;
			int index = 0;

			for (index = 0; index < length; index++)
			{
				print.Append(thisIndent).Append(children[index].printTree(childIndent));
			}
			if (length >= 0)
			{
				print.Append(lastIndent).Append(children[length].printTree(lastChildIndent));
			}
			return print.ToString();
		}

	    /**
	     * Removes all existing complements on the phrase.
	     */
		public virtual void clearComplements()
		{
			removeFeature(InternalFeature.COMPLEMENTS);
		}

	    /**
	     * Sets the determiner for the phrase. This only has real meaning on noun
	     * phrases and is added here for convenience. Determiners are some times
	     * referred to as specifiers.
	     * 
	     * @param newDeterminer
	     *            the new determiner for the phrase.
	     * @deprecated Use {@link NPPhraseSpec#setSpecifier(Object)} directly
	     */
		[Obsolete("use setDeterminer instead",true)]
		public virtual object Determiner
		{
			set
			{
				NLGFactory factory = new NLGFactory();
				NLGElement determinerElement = factory.createWord(value, new LexicalCategory(LexicalCategory.LexicalCategoryEnum.DETERMINER));
    
				if (determinerElement != null)
				{
					determinerElement.setFeature(InternalFeature.DISCOURSE_FUNCTION, DiscourseFunction.SPECIFIER);
					setFeature(InternalFeature.SPECIFIER, determinerElement);
					determinerElement.Parent = this;
				}
			}
		}
	}

}