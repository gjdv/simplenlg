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

using NUnit.Framework;
using SimpleNLG.Main.features;
using SimpleNLG.Main.framework;
using SimpleNLG.Main.phrasespec;
using Assert = NUnit.Framework.Assert;

namespace SimpleNLG.Test.syntax.english
{
    using ClauseStatus = ClauseStatus;
    using DiscourseFunction = DiscourseFunction;
    using Feature = Feature;
    using Form = Form;
    using InternalFeature = InternalFeature;
    using NumberAgreement = NumberAgreement;
    using Tense = Tense;
    using CoordinatedPhraseElement = CoordinatedPhraseElement;
    using LexicalCategory = LexicalCategory;
    using NLGElement = NLGElement;
    using PhraseCategory = PhraseCategory;
    using PhraseElement = PhraseElement;
    using AdjPhraseSpec = AdjPhraseSpec;
    using AdvPhraseSpec = AdvPhraseSpec;
    using NPPhraseSpec = NPPhraseSpec;
    using PPPhraseSpec = PPPhraseSpec;
    using SPhraseSpec = SPhraseSpec;
    using VPPhraseSpec = VPPhraseSpec;

    // TODO: Auto-generated Javadoc
    /**
     * The Class STest.
     */
    [TestFixture]
    public class ClauseTest : SimpleNLG4Test
    {
        // set up a few more fixtures
        /** The s4. */
        internal SPhraseSpec s1, s2, s3, s4;

        public ClauseTest() : this(null)
        {
        }

        /*
         * Instantiates a new s test.
         *
         * @param name
         *            the name
         */
        public ClauseTest(string name) : base(name)
        {
        }

        /*
         * (non-Javadoc)
         *
         * @see simplenlg.test.SimplenlgTest#setUp()
         */
        [SetUp]
        public override void setUp()
        {
            base.setUp();

            // the woman kisses the man
            s1 = phraseFactory.createClause();
            s1.setSubject(woman);
            s1.VerbPhrase = kiss;
            s1.setObject(man);

            // there is the dog on the rock
            s2 = phraseFactory.createClause();
            s2.setSubject("there"); //$NON-NLS-1$
            s2.setVerb("be"); //$NON-NLS-1$
            s2.setObject(dog);
            s2.addPostModifier(onTheRock);

            // the man gives the woman John's flower
            s3 = phraseFactory.createClause();
            s3.setSubject(man);
            s3.VerbPhrase = give;

            NPPhraseSpec flower = phraseFactory.createNounPhrase("flower"); //$NON-NLS-1$
            NPPhraseSpec john = phraseFactory.createNounPhrase("John"); //$NON-NLS-1$
            john.setFeature(Feature.POSSESSIVE, true);
            flower.setFeature(InternalFeature.SPECIFIER, john);
            s3.setObject(flower);
            s3.setIndirectObject(woman);

            s4 = phraseFactory.createClause();
            s4.setFeature(Feature.CUE_PHRASE, "however"); //$NON-NLS-1$
            s4.addFrontModifier("tomorrow"); //$NON-NLS-1$

            CoordinatedPhraseElement subject =
                phraseFactory.createCoordinatedPhrase(phraseFactory.createNounPhrase("Jane"),
                    phraseFactory.createNounPhrase("Andrew")); //$NON-NLS-1$ - $NON-NLS-1$

            s4.setSubject(subject);

            PhraseElement pick = phraseFactory.createVerbPhrase("pick up"); //$NON-NLS-1$
            s4.VerbPhrase = pick;
            s4.setObject("the balls"); //$NON-NLS-1$
            s4.addPostModifier("in the shop"); //$NON-NLS-1$
            s4.setFeature(Feature.TENSE, Tense.FUTURE);
        }

        [OneTimeTearDown]
        public override void tearDown()
        {
            base.tearDown();

            s1 = null;
            s2 = null;
            s3 = null;
            s4 = null;
        }


        /**
         * Initial test for basic sentences.
         */
        [Test]
        public virtual void testBasic()
        {
            Assert.AreEqual("the woman kisses the man", realiser.realise(s1).Realisation); //$NON-NLS-1$
            Assert.AreEqual("there is the dog on the rock", realiser.realise(s2).Realisation); //$NON-NLS-1$

            setUp();
            Assert.AreEqual("the man gives the woman John's flower", realiser.realise(s3).Realisation); //$NON-NLS-1$
            Assert.AreEqual("however tomorrow Jane and Andrew will pick up the balls in the shop",
                realiser.realise(s4).Realisation); //$NON-NLS-1$
        }

        /**
         * Test did not
         */
        [Test]
        public virtual void testDidNot()
        {
            PhraseElement s = phraseFactory.createClause("John", "eat");
            s.setFeature(Feature.TENSE, Tense.PAST);
            s.setFeature(Feature.NEGATED, true);

            Assert.AreEqual("John did not eat", realiser.realise(s).Realisation); //$NON-NLS-1$
        }

        /**
         * Test did not
         */
        [Test]
        public virtual void testVPNegation()
        {
            PhraseElement vp = phraseFactory.createVerbPhrase("lie");
            vp.setFeature(Feature.TENSE, Tense.PAST);
            vp.setFeature(Feature.NEGATED, true);
            PhraseElement compl = phraseFactory.createVerbPhrase("etherize");
            compl.setFeature(Feature.TENSE, Tense.PAST);
            vp.Complement = compl;

            SPhraseSpec s = phraseFactory.createClause(phraseFactory.createNounPhrase("the", "patient"), vp);

            Assert.AreEqual("the patient did not lie etherized", realiser.realise(s).Realisation); //$NON-NLS-1$
        }

        /**
         * Test that pronominal args are being correctly cast as NPs.
         */
        [Test]
        public virtual void testPronounArguments()
        {
            // the subject of s2 should have been cast into a pronominal NP
            NLGElement subj = s2.getFeatureAsElementList(InternalFeature.SUBJECTS)[0];
            Assert.IsTrue(subj.isA(new PhraseCategory(PhraseCategory.PhraseCategoryEnum.NOUN_PHRASE)));
            // Assert.assertTrue(LexicalCategory.PRONOUN.equals(((PhraseElement) subj).getCategory()));
        }

        /**
         * Tests for setting tense, aspect and passive from the sentence interface.
         */
        [Test]
        public virtual void testTenses()
        {
            // simple past
            s3.setFeature(Feature.TENSE, Tense.PAST);
            Assert.AreEqual("the man gave the woman John's flower", realiser.realise(s3).Realisation); //$NON-NLS-1$


            // perfect
            s3.setFeature(Feature.PERFECT, true);
            Assert.AreEqual("the man had given the woman John's flower",
                realiser.realise(s3).Realisation); //$NON-NLS-1$


            // negation
            s3.setFeature(Feature.NEGATED, true);
            Assert.AreEqual("the man had not given the woman John's flower",
                realiser.realise(s3).Realisation); //$NON-NLS-1$

            s3.setFeature(Feature.PROGRESSIVE, true);
            Assert.AreEqual("the man had not been giving the woman John's flower",
                realiser.realise(s3).Realisation); //$NON-NLS-1$


            // passivisation with direct and indirect object
            s3.setFeature(Feature.PASSIVE, true);
            // Assert.assertEquals(
            //				"John's flower had not been being given the woman by the man", //$NON-NLS-1$
            // this.realiser.realise(this.s3).getRealisation());
        }

        /**
         * Test what happens when a sentence is subordinated as complement of a
         * verb.
         */
        [Test]
        public virtual void testSubordination()
        {
            // subordinate sentence by setting it as complement of a verb

            say.addComplement(s3);
            // check the getter

            Assert.AreEqual(ClauseStatus.SUBORDINATE, s3.getFeature(InternalFeature.CLAUSE_STATUS));

            // check realisation
            Assert.AreEqual("says that the man gives the woman John's flower",
                realiser.realise(say).Realisation); //$NON-NLS-1$
        }

        /**
         * Test the various forms of a sentence, including subordinates.
         */
        [Test]
        public virtual void testForm()
        {
            // check the getter method

            Assert.AreEqual(Form.NORMAL, s1.getFeatureAsElement(InternalFeature.VERB_PHRASE).getFeature(Feature.FORM));

            // infinitive
            s1.setFeature(Feature.FORM, Form.INFINITIVE);
            Assert.AreEqual("to kiss the man", realiser.realise(s1).Realisation); //$NON-NLS-1$

            // gerund with "there"
            s2.setFeature(Feature.FORM, Form.GERUND);
            Assert.AreEqual("there being the dog on the rock", realiser.realise(s2).Realisation); //$NON-NLS-1$


            // gerund with possessive
            s3.setFeature(Feature.FORM, Form.GERUND);
            Assert.AreEqual("the man's giving the woman John's flower", realiser.realise(s3).Realisation); //$NON-NLS-1$


            // imperative
            s3.setFeature(Feature.FORM, Form.IMPERATIVE);

            Assert.AreEqual("give the woman John's flower", realiser.realise(s3).Realisation); //$NON-NLS-1$


            // subordinating the imperative to a verb should turn it to infinitive
            say.addComplement(s3);

            Assert.AreEqual("says to give the woman John's flower", realiser.realise(say).Realisation); //$NON-NLS-1$


            // imperative -- case II
            s4.setFeature(Feature.FORM, Form.IMPERATIVE);
            Assert.AreEqual("however tomorrow pick up the balls in the shop",
                realiser.realise(s4).Realisation); //$NON-NLS-1$


            // infinitive -- case II
            s4 = phraseFactory.createClause();
            s4.setFeature(Feature.CUE_PHRASE, "however"); //$NON-NLS-1$
            s4.addFrontModifier("tomorrow"); //$NON-NLS-1$

            CoordinatedPhraseElement subject = new CoordinatedPhraseElement(phraseFactory.createNounPhrase("Jane"),
                phraseFactory.createNounPhrase("Andrew")); //$NON-NLS-1$ - $NON-NLS-1$

            s4.setFeature(InternalFeature.SUBJECTS, subject);

            PhraseElement pick = phraseFactory.createVerbPhrase("pick up"); //$NON-NLS-1$
            s4.setFeature(InternalFeature.VERB_PHRASE, pick);
            s4.setObject("the balls"); //$NON-NLS-1$
            s4.addPostModifier("in the shop"); //$NON-NLS-1$
            s4.setFeature(Feature.TENSE, Tense.FUTURE);
            s4.setFeature(Feature.FORM, Form.INFINITIVE);
            Assert.AreEqual("however to pick up the balls in the shop tomorrow",
                realiser.realise(s4).Realisation); //$NON-NLS-1$
        }

        /**
         * Slightly more complex tests for forms.
         */
        [Test]
        public virtual void testForm2()
        {
            // set s4 as subject of a new sentence
            SPhraseSpec temp = phraseFactory.createClause(s4, "be", "recommended"); //$NON-NLS-1$ - $NON-NLS-1$

            Assert.AreEqual("however tomorrow Jane and Andrew's picking up the " + "balls in the shop is recommended",
                realiser.realise(temp).Realisation); //$NON-NLS-1$ - $NON-NLS-1$


            // compose this with a new sentence
            // ER - switched direct and indirect object in sentence
            SPhraseSpec temp2 = phraseFactory.createClause("I", "tell", temp); //$NON-NLS-1$ //$NON-NLS-2$
            temp2.setFeature(Feature.TENSE, Tense.FUTURE);

            PhraseElement indirectObject = phraseFactory.createNounPhrase("John"); //$NON-NLS-1$

            temp2.setIndirectObject(indirectObject);

            Assert.AreEqual(
                "I will tell John that however tomorrow Jane and " + "Andrew's picking up the balls in the shop is " +
                "recommended", realiser.realise(temp2).Realisation); //$NON-NLS-1$ - $NON-NLS-1$ - $NON-NLS-1$


            // turn s4 to imperative and put it in indirect object position

            s4 = phraseFactory.createClause();
            s4.setFeature(Feature.CUE_PHRASE, "however"); //$NON-NLS-1$
            s4.addFrontModifier("tomorrow"); //$NON-NLS-1$

            CoordinatedPhraseElement subject = new CoordinatedPhraseElement(phraseFactory.createNounPhrase("Jane"),
                phraseFactory.createNounPhrase("Andrew")); //$NON-NLS-1$ - $NON-NLS-1$

            s4.setSubject(subject);

            PhraseElement pick = phraseFactory.createVerbPhrase("pick up"); //$NON-NLS-1$
            s4.VerbPhrase = pick;
            s4.setObject("the balls"); //$NON-NLS-1$
            s4.addPostModifier("in the shop"); //$NON-NLS-1$
            s4.setFeature(Feature.TENSE, Tense.FUTURE);
            s4.setFeature(Feature.FORM, Form.IMPERATIVE);

            temp2 = phraseFactory.createClause("I", "tell", s4); //$NON-NLS-1$ //$NON-NLS-2$
            indirectObject = phraseFactory.createNounPhrase("John"); //$NON-NLS-1$
            temp2.setIndirectObject(indirectObject);
            temp2.setFeature(Feature.TENSE, Tense.FUTURE);

            Assert.AreEqual("I will tell John however to pick up the balls " + "in the shop tomorrow",
                realiser.realise(temp2).Realisation); //$NON-NLS-1$ - $NON-NLS-1$
        }

        /**
         * Tests for gerund forms and genitive subjects.
         */
        [Test]
        public virtual void testGerundsubject()
        {
            // the man's giving the woman John's flower upset Peter
            SPhraseSpec _s4 = phraseFactory.createClause();
            _s4.VerbPhrase = phraseFactory.createVerbPhrase("upset"); //$NON-NLS-1$
            _s4.setFeature(Feature.TENSE, Tense.PAST);
            _s4.setObject(phraseFactory.createNounPhrase("Peter")); //$NON-NLS-1$
            s3.setFeature(Feature.PERFECT, true);

            // set the sentence as subject of another: makes it a gerund
            _s4.setSubject(s3);

            // suppress the genitive realisation of the NP subject in gerund sentences
            s3.setFeature(Feature.SUPPRESS_GENITIVE_IN_GERUND, true);

            // check the realisation: subject should not be genitive
            Assert.AreEqual("the man having given the woman John's flower upset Peter",
                realiser.realise(_s4).Realisation); //$NON-NLS-1$
        }

        /**
         * Some tests for multiple embedded sentences.
         */
        [Test]
        public virtual void testComplexSentence1()
        {
            setUp();
            // the man's giving the woman John's flower upset Peter
            SPhraseSpec complexS = phraseFactory.createClause();
            complexS.VerbPhrase = phraseFactory.createVerbPhrase("upset"); //$NON-NLS-1$
            complexS.setFeature(Feature.TENSE, Tense.PAST);
            complexS.setObject(phraseFactory.createNounPhrase("Peter")); //$NON-NLS-1$
            s3.setFeature(Feature.PERFECT, true);
            complexS.setSubject(s3);

            // check the realisation: subject should be genitive
            Assert.AreEqual("the man's having given the woman John's flower upset Peter",
                realiser.realise(complexS).Realisation); //$NON-NLS-1$

            setUp();
            // coordinate sentences in subject position
            SPhraseSpec s5 = phraseFactory.createClause();
            s5.setSubject(phraseFactory.createNounPhrase("some", "person")); //$NON-NLS-1$ //$NON-NLS-2$
            s5.VerbPhrase = phraseFactory.createVerbPhrase("stroke"); //$NON-NLS-1$
            s5.setObject(phraseFactory.createNounPhrase("the", "cat")); //$NON-NLS-1$ //$NON-NLS-2$

            CoordinatedPhraseElement coord = new CoordinatedPhraseElement(s3, s5);
            complexS = phraseFactory.createClause();
            complexS.VerbPhrase = phraseFactory.createVerbPhrase("upset"); //$NON-NLS-1$
            complexS.setFeature(Feature.TENSE, Tense.PAST);
            complexS.setObject(phraseFactory.createNounPhrase("Peter")); //$NON-NLS-1$
            complexS.setSubject(coord);
            s3.setFeature(Feature.PERFECT, true);

            Assert.AreEqual(
                "the man's having given the woman John's flower " + "and some person's stroking the cat upset Peter",
                realiser.realise(complexS).Realisation); //$NON-NLS-1$ - $NON-NLS-1$

            setUp();
            // now subordinate the complex sentence
            // coord.setClauseStatus(SPhraseSpec.ClauseType.MAIN);
            SPhraseSpec s6 = phraseFactory.createClause();
            s6.VerbPhrase = phraseFactory.createVerbPhrase("tell"); //$NON-NLS-1$
            s6.setFeature(Feature.TENSE, Tense.PAST);
            s6.setSubject(phraseFactory.createNounPhrase("the", "boy")); //$NON-NLS-1$ //$NON-NLS-2$
            // ER - switched indirect and direct object
            PhraseElement indirect = phraseFactory.createNounPhrase("every", "girl"); //$NON-NLS-1$ - $NON-NLS-1$
            s6.setIndirectObject(indirect);
            complexS = phraseFactory.createClause();
            complexS.VerbPhrase = phraseFactory.createVerbPhrase("upset"); //$NON-NLS-1$
            complexS.setFeature(Feature.TENSE, Tense.PAST);
            complexS.setObject(phraseFactory.createNounPhrase("Peter")); //$NON-NLS-1$
            s6.setObject(complexS);
            coord = new CoordinatedPhraseElement(s3, s5);
            complexS.setSubject(coord);
            s3.setFeature(Feature.PERFECT, true);
            Assert.AreEqual(
                "the boy told every girl that the man's having given the woman " +
                "John's flower and some person's stroking the cat " + "upset Peter",
                realiser.realise(s6).Realisation); //$NON-NLS-1$ - $NON-NLS-1$ - $NON-NLS-1$
        }

        /**
         * More coordination tests.
         */
        [Test]
        public virtual void testComplexSentence3()
        {
            setUp();

            s1 = phraseFactory.createClause();
            s1.setSubject(woman);
            s1.setVerb("kiss");
            s1.setObject(man);

            PhraseElement _man = phraseFactory.createNounPhrase("the", "man"); //$NON-NLS-1$ //$NON-NLS-2$
            s3 = phraseFactory.createClause();
            s3.setSubject(_man);
            s3.setVerb("give");

            NPPhraseSpec flower = phraseFactory.createNounPhrase("flower"); //$NON-NLS-1$
            NPPhraseSpec john = phraseFactory.createNounPhrase("John"); //$NON-NLS-1$
            john.setFeature(Feature.POSSESSIVE, true);
            flower.setSpecifier(john);
            s3.setObject(flower);

            PhraseElement _woman = phraseFactory.createNounPhrase("the", "woman"); //$NON-NLS-1$ //$NON-NLS-2$
            s3.setIndirectObject(_woman);

            // the coordinate sentence allows us to raise and lower complementiser
            CoordinatedPhraseElement coord2 = new CoordinatedPhraseElement(s1, s3);
            coord2.setFeature(Feature.TENSE, Tense.PAST);

            realiser.DebugMode = true;
            Assert.AreEqual("the woman kissed the man and the man gave the woman John's flower",
                realiser.realise(coord2).Realisation); //$NON-NLS-1$
        }

        // /**
        // * Sentence with clausal subject with verb "be" and a progressive feature
        // */
        // public void testComplexSentence2() {
        // SPhraseSpec subject = this.phraseFactory.createClause(
        // this.phraseFactory.createNounPhrase("the", "child"),
        // this.phraseFactory.createVerbPhrase("be"), this.phraseFactory
        // .createWord("difficult", LexicalCategory.ADJECTIVE));
        // subject.setFeature(Feature.PROGRESSIVE, true);
        // }

        /**
         * Tests recogition of strings in API.
         */
        [Test]
        public virtual void testStringRecognition()
        {
            // test recognition of forms of "be"
            PhraseElement
                _s1 = phraseFactory.createClause("my cat", "be", "sad"); //$NON-NLS-1$ //$NON-NLS-2$ //$NON-NLS-3$
            Assert.AreEqual("my cat is sad", realiser.realise(_s1).Realisation); //$NON-NLS-1$

            // test recognition of pronoun for afreement
            PhraseElement
                _s2 = phraseFactory.createClause("I", "want", "Mary"); //$NON-NLS-1$ //$NON-NLS-2$ //$NON-NLS-3$

            Assert.AreEqual("I want Mary", realiser.realise(_s2).Realisation); //$NON-NLS-1$

            // test recognition of pronoun for correct form
            PhraseElement subject = phraseFactory.createNounPhrase("dog"); //$NON-NLS-1$
            subject.setFeature(InternalFeature.SPECIFIER, "a"); //$NON-NLS-1$
            subject.addPostModifier("from next door"); //$NON-NLS-1$
            PhraseElement @object = phraseFactory.createNounPhrase("I"); //$NON-NLS-1$
            PhraseElement s = phraseFactory.createClause(subject, "chase", @object); //$NON-NLS-1$
            s.setFeature(Feature.PROGRESSIVE, true);
            Assert.AreEqual("a dog from next door is chasing me", realiser.realise(s).Realisation); //$NON-NLS-1$
        }

        /**
         * Tests complex agreement.
         */
        [Test]
        public virtual void testAgreement()
        {
            // basic agreement
            NPPhraseSpec np = phraseFactory.createNounPhrase("dog"); //$NON-NLS-1$
            np.setSpecifier("the"); //$NON-NLS-1$
            np.addPreModifier("angry"); //$NON-NLS-1$
            PhraseElement _s1 = phraseFactory.createClause(np, "chase", "John"); //$NON-NLS-1$ //$NON-NLS-2$
            Assert.AreEqual("the angry dog chases John", realiser.realise(_s1).Realisation); //$NON-NLS-1$


            // plural
            np = phraseFactory.createNounPhrase("dog"); //$NON-NLS-1$
            np.setSpecifier("the"); //$NON-NLS-1$
            np.addPreModifier("angry"); //$NON-NLS-1$
            np.setFeature(Feature.NUMBER, NumberAgreement.PLURAL);
            _s1 = phraseFactory.createClause(np, "chase", "John"); //$NON-NLS-1$ //$NON-NLS-2$
            Assert.AreEqual("the angry dogs chase John", realiser.realise(_s1).Realisation); //$NON-NLS-1$


            // test agreement with "there is"
            np = phraseFactory.createNounPhrase("dog"); //$NON-NLS-1$
            np.addPreModifier("angry"); //$NON-NLS-1$
            np.setFeature(Feature.NUMBER, NumberAgreement.SINGULAR);
            np.setSpecifier("a"); //$NON-NLS-1$
            PhraseElement _s2 = phraseFactory.createClause("there", "be", np); //$NON-NLS-1$ //$NON-NLS-2$
            Assert.AreEqual("there is an angry dog", realiser.realise(_s2).Realisation); //$NON-NLS-1$


            // plural with "there"
            np = phraseFactory.createNounPhrase("dog"); //$NON-NLS-1$
            np.addPreModifier("angry"); //$NON-NLS-1$
            np.setSpecifier("a"); //$NON-NLS-1$
            np.setFeature(Feature.NUMBER, NumberAgreement.PLURAL);
            _s2 = phraseFactory.createClause("there", "be", np); //$NON-NLS-1$ //$NON-NLS-2$
            Assert.AreEqual("there are some angry dogs", realiser.realise(_s2).Realisation); //$NON-NLS-1$
        }

        /**
         * Tests passive.
         */
        [Test]
        public virtual void testPassive()
        {
            // passive with just complement
            SPhraseSpec _s1 =
                phraseFactory.createClause(null, "intubate",
                    phraseFactory.createNounPhrase("the", "baby")); //$NON-NLS-1$ //$NON-NLS-2$ //$NON-NLS-3$
            _s1.setFeature(Feature.PASSIVE, true);

            Assert.AreEqual("the baby is intubated", realiser.realise(_s1).Realisation); //$NON-NLS-1$


            // passive with subject and complement
            _s1 = phraseFactory.createClause(null, "intubate",
                phraseFactory.createNounPhrase("the", "baby")); //$NON-NLS-1$ //$NON-NLS-2$ //$NON-NLS-3$
            _s1.setSubject(phraseFactory.createNounPhrase("the nurse")); //$NON-NLS-1$
            _s1.setFeature(Feature.PASSIVE, true);
            Assert.AreEqual("the baby is intubated by the nurse", realiser.realise(_s1).Realisation); //$NON-NLS-1$


            // passive with subject and indirect object
            SPhraseSpec _s2 =
                phraseFactory.createClause(null, "give",
                    phraseFactory.createNounPhrase("the", "baby")); //$NON-NLS-1$ //$NON-NLS-2$ - $NON-NLS-1$

            PhraseElement morphine = phraseFactory.createNounPhrase("50ug of morphine"); //$NON-NLS-1$
            _s2.setIndirectObject(morphine);
            _s2.setFeature(Feature.PASSIVE, true);
            Assert.AreEqual("the baby is given 50ug of morphine", realiser.realise(_s2).Realisation); //$NON-NLS-1$


            // passive with subject, complement and indirect object
            _s2 = phraseFactory.createClause(phraseFactory.createNounPhrase("the", "nurse"), "give",
                phraseFactory.createNounPhrase("the",
                    "baby")); //$NON-NLS-1$ //$NON-NLS-2$ - $NON-NLS-1$ //$NON-NLS-2$ //$NON-NLS-3$

            morphine = phraseFactory.createNounPhrase("50ug of morphine"); //$NON-NLS-1$
            _s2.setIndirectObject(morphine);
            _s2.setFeature(Feature.PASSIVE, true);
            Assert.AreEqual("the baby is given 50ug of morphine by the nurse",
                realiser.realise(_s2).Realisation); //$NON-NLS-1$


            // test agreement in passive
            PhraseElement _s3 =
                phraseFactory.createClause(new CoordinatedPhraseElement("my dog", "your cat"), "chase",
                    "George"); //$NON-NLS-1$ - $NON-NLS-1$ //$NON-NLS-2$ //$NON-NLS-3$
            _s3.setFeature(Feature.TENSE, Tense.PAST);
            _s3.addFrontModifier("yesterday"); //$NON-NLS-1$
            Assert.AreEqual("yesterday my dog and your cat chased George",
                realiser.realise(_s3).Realisation); //$NON-NLS-1$

            _s3 = phraseFactory.createClause(new CoordinatedPhraseElement("my dog", "your cat"), "chase",
                phraseFactory.createNounPhrase("George")); //$NON-NLS-1$ - $NON-NLS-1$ //$NON-NLS-2$ //$NON-NLS-3$
            _s3.setFeature(Feature.TENSE, Tense.PAST);
            _s3.addFrontModifier("yesterday"); //$NON-NLS-1$
            _s3.setFeature(Feature.PASSIVE, true);
            Assert.AreEqual("yesterday George was chased by my dog and your cat",
                realiser.realise(_s3).Realisation); //$NON-NLS-1$


            // test correct pronoun forms
            PhraseElement _s4 = phraseFactory.createClause(phraseFactory.createNounPhrase("he"), "chase",
                phraseFactory.createNounPhrase("I")); //$NON-NLS-1$ - $NON-NLS-1$ //$NON-NLS-2$
            Assert.AreEqual("he chases me", realiser.realise(_s4).Realisation); //$NON-NLS-1$
            _s4 = phraseFactory.createClause(phraseFactory.createNounPhrase("he"), "chase",
                phraseFactory.createNounPhrase("me")); //$NON-NLS-1$ //$NON-NLS-2$ //$NON-NLS-3$
            _s4.setFeature(Feature.PASSIVE, true);
            Assert.AreEqual("I am chased by him", realiser.realise(_s4).Realisation); //$NON-NLS-1$

            // same thing, but giving the S constructor "me". Should recognise correct pro anyway
            PhraseElement
                _s5 = phraseFactory.createClause("him", "chase", "I"); //$NON-NLS-1$ //$NON-NLS-2$ //$NON-NLS-3$
            Assert.AreEqual("he chases me", realiser.realise(_s5).Realisation); //$NON-NLS-1$

            _s5 = phraseFactory.createClause("him", "chase", "I"); //$NON-NLS-1$ //$NON-NLS-2$ //$NON-NLS-3$
            _s5.setFeature(Feature.PASSIVE, true);
            Assert.AreEqual("I am chased by him", realiser.realise(_s5).Realisation); //$NON-NLS-1$
        }

        /**
         * Test that complements set within the VP are raised when sentence is
         * passivised.
         */
        [Test]
        public virtual void testPassiveWithInternalVPComplement()
        {
            PhraseElement vp = phraseFactory.createVerbPhrase(phraseFactory.createWord("upset",
                new LexicalCategory(LexicalCategory.LexicalCategoryEnum.VERB)));
            vp.addComplement(phraseFactory.createNounPhrase("the", "man"));
            PhraseElement _s6 = phraseFactory.createClause(phraseFactory.createNounPhrase("the", "child"), vp);
            _s6.setFeature(Feature.TENSE, Tense.PAST);
            Assert.AreEqual("the child upset the man", realiser.realise(_s6).Realisation);

            _s6.setFeature(Feature.PASSIVE, true);
            Assert.AreEqual("the man was upset by the child", realiser.realise(_s6).Realisation);
        }

        /**
         * Tests tenses with modals.
         */
        [Test]
        public virtual void testModal()
        {
            setUp();
            // simple modal in present tense
            s3.setFeature(Feature.MODAL, "should"); //$NON-NLS-1$
            Assert.AreEqual("the man should give the woman John's flower",
                realiser.realise(s3).Realisation); //$NON-NLS-1$


            // modal + future -- uses present
            setUp();
            s3.setFeature(Feature.MODAL, "should"); //$NON-NLS-1$
            s3.setFeature(Feature.TENSE, Tense.FUTURE);
            Assert.AreEqual("the man should give the woman John's flower",
                realiser.realise(s3).Realisation); //$NON-NLS-1$


            // modal + present progressive
            setUp();
            s3.setFeature(Feature.MODAL, "should"); //$NON-NLS-1$
            s3.setFeature(Feature.TENSE, Tense.FUTURE);
            s3.setFeature(Feature.PROGRESSIVE, true);
            Assert.AreEqual("the man should be giving the woman John's flower",
                realiser.realise(s3).Realisation); //$NON-NLS-1$


            // modal + past tense
            setUp();
            s3.setFeature(Feature.MODAL, "should"); //$NON-NLS-1$
            s3.setFeature(Feature.TENSE, Tense.PAST);
            Assert.AreEqual("the man should have given the woman John's flower",
                realiser.realise(s3).Realisation); //$NON-NLS-1$


            // modal + past progressive
            setUp();
            s3.setFeature(Feature.MODAL, "should"); //$NON-NLS-1$
            s3.setFeature(Feature.TENSE, Tense.PAST);
            s3.setFeature(Feature.PROGRESSIVE, true);

            Assert.AreEqual("the man should have been giving the woman John's flower",
                realiser.realise(s3).Realisation); //$NON-NLS-1$
        }

        /**
         * Test for passivisation with mdoals
         */
        [Test]
        public virtual void testModalWithPassive()
        {
            NPPhraseSpec @object = phraseFactory.createNounPhrase("the", "pizza");
            AdjPhraseSpec post = phraseFactory.createAdjectivePhrase("good");
            AdvPhraseSpec @as = phraseFactory.createAdverbPhrase("as");
            @as.addComplement(post);
            VPPhraseSpec verb = phraseFactory.createVerbPhrase("classify");
            verb.addPostModifier(@as);
            verb.addComplement(@object);
            SPhraseSpec s = phraseFactory.createClause();
            s.VerbPhrase = verb;
            s.setFeature(Feature.MODAL, "can");
            // s.setFeature(Feature.FORM, Form.INFINITIVE);
            s.setFeature(Feature.PASSIVE, true);
            Assert.AreEqual("the pizza can be classified as good", realiser.realise(s).Realisation);
        }

        [Test]
        public virtual void testPassiveWithPPCompl()
        {
            // passive with just complement
            NPPhraseSpec subject = phraseFactory.createNounPhrase("wave");
            subject.setFeature(Feature.NUMBER, NumberAgreement.PLURAL);
            NPPhraseSpec @object = phraseFactory.createNounPhrase("surfer");
            @object.setFeature(Feature.NUMBER, NumberAgreement.PLURAL);

            SPhraseSpec
                _s1 = phraseFactory.createClause(subject, "carry", @object); //$NON-NLS-1$ //$NON-NLS-2$ //$NON-NLS-3$


            // add a PP complement
            PPPhraseSpec pp =
                phraseFactory.createPrepositionPhrase("to", phraseFactory.createNounPhrase("the", "shore"));
            pp.setFeature(InternalFeature.DISCOURSE_FUNCTION, DiscourseFunction.INDIRECT_OBJECT);
            _s1.addComplement(pp);

            _s1.setFeature(Feature.PASSIVE, true);

            Assert.AreEqual("surfers are carried to the shore by waves",
                realiser.realise(_s1).Realisation); //$NON-NLS-1$
        }

        [Test]
        public virtual void testPassiveWithPPMod()
        {
            // passive with just complement
            NPPhraseSpec subject = phraseFactory.createNounPhrase("wave");
            subject.setFeature(Feature.NUMBER, NumberAgreement.PLURAL);
            NPPhraseSpec @object = phraseFactory.createNounPhrase("surfer");
            @object.setFeature(Feature.NUMBER, NumberAgreement.PLURAL);

            SPhraseSpec
                _s1 = phraseFactory.createClause(subject, "carry", @object); //$NON-NLS-1$ //$NON-NLS-2$ //$NON-NLS-3$


            // add a PP complement
            PPPhraseSpec pp =
                phraseFactory.createPrepositionPhrase("to", phraseFactory.createNounPhrase("the", "shore"));
            _s1.addPostModifier(pp);

            _s1.setFeature(Feature.PASSIVE, true);

            Assert.AreEqual("surfers are carried to the shore by waves",
                realiser.realise(_s1).Realisation); //$NON-NLS-1$
        }

        [Test]
        public virtual void testCuePhrase()
        {
            NPPhraseSpec subject = phraseFactory.createNounPhrase("wave");
            subject.setFeature(Feature.NUMBER, NumberAgreement.PLURAL);
            NPPhraseSpec @object = phraseFactory.createNounPhrase("surfer");
            @object.setFeature(Feature.NUMBER, NumberAgreement.PLURAL);

            SPhraseSpec
                _s1 = phraseFactory.createClause(subject, "carry", @object); //$NON-NLS-1$ //$NON-NLS-2$ //$NON-NLS-3$


            // add a PP complement
            PPPhraseSpec pp =
                phraseFactory.createPrepositionPhrase("to", phraseFactory.createNounPhrase("the", "shore"));
            _s1.addPostModifier(pp);

            _s1.setFeature(Feature.PASSIVE, true);

            _s1.addFrontModifier("however");


            //without comma separation of cue phrase
            Assert.AreEqual("however surfers are carried to the shore by waves",
                realiser.realise(_s1).Realisation); //$NON-NLS-1$


            realiser.CommaSepCuephrase = true;
            Assert.AreEqual("however, surfers are carried to the shore by waves",
                realiser.realise(_s1).Realisation); //$NON-NLS-1$
        }


        /**
         * Check that setComplement replaces earlier complements
         */
        [Test]
        public virtual void setComplementTest()
        {
            SPhraseSpec s = phraseFactory.createClause();
            s.setSubject("I");
            s.setVerb("see");
            s.setObject("a dog");

            Assert.AreEqual("I see a dog", realiser.realise(s).Realisation);

            s.setObject("a cat");
            Assert.AreEqual("I see a cat", realiser.realise(s).Realisation);

            s.setObject("a wolf");
            Assert.AreEqual("I see a wolf", realiser.realise(s).Realisation);
        }


        /**
         * Test for subclauses involving WH-complements Based on a query by Owen
         * Bennett
         */
        [Test]
        public virtual void subclausesTest()
        {
            // Once upon a time, there was an Accountant, called Jeff, who lived in a forest.
            // main sentence
            NPPhraseSpec acct = phraseFactory.createNounPhrase("a", "accountant");


            // first postmodifier of "an accountant"
            VPPhraseSpec sub1 = phraseFactory.createVerbPhrase("call");
            sub1.addComplement("Jeff");
            sub1.setFeature(Feature.FORM, Form.PAST_PARTICIPLE);
            // this is an appositive modifier, which makes simplenlg put it between commas
            sub1.setFeature(Feature.APPOSITIVE, true);
            acct.addPostModifier(sub1);

            // second postmodifier of "an accountant" is "who lived in a forest"
            SPhraseSpec sub2 = phraseFactory.createClause();
            VPPhraseSpec subvp = phraseFactory.createVerbPhrase("live");
            subvp.setFeature(Feature.TENSE, Tense.PAST);
            subvp.Complement =
                phraseFactory.createPrepositionPhrase("in", phraseFactory.createNounPhrase("a", "forest"));
            sub2.VerbPhrase = subvp;
            // simplenlg can't yet handle wh-clauses in NPs, so we need to hack it
            // by setting the subject to "who"
            sub2.setSubject("who");
            acct.addPostModifier(sub2);

            // main sentence
            SPhraseSpec s = phraseFactory.createClause("there", "be", acct);
            s.setFeature(Feature.TENSE, Tense.PAST);

            // add front modifier "once upon a time"
            s.addFrontModifier("once upon a time");

            Assert.AreEqual("once upon a time there was an accountant, called Jeff, who lived in a forest",
                realiser.realise(s).Realisation);
        }
    }
}