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

using System;
using NUnit.Framework;
using SimpleNLG.Main.features;
using SimpleNLG.Main.framework;
using SimpleNLG.Main.lexicon;
using SimpleNLG.Main.phrasespec;
using SimpleNLG.Main.realiser.english;

namespace SimpleNLG.Test.syntax.english
{
    using Feature = Feature;
    using InterrogativeType = InterrogativeType;
    using Person = Person;
    using Tense = Tense;
    using CoordinatedPhraseElement = CoordinatedPhraseElement;
    using DocumentElement = DocumentElement;
    using LexicalCategory = LexicalCategory;
    using NLGElement = NLGElement;
    using NLGFactory = NLGFactory;
    using PhraseElement = PhraseElement;
    using Lexicon = Lexicon;
    using NPPhraseSpec = NPPhraseSpec;
    using PPPhraseSpec = PPPhraseSpec;
    using SPhraseSpec = SPhraseSpec;
    using Realiser = Realiser;

    /**
     * JUnit test case for interrogatives.
     * 
     * @author agatt
     */
    [TestFixture]
    public class InterrogativeTest : SimpleNLG4Test
    {
        // set up a few more fixtures
        /** The s5. */

        private SPhraseSpec s1, s2, s3, s4; //s5

        /**
         * Instantiates a new interrogative test.
         * 
         * @param name
         *            the name
         */
        public InterrogativeTest() : this(null)
        {
        }

        public InterrogativeTest(string name) : base(name)
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

            // // the man gives the woman John's flower
            PhraseElement john = phraseFactory.createNounPhrase("John"); //$NON-NLS-1$
            john.setFeature(Feature.POSSESSIVE, true);
            PhraseElement flower = phraseFactory.createNounPhrase(john, "flower"); //$NON-NLS-1$
            PhraseElement _woman = phraseFactory.createNounPhrase("the", "woman"); //$NON-NLS-1$ //$NON-NLS-2$
            s3 = phraseFactory.createClause(man, give, flower);
            s3.setIndirectObject(_woman);

            CoordinatedPhraseElement subjects = new CoordinatedPhraseElement(phraseFactory.createNounPhrase("Jane"),
                phraseFactory.createNounPhrase("Andrew")); //$NON-NLS-1$ - $NON-NLS-1$
            s4 = phraseFactory.createClause(subjects, "pick up", "the balls"); //$NON-NLS-1$ - $NON-NLS-1$
            s4.addPostModifier("in the shop"); //$NON-NLS-1$
            s4.setFeature(Feature.CUE_PHRASE, "however"); //$NON-NLS-1$
            s4.addFrontModifier("tomorrow"); //$NON-NLS-1$
            s4.setFeature(Feature.TENSE, Tense.FUTURE);
            // this.s5 = new SPhraseSpec();
            // this.s5.setSubject(new NPPhraseSpec("the", "dog"));
            // this.s5.setHead("be");
            // this.s5.setComplement(new NPPhraseSpec("the", "rock"),
            // DiscourseFunction.OBJECT);
        }

        /**
         * Tests a couple of fairly simple questions.
         */
        [Test]
        public virtual void testSimpleQuestions()
        {
            setUp();
            phraseFactory.Lexicon = lexicon;
            realiser.Lexicon = lexicon;

            // simple present
            s1 = phraseFactory.createClause(woman, kiss, man);
            s1.setFeature(Feature.TENSE, Tense.PRESENT);
            s1.setFeature(Feature.INTERROGATIVE_TYPE, InterrogativeType.YES_NO);

            NLGFactory docFactory = new NLGFactory(lexicon);
            DocumentElement sent = docFactory.createSentence(s1);
            Assert.AreEqual("Does the woman kiss the man?", realiser.realise(sent).Realisation); //$NON-NLS-1$


            // simple past
            // sentence: "the woman kissed the man"
            s1 = phraseFactory.createClause(woman, kiss, man);
            s1.setFeature(Feature.TENSE, Tense.PAST);
            s1.setFeature(Feature.INTERROGATIVE_TYPE, InterrogativeType.YES_NO);
            Assert.AreEqual("did the woman kiss the man", realiser.realise(s1).Realisation); //$NON-NLS-1$


            // copular/existential: be-fronting
            // sentence = "there is the dog on the rock"
            s2 = phraseFactory.createClause("there", "be", dog); //$NON-NLS-1$ //$NON-NLS-2$
            s2.addPostModifier(onTheRock);
            s2.setFeature(Feature.INTERROGATIVE_TYPE, InterrogativeType.YES_NO);
            Assert.AreEqual("is there the dog on the rock", realiser.realise(s2).Realisation); //$NON-NLS-1$


            // perfective
            // sentence -- "there has been the dog on the rock"
            s2 = phraseFactory.createClause("there", "be", dog); //$NON-NLS-1$ //$NON-NLS-2$
            s2.addPostModifier(onTheRock);
            s2.setFeature(Feature.INTERROGATIVE_TYPE, InterrogativeType.YES_NO);
            s2.setFeature(Feature.PERFECT, true);
            Assert.AreEqual("has there been the dog on the rock", realiser.realise(s2).Realisation); //$NON-NLS-1$


            // progressive
            // sentence: "the man was giving the woman John's flower"
            PhraseElement john = phraseFactory.createNounPhrase("John"); //$NON-NLS-1$
            john.setFeature(Feature.POSSESSIVE, true);
            PhraseElement flower = phraseFactory.createNounPhrase(john, "flower"); //$NON-NLS-1$
            PhraseElement _woman = phraseFactory.createNounPhrase("the", "woman"); //$NON-NLS-1$ //$NON-NLS-2$
            s3 = phraseFactory.createClause(man, give, flower);
            s3.setIndirectObject(_woman);
            s3.setFeature(Feature.TENSE, Tense.PAST);
            s3.setFeature(Feature.PROGRESSIVE, true);
            s3.setFeature(Feature.INTERROGATIVE_TYPE, InterrogativeType.YES_NO);
            NLGElement realised = realiser.realise(s3);
            Assert.AreEqual("was the man giving the woman John's flower", realised.Realisation); //$NON-NLS-1$


            // modal
            // sentence: "the man should be giving the woman John's flower"
            setUp();
            john = phraseFactory.createNounPhrase("John"); //$NON-NLS-1$
            john.setFeature(Feature.POSSESSIVE, true);
            flower = phraseFactory.createNounPhrase(john, "flower"); //$NON-NLS-1$
            _woman = phraseFactory.createNounPhrase("the", "woman"); //$NON-NLS-1$ //$NON-NLS-2$
            s3 = phraseFactory.createClause(man, give, flower);
            s3.setIndirectObject(_woman);
            s3.setFeature(Feature.TENSE, Tense.PAST);
            s3.setFeature(Feature.INTERROGATIVE_TYPE, InterrogativeType.YES_NO);
            s3.setFeature(Feature.MODAL, "should"); //$NON-NLS-1$
            Assert.AreEqual("should the man have given the woman John's flower",
                realiser.realise(s3).Realisation); //$NON-NLS-1$


            // complex case with cue phrases
            // sentence: "however, tomorrow, Jane and Andrew will pick up the balls
            // in the shop"
            // this gets the front modifier "tomorrow" shifted to the end
            setUp();
            CoordinatedPhraseElement subjects = new CoordinatedPhraseElement(phraseFactory.createNounPhrase("Jane"),
                phraseFactory.createNounPhrase("Andrew")); //$NON-NLS-1$ - $NON-NLS-1$
            s4 = phraseFactory.createClause(subjects, "pick up", "the balls"); //$NON-NLS-1$ - $NON-NLS-1$
            s4.addPostModifier("in the shop"); //$NON-NLS-1$
            s4.setFeature(Feature.CUE_PHRASE, "however,"); //$NON-NLS-1$
            s4.addFrontModifier("tomorrow"); //$NON-NLS-1$
            s4.setFeature(Feature.TENSE, Tense.FUTURE);
            s4.setFeature(Feature.INTERROGATIVE_TYPE, InterrogativeType.YES_NO);
            Assert.AreEqual("however, will Jane and Andrew pick up the balls in the shop tomorrow",
                realiser.realise(s4).Realisation); //$NON-NLS-1$
        }

        /**
         * Test for sentences with negation.
         */
        [Test]
        public virtual void testNegatedQuestions()
        {
            setUp();
            phraseFactory.Lexicon = lexicon;
            realiser.Lexicon = lexicon;

            // sentence: "the woman did not kiss the man"
            s1 = phraseFactory.createClause(woman, "kiss", man);
            s1.setFeature(Feature.TENSE, Tense.PAST);
            s1.setFeature(Feature.NEGATED, true);
            s1.setFeature(Feature.INTERROGATIVE_TYPE, InterrogativeType.YES_NO);
            Assert.AreEqual("did the woman not kiss the man", realiser.realise(s1).Realisation); //$NON-NLS-1$


            // sentence: however, tomorrow, Jane and Andrew will not pick up the
            // balls in the shop
            CoordinatedPhraseElement subjects = new CoordinatedPhraseElement(phraseFactory.createNounPhrase("Jane"),
                phraseFactory.createNounPhrase("Andrew")); //$NON-NLS-1$ - $NON-NLS-1$
            s4 = phraseFactory.createClause(subjects, "pick up", "the balls"); //$NON-NLS-1$ - $NON-NLS-1$
            s4.addPostModifier("in the shop"); //$NON-NLS-1$
            s4.setFeature(Feature.CUE_PHRASE, "however,"); //$NON-NLS-1$
            s4.addFrontModifier("tomorrow"); //$NON-NLS-1$
            s4.setFeature(Feature.NEGATED, true);
            s4.setFeature(Feature.TENSE, Tense.FUTURE);
            s4.setFeature(Feature.INTERROGATIVE_TYPE, InterrogativeType.YES_NO);
            Assert.AreEqual("however, will Jane and Andrew not pick up the balls in the shop tomorrow",
                realiser.realise(s4).Realisation); //$NON-NLS-1$
        }

        /**
         * Tests for coordinate VPs in question form.
         */
        [Test]
        public virtual void testCoordinateVPQuestions()
        {
            // create a complex vp: "kiss the dog and walk in the room"
            setUp();
            CoordinatedPhraseElement complex = new CoordinatedPhraseElement(kiss, walk);
            kiss.addComplement(dog);
            walk.addComplement(inTheRoom);

            // sentence: "However, tomorrow, Jane and Andrew will kiss the dog and
            // will walk in the room"
            CoordinatedPhraseElement subjects = new CoordinatedPhraseElement(phraseFactory.createNounPhrase("Jane"),
                phraseFactory.createNounPhrase("Andrew")); //$NON-NLS-1$ - $NON-NLS-1$
            s4 = phraseFactory.createClause(subjects, complex);
            s4.setFeature(Feature.CUE_PHRASE, "however"); //$NON-NLS-1$
            s4.addFrontModifier("tomorrow"); //$NON-NLS-1$
            s4.setFeature(Feature.TENSE, Tense.FUTURE);

            Assert.AreEqual("however tomorrow Jane and Andrew will kiss the dog and will walk in the room",
                realiser.realise(s4).Realisation); //$NON-NLS-1$


            // setting to interrogative should automatically give us a single,
            // wide-scope aux
            setUp();
            subjects = new CoordinatedPhraseElement(phraseFactory.createNounPhrase("Jane"),
                phraseFactory.createNounPhrase("Andrew")); //$NON-NLS-1$ - $NON-NLS-1$
            kiss.addComplement(dog);
            walk.addComplement(inTheRoom);
            complex = new CoordinatedPhraseElement(kiss, walk);
            s4 = phraseFactory.createClause(subjects, complex);
            s4.setFeature(Feature.CUE_PHRASE, "however"); //$NON-NLS-1$
            s4.addFrontModifier("tomorrow"); //$NON-NLS-1$
            s4.setFeature(Feature.TENSE, Tense.FUTURE);
            s4.setFeature(Feature.INTERROGATIVE_TYPE, InterrogativeType.YES_NO);

            Assert.AreEqual("however will Jane and Andrew kiss the dog and walk in the room tomorrow",
                realiser.realise(s4).Realisation); //$NON-NLS-1$


            // slightly more complex -- perfective
            setUp();
            realiser.Lexicon = lexicon;
            subjects = new CoordinatedPhraseElement(phraseFactory.createNounPhrase("Jane"),
                phraseFactory.createNounPhrase("Andrew")); //$NON-NLS-1$ - $NON-NLS-1$
            complex = new CoordinatedPhraseElement(kiss, walk);
            kiss.addComplement(dog);
            walk.addComplement(inTheRoom);
            s4 = phraseFactory.createClause(subjects, complex);
            s4.setFeature(Feature.CUE_PHRASE, "however"); //$NON-NLS-1$
            s4.addFrontModifier("tomorrow"); //$NON-NLS-1$
            s4.setFeature(Feature.TENSE, Tense.FUTURE);
            s4.setFeature(Feature.INTERROGATIVE_TYPE, InterrogativeType.YES_NO);
            s4.setFeature(Feature.PERFECT, true);

            Assert.AreEqual("however will Jane and Andrew have kissed the dog and walked in the room tomorrow",
                realiser.realise(s4).Realisation); //$NON-NLS-1$
        }

        /**
         * Test for simple WH questions in present tense.
         */
        [Test]
        public virtual void testSimpleQuestions2()
        {
            setUp();
            realiser.Lexicon = lexicon;
            PhraseElement
                s = phraseFactory.createClause("the woman", "kiss",
                    "the man"); //$NON-NLS-1$ - $NON-NLS-1$ //$NON-NLS-2$


            // try with the simple yes/no type first
            s.setFeature(Feature.INTERROGATIVE_TYPE, InterrogativeType.YES_NO);
            Assert.AreEqual("does the woman kiss the man", realiser.realise(s).Realisation); //$NON-NLS-1$


            // now in the passive
            s = phraseFactory.createClause("the woman", "kiss", "the man"); //$NON-NLS-1$ - $NON-NLS-1$ //$NON-NLS-2$
            s.setFeature(Feature.INTERROGATIVE_TYPE, InterrogativeType.YES_NO);
            s.setFeature(Feature.PASSIVE, true);
            Assert.AreEqual("is the man kissed by the woman", realiser.realise(s).Realisation); //$NON-NLS-1$


            // // subject interrogative with simple present
            // // sentence: "the woman kisses the man"
            s = phraseFactory.createClause("the woman", "kiss", "the man"); //$NON-NLS-1$ - $NON-NLS-1$ //$NON-NLS-2$
            s.setFeature(Feature.INTERROGATIVE_TYPE, InterrogativeType.WHO_SUBJECT);

            Assert.AreEqual("who kisses the man", realiser.realise(s).Realisation); //$NON-NLS-1$


            // object interrogative with simple present
            s = phraseFactory.createClause("the woman", "kiss", "the man"); //$NON-NLS-1$ - $NON-NLS-1$ //$NON-NLS-2$
            s.setFeature(Feature.INTERROGATIVE_TYPE, InterrogativeType.WHO_OBJECT);
            Assert.AreEqual("who does the woman kiss", realiser.realise(s).Realisation); //$NON-NLS-1$


            // subject interrogative with passive
            s = phraseFactory.createClause("the woman", "kiss", "the man"); //$NON-NLS-1$ - $NON-NLS-1$ //$NON-NLS-2$
            s.setFeature(Feature.INTERROGATIVE_TYPE, InterrogativeType.WHO_SUBJECT);
            s.setFeature(Feature.PASSIVE, true);
            Assert.AreEqual("who is the man kissed by", realiser.realise(s).Realisation); //$NON-NLS-1$
        }

        /**
         * Test for wh questions.
         */
        [Test]
        public virtual void testWHQuestions()
        {
            // subject interrogative
            setUp();
            realiser.Lexicon = lexicon;
            s4.setFeature(Feature.INTERROGATIVE_TYPE, InterrogativeType.WHO_SUBJECT);
            Assert.AreEqual("however who will pick up the balls in the shop tomorrow",
                realiser.realise(s4).Realisation); //$NON-NLS-1$


            // subject interrogative in passive
            setUp();
            s4.setFeature(Feature.PASSIVE, true);
            s4.setFeature(Feature.INTERROGATIVE_TYPE, InterrogativeType.WHO_SUBJECT);

            Assert.AreEqual("however who will the balls be picked up in the shop by tomorrow",
                realiser.realise(s4).Realisation); //$NON-NLS-1$


            // object interrogative
            setUp();
            s4.setFeature(Feature.INTERROGATIVE_TYPE, InterrogativeType.WHAT_OBJECT);
            Assert.AreEqual("however what will Jane and Andrew pick up in the shop tomorrow",
                realiser.realise(s4).Realisation); //$NON-NLS-1$


            // object interrogative with passive
            setUp();
            s4.setFeature(Feature.INTERROGATIVE_TYPE, InterrogativeType.WHAT_OBJECT);
            s4.setFeature(Feature.PASSIVE, true);

            Assert.AreEqual("however what will be picked up in the shop by Jane and Andrew tomorrow",
                realiser.realise(s4).Realisation); //$NON-NLS-1$


            // how-question + passive
            setUp();
            s4.setFeature(Feature.PASSIVE, true);
            s4.setFeature(Feature.INTERROGATIVE_TYPE, InterrogativeType.HOW);
            Assert.AreEqual("however how will the balls be picked up in the shop by Jane and Andrew tomorrow",
                realiser.realise(s4).Realisation); //$NON-NLS-1$


            // // why-question + passive
            setUp();
            s4.setFeature(Feature.PASSIVE, true);
            s4.setFeature(Feature.INTERROGATIVE_TYPE, InterrogativeType.WHY);
            Assert.AreEqual("however why will the balls be picked up in the shop by Jane and Andrew tomorrow",
                realiser.realise(s4).Realisation); //$NON-NLS-1$


            // how question with modal
            setUp();
            s4.setFeature(Feature.PASSIVE, true);
            s4.setFeature(Feature.INTERROGATIVE_TYPE, InterrogativeType.HOW);
            s4.setFeature(Feature.MODAL, "should"); //$NON-NLS-1$
            Assert.AreEqual("however how should the balls be picked up in the shop by Jane and Andrew tomorrow",
                realiser.realise(s4).Realisation); //$NON-NLS-1$


            // indirect object
            setUp();
            realiser.Lexicon = lexicon;
            s3.setFeature(Feature.INTERROGATIVE_TYPE, InterrogativeType.WHO_INDIRECT_OBJECT);
            Assert.AreEqual("who does the man give John's flower to", realiser.realise(s3).Realisation); //$NON-NLS-1$
        }

        /**
         * WH movement in the progressive
         */
        [Test]
        public virtual void testProgrssiveWHSubjectQuestions()
        {
            SPhraseSpec p = phraseFactory.createClause();
            p.setSubject("Mary");
            p.setVerb("eat");
            p.setObject(phraseFactory.createNounPhrase("the", "pie"));
            p.setFeature(Feature.PROGRESSIVE, true);
            p.setFeature(Feature.INTERROGATIVE_TYPE, InterrogativeType.WHO_SUBJECT);
            Assert.AreEqual("who is eating the pie", realiser.realise(p).Realisation); //$NON-NLS-1$
        }

        /**
         * WH movement in the progressive
         */
        [Test]
        public virtual void testProgrssiveWHObjectQuestions()
        {
            SPhraseSpec p = phraseFactory.createClause();
            p.setSubject("Mary");
            p.setVerb("eat");
            p.setObject(phraseFactory.createNounPhrase("the", "pie"));
            p.setFeature(Feature.PROGRESSIVE, true);
            p.setFeature(Feature.INTERROGATIVE_TYPE, InterrogativeType.WHAT_OBJECT);
            Assert.AreEqual("what is Mary eating", realiser.realise(p).Realisation); //$NON-NLS-1$

            // AG -- need to check this; it doesn't work
            // p.setFeature(Feature.NEGATED, true);
            //		Assert.assertEquals("what is Mary not eating", //$NON-NLS-1$
            // this.realiser.realise(p).getRealisation());
        }

        /**
         * Negation with WH movement for subject
         */
        [Test]
        public virtual void testNegatedWHSubjQuestions()
        {
            SPhraseSpec p = phraseFactory.createClause();
            p.setSubject("Mary");
            p.setVerb("eat");
            p.setObject(phraseFactory.createNounPhrase("the", "pie"));
            p.setFeature(Feature.NEGATED, true);
            p.setFeature(Feature.INTERROGATIVE_TYPE, InterrogativeType.WHO_SUBJECT);
            Assert.AreEqual("who does not eat the pie", realiser.realise(p).Realisation); //$NON-NLS-1$
        }

        /**
         * Negation with WH movement for object
         */
        [Test]
        public virtual void testNegatedWHObjQuestions()
        {
            SPhraseSpec p = phraseFactory.createClause();
            p.setSubject("Mary");
            p.setVerb("eat");
            p.setObject(phraseFactory.createNounPhrase("the", "pie"));
            p.setFeature(Feature.NEGATED, true);

            p.setFeature(Feature.INTERROGATIVE_TYPE, InterrogativeType.WHAT_OBJECT);
            NLGElement realisation = realiser.realise(p);
            Assert.AreEqual("what does Mary not eat", realisation.Realisation); //$NON-NLS-1$
        }

        /**
         * Test questyions in the tutorial.
         */
        [Test]
        public virtual void testTutorialQuestions()
        {
            setUp();
            realiser.Lexicon = lexicon;

            PhraseElement
                p = phraseFactory.createClause("Mary", "chase", "George"); //$NON-NLS-1$ - $NON-NLS-1$ //$NON-NLS-2$
            p.setFeature(Feature.INTERROGATIVE_TYPE, InterrogativeType.YES_NO);
            Assert.AreEqual("does Mary chase George", realiser.realise(p).Realisation); //$NON-NLS-1$

            p = phraseFactory.createClause("Mary", "chase", "George"); //$NON-NLS-1$ - $NON-NLS-1$ //$NON-NLS-2$
            p.setFeature(Feature.INTERROGATIVE_TYPE, InterrogativeType.WHO_OBJECT);
            Assert.AreEqual("who does Mary chase", realiser.realise(p).Realisation); //$NON-NLS-1$
        }

        /**
         * Subject WH Questions with modals
         */
        [Test]
        public virtual void testModalWHSubjectQuestion()
        {
            SPhraseSpec p = phraseFactory.createClause(dog, "upset", man);
            p.setFeature(Feature.TENSE, Tense.PAST);
            Assert.AreEqual("the dog upset the man", realiser.realise(p).Realisation);


            // first without modal
            p.setFeature(Feature.INTERROGATIVE_TYPE, InterrogativeType.WHO_SUBJECT);
            Assert.AreEqual("who upset the man", realiser.realise(p).Realisation);

            p.setFeature(Feature.INTERROGATIVE_TYPE, InterrogativeType.WHAT_SUBJECT);
            Assert.AreEqual("what upset the man", realiser.realise(p).Realisation);


            // now with modal auxiliary
            p.setFeature(Feature.MODAL, "may");

            p.setFeature(Feature.INTERROGATIVE_TYPE, InterrogativeType.WHO_SUBJECT);
            Assert.AreEqual("who may have upset the man", realiser.realise(p).Realisation);

            p.setFeature(Feature.TENSE, Tense.FUTURE);
            Assert.AreEqual("who may upset the man", realiser.realise(p).Realisation);

            p.setFeature(Feature.TENSE, Tense.PAST);
            p.setFeature(Feature.INTERROGATIVE_TYPE, InterrogativeType.WHAT_SUBJECT);
            Assert.AreEqual("what may have upset the man", realiser.realise(p).Realisation);

            p.setFeature(Feature.TENSE, Tense.FUTURE);
            Assert.AreEqual("what may upset the man", realiser.realise(p).Realisation);
        }

        /**
         * Subject WH Questions with modals
         */
        [Test]
        public virtual void testModalWHObjectQuestion()
        {
            SPhraseSpec p = phraseFactory.createClause(dog, "upset", man);
            p.setFeature(Feature.TENSE, Tense.PAST);
            p.setFeature(Feature.INTERROGATIVE_TYPE, InterrogativeType.WHO_OBJECT);

            Assert.AreEqual("who did the dog upset", realiser.realise(p).Realisation);

            p.setFeature(Feature.MODAL, "may");
            Assert.AreEqual("who may the dog have upset", realiser.realise(p).Realisation);

            p.setFeature(Feature.INTERROGATIVE_TYPE, InterrogativeType.WHAT_OBJECT);
            Assert.AreEqual("what may the dog have upset", realiser.realise(p).Realisation);

            p.setFeature(Feature.TENSE, Tense.FUTURE);
            p.setFeature(Feature.INTERROGATIVE_TYPE, InterrogativeType.WHO_OBJECT);
            Assert.AreEqual("who may the dog upset", realiser.realise(p).Realisation);

            p.setFeature(Feature.INTERROGATIVE_TYPE, InterrogativeType.WHAT_OBJECT);
            Assert.AreEqual("what may the dog upset", realiser.realise(p).Realisation);
        }

        /**
         * Questions with tenses requiring auxiliaries + subject WH
         */
        [Test]
        public virtual void testAuxWHSubjectQuestion()
        {
            SPhraseSpec p = phraseFactory.createClause(dog, "upset", man);
            p.setFeature(Feature.TENSE, Tense.PRESENT);
            p.setFeature(Feature.PERFECT, true);
            Assert.AreEqual("the dog has upset the man", realiser.realise(p).Realisation);

            p.setFeature(Feature.INTERROGATIVE_TYPE, InterrogativeType.WHO_SUBJECT);
            Assert.AreEqual("who has upset the man", realiser.realise(p).Realisation);

            p.setFeature(Feature.INTERROGATIVE_TYPE, InterrogativeType.WHAT_SUBJECT);
            Assert.AreEqual("what has upset the man", realiser.realise(p).Realisation);
        }

        /**
         * Questions with tenses requiring auxiliaries + subject WH
         */
        [Test]
        public virtual void testAuxWHObjectQuestion()
        {
            SPhraseSpec p = phraseFactory.createClause(dog, "upset", man);


            // first without any aux
            p.setFeature(Feature.TENSE, Tense.PAST);
            p.setFeature(Feature.INTERROGATIVE_TYPE, InterrogativeType.WHAT_OBJECT);
            Assert.AreEqual("what did the dog upset", realiser.realise(p).Realisation);

            p.setFeature(Feature.INTERROGATIVE_TYPE, InterrogativeType.WHO_OBJECT);
            Assert.AreEqual("who did the dog upset", realiser.realise(p).Realisation);

            p.setFeature(Feature.TENSE, Tense.PRESENT);
            p.setFeature(Feature.PERFECT, true);

            p.setFeature(Feature.INTERROGATIVE_TYPE, InterrogativeType.WHO_OBJECT);
            Assert.AreEqual("who has the dog upset", realiser.realise(p).Realisation);

            p.setFeature(Feature.INTERROGATIVE_TYPE, InterrogativeType.WHAT_OBJECT);
            Assert.AreEqual("what has the dog upset", realiser.realise(p).Realisation);

            p.setFeature(Feature.TENSE, Tense.FUTURE);
            p.setFeature(Feature.PERFECT, true);

            p.setFeature(Feature.INTERROGATIVE_TYPE, InterrogativeType.WHO_OBJECT);
            Assert.AreEqual("who will the dog have upset", realiser.realise(p).Realisation);

            p.setFeature(Feature.INTERROGATIVE_TYPE, InterrogativeType.WHAT_OBJECT);
            Assert.AreEqual("what will the dog have upset", realiser.realise(p).Realisation);
        }

        /**
         * Test for questions with "be"
         */
        [Test]
        public virtual void testBeQuestions()
        {
            SPhraseSpec p = phraseFactory.createClause(phraseFactory.createNounPhrase("a", "ball"),
                phraseFactory.createWord("be", new LexicalCategory(LexicalCategory.LexicalCategoryEnum.VERB)),
                phraseFactory.createNounPhrase("a", "toy"));

            p.setFeature(Feature.INTERROGATIVE_TYPE, InterrogativeType.WHAT_OBJECT);
            Assert.AreEqual("what is a ball", realiser.realise(p).Realisation);

            p.setFeature(Feature.INTERROGATIVE_TYPE, InterrogativeType.YES_NO);
            Assert.AreEqual("is a ball a toy", realiser.realise(p).Realisation);

            p.setFeature(Feature.INTERROGATIVE_TYPE, InterrogativeType.WHAT_SUBJECT);
            Assert.AreEqual("what is a toy", realiser.realise(p).Realisation);

            SPhraseSpec p2 = phraseFactory.createClause("Mary", "be", "beautiful");
            p2.setFeature(Feature.INTERROGATIVE_TYPE, InterrogativeType.WHY);
            Assert.AreEqual("why is Mary beautiful", realiser.realise(p2).Realisation);

            p2.setFeature(Feature.INTERROGATIVE_TYPE, InterrogativeType.WHERE);
            Assert.AreEqual("where is Mary beautiful", realiser.realise(p2).Realisation);

            p2.setFeature(Feature.INTERROGATIVE_TYPE, InterrogativeType.WHO_SUBJECT);
            Assert.AreEqual("who is beautiful", realiser.realise(p2).Realisation);
        }

        /**
         * Test for questions with "be" in future tense
         */
        [Test]
        public virtual void testBeQuestionsFuture()
        {
            SPhraseSpec p = phraseFactory.createClause(phraseFactory.createNounPhrase("a", "ball"),
                phraseFactory.createWord("be", new LexicalCategory(LexicalCategory.LexicalCategoryEnum.VERB)),
                phraseFactory.createNounPhrase("a", "toy"));
            p.setFeature(Feature.TENSE, Tense.FUTURE);

            p.setFeature(Feature.INTERROGATIVE_TYPE, InterrogativeType.WHAT_OBJECT);
            Assert.AreEqual("what will a ball be", realiser.realise(p).Realisation);

            p.setFeature(Feature.INTERROGATIVE_TYPE, InterrogativeType.YES_NO);
            Assert.AreEqual("will a ball be a toy", realiser.realise(p).Realisation);

            p.setFeature(Feature.INTERROGATIVE_TYPE, InterrogativeType.WHAT_SUBJECT);
            Assert.AreEqual("what will be a toy", realiser.realise(p).Realisation);

            SPhraseSpec p2 = phraseFactory.createClause("Mary", "be", "beautiful");
            p2.setFeature(Feature.TENSE, Tense.FUTURE);
            p2.setFeature(Feature.INTERROGATIVE_TYPE, InterrogativeType.WHY);
            Assert.AreEqual("why will Mary be beautiful", realiser.realise(p2).Realisation);

            p2.setFeature(Feature.INTERROGATIVE_TYPE, InterrogativeType.WHERE);
            Assert.AreEqual("where will Mary be beautiful", realiser.realise(p2).Realisation);

            p2.setFeature(Feature.INTERROGATIVE_TYPE, InterrogativeType.WHO_SUBJECT);
            Assert.AreEqual("who will be beautiful", realiser.realise(p2).Realisation);
        }

        /**
         * Tests for WH questions with be in past tense
         */
        [Test]
        public virtual void testBeQuestionsPast()
        {
            SPhraseSpec p = phraseFactory.createClause(phraseFactory.createNounPhrase("a", "ball"),
                phraseFactory.createWord("be", new LexicalCategory(LexicalCategory.LexicalCategoryEnum.VERB)),
                phraseFactory.createNounPhrase("a", "toy"));
            p.setFeature(Feature.TENSE, Tense.PAST);

            p.setFeature(Feature.INTERROGATIVE_TYPE, InterrogativeType.WHAT_OBJECT);
            Assert.AreEqual("what was a ball", realiser.realise(p).Realisation);

            p.setFeature(Feature.INTERROGATIVE_TYPE, InterrogativeType.YES_NO);
            Assert.AreEqual("was a ball a toy", realiser.realise(p).Realisation);

            p.setFeature(Feature.INTERROGATIVE_TYPE, InterrogativeType.WHAT_SUBJECT);
            Assert.AreEqual("what was a toy", realiser.realise(p).Realisation);

            SPhraseSpec p2 = phraseFactory.createClause("Mary", "be", "beautiful");
            p2.setFeature(Feature.TENSE, Tense.PAST);
            p2.setFeature(Feature.INTERROGATIVE_TYPE, InterrogativeType.WHY);
            Assert.AreEqual("why was Mary beautiful", realiser.realise(p2).Realisation);

            p2.setFeature(Feature.INTERROGATIVE_TYPE, InterrogativeType.WHERE);
            Assert.AreEqual("where was Mary beautiful", realiser.realise(p2).Realisation);

            p2.setFeature(Feature.INTERROGATIVE_TYPE, InterrogativeType.WHO_SUBJECT);
            Assert.AreEqual("who was beautiful", realiser.realise(p2).Realisation);
        }


        /**
         * Test WHERE, HOW and WHY questions, with copular predicate "be"
         */
        [Test]
        public virtual void testSimpleBeWHQuestions()
        {
            SPhraseSpec p = phraseFactory.createClause("I", "be");

            p.setFeature(Feature.INTERROGATIVE_TYPE, InterrogativeType.WHERE);
            Assert.AreEqual("Where am I?", realiser.realiseSentence(p));

            p.setFeature(Feature.INTERROGATIVE_TYPE, InterrogativeType.WHY);
            Assert.AreEqual("Why am I?", realiser.realiseSentence(p));

            p.setFeature(Feature.INTERROGATIVE_TYPE, InterrogativeType.HOW);
            Assert.AreEqual("How am I?", realiser.realiseSentence(p));
        }

        /**
         * Test a simple "how" question, based on query from Albi Oxa
         */
        [Test]
        public virtual void testHowPredicateQuestion()
        {
            SPhraseSpec test = phraseFactory.createClause();
            NPPhraseSpec subject = phraseFactory.createNounPhrase("You");

            subject.setFeature(Feature.PRONOMINAL, true);
            subject.setFeature(Feature.PERSON, Person.SECOND);
            test.setSubject(subject);
            test.setVerb("be");

            test.setFeature(Feature.INTERROGATIVE_TYPE, InterrogativeType.HOW_PREDICATE);
            test.setFeature(Feature.TENSE, Tense.PRESENT);

            string result = realiser.realiseSentence(test);
            Assert.AreEqual("How are you?", result);
        }

        /**
         * Case 1 checks that "What do you think about John?" can be generated.
         * 
         * Case 2 checks that the same clause is generated, even when an object is
         * declared.
         */
        [Test]
        public virtual void testWhatObjectInterrogative()
        {
            Lexicon lexicon = Lexicon.DefaultLexicon;
            NLGFactory nlg = new NLGFactory(lexicon);
            Realiser realiser = new Realiser(lexicon);

            // Case 1, no object is explicitly given:
            SPhraseSpec clause = nlg.createClause("you", "think");
            PPPhraseSpec aboutJohn = nlg.createPrepositionPhrase("about", "John");
            clause.addPostModifier(aboutJohn);
            clause.setFeature(Feature.INTERROGATIVE_TYPE, InterrogativeType.WHAT_OBJECT);
            string realisation = realiser.realiseSentence(clause);
            Console.WriteLine(realisation);
            Assert.AreEqual("What do you think about John?", realisation);

            // Case 2:
            // Add "bad things" as the object so the object doesn't remain null:
            clause.setObject("bad things");
            realisation = realiser.realiseSentence(clause);
            Console.WriteLine(realiser.realiseSentence(clause));
            Assert.AreEqual("What do you think about John?", realisation);
        }
    }
}