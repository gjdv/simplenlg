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

	using DiscourseFunction = features.DiscourseFunction;
	using InternalFeature = features.InternalFeature;
	using ElementCategory = framework.ElementCategory;
	using LexicalCategory = framework.LexicalCategory;
	using ListElement = framework.ListElement;
	using NLGElement = framework.NLGElement;
	using PhraseCategory = framework.PhraseCategory;

	public class AggregationHelper
	{

		public static IList<DiscourseFunction> FUNCTIONS = new List<DiscourseFunction>{DiscourseFunction.SUBJECT, DiscourseFunction.HEAD, DiscourseFunction.COMPLEMENT, DiscourseFunction.PRE_MODIFIER, DiscourseFunction.POST_MODIFIER, DiscourseFunction.VERB_PHRASE};

		public static IList<DiscourseFunction> RECURSIVE = new List<DiscourseFunction>{DiscourseFunction.VERB_PHRASE};

		public static IList<FunctionalSet> collectFunctionalPairs(NLGElement phrase1, NLGElement phrase2)
		{
			IList<NLGElement> children1 = getAllChildren(phrase1);
			IList<NLGElement> children2 = getAllChildren(phrase2);
			IList<FunctionalSet> pairs = new List<FunctionalSet>();

			if (children1.Count == children2.Count)
			{
				Periphery periph = Periphery.LEFT;

				for (int i = 0; i < children1.Count; i++)
				{
					NLGElement child1 = children1[i];
					NLGElement child2 = children2[i];
					ElementCategory cat1 = child1.Category;
					ElementCategory cat2 = child2.Category;
					DiscourseFunction func1 = (DiscourseFunction) child1.getFeature(InternalFeature.DISCOURSE_FUNCTION);
					DiscourseFunction func2 = (DiscourseFunction) child2.getFeature(InternalFeature.DISCOURSE_FUNCTION);

					if (cat1 == cat2 && func1 == func2)
					{
						pairs.Add(FunctionalSet.newInstance(func1, cat1, periph, child1, child2));

						if (cat1 == LexicalCategory.LexicalCategoryEnum.VERB)
						{
							periph = Periphery.RIGHT;
						}

					}
					else
					{
						pairs.Clear();
						break;
					}
				}
			}

			return pairs;
		}

		private static IList<NLGElement> getAllChildren(NLGElement element)
		{
			IList<NLGElement> children = new List<NLGElement>();
			IList<NLGElement> components = element is ListElement ? element.getFeatureAsElementList(InternalFeature.COMPONENTS) : element.Children;

			foreach (NLGElement child in components)
			{
				children.Add(child);

				if (child.Category == PhraseCategory.PhraseCategoryEnum.VERB_PHRASE || (DiscourseFunction)child.getFeature(InternalFeature.DISCOURSE_FUNCTION) == DiscourseFunction.VERB_PHRASE)
				{
					((List<NLGElement>)children).AddRange(getAllChildren(child));
				}
			}

			return children;
		}

	}

}