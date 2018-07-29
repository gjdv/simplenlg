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

	using CoordinatedPhraseElement = framework.CoordinatedPhraseElement;
	using NLGElement = framework.NLGElement;
	using NLGFactory = framework.NLGFactory;

    /**
     * This class represents an aggregation rule. All such rules need to implement
     * an {@link #apply(NLGElement, NLGElement)} which takes an arbitrary number of
     * {@link simplenlg.framework.NLGElement}s and perform some form of aggregation
     * on them, returning an <code>SPhraseSpec</code> as a result, or
     * <code>null</code> if the operation fails.
     * 
     * @author Albert Gatt, University of Malta and University of Aberdeen
     * 
     */
	public abstract class AggregationRule
	{

		protected internal NLGFactory factory;

	    /**
	     * Creates a new instance of AggregationRule
	     */
		public AggregationRule()
		{
			factory = new NLGFactory();
		}

	    /**
	     * Set / get the factory that the rule should use to create phrases.
	     * 
	     * @param factory
	     *            the factory
	     */
		public virtual NLGFactory Factory
		{
			set
			{
				factory = value;
			}
			get
			{
				return factory;
			}
		}
	    /**
	     * Performs aggregation on an arbitrary number of elements in a list. This
	     * method calls {{@link #apply(NLGElement, NLGElement)} on all pairs of
	     * elements in the list, recursively aggregating whenever it can.
	     * 
	     * @param phrases
	     *            the sentences
	     * @return a list containing the phrases, such that, for any two phrases s1
	     *         and s2, if {@link #apply(NLGElement, NLGElement)} succeeds on s1
	     *         and s2, the list contains the result; otherwise, the list
	     *         contains s1 and s2.
	     */
		public virtual IList<NLGElement> apply(IList<NLGElement> phrases)
		{
			IList<NLGElement> results = new List<NLGElement>();

			if (phrases.Count >= 2)
			{
				IList<NLGElement> removed = new List<NLGElement>();

				for (int i = 0; i < phrases.Count; i++)
				{
					NLGElement current = phrases[i];

					if (removed.Contains(current))
					{
						continue;
					}

					for (int j = i + 1; j < phrases.Count; j++)
					{
						NLGElement next = phrases[j];
						NLGElement aggregated = apply(current, next);

						if (aggregated != null)
						{
							current = aggregated;
							removed.Add(next);
						}
					}

					results.Add(current);
				}

			}
			else if (phrases.Count == 1)
			{
				results.Add(apply(phrases[0]));
			}

			return results;
		}

	    /**
         * Perform aggregation on a single phrase. This method only works on a
         * {@link simplenlg.framework.CoordinatedPhraseElement}, in which case it
         * calls {@link #apply(List)} on the children of the coordinated phrase,
         * returning a coordinated phrase whose children are the result.
         * 
         * @param phrase
         * @return aggregated result
         */
		public virtual NLGElement apply(NLGElement phrase)
		{
			NLGElement result = null;

			if (phrase is CoordinatedPhraseElement)
			{
				IList<NLGElement> children = ((CoordinatedPhraseElement) phrase).Children;
				IList<NLGElement> aggregated = apply(children);

				if (aggregated.Count == 1)
				{
					result = aggregated[0];

				}
				else
				{
					result = factory.createCoordinatedPhrase();

					foreach (NLGElement agg in aggregated)
					{
						((CoordinatedPhraseElement) result).addCoordinate(agg);
					}
				}
			}


			if (result != null)
			{
				foreach (string feature in phrase.AllFeatureNames)
				{
					result.setFeature(feature, phrase.getFeature(feature));
				}
			}

			return result;
		}

	    /**
	     * Performs aggregation on a pair of sentences. This is the only method that
	     * extensions of <code>AggregationRule</code> need to implement.
	     * 
	     * @param sentence1
	     *            the first sentence
	     * @param sentence2
	     *            the second sentence
	     * @return an aggregated sentence, if the method succeeds, <code>null</code>
	     *         otherwise
	     */
		public abstract NLGElement apply(NLGElement sentence1, NLGElement sentence2);

	}

}