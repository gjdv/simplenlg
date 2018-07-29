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
using SimpleNLG.Main.features;

namespace SimpleNLG.Main.syntax.english
{

	using ClauseStatus = ClauseStatus;
	using DiscourseFunction = DiscourseFunction;
	using Feature = Feature;
	using Form = Form;
	using InternalFeature = InternalFeature;
	using InterrogativeType = InterrogativeType;
	using NumberAgreement = NumberAgreement;
	using Person = Person;
	using Tense = Tense;
	using CoordinatedPhraseElement = framework.CoordinatedPhraseElement;
	using LexicalCategory = framework.LexicalCategory;
	using ListElement = framework.ListElement;
	using NLGElement = framework.NLGElement;
	using NLGFactory = framework.NLGFactory;
	using PhraseCategory = framework.PhraseCategory;
	using PhraseElement = framework.PhraseElement;
	using SPhraseSpec = phrasespec.SPhraseSpec;
	using VPPhraseSpec = phrasespec.VPPhraseSpec;

    /**
     * <p>
     * This is a helper class containing the main methods for realising the syntax
     * of clauses. It is used exclusively by the <code>SyntaxProcessor</code>.
     * </p>
     * 
     * @author D. Westwater, University of Aberdeen.
     * @version 4.0
     * 
     */
	internal abstract class ClauseHelper
	{
	    /**
	     * The main method for controlling the syntax realisation of clauses.
	     * 
	     * @param parent
	     *            the parent <code>SyntaxProcessor</code> that called this
	     *            method.
	     * @param phrase
	     *            the <code>PhraseElement</code> representation of the clause.
	     * @return the <code>NLGElement</code> representing the realised clause.
	     */
		internal static NLGElement realise(SyntaxProcessor parent, PhraseElement phrase)
		{
			ListElement realisedElement = null;
			NLGFactory phraseFactory = phrase.Factory;
			NLGElement splitVerb = null;
			bool interrogObj = false;

			if (phrase != null)
			{
				realisedElement = new ListElement();
				NLGElement verbElement = phrase.getFeatureAsElement(InternalFeature.VERB_PHRASE);

				if (verbElement == null)
				{
					verbElement = phrase.getHead();
				}

				checkSubjectNumberPerson(phrase, verbElement);
				checkDiscourseFunction(phrase);
				copyFrontModifiers(phrase, verbElement);
				addComplementiser(phrase, parent, realisedElement);
				addCuePhrase(phrase, parent, realisedElement);

				if (phrase.hasFeature(Feature.INTERROGATIVE_TYPE))
				{
					object inter = phrase.getFeature(Feature.INTERROGATIVE_TYPE);
					interrogObj = (InterrogativeType.WHAT_OBJECT.Equals(inter) || InterrogativeType.WHO_OBJECT.Equals(inter) || InterrogativeType.HOW_PREDICATE.Equals(inter) || InterrogativeType.HOW.Equals(inter) || InterrogativeType.WHY.Equals(inter) || InterrogativeType.WHERE.Equals(inter));
					splitVerb = realiseInterrogative(phrase, parent, realisedElement, phraseFactory, verbElement);
				}
				else
				{
					PhraseHelper.realiseList(parent, realisedElement, phrase.getFeatureAsElementList(InternalFeature.FRONT_MODIFIERS), DiscourseFunction.FRONT_MODIFIER);
				}

				addSubjectsToFront(phrase, parent, realisedElement, splitVerb);

				NLGElement passiveSplitVerb = addPassiveComplementsNumberPerson(phrase, parent, realisedElement, verbElement);

				if (passiveSplitVerb != null)
				{
					splitVerb = passiveSplitVerb;
				}

			    // realise verb needs to know if clause is object interrogative
				realiseVerb(phrase, parent, realisedElement, splitVerb, verbElement, interrogObj);
				addPassiveSubjects(phrase, parent, realisedElement, phraseFactory);
				addInterrogativeFrontModifiers(phrase, parent, realisedElement);
				addEndingTo(phrase, parent, realisedElement, phraseFactory);
			}
			return realisedElement;
		}

	    /**
	     * Adds <em>to</em> to the end of interrogatives concerning indirect
	     * objects. For example, <em>who did John give the flower <b>to</b></em>.
	     * 
	     * @param phrase
	     *            the <code>PhraseElement</code> representing this clause.
	     * @param parent
	     *            the parent <code>SyntaxProcessor</code> that will do the
	     *            realisation of the complementiser.
	     * @param realisedElement
	     *            the current realisation of the clause.
	     * @param phraseFactory
	     *            the phrase factory to be used.
	     */
		private static void addEndingTo(PhraseElement phrase, SyntaxProcessor parent, ListElement realisedElement, NLGFactory phraseFactory)
		{

			if (InterrogativeType.WHO_INDIRECT_OBJECT.Equals(phrase.getFeature(Feature.INTERROGATIVE_TYPE)))
			{
				NLGElement word = phraseFactory.createWord("to", new LexicalCategory(LexicalCategory.LexicalCategoryEnum.PREPOSITION)); //$NON-NLS-1$
				realisedElement.addComponent(parent.realise(word));
			}
		}

	    /**
	     * Adds the front modifiers to the end of the clause when dealing with
	     * interrogatives.
	     * 
	     * @param phrase
	     *            the <code>PhraseElement</code> representing this clause.
	     * @param parent
	     *            the parent <code>SyntaxProcessor</code> that will do the
	     *            realisation of the complementiser.
	     * @param realisedElement
	     *            the current realisation of the clause.
	     */
		private static void addInterrogativeFrontModifiers(PhraseElement phrase, SyntaxProcessor parent, ListElement realisedElement)
		{
			NLGElement currentElement = null;
			if (phrase.hasFeature(Feature.INTERROGATIVE_TYPE))
			{
				foreach (NLGElement subject in phrase.getFeatureAsElementList(InternalFeature.FRONT_MODIFIERS))
				{
					currentElement = parent.realise(subject);
					if (currentElement != null)
					{
						currentElement.setFeature(InternalFeature.DISCOURSE_FUNCTION, DiscourseFunction.FRONT_MODIFIER);

						realisedElement.addComponent(currentElement);
					}
				}
			}
		}

	    /**
	     * Realises the subjects of a passive clause.
	     * 
	     * @param phrase
	     *            the <code>PhraseElement</code> representing this clause.
	     * @param parent
	     *            the parent <code>SyntaxProcessor</code> that will do the
	     *            realisation of the complementiser.
	     * @param realisedElement
	     *            the current realisation of the clause.
	     * @param phraseFactory
	     *            the phrase factory to be used.
	     */
		private static void addPassiveSubjects(PhraseElement phrase, SyntaxProcessor parent, ListElement realisedElement, NLGFactory phraseFactory)
		{
			NLGElement currentElement = null;

			if (phrase.getFeatureAsBoolean(Feature.PASSIVE))
			{
				IList<NLGElement> allSubjects = phrase.getFeatureAsElementList(InternalFeature.SUBJECTS);

				if (allSubjects.Any() || phrase.hasFeature(Feature.INTERROGATIVE_TYPE))
				{
					realisedElement.addComponent(parent.realise(phraseFactory.createPrepositionPhrase("by"))); //$NON-NLS-1$
				}

				foreach (NLGElement subject in allSubjects)
				{

					subject.setFeature(Feature.PASSIVE, true);
					if (subject.isA(new PhraseCategory(PhraseCategory.PhraseCategoryEnum.NOUN_PHRASE)) || subject is CoordinatedPhraseElement)
					{
						currentElement = parent.realise(subject);
						if (currentElement != null)
						{
							currentElement.setFeature(InternalFeature.DISCOURSE_FUNCTION, DiscourseFunction.SUBJECT);
							realisedElement.addComponent(currentElement);
						}
					}
				}
			}
		}

	    /**
	     * Realises the verb part of the clause.
	     * 
	     * @param phrase
	     *            the <code>PhraseElement</code> representing this clause.
	     * @param parent
	     *            the parent <code>SyntaxProcessor</code> that will do the
	     *            realisation of the complementiser.
	     * @param realisedElement
	     *            the current realisation of the clause.
	     * @param splitVerb
	     *            an <code>NLGElement</code> representing the subjects that
	     *            should split the verb
	     * @param verbElement
	     *            the <code>NLGElement</code> representing the verb phrase for
	     *            this clause.
	     * @param whObj
	     *            whether the VP is part of an object WH-interrogative
	     */
		private static void realiseVerb(PhraseElement phrase, SyntaxProcessor parent, ListElement realisedElement, NLGElement splitVerb, NLGElement verbElement, bool whObj)
		{

			setVerbFeatures(phrase, verbElement);

			NLGElement currentElement = parent.realise(verbElement);
			if (currentElement != null)
			{
				if (splitVerb == null)
				{
					currentElement.setFeature(InternalFeature.DISCOURSE_FUNCTION, DiscourseFunction.VERB_PHRASE);

					realisedElement.addComponent(currentElement);

				}
				else
				{
					if (currentElement is ListElement)
					{
						IList<NLGElement> children = currentElement.Children;
						currentElement = children[0];
						currentElement.setFeature(InternalFeature.DISCOURSE_FUNCTION, DiscourseFunction.VERB_PHRASE);
						realisedElement.addComponent(currentElement);
						realisedElement.addComponent(splitVerb);

						for (int eachChild = 1; eachChild < children.Count; eachChild++)
						{
							currentElement = children[eachChild];
							currentElement.setFeature(InternalFeature.DISCOURSE_FUNCTION, DiscourseFunction.VERB_PHRASE);
							realisedElement.addComponent(currentElement);
						}
					}
					else
					{
						currentElement.setFeature(InternalFeature.DISCOURSE_FUNCTION, DiscourseFunction.VERB_PHRASE);

						if (whObj)
						{
							realisedElement.addComponent(currentElement);
							realisedElement.addComponent(splitVerb);
						}
						else
						{
							realisedElement.addComponent(splitVerb);
							realisedElement.addComponent(currentElement);
						}
					}
				}
			}
		}

	    /**
	     * Ensures that the verb inherits the features from the clause.
	     * 
	     * @param phrase
	     *            the <code>PhraseElement</code> representing this clause.
	     * @param verbElement
	     *            the <code>NLGElement</code> representing the verb phrase for
	     *            this clause.
	     */
		private static void setVerbFeatures(PhraseElement phrase, NLGElement verbElement)
		{
		    // this routine copies features from the clause to the VP.
		    // it is disabled, as this copying is now done automatically
		    // when features are set in SPhraseSpec
		    // if (verbElement != null) {
		    // verbElement.setFeature(Feature.INTERROGATIVE_TYPE, phrase
		    // .getFeature(Feature.INTERROGATIVE_TYPE));
		    // verbElement.setFeature(InternalFeature.COMPLEMENTS, phrase
		    // .getFeature(InternalFeature.COMPLEMENTS));
		    // verbElement.setFeature(InternalFeature.PREMODIFIERS, phrase
		    // .getFeature(InternalFeature.PREMODIFIERS));
		    // verbElement.setFeature(Feature.FORM, phrase
		    // .getFeature(Feature.FORM));
		    // verbElement.setFeature(Feature.MODAL, phrase
		    // .getFeature(Feature.MODAL));
		    // verbElement.setNegated(phrase.isNegated());
		    // verbElement.setFeature(Feature.PASSIVE, phrase
		    // .getFeature(Feature.PASSIVE));
		    // verbElement.setFeature(Feature.PERFECT, phrase
		    // .getFeature(Feature.PERFECT));
		    // verbElement.setFeature(Feature.PROGRESSIVE, phrase
		    // .getFeature(Feature.PROGRESSIVE));
		    // verbElement.setTense(phrase.getTense());
		    // verbElement.setFeature(Feature.FORM, phrase
		    // .getFeature(Feature.FORM));
		    // verbElement.setFeature(LexicalFeature.GENDER, phrase
		    // .getFeature(LexicalFeature.GENDER));
		    // }
		}

	    /**
	     * Realises the complements of passive clauses; also sets number, person for
	     * passive
	     * 
	     * @param phrase
	     *            the <code>PhraseElement</code> representing this clause.
	     * @param parent
	     *            the parent <code>SyntaxProcessor</code> that will do the
	     *            realisation of the complementiser.
	     * @param realisedElement
	     *            the current realisation of the clause.
	     * @param verbElement
	     *            the <code>NLGElement</code> representing the verb phrase for
	     *            this clause.
	     */
		private static NLGElement addPassiveComplementsNumberPerson(PhraseElement phrase, SyntaxProcessor parent, ListElement realisedElement, NLGElement verbElement)
		{
			object passiveNumber = null;
			object passivePerson = null;
			NLGElement currentElement = null;
			NLGElement splitVerb = null;
			NLGElement verbPhrase = phrase.getFeatureAsElement(InternalFeature.VERB_PHRASE);

		    // count complements to set plural feature if more than one
			int numComps = 0;
			bool coordSubj = false;

			if (phrase.getFeatureAsBoolean(Feature.PASSIVE) && verbPhrase != null && !InterrogativeType.WHAT_OBJECT.Equals(phrase.getFeature(Feature.INTERROGATIVE_TYPE)))
			{

			    // complements of a clause are stored in the VPPhraseSpec
				foreach (NLGElement subject in verbPhrase.getFeatureAsElementList(InternalFeature.COMPLEMENTS))
				{

				    // AG: complement needn't be an NP
				    // subject.isA(PhraseCategory.NOUN_PHRASE) &&
					if (DiscourseFunction.OBJECT.Equals(subject.getFeature(InternalFeature.DISCOURSE_FUNCTION)))
					{
						subject.setFeature(Feature.PASSIVE, true);
						numComps++;
						currentElement = parent.realise(subject);

						if (currentElement != null)
						{
							currentElement.setFeature(InternalFeature.DISCOURSE_FUNCTION, DiscourseFunction.OBJECT);

							if (phrase.hasFeature(Feature.INTERROGATIVE_TYPE))
							{
								splitVerb = currentElement;
							}
							else
							{
								realisedElement.addComponent(currentElement);
							}
						}

					    // flag if passive subject is coordinated with an "and"
						if (!coordSubj && subject is CoordinatedPhraseElement)
						{
							string conj = ((CoordinatedPhraseElement) subject).Conjunction;
							coordSubj = (!ReferenceEquals(conj, null) && conj.Equals("and"));
						}

						if (passiveNumber == null)
						{
							passiveNumber = subject.getFeature(Feature.NUMBER);
						}
						else
						{
							passiveNumber = NumberAgreement.PLURAL;
						}

						if (Person.FIRST.Equals(subject.getFeature(Feature.PERSON)))
						{
							passivePerson = Person.FIRST;
						}
						else if (Person.SECOND.Equals(subject.getFeature(Feature.PERSON)) && !Person.FIRST.Equals(passivePerson))
						{
							passivePerson = Person.SECOND;
						}
						else if (passivePerson == null)
						{
							passivePerson = Person.THIRD;
						}

						if (Form.GERUND.Equals(phrase.getFeature(Feature.FORM)) && !phrase.getFeatureAsBoolean(Feature.SUPPRESS_GENITIVE_IN_GERUND))
						{
							subject.setFeature(Feature.POSSESSIVE, true);
						}
					}
				}
			}

			if (verbElement != null)
			{
				if (passivePerson != null)
				{
					verbElement.setFeature(Feature.PERSON, passivePerson);
				    // below commented out. for non-passive, number and person set
				    // by checkSubjectNumberPerson
				    // } else {
				    // verbElement.setFeature(Feature.PERSON, phrase
				    // .getFeature(Feature.PERSON));
				}

				if (numComps > 1 || coordSubj)
				{
					verbElement.setFeature(Feature.NUMBER, NumberAgreement.PLURAL);
				}
				else if (passiveNumber != null)
				{
					verbElement.setFeature(Feature.NUMBER, passiveNumber);
				}
			}
			return splitVerb;
		}

	    /**
	     * Adds the subjects to the beginning of the clause unless the clause is
	     * infinitive, imperative or passive, or the subjects split the verb.
	     * 
	     * @param phrase
	     *            the <code>PhraseElement</code> representing this clause.
	     * @param parent
	     *            the parent <code>SyntaxProcessor</code> that will do the
	     *            realisation of the complementiser.
	     * @param realisedElement
	     *            the current realisation of the clause.
	     * @param splitVerb
	     *            an <code>NLGElement</code> representing the subjects that
	     *            should split the verb
	     */
		private static void addSubjectsToFront(PhraseElement phrase, SyntaxProcessor parent, ListElement realisedElement, NLGElement splitVerb)
		{
			if (!Form.INFINITIVE.Equals(phrase.getFeature(Feature.FORM)) && !Form.IMPERATIVE.Equals(phrase.getFeature(Feature.FORM)) && !phrase.getFeatureAsBoolean(Feature.PASSIVE) && splitVerb == null)
			{
				realisedElement.addComponents(realiseSubjects(phrase, parent).Children);
			}
		}

	    /**
	     * Realises the subjects for the clause.
	     * 
	     * @param phrase
	     *            the <code>PhraseElement</code> representing this clause.
	     * @param parent
	     *            the parent <code>SyntaxProcessor</code> that will do the
	     *            realisation of the complementiser.
	     * @param realisedElement
	     *            the current realisation of the clause.
	     */
		private static ListElement realiseSubjects(PhraseElement phrase, SyntaxProcessor parent)
		{

			NLGElement currentElement = null;
			ListElement realisedElement = new ListElement();

			foreach (NLGElement subject in phrase.getFeatureAsElementList(InternalFeature.SUBJECTS))
			{

				subject.setFeature(InternalFeature.DISCOURSE_FUNCTION, DiscourseFunction.SUBJECT);
				if (Form.GERUND.Equals(phrase.getFeature(Feature.FORM)) && !phrase.getFeatureAsBoolean(Feature.SUPPRESS_GENITIVE_IN_GERUND))
				{
					subject.setFeature(Feature.POSSESSIVE, true);
				}
				currentElement = parent.realise(subject);
				if (currentElement != null)
				{
					realisedElement.addComponent(currentElement);
				}
			}
			return realisedElement;
		}

	    /**
	     * This is the main controlling method for handling interrogative clauses.
	     * The actual steps taken are dependent on the type of question being asked.
	     * The method also determines if there is a subject that will split the verb
	     * group of the clause. For example, the clause
	     * <em>the man <b>should give</b> the woman the flower</em> has the verb
	     * group indicated in <b>bold</b>. The phrase is rearranged as yes/no
	     * question as
	     * <em><b>should</b> the man <b>give</b> the woman the flower</em> with the
	     * subject <em>the man</em> splitting the verb group.
	     * 
	     * @param phrase
	     *            the <code>PhraseElement</code> representing this clause.
	     * @param parent
	     *            the parent <code>SyntaxProcessor</code> that will do the
	     *            realisation of the complementiser.
	     * @param realisedElement
	     *            the current realisation of the clause.
	     * @param phraseFactory
	     *            the phrase factory to be used.
	     * @param verbElement
	     *            the <code>NLGElement</code> representing the verb phrase for
	     *            this clause.
	     * @return an <code>NLGElement</code> representing a subject that should
	     *         split the verb
	     */
		private static NLGElement realiseInterrogative(PhraseElement phrase, SyntaxProcessor parent, ListElement realisedElement, NLGFactory phraseFactory, NLGElement verbElement)
		{
			NLGElement splitVerb = null;

			if (phrase.Parent != null)
			{
				phrase.Parent.setFeature(InternalFeature.INTERROGATIVE, true);
			}

			object type = phrase.getFeature(Feature.INTERROGATIVE_TYPE);

			if (type is InterrogativeType)
			{
				switch ((InterrogativeType) type)
				{
				case InterrogativeType.YES_NO:
					splitVerb = realiseYesNo(phrase, parent, verbElement, phraseFactory, realisedElement);
					break;

				case InterrogativeType.WHO_SUBJECT :
				case InterrogativeType.WHAT_SUBJECT :
					realiseInterrogativeKeyWord(((InterrogativeType) type).getString(), new LexicalCategory(LexicalCategory.LexicalCategoryEnum.PRONOUN), parent, realisedElement, phraseFactory); //$NON-NLS-1$
					phrase.removeFeature(InternalFeature.SUBJECTS);
					break;

				case InterrogativeType.HOW_MANY :
					realiseInterrogativeKeyWord("how", new LexicalCategory(LexicalCategory.LexicalCategoryEnum.PRONOUN), parent, realisedElement, phraseFactory); //$NON-NLS-1$
					realiseInterrogativeKeyWord("many", new LexicalCategory(LexicalCategory.LexicalCategoryEnum.ADVERB), parent, realisedElement, phraseFactory); //$NON-NLS-1$
					break;

				case InterrogativeType.HOW :
				case InterrogativeType.WHY :
				case InterrogativeType.WHERE :
				case InterrogativeType.WHO_OBJECT :
				case InterrogativeType.WHO_INDIRECT_OBJECT :
				case InterrogativeType.WHAT_OBJECT :
					splitVerb = realiseObjectWHInterrogative(((InterrogativeType) type).getString(), phrase, parent, realisedElement, phraseFactory);
					break;

				case InterrogativeType.HOW_PREDICATE :
					splitVerb = realiseObjectWHInterrogative("how", phrase, parent, realisedElement, phraseFactory);
					break;

				default :
					break;
				}
			}

			return splitVerb;
		}

	    /*
	     * Check if a sentence has an auxiliary (needed to relise questions
	     * correctly)
	     */
		private static bool hasAuxiliary(PhraseElement phrase)
		{
			return phrase.hasFeature(Feature.MODAL) || phrase.getFeatureAsBoolean(Feature.PERFECT) || phrase.getFeatureAsBoolean(Feature.PROGRESSIVE) || Tense.FUTURE.Equals(phrase.getFeature(Feature.TENSE));
		}

	    /**
	     * Controls the realisation of <em>wh</em> object questions.
	     * 
	     * @param keyword
	     *            the wh word
	     * @param phrase
	     *            the <code>PhraseElement</code> representing this clause.
	     * @param parent
	     *            the parent <code>SyntaxProcessor</code> that will do the
	     *            realisation of the complementiser.
	     * @param realisedElement
	     *            the current realisation of the clause.
	     * @param phraseFactory
	     *            the phrase factory to be used.
	     * @param subjects
	     *            the <code>List</code> of subjects in the clause.
	     * @return an <code>NLGElement</code> representing a subject that should
	     *         split the verb
	     */
		private static NLGElement realiseObjectWHInterrogative(string keyword, PhraseElement phrase, SyntaxProcessor parent, ListElement realisedElement, NLGFactory phraseFactory)
		{
			NLGElement splitVerb = null;
			realiseInterrogativeKeyWord(keyword, new LexicalCategory(LexicalCategory.LexicalCategoryEnum.PRONOUN), parent, realisedElement, phraseFactory); //$NON-NLS-1$


		    // if (!Tense.FUTURE.equals(phrase.getFeature(Feature.TENSE)) && !copular) {
			if (!hasAuxiliary(phrase) && !VerbPhraseHelper.isCopular(phrase))
			{
				addDoAuxiliary(phrase, parent, phraseFactory, realisedElement);

			}
			else if (!phrase.getFeatureAsBoolean(Feature.PASSIVE))
			{
				splitVerb = realiseSubjects(phrase, parent);
			}

			return splitVerb;
		}

	    /**
	     * Adds a <em>do</em> verb to the realisation of this clause.
	     * 
	     * @param phrase
	     *            the <code>PhraseElement</code> representing this clause.
	     * @param parent
	     *            the parent <code>SyntaxProcessor</code> that will do the
	     *            realisation of the complementiser.
	     * @param realisedElement
	     *            the current realisation of the clause.
	     * @param phraseFactory
	     *            the phrase factory to be used.
	     */
		private static void addDoAuxiliary(PhraseElement phrase, SyntaxProcessor parent, NLGFactory phraseFactory, ListElement realisedElement)
		{

			PhraseElement doPhrase = phraseFactory.createVerbPhrase("do"); //$NON-NLS-1$
			doPhrase.setFeature(Feature.TENSE, phrase.getFeature(Feature.TENSE));
			doPhrase.setFeature(Feature.PERSON, phrase.getFeature(Feature.PERSON));
			doPhrase.setFeature(Feature.NUMBER, phrase.getFeature(Feature.NUMBER));
			realisedElement.addComponent(parent.realise(doPhrase));
		}

	    /**
	     * Realises the key word of the interrogative. For example, <em>who</em>,
	     * <em>what</em>
	     * 
	     * @param keyWord
	     *            the key word of the interrogative.
	     * @param cat
	     *            the category (usually pronoun, but not in the case of
	     *            "how many")
	     * @param parent
	     *            the parent <code>SyntaxProcessor</code> that will do the
	     *            realisation of the complementiser.
	     * @param realisedElement
	     *            the current realisation of the clause.
	     * @param phraseFactory
	     *            the phrase factory to be used.
	     */
		private static void realiseInterrogativeKeyWord(string keyWord, LexicalCategory cat, SyntaxProcessor parent, ListElement realisedElement, NLGFactory phraseFactory)
		{

			if (!ReferenceEquals(keyWord, null))
			{
				NLGElement question = phraseFactory.createWord(keyWord, cat);
				NLGElement currentElement = parent.realise(question);

				if (currentElement != null)
				{
					realisedElement.addComponent(currentElement);
				}
			}
		}

	    /**
	     * Performs the realisation for YES/NO types of questions. This may involve
	     * adding an optional <em>do</em> auxiliary verb to the beginning of the
	     * clause. The method also determines if there is a subject that will split
	     * the verb group of the clause. For example, the clause
	     * <em>the man <b>should give</b> the woman the flower</em> has the verb
	     * group indicated in <b>bold</b>. The phrase is rearranged as yes/no
	     * question as
	     * <em><b>should</b> the man <b>give</b> the woman the flower</em> with the
	     * subject <em>the man</em> splitting the verb group.
	     * 
	     * 
	     * @param phrase
	     *            the <code>PhraseElement</code> representing this clause.
	     * @param parent
	     *            the parent <code>SyntaxProcessor</code> that will do the
	     *            realisation of the complementiser.
	     * @param realisedElement
	     *            the current realisation of the clause.
	     * @param phraseFactory
	     *            the phrase factory to be used.
	     * @param verbElement
	     *            the <code>NLGElement</code> representing the verb phrase for
	     *            this clause.
	     * @param subjects
	     *            the <code>List</code> of subjects in the clause.
	     * @return an <code>NLGElement</code> representing a subject that should
	     *         split the verb
	     */
		private static NLGElement realiseYesNo(PhraseElement phrase, SyntaxProcessor parent, NLGElement verbElement, NLGFactory phraseFactory, ListElement realisedElement)
		{

			NLGElement splitVerb = null;

			if (!(verbElement is VPPhraseSpec && VerbPhraseHelper.isCopular(((VPPhraseSpec) verbElement).getVerb())) && !phrase.getFeatureAsBoolean(Feature.PROGRESSIVE) && !phrase.hasFeature(Feature.MODAL) && !Tense.FUTURE.Equals(phrase.getFeature(Feature.TENSE)) && !phrase.getFeatureAsBoolean(Feature.NEGATED) && !phrase.getFeatureAsBoolean(Feature.PASSIVE))
			{
				addDoAuxiliary(phrase, parent, phraseFactory, realisedElement);
			}
			else
			{
				splitVerb = realiseSubjects(phrase, parent);
			}
			return splitVerb;
		}

	    /**
	     * Realises the cue phrase for the clause if it exists.
	     * 
	     * @param phrase
	     *            the <code>PhraseElement</code> representing this clause.
	     * @param parent
	     *            the parent <code>SyntaxProcessor</code> that will do the
	     *            realisation of the complementiser.
	     * @param realisedElement
	     *            the current realisation of the clause.
	     */
		private static void addCuePhrase(PhraseElement phrase, SyntaxProcessor parent, ListElement realisedElement)
		{

			NLGElement currentElement = parent.realise(phrase.getFeatureAsElement(Feature.CUE_PHRASE));

			if (currentElement != null)
			{
				currentElement.setFeature(InternalFeature.DISCOURSE_FUNCTION, DiscourseFunction.CUE_PHRASE);
				realisedElement.addComponent(currentElement);
			}
		}

	    /**
	     * Checks to see if this clause is a subordinate clause. If it is then the
	     * complementiser is added as a component to the realised element
	     * <b>unless</b> the complementiser has been suppressed.
	     * 
	     * @param phrase
	     *            the <code>PhraseElement</code> representing this clause.
	     * @param parent
	     *            the parent <code>SyntaxProcessor</code> that will do the
	     *            realisation of the complementiser.
	     * @param realisedElement
	     *            the current realisation of the clause.
	     */
		private static void addComplementiser(PhraseElement phrase, SyntaxProcessor parent, ListElement realisedElement)
		{

			NLGElement currentElement;

			if (ClauseStatus.SUBORDINATE.Equals(phrase.getFeature(InternalFeature.CLAUSE_STATUS)) && !phrase.getFeatureAsBoolean(Feature.SUPRESSED_COMPLEMENTISER))
			{

				currentElement = parent.realise(phrase.getFeatureAsElement(Feature.COMPLEMENTISER));

				if (currentElement != null)
				{
					realisedElement.addComponent(currentElement);
				}
			}
		}

	    /**
	     * Copies the front modifiers of the clause to the list of post-modifiers of
	     * the verb only if the phrase has infinitive form.
	     * 
	     * @param phrase
	     *            the <code>PhraseElement</code> representing this clause.
	     * @param verbElement
	     *            the <code>NLGElement</code> representing the verb phrase for
	     *            this clause.
	     */
		private static void copyFrontModifiers(PhraseElement phrase, NLGElement verbElement)
		{
			IList<NLGElement> frontModifiers = phrase.getFeatureAsElementList(InternalFeature.FRONT_MODIFIERS);
			object clauseForm = phrase.getFeature(Feature.FORM);

		    // bug fix by Chris Howell (Agfa) -- do not overwrite existing post-mods in the VP
			if (verbElement != null)
			{
				IList<NLGElement> phrasePostModifiers = phrase.getFeatureAsElementList(InternalFeature.POSTMODIFIERS);

				if (verbElement is PhraseElement)
				{
					IList<NLGElement> verbPostModifiers = verbElement.getFeatureAsElementList(InternalFeature.POSTMODIFIERS);

					foreach (NLGElement eachModifier in phrasePostModifiers)
					{

					    // need to check that VP doesn't already contain the
					    // post-modifier
					    // this only happens if the phrase has already been realised
					    // and later modified, with realiser called again. In that
					    // case, postmods will be copied over twice
						if (!verbPostModifiers.Contains(eachModifier))
						{
							((PhraseElement) verbElement).addPostModifier(eachModifier);
						}
					}
				}
			}

		    // if (verbElement != null) {
		    // verbElement.setFeature(InternalFeature.POSTMODIFIERS, phrase
		    // .getFeature(InternalFeature.POSTMODIFIERS));
		    // }

			if (Form.INFINITIVE.Equals(clauseForm))
			{
				phrase.setFeature(Feature.SUPRESSED_COMPLEMENTISER, true);

				foreach (NLGElement eachModifier in frontModifiers)
				{
					if (verbElement is PhraseElement)
					{
						((PhraseElement) verbElement).addPostModifier(eachModifier);
					}
				}
				phrase.removeFeature(InternalFeature.FRONT_MODIFIERS);
				if (verbElement != null)
				{
					verbElement.setFeature(InternalFeature.NON_MORPH, true);
				}
			}
		}

	    /**
	     * Checks the discourse function of the clause and alters the form of the
	     * clause as necessary. The following algorithm is used: <br>
	     * 
	     * <pre>
	     * If the clause represents a direct or indirect object then 
	     *      If form is currently Imperative then
	     *           Set form to Infinitive
	     *           Suppress the complementiser
	     *      If form is currently Gerund and there are no subjects
	     *      	 Suppress the complementiser
	     * If the clause represents a subject then
	     *      Set the form to be Gerund
	     *      Suppress the complementiser
	     * </pre>
	     * 
	     * @param phrase
	     *            the <code>PhraseElement</code> representing this clause.
	     */
		private static void checkDiscourseFunction(PhraseElement phrase)
		{
			IList<NLGElement> subjects = phrase.getFeatureAsElementList(InternalFeature.SUBJECTS);
			object clauseForm = phrase.getFeature(Feature.FORM);
			object discourseValue = phrase.getFeature(InternalFeature.DISCOURSE_FUNCTION);

			if (DiscourseFunction.OBJECT.Equals(discourseValue) || DiscourseFunction.INDIRECT_OBJECT.Equals(discourseValue))
			{

				if (Form.IMPERATIVE.Equals(clauseForm))
				{
					phrase.setFeature(Feature.SUPRESSED_COMPLEMENTISER, true);
					phrase.setFeature(Feature.FORM, Form.INFINITIVE);
				}
				else if (Form.GERUND.Equals(clauseForm) && subjects.Count == 0)
				{
					phrase.setFeature(Feature.SUPRESSED_COMPLEMENTISER, true);
				}
			}
			else if (DiscourseFunction.SUBJECT.Equals(discourseValue))
			{
				phrase.setFeature(Feature.FORM, Form.GERUND);
				phrase.setFeature(Feature.SUPRESSED_COMPLEMENTISER, true);
			}
		}

	    /**
	     * Checks the subjects of the phrase to determine if there is more than one
	     * subject. This ensures that the verb phrase is correctly set. Also set
	     * person correctly
	     * 
	     * @param phrase
	     *            the <code>PhraseElement</code> representing this clause.
	     * @param verbElement
	     *            the <code>NLGElement</code> representing the verb phrase for
	     *            this clause.
	     */
		private static void checkSubjectNumberPerson(PhraseElement phrase, NLGElement verbElement)
		{
			NLGElement currentElement = null;
			IList<NLGElement> subjects = phrase.getFeatureAsElementList(InternalFeature.SUBJECTS);
			bool pluralSubjects = false;
			Person? person = null;

			if (subjects != null)
			{
				switch (subjects.Count)
				{
				case 0 :
					break;

				case 1 :
					currentElement = subjects[0];
				    // coordinated NP with "and" are plural (not coordinated NP with "or")
					if (currentElement is CoordinatedPhraseElement && ((CoordinatedPhraseElement) currentElement).checkIfPlural())
					{
						pluralSubjects = true;
					}
					else if (((NumberAgreement?)currentElement.getFeature(Feature.NUMBER) == NumberAgreement.PLURAL) && !(currentElement is SPhraseSpec)) // ER mod-
					{
																		 // ER mod-clauses are singular as NPs, even if they are plural internally
						pluralSubjects = true;
					}
					else if (currentElement.isA(new PhraseCategory(PhraseCategory.PhraseCategoryEnum.NOUN_PHRASE)))
					{
						NLGElement currentHead = currentElement.getFeatureAsElement(InternalFeature.HEAD);
						person = (Person?) currentElement.getFeature(Feature.PERSON);

						if (currentHead == null)
						{
						    // subject is null and therefore is not gonna be plural
							pluralSubjects = false;
						}
						else if ((NumberAgreement?)currentHead.getFeature(Feature.NUMBER) == NumberAgreement.PLURAL)
						{
							pluralSubjects = true;
						}
						else if (currentHead is ListElement)
						{
							pluralSubjects = true;
						    /*
						     * } else if (currentElement instanceof
						     * CoordinatedPhraseElement &&
						     * "and".equals(currentElement.getFeatureAsString(
						     * //$NON-NLS-1$ Feature.CONJUNCTION))) { pluralSubjects
						     * = true;
						     */
						}
					}
					break;

				default :
					pluralSubjects = true;
					break;
				}
			}
			if (verbElement != null)
			{
				verbElement.setFeature(Feature.NUMBER, pluralSubjects ? NumberAgreement.PLURAL : (NumberAgreement?)phrase.getFeature(Feature.NUMBER));
				if (person != null)
				{
					verbElement.setFeature(Feature.PERSON, person);
				}
			}
		}
	}

}