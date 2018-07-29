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

namespace SimpleNLG.Main.phrasespec
{
	using DiscourseFunction = features.DiscourseFunction;
	using Feature = features.Feature;
	using Gender = features.Gender;
	using InternalFeature = features.InternalFeature;
	using LexicalFeature = features.LexicalFeature;
	using Person = features.Person;
	using InflectedWordElement = framework.InflectedWordElement;
	using LexicalCategory = framework.LexicalCategory;
	using NLGElement = framework.NLGElement;
	using PhraseCategory = framework.PhraseCategory;
	using PhraseElement = framework.PhraseElement;
	using NLGFactory = framework.NLGFactory;
	using WordElement = framework.WordElement;

    /**
     * <p>
     * This class defines a noun phrase. It is essentially a wrapper around the
     * <code>PhraseElement</code> class, with methods for setting common
     * constituents such as specifier. For example, the <code>setNoun</code> method
     * in this class sets the head of the element to be the specified noun
     * 
     * From an API perspective, this class is a simplified version of the
     * NPPhraseSpec class in simplenlg V3. It provides an alternative way for
     * creating syntactic structures, compared to directly manipulating a V4
     * <code>PhraseElement</code>.
     * 
     * Methods are provided for setting and getting the following constituents:
     * <UL>
     * <li>Specifier (eg, "the")
     * <LI>PreModifier (eg, "green")
     * <LI>Noun (eg, "apple")
     * <LI>PostModifier (eg, "in the shop")
     * </UL>
     * 
     * NOTE: The setModifier method will attempt to automatically determine whether
     * a modifier should be expressed as a PreModifier, or PostModifier
     * 
     * NOTE: Specifiers are currently pretty basic, this needs more development
     * 
     * Features (such as number) must be accessed via the <code>setFeature</code>
     * and <code>getFeature</code> methods (inherited from <code>NLGElement</code>).
     * Features which are often set on NPPhraseSpec include
     * <UL>
     * <LI>Number (eg, "the apple" vs "the apples")
     * <LI>Possessive (eg, "John" vs "John's")
     * <LI>Pronominal (eg, "the apple" vs "it")
     * </UL>
     * 
     * <code>NPPhraseSpec</code> are produced by the <code>createNounPhrase</code>
     * method of a <code>PhraseFactory</code>
     * </p>
     * @author E. Reiter, University of Aberdeen.
     * @version 4.1
     * 
     */
	public class NPPhraseSpec : PhraseElement
	{

		public NPPhraseSpec(NLGFactory phraseFactory) : base(new PhraseCategory(PhraseCategory.PhraseCategoryEnum.NOUN_PHRASE))
		{
			Factory = phraseFactory;
			setFeature(Feature.IS_CAPITALIZED, false);
		}

	    /*
	     * (non-Javadoc)
	     * 
	     * @see simplenlg.framework.PhraseElement#setHead(java.lang.Object) This
	     * version sets NP default features from the head
	     */
		public override void setHead(object value)
		{
				base.setHead(value);
				NounPhraseFeatures = getFeatureAsElement(InternalFeature.HEAD);
		}

	    /**
	     * A helper method to set the features required for noun phrases, from the
	     * head noun
	     * 
	     * @param phraseElement
	     *            the phrase element.
	     * @param nounElement
	     *            the element representing the noun.
	     */
		private NLGElement NounPhraseFeatures
		{
			set
			{
				if (value == null)
				{
					return;
				}
    
				setFeature(Feature.POSSESSIVE, value != null ? value.getFeatureAsBoolean(Feature.POSSESSIVE) : false);
				setFeature(InternalFeature.RAISED, false);
				setFeature(InternalFeature.ACRONYM, false);
    
				if (value != null && value.hasFeature(Feature.NUMBER))
				{
    
					setFeature(Feature.NUMBER, value.getFeature(Feature.NUMBER));
				}
				else
				{
					Plural = false;
				}
				if (value != null && value.hasFeature(Feature.PERSON))
				{
    
					setFeature(Feature.PERSON, value.getFeature(Feature.PERSON));
				}
				else
				{
					setFeature(Feature.PERSON, Person.THIRD);
				}
				if (value != null && value.hasFeature(LexicalFeature.GENDER))
				{
    
					setFeature(LexicalFeature.GENDER, value.getFeature(LexicalFeature.GENDER));
				}
				else
				{
					setFeature(LexicalFeature.GENDER, Gender.NEUTER);
				}
    
				if (value != null && value.hasFeature(LexicalFeature.EXPLETIVE_SUBJECT))
				{
    
					setFeature(LexicalFeature.EXPLETIVE_SUBJECT, value.getFeature(LexicalFeature.EXPLETIVE_SUBJECT));
				}
    
				setFeature(Feature.ADJECTIVE_ORDERING, true);
				setFeature(Feature.IS_CAPITALIZED, false);
			}
		}
	    /**
	     * sets the noun (head) of a noun phrase
	     * 
	     * @param noun
	     */
		public virtual void setNoun(object noun)
		{
			NLGElement nounElement = Factory.createNLGElement(noun, new LexicalCategory(LexicalCategory.LexicalCategoryEnum.NOUN));
			setHead(nounElement);
		}

	    /**
	     * @return noun (head) of noun phrase
	     */
		public virtual NLGElement getNoun()
		{
			return getHead();
		}


	    /**
	     * setDeterminer - Convenience method for when a person tries to set 
	     *                 a determiner (e.g. "the") to a NPPhraseSpec.
	     */
		public virtual void setDeterminer(object determiner)
		{
			setSpecifier(determiner);
		}

	    /**
	     * getDeterminer - Convenience method for when a person tries to get a
	     *                 determiner (e.g. "the") from a NPPhraseSpec.
	     */
		public virtual NLGElement getDeterminer()
		{
			return getSpecifier();
		}

	    /**
	     * sets the specifier of a noun phrase. Can be determiner (eg "the"),
	     * possessive (eg, "John's")
	     * 
	     * @param specifier
	     */
		public virtual void setSpecifier(object specifier)
		{
			if (specifier is NLGElement)
			{
				setFeature(InternalFeature.SPECIFIER, specifier);
				((NLGElement) specifier).setFeature(InternalFeature.DISCOURSE_FUNCTION, DiscourseFunction.SPECIFIER);
			}
			else
			{
			    // create specifier as word (assume determiner)
				NLGElement specifierElement = Factory.createWord(specifier, new LexicalCategory(LexicalCategory.LexicalCategoryEnum.DETERMINER));


			    // set specifier feature
				if (specifierElement != null)
				{
					setFeature(InternalFeature.SPECIFIER, specifierElement);
					specifierElement.setFeature(InternalFeature.DISCOURSE_FUNCTION, DiscourseFunction.SPECIFIER);
				}
			}
		}

	    /**
	     * @return specifier (eg, determiner) of noun phrase
	     */
		public virtual NLGElement getSpecifier()
		{
			return getFeatureAsElement(InternalFeature.SPECIFIER);
		}

	    /**
	     * Add a modifier to an NP Use heuristics to decide where it goes
	     * 
	     * @param modifier
	     */
		public override void addModifier(object modifier)
		{
		    // string which is one lexicographic word is looked up in lexicon,
		    // adjective is preModifier
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
				string modifierString = (string) modifier;
				if (modifierString.Length > 0 && !modifierString.Contains(" "))
				{
					modifierElement = Factory.createWord(modifier, new LexicalCategory(LexicalCategory.LexicalCategoryEnum.ANY));
				}
			}

		    // if no modifier element, must be a complex string, add as postModifier
			if (modifierElement == null)
			{
				addPostModifier((string) modifier);
				return;
			}

		    // AdjP is premodifer
			if (modifierElement is AdjPhraseSpec)
			{
				addPreModifier(modifierElement);
				return;
			}

		    // else extract WordElement if modifier is a single word
			WordElement modifierWord = null;
			if (modifierElement != null && modifierElement is WordElement)
			{
				modifierWord = (WordElement) modifierElement;
			}
			else if (modifierElement != null && modifierElement is InflectedWordElement)
			{
				modifierWord = ((InflectedWordElement) modifierElement).BaseWord;
			}
		    // check if modifier is an adjective

			if (modifierWord != null && modifierWord.Category == LexicalCategory.LexicalCategoryEnum.ADJECTIVE)
			{
				addPreModifier(modifierWord);
				return;
			}

		    // default case
			addPostModifier(modifierElement);
		}
	}

}