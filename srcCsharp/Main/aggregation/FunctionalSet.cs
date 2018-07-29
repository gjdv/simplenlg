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
using System.Linq;
using System.Text;

namespace SimpleNLG.Main.aggregation
{

	using DiscourseFunction = features.DiscourseFunction;
	using Feature = features.Feature;
	using InternalFeature = features.InternalFeature;
	using ElementCategory = framework.ElementCategory;
	using ListElement = framework.ListElement;
	using NLGElement = framework.NLGElement;

	public class FunctionalSet
	{

		private IList<NLGElement> components;
		private DiscourseFunction function;
		private ElementCategory category;
		private Periphery periphery;

		public static FunctionalSet newInstance(DiscourseFunction func, ElementCategory category, Periphery periphery, params NLGElement[] components)
		{

			FunctionalSet pair = null;

			if (components.Length >= 2)
			{
				pair = new FunctionalSet(func, category, periphery, components);
			}

			return pair;

		}

		internal FunctionalSet(DiscourseFunction func, ElementCategory category, Periphery periphery, params NLGElement[] components)
		{
			function = func;
			this.category = category;
			this.periphery = periphery;
			this.components = components.ToList();
		}

		public virtual bool formIdentical()
		{
			bool ident = true;
			NLGElement firstElement = components[0];

			for (int i = 1; i < components.Count && ident; i++)
			{
				ident = firstElement.Equals(components[i]);
			}

			return ident;
		}

		public virtual bool lemmaIdentical()
		{
			return false;
		}

		public virtual void elideLeftMost()
		{
			for (int i = 0; i < components.Count - 1; i++)
			{
				recursiveElide(components[i]);
			}
		}

		public virtual void elideRightMost()
		{
			for (int i = components.Count - 1; i > 0; i--)
			{
				recursiveElide(components[i]);

			}
		}

		private void recursiveElide(NLGElement component)
		{
			if (component is ListElement)
			{
				foreach (NLGElement subcomponent in component.getFeatureAsElementList(InternalFeature.COMPONENTS))
				{
					recursiveElide(subcomponent);
				}
			}
			else
			{
				component.setFeature(Feature.ELIDED, true);
			}
		}

		public virtual DiscourseFunction Function
		{
			get
			{
				return function;
			}
		}

		public virtual ElementCategory Category
		{
			get
			{
				return category;
			}
		}

		public virtual Periphery Periphery
		{
			get
			{
				return periphery;
			}
		}

		public virtual IList<NLGElement> Components
		{
			get
			{
				return components;
			}
		}

		public override string ToString()
		{
			StringBuilder buffer = new StringBuilder();

			foreach (NLGElement elem in components)
			{
				buffer.Append("ELEMENT: ").Append(elem.ToString()).Append("\n");
			}

			return buffer.ToString();
		}

	}

}