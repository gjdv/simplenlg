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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleNLG.Main.features;
using SimpleNLG.Main.framework;
using SimpleNLG.Main.phrasespec;

namespace SimpleNLG.Test.syntax.english
{
    using DiscourseFunction = DiscourseFunction;
    using Feature = Feature;
    using Tense = Tense;
    using Gender = Gender;
    using InternalFeature = InternalFeature;
    using LexicalFeature = LexicalFeature;
    using NumberAgreement = NumberAgreement;
    using Person = Person;
    using CoordinatedPhraseElement = CoordinatedPhraseElement;
    using LexicalCategory = LexicalCategory;
    using NLGElement = NLGElement;
    using PhraseElement = PhraseElement;
    using NPPhraseSpec = NPPhraseSpec;
    using SPhraseSpec = SPhraseSpec;

    /**
     * Tests for the NPPhraseSpec and CoordinateNPPhraseSpec classes.
     * 
     * @author agatt
     */
    [TestClass]
    public class NounPhraseTest : SimpleNLG4Test
    {
        public NounPhraseTest() : this(null)
        {
        }

        /**
         * Instantiates a new nP test.
         * 
         * @param name
         *            the name
         */
        public NounPhraseTest(string name) : base(name)
        {
        }

        [TestCleanup]
        public override void tearDown()
        {
            base.tearDown();
        }


        /**
         * Test the setPlural() method in noun phrases.
         */
        [TestMethod]
        public virtual void testPlural()
        {
            np4.setFeature(Feature.NUMBER, NumberAgreement.PLURAL);
            Assert.AreEqual("the rocks", realiser.realise(np4).Realisation); //$NON-NLS-1$

            np5.setFeature(Feature.NUMBER, NumberAgreement.PLURAL);
            Assert.AreEqual("the curtains", realiser.realise(np5).Realisation); //$NON-NLS-1$

            np5.setFeature(Feature.NUMBER, NumberAgreement.SINGULAR);
            Assert.AreEqual(NumberAgreement.SINGULAR, np5.getFeature(Feature.NUMBER));
            Assert.AreEqual("the curtain", realiser.realise(np5).Realisation); //$NON-NLS-1$

            np5.setFeature(Feature.NUMBER, NumberAgreement.PLURAL);
            Assert.AreEqual("the curtains", realiser.realise(np5).Realisation); //$NON-NLS-1$
        }

        /**
         * Test the pronominalisation method for full NPs.
         */
        [TestMethod]
        public virtual void testPronominalisation()
        {
            // sing
            proTest1.setFeature(LexicalFeature.GENDER, Gender.FEMININE);
            proTest1.setFeature(Feature.PRONOMINAL, true);
            Assert.AreEqual("she", realiser.realise(proTest1).Realisation); //$NON-NLS-1$

            // sing, possessive
            proTest1.setFeature(Feature.POSSESSIVE, true);
            Assert.AreEqual("her", realiser.realise(proTest1).Realisation); //$NON-NLS-1$

            // plural pronoun
            proTest2.setFeature(Feature.NUMBER, NumberAgreement.PLURAL);
            proTest2.setFeature(Feature.PRONOMINAL, true);
            Assert.AreEqual("they", realiser.realise(proTest2).Realisation); //$NON-NLS-1$

            // accusative: "them"
            proTest2.setFeature(InternalFeature.DISCOURSE_FUNCTION, DiscourseFunction.OBJECT);
            Assert.AreEqual("them", realiser.realise(proTest2).Realisation); //$NON-NLS-1$
        }

        /**
         * Test the pronominalisation method for full NPs (more thorough than above)
         */
        [TestMethod]
        public virtual void testPronominalisation2()
        {
            // Ehud - added extra pronominalisation tests
            NPPhraseSpec pro = phraseFactory.createNounPhrase("Mary");
            pro.setFeature(Feature.PRONOMINAL, true);
            pro.setFeature(Feature.PERSON, Person.FIRST);
            SPhraseSpec sent = phraseFactory.createClause(pro, "like", "John");
            Assert.AreEqual("I like John.", realiser.realiseSentence(sent));

            pro = phraseFactory.createNounPhrase("Mary");
            pro.setFeature(Feature.PRONOMINAL, true);
            pro.setFeature(Feature.PERSON, Person.SECOND);
            sent = phraseFactory.createClause(pro, "like", "John");
            Assert.AreEqual("You like John.", realiser.realiseSentence(sent));

            pro = phraseFactory.createNounPhrase("Mary");
            pro.setFeature(Feature.PRONOMINAL, true);
            pro.setFeature(Feature.PERSON, Person.THIRD);
            pro.setFeature(LexicalFeature.GENDER, Gender.FEMININE);
            sent = phraseFactory.createClause(pro, "like", "John");
            Assert.AreEqual("She likes John.", realiser.realiseSentence(sent));

            pro = phraseFactory.createNounPhrase("Mary");
            pro.setFeature(Feature.PRONOMINAL, true);
            pro.setFeature(Feature.PERSON, Person.FIRST);
            pro.Plural = true;
            sent = phraseFactory.createClause(pro, "like", "John");
            Assert.AreEqual("We like John.", realiser.realiseSentence(sent));

            pro = phraseFactory.createNounPhrase("Mary");
            pro.setFeature(Feature.PRONOMINAL, true);
            pro.setFeature(Feature.PERSON, Person.SECOND);
            pro.Plural = true;
            sent = phraseFactory.createClause(pro, "like", "John");
            Assert.AreEqual("You like John.", realiser.realiseSentence(sent));

            pro = phraseFactory.createNounPhrase("Mary");
            pro.setFeature(Feature.PRONOMINAL, true);
            pro.setFeature(Feature.PERSON, Person.THIRD);
            pro.Plural = true;
            pro.setFeature(LexicalFeature.GENDER, Gender.FEMININE);
            sent = phraseFactory.createClause(pro, "like", "John");
            Assert.AreEqual("They like John.", realiser.realiseSentence(sent));

            pro = phraseFactory.createNounPhrase("John");
            pro.setFeature(Feature.PRONOMINAL, true);
            pro.setFeature(Feature.PERSON, Person.FIRST);
            sent = phraseFactory.createClause("Mary", "like", pro);
            Assert.AreEqual("Mary likes me.", realiser.realiseSentence(sent));

            pro = phraseFactory.createNounPhrase("John");
            pro.setFeature(Feature.PRONOMINAL, true);
            pro.setFeature(Feature.PERSON, Person.SECOND);
            sent = phraseFactory.createClause("Mary", "like", pro);
            Assert.AreEqual("Mary likes you.", realiser.realiseSentence(sent));

            pro = phraseFactory.createNounPhrase("John");
            pro.setFeature(Feature.PRONOMINAL, true);
            pro.setFeature(Feature.PERSON, Person.THIRD);
            pro.setFeature(LexicalFeature.GENDER, Gender.MASCULINE);
            sent = phraseFactory.createClause("Mary", "like", pro);
            Assert.AreEqual("Mary likes him.", realiser.realiseSentence(sent));

            pro = phraseFactory.createNounPhrase("John");
            pro.setFeature(Feature.PRONOMINAL, true);
            pro.setFeature(Feature.PERSON, Person.FIRST);
            pro.Plural = true;
            sent = phraseFactory.createClause("Mary", "like", pro);
            Assert.AreEqual("Mary likes us.", realiser.realiseSentence(sent));

            pro = phraseFactory.createNounPhrase("John");
            pro.setFeature(Feature.PRONOMINAL, true);
            pro.setFeature(Feature.PERSON, Person.SECOND);
            pro.Plural = true;
            sent = phraseFactory.createClause("Mary", "like", pro);
            Assert.AreEqual("Mary likes you.", realiser.realiseSentence(sent));

            pro = phraseFactory.createNounPhrase("John");
            pro.setFeature(Feature.PRONOMINAL, true);
            pro.setFeature(Feature.PERSON, Person.THIRD);
            pro.setFeature(LexicalFeature.GENDER, Gender.MASCULINE);
            pro.Plural = true;
            sent = phraseFactory.createClause("Mary", "like", pro);
            Assert.AreEqual("Mary likes them.", realiser.realiseSentence(sent));
        }

        /**
         * Test premodification in NPS.
         */
        [TestMethod]
        public virtual void testPremodification()
        {
            man.addPreModifier(salacious);
            Assert.AreEqual("the salacious man", realiser.realise(man).Realisation); //$NON-NLS-1$

            woman.addPreModifier(beautiful);
            Assert.AreEqual("the beautiful woman", realiser.realise(woman).Realisation); //$NON-NLS-1$

            dog.addPreModifier(stunning);
            Assert.AreEqual("the stunning dog", realiser.realise(dog).Realisation); //$NON-NLS-1$


            // premodification with a WordElement
            man.setPreModifier(phraseFactory.createWord("idiotic",
                new LexicalCategory(LexicalCategory.LexicalCategoryEnum.ADJECTIVE)));
            Assert.AreEqual("the idiotic man", realiser.realise(man).Realisation); //$NON-NLS-1$
        }

        /**
         * Test prepositional postmodification.
         */
        [TestMethod]
        public virtual void testPostmodification()
        {
            man.addPostModifier(onTheRock);
            Assert.AreEqual("the man on the rock", realiser.realise(man).Realisation); //$NON-NLS-1$

            woman.addPostModifier(behindTheCurtain);
            Assert.AreEqual("the woman behind the curtain", realiser.realise(woman).Realisation); //$NON-NLS-1$


            // postmodification with a WordElement
            man.setPostModifier(phraseFactory.createWord("jack",
                new LexicalCategory(LexicalCategory.LexicalCategoryEnum.NOUN)));
            Assert.AreEqual("the man jack", realiser.realise(man).Realisation); //$NON-NLS-1$
        }

        /**
         * Test nominal complementation
         */
        [TestMethod]
        public virtual void testComplementation()
        {
            // complementation with a WordElement
            man.Complement =
                phraseFactory.createWord("jack", new LexicalCategory(LexicalCategory.LexicalCategoryEnum.NOUN));
            Assert.AreEqual("the man jack", realiser.realise(man).Realisation); //$NON-NLS-1$

            woman.addComplement(behindTheCurtain);
            Assert.AreEqual("the woman behind the curtain", realiser.realise(woman).Realisation); //$NON-NLS-1$
        }

        /**
         * Test possessive constructions.
         */
        [TestMethod]
        public virtual void testPossessive()
        {
            // simple possessive 's: 'a man's'
            PhraseElement possNP = phraseFactory.createNounPhrase("a", "man"); //$NON-NLS-1$ //$NON-NLS-2$
            possNP.setFeature(Feature.POSSESSIVE, true);
            Assert.AreEqual("a man's", realiser.realise(possNP).Realisation); //$NON-NLS-1$


            // now set this possessive as specifier of the NP 'the dog'
            dog.setFeature(InternalFeature.SPECIFIER, possNP);
            Assert.AreEqual("a man's dog", realiser.realise(dog).Realisation); //$NON-NLS-1$


            // convert possNP to pronoun and turn "a dog" into "his dog"
            // need to specify gender, as default is NEUTER
            possNP.setFeature(LexicalFeature.GENDER, Gender.MASCULINE);
            possNP.setFeature(Feature.PRONOMINAL, true);
            Assert.AreEqual("his dog", realiser.realise(dog).Realisation); //$NON-NLS-1$


            // make it slightly more complicated: "his dog's rock"
            dog.setFeature(Feature.POSSESSIVE, true); // his dog's

            // his dog's rock (substituting "the"  for the entire phrase)
            np4.setFeature(InternalFeature.SPECIFIER, dog);
            Assert.AreEqual("his dog's rock", realiser.realise(np4).Realisation); //$NON-NLS-1$
        }

        /**
         * Test NP coordination.
         */
        [TestMethod]
        public virtual void testCoordination()
        {
            CoordinatedPhraseElement cnp1 = new CoordinatedPhraseElement(dog, woman);

            // simple coordination
            Assert.AreEqual("the dog and the woman", realiser.realise(cnp1).Realisation); //$NON-NLS-1$


            // simple coordination with complementation of entire coordinate NP
            cnp1.addComplement(behindTheCurtain);
            Assert.AreEqual("the dog and the woman behind the curtain",
                realiser.realise(cnp1).Realisation); //$NON-NLS-1$


            // raise the specifier in this cnp
            // Assert.assertEquals(true, cnp1.raiseSpecifier()); // should return
            // true as all
            // sub-nps have same spec
            // assertEquals("the dog and woman behind the curtain",
            // realiser.realise(cnp1));
        }

        /**
         * Another battery of tests for NP coordination.
         */
        [TestMethod]
        public virtual void testCoordination2()
        {
            // simple coordination of complementised nps
            dog.clearComplements();
            woman.clearComplements();

            CoordinatedPhraseElement cnp1 = new CoordinatedPhraseElement(dog, woman);
            cnp1.setFeature(Feature.RAISE_SPECIFIER, true);
            NLGElement realised = realiser.realise(cnp1);
            Assert.AreEqual("the dog and woman", realised.Realisation);

            dog.addComplement(onTheRock);
            woman.addComplement(behindTheCurtain);

            CoordinatedPhraseElement cnp2 = new CoordinatedPhraseElement(dog, woman);

            woman.setFeature(InternalFeature.RAISED, false);
            Assert.AreEqual("the dog on the rock and the woman behind the curtain",
                realiser.realise(cnp2).Realisation); //$NON-NLS-1$


            // complementised coordinates + outer pp modifier
            cnp2.addPostModifier(inTheRoom);
            Assert.AreEqual("the dog on the rock and the woman behind the curtain in the room",
                realiser.realise(cnp2).Realisation); //$NON-NLS-1$


            // set the specifier for this cnp; should unset specifiers for all inner coordinates
            NLGElement every = phraseFactory.createWord("every",
                new LexicalCategory(LexicalCategory.LexicalCategoryEnum.DETERMINER)); //$NON-NLS-1$

            cnp2.setFeature(InternalFeature.SPECIFIER, every);

            Assert.AreEqual("every dog on the rock and every woman behind the curtain in the room",
                realiser.realise(cnp2).Realisation); //$NON-NLS-1$


            // pronominalise one of the constituents
            dog.setFeature(Feature.PRONOMINAL, true); // ="it"
            dog.setFeature(InternalFeature.SPECIFIER,
                phraseFactory.createWord("the", new LexicalCategory(LexicalCategory.LexicalCategoryEnum.DETERMINER)));

            // raising spec still returns true as spec has been set
            cnp2.setFeature(Feature.RAISE_SPECIFIER, true);

            // CNP should be realised with pronominal internal const
            Assert.AreEqual("it and every woman behind the curtain in the room",
                realiser.realise(cnp2).Realisation); //$NON-NLS-1$
        }

        /**
         * Test possessives in coordinate NPs.
         */
        [TestMethod]
        public virtual void testPossessiveCoordinate()
        {
            // simple coordination
            CoordinatedPhraseElement cnp2 = new CoordinatedPhraseElement(dog, woman);
            Assert.AreEqual("the dog and the woman", realiser.realise(cnp2).Realisation); //$NON-NLS-1$


            // set possessive -- wide-scope by default
            cnp2.setFeature(Feature.POSSESSIVE, true);
            Assert.AreEqual("the dog and the woman's", realiser.realise(cnp2).Realisation); //$NON-NLS-1$


            // set possessive with pronoun
            dog.setFeature(Feature.PRONOMINAL, true);
            dog.setFeature(Feature.POSSESSIVE, true);
            cnp2.setFeature(Feature.POSSESSIVE, true);
            Assert.AreEqual("its and the woman's", realiser.realise(cnp2).Realisation); //$NON-NLS-1$
        }

        /**
         * Test A vs An.
         */
        [TestMethod]
        public virtual void testAAn()
        {
            PhraseElement _dog = phraseFactory.createNounPhrase("a", "dog"); //$NON-NLS-1$ //$NON-NLS-2$
            Assert.AreEqual("a dog", realiser.realise(_dog).Realisation); //$NON-NLS-1$

            _dog.addPreModifier("enormous"); //$NON-NLS-1$

            Assert.AreEqual("an enormous dog", realiser.realise(_dog).Realisation); //$NON-NLS-1$

            PhraseElement elephant = phraseFactory.createNounPhrase("a", "elephant"); //$NON-NLS-1$ //$NON-NLS-2$
            Assert.AreEqual("an elephant", realiser.realise(elephant).Realisation); //$NON-NLS-1$

            elephant.addPreModifier("big"); //$NON-NLS-1$
            Assert.AreEqual("a big elephant", realiser.realise(elephant).Realisation); //$NON-NLS-1$


            // test treating of plural specifiers
            _dog.setFeature(Feature.NUMBER, NumberAgreement.PLURAL);

            Assert.AreEqual("some enormous dogs", realiser.realise(_dog).Realisation); //$NON-NLS-1$
        }

        /**
         * Further tests for a/an agreement with coordinated premodifiers
         */
        [TestMethod]
        public virtual void testAAnCoord()
        {
            NPPhraseSpec _dog = phraseFactory.createNounPhrase("a", "dog");
            _dog.addPreModifier(phraseFactory.createCoordinatedPhrase("enormous", "black"));
            string realisation = realiser.realise(_dog).Realisation;
            Assert.AreEqual("an enormous and black dog", realisation);
        }

        /**
         * Test for a/an agreement with numbers
         */
        [TestMethod]
        public virtual void testAAnWithNumbers()
        {
            NPPhraseSpec num = phraseFactory.createNounPhrase("a", "change");
            string realisation;

            // no an with "one"
            num.setPreModifier("one percent");
            realisation = realiser.realise(num).Realisation;
            Assert.AreEqual("a one percent change", realisation);

            // an with "eighty"
            num.setPreModifier("eighty percent");
            realisation = realiser.realise(num).Realisation;
            Assert.AreEqual("an eighty percent change", realisation);

            // an with 80
            num.setPreModifier("80%");
            realisation = realiser.realise(num).Realisation;
            Assert.AreEqual("an 80% change", realisation);

            // an with 80000
            num.setPreModifier("80000");
            realisation = realiser.realise(num).Realisation;
            Assert.AreEqual("an 80000 change", realisation);

            // an with 11,000
            num.setPreModifier("11,000");
            realisation = realiser.realise(num).Realisation;
            Assert.AreEqual("an 11,000 change", realisation);

            // an with 18
            num.setPreModifier("18%");
            realisation = realiser.realise(num).Realisation;
            Assert.AreEqual("an 18% change", realisation);

            // a with 180
            num.setPreModifier("180");
            realisation = realiser.realise(num).Realisation;
            Assert.AreEqual("a 180 change", realisation);

            // a with 1100
            num.setPreModifier("1100");
            realisation = realiser.realise(num).Realisation;
            Assert.AreEqual("a 1100 change", realisation);

            // a with 180,000
            num.setPreModifier("180,000");
            realisation = realiser.realise(num).Realisation;
            Assert.AreEqual("a 180,000 change", realisation);

            // an with 11000
            num.setPreModifier("11000");
            realisation = realiser.realise(num).Realisation;
            Assert.AreEqual("an 11000 change", realisation);

            // an with 18000
            num.setPreModifier("18000");
            realisation = realiser.realise(num).Realisation;
            Assert.AreEqual("an 18000 change", realisation);

            // an with 18.1
            num.setPreModifier("18.1%");
            realisation = realiser.realise(num).Realisation;
            Assert.AreEqual("an 18.1% change", realisation);

            // an with 11.1
            num.setPreModifier("11.1%");
            realisation = realiser.realise(num).Realisation;
            Assert.AreEqual("an 11.1% change", realisation);
        }

        /**
         * Test Modifier "guess" placement.
         */
        [TestMethod]
        public virtual void testModifier()
        {
            PhraseElement _dog = phraseFactory.createNounPhrase("a", "dog"); //$NON-NLS-1$ //$NON-NLS-2$
            _dog.addPreModifier("angry"); //$NON-NLS-1$

            Assert.AreEqual("an angry dog", realiser.realise(_dog).Realisation); //$NON-NLS-1$

            _dog.addPostModifier("in the park"); //$NON-NLS-1$
            Assert.AreEqual("an angry dog in the park", realiser.realise(_dog).Realisation); //$NON-NLS-1$

            PhraseElement cat = phraseFactory.createNounPhrase("a", "cat"); //$NON-NLS-1$ //$NON-NLS-2$
            cat.addPreModifier(phraseFactory.createAdjectivePhrase("angry")); //$NON-NLS-1$
            Assert.AreEqual("an angry cat", realiser.realise(cat).Realisation); //$NON-NLS-1$

            cat.addPostModifier(phraseFactory.createPrepositionPhrase("in", "the park")); //$NON-NLS-1$ //$NON-NLS-2$
            Assert.AreEqual("an angry cat in the park", realiser.realise(cat).Realisation); //$NON-NLS-1$
        }

        [TestMethod]
        public virtual void testPluralNounsBelongingToASingular()
        {
            SPhraseSpec sent = phraseFactory.createClause("I", "count up");
            sent.setFeature(Feature.TENSE, Tense.PAST);
            NPPhraseSpec obj = phraseFactory.createNounPhrase("digit");
            obj.Plural = true;
            NPPhraseSpec possessor = phraseFactory.createNounPhrase("the", "box");
            possessor.Plural = false;
            possessor.setFeature(Feature.POSSESSIVE, true);
            obj.setSpecifier(possessor);
            sent.setObject(obj);

            Assert.AreEqual("I counted up the box's digits", realiser.realise(sent).Realisation); //$NON-NLS-1$
        }


        [TestMethod]
        public virtual void testSingularNounsBelongingToAPlural()
        {
            SPhraseSpec sent = phraseFactory.createClause("I", "clean");
            sent.setFeature(Feature.TENSE, Tense.PAST);
            NPPhraseSpec obj = phraseFactory.createNounPhrase("car");
            obj.Plural = false;
            NPPhraseSpec possessor = phraseFactory.createNounPhrase("the", "parent");
            possessor.Plural = true;
            possessor.setFeature(Feature.POSSESSIVE, true);
            obj.setSpecifier(possessor);
            sent.setObject(obj);

            Assert.AreEqual("I cleaned the parents' car", realiser.realise(sent).Realisation); //$NON-NLS-1$
        }

        /**
         * Test for appositive postmodifiers
         */
        [TestMethod]
        public virtual void testAppositivePostmodifier()
        {
            PhraseElement _dog = phraseFactory.createNounPhrase("the", "dog");
            PhraseElement _rott = phraseFactory.createNounPhrase("a", "rottweiler");
            _rott.setFeature(Feature.APPOSITIVE, true);
            _dog.addPostModifier(_rott);
            SPhraseSpec _sent = phraseFactory.createClause(_dog, "ran");
            Assert.AreEqual("The dog, a rottweiler, runs.", realiser.realiseSentence(_sent));
        }
    }
}