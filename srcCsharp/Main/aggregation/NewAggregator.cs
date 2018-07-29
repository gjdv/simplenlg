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

namespace SimpleNLG.Main.aggregation
{

	using NLGElement = framework.NLGElement;
	using NLGFactory = framework.NLGFactory;
	using NLGModule = framework.NLGModule;
	using PhraseCategory = framework.PhraseCategory;
	using PhraseElement = framework.PhraseElement;
	using SyntaxProcessor = syntax.english.SyntaxProcessor;

	public class NewAggregator : NLGModule
	{
		private SyntaxProcessor _syntax;
		private NLGFactory _factory;

		public NewAggregator()
		{

		}

		public override void initialise()
		{
			_syntax = new SyntaxProcessor();
			_factory = new NLGFactory();
		}

		public override IList<NLGElement> realise(IList<NLGElement> elements)
		{
    		// TODO Auto-generated method stub
			return null;
		}

		public override NLGElement realise(NLGElement element)
		{
	    	// TODO Auto-generated method stub
			return null;
		}

		public virtual NLGElement realise(NLGElement phrase1, NLGElement phrase2)
		{
			NLGElement result = null;

			if (phrase1 is PhraseElement && phrase2 is PhraseElement && phrase1.Category == PhraseCategory.PhraseCategoryEnum.CLAUSE && phrase2.Category == PhraseCategory.PhraseCategoryEnum.CLAUSE)
			{

				IList<FunctionalSet> funcSets = AggregationHelper.collectFunctionalPairs(_syntax.realise(phrase1),_syntax.realise(phrase2));

				applyForwardConjunctionReduction(funcSets);
				applyBackwardConjunctionReduction(funcSets);
				result = _factory.createCoordinatedPhrase(phrase1, phrase2);
			}

			return result;
		}

	    // private void applyGapping(List<FunctionalSet> funcPairs) {

		private void applyForwardConjunctionReduction(IList<FunctionalSet> funcSets)
		{

			foreach (FunctionalSet pair in funcSets)
			{
				if (pair.Periphery == Periphery.LEFT && pair.formIdentical())
				{
					pair.elideRightMost();
				}
			}

		}

		private void applyBackwardConjunctionReduction(IList<FunctionalSet> funcSets)
		{
			foreach (FunctionalSet pair in funcSets)
			{
				if (pair.Periphery == Periphery.RIGHT && pair.formIdentical())
				{
					pair.elideLeftMost();
				}
			}
		}

	}

}