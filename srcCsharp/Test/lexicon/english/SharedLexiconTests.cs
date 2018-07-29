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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleNLG.Main.features;
using SimpleNLG.Main.framework;
using SimpleNLG.Main.lexicon;

namespace SimpleNLG.Test.lexicon.english
{
    using Inflection = Inflection;
    using LexicalFeature = LexicalFeature;
    using LexicalCategory = LexicalCategory;
    using WordElement = WordElement;

    /**
	 * @author Dave Westwater, Data2Text Ltd
	 * 
	 */
    public class SharedLexiconTests
    {
        public virtual void doBasicTests(Lexicon lexicon)
        {
            // test getWords. Should be 2 "can" (of any cat), 1 noun tree, 0 adj
            // trees
            Assert.AreEqual(3, lexicon.getWords("can").Count);
            Assert.AreEqual(1,
                lexicon.getWords("can", new LexicalCategory(LexicalCategory.LexicalCategoryEnum.NOUN)).Count);
            Assert.AreEqual(0,
                lexicon.getWords("can", new LexicalCategory(LexicalCategory.LexicalCategoryEnum.ADJECTIVE)).Count);
            // below test removed as standard morph variants no longer recorded in
            // lexicon
            // WordElement early = lexicon.getWord("early",
            // LexicalCategory.ADJECTIVE);
            // Assert.assertEquals("earlier",
            // early.getFeatureAsString(Feature.COMPARATIVE));

            // test getWord. Comparative of ADJ "good" is "better", superlative is
            // "best", this is a qualitative and predicative adjective
            WordElement good =
                lexicon.getWord("good", new LexicalCategory(LexicalCategory.LexicalCategoryEnum.ADJECTIVE));
            Assert.AreEqual("better", good.getFeatureAsString(LexicalFeature.COMPARATIVE));
            Assert.AreEqual("best", good.getFeatureAsString(LexicalFeature.SUPERLATIVE));
            Assert.AreEqual(true, good.getFeatureAsBoolean(LexicalFeature.QUALITATIVE));
            Assert.AreEqual(true, good.getFeatureAsBoolean(LexicalFeature.PREDICATIVE));
            Assert.AreEqual(false, good.getFeatureAsBoolean(LexicalFeature.COLOUR));
            Assert.AreEqual(false, good.getFeatureAsBoolean(LexicalFeature.CLASSIFYING));


            // test getWord. There is only one "woman", and its plural is "women".
            // It is not an acronym, not proper, and countable
            WordElement woman = lexicon.getWord("woman");

            Assert.AreEqual("women", woman.getFeatureAsString(LexicalFeature.PLURAL));
            Assert.AreEqual(null, woman.getFeatureAsString(LexicalFeature.ACRONYM_OF));
            Assert.AreEqual(false, woman.getFeatureAsBoolean(LexicalFeature.PROPER));
            Assert.IsFalse(woman.hasInflectionalVariant(Inflection.UNCOUNT));

            // NB: This fails if the lexicon is XMLLexicon. No idea why.
            // Assert.assertEquals("irreg",
            // woman.getFeatureAsString(LexicalFeature.DEFAULT_INFL));

            // test getWord. Noun "sand" is non-count
            WordElement sand = lexicon.getWord("sand", new LexicalCategory(LexicalCategory.LexicalCategoryEnum.NOUN));
            Assert.AreEqual(true, sand.hasInflectionalVariant(Inflection.UNCOUNT));
            Assert.AreEqual(Inflection.UNCOUNT, sand.getDefaultInflectionalVariant());

            // test hasWord
            Assert.AreEqual(true, lexicon.hasWord("tree")); // "tree" exists
            Assert.AreEqual(false,
                lexicon.hasWord("tree",
                    new LexicalCategory(LexicalCategory.LexicalCategoryEnum.ADVERB))); // but not as an adverb


            // test getWordByID; quickly, also check that this is a verb_modifier
            WordElement quickly = lexicon.getWordByID("E0051632");
            Assert.AreEqual("quickly", quickly.BaseForm);
            Assert.AreEqual(new LexicalCategory(LexicalCategory.LexicalCategoryEnum.ADVERB), quickly.Category);
            Assert.AreEqual(true, quickly.getFeatureAsBoolean(LexicalFeature.VERB_MODIFIER));
            Assert.AreEqual(false, quickly.getFeatureAsBoolean(LexicalFeature.SENTENCE_MODIFIER));
            Assert.AreEqual(false, quickly.getFeatureAsBoolean(LexicalFeature.INTENSIFIER));


            // test getWordFromVariant, verb type (tran or intran, not ditran)
            WordElement eat = lexicon.getWordFromVariant("eating");
            Assert.AreEqual("eat", eat.BaseForm);
            Assert.AreEqual(new LexicalCategory(LexicalCategory.LexicalCategoryEnum.VERB), eat.Category);
            Assert.AreEqual(true, eat.getFeatureAsBoolean(LexicalFeature.INTRANSITIVE));
            Assert.AreEqual(true, eat.getFeatureAsBoolean(LexicalFeature.TRANSITIVE));
            Assert.AreEqual(false, eat.getFeatureAsBoolean(LexicalFeature.DITRANSITIVE));


            Assert.AreEqual("been",
                lexicon.getWordFromVariant("is", new LexicalCategory(LexicalCategory.LexicalCategoryEnum.VERB))
                    .getFeatureAsString(LexicalFeature.PAST_PARTICIPLE));
            // test BE is handled OK

            // test modal
            WordElement can = lexicon.getWord("can", new LexicalCategory(LexicalCategory.LexicalCategoryEnum.MODAL));
            Assert.AreEqual("could", can.getFeatureAsString(LexicalFeature.PAST));

            // test non-existent word
            Assert.AreEqual(0, lexicon.getWords("akjmchsgk").Count);

            // test lookup word method
            Assert.AreEqual(
                lexicon.lookupWord("say", new LexicalCategory(LexicalCategory.LexicalCategoryEnum.VERB)).BaseForm,
                "say");
            Assert.AreEqual(
                lexicon.lookupWord("said", new LexicalCategory(LexicalCategory.LexicalCategoryEnum.VERB)).BaseForm,
                "say");
            Assert.AreEqual(
                lexicon.lookupWord("E0054448", new LexicalCategory(LexicalCategory.LexicalCategoryEnum.VERB)).BaseForm,
                "say");
        }
    }
}