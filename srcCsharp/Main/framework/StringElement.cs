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
using System.Text;

namespace SimpleNLG.Main.framework
{

	using Feature = features.Feature;


    /**
     * <p>
     * This class defines an element for representing canned text within the
     * SimpleNLG library. Once assigned a value, the string element should not be
     * changed by any other processors.
     * </p>
     * 
     * 
     * @author D. Westwater, University of Aberdeen.
     * @version 4.0
     * 
     */
	public class StringElement : NLGElement
	{
	    /**
	     * Constructs a new string element representing some canned text.
	     * 
	     * @param value
	     *            the text for this string element.
	     */
		public StringElement(string value) : this(value,false)
		{
		}

	    /**
	     * Constructs a new string element representing some canned text.
	     * 
	     * @param value
	     *            the text for this string element.
	     * @param isCapitalized
	     *            whether this string element is be capitalized.
	     */
		public StringElement(string value, bool isCapitalized)
		{ 
			Category = new PhraseCategory(PhraseCategory.PhraseCategoryEnum.CANNED_TEXT);
			setFeature(Feature.ELIDED, false);
			setFeature(Feature.IS_CAPITALIZED, isCapitalized);
			Realisation = value;
		}

	    /**
	     * The string element contains no children so this method will always return
	     * an empty list.
	     */
		public override IList<NLGElement> Children
		{
			get
			{
				return new List<NLGElement>();
			}
		}

		public override string ToString()
		{
			return Realisation;
		}

	    /* (non-Javadoc)
	     * @see simplenlg.framework.NLGElement#equals(java.lang.Object)
	     */
		public override bool Equals(object o)
		{
		    // TODO Auto-generated method stub
			return base.Equals(o) && (o is StringElement) && realisationsMatch((StringElement) o);
		}

		private bool realisationsMatch(StringElement o)
		{
			if (ReferenceEquals(Realisation, null))
			{
				return ReferenceEquals(o.Realisation, null);
			}
			else
			{
				return Realisation.Equals(o.Realisation);
			}
		}

		public override string printTree(string indent)
		{
			StringBuilder print = new StringBuilder();
			print.Append("StringElement: content=\"").Append(Realisation).Append('\"'); //$NON-NLS-1$
			IDictionary<string, object> features = AllFeatures;

			if (features != null)
			{
				print.Append(", features=").Append(features.ToStringNLG()); //$NON-NLS-1$
			}
			print.Append('\n');
			return print.ToString();
		}
	}

}