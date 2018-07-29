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

	using ClauseStatus = features.ClauseStatus;
	using Feature = features.Feature;
	using InternalFeature = features.InternalFeature;
	using LexicalFeature = features.LexicalFeature;
	using CoordinatedPhraseElement = framework.CoordinatedPhraseElement;
	using InflectedWordElement = framework.InflectedWordElement;
	using LexicalCategory = framework.LexicalCategory;
	using NLGElement = framework.NLGElement;
	using NLGFactory = framework.NLGFactory;
	using PhraseCategory = framework.PhraseCategory;
	using PhraseElement = framework.PhraseElement;
	using WordElement = framework.WordElement;

    /**
     * <p>
     * This class defines a clause (sentence-like phrase). It is essentially a
     * wrapper around the <code>PhraseElement</code> class, with methods for setting
     * common constituents such as Subject. For example, the <code>setVerb</code>
     * method in this class sets the head of the element to be the specified verb
     * 
     * From an API perspective, this class is a simplified version of the
     * SPhraseSpec class in simplenlg V3. It provides an alternative way for
     * creating syntactic structures, compared to directly manipulating a V4
     * <code>PhraseElement</code>.
     * 
     * Methods are provided for setting and getting the following constituents:
     * <UL>
     * <li>FrontModifier (eg, "Yesterday")
     * <LI>Subject (eg, "John")
     * <LI>PreModifier (eg, "reluctantly")
     * <LI>Verb (eg, "gave")
     * <LI>IndirectObject (eg, "Mary")
     * <LI>Object (eg, "an apple")
     * <LI>PostModifier (eg, "before school")
     * </UL>
     * Note that verb, indirect object, and object are propagated to the underlying
     * verb phrase
     * 
     * NOTE: The setModifier method will attempt to automatically determine whether
     * a modifier should be expressed as a FrontModifier, PreModifier, or
     * PostModifier
     * 
     * Features (such as negated) must be accessed via the <code>setFeature</code>
     * and <code>getFeature</code> methods (inherited from <code>NLGElement</code>).
     * Features which are often set on SPhraseSpec include
     * <UL>
     * <LI>Form (eg, "John eats an apple" vs "John eating an apple")
     * <LI>InterrogativeType (eg, "John eats an apple" vs "Is John eating an apple"
     * vs "What is John eating")
     * <LI>Modal (eg, "John eats an apple" vs "John can eat an apple")
     * <LI>Negated (eg, "John eats an apple" vs "John does not eat an apple")
     * <LI>Passive (eg, "John eats an apple" vs "An apple is eaten by John")
     * <LI>Perfect (eg, "John ate an apple" vs "John has eaten an apple")
     * <LI>Progressive (eg, "John eats an apple" vs "John is eating an apple")
     * <LI>Tense (eg, "John ate" vs "John eats" vs "John will eat")
     * </UL>
     * Note that most features are propagated to the underlying verb phrase
     * Premodifers are also propogated to the underlying VP
     * 
     * <code>SPhraseSpec</code> are produced by the <code>createClause</code> method
     * of a <code>PhraseFactory</code>
     * </p>
     * 
     * @author E. Reiter, University of Aberdeen.
     * @version 4.1
     * 
     */
	public class SPhraseSpec : PhraseElement
	{
	    // the following features are copied to the VPPhraseSpec

		internal static readonly IList<string> vpFeatures = new List<string>{Feature.MODAL, Feature.TENSE, Feature.NEGATED, Feature.NUMBER, Feature.PASSIVE, Feature.PERFECT, Feature.PARTICLE, Feature.PERSON, Feature.PROGRESSIVE, InternalFeature.REALISE_AUXILIARY, Feature.FORM, Feature.INTERROGATIVE_TYPE};


		public SPhraseSpec(NLGFactory phraseFactory) : base(new PhraseCategory(PhraseCategory.PhraseCategoryEnum.CLAUSE))
		{
	    /**
	     * create an empty clause
	     */
			Factory = phraseFactory;

		    // create VP
			VerbPhrase = phraseFactory.createVerbPhrase();

		    // set default values
			setFeature(Feature.ELIDED, false);
			setFeature(InternalFeature.CLAUSE_STATUS, ClauseStatus.MATRIX);
			setFeature(Feature.SUPRESSED_COMPLEMENTISER, false);
			setFeature(LexicalFeature.EXPLETIVE_SUBJECT, false);
			setFeature(Feature.COMPLEMENTISER, phraseFactory.createWord("that", new LexicalCategory(LexicalCategory.LexicalCategoryEnum.COMPLEMENTISER))); //$NON-NLS-1$

		}

	    // intercept and override setFeature, to set VP features as needed

	    /**
	     * adds a feature, possibly to the underlying VP as well as the SPhraseSpec
	     * itself
	     * 
	     * @see simplenlg.framework.NLGElement#setFeature(java.lang.String,
	     * java.lang.Object)
	     */
		public override void setFeature(string featureName, object featureValue)
		{
			base.setFeature(featureName, featureValue);

			if (vpFeatures.Contains(featureName))
			{
				NLGElement verbPhrase = (NLGElement) getFeatureAsElement(InternalFeature.VERB_PHRASE);
				if (verbPhrase != null || verbPhrase is VPPhraseSpec)
				{
					verbPhrase.setFeature(featureName, featureValue);
				}
			}
		}
	    /*
	     * adds a premodifier, if possible to the underlying VP
	     * 
	     * @see
	     * simplenlg.framework.PhraseElement#addPreModifier(simplenlg.framework.
	     * NLGElement)
	     */
		public override void addPreModifier(NLGElement newPreModifier)
		{
			NLGElement verbPhrase = (NLGElement) getFeatureAsElement(InternalFeature.VERB_PHRASE);

			if (verbPhrase != null)
			{

				if (verbPhrase is PhraseElement)
				{
					((PhraseElement) verbPhrase).addPreModifier(newPreModifier);
				}
				else if (verbPhrase is CoordinatedPhraseElement)
				{
					((CoordinatedPhraseElement) verbPhrase).addPreModifier(newPreModifier);
				}
				else
				{
					base.addPreModifier(newPreModifier);
				}
			}
		}

	    /*
	     * adds a complement, if possible to the underlying VP
	     * 
	     * @seesimplenlg.framework.PhraseElement#addComplement(simplenlg.framework.
	     * NLGElement)
	     */
		public override void addComplement(NLGElement complement)
		{
			PhraseElement verbPhrase = (PhraseElement) getFeatureAsElement(InternalFeature.VERB_PHRASE);
			if (verbPhrase != null || verbPhrase is VPPhraseSpec)
			{
				verbPhrase.addComplement(complement);
			}
			else
			{
				base.addComplement(complement);
			}
		}
	    /*
	     * adds a complement, if possible to the underlying VP
	     * 
	     * @see simplenlg.framework.PhraseElement#addComplement(java.lang.String)
	     */
		public override void addComplement(string newComplement)
		{
			PhraseElement verbPhrase = (PhraseElement) getFeatureAsElement(InternalFeature.VERB_PHRASE);
			if (verbPhrase != null || verbPhrase is VPPhraseSpec)
			{
				verbPhrase.addComplement(newComplement);
			}
			else
			{
				base.addComplement(newComplement);
			}
		}
	    /*
	     * (non-Javadoc)
	     * 
	     * @see simplenlg.framework.NLGElement#setFeature(java.lang.String, boolean)
	     */
		public override void setFeature(string featureName, bool featureValue)
		{
			base.setFeature(featureName, featureValue);
			if (vpFeatures.Contains(featureName))
			{
			    //PhraseElement verbPhrase = (PhraseElement) getFeatureAsElement(InternalFeature.VERB_PHRASE);
			    //AG: bug fix: VP could be coordinate phrase, so cast to NLGElement not PhraseElement
				NLGElement verbPhrase = (NLGElement) getFeatureAsElement(InternalFeature.VERB_PHRASE);
				if (verbPhrase != null || verbPhrase is VPPhraseSpec)
				{
					verbPhrase.setFeature(featureName, featureValue);
				}
			}
		}
	    /* (non-Javadoc)
	     * @see simplenlg.framework.NLGElement#getFeature(java.lang.String)
	     */
		public override object getFeature(string featureName)
		{
			if (base.getFeature(featureName) != null)
			{
				return base.getFeature(featureName);
			}
			if (vpFeatures.Contains(featureName))
			{
				NLGElement verbPhrase = (NLGElement) getFeatureAsElement(InternalFeature.VERB_PHRASE);
				if (verbPhrase != null || verbPhrase is VPPhraseSpec)
				{
					return verbPhrase.getFeature(featureName);
				}
			}
			return null;
		}

	    /**
	     * @return VP for this clause
	     */
		public virtual NLGElement VerbPhrase
		{
			get
			{
				return getFeatureAsElement(InternalFeature.VERB_PHRASE);
			}
			set
			{
				setFeature(InternalFeature.VERB_PHRASE, value);
				value.Parent = this; // needed for syntactic processing
			}
		}
	    /**
	     * Set the verb of a clause
	     * 
	     * @param verb
	     */
		public virtual void setVerb(object verb)
		{
		    // get verb phrase element (create if necessary)
			NLGElement verbPhraseElement = VerbPhrase;

		    // set head of VP to verb (if this is VPPhraseSpec, and not a coord)
			if (verbPhraseElement != null && verbPhraseElement is VPPhraseSpec)
			{
				((VPPhraseSpec) verbPhraseElement).setVerb(verb);
			}
		    /*
		     * // WARNING - I don't understand verb phrase, so this may not work!!
		     * NLGElement verbElement = getFactory().createWord(verb,
		     * LexicalCategory.VERB);
		     * 
		     * // get verb phrase element (create if necessary) NLGElement
		     * verbPhraseElement = getVerbPhrase();
		     * 
		     * // set head of VP to verb (if this is VPPhraseSpec, and not a coord)
		     * if (verbPhraseElement != null && verbPhraseElement instanceof
		     * VPPhraseSpec) ((VPPhraseSpec)
		     * verbPhraseElement).setHead(verbElement);
		     */
		 }

	    /**
	     * Returns the verb of a clause
	     * 
	     * @return verb of clause
	     */
		public virtual NLGElement getVerb()
		{

		    // WARNING - I don't understand verb phrase, so this may not work!!
			PhraseElement verbPhrase = (PhraseElement) getFeatureAsElement(InternalFeature.VERB_PHRASE);
			if (verbPhrase != null || verbPhrase is VPPhraseSpec)
			{
				return verbPhrase.getHead();
			}
			else
			{
			    // return null if VP is coordinated phrase
				return null;
			}
		}
	    /**
	     * Sets the subject of a clause (assumes this is the only subject)
	     * 
	     * @param subject
	     */
		public virtual void setSubject(object subject)
		{
			NLGElement subjectPhrase;
			if (subject is PhraseElement || subject is CoordinatedPhraseElement)
			{
				subjectPhrase = (NLGElement) subject;
			}
			else
			{
				subjectPhrase = Factory.createNounPhrase(subject);
			}
			IList<NLGElement> subjects = new List<NLGElement>();
			subjects.Add(subjectPhrase);
			setFeature(InternalFeature.SUBJECTS, subjects);
		}

	    /**
	     * Returns the subject of a clause (assumes there is only one)
	     * 
	     * @return subject of clause (assume only one)
	     */
		public virtual NLGElement getSubject()
		{
			IList<NLGElement> subjects = getFeatureAsElementList(InternalFeature.SUBJECTS);
			if (subjects == null || subjects.Count == 0)
			{
				return null;
			}
			return subjects[0];
		}

	    /**
	     * Sets the direct object of a clause (assumes this is the only direct
	     * object)
	     * 
	     * @param object
	     */
		public virtual void setObject(object @object)
		{

		    // get verb phrase element (create if necessary)
			NLGElement verbPhraseElement = VerbPhrase;

		    // set object of VP to verb (if this is VPPhraseSpec, and not a coord)
			if (verbPhraseElement != null && verbPhraseElement is VPPhraseSpec)
			{
				((VPPhraseSpec) verbPhraseElement).setObject(@object);
			}
		}
	    /**
	     * Returns the direct object of a clause (assumes there is only one)
	     * 
	     * @return subject of clause (assume only one)
	     */
		public virtual NLGElement getObject()
		{
			PhraseElement verbPhrase = (PhraseElement) getFeatureAsElement(InternalFeature.VERB_PHRASE);
			if (verbPhrase != null || verbPhrase is VPPhraseSpec)
			{
				return ((VPPhraseSpec) verbPhrase).getObject();
			}
			else
			{
			    // return null if VP is coordinated phrase
				return null;
			}
		}
	    /**
	     * Set the indirect object of a clause (assumes this is the only direct
	     * indirect object)
	     * 
	     * @param indirectObject
	     */
		public virtual void setIndirectObject(object indirectObject)
		{

		    // get verb phrase element (create if necessary)
			NLGElement verbPhraseElement = VerbPhrase;

		    // set head of VP to verb (if this is VPPhraseSpec, and not a coord)
			if (verbPhraseElement != null && verbPhraseElement is VPPhraseSpec)
			{
				((VPPhraseSpec) verbPhraseElement).setIndirectObject(indirectObject);
			}
		}
	    /**
	     * Returns the indirect object of a clause (assumes there is only one)
	     * 
	     * @return subject of clause (assume only one)
	     */
		public virtual NLGElement getIndirectObject()
		{
			PhraseElement verbPhrase = (PhraseElement) getFeatureAsElement(InternalFeature.VERB_PHRASE);
			if (verbPhrase != null || verbPhrase is VPPhraseSpec)
			{
				return ((VPPhraseSpec) verbPhrase).getIndirectObject();
			}
			else
			{
			    // return null if VP is coordinated phrase
				return null;
			}
		}
	    // note that addFrontModifier, addPostModifier, addPreModifier are inherited
	    // from PhraseElement
	    // likewise getFrontModifiers, getPostModifiers, getPreModifiers

	    /**
	     * Add a modifier to a clause Use heuristics to decide where it goes
	     * 
	     * @param modifier
	     */
		public override void addModifier(object modifier)
		{
		    // adverb is frontModifier if sentenceModifier
		    // otherwise adverb is preModifier
		    // string which is one lexicographic word is looked up in lexicon,
		    // above rules apply if adverb
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

		    // if no modifier element, must be a complex string
			if (modifierElement == null)
			{
				addPostModifier((string) modifier);
				return;
			}

		    // AdvP is premodifer (probably should look at head to see if sentenceModifier)
			if (modifierElement is AdvPhraseSpec)
			{
				addPreModifier(modifierElement);
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
			    // adverb rules
				if (modifierWord.getFeatureAsBoolean(LexicalFeature.SENTENCE_MODIFIER))
				{
					addFrontModifier(modifierWord);
				}
				else
				{
					addPreModifier(modifierWord);
				}
				return;
			}

		    // default case
			addPostModifier(modifierElement);
		}

	}

}