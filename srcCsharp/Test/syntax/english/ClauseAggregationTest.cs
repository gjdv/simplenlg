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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleNLG.Main.aggregation;
using SimpleNLG.Main.features;
using SimpleNLG.Main.framework;
using SimpleNLG.Main.phrasespec;

namespace SimpleNLG.Test.syntax.english
{
    using BackwardConjunctionReductionRule = BackwardConjunctionReductionRule;
    using Aggregator = Aggregator;
    using ClauseCoordinationRule = ClauseCoordinationRule;
    using ForwardConjunctionReductionRule = ForwardConjunctionReductionRule;
    using Feature = Feature;
    using NLGElement = NLGElement;
    using SPhraseSpec = SPhraseSpec;

    /**
     * Some tests for aggregation.
     * 
     * @author Albert Gatt, University of Malta & University of Aberdeen
     * 
     */
    [TestClass]
    public class ClauseAggregationTest : SimpleNLG4Test
    {
        // set up a few more fixtures
        /** The s4. */
        internal SPhraseSpec s1, s2, s3, s4, s5, s6;
        internal Aggregator aggregator;
        internal ClauseCoordinationRule coord;
        internal ForwardConjunctionReductionRule fcr;
        internal BackwardConjunctionReductionRule bcr;


        public ClauseAggregationTest() : this(null)
        {
        }

        /**
         * Instantiates a new clause aggregation test.
         * 
         * @param name
         *            the name
         */
        public ClauseAggregationTest(string name) : base(name)
        {
            aggregator = new Aggregator();
            aggregator.initialise();
            coord = new ClauseCoordinationRule();
            fcr = new ForwardConjunctionReductionRule();
            bcr = new BackwardConjunctionReductionRule();
        }

        /*
         * (non-Javadoc)
         * 
         * @see simplenlg.test.SimplenlgTest#setUp()
         */
        [TestInitialize]
        public override void setUp()
        {
            base.setUp();

            // the woman kissed the man behind the curtain
            s1 = phraseFactory.createClause();
            s1.setSubject(woman);
            s1.VerbPhrase = phraseFactory.createVerbPhrase("kiss");
            s1.setObject(man);
            s1.addPostModifier(
                phraseFactory.createPrepositionPhrase("behind", phraseFactory.createNounPhrase("the", "curtain")));


            // the woman kicked the dog on the rock
            s2 = phraseFactory.createClause();
            s2.setSubject(phraseFactory.createNounPhrase("the", "woman")); //$NON-NLS-1$
            s2.VerbPhrase = phraseFactory.createVerbPhrase("kick"); //$NON-NLS-1$
            s2.setObject(phraseFactory.createNounPhrase("the", "dog"));
            s2.addPostModifier(onTheRock);

            // the woman kicked the dog behind the curtain
            s3 = phraseFactory.createClause();
            s3.setSubject(phraseFactory.createNounPhrase("the", "woman")); //$NON-NLS-1$
            s3.VerbPhrase = phraseFactory.createVerbPhrase("kick"); //$NON-NLS-1$
            s3.setObject(phraseFactory.createNounPhrase("the", "dog"));
            s3.addPostModifier(
                phraseFactory.createPrepositionPhrase("behind", phraseFactory.createNounPhrase("the", "curtain")));


            // the man kicked the dog behind the curtain
            s4 = phraseFactory.createClause();
            s4.setSubject(man); //$NON-NLS-1$
            s4.VerbPhrase = phraseFactory.createVerbPhrase("kick"); //$NON-NLS-1$
            s4.setObject(phraseFactory.createNounPhrase("the", "dog"));
            s4.addPostModifier(behindTheCurtain);

            // the girl kicked the dog behind the curtain
            s5 = phraseFactory.createClause();
            s5.setSubject(phraseFactory.createNounPhrase("the", "girl")); //$NON-NLS-1$
            s5.VerbPhrase = phraseFactory.createVerbPhrase("kick"); //$NON-NLS-1$
            s5.setObject(phraseFactory.createNounPhrase("the", "dog"));
            s5.addPostModifier(behindTheCurtain);

            // the woman kissed the dog behind the curtain
            s6 = phraseFactory.createClause();
            s6.setSubject(phraseFactory.createNounPhrase("the", "woman")); //$NON-NLS-1$
            s6.VerbPhrase = phraseFactory.createVerbPhrase("kiss"); //$NON-NLS-1$
            s6.setObject(phraseFactory.createNounPhrase("the", "dog"));
            s6.addPostModifier(
                phraseFactory.createPrepositionPhrase("behind", phraseFactory.createNounPhrase("the", "curtain")));
        }

        [TestCleanup]
        public override void tearDown()
        {
            base.tearDown();

            s1 = null;
            s2 = null;
            s3 = null;
            s4 = null;
            s5 = null;
            s6 = null;
            aggregator = null;
            coord = null;
            fcr = null;
            bcr = null;
        }

        /**
         * Test clause coordination with two sentences with same subject but
         * different postmodifiers: fails
         */
        [TestMethod]
        public virtual void testCoordinationSameSubjectFail()
        {
            IList<NLGElement> elements = new List<NLGElement> {s1, s2};
            IList<NLGElement> result = coord.apply(elements);
            Assert.AreEqual(2, result.Count);
        }

        /**
         * Test clause coordination with two sentences one of which is passive:
         * fails
         */
        [TestMethod]
        public virtual void testCoordinationPassiveFail()
        {
            s1.setFeature(Feature.PASSIVE, true);
            IList<NLGElement> elements = new List<NLGElement> {s1, s2};
            IList<NLGElement> result = coord.apply(elements);
            Assert.AreEqual(2, result.Count);
        }

        //	/**
        //	 * Test clause coordination with 2 sentences with same subject: succeeds
        //	 */
        //	@Test
        //	public void testCoordinationSameSubjectSuccess() {
        //		List<NLGElement> elements = Arrays.asList((NLGElement) this.s1,
        //				(NLGElement) this.s3);
        //		List<NLGElement> result = this.coord.apply(elements);
        //		Assert.IsTrue(result.size() == 1); // should only be one sentence
        //		NLGElement aggregated = result.get(0);
        //		Assert
        //				.assertEquals(
        //						"the woman kisses the man and kicks the dog behind the curtain", //$NON-NLS-1$
        //						this.realiser.realise(aggregated).getRealisation());
        //	}

        /**
         * Test clause coordination with 2 sentences with same VP: succeeds
         */
        [TestMethod]
        public virtual void testCoordinationSameVP()
        {
            IList<NLGElement> elements = new List<NLGElement> {s3, s4};
            IList<NLGElement> result = coord.apply(elements);
            Assert.IsTrue(result.Count == 1); // should only be one sentence
            NLGElement aggregated = result[0];
            Assert.AreEqual("the woman and the man kick the dog behind the curtain",
                realiser.realise(aggregated).Realisation); //$NON-NLS-1$
        }

        /**
         * Coordination of sentences with front modifiers: should preserve the mods
         */
        [TestMethod]
        public virtual void testCoordinationWithModifiers()
        {
            // now add a couple of front modifiers
            s3.addFrontModifier(phraseFactory.createAdverbPhrase("however"));
            s4.addFrontModifier(phraseFactory.createAdverbPhrase("however"));
            IList<NLGElement> elements = new List<NLGElement> {s3, s4};
            IList<NLGElement> result = coord.apply(elements);
            Assert.IsTrue(result.Count == 1); // should only be one sentence
            NLGElement aggregated = result[0];
            Assert.AreEqual("however the woman and the man kick the dog behind the curtain",
                realiser.realise(aggregated).Realisation); //$NON-NLS-1$
        }

        /**
         * Test coordination of 3 sentences with the same VP
         */
        [TestMethod]
        public virtual void testCoordinationSameVP2()
        {
            IList<NLGElement> elements = new List<NLGElement> {s3, s4, s5};
            IList<NLGElement> result = coord.apply(elements);
            Assert.IsTrue(result.Count == 1); // should only be one sentence
            NLGElement aggregated = result[0];
            Assert.AreEqual("the woman and the man and the girl kick the dog behind the curtain",
                realiser.realise(aggregated).Realisation); //$NON-NLS-1$
        }

        /**
         * Forward conjunction reduction test
         */
        [TestMethod]
        public virtual void testForwardConjReduction()
        {
            NLGElement aggregated = fcr.apply(s2, s3);
            Assert.AreEqual("the woman kicks the dog on the rock and kicks the dog behind the curtain",
                realiser.realise(aggregated).Realisation); //$NON-NLS-1$
        }

        /**
         * Backward conjunction reduction test
         */
        [TestMethod]
        public virtual void testBackwardConjunctionReduction()
        {
            NLGElement aggregated = bcr.apply(s3, s6);
            Assert.AreEqual("the woman kicks and the woman kisses the dog behind the curtain",
                realiser.realise(aggregated).Realisation);
        }

        /**
         * Test multiple aggregation procedures in a single aggregator. 
         */
        //	[TestMethod]
        //	public void testForwardBackwardConjunctionReduction() {
        //		this.aggregator.addRule(this.fcr);
        //		this.aggregator.addRule(this.bcr);
        //		realiser.setDebugMode(true);
        //		List<NLGElement> result = this.aggregator.realise(Arrays.asList((NLGElement) this.s2, (NLGElement) this.s3));
        //		Assert.IsTrue(result.size() == 1); // should only be one sentence
        //		NLGElement aggregated = result.get(0);
        //		NLGElement aggregated = this.phraseFactory.createdCoordinatedPhrase(this.s2, this.s3);
        //		Assert
        //				.assertEquals(
        //						"the woman kicks the dog on the rock and behind the curtain", //$NON-NLS-1$
        //						this.realiser.realise(aggregated).getRealisation());
        //	}
    }
}