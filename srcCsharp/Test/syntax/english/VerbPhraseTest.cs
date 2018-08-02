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
 * Contributor(s): Ehud Reiter, Albert Gatt, Dave Wewstwater, Roman Kutlak, Margaret Mitchell, Saad Mahamood.
 *
 * Ported to C# by Gert-Jan de Vries
 */

using System.Collections.Generic;
using NUnit.Framework;
using SimpleNLG.Main.features;
using SimpleNLG.Main.framework;
using SimpleNLG.Main.phrasespec;

namespace SimpleNLG.Test.syntax.english
{
    using DiscourseFunction = DiscourseFunction;
    using Feature = Feature;
    using Form = Form;
    using InternalFeature = InternalFeature;
    using NumberAgreement = NumberAgreement;
    using Person = Person;
    using Tense = Tense;
    using CoordinatedPhraseElement = CoordinatedPhraseElement;
    using NLGElement = NLGElement;
    using PhraseElement = PhraseElement;
    using WordElement = WordElement;
    using SPhraseSpec = SPhraseSpec;
    using VPPhraseSpec = VPPhraseSpec;

    /**
     * These are tests for the verb phrase and coordinate VP classes.
     * @author agatt
     */
    [TestFixture]
    public class VerbPhraseTest : SimpleNLG4Test
    {
        public VerbPhraseTest() : this(null)
        {
        }

        /**
         * Instantiates a new vP test.
         * 
         * @param name
         *            the name
         */
        public VerbPhraseTest(string name) : base(name)
        {
        }


        /**
         * Some tests to check for an early bug which resulted in reduplication of
         * verb particles in the past tense e.g. "fall down down" or "creep up up"
         */
        [Test]
        public virtual void testVerbParticle()
        {
            VPPhraseSpec v = phraseFactory.createVerbPhrase("fall down"); //$NON-NLS-1$

            Assert.AreEqual("down", v.getFeatureAsString(Feature.PARTICLE)); //$NON-NLS-1$

            Assert.AreEqual("fall", ((WordElement) v.getVerb()).BaseForm); //$NON-NLS-1$

            v.setFeature(Feature.TENSE, Tense.PAST);
            v.setFeature(Feature.PERSON, Person.THIRD);
            v.setFeature(Feature.NUMBER, NumberAgreement.PLURAL);

            Assert.AreEqual("fell down", realiser.realise(v).Realisation); //$NON-NLS-1$

            v.setFeature(Feature.FORM, Form.PAST_PARTICIPLE);
            Assert.AreEqual("fallen down", realiser.realise(v).Realisation); //$NON-NLS-1$
        }

        /**
         * Tests for the tense and aspect.
         */
        [Test]
        public virtual void simplePastTest()
        {
            // "fell down"
            fallDown.setFeature(Feature.TENSE, Tense.PAST);
            Assert.AreEqual("fell down", realiser.realise(fallDown).Realisation); //$NON-NLS-1$
        }

        /**
         * Test tense aspect.
         */
        [Test]
        public virtual void tenseAspectTest()
        {
            // had fallen down
            realiser.Lexicon = lexicon;
            fallDown.setFeature(Feature.TENSE, Tense.PAST);
            fallDown.setFeature(Feature.PERFECT, true);

            Assert.AreEqual("had fallen down", realiser.realise(fallDown).Realisation); //$NON-NLS-1$


            // had been falling down
            fallDown.setFeature(Feature.PROGRESSIVE, true);
            Assert.AreEqual("had been falling down", realiser.realise(fallDown).Realisation); //$NON-NLS-1$


            // will have been kicked
            kick.setFeature(Feature.PASSIVE, true);
            kick.setFeature(Feature.PERFECT, true);
            kick.setFeature(Feature.TENSE, Tense.FUTURE);
            Assert.AreEqual("will have been kicked", realiser.realise(kick).Realisation); //$NON-NLS-1$


            // will have been being kicked
            kick.setFeature(Feature.PROGRESSIVE, true);
            Assert.AreEqual("will have been being kicked", realiser.realise(kick).Realisation); //$NON-NLS-1$


            // will not have been being kicked
            kick.setFeature(Feature.NEGATED, true);
            Assert.AreEqual("will not have been being kicked", realiser.realise(kick).Realisation); //$NON-NLS-1$


            // passivisation should suppress the complement
            kick.clearComplements();
            kick.addComplement(man);
            Assert.AreEqual("will not have been being kicked", realiser.realise(kick).Realisation); //$NON-NLS-1$


            // de-passivisation should now give us "will have been kicking the man"
            kick.setFeature(Feature.PASSIVE, false);
            Assert.AreEqual("will not have been kicking the man", realiser.realise(kick).Realisation); //$NON-NLS-1$


            // remove the future tense --
            // this is a test of an earlier bug that would still realise "will"
            kick.setFeature(Feature.TENSE, Tense.PRESENT);
            Assert.AreEqual("has not been kicking the man", realiser.realise(kick).Realisation); //$NON-NLS-1$
        }

        /**
         * Test for realisation of VP complements.
         */
        [Test]
        public virtual void complementationTest()
        {
            // was kissing Mary
            PhraseElement mary = phraseFactory.createNounPhrase("Mary"); //$NON-NLS-1$
            mary.setFeature(InternalFeature.DISCOURSE_FUNCTION, DiscourseFunction.OBJECT);
            kiss.clearComplements();
            kiss.addComplement(mary);
            kiss.setFeature(Feature.PROGRESSIVE, true);
            kiss.setFeature(Feature.TENSE, Tense.PAST);

            Assert.AreEqual("was kissing Mary", realiser.realise(kiss).Realisation); //$NON-NLS-1$

            CoordinatedPhraseElement
                mary2 = new CoordinatedPhraseElement(mary, phraseFactory.createNounPhrase("Susan")); //$NON-NLS-1$

            // add another complement -- should come out as "Mary and Susan"
            kiss.clearComplements();
            kiss.addComplement(mary2);
            Assert.AreEqual("was kissing Mary and Susan", realiser.realise(kiss).Realisation); //$NON-NLS-1$


            // passivise -- should make the direct object complement disappear
            // Note: The verb doesn't come out as plural because agreement
            // is determined by the sentential subjects and this VP isn't inside a
            // sentence
            kiss.setFeature(Feature.PASSIVE, true);
            Assert.AreEqual("was being kissed", realiser.realise(kiss).Realisation); //$NON-NLS-1$


            // make it plural (this is usually taken care of in SPhraseSpec)
            kiss.setFeature(Feature.NUMBER, NumberAgreement.PLURAL);
            Assert.AreEqual("were being kissed", realiser.realise(kiss).Realisation); //$NON-NLS-1$


            // depassivise and add post-mod: yields "was kissing Mary in the room"
            kiss.addPostModifier(inTheRoom);
            kiss.setFeature(Feature.PASSIVE, false);
            kiss.setFeature(Feature.NUMBER, NumberAgreement.SINGULAR);
            Assert.AreEqual("was kissing Mary and Susan in the room", realiser.realise(kiss).Realisation); //$NON-NLS-1$


            // passivise again: should make direct object disappear, but not postMod
            // ="was being kissed in the room"
            kiss.setFeature(Feature.PASSIVE, true);
            kiss.setFeature(Feature.NUMBER, NumberAgreement.PLURAL);
            Assert.AreEqual("were being kissed in the room", realiser.realise(kiss).Realisation); //$NON-NLS-1$
        }

        /**
         * This tests for the default complement ordering, relative to pre and
         * postmodifiers.
         */
        [Test]
        public virtual void complementationTest_2()
        {
            // give the woman the dog
            woman.setFeature(InternalFeature.DISCOURSE_FUNCTION, DiscourseFunction.INDIRECT_OBJECT);
            dog.setFeature(InternalFeature.DISCOURSE_FUNCTION, DiscourseFunction.OBJECT);
            give.clearComplements();
            give.addComplement(dog);
            give.addComplement(woman);
            Assert.AreEqual("gives the woman the dog", realiser.realise(give).Realisation); //$NON-NLS-1$


            // add a few premodifiers and postmodifiers
            give.addPreModifier("slowly"); //$NON-NLS-1$
            give.addPostModifier(behindTheCurtain);
            give.addPostModifier(inTheRoom);
            Assert.AreEqual("slowly gives the woman the dog behind the curtain in the room",
                realiser.realise(give).Realisation); //$NON-NLS-1$


            // reset the arguments
            give.clearComplements();
            give.addComplement(dog);
            CoordinatedPhraseElement womanBoy = new CoordinatedPhraseElement(woman, boy);
            womanBoy.setFeature(InternalFeature.DISCOURSE_FUNCTION, DiscourseFunction.INDIRECT_OBJECT);
            give.addComplement(womanBoy);

            // if we unset the passive, we should get the indirect objects
            // they won't be coordinated
            give.setFeature(Feature.PASSIVE, false);
            Assert.AreEqual("slowly gives the woman and the boy the dog behind the curtain in the room",
                realiser.realise(give).Realisation); //$NON-NLS-1$


            // set them to a coordinate instead
            // set ONLY the complement INDIRECT_OBJECT, leaves OBJECT intact
            give.clearComplements();
            give.addComplement(womanBoy);
            give.addComplement(dog);
            IList<NLGElement> complements = give.getFeatureAsElementList(InternalFeature.COMPLEMENTS);

            int indirectCount = 0;
            foreach (NLGElement eachElement in complements)
            {
                if (DiscourseFunction.INDIRECT_OBJECT.Equals(eachElement.getFeature(InternalFeature.DISCOURSE_FUNCTION))
                )
                {
                    indirectCount++;
                }
            }

            Assert.AreEqual(1, indirectCount); // only one indirect object
            // where
            // there were two before

            Assert.AreEqual("slowly gives the woman and the boy the dog behind the curtain in the room",
                realiser.realise(give).Realisation); //$NON-NLS-1$
        }

        /**
         * Test for complements raised in the passive case.
         */
        [Test]
        public virtual void passiveComplementTest()
        {
            // add some arguments
            dog.setFeature(InternalFeature.DISCOURSE_FUNCTION, DiscourseFunction.OBJECT);
            woman.setFeature(InternalFeature.DISCOURSE_FUNCTION, DiscourseFunction.INDIRECT_OBJECT);
            give.addComplement(dog);
            give.addComplement(woman);
            Assert.AreEqual("gives the woman the dog", realiser.realise(give).Realisation); //$NON-NLS-1$


            // add a few premodifiers and postmodifiers
            give.addPreModifier("slowly"); //$NON-NLS-1$
            give.addPostModifier(behindTheCurtain);
            give.addPostModifier(inTheRoom);
            Assert.AreEqual("slowly gives the woman the dog behind the curtain in the room",
                realiser.realise(give).Realisation); //$NON-NLS-1$


            // passivise: This should suppress "the dog"
            give.clearComplements();
            give.addComplement(dog);
            give.addComplement(woman);
            give.setFeature(Feature.PASSIVE, true);

            Assert.AreEqual("is slowly given the woman behind the curtain in the room",
                realiser.realise(give).Realisation); //$NON-NLS-1$
        }

        /**
         * Test VP with sentential complements. This tests for structures like "said
         * that John was walking"
         */
        [Test]
        public virtual void clausalComplementTest()
        {
            phraseFactory.Lexicon = lexicon;
            SPhraseSpec s = phraseFactory.createClause();

            s.setSubject(phraseFactory.createNounPhrase("John")); //$NON-NLS-1$


            // Create a sentence first
            CoordinatedPhraseElement maryAndSusan = new CoordinatedPhraseElement(phraseFactory.createNounPhrase("Mary"),
                phraseFactory.createNounPhrase("Susan")); //$NON-NLS-1$ - $NON-NLS-1$

            kiss.clearComplements();
            s.VerbPhrase = kiss;
            s.setObject(maryAndSusan);
            s.setFeature(Feature.PROGRESSIVE, true);
            s.setFeature(Feature.TENSE, Tense.PAST);
            s.addPostModifier(inTheRoom);
            Assert.AreEqual("John was kissing Mary and Susan in the room",
                realiser.realise(s).Realisation); //$NON-NLS-1$


            // make the main VP past
            say.setFeature(Feature.TENSE, Tense.PAST);
            Assert.AreEqual("said", realiser.realise(say).Realisation); //$NON-NLS-1$


            // now add the sentence as complement of "say". Should make the sentence
            // subordinate
            // note that sentential punctuation is suppressed
            say.addComplement(s);
            Assert.AreEqual("said that John was kissing Mary and Susan in the room",
                realiser.realise(say).Realisation); //$NON-NLS-1$


            // add a postModifier to the main VP
            // yields [says [that John was kissing Mary and Susan in the room]
            // [behind the curtain]]
            say.addPostModifier(behindTheCurtain);
            Assert.AreEqual("said that John was kissing Mary and Susan in the room behind the curtain",
                realiser.realise(say).Realisation); //$NON-NLS-1$


            // create a new sentential complement
            PhraseElement s2 = phraseFactory.createClause(phraseFactory.createNounPhrase("all"), "be",
                phraseFactory.createAdjectivePhrase("fine")); //$NON-NLS-1$ - $NON-NLS-1$ - $NON-NLS-1$

            s2.setFeature(Feature.TENSE, Tense.FUTURE);
            Assert.AreEqual("all will be fine", realiser.realise(s2).Realisation); //$NON-NLS-1$


            // add the new complement to the VP
            // yields [said [that John was kissing Mary and Susan in the room and
            // all will be fine] [behind the curtain]]
            CoordinatedPhraseElement s3 = new CoordinatedPhraseElement(s, s2);
            say.clearComplements();
            say.addComplement(s3);

            // first with outer complementiser suppressed
            s3.setFeature(Feature.SUPRESSED_COMPLEMENTISER, true);
            Assert.AreEqual(
                "said that John was kissing Mary and Susan in the room " + "and all will be fine behind the curtain",
                realiser.realise(say).Realisation); //$NON-NLS-1$ - $NON-NLS-1$

            setUp();
            s = phraseFactory.createClause();

            s.setSubject(phraseFactory.createNounPhrase("John")); //$NON-NLS-1$


            // Create a sentence first

            s.VerbPhrase = kiss;
            s.setObject(maryAndSusan);
            s.setFeature(Feature.PROGRESSIVE, true);
            s.setFeature(Feature.TENSE, Tense.PAST);
            s.addPostModifier(inTheRoom);
            s2 = phraseFactory.createClause(phraseFactory.createNounPhrase("all"), "be",
                phraseFactory.createAdjectivePhrase("fine")); //$NON-NLS-1$ - $NON-NLS-1$ - $NON-NLS-1$

            s2.setFeature(Feature.TENSE, Tense.FUTURE);
            // then with complementiser not suppressed and not aggregated
            s3 = new CoordinatedPhraseElement(s, s2);
            say.addComplement(s3);
            say.setFeature(Feature.TENSE, Tense.PAST);
            say.addPostModifier(behindTheCurtain);

            Assert.AreEqual(
                "said that John was kissing Mary and Susan in the room and " +
                "that all will be fine behind the curtain",
                realiser.realise(say).Realisation); //$NON-NLS-1$ - $NON-NLS-1$
        }

        /**
         * Test VP coordination and aggregation:
         * <OL>
         * <LI>If the simplenlg.features of a coordinate VP are set, they should be
         * inherited by its daughter VP;</LI>
         * <LI>2. We can aggregate the coordinate VP so it's realised with one
         * wide-scope auxiliary</LI>
         */
        [Test]
        public virtual void coordinationTest()
        {
            // simple case
            kiss.addComplement(dog);
            kick.addComplement(boy);

            CoordinatedPhraseElement coord1 = new CoordinatedPhraseElement(kiss, kick);

            coord1.setFeature(Feature.PERSON, Person.THIRD);
            coord1.setFeature(Feature.TENSE, Tense.PAST);
            Assert.AreEqual("kissed the dog and kicked the boy", realiser.realise(coord1).Realisation); //$NON-NLS-1$


            // with negation: should be inherited by all components
            coord1.setFeature(Feature.NEGATED, true);
            realiser.Lexicon = lexicon;
            Assert.AreEqual("did not kiss the dog and did not kick the boy",
                realiser.realise(coord1).Realisation); //$NON-NLS-1$


            // set a modal
            coord1.setFeature(Feature.MODAL, "could"); //$NON-NLS-1$
            Assert.AreEqual("could not have kissed the dog and could not have kicked the boy",
                realiser.realise(coord1).Realisation); //$NON-NLS-1$


            // set perfect and progressive
            coord1.setFeature(Feature.PERFECT, true);
            coord1.setFeature(Feature.PROGRESSIVE, true);
            Assert.AreEqual("could not have been kissing the dog and " + "could not have been kicking the boy",
                realiser.realise(coord1).Realisation); //$NON-NLS-1$ - $NON-NLS-1$


            // now aggregate
            coord1.setFeature(Feature.AGGREGATE_AUXILIARY, true);
            Assert.AreEqual("could not have been kissing the dog and kicking the boy",
                realiser.realise(coord1).Realisation); //$NON-NLS-1$
        }
    }
}