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
 * Contributor(s): Ehud Reiter, Albert Gatt, Dave Westwater, Roman Kutlak, Margaret Mitchell, Saad Mahamood.
 *
 * Ported to C# by Gert-Jan de Vries
 */

using System.Collections.Generic;
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
    using Tense = Tense;
    using CoordinatedPhraseElement = CoordinatedPhraseElement;
    using DocumentElement = DocumentElement;
    using NLGElement = NLGElement;
    using NLGFactory = NLGFactory;
    using Lexicon = Lexicon;
    using NPPhraseSpec = NPPhraseSpec;
    using PPPhraseSpec = PPPhraseSpec;
    using SPhraseSpec = SPhraseSpec;
    using VPPhraseSpec = VPPhraseSpec;
    using Realiser = Realiser;

    /**
     * Tests from SimpleNLG tutorial
     * <hr>
     * 
     * <p>
     * Copyright (C) 2011, University of Aberdeen
     * </p>
     * 
     * @author Ehud Reiter
     * 
     */
    [TestFixture]
    public class TutorialTest
    {
        // no code in sections 1 and 2

        /**
         * test section 3 code
         */
        [Test]
        public virtual void section3_Test()
        {
            Lexicon lexicon = Lexicon.DefaultLexicon; // default simplenlg lexicon
            NLGFactory nlgFactory = new NLGFactory(lexicon); // factory based on lexicon

            NLGElement s1 = nlgFactory.createSentence("my dog is happy");

            Realiser r = new Realiser(lexicon);

            string output = r.realiseSentence(s1);

            Assert.AreEqual("My dog is happy.", output);
        }

        /**
         * test section 5 code
         */
        [Test]
        public virtual void section5_Test()
        {
            Lexicon lexicon = Lexicon.DefaultLexicon; // default simplenlg lexicon
            NLGFactory nlgFactory = new NLGFactory(lexicon); // factory based on lexicon
            Realiser realiser = new Realiser(lexicon);

            SPhraseSpec p = nlgFactory.createClause();
            p.setSubject("my dog");
            p.setVerb("chase");
            p.setObject("George");

            string output = realiser.realiseSentence(p);
            Assert.AreEqual("My dog chases George.", output);
        }

        /**
         * test section 6 code
         */
        [Test]
        public virtual void section6_Test()
        {
            Lexicon lexicon = Lexicon.DefaultLexicon; // default simplenlg lexicon
            NLGFactory nlgFactory = new NLGFactory(lexicon); // factory based on lexicon
            Realiser realiser = new Realiser(lexicon);

            SPhraseSpec p = nlgFactory.createClause();
            p.setSubject("Mary");
            p.setVerb("chase");
            p.setObject("George");

            p.setFeature(Feature.TENSE, Tense.PAST);
            string output = realiser.realiseSentence(p);
            Assert.AreEqual("Mary chased George.", output);

            p.setFeature(Feature.TENSE, Tense.FUTURE);
            output = realiser.realiseSentence(p);
            Assert.AreEqual("Mary will chase George.", output);

            p.setFeature(Feature.NEGATED, true);
            output = realiser.realiseSentence(p);
            Assert.AreEqual("Mary will not chase George.", output);

            p = nlgFactory.createClause();
            p.setSubject("Mary");
            p.setVerb("chase");
            p.setObject("George");

            p.setFeature(Feature.INTERROGATIVE_TYPE, InterrogativeType.YES_NO);
            output = realiser.realiseSentence(p);
            Assert.AreEqual("Does Mary chase George?", output);

            p.setSubject("Mary");
            p.setVerb("chase");
            p.setFeature(Feature.INTERROGATIVE_TYPE, InterrogativeType.WHO_OBJECT);
            output = realiser.realiseSentence(p);
            Assert.AreEqual("Who does Mary chase?", output);

            p = nlgFactory.createClause();
            p.setSubject("the dog");
            p.setVerb("wake up");
            output = realiser.realiseSentence(p);
            Assert.AreEqual("The dog wakes up.", output);
        }

        /**
         * test ability to use variant words
         */
        [Test]
        public virtual void variantsTest()
        {
            Lexicon lexicon = Lexicon.DefaultLexicon; // default simplenlg lexicon
            NLGFactory nlgFactory = new NLGFactory(lexicon); // factory based on lexicon
            Realiser realiser = new Realiser(lexicon);

            SPhraseSpec p = nlgFactory.createClause();
            p.setSubject("my dog");
            p.setVerb("is"); // variant of be
            p.setObject("George");

            string output = realiser.realiseSentence(p);
            Assert.AreEqual("My dog is George.", output);

            p = nlgFactory.createClause();
            p.setSubject("my dog");
            p.setVerb("chases"); // variant of chase
            p.setObject("George");

            output = realiser.realiseSentence(p);
            Assert.AreEqual("My dog chases George.", output);


            p = nlgFactory.createClause();
            p.setSubject(nlgFactory.createNounPhrase("the", "dogs")); // variant of "dog"
            p.setVerb("is"); // variant of be
            p.setObject("happy"); // variant of happy
            output = realiser.realiseSentence(p);
            Assert.AreEqual("The dog is happy.", output);

            p = nlgFactory.createClause();
            p.setSubject(nlgFactory.createNounPhrase("the", "children")); // variant of "child"
            p.setVerb("is"); // variant of be
            p.setObject("happy"); // variant of happy
            output = realiser.realiseSentence(p);
            Assert.AreEqual("The child is happy.", output);

            // following functionality is enabled
            p = nlgFactory.createClause();
            p.setSubject(nlgFactory.createNounPhrase("the", "dogs")); // variant of "dog"
            p.setVerb("is"); // variant of be
            p.setObject("happy"); // variant of happy
            output = realiser.realiseSentence(p);
            Assert.AreEqual("The dog is happy.", output); //corrected automatically
        }


        /* Following code tests the section 5 to 15
         * sections 5 & 6 are repeated here in order to match the simplenlg tutorial version 4
         * James Christie
         * June 2011
         */

        /**
         * test section 5 to match simplenlg tutorial version 4's code
         */
        [Test]
        public virtual void section5A_Test()
        {
            Lexicon lexicon = Lexicon.DefaultLexicon; // default simplenlg lexicon
            NLGFactory nlgFactory = new NLGFactory(lexicon); // factory based on lexicon
            Realiser realiser = new Realiser(lexicon);

            SPhraseSpec p = nlgFactory.createClause();
            p.setSubject("Mary");
            p.setVerb("chase");
            p.setObject("the monkey");

            string output = realiser.realiseSentence(p);
            Assert.AreEqual("Mary chases the monkey.", output);
        } // testSection5A

        /**
         * test section 6 to match simplenlg tutorial version 4' code
         */
        [Test]
        public virtual void section6A_Test()
        {
            Lexicon lexicon = Lexicon.DefaultLexicon; // default simplenlg lexicon
            NLGFactory nlgFactory = new NLGFactory(lexicon); // factory based on lexicon
            Realiser realiser = new Realiser(lexicon);

            SPhraseSpec p = nlgFactory.createClause();
            p.setSubject("Mary");
            p.setVerb("chase");
            p.setObject("the monkey");

            p.setFeature(Feature.TENSE, Tense.PAST);
            string output = realiser.realiseSentence(p);
            Assert.AreEqual("Mary chased the monkey.", output);

            p.setFeature(Feature.TENSE, Tense.FUTURE);
            output = realiser.realiseSentence(p);
            Assert.AreEqual("Mary will chase the monkey.", output);

            p.setFeature(Feature.NEGATED, true);
            output = realiser.realiseSentence(p);
            Assert.AreEqual("Mary will not chase the monkey.", output);

            p = nlgFactory.createClause();
            p.setSubject("Mary");
            p.setVerb("chase");
            p.setObject("the monkey");

            p.setFeature(Feature.INTERROGATIVE_TYPE, InterrogativeType.YES_NO);
            output = realiser.realiseSentence(p);
            Assert.AreEqual("Does Mary chase the monkey?", output);

            p.setSubject("Mary");
            p.setVerb("chase");
            p.setFeature(Feature.INTERROGATIVE_TYPE, InterrogativeType.WHO_OBJECT);
            output = realiser.realiseSentence(p);
            Assert.AreEqual("Who does Mary chase?", output);
        }

        /**
         * test section 7 code
         */
        [Test]
        public virtual void section7_Test()
        {
            Lexicon lexicon = Lexicon.DefaultLexicon; // default simplenlg lexicon
            NLGFactory nlgFactory = new NLGFactory(lexicon); // factory based on lexicon
            Realiser realiser = new Realiser(lexicon);

            SPhraseSpec p = nlgFactory.createClause();
            p.setSubject("Mary");
            p.setVerb("chase");
            p.setObject("the monkey");
            p.addComplement("very quickly");
            p.addComplement("despite her exhaustion");

            string output = realiser.realiseSentence(p);
            Assert.AreEqual("Mary chases the monkey very quickly despite her exhaustion.", output);
        }

        /**
         * test section 8 code
         */
        [Test]
        public virtual void section8_Test()
        {
            Lexicon lexicon = Lexicon.DefaultLexicon; // default simplenlg lexicon
            NLGFactory nlgFactory = new NLGFactory(lexicon); // factory based on lexicon
            Realiser realiser = new Realiser(lexicon);

            NPPhraseSpec subject = nlgFactory.createNounPhrase("Mary");
            NPPhraseSpec @object = nlgFactory.createNounPhrase("the monkey");
            VPPhraseSpec verb = nlgFactory.createVerbPhrase("chase");
            subject.addModifier("fast");

            SPhraseSpec p = nlgFactory.createClause();
            p.setSubject(subject);
            p.setVerb(verb);
            p.setObject(@object);

            string outputA = realiser.realiseSentence(p);
            Assert.AreEqual("Fast Mary chases the monkey.", outputA);

            verb.addModifier("quickly");

            string outputB = realiser.realiseSentence(p);
            Assert.AreEqual("Fast Mary quickly chases the monkey.", outputB);
        }

        // there is no code specified in section 9

        /**
         * test section 10 code
         */
        [Test]
        public virtual void section10_Test()
        {
            Lexicon lexicon = Lexicon.DefaultLexicon; // default simplenlg lexicon
            NLGFactory nlgFactory = new NLGFactory(lexicon); // factory based on lexicon
            Realiser realiser = new Realiser(lexicon);

            NPPhraseSpec subject1 = nlgFactory.createNounPhrase("Mary");
            NPPhraseSpec subject2 = nlgFactory.createNounPhrase("your", "giraffe");

            // next line is not correct ~ should be nlgFactory.createCoordinatedPhrase ~ may be corrected in the API
            CoordinatedPhraseElement subj = nlgFactory.createCoordinatedPhrase(subject1, subject2);

            VPPhraseSpec verb = nlgFactory.createVerbPhrase("chase");

            SPhraseSpec p = nlgFactory.createClause();
            p.setSubject(subj);
            p.setVerb(verb);
            p.setObject("the monkey");

            string outputA = realiser.realiseSentence(p);
            Assert.AreEqual("Mary and your giraffe chase the monkey.", outputA);

            NPPhraseSpec object1 = nlgFactory.createNounPhrase("the monkey");
            NPPhraseSpec object2 = nlgFactory.createNounPhrase("George");

            // next line is not correct ~ should be nlgFactory.createCoordinatedPhrase ~ may be corrected in the API
            CoordinatedPhraseElement obj = nlgFactory.createCoordinatedPhrase(object1, object2);
            obj.addCoordinate("Martha");
            p.setObject(obj);

            string outputB = realiser.realiseSentence(p);
            Assert.AreEqual("Mary and your giraffe chase the monkey, George and Martha.", outputB);

            obj.setFeature(Feature.CONJUNCTION, "or");

            string outputC = realiser.realiseSentence(p);
            Assert.AreEqual("Mary and your giraffe chase the monkey, George or Martha.", outputC);
        }

        /**
         * test section 11 code
         */
        [Test]
        public virtual void section11_Test()
        {
            Lexicon lexicon = Lexicon.DefaultLexicon; // default simplenlg lexicon
            NLGFactory nlgFactory = new NLGFactory(lexicon); // factory based on lexicon

            Realiser realiser = new Realiser(lexicon);

            SPhraseSpec pA = nlgFactory.createClause("Mary", "chase", "the monkey");
            pA.addComplement("in the park");

            string outputA = realiser.realiseSentence(pA);
            Assert.AreEqual("Mary chases the monkey in the park.", outputA);

            // alternative build paradigm
            NPPhraseSpec place = nlgFactory.createNounPhrase("park");
            SPhraseSpec pB = nlgFactory.createClause("Mary", "chase", "the monkey");

            // next line is depreciated ~ may be corrected in the API
            place.setDeterminer("the");
            PPPhraseSpec pp = nlgFactory.createPrepositionPhrase();
            pp.addComplement(place);
            pp.setPreposition("in");

            pB.addComplement(pp);

            string outputB = realiser.realiseSentence(pB);
            Assert.AreEqual("Mary chases the monkey in the park.", outputB);

            place.addPreModifier("leafy");

            string outputC = realiser.realiseSentence(pB);
            Assert.AreEqual("Mary chases the monkey in the leafy park.", outputC);
        } // testSection11

        // section12 only has a code table as illustration

        /**
         * test section 13 code
         */
        [Test]
        public virtual void section13_Test()
        {
            Lexicon lexicon = Lexicon.DefaultLexicon; // default simplenlg lexicon
            NLGFactory nlgFactory = new NLGFactory(lexicon); // factory based on lexicon

            Realiser realiser = new Realiser(lexicon);

            SPhraseSpec s1 = nlgFactory.createClause("my cat", "like", "fish");
            SPhraseSpec s2 = nlgFactory.createClause("my dog", "like", "big bones");
            SPhraseSpec s3 = nlgFactory.createClause("my horse", "like", "grass");

            CoordinatedPhraseElement c = nlgFactory.createCoordinatedPhrase();
            c.addCoordinate(s1);
            c.addCoordinate(s2);
            c.addCoordinate(s3);

            string outputA = realiser.realiseSentence(c);
            Assert.AreEqual("My cat likes fish, my dog likes big bones and my horse likes grass.", outputA);

            SPhraseSpec p = nlgFactory.createClause("I", "be", "happy");
            SPhraseSpec q = nlgFactory.createClause("I", "eat", "fish");
            q.setFeature(Feature.COMPLEMENTISER, "because");
            q.setFeature(Feature.TENSE, Tense.PAST);
            p.addComplement(q);

            string outputB = realiser.realiseSentence(p);
            Assert.AreEqual("I am happy because I ate fish.", outputB);
        }

        /**
         * test section 14 code
         */
        [Test]
        public virtual void section14_Test()
        {
            Lexicon lexicon = Lexicon.DefaultLexicon; // default simplenlg lexicon
            NLGFactory nlgFactory = new NLGFactory(lexicon); // factory based on lexicon

            Realiser realiser = new Realiser(lexicon);

            SPhraseSpec p1 = nlgFactory.createClause("Mary", "chase", "the monkey");
            SPhraseSpec p2 = nlgFactory.createClause("The monkey", "fight back");
            SPhraseSpec p3 = nlgFactory.createClause("Mary", "be", "nervous");

            DocumentElement s1 = nlgFactory.createSentence(p1);
            DocumentElement s2 = nlgFactory.createSentence(p2);
            DocumentElement s3 = nlgFactory.createSentence(p3);

            DocumentElement par1 = nlgFactory.createParagraph(new List<DocumentElement> {s1, s2, s3});

            string output14a = realiser.realise(par1).Realisation;
            Assert.AreEqual("Mary chases the monkey. The monkey fights back. Mary is nervous.\n\n", output14a);

            DocumentElement section = nlgFactory.createSection("The Trials and Tribulation of Mary and the Monkey");
            section.addComponent(par1);
            string output14b = realiser.realise(section).Realisation;
            Assert.AreEqual(
                "The Trials and Tribulation of Mary and the Monkey\nMary chases the monkey. The monkey fights back. Mary is nervous.\n\n",
                output14b);
        }
    }
}