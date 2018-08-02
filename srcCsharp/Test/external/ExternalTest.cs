/*
 * Ported to C# by Gert-Jan de Vries
 */
 
using System.Collections.Generic;
using NUnit.Framework;
using SimpleNLG.Main;
using SimpleNLG.Main.aggregation;
using SimpleNLG.Main.features;
using SimpleNLG.Main.framework;
using SimpleNLG.Main.lexicon;
using SimpleNLG.Main.phrasespec;
using SimpleNLG.Main.realiser.english;

namespace SimpleNLG.Test.external
{
    [TestFixture]
    public class ExternalTest
    {
        private Lexicon lexicon = null;
        private NLGFactory phraseFactory = null;
        private Realiser realiser = null;

        [SetUp]
        public virtual void setUp()
        {
            lexicon = Lexicon.DefaultLexicon;
            phraseFactory = new NLGFactory(lexicon);
            realiser = new Realiser(lexicon);
        }

        [Test]
        public virtual void forcherTest()
        {
            phraseFactory.Lexicon = lexicon;
            PhraseElement s1 = phraseFactory.createClause(null, "associate", "Marie");
            s1.setFeature(Feature.PASSIVE, true);
            PhraseElement pp1 = phraseFactory.createPrepositionPhrase("with"); //$NON-NLS-1$
            pp1.addComplement("Peter"); //$NON-NLS-1$
            pp1.addComplement("Paul"); //$NON-NLS-1$
            s1.addPostModifier(pp1);

            Assert.AreEqual("Marie is associated with Peter and Paul", realiser.realise(s1).Realisation); //$NON-NLS-1$
            SPhraseSpec s2 = phraseFactory.createClause();
            s2.setSubject(phraseFactory.createNounPhrase("Peter")); //$NON-NLS-1$
            s2.setVerb("have"); //$NON-NLS-1$
            s2.setObject("something to do"); //$NON-NLS-1$
            s2.addPostModifier(phraseFactory.createPrepositionPhrase("with", "Paul")); //$NON-NLS-1$ //$NON-NLS-2$


            Assert.AreEqual("Peter has something to do with Paul", realiser.realise(s2).Realisation); //$NON-NLS-1$
        }

        [Test]
        public virtual void luTest()
        {
            phraseFactory.Lexicon = lexicon;
            PhraseElement
                s1 = phraseFactory.createClause("we", "consider", "John"); //$NON-NLS-1$ - $NON-NLS-1$ - $NON-NLS-1$
            s1.addPostModifier("a friend"); //$NON-NLS-1$

            Assert.AreEqual("we consider John a friend", realiser.realise(s1).Realisation); //$NON-NLS-1$
        }

        [Test]
        public virtual void dwightTest()
        {
            phraseFactory.Lexicon = lexicon;

            NPPhraseSpec noun4 = phraseFactory.createNounPhrase("FGFR3 gene in every cell"); //$NON-NLS-1$

            noun4.setSpecifier("the");

            PhraseElement prep1 = phraseFactory.createPrepositionPhrase("of", noun4); //$NON-NLS-1$

            PhraseElement noun1 = phraseFactory.createNounPhrase("the", "patient's mother"); //$NON-NLS-1$ //$NON-NLS-2$

            PhraseElement noun2 = phraseFactory.createNounPhrase("the", "patient's father"); //$NON-NLS-1$ //$NON-NLS-2$

            PhraseElement noun3 = phraseFactory.createNounPhrase("changed copy"); //$NON-NLS-1$
            noun3.addPreModifier("one"); //$NON-NLS-1$
            noun3.addComplement(prep1);

            CoordinatedPhraseElement coordNoun1 = new CoordinatedPhraseElement(noun1, noun2);
            coordNoun1.Conjunction = "or"; //$NON-NLS-1$

            PhraseElement verbPhrase1 = phraseFactory.createVerbPhrase("have"); //$NON-NLS-1$
            verbPhrase1.setFeature(Feature.TENSE, Tense.PRESENT);

            PhraseElement sentence1 = phraseFactory.createClause(coordNoun1, verbPhrase1, noun3);

            realiser.DebugMode = true;
            Assert.AreEqual(
                "the patient's mother or the patient's father has one changed copy of the FGFR3 gene in every cell",
                realiser.realise(sentence1).Realisation); //$NON-NLS-1$


            noun3 = phraseFactory.createNounPhrase("a", "gene test"); //$NON-NLS-1$ //$NON-NLS-2$
            noun2 = phraseFactory.createNounPhrase("an", "LDL test"); //$NON-NLS-1$ //$NON-NLS-2$
            noun1 = phraseFactory.createNounPhrase("the", "clinic"); //$NON-NLS-1$ //$NON-NLS-2$
            verbPhrase1 = phraseFactory.createVerbPhrase("perform"); //$NON-NLS-1$

            CoordinatedPhraseElement coord1 = new CoordinatedPhraseElement(noun2, noun3);
            sentence1 = phraseFactory.createClause(noun1, verbPhrase1, coord1);
            sentence1.setFeature(Feature.TENSE, Tense.PAST);

            Assert.AreEqual("the clinic performed an LDL test and a gene test",
                realiser.realise(sentence1).Realisation); //$NON-NLS-1$
        }

        [Test]
        public virtual void novelliTest()
        {
            PhraseElement
                p = phraseFactory.createClause("Mary", "chase", "George"); //$NON-NLS-1$ //$NON-NLS-2$ //$NON-NLS-3$

            PhraseElement pp = phraseFactory.createPrepositionPhrase("in", "the park"); //$NON-NLS-1$ //$NON-NLS-2$
            p.addPostModifier(pp);

            Assert.AreEqual("Mary chases George in the park", realiser.realise(p).Realisation); //$NON-NLS-1$


            SPhraseSpec
                run = phraseFactory.createClause("you", "go", "running"); //$NON-NLS-1$ //$NON-NLS-2$ //$NON-NLS-3$
            run.setFeature(Feature.MODAL, "should"); //$NON-NLS-1$
            run.addPreModifier("really"); //$NON-NLS-1$
            SPhraseSpec think = phraseFactory.createClause("I", "think"); //$NON-NLS-1$ //$NON-NLS-2$
            think.setObject(run);
            run.setFeature(Feature.SUPRESSED_COMPLEMENTISER, true);

            string text = realiser.realise(think).Realisation;
            Assert.AreEqual("I think you should really go running", text); //$NON-NLS-1$
        }

        [Test]
        public virtual void piotrekTest()
        {
            phraseFactory.Lexicon = lexicon;
            PhraseElement
                sent = phraseFactory.createClause("I", "shoot", "the duck"); //$NON-NLS-1$ //$NON-NLS-2$ //$NON-NLS-3$
            sent.setFeature(Feature.TENSE, Tense.PAST);

            PhraseElement
                loc = phraseFactory.createPrepositionPhrase("at", "the Shooting Range"); //$NON-NLS-1$ //$NON-NLS-2$
            sent.addPostModifier(loc);
            sent.setFeature(Feature.CUE_PHRASE, "then"); //$NON-NLS-1$

            Assert.AreEqual("then I shot the duck at the Shooting Range",
                realiser.realise(sent).Realisation); //$NON-NLS-1$
        }

        [Test]
        public virtual void prescottTest()
        {
            phraseFactory.Lexicon = lexicon;
            PhraseElement
                embedded = phraseFactory.createClause("Jill", "prod", "Spot"); //$NON-NLS-1$ //$NON-NLS-2$ //$NON-NLS-3$
            PhraseElement sent = phraseFactory.createClause("Jack", "see", embedded); //$NON-NLS-1$ //$NON-NLS-2$
            embedded.setFeature(Feature.SUPRESSED_COMPLEMENTISER, true);
            embedded.setFeature(Feature.FORM, Form.BARE_INFINITIVE);

            Assert.AreEqual("Jack sees Jill prod Spot", realiser.realise(sent).Realisation); //$NON-NLS-1$
        }

        [Test]
        public virtual void wissnerTest()
        {
            PhraseElement p = phraseFactory.createClause("a wolf", "eat"); //$NON-NLS-1$ //$NON-NLS-2$
            p.setFeature(Feature.INTERROGATIVE_TYPE, InterrogativeType.WHAT_OBJECT);
            Assert.AreEqual("what does a wolf eat", realiser.realise(p).Realisation); //$NON-NLS-1$
        }

        [Test]
        public virtual void phanTest()
        {
            PhraseElement subjectElement = phraseFactory.createNounPhrase("I");
            PhraseElement verbElement = phraseFactory.createVerbPhrase("run");

            PhraseElement prepPhrase = phraseFactory.createPrepositionPhrase("from");
            prepPhrase.addComplement("home");

            verbElement.addComplement(prepPhrase);
            SPhraseSpec newSentence = phraseFactory.createClause();
            newSentence.setSubject(subjectElement);
            newSentence.VerbPhrase = verbElement;

            Assert.AreEqual("I run from home", realiser.realise(newSentence).Realisation); //$NON-NLS-1$
        }

        [Test]
        public virtual void kerberTest()
        {
            SPhraseSpec sp = phraseFactory.createClause("he", "need");
            SPhraseSpec secondSp = phraseFactory.createClause();
            secondSp.setVerb("build");
            secondSp.setObject("a house");
            secondSp.setFeature(Feature.FORM, Form.INFINITIVE);
            sp.setObject("stone");
            sp.addComplement(secondSp);
            Assert.AreEqual("he needs stone to build a house", realiser.realise(sp).Realisation);

            SPhraseSpec sp2 = phraseFactory.createClause("he", "give");
            sp2.setIndirectObject("I");
            sp2.setObject("the book");
            Assert.AreEqual("he gives me the book", realiser.realise(sp2).Realisation);
        }

        [Test]
        public virtual void stephensonTest()
        {
            SPhraseSpec qs2 = phraseFactory.createClause();
            qs2 = phraseFactory.createClause();
            qs2.setSubject("moles of Gold");
            qs2.setVerb("are");
            qs2.setFeature(Feature.NUMBER, NumberAgreement.PLURAL);
            qs2.setFeature(Feature.PASSIVE, false);
            qs2.setFeature(Feature.INTERROGATIVE_TYPE, InterrogativeType.HOW_MANY);
            qs2.setObject("in a 2.50 g sample of pure Gold");
            DocumentElement sentence = phraseFactory.createSentence(qs2);
            Assert.AreEqual("How many moles of Gold are in a 2.50 g sample of pure Gold?",
                realiser.realise(sentence).Realisation);
        }

        [Test]
        public virtual void pierreTest()
        {
            SPhraseSpec p = phraseFactory.createClause("Mary", "chase", "George");
            p.setFeature(Feature.INTERROGATIVE_TYPE, InterrogativeType.WHAT_OBJECT);
            Assert.AreEqual("What does Mary chase?", realiser.realiseSentence(p));

            p = phraseFactory.createClause("Mary", "chase", "George");
            p.setFeature(Feature.INTERROGATIVE_TYPE, InterrogativeType.YES_NO);
            Assert.AreEqual("Does Mary chase George?", realiser.realiseSentence(p));

            p = phraseFactory.createClause("Mary", "chase", "George");
            p.setFeature(Feature.INTERROGATIVE_TYPE, InterrogativeType.WHERE);
            Assert.AreEqual("Where does Mary chase George?", realiser.realiseSentence(p));

            p = phraseFactory.createClause("Mary", "chase", "George");
            p.setFeature(Feature.INTERROGATIVE_TYPE, InterrogativeType.WHY);
            Assert.AreEqual("Why does Mary chase George?", realiser.realiseSentence(p));

            p = phraseFactory.createClause("Mary", "chase", "George");
            p.setFeature(Feature.INTERROGATIVE_TYPE, InterrogativeType.HOW);
            Assert.AreEqual("How does Mary chase George?", realiser.realiseSentence(p));
        }

        [Test]
        public virtual void data2TextTest()
        {
            SPhraseSpec p = phraseFactory.createClause("the dog", "weigh", "12");
            Assert.AreEqual("The dog weighes 12.", realiser.realiseSentence(p));


            NLGElement dataDropout2 = phraseFactory.createNLGElement("data dropouts");
            dataDropout2.Plural = true;
            SPhraseSpec sentence2 = phraseFactory.createClause();
            sentence2.setSubject(phraseFactory.createStringElement("there"));
            sentence2.setVerb("be");
            sentence2.setObject(dataDropout2);
            Assert.AreEqual("There are data dropouts.", realiser.realiseSentence(sentence2));


            SPhraseSpec weather1 = phraseFactory.createClause("SE 10-15", "veer", "S 15-20");
            weather1.setFeature(Feature.FORM, Form.GERUND);
            Assert.AreEqual("SE 10-15 veering S 15-20.", realiser.realiseSentence(weather1));


            SPhraseSpec weather2 = phraseFactory.createClause("cloudy and misty", "be", "XXX");
            weather2.VerbPhrase.setFeature(Feature.ELIDED, true);
            Assert.AreEqual("Cloudy and misty.", realiser.realiseSentence(weather2));


            SPhraseSpec weather3 = phraseFactory.createClause("S 15-20", "increase", "20-25");
            weather3.setFeature(Feature.FORM, Form.GERUND);
            weather3.getSubject().setFeature(Feature.ELIDED, true);
            Assert.AreEqual("Increasing 20-25.", realiser.realiseSentence(weather3));


            SPhraseSpec weather4 = phraseFactory.createClause("S 20-25", "back", "SSE");
            weather4.setFeature(Feature.FORM, Form.GERUND);
            weather4.getSubject().setFeature(Feature.ELIDED, true);

            CoordinatedPhraseElement coord = new CoordinatedPhraseElement();
            coord.addCoordinate(weather1);
            coord.addCoordinate(weather3);
            coord.addCoordinate(weather4);
            coord.Conjunction = "then";
            Assert.AreEqual("SE 10-15 veering S 15-20, increasing 20-25 then backing SSE.",
                realiser.realiseSentence(coord));


            SPhraseSpec weather5 = phraseFactory.createClause("rain", null, "likely");
            Assert.AreEqual("Rain likely.", realiser.realiseSentence(weather5));
        }

        [Test]
        public virtual void rafaelTest()
        {
            IList<NLGElement> ss = new List<NLGElement>();
            ClauseCoordinationRule coord = new ClauseCoordinationRule();
            coord.Factory = phraseFactory;

            ss.Add(agreePhrase("John Lennon")); // john lennon agreed with it
            ss.Add(disagreePhrase("Geri Halliwell")); // Geri Halliwell disagreed with it
            ss.Add(commentPhrase("Melanie B")); // Mealnie B commented on it
            ss.Add(agreePhrase("you")); // you agreed with it
            ss.Add(commentPhrase("Emma Bunton")); //Emma Bunton commented on it

            IList<NLGElement> results = coord.apply(ss);
            IList<string> ret = realizeAll(results);
            Assert.AreEqual(
                "[John Lennon and you agreed with it, Geri Halliwell disagreed with it, Melanie B and Emma Bunton commented on it]",
                ret.ToStringNLG());
        }

        private NLGElement commentPhrase(string name)
        {
            // used by testRafael
            SPhraseSpec s = phraseFactory.createClause();
            s.setSubject(phraseFactory.createNounPhrase(name));
            s.VerbPhrase = phraseFactory.createVerbPhrase("comment on");
            s.setObject("it");
            s.setFeature(Feature.TENSE, Tense.PAST);
            return s;
        }

        private NLGElement agreePhrase(string name)
        {
            // used by testRafael
            SPhraseSpec s = phraseFactory.createClause();
            s.setSubject(phraseFactory.createNounPhrase(name));
            s.VerbPhrase = phraseFactory.createVerbPhrase("agree with");
            s.setObject("it");
            s.setFeature(Feature.TENSE, Tense.PAST);
            return s;
        }

        private NLGElement disagreePhrase(string name)
        {
            // used by testRafael
            SPhraseSpec s = phraseFactory.createClause();
            s.setSubject(phraseFactory.createNounPhrase(name));
            s.VerbPhrase = phraseFactory.createVerbPhrase("disagree with");
            s.setObject("it");
            s.setFeature(Feature.TENSE, Tense.PAST);
            return s;
        }

        private List<string> realizeAll(IList<NLGElement> results)
        {
            // used by testRafael
            List<string> ret = new List<string>();
            foreach (NLGElement e in results)
            {
                string r = realiser.realise(e).Realisation;
                ret.Add(r);
            }

            return ret;
        }

        [Test]
        public virtual void wikipediaTest()
        {
            NPPhraseSpec subject = phraseFactory.createNounPhrase("the", "woman");
            subject.Plural = true;
            SPhraseSpec sentence = phraseFactory.createClause(subject, "smoke");
            sentence.setFeature(Feature.NEGATED, true);
            Assert.AreEqual("The women do not smoke.", realiser.realiseSentence(sentence));


            SPhraseSpec s1 = phraseFactory.createClause("the man", "be", "hungry");
            SPhraseSpec s2 = phraseFactory.createClause("the man", "buy", "an apple");
            NLGElement result = (new ClauseCoordinationRule()).apply(s1, s2);
            Assert.AreEqual("The man is hungry and buys an apple.", realiser.realiseSentence(result));
        }

        [Test]
        public virtual void leanTest()
        {
            SPhraseSpec sentence = phraseFactory.createClause();
            sentence.setVerb("be");
            sentence.setObject("a ball");
            sentence.setFeature(Feature.INTERROGATIVE_TYPE, InterrogativeType.WHAT_SUBJECT);
            Assert.AreEqual("What is a ball?", realiser.realiseSentence(sentence));

            sentence = phraseFactory.createClause();
            sentence.setVerb("be");
            NPPhraseSpec @object = phraseFactory.createNounPhrase("example");
            @object.Plural = true;
            @object.addModifier("of jobs");
            sentence.setFeature(Feature.INTERROGATIVE_TYPE, InterrogativeType.WHAT_SUBJECT);
            sentence.setObject(@object);
            Assert.AreEqual("What are examples of jobs?", realiser.realiseSentence(sentence));

            SPhraseSpec p = phraseFactory.createClause();
            NPPhraseSpec sub1 = phraseFactory.createNounPhrase("Mary");

            sub1.setFeature(LexicalFeature.GENDER, Gender.FEMININE);
            sub1.setFeature(Feature.PRONOMINAL, true);
            sub1.setFeature(Feature.PERSON, Person.FIRST);
            p.setSubject(sub1);
            p.setVerb("chase");
            p.setObject("the monkey");


            string output2 = realiser.realiseSentence(p); // Realiser created earlier.
            Assert.AreEqual("I chase the monkey.", output2);

            SPhraseSpec test = phraseFactory.createClause();
            NPPhraseSpec subject = phraseFactory.createNounPhrase("Mary");

            subject.setFeature(Feature.PRONOMINAL, true);
            subject.setFeature(Feature.PERSON, Person.SECOND);
            test.setSubject(subject);
            test.setVerb("cry");

            test.setFeature(Feature.INTERROGATIVE_TYPE, InterrogativeType.WHY);
            test.setFeature(Feature.TENSE, Tense.PRESENT);
            Assert.AreEqual("Why do you cry?", realiser.realiseSentence(test));

            test = phraseFactory.createClause();
            subject = phraseFactory.createNounPhrase("Mary");

            subject.setFeature(Feature.PRONOMINAL, true);
            subject.setFeature(Feature.PERSON, Person.SECOND);
            test.setSubject(subject);
            test.setVerb("be");
            test.setObject("crying");

            test.setFeature(Feature.INTERROGATIVE_TYPE, InterrogativeType.WHY);
            test.setFeature(Feature.TENSE, Tense.PRESENT);
            Assert.AreEqual("Why are you crying?", realiser.realiseSentence(test));
        }

        [Test]
        public virtual void kalijurandTest()
        {
            string lemma = "walk";


            WordElement word = lexicon.lookupWord(lemma, new LexicalCategory(LexicalCategory.LexicalCategoryEnum.VERB));
            InflectedWordElement inflectedWord = new InflectedWordElement(word);

            inflectedWord.setFeature(Feature.FORM, Form.PAST_PARTICIPLE);
            string form = realiser.realise(inflectedWord).Realisation;
            Assert.AreEqual("walked", form);


            inflectedWord = new InflectedWordElement(word);

            inflectedWord.setFeature(Feature.PERSON, Person.THIRD);
            form = realiser.realise(inflectedWord).Realisation;
            Assert.AreEqual("walks", form);
        }

        [Test]
        public virtual void layTest()
        {
            string lemma = "slap";

            WordElement word = lexicon.lookupWord(lemma, new LexicalCategory(LexicalCategory.LexicalCategoryEnum.VERB));
            InflectedWordElement inflectedWord = new InflectedWordElement(word);
            inflectedWord.setFeature(Feature.FORM, Form.PRESENT_PARTICIPLE);
            string form = realiser.realise(inflectedWord).Realisation;
            Assert.AreEqual("slapping", form);

            VPPhraseSpec v = phraseFactory.createVerbPhrase("slap");
            v.setFeature(Feature.PROGRESSIVE, true);
            string progressive = realiser.realise(v).Realisation;
            Assert.AreEqual("is slapping", progressive);
        }
    }
}