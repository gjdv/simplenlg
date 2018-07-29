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
using System.Text.RegularExpressions;

namespace SimpleNLG.Main.framework
{

	using Feature = features.Feature;
	using Gender = features.Gender;
	using InternalFeature = features.InternalFeature;
	using LexicalFeature = features.LexicalFeature;
	using NumberAgreement = features.NumberAgreement;
	using Person = features.Person;
	using Lexicon = lexicon.Lexicon;
	using AdjPhraseSpec = phrasespec.AdjPhraseSpec;
	using AdvPhraseSpec = phrasespec.AdvPhraseSpec;
	using NPPhraseSpec = phrasespec.NPPhraseSpec;
	using PPPhraseSpec = phrasespec.PPPhraseSpec;
	using SPhraseSpec = phrasespec.SPhraseSpec;
	using VPPhraseSpec = phrasespec.VPPhraseSpec;

    /**
     * <p>
     * This class contains methods for creating syntactic phrases. These methods
     * should be used instead of directly invoking new on SPhraseSpec, etc.
     * 
     * The phrase factory should be linked to s lexicon if possible (although it
     * will work without one)
     * </p>
     * 
     * 
     * @author D. Westwater, University of Aberdeen.
     * @version 4.0
     * 
     */
	public class NLGFactory
	{
	    /***
	     * CODING COMMENTS The original version of phraseFactory created a crude
	     * realisation of the phrase in the BASE_FORM feature. This was just for
	     * debugging purposes (note BASE_FORM on a WordElement is meaningful), I've
	     * zapped this as it was makig things too complex
	     * 
	     * This version of phraseFactory replicates WordElements (instead of reusing
	     * them). I think this is because elemente are linked to their parent
	     * phrases, via the parent data member. May be good to check if this is
	     * actually necessary
	     * 
	     * The explicit list of pronouns below should be replaced by a reference to
	     * the lexicon
	     * 
	     * Things to sort out at some point...
	     * 
	     */
	    /** The lexicon to be used with this factory. */
		private Lexicon lexicon;

    	/** The list of English pronouns. */
		private static readonly IList<string> PRONOUNS = new List<string>{"I", "you", "he", "she", "it", "me", "you", "him", "her", "it", "myself", "yourself", "himself", "herself", "itself", "mine", "yours", "his", "hers", "its", "we", "you", "they", "they", "they", "us", "you", "them", "them", "them", "ourselves", "yourselves", "themselves", "themselves", "themselves", "ours", "yours", "theirs", "theirs", "theirs", "there"};

		/** The list of first-person English pronouns. */
		private static readonly IList<string> FIRST_PRONOUNS = new List<string> {"I", "me", "myself", "we", "us", "ourselves", "mine", "my", "ours", "our"};

    	/** The list of second person English pronouns. */
		private static readonly IList<string> SECOND_PRONOUNS = new List<string> {"you", "yourself", "yourselves", "yours", "your"};

		/** The list of reflexive English pronouns. */
		private static readonly IList<string> REFLEXIVE_PRONOUNS = new List<string> { "myself", "yourself", "himself", "herself", "itself", "ourselves", "yourselves", "themselves"};

    	/** The list of masculine English pronouns. */
		private static readonly IList<string> MASCULINE_PRONOUNS = new List<string> { "he", "him", "himself", "his"};
	    /** The list of feminine English pronouns. */

		private static readonly IList<string> FEMININE_PRONOUNS = new List<string> { "she", "her", "herself", "hers"};

        /** The list of possessive English pronouns. */
		private static readonly IList<string> POSSESSIVE_PRONOUNS = new List<string> { "mine", "ours", "yours", "his", "hers", "its", "theirs", "my", "our", "your", "her", "their"};

        /** The list of plural English pronouns. */
		private static readonly IList<string> PLURAL_PRONOUNS = new List<string> { "we", "us", "ourselves", "ours", "our", "they", "them", "theirs", "their"};

    	/** The list of English pronouns that can be singular or plural. */
		private static readonly IList<string> EITHER_NUMBER_PRONOUNS = new List<string> { "there"};

	    /** The list of expletive English pronouns. */
		private static readonly IList<string> EXPLETIVE_PRONOUNS = new List<string> { "there"};

	    /** regex for determining if a string is a single word or not **/
		private const string WORD_REGEX = "\\w*";

	    /**
	     * Creates a new phrase factory with no associated lexicon.
	     */
		public NLGFactory() : this(null)
		{
		}

	    /**
	     * Creates a new phrase factory with the associated lexicon.
	     * 
	     * @param newLexicon
	     *            the <code>Lexicon</code> to be used with this factory.
	     */
		public NLGFactory(Lexicon newLexicon)
		{
			Lexicon = newLexicon;
		}

	    /**
	     * Sets the lexicon to be used by this factory. Passing a parameter of
	     * <code>null</code> will remove any existing lexicon from the factory.
	     * 
	     * @param newLexicon
	     *            the new <code>Lexicon</code> to be used.
	     */
		public virtual Lexicon Lexicon
		{
			set
			{
				lexicon = value;
			}
		}
	    /**
	     * Creates a new element representing a word. If the word passed is already
	     * an <code>NLGElement</code> then that is returned unchanged. If a
	     * <code>String</code> is passed as the word then the factory will look up
	     * the <code>Lexicon</code> if one exists and use the details found to
	     * create a new <code>WordElement</code>.
	     * 
	     * @param word
	     *            the base word for the new element. This can be a
	     *            <code>NLGElement</code>, which is returned unchanged, or a
	     *            <code>String</code>, which is used to construct a new
	     *            <code>WordElement</code>.
	     * @param category
	     *            the <code>LexicalCategory</code> for the word.
	     * 
	     * @return an <code>NLGElement</code> representing the word.
	     */
		public virtual NLGElement createWord(object word, LexicalCategory category)
		{
			NLGElement wordElement = null;
			if (word is NLGElement)
			{
				wordElement = (NLGElement) word;

			}
			else if (word is string && lexicon != null)
			{
			    // AG: change: should create a WordElement, not an
			    // InflectedWordElement
			    // wordElement = new InflectedWordElement(
			    // (String) word, category);
			    // if (this.lexicon != null) {
			    // doLexiconLookUp(category, (String) word, wordElement);
			    // }
			    // wordElement = lexicon.getWord((String) word, category);
				wordElement = lexicon.lookupWord((string) word, category);
				if (PRONOUNS.Contains((string) word))
				{
					setPronounFeatures(wordElement, (string) word);
				}
			}

			return wordElement;
		}

	    /**
	     * Create an inflected word element. InflectedWordElement represents a word
	     * that already specifies the morphological and other features that it
	     * should exhibit in a realisation. While normally, phrases are constructed
	     * using <code>WordElement</code>s, and features are set on phrases, it is
	     * sometimes desirable to set features directly on words (for example, when
	     * one wants to elide a specific word, but not its parent phrase).
	     * 
	     * <P>
	     * If the object passed is already a <code>WordElement</code>, then a new
	     * 
	     * <code>InflectedWordElement<code> is returned which wraps this <code>WordElement</code>
	     * . If the object is a <code>String</code>, then the
	     * <code>WordElement</code> representing this <code>String</code> is looked
	     * up, and a new
	     * <code>InflectedWordElement<code> wrapping this is returned. If no such <code>WordElement</code>
	     * is found, the element returned is an <code>InflectedWordElement</code>
	     * with the supplied string as baseform and no base <code>WordElement</code>
	     * . If an <code>NLGElement</code> is passed, this is returned unchanged.
	     * 
	     * @param word
	     *            the word
	     * @param category
	     *            the category
	     * @return an <code>InflectedWordElement</code>, or the original supplied
	     *         object if it is an <code>NLGElement</code>.
	     */
		public virtual NLGElement createInflectedWord(object word, LexicalCategory category)
		{
		    // first get the word element
			NLGElement inflElement = null;

			if (word is WordElement)
			{
				inflElement = new InflectedWordElement((WordElement) word);

			}
			else if (word is string)
			{
				NLGElement baseword = createWord((string) word, category);

				if (baseword != null && baseword is WordElement)
				{
					inflElement = new InflectedWordElement((WordElement) baseword);
				}
				else
				{
					inflElement = new InflectedWordElement((string) word, category);
				}

			}
			else if (word is NLGElement)
			{
				inflElement = (NLGElement) word;
			}

			return inflElement;

		}

        /**
         * A helper method to set the features on newly created pronoun words.
         * 
         * @param wordElement
         *            the created element representing the pronoun.
         * @param word
         *            the base word for the pronoun.
         */
		private void setPronounFeatures(NLGElement wordElement, string word)
		{
			wordElement.Category = new LexicalCategory(LexicalCategory.LexicalCategoryEnum.PRONOUN);
			if (FIRST_PRONOUNS.Contains(word))
			{
				wordElement.setFeature(Feature.PERSON, Person.FIRST);
			}
			else if (SECOND_PRONOUNS.Contains(word))
			{
				wordElement.setFeature(Feature.PERSON, Person.SECOND);

				if ("yourself".Equals(word, StringComparison.OrdinalIgnoreCase))
				{ //$NON-NLS-1$
					wordElement.Plural = false;
				}
				else if ("yourselves".Equals(word, StringComparison.OrdinalIgnoreCase))
				{ //$NON-NLS-1$
					wordElement.Plural = true;
				}
				else
				{
					wordElement.setFeature(Feature.NUMBER, NumberAgreement.BOTH);
				}
			}
			else
			{
				wordElement.setFeature(Feature.PERSON, Person.THIRD);
			}
			if (REFLEXIVE_PRONOUNS.Contains(word))
			{
				wordElement.setFeature(LexicalFeature.REFLEXIVE, true);
			}
			else
			{
				wordElement.setFeature(LexicalFeature.REFLEXIVE, false);
			}
			if (MASCULINE_PRONOUNS.Contains(word))
			{
				wordElement.setFeature(LexicalFeature.GENDER, Gender.MASCULINE);
			}
			else if (FEMININE_PRONOUNS.Contains(word))
			{
				wordElement.setFeature(LexicalFeature.GENDER, Gender.FEMININE);
			}
			else
			{
				wordElement.setFeature(LexicalFeature.GENDER, Gender.NEUTER);
			}

			if (POSSESSIVE_PRONOUNS.Contains(word))
			{
				wordElement.setFeature(Feature.POSSESSIVE, true);
			}
			else
			{
				wordElement.setFeature(Feature.POSSESSIVE, false);
			}

			if (PLURAL_PRONOUNS.Contains(word) && !SECOND_PRONOUNS.Contains(word))
			{
				wordElement.Plural = true;
			}
			else if (!EITHER_NUMBER_PRONOUNS.Contains(word))
			{
				wordElement.Plural = false;
			}

			if (EXPLETIVE_PRONOUNS.Contains(word))
			{
				wordElement.setFeature(InternalFeature.NON_MORPH, true);
				wordElement.setFeature(LexicalFeature.EXPLETIVE_SUBJECT, true);
			}
			wordElement.setFeature(Feature.IS_CAPITALIZED, false); //added by GJdV, some default value
		}

	    /**
	     * A helper method to look up the lexicon for the given word.
	     * 
	     * @param category
	     *            the <code>LexicalCategory</code> of the word.
	     * @param word
	     *            the base form of the word.
	     * @param wordElement
	     *            the created element representing the word.
	     */
		private void doLexiconLookUp(LexicalCategory category, string word, NLGElement wordElement)
		{
			WordElement baseWord = null;

			if (category.GetLexicalCategory() == LexicalCategory.LexicalCategoryEnum.NOUN && lexicon.hasWord(word, new LexicalCategory(LexicalCategory.LexicalCategoryEnum.PRONOUN)))
			{
				baseWord = lexicon.lookupWord(word, new LexicalCategory(LexicalCategory.LexicalCategoryEnum.PRONOUN));

				if (baseWord != null)
				{
					wordElement.setFeature(InternalFeature.BASE_WORD, baseWord);
					wordElement.Category = new LexicalCategory(LexicalCategory.LexicalCategoryEnum.PRONOUN);
					if (!PRONOUNS.Contains(word))
					{
						wordElement.setFeature(InternalFeature.NON_MORPH, true);
					}
				}
			}
			else
			{
				baseWord = lexicon.lookupWord(word, category);
				wordElement.setFeature(InternalFeature.BASE_WORD, baseWord);
			}
		}

	    /**
	     * Creates a blank preposition phrase with no preposition or complements.
	     * 
	     * @return a <code>PPPhraseSpec</code> representing this phrase.
	     */
		public virtual PPPhraseSpec createPrepositionPhrase()
		{
			return createPrepositionPhrase(null, null);
		}

	    /**
	     * Creates a preposition phrase with the given preposition.
	     * 
	     * @param preposition
	     *            the preposition to be used.
	     * @return a <code>PPPhraseSpec</code> representing this phrase.
	     */
		public virtual PPPhraseSpec createPrepositionPhrase(object preposition)
		{
			return createPrepositionPhrase(preposition, null);
		}

	    /**
	     * Creates a preposition phrase with the given preposition and complement.
	     * An <code>NLGElement</code> representing the preposition is added as the
	     * head feature of this phrase while the complement is added as a normal
	     * phrase complement.
	     * 
	     * @param preposition
	     *            the preposition to be used.
	     * @param complement
	     *            the complement of the phrase.
	     * @return a <code>PPPhraseSpec</code> representing this phrase.
	     */
		public virtual PPPhraseSpec createPrepositionPhrase(object preposition, object complement)
		{

			PPPhraseSpec phraseElement = new PPPhraseSpec(this);

			NLGElement prepositionalElement = createNLGElement(preposition, new LexicalCategory(LexicalCategory.LexicalCategoryEnum.PREPOSITION));
			setPhraseHead(phraseElement, prepositionalElement);

			if (complement != null)
			{
				setComplement(phraseElement, complement);
			}
			return phraseElement;
		}

	    /**
	     * A helper method for setting the complement of a phrase.
	     * 
	     * @param phraseElement
	     *            the created element representing this phrase.
	     * @param complement
	     *            the complement to be added.
	     */
		private void setComplement(PhraseElement phraseElement, object complement)
		{
			NLGElement complementElement = createNLGElement(complement);
			phraseElement.addComplement(complementElement);
		}

	    /**
	     * this method creates an NLGElement from an object If object is null,
	     * return null If the object is already an NLGElement, it is returned
	     * unchanged Exception: if it is an InflectedWordElement, return underlying
	     * WordElement If it is a String which matches a lexicon entry or pronoun,
	     * the relevant WordElement is returned If it is a different String, a
	     * wordElement is created if the string is a single word Otherwise a
	     * StringElement is returned Otherwise throw an exception
	     * 
	     * @param element
	     *            - object to look up
	     * @param category
	     *            - default lexical category of object
	     * @return NLGelement
	     */
		public virtual NLGElement createNLGElement(object element, LexicalCategory category)
		{
			if (element == null)
			{
				return null;
			}

    		// InflectedWordElement - return underlying word
			else if (element is InflectedWordElement)
			{
				return ((InflectedWordElement) element).BaseWord;
			}

		    // StringElement - look up in lexicon if it is a word
		    // otherwise return element
		    else if (element is StringElement)
			{
				if (stringIsWord(((StringElement) element).Realisation, category))
				{
					return createWord(((StringElement) element).Realisation, category);
				}
				else
				{
					return (StringElement) element;
				}
			}

    		// other NLGElement - return element
			else if (element is NLGElement)
			{
				return (NLGElement) element;
			}

	    	// String - look up in lexicon if a word, otherwise return StringElement
			else if (element is string)
			{
				if (stringIsWord((string) element, category))
				{
					return createWord(element, category);
				}
				else
				{
					return new StringElement((string) element);
				}
			}

			throw new ArgumentException(element.ToString() + " is not a valid type");
		}

        /**
         * return true if string is a word
         * 
         * @param string
         * @param category
         * @return
         */
		private bool stringIsWord(string str, LexicalCategory category)
		{
			return lexicon != null && (lexicon.hasWord(str, category) || PRONOUNS.Contains(str) || (Regex.IsMatch(str,"^"+WORD_REGEX+"$")));
		}

	    /**
	     * create an NLGElement from the element, no default lexical category
	     * 
	     * @param element
	     * @return NLGelement
	     */
		public virtual NLGElement createNLGElement(object element)
		{
			return createNLGElement(element, new LexicalCategory(LexicalCategory.LexicalCategoryEnum.ANY));
		}

	    /**
	     * Creates a blank noun phrase with no subject or specifier.
	     * 
	     * @return a <code>NPPhraseSpec</code> representing this phrase.
	     */
		public virtual NPPhraseSpec createNounPhrase()
		{
			return createNounPhrase(null, null);
		}

	    /**
	     * Creates a noun phrase with the given subject but no specifier.
	     * 
	     * @param noun
	     *            the subject of the phrase.
	     * @return a <code>NPPhraseSpec</code> representing this phrase.
	     */
		public virtual NPPhraseSpec createNounPhrase(object noun)
		{
			if (noun is NPPhraseSpec)
			{
				return (NPPhraseSpec) noun;
			}
			else
			{
				return createNounPhrase(null, noun);
			}
		}
	    /**
	     * Creates a noun phrase with the given specifier and subject.
	     * 
	     * @param specifier
	     *            the specifier or determiner for the noun phrase.
	     * @param noun
	     *            the subject of the phrase.
	     * @return a <code>NPPhraseSpec</code> representing this phrase.
	     */
		public virtual NPPhraseSpec createNounPhrase(object specifier, object noun)
		{
			if (noun is NPPhraseSpec)
			{
				return (NPPhraseSpec) noun;
			}

			NPPhraseSpec phraseElement = new NPPhraseSpec(this);
			NLGElement nounElement = createNLGElement(noun, new LexicalCategory(LexicalCategory.LexicalCategoryEnum.NOUN));
			setPhraseHead(phraseElement, nounElement);

			if (specifier != null)
			{
				phraseElement.setSpecifier(specifier);
			}

			return phraseElement;
		}

	    /**
	     * A helper method to set the head feature of the phrase.
	     * 
	     * @param phraseElement
	     *            the phrase element.
	     * @param headElement
	     *            the head element.
	     */
		private void setPhraseHead(PhraseElement phraseElement, NLGElement headElement)
		{
			if (headElement != null)
			{
				phraseElement.setHead(headElement);
				headElement.Parent = phraseElement;
			}
		}

	    /**
	     * Creates a blank adjective phrase with no base adjective set.
	     * 
	     * @return a <code>AdjPhraseSpec</code> representing this phrase.
	     */
		public virtual AdjPhraseSpec createAdjectivePhrase()
		{
			return createAdjectivePhrase(null);
		}

	    /**
	     * Creates an adjective phrase wrapping the given adjective.
	     * 
	     * @param adjective
	     *            the main adjective for this phrase.
	     * @return a <code>AdjPhraseSpec</code> representing this phrase.
	     */
		public virtual AdjPhraseSpec createAdjectivePhrase(object adjective)
		{
			AdjPhraseSpec phraseElement = new AdjPhraseSpec(this);

			NLGElement adjectiveElement = createNLGElement(adjective, new LexicalCategory(LexicalCategory.LexicalCategoryEnum.ADJECTIVE));
			setPhraseHead(phraseElement, adjectiveElement);

			return phraseElement;
		}

	    /**
	     * Creates a blank verb phrase with no main verb.
	     * 
	     * @return a <code>VPPhraseSpec</code> representing this phrase.
	     */
		public virtual VPPhraseSpec createVerbPhrase()
		{
			return createVerbPhrase(null);
		}

	    /**
	     * Creates a verb phrase wrapping the main verb given. If a
	     * <code>String</code> is passed in then some parsing is done to see if the
	     * verb contains a particle, for example <em>fall down</em>. The first word
	     * is taken to be the verb while all other words are assumed to form the
	     * particle.
	     * 
	     * @param verb
	     *            the verb to be wrapped.
	     * @return a <code>VPPhraseSpec</code> representing this phrase.
	     */
		public virtual VPPhraseSpec createVerbPhrase(object verb)
		{
			VPPhraseSpec phraseElement = new VPPhraseSpec(this);
			phraseElement.setVerb(verb);
			setPhraseHead(phraseElement, phraseElement.getVerb());
			return phraseElement;
		}

	    /**
	     * Creates a blank adverb phrase that has no adverb.
	     * 
	     * @return a <code>AdvPhraseSpec</code> representing this phrase.
	     */
		public virtual AdvPhraseSpec createAdverbPhrase()
		{
			return createAdverbPhrase(null);
		}

	    /**
	     * Creates an adverb phrase wrapping the given adverb.
	     * 
	     * @param adverb
	     *            the adverb for this phrase.
	     * @return a <code>AdvPhraseSpec</code> representing this phrase.
	     */
		public virtual AdvPhraseSpec createAdverbPhrase(string adverb)
		{
			AdvPhraseSpec phraseElement = new AdvPhraseSpec(this);

			NLGElement adverbElement = createNLGElement(adverb, new LexicalCategory(LexicalCategory.LexicalCategoryEnum.ADVERB));
			setPhraseHead(phraseElement, adverbElement);
			return phraseElement;
		}

	    /**
	     * Creates a blank clause with no subject, verb or objects.
	     * 
	     * @return a <code>SPhraseSpec</code> representing this phrase.
	     */
		public virtual SPhraseSpec createClause()
		{
			return createClause(null, null, null);
		}

	    /**
	     * Creates a clause with the given subject and verb but no objects.
	     * 
	     * @param subject
	     *            the subject for the clause as a <code>NLGElement</code> or
	     *            <code>String</code>. This forms a noun phrase.
	     * @param verb
	     *            the verb for the clause as a <code>NLGElement</code> or
	     *            <code>String</code>. This forms a verb phrase.
	     * @return a <code>SPhraseSpec</code> representing this phrase.
	     */
		public virtual SPhraseSpec createClause(object subject, object verb)
		{
			return createClause(subject, verb, null);
		}

	    /**
	     * Creates a clause with the given subject, verb or verb phrase and direct
	     * object but no indirect object.
	     * 
	     * @param subject
	     *            the subject for the clause as a <code>NLGElement</code> or
	     *            <code>String</code>. This forms a noun phrase.
	     * @param verb
	     *            the verb for the clause as a <code>NLGElement</code> or
	     *            <code>String</code>. This forms a verb phrase.
	     * @param directObject
	     *            the direct object for the clause as a <code>NLGElement</code>
	     *            or <code>String</code>. This forms a complement for the
	     *            clause.
	     * @return a <code>SPhraseSpec</code> representing this phrase.
	     */
		public virtual SPhraseSpec createClause(object subject, object verb, object directObject)
		{

			SPhraseSpec phraseElement = new SPhraseSpec(this);

			if (verb != null)
			{
    			// AG: fix here: check if "verb" is a VPPhraseSpec or a Verb
				if (verb is PhraseElement)
				{
					phraseElement.VerbPhrase = (PhraseElement) verb;
				}
				else
				{
					phraseElement.setVerb(verb);
				}
			}

			if (subject != null)
			{
				phraseElement.setSubject(subject);
			}

			if (directObject != null)
			{
				phraseElement.setObject(directObject);
			}

			return phraseElement;
		}

	    /*	*//**
	          * A helper method to set the verb phrase for a clause.
	          * 
	          * @param baseForm
	          *            the base form of the clause.
	          * @param verbPhrase
	          *            the verb phrase to be used in the clause.
	          * @param phraseElement
	          *            the current representation of the clause.
	          */
	    /*
	     * private void setVerbPhrase(StringBuffer baseForm, NLGElement verbPhrase,
	     * PhraseElement phraseElement) { if (baseForm.length() > 0) {
	     * baseForm.append(' '); }
	     * baseForm.append(verbPhrase.getFeatureAsString(Feature.BASE_FORM));
	     * phraseElement.setFeature(Feature.VERB_PHRASE, verbPhrase);
	     * verbPhrase.setParent(phraseElement);
	     * verbPhrase.setFeature(Feature.DISCOURSE_FUNCTION,
	     * DiscourseFunction.VERB_PHRASE); if
	     * (phraseElement.hasFeature(Feature.GENDER)) {
	     * verbPhrase.setFeature(Feature.GENDER, phraseElement
	     * .getFeature(Feature.GENDER)); } if
	     * (phraseElement.hasFeature(Feature.NUMBER)) {
	     * verbPhrase.setFeature(Feature.NUMBER, phraseElement
	     * .getFeature(Feature.NUMBER)); } if
	     * (phraseElement.hasFeature(Feature.PERSON)) {
	     * verbPhrase.setFeature(Feature.PERSON, phraseElement
	     * .getFeature(Feature.PERSON)); } }
	     *//**
	       * A helper method to add the direct object to the clause.
	       * 
	       * @param baseForm
	       *            the base form for the clause.
	       * @param directObject
	       *            the direct object to be added.
	       * @param phraseElement
	       *            the current representation of this clause.
	       * @param function
	       *            the discourse function for this object.
	       */
	    /*
	     * private void setObject(StringBuffer baseForm, Object object,
	     * PhraseElement phraseElement, DiscourseFunction function) { if
	     * (baseForm.length() > 0) { baseForm.append(' '); } if (object instanceof
	     * NLGElement) { phraseElement.addComplement((NLGElement) object);
	     * baseForm.append(((NLGElement) object)
	     * .getFeatureAsString(Feature.BASE_FORM));
	     * 
	     * ((NLGElement) object).setFeature(Feature.DISCOURSE_FUNCTION, function);
	     * 
	     * if (((NLGElement) object).hasFeature(Feature.NUMBER)) {
	     * phraseElement.setFeature(Feature.NUMBER, ((NLGElement) object)
	     * .getFeature(Feature.NUMBER)); } } else if (object instanceof String) {
	     * NLGElement complementElement = createNounPhrase(object);
	     * phraseElement.addComplement(complementElement);
	     * complementElement.setFeature(Feature.DISCOURSE_FUNCTION, function);
	     * baseForm.append((String) object); } }
	     */
	    /*	*//**
	          * A helper method that sets the subjects on a clause.
	          * 
	          * @param phraseElement
	          *            the element representing the clause.
	          * @param subjectPhrase
	          *            the subject phrase for the clause.
	          * @param baseForm
	          *            the base form for the clause.
	          */
	    /*
	     * private void setPhraseSubjects(PhraseElement phraseElement, NLGElement
	     * subjectPhrase, StringBuffer baseForm) {
	     * subjectPhrase.setParent(phraseElement); List<NLGElement> allSubjects =
	     * new ArrayList<NLGElement>(); allSubjects.add(subjectPhrase);
	     * phraseElement.setFeature(Feature.SUBJECTS, allSubjects);
	     * baseForm.append(subjectPhrase.getFeatureAsString(Feature.BASE_FORM));
	     * subjectPhrase.setFeature(Feature.DISCOURSE_FUNCTION,
	     * DiscourseFunction.SUBJECT);
	     * 
	     * if (subjectPhrase.hasFeature(Feature.GENDER)) {
	     * phraseElement.setFeature(Feature.GENDER, subjectPhrase
	     * .getFeature(Feature.GENDER)); } if
	     * (subjectPhrase.hasFeature(Feature.NUMBER)) {
	     * phraseElement.setFeature(Feature.NUMBER, subjectPhrase
	     * .getFeature(Feature.NUMBER));
	     * 
	     * } if (subjectPhrase.hasFeature(Feature.PERSON)) {
	     * phraseElement.setFeature(Feature.PERSON, subjectPhrase
	     * .getFeature(Feature.PERSON)); } }
	     */
	    /**
	     * Creates a blank canned text phrase with no text.
	     * 
	     * @return a <code>PhraseElement</code> representing this phrase.
	     */
	    public virtual NLGElement createStringElement()
		{
			return createStringElement(null);
		}

	    /**
	     * Creates a canned text phrase with the given text.
	     * 
	     * @param text
	     *            the canned text.
	     * @return a <code>PhraseElement</code> representing this phrase.
	     */
		public virtual NLGElement createStringElement(string text)
		{
			return new StringElement(text);
		}

	    /**
	     * Creates a new (empty) coordinated phrase
	     * 
	     * @return empty <code>CoordinatedPhraseElement</code>
	     */
		public virtual CoordinatedPhraseElement createCoordinatedPhrase()
		{
			return new CoordinatedPhraseElement();
		}

	    /**
	     * Creates a new coordinated phrase with two elements (initially)
	     * 
	     * @param coord1
	     *            - first phrase to be coordinated
	     * @param coord2
	     *            = second phrase to be coordinated
	     * @return <code>CoordinatedPhraseElement</code> for the two given elements
	     */
		public virtual CoordinatedPhraseElement createCoordinatedPhrase(object coord1, object coord2)
		{
			return new CoordinatedPhraseElement(coord1, coord2);
		}

	    /***********************************************************************************
	     * Document level stuff
	     ***********************************************************************************/

	    /**
	     * Creates a new document element with no title.
	     * 
	     * @return a <code>DocumentElement</code>
	     */
		public virtual DocumentElement createDocument()
		{
			return createDocument(null);
		}

	    /**
	     * Creates a new document element with the given title.
	     * 
	     * @param title
	     *            the title for this element.
	     * @return a <code>DocumentElement</code>.
	     */
		public virtual DocumentElement createDocument(string title)
		{
			return new DocumentElement(new DocumentCategory(DocumentCategory.DocumentCategoryEnum.DOCUMENT), title);
		}

	    /**
	     * Creates a new document element with the given title and adds all of the
	     * given components in the list
	     * 
	     * @param title
	     *            the title of this element.
	     * @param components
	     *            a <code>List</code> of <code>NLGElement</code>s that form the
	     *            components of this element.
	     * @return a <code>DocumentElement</code>
	     */
		public virtual DocumentElement createDocument(string title, IList<DocumentElement> components)
		{

			DocumentElement document = new DocumentElement(new DocumentCategory(DocumentCategory.DocumentCategoryEnum.DOCUMENT), title);
			if (components != null)
			{
				document.addComponents(components);
			}
			return document;
		}

	    /**
	     * Creates a new document element with the given title and adds the given
	     * component.
	     * 
	     * @param title
	     *            the title for this element.
	     * @param component
	     *            an <code>NLGElement</code> that becomes the first component of
	     *            this document element.
	     * @return a <code>DocumentElement</code>
	     */
		public virtual DocumentElement createDocument(string title, NLGElement component)
		{
			DocumentElement element = new DocumentElement(new DocumentCategory(DocumentCategory.DocumentCategoryEnum.DOCUMENT), title);

			if (component != null)
			{
				element.addComponent(component);
			}
			return element;
		}

	    /**
	     * Creates a new list element with no components.
	     * 
	     * @return a <code>DocumentElement</code> representing the list.
	     */
		public virtual DocumentElement createList()
		{
			return new DocumentElement(new DocumentCategory(DocumentCategory.DocumentCategoryEnum.LIST), null);
		}

	    /**
	     * Creates a new list element and adds all of the given components in the
	     * list
	     * 
	     * @param textComponents
	     *            a <code>List</code> of <code>NLGElement</code>s that form the
	     *            components of this element.
	     * @return a <code>DocumentElement</code> representing the list.
	     */
		public virtual DocumentElement createList(IList<DocumentElement> textComponents)
		{
			DocumentElement list = new DocumentElement(new DocumentCategory(DocumentCategory.DocumentCategoryEnum.LIST), null);
			list.addComponents(textComponents);
			return list;
		}

	    /**
	     * Creates a new section element with the given title and adds the given
	     * component.
	     * 
	     * @param component
	     *            an <code>NLGElement</code> that becomes the first component of
	     *            this document element.
	     * @return a <code>DocumentElement</code> representing the section.
	     */
		public virtual DocumentElement createList(NLGElement component)
		{
			DocumentElement list = new DocumentElement(new DocumentCategory(DocumentCategory.DocumentCategoryEnum.LIST), null);
			list.addComponent(component);
			return list;
		}

	    /**
	     * Creates a new enumerated list element with no components.
	     * 
	     * @return a <code>DocumentElement</code> representing the list.
	     * @author Rodrigo de Oliveira - Data2Text Ltd
	     */
		public virtual DocumentElement createEnumeratedList()
		{
			return new DocumentElement(new DocumentCategory(DocumentCategory.DocumentCategoryEnum.ENUMERATED_LIST), null);
		}

	    /**
	     * Creates a new enumerated list element and adds all of the given components in the
	     * list
	     * 
	     * @param textComponents
	     *            a <code>List</code> of <code>NLGElement</code>s that form the
	     *            components of this element.
	     * @return a <code>DocumentElement</code> representing the list.
	     * @author Rodrigo de Oliveira - Data2Text Ltd
	     */
		public virtual DocumentElement createEnumeratedList(IList<DocumentElement> textComponents)
		{
			DocumentElement list = new DocumentElement(new DocumentCategory(DocumentCategory.DocumentCategoryEnum.ENUMERATED_LIST), null);
			list.addComponents(textComponents);
			return list;
		}

	    /**
	     * Creates a new section element with the given title and adds the given
	     * component.
	     * 
	     * @param component
	     *            an <code>NLGElement</code> that becomes the first component of
	     *            this document element.
	     * @return a <code>DocumentElement</code> representing the section.
	     * @author Rodrigo de Oliveira - Data2Text Ltd
	     */
		public virtual DocumentElement createEnumeratedList(NLGElement component)
		{
			DocumentElement list = new DocumentElement(new DocumentCategory(DocumentCategory.DocumentCategoryEnum.ENUMERATED_LIST), null);
			list.addComponent(component);
			return list;
		}

	    /**
	     * Creates a list item for adding to a list element.
	     * 
	     * @return a <code>DocumentElement</code> representing the list item.
	     */
		public virtual DocumentElement createListItem()
		{
			return new DocumentElement(new DocumentCategory(DocumentCategory.DocumentCategoryEnum.LIST_ITEM), null);
		}

	    /**
	     * Creates a list item for adding to a list element. The list item has the
	     * given component.
	     * 
	     * @return a <code>DocumentElement</code> representing the list item.
	     */
		public virtual DocumentElement createListItem(NLGElement component)
		{
			DocumentElement listItem = new DocumentElement(new DocumentCategory(DocumentCategory.DocumentCategoryEnum.LIST_ITEM), null);
			listItem.addComponent(component);
			return listItem;
		}

	    /**
	     * Creates a new paragraph element with no components.
	     * 
	     * @return a <code>DocumentElement</code> representing this paragraph
	     */
		public virtual DocumentElement createParagraph()
		{
			return new DocumentElement(new DocumentCategory(DocumentCategory.DocumentCategoryEnum.PARAGRAPH), null);
		}

	    /**
	     * Creates a new paragraph element and adds all of the given components in
	     * the list
	     * 
	     * @param components
	     *            a <code>List</code> of <code>NLGElement</code>s that form the
	     *            components of this element.
	     * @return a <code>DocumentElement</code> representing this paragraph
	     */
		public virtual DocumentElement createParagraph(IList<DocumentElement> components)
		{
			DocumentElement paragraph = new DocumentElement(new DocumentCategory(DocumentCategory.DocumentCategoryEnum.PARAGRAPH), null);
			if (components != null)
			{
				paragraph.addComponents(components);
			}
			return paragraph;
		}

	    /**
	     * Creates a new paragraph element and adds the given component
	     * 
	     * @param component
	     *            an <code>NLGElement</code> that becomes the first component of
	     *            this document element.
	     * @return a <code>DocumentElement</code> representing this paragraph
	     */
		public virtual DocumentElement createParagraph(NLGElement component)
		{
			DocumentElement paragraph = new DocumentElement(new DocumentCategory(DocumentCategory.DocumentCategoryEnum.PARAGRAPH), null);
			if (component != null)
			{
				paragraph.addComponent(component);
			}
			return paragraph;
		}

	    /**
	     * Creates a new section element.
	     * 
	     * @return a <code>DocumentElement</code> representing the section.
	     */
		public virtual DocumentElement createSection()
		{
			return new DocumentElement(new DocumentCategory(DocumentCategory.DocumentCategoryEnum.SECTION), null);
		}

	    /**
	     * Creates a new section element with the given title.
	     * 
	     * @param title
	     *            the title of the section.
	     * @return a <code>DocumentElement</code> representing the section.
	     */
		public virtual DocumentElement createSection(string title)
		{
			return new DocumentElement(new DocumentCategory(DocumentCategory.DocumentCategoryEnum.SECTION), title);
		}

	    /**
	     * Creates a new section element with the given title and adds all of the
	     * given components in the list
	     * 
	     * @param title
	     *            the title of this element.
	     * @param components
	     *            a <code>List</code> of <code>NLGElement</code>s that form the
	     *            components of this element.
	     * @return a <code>DocumentElement</code> representing the section.
	     */
		public virtual DocumentElement createSection(string title, IList<DocumentElement> components)
		{

			DocumentElement section = new DocumentElement(new DocumentCategory(DocumentCategory.DocumentCategoryEnum.SECTION), title);
			if (components != null)
			{
				section.addComponents(components);
			}
			return section;
		}

	    /**
	     * Creates a new section element with the given title and adds the given
	     * component.
	     * 
	     * @param title
	     *            the title for this element.
	     * @param component
	     *            an <code>NLGElement</code> that becomes the first component of
	     *            this document element.
	     * @return a <code>DocumentElement</code> representing the section.
	     */
		public virtual DocumentElement createSection(string title, NLGElement component)
		{
			DocumentElement section = new DocumentElement(new DocumentCategory(DocumentCategory.DocumentCategoryEnum.SECTION), title);
			if (component != null)
			{
				section.addComponent(component);
			}
			return section;
		}

	    /**
	     * Creates a new sentence element with no components.
	     * 
	     * @return a <code>DocumentElement</code> representing this sentence
	     */
		public virtual DocumentElement createSentence()
		{
			return new DocumentElement(new DocumentCategory(DocumentCategory.DocumentCategoryEnum.SENTENCE), null);
		}

	    /**
	     * Creates a new sentence element and adds all of the given components.
	     * 
	     * @param components
	     *            a <code>List</code> of <code>NLGElement</code>s that form the
	     *            components of this element.
	     * @return a <code>DocumentElement</code> representing this sentence
	     */
		public virtual DocumentElement createSentence(IList<NLGElement> components)
		{
			DocumentElement sentence = new DocumentElement(new DocumentCategory(DocumentCategory.DocumentCategoryEnum.SENTENCE), null);
			sentence.addComponents(components);
			return sentence;
		}

	    /**
	     * Creates a new sentence element
	     * 
	     * @param components
	     *            an <code>NLGElement</code> that becomes the first component of
	     *            this document element.
	     * @return a <code>DocumentElement</code> representing this sentence
	     */
		public virtual DocumentElement createSentence(NLGElement components)
		{
			DocumentElement sentence = new DocumentElement(new DocumentCategory(DocumentCategory.DocumentCategoryEnum.SENTENCE), null);
			sentence.addComponent(components);
			return sentence;
		}

	    /**
	     * Creates a sentence with the given subject and verb. The phrase factory is
	     * used to construct a clause that then forms the components of the
	     * sentence.
	     * 
	     * @param subject
	     *            the subject of the sentence.
	     * @param verb
	     *            the verb of the sentence.
	     * @return a <code>DocumentElement</code> representing this sentence
	     */
		public virtual DocumentElement createSentence(object subject, object verb)
		{
			return createSentence(subject, verb, null);
		}

	    /**
	     * Creates a sentence with the given subject, verb and direct object. The
	     * phrase factory is used to construct a clause that then forms the
	     * components of the sentence.
	     * 
	     * @param subject
	     *            the subject of the sentence.
	     * @param verb
	     *            the verb of the sentence.
	     * @param complement
	     *            the object of the sentence.
	     * @return a <code>DocumentElement</code> representing this sentence
	     */
		public virtual DocumentElement createSentence(object subject, object verb, object complement)
		{

			DocumentElement sentence = new DocumentElement(new DocumentCategory(DocumentCategory.DocumentCategoryEnum.SENTENCE), null);
			sentence.addComponent(createClause(subject, verb, complement));
			return sentence;
		}

	    /**
	     * Creates a new sentence with the given canned text. The canned text is
	     * used to form a canned phrase (from the phrase factory) which is then
	     * added as the component to sentence element.
	     * 
	     * @param cannedSentence
	     *            the canned text as a <code>String</code>.
	     * @return a <code>DocumentElement</code> representing this sentence
	     */
		public virtual DocumentElement createSentence(string cannedSentence)
		{
			DocumentElement sentence = new DocumentElement(new DocumentCategory(DocumentCategory.DocumentCategoryEnum.SENTENCE), null);

			if (!ReferenceEquals(cannedSentence, null))
			{
				sentence.addComponent(createStringElement(cannedSentence));
			}
			return sentence;
		}
	}

}