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

namespace SimpleNLG.Main.phrasespec
{

	using DiscourseFunction = features.DiscourseFunction;
	using Feature = features.Feature;
	using Form = features.Form;
	using InternalFeature = features.InternalFeature;
	using Person = features.Person;
	using Tense = features.Tense;
	using CoordinatedPhraseElement = framework.CoordinatedPhraseElement;
	using InflectedWordElement = framework.InflectedWordElement;
	using LexicalCategory = framework.LexicalCategory;
	using NLGElement = framework.NLGElement;
	using PhraseCategory = framework.PhraseCategory;
	using PhraseElement = framework.PhraseElement;
	using NLGFactory = framework.NLGFactory;
	using WordElement = framework.WordElement;
	
    /**
     * <p>
     * This class defines a verb phrase.  It is essentially
     * a wrapper around the <code>PhraseElement</code> class, with methods
     * for setting common constituents such as Objects.
     * For example, the <code>setVerb</code> method in this class sets
     * the head of the element to be the specified verb
     *
     * From an API perspective, this class is a simplified version of the SPhraseSpec
     * class in simplenlg V3.  It provides an alternative way for creating syntactic
     * structures, compared to directly manipulating a V4 <code>PhraseElement</code>.
     * 
     * Methods are provided for setting and getting the following constituents: 
     * <UL>
     * <LI>PreModifier		(eg, "reluctantly")
     * <LI>Verb				(eg, "gave")
     * <LI>IndirectObject	(eg, "Mary")
     * <LI>Object	        (eg, "an apple")
     * <LI>PostModifier     (eg, "before school")
     * </UL>
     * 
     * NOTE: If there is a complex verb group, a preModifer set at the VP level appears before
     * the verb, while a preModifier set at the clause level appears before the verb group.  Eg
     *   "Mary unfortunately will eat the apple"  ("unfortunately" is clause preModifier)
     *   "Mary will happily eat the apple"  ("happily" is VP preModifier)
     *   
     * NOTE: The setModifier method will attempt to automatically determine whether
     * a modifier should be expressed as a PreModifier or PostModifier
     * 
     * Features (such as negated) must be accessed via the <code>setFeature</code> and
     * <code>getFeature</code> methods (inherited from <code>NLGElement</code>).
     * Features which are often set on VPPhraseSpec include
     * <UL>
     * <LI>Modal    (eg, "John eats an apple" vs "John can eat an apple")
     * <LI>Negated  (eg, "John eats an apple" vs "John does not eat an apple")
     * <LI>Passive  (eg, "John eats an apple" vs "An apple is eaten by John")
     * <LI>Perfect  (eg, "John ate an apple" vs "John has eaten an apple")
     * <LI>Progressive  (eg, "John eats an apple" vs "John is eating an apple")
     * <LI>Tense    (eg, "John ate" vs "John eats" vs "John will eat")
     * </UL>
     * Note that most VP features can be set on an SPhraseSpec, they will automatically
     * be propogated to the VP
     * 
     * <code>VPPhraseSpec</code> are produced by the <code>createVerbPhrase</code>
     * method of a <code>PhraseFactory</code>
     * </p>
     * 
     * @author E. Reiter, University of Aberdeen.
     * @version 4.1
     * 
     */
	public class VPPhraseSpec : PhraseElement
	{
	
	    /** create an empty clause
	     */
		public VPPhraseSpec(NLGFactory phraseFactory) : base(new PhraseCategory(PhraseCategory.PhraseCategoryEnum.VERB_PHRASE))
		{
			Factory = phraseFactory;

		    // set default feature values
			setFeature(Feature.PERFECT, false);
			setFeature(Feature.PROGRESSIVE, false);
			setFeature(Feature.PASSIVE, false);
			setFeature(Feature.NEGATED, false);
			setFeature(Feature.TENSE, Tense.PRESENT);
			setFeature(Feature.PERSON, Person.THIRD);
			Plural = false;
			setFeature(Feature.FORM, Form.NORMAL);
			setFeature(InternalFeature.REALISE_AUXILIARY, true);
			setFeature(Feature.IS_CAPITALIZED, false);
		}

	    /** sets the verb (head) of a verb phrase.
	     * Extract particle from verb if necessary
	     * @param verb
	     */
		public virtual void setVerb(object verb)
		{
			NLGElement verbElement;

			if (verb is string)
			{ // if String given, check for particle
				int space = ((string) verb).IndexOf(' ');

				if (space == -1)
				{ // no space, so no particle
					verbElement = Factory.createWord(verb, new LexicalCategory(LexicalCategory.LexicalCategoryEnum.VERB));

				}
				else
				{ // space, so break up into verb and particle
					verbElement = Factory.createWord(((string) verb).Substring(0, space), new LexicalCategory(LexicalCategory.LexicalCategoryEnum.VERB));
					setFeature(Feature.PARTICLE, ((string) verb).Substring(space + 1, (((string) verb).Length) - (space + 1)));
				}
			}
			else
			{ // Object is not a String
				verbElement = Factory.createNLGElement(verb, new LexicalCategory(LexicalCategory.LexicalCategoryEnum.VERB));
			}
			setHead(verbElement);
		}


	    /**
	     * @return verb (head) of verb phrase
	     */
		public virtual NLGElement getVerb()
		{
			return getHead();
		}

	    /** Sets the direct object of a clause  (assumes this is the only direct object)
	     *
	     * @param object
	     */
		public virtual void setObject(object @object)
		{
			NLGElement objectPhrase;
			if (@object is PhraseElement || @object is CoordinatedPhraseElement)
			{
				objectPhrase = (NLGElement) @object;
			}
			else
			{
				objectPhrase = Factory.createNounPhrase(@object);
			}

			objectPhrase.setFeature(InternalFeature.DISCOURSE_FUNCTION, DiscourseFunction.OBJECT);
			Complement = objectPhrase;
		}


	    /** Returns the direct object of a clause (assumes there is only one)
	     * 
	     * @return subject of clause (assume only one)
	     */
		public virtual NLGElement getObject()
		{
			IList<NLGElement> complements = getFeatureAsElementList(InternalFeature.COMPLEMENTS);
			foreach (NLGElement complement in complements)
			{
				if ((DiscourseFunction)complement.getFeature(InternalFeature.DISCOURSE_FUNCTION) == DiscourseFunction.OBJECT)
				{
					return complement;
				}
			}
			return null;
		}

	    /** Set the indirect object of a clause (assumes this is the only direct indirect object)
	     *
	     * @param indirectObject
	     */
		public virtual void setIndirectObject(object indirectObject)
		{
			NLGElement indirectObjectPhrase;
			if (indirectObject is PhraseElement || indirectObject is CoordinatedPhraseElement)
			{
				indirectObjectPhrase = (NLGElement) indirectObject;
			}
			else
			{
				indirectObjectPhrase = Factory.createNounPhrase(indirectObject);
			}

			indirectObjectPhrase.setFeature(InternalFeature.DISCOURSE_FUNCTION, DiscourseFunction.INDIRECT_OBJECT);
			Complement = indirectObjectPhrase;
		}

	    /** Returns the indirect object of a clause (assumes there is only one)
	     * 
	     * @return subject of clause (assume only one)
	     */
		public virtual NLGElement getIndirectObject()
		{
			IList<NLGElement> complements = getFeatureAsElementList(InternalFeature.COMPLEMENTS);
			foreach (NLGElement complement in complements)
			{
				if ((DiscourseFunction)complement.getFeature(InternalFeature.DISCOURSE_FUNCTION) == DiscourseFunction.INDIRECT_OBJECT)
				{
					return complement;
				}
			}
			return null;
		}

	    // note that addFrontModifier, addPostModifier, addPreModifier are inherited from PhraseElement
	    // likewise getFrontModifiers, getPostModifiers, getPreModifiers

	    
	    /** Add a modifier to a verb phrase
	     * Use heuristics to decide where it goes
	     * @param modifier
	     */
		public override void addModifier(object modifier)
		{
		    // adverb is preModifier
		    // string which is one lexicographic word is looked up in lexicon,
		    // if it is an adverb than it becomes a preModifier
		    // Everything else is postModifier

			if (modifier == null)
			{
				return;
			}

		    // get modifier as NLGElement if possible
			NLGElement modifierElement = null;
			if (modifier is NLGElement)
			{
				modifierElement = (NLGElement) modifier;
			}
			else if (modifier is string)
			{
				string modifierString = (string)modifier;
				if (modifierString.Length > 0 && !modifierString.Contains(" "))
				{
					modifierElement = Factory.createWord(modifier, new LexicalCategory(LexicalCategory.LexicalCategoryEnum.ANY));
				}
			}

		    // if no modifier element, must be a complex string
			if (modifierElement == null)
			{
				addPostModifier((string)modifier);
				return;
			}

		    // extract WordElement if modifier is a single word
			WordElement modifierWord = null;
			if (modifierElement != null && modifierElement is WordElement)
			{
				modifierWord = (WordElement) modifierElement;
			}
			else if (modifierElement != null && modifierElement is InflectedWordElement)
			{
				modifierWord = ((InflectedWordElement) modifierElement).BaseWord;
			}

			if (modifierWord != null && modifierWord.Category == LexicalCategory.LexicalCategoryEnum.ADVERB)
			{
				addPreModifier(modifierWord);
				return;
			}

		    // default case
			addPostModifier(modifierElement);
		}


	}

}