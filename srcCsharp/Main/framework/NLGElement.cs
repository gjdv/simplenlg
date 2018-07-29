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

using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleNLG.Main.framework
{

	using Feature = features.Feature;
	using NumberAgreement = features.NumberAgreement;
	using Tense = features.Tense;

    /**
     * <p>
     * <code>NLGElement</code> is the base class that all elements extend from. This
     * is abstract and cannot therefore be instantiated itself. The additional
     * element classes should be used to correctly identify the type of element
     * required.
     * </p>
     * 
     * <p>
     * Realisation in SimpleNLG revolves around a tree structure. Each node in the
     * tree is represented by a <code>NLGElement</code>, which in turn may have
     * child nodes. The job of the processors is to replace various types of
     * elements with other elements. The eventual goal, once all the processors have
     * been run, is to produce a single string element representing the final
     * realisation.
     * </p>
     * 
     * <p>
     * The features are stored in a <code>Map</code> of <code>String</code> (the
     * feature name) and <code>Object</code> (the value of the feature).
     * </p>
     * 
     * 
     * @author D. Westwater, University of Aberdeen.
     * @version 4.0
     */
	public abstract class NLGElement
	{

    	/** The category of this element. */
		private ElementCategory category;

	    /** The features of this element. */
		protected internal EqualElmDictionary<string, object> features = new EqualElmDictionary<string, object>();

	    /** The parent of this element. */
		private NLGElement parent;

	    /** The realisation of this element. */
		private string realisation;

	    /** The NLGFactory which created this element */
		private NLGFactory factory;

	    /**
	     * Sets the category of this element.
	     * 
	     * @param newCategory
	     *            the new <code>ElementCategory</code> for this element.
	     */
		public virtual ElementCategory Category
		{
			set
			{
				category = value;
			}
			get
			{

				return category;
			}
		}
	    /**
	     * Adds a feature to the feature map. If the feature already exists then it
	     * is given the new value. If the value provided is <code>null</code> the
	     * feature is removed from the map.
	     * 
	     * @param featureName
	     *            the name of the feature.
	     * @param featureValue
	     *            the new value of the feature or <code>null</code> if the
	     *            feature is to be removed.
	     */
		public virtual void setFeature(string featureName, object featureValue)
		{
			if (featureName != null)
			{
				if (featureValue == null)
				{
					features.Remove(featureName);
				}
				else
				{
					features[featureName] = featureValue;
				}
			}
		}

	    /**
	     * A convenience method for setting boolean features.
	     * 
	     * @param featureName
	     *            the name of the feature.
	     * @param featureValue
	     *            the <code>boolean</code> value of the feature.
	     */
		public virtual void setFeature(string featureName, bool featureValue)
		{
			if (!ReferenceEquals(featureName, null))
			{
				features[featureName] = new bool?(featureValue);
			}
		}

	    /**
	     * A convenience method for setting integer features.
	     * 
	     * @param featureName
	     *            the name of the feature.
	     * @param featureValue
	     *            the <code>int</code> value of the feature.
	     */
		public virtual void setFeature(string featureName, int featureValue)
		{
			if (!ReferenceEquals(featureName, null))
			{
				features[featureName] = new int?(featureValue);
			}
		}

	    /**
	     * A convenience method for setting long integer features.
	     * 
	     * @param featureName
	     *            the name of the feature.
	     * @param featureValue
	     *            the <code>long</code> value of the feature.
	     */
		public virtual void setFeature(string featureName, long featureValue)
		{
			if (!ReferenceEquals(featureName, null))
			{
				features[featureName] = new long?(featureValue);
			}
		}

	    /**
	     * A convenience method for setting floating point number features.
	     * 
	     * @param featureName
	     *            the name of the feature.
	     * @param featureValue
	     *            the <code>float</code> value of the feature.
	     */
		public virtual void setFeature(string featureName, float featureValue)
		{
			if (!ReferenceEquals(featureName, null))
			{
				features[featureName] = new float?(featureValue);
			}
		}

	    /**
	     * A convenience method for setting double precision floating point number
	     * features.
	     * 
	     * @param featureName
	     *            the name of the feature.
	     * @param featureValue
	     *            the <code>double</code> value of the feature.
	     */
		public virtual void setFeature(string featureName, double featureValue)
		{
			if (!ReferenceEquals(featureName, null))
			{
				features[featureName] = new double?(featureValue);
			}
		}

	    /**
	     * Retrieves the value of the feature.
	     * 
	     * @param featureName
	     *            the name of the feature.
	     * @return the <code>Object</code> value of the feature.
	     */
		public virtual object getFeature(string featureName)
		{
			return featureName == null ?  null : (features.ContainsKey(featureName) ? features[featureName] : null);
		}

	    /**
	     * Retrieves the value of the feature as a string. If the feature doesn't
	     * exist then <code>null</code> is returned.
	     * 
	     * @param featureName
	     *            the name of the feature.
	     * @return the <code>String</code> representation of the value. This is
	     *         taken by calling the object's <code>toString()</code> method.
	     */
		public virtual string getFeatureAsString(string featureName)
		{
			object value = getFeature(featureName);
			string stringValue = null;

			if (value != null)
			{
				stringValue = value.ToString();
			}
			return stringValue;
		}

	    /**
	     * <p>
	     * Retrieves the value of the feature as a list of elements. If the feature
	     * is a single <code>NLGElement</code> then it is wrapped in a list. If the
	     * feature is a <code>Collection</code> then each object in the collection
	     * is checked and only <code>NLGElement</code>s are returned in the list.
	     * </p>
	     * <p>
	     * If the feature does not exist then an empty list is returned.
	     * </p>
	     * 
	     * @param featureName
	     *            the name of the feature.
	     * @return the <code>List</code> of <code>NLGElement</code>s
	     */
		public virtual EqualElmList<NLGElement> getFeatureAsElementList(string featureName)
		{
			EqualElmList<NLGElement> list = new EqualElmList<NLGElement>();

			object value = getFeature(featureName);
			if (value is NLGElement)
			{
				list.Add((NLGElement) value);
			}
			else if (value is IEnumerable<object>)
			{
			    IEnumerable<object> enumerable = ((IEnumerable<object>) value);
				foreach(object nextObject in enumerable)
				{
					if (nextObject is NLGElement)
					{
						list.Add((NLGElement) nextObject);
					}
				}
			}
			return list;
		}

	    /**
	     * <p>
	     * Retrieves the value of the feature as a list of java objects. If the feature
	     * is a single element, the list contains only this element.
	     * If the feature is a <code>Collection</code> each object in the collection is
	     * returned in the list.
	     * </p>
	     * <p>
	     * If the feature does not exist then an empty list is returned.
	     * </p>
	     * 
	     * @param featureName
	     *            the name of the feature.
	     * @return the <code>List</code> of <code>Object</code>s
	     */
		public virtual IList<object> getFeatureAsList(string featureName)
		{
			IList<object> values = new List<object>();
			object value = getFeature(featureName);

			if (value != null)
			{
				if (value is IEnumerable<object>)
				{
				    IEnumerable<object> enumerable = ((IEnumerable<object>)value);
				    foreach (object nextObject in enumerable)
                    {
						values.Add(nextObject);
					}
				}
				else
				{
					values.Add(value);
				}
			}

			return values;
		}

	    /**
	     * <p>
	     * Retrieves the value of the feature as a list of strings. If the feature
	     * is a single element, then its <code>toString()</code> value is wrapped in
	     * a list. If the feature is a <code>Collection</code> then the
	     * <code>toString()</code> value of each object in the collection is
	     * returned in the list.
	     * </p>
	     * <p>
	     * If the feature does not exist then an empty list is returned.
	     * </p>
	     * 
	     * @param featureName
	     *            the name of the feature.
	     * @return the <code>List</code> of <code>String</code>s
	     */
		public virtual IList<string> getFeatureAsStringList(string featureName)
		{
			IList<string> values = new List<string>();
			object value = getFeature(featureName);

			if (value != null)
			{
			    if (value is IEnumerable<object>)
			    {
			        IEnumerable<object> enumerable = ((IEnumerable<object>)value);
			        foreach (object nextObject in enumerable)
                    {
						values.Add(nextObject.ToString());
					}
				}
				else
				{
					values.Add(value.ToString());
				}
			}

			return values;
		}

	    /**
	     * Retrieves the value of the feature as an <code>Integer</code>. If the
	     * feature does not exist or cannot be converted to an integer then
	     * <code>null</code> is returned.
	     * 
	     * @param featureName
	     *            the name of the feature.
	     * @return the <code>Integer</code> representation of the value. Numbers are
	     *         converted to integers while Strings are parsed for integer
	     *         values. Any other type will return <code>null</code>.
	     */
		public virtual int? getFeatureAsInteger(string featureName)
		{
			object value = getFeature(featureName);
			int? intValue = null;
			if (value is int?)
			{
				intValue = (int?) value;
			}
			else if (value is double)
			{
				intValue = (int) value;
			}
			else if (value is string)
			{
				try
				{
					intValue = Convert.ToInt32((string) value);
				}
				catch (FormatException)
	
				{
					intValue = null;
				}
			}
			return intValue;
		}

	    /**
         * Retrieves the value of the feature as a <code>Double</code>. If the
         * feature does not exist or cannot be converted to a double then
         * <code>null</code> is returned.
         * 
         * @param featureName
         *            the name of the feature.
         * @return the <code>Double</code> representation of the value. Numbers are
         *         converted to doubles while Strings are parsed for double values.
         *         Any other type will return <code>null</code>.
         */
        public virtual double? getFeatureAsDouble(string featureName)
		{
			object value = getFeature(featureName);
			double? doubleValue = null;
			if (value is double?)
			{
				doubleValue = (double?) value;
			}
			else if (value is string)
			{
				try
				{
					doubleValue = Convert.ToDouble((string) value);
				}
				catch (FormatException)
				{
					doubleValue = null;
				}
			}
			return doubleValue;
		}

	    /**
	     * Retrieves the value of the feature as a <code>Boolean</code>. If the
	     * feature does not exist or is not a boolean then
	     * <code>Boolean.FALSE</code> is returned.
	     * 
	     * @param featureName
	     *            the name of the feature.
	     * @return the <code>Boolean</code> representation of the value. Any
	     *         non-Boolean type will return <code>Boolean.FALSE</code>.
	     */
		public virtual bool getFeatureAsBoolean(string featureName)
		{
			object value = getFeature(featureName);
			bool boolValue = false;

			if (value is bool)
			{
				boolValue = (bool) value;
			}

			return boolValue;
		}

	    /**
	     * Retrieves the value of the feature as a <code>NLGElement</code>. If the
	     * value is a string then it is wrapped in a <code>StringElement</code>. If
	     * the feature does not exist or is of any other type then <code>null</code>
	     * is returned.
	     * 
	     * @param featureName
	     *            the name of the feature.
	     * @return the <code>NLGElement</code>.
	     */
		public virtual NLGElement getFeatureAsElement(string featureName)
		{
			object value = getFeature(featureName);
			NLGElement elementValue = null;

			if (value is NLGElement)
			{
				elementValue = (NLGElement) value;
			}
			else if (value is string)
			{
				elementValue = new StringElement((string) value);
			}
			return elementValue;
		}

	    /**
	     * Retrieves the map containing all the features for this element.
	     * 
	     * @return a <code>Map</code> of <code>String</code>, <code>Object</code>.
	     */
		public virtual IDictionary<string, object> AllFeatures
		{
			get
			{
				return features;
			}
		}
	    /**
	     * Checks the feature map to see if the named feature is present in the map.
	     * 
	     * @param featureName
	     *            the name of the feature to look for.
	     * @return <code>true</code> if the feature exists, <code>false</code>
	     *         otherwise.
	     */
		public virtual bool hasFeature(string featureName)
		{
			return !ReferenceEquals(featureName, null) ? features.ContainsKey(featureName) : false;
		}

	    /**
	     * Deletes the named feature from the map.
	     * 
	     * @param featureName
	     *            the name of the feature to be removed.
	     */
		public virtual void removeFeature(string featureName)
		{
			features.Remove(featureName);
		}

	    /**
	     * Deletes all the features in the map.
	     */
		public virtual void clearAllFeatures()
		{
			features.Clear();
		}

	    /**
	     * Set / get the parent element of this element.
	     * 
	     * @param newParent
	     *            the <code>NLGElement</code> that is the parent of this
	     *            element.
	     */
		public virtual NLGElement Parent
		{
			set
			{
				parent = value;
			}
			get
			{
				return parent;
			}
		}
	    /**
	     * Set / get the realisation of this element.
	     * 
	     * @param realised
	     *            the <code>String</code> representing the final realisation for
	     *            this element.
	     */
		public virtual string Realisation
		{
			set
			{
				realisation = value;
			}
			get
			{
				int start = 0;
				int end = 0;
				if (null != realisation)
				{
					end = realisation.Length;
    
					while (start < realisation.Length && ' ' == realisation[start])
					{
						start++;
					}
					if (start == realisation.Length)
					{
						realisation = null;
					}
					else
					{
						while (end > 0 && ' ' == realisation[end - 1])
						{
							end--;
						}
					}
				}
    
		        // AG: changed this to return the empty string if the realisation is
		        // null
		        // avoids spurious nulls appearing in output for empty phrases.
				return ReferenceEquals(realisation, null) ? "" : realisation.Substring(start, end - start);
			}
		}



		public override string ToString()
		{
			StringBuilder buffer = (new StringBuilder("{realisation=")).Append(realisation); //$NON-NLS-1$
			if (category != null)
			{
				buffer.Append(", category=").Append(category.ToString()); //$NON-NLS-1$
			}
			if (features != null)
			{
				buffer.Append(", features=").Append(features.ToStringNLG()); //$NON-NLS-1$
			}
			buffer.Append('}');
			return buffer.ToString();
		}

		public virtual bool isA(ElementCategory checkCategory)
		{
			bool isA = false;

			if (category != null)
			{
				isA = category.Equals(checkCategory);
			}
			else if (checkCategory == null)
			{
				isA = true;
			}
			return isA;
		}

	    /**
	     * Retrieves the children for this element. This method needs to be
	     * overridden for each specific type of element. Each type of element will
	     * have its own way of determining the child elements.
	     * 
	     * @return a <code>List</code> of <code>NLGElement</code>s representing the
	     *         children of this element.
	     */
		public abstract IList<NLGElement> Children {get;}

        /**
	     * Retrieves the set of features currently contained in the feature map.
	     * 
	     * @return a <code>Set</code> of <code>String</code>s representing the
	     *         feature names. The set is unordered.
	     */
		public virtual Dictionary<string, object>.KeyCollection AllFeatureNames
		{
			get
			{
				return features.Keys;
			}
		}

		public virtual string printTree(string indent)
		{
			string thisIndent = ReferenceEquals(indent, null) ? " |-" : indent + " |-"; //$NON-NLS-1$ //$NON-NLS-2$
			string childIndent = ReferenceEquals(indent, null) ? " |-" : indent + " |-"; //$NON-NLS-1$ //$NON-NLS-2$
			StringBuilder print = new StringBuilder();
			print.Append("NLGElement: ").Append(ToString()).Append('\n'); //$NON-NLS-1$

			IList<NLGElement> children = Children;

			if (children != null)
			{
				foreach (NLGElement eachChild in Children)
				{
					print.Append(thisIndent).Append(eachChild.printTree(childIndent));
				}
			}
			return print.ToString();
		}

	    /**
	     * Determines if this element has its realisation equal to the given string.
	     * 
	     * @param elementRealisation
	     *            the string to check against.
	     * @return <code>true</code> if the string matches the element's
	     *         realisation, <code>false</code> otherwise.
	     */
		public virtual bool Equals(string elementRealisation)
		{
			bool match = false;

			if (ReferenceEquals(elementRealisation, null) && ReferenceEquals(realisation, null))
			{
				match = true;
			}
			else if (!ReferenceEquals(elementRealisation, null) && !ReferenceEquals(realisation, null))
			{
				match = elementRealisation.Equals(realisation);
			}
			return match;
		}

	    /**
	     * Sets the number agreement on this element. This method is added for
	     * convenience and not all element types will make use of the number
	     * agreement feature. The method is identical to calling {@code
	     * setFeature(Feature.NUMBER, NumberAgreement.PLURAL)} for plurals or
	     * {@code setFeature(Feature.NUMBER, NumberAgreement.SINGULAR)} for the
	     * singular.
	     * 
	     * @param isPlural
	     *            <code>true</code> if this element is to be treated as a
	     *            plural, <code>false</code> otherwise.
	     */
		public virtual bool Plural
		{
			set
			{
				if (value)
				{
					setFeature(Feature.NUMBER, NumberAgreement.PLURAL);
				}
				else
				{
					setFeature(Feature.NUMBER, NumberAgreement.SINGULAR);
				}
			}
			get
			{

				return NumberAgreement.PLURAL.Equals(getFeature(Feature.NUMBER));
			}
		}
        

	    // Following should be deleted at some point, as it makes more sense to have
	    // them in SPhraseSpec
	    /**
	     * Retrieves the tense for this element. The method is identical to calling
	     * {@code getFeature(Feature.TENSE)} and casting the result as
	     * <code>Tense<code>.
	     * *
	     * WARNING: You should use getFeature(Feature.TENSE)
	     * getTense will be dropped from simplenlg at some point
	     * 
	     * @return the <code>Tense</code> of this element.
	     */
		[Obsolete("use getFeature(Feature.TENSE) instead",true)]
		public virtual Tense Tense
		{
			get
			{
				Tense tense = Tense.PRESENT;
				object tenseValue = getFeature(Feature.TENSE);
				if (tenseValue is Tense)
				{
					tense = (Tense) tenseValue;
				}
				return tense;
			}
			set
			{
				setFeature(Feature.TENSE, value);
			}
		}



        /**
	     * Sets the negation on this element. The method is identical to calling
	     * {@code setFeature(Feature.NEGATED, isNegated)}.
	     * 
	     * WARNING: You should use setFeature(Feature.NEGATED, isNegated) setNegated
	     * will be dropped from simplenlg at some point
	     * 
	     * @param isNegated
	     *            <code>true</code> if the element is to be negated,
	     *            <code>false</code> otherwise.
	     */
		[Obsolete("use setFeature(Feature.NEGATED,isNegated) instead",true)]
		public virtual bool Negated
		{
			set
			{
				setFeature(Feature.NEGATED, value);
			}
			get
			{
				return getFeatureAsBoolean(Feature.NEGATED);
			}
		}

	    /**
	     * @return the NLG factory
	     */
		public virtual NLGFactory Factory
		{
			get
			{
				return factory;
			}
			set
			{
				factory = value;
			}
		}
	    /**
	     * An NLG element is equal to some object if the object is an NLGElement,
	     * they have the same category and the same features.
	     */
		public override bool Equals(object o)
		{
			bool eq = false;

			if (o is NLGElement)
			{
				NLGElement element = (NLGElement) o;
				eq = category == element.category && features.Equals(element.features);
			}

			return eq;
		}

		public virtual bool Capitalized
		{
			get
			{ 
				if (features.ContainsKey(Feature.IS_CAPITALIZED))
				{
					return (bool) getFeature(Feature.IS_CAPITALIZED);
				}
				else
				{
					return false;
				}
			}
		}

	}

}