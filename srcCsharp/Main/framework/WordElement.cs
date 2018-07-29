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

	using Inflection = features.Inflection;
	using LexicalFeature = features.LexicalFeature;

    /**
     * This is the class for a lexical entry (ie, a word). Words are stored in a
     * {@link simplenlg.lexicon.Lexicon}, and usually the developer retrieves a
     * WordElement via a lookup method in the lexicon
     * 
     * Words always have a base form, and usually have a
     * {@link simplenlg.framework.LexicalCategory}. They may also have a Lexicon ID.
     * 
     * Words also have features (which are retrieved from the Lexicon), these are
     * held in the standard NLGElement feature map
     * 
     * @author E. Reiter, University of Aberdeen.
     * @version 4.0
     */
	public class WordElement : NLGElement
	{
	    /*
	     * Internal class. This maintains inflectional variants of the word, which
	     * may be available in the lexicon. For example, a word may have both a
	     * regular and an irregular variant. If the desired type is the irregular,
	     * it is convenient to have the set of irregular inflected forms available
	     * without necessitating a new call to the lexicon to get the forms.
	     */
	    public class InflectionSet
		{
			private readonly WordElement outerInstance;


			// the infl type
			internal Inflection infl;

		    // the forms, mapping values of LexicalFeature to actual word forms
			internal IDictionary<string, string> forms;

			internal InflectionSet(WordElement outerInstance, Inflection infl)
			{
				this.outerInstance = outerInstance;
				this.infl = infl;
				forms = new Dictionary<string, string>();
			}

		    /*
		     * set an inflectional form
		     * 
		     * @param feature
		     * 
		     * @param form
		     */
			internal virtual void addForm(string feature, string form)
			{
				forms[feature] = form;
			}

		/*
		 * get an inflectional form
		 */ 
			internal virtual string getForm(string feature)
			{
				return forms[feature];
			}
		}

	    // Words have baseForm, category, id, and features
	    // features are inherited from NLGElement

		internal string baseForm; // base form, eg "dog". currently also in NLG Element, but will be removed from there

		internal string id; // id in lexicon (may be null);

		internal IDictionary<Inflection, InflectionSet> inflVars; // the inflectional variants

		internal Inflection defaultInfl; // the default inflectional variant



	    /**********************************************************/
	    // constructors
	    /**********************************************************/

	    /**
	     * empty constructor
	     * 
	     */
		public WordElement() : this(null, new LexicalCategory(LexicalCategory.LexicalCategoryEnum.ANY), null)
		{
		}

	    /**
	     * create a WordElement with the specified baseForm (no category or ID
	     * specified)
	     * 
	     * @param baseForm
	     *            - base form of WordElement
	     */
		public WordElement(string baseForm) : this(baseForm, new LexicalCategory(LexicalCategory.LexicalCategoryEnum.ANY), null)
		{
		}

	    /**
	     * create a WordElement with the specified baseForm and category
	     * 
	     * @param baseForm
	     *            - base form of WordElement
	     * @param category
	     *            - category of WordElement
	     */
		public WordElement(string baseForm, LexicalCategory category) : this(baseForm, category, null)
		{
		}

	    /**
	     * create a WordElement with the specified baseForm, category, ID
	     * 
	     * @param baseForm
	     *            - base form of WordElement
	     * @param category
	     *            - category of WordElement
	     * @param id
	     *            - ID of word in lexicon
	     */
		public WordElement(string baseForm, LexicalCategory category, string id) : base()
		{
			this.baseForm = baseForm;
			Category = category;
			this.id = id;
			inflVars = new Dictionary<Inflection, InflectionSet>();
		}

	    /**
	     * creates a duplicate WordElement from an existing WordElement
	     * 
	     * @param currentWord
	     *            - An existing WordElement
	     */
		public WordElement(WordElement currentWord) : base()
		{
			baseForm = currentWord.BaseForm;
			Category = currentWord.Category;
			id = currentWord.Id;
			inflVars = currentWord.InflectionalVariants;
			defaultInfl = (Inflection) currentWord.getDefaultInflectionalVariant();
			Features = currentWord;
		}




	    /**********************************************************/
	    // getters and setters
	    /**********************************************************/

	    /**
	     * @return the baseForm
	     */
		public virtual string BaseForm
		{
			get
			{
				return baseForm;
			}
			set
			{
				baseForm = value;
			}
		}

        /**
	     * @return the id
	     */
		public virtual string Id
		{
			get
			{
				return id;
			}
			set
			{
				id = value;
			}
		}

	    /**
	     * Set the default inflectional variant of a word. This is mostly relevant
	     * if the word has more than one possible inflectional variant (for example,
	     * it can be inflected in both a regular and irregular way).
	     * 
	     * <P>
	     * If the default inflectional variant is set, the inflectional forms of the
	     * word may change as a result. This depends on whether inflectional forms
	     * have been specifically associated with this variant, via
	     * {@link #addInflectionalVariant(Inflection, String, String)}.
	     * 
	     * <P>
	     * The <code>NIHDBLexicon</code> associates different inflectional variants
	     * with words, if they are so specified, and adds the correct forms.
	     * 
	     * @param variant
	     *            The variant
	     */
		public virtual void setDefaultInflectionalVariant(Inflection variant)
		{
			setFeature(LexicalFeature.DEFAULT_INFL, variant);
			defaultInfl = variant;

			if (inflVars.ContainsKey(variant))
			{
				InflectionSet set = inflVars[variant];
				string[] forms = LexicalFeature.getInflectionalFeatures(Category);

				if (forms != null)
				{
					foreach (string f in forms)
					{
						setFeature(f, set.getForm(f));
					}
				}
			}
		}

	    /**
	     * @return the default inflectional variant
	     */
		public virtual object getDefaultInflectionalVariant()
		{
		// return getFeature(LexicalFeature.DEFAULT_INFL);
			return defaultInfl;
		}

	    /**
	     * Convenience method to get all the inflectional forms of the word.
	     * 
	     * @return the HashMap of inflectional variants
	     */
		 public virtual IDictionary<Inflection, InflectionSet> InflectionalVariants
		 {
			 get
			 {
				 return inflVars;
			 }
		 }

	    /**
	     * Convenience method to set the default spelling variant of a word.
	     * Equivalent to
	     * <code>setFeature(LexicalFeature.DEFAULT_SPELL, variant)</code>.
	     * 
	     * <P>
	     * By default, the spelling variant used is the base form. If otherwise set,
	     * this forces the realiser to always use the spelling variant specified.
	     * 
	     * @param variant
	     *            The spelling variant
	     */
		public virtual string DefaultSpellingVariant
		{
			set
			{
				setFeature(LexicalFeature.DEFAULT_SPELL, value);
			}
			get
			{
				string defSpell = getFeatureAsString(LexicalFeature.DEFAULT_SPELL);
				return ReferenceEquals(defSpell, null) ? BaseForm : defSpell;
			}
		}

	    /**
	     * Add an inflectional variant to this word element. This method is intended
	     * for use by a <code>Lexicon</code>. The idea is that words which have more
	     * than one inflectional variant (for example, a regular and an irregular
	     * form of the past tense), can have a default variant (for example, the
	     * regular), but also store information about the other variants. This comes
	     * in useful in case the default inflectional variant is reset to a new one.
	     * In that case, the stored forms for the new variant are used to inflect
	     * the word.
	     * 
	     * <P>
	     * <strong>An example:</strong> The verb <i>lie</i> has both a regular form
	     * (<I>lies, lied, lying</I>) and an irregular form (<I>lay, lain,</I> etc).
	     * Assume that the <code>Lexicon</code> provides this information and treats
	     * this as variant information of the same word (as does the
	     * <code>NIHDBLexicon</code>, for example). Typically, the default
	     * inflectional variant is the <code>Inflection.REGULAR</code>. This means
	     * that morphology proceeds to inflect the verb as <I>lies, lying</I> and so
	     * on. If the default inflectional variant is reset to
	     * <code>Inflection.IRREGULAR</code>, the stored irregular forms will be
	     * used instead.
	     * 
	     * @param infl
	     *            the Inflection pattern with which this form is associated
	     * @param lexicalFeature
	     *            the actual inflectional feature being set, for example
	     *            <code>LexicalFeature.PRESENT_3S</code>
	     * @param form
	     *            the actual inflected word form
	     */
		public virtual void addInflectionalVariant(Inflection infl, string lexicalFeature, string form)
		{
			if (inflVars.ContainsKey(infl))
			{
				inflVars[infl].addForm(lexicalFeature, form);
			}
			else
			{
				InflectionSet set = new InflectionSet(this, infl);
				set.addForm(lexicalFeature, form);
				inflVars[infl] = set;
			}
		}

	    /**
	     * Specify that this word has an inflectional variant (e.g. irregular)
	     * 
	     * @param infl
	     *            the variant
	     */
		public virtual void addInflectionalVariant(Inflection infl)
		{
			inflVars[infl] = new InflectionSet(this, infl);
		}

	    /**
	     * Check whether this word has a particular inflectional variant
	     * 
	     * @param infl
	     *            the variant
	     * @return <code>true</code> if this word has the variant
	     */
		public virtual bool hasInflectionalVariant(Inflection infl)
		{
			return inflVars.ContainsKey(infl);
		}

	    /**
	     * Sets Features from another existing WordElement into this WordElement.
	     * 
	     * @param currentWord
	     * 				the WordElement to copy features from
	     */
		public virtual WordElement Features
		{
			set
			{
				if (null != value && null != value.AllFeatures)
				{
					foreach (string feature in value.AllFeatureNames)
					{
						setFeature(feature, value.getFeature(feature));
					}
				}
			}
		}

	    /**********************************************************/
	    // other methods
	    /**********************************************************/

		public override string ToString()
		{
			ElementCategory _category = Category;
			StringBuilder buffer = new StringBuilder("WordElement["); //$NON-NLS-1$
			buffer.Append(BaseForm).Append(':');
			if (_category != null)
			{
				buffer.Append(_category.ToString());
			}
			else
			{
				buffer.Append("no category"); //$NON-NLS-1$
			}
			buffer.Append(']');
			return buffer.ToString();
		}

		public virtual string toXML()
		{
			string xml = string.Format("<word>%n"); //$NON-NLS-1$
			if (!ReferenceEquals(BaseForm, null))
			{
				xml = xml + string.Format("  <base>%s</base>%n", BaseForm); //$NON-NLS-1$
			}
			if (!Category.Equals(LexicalCategory.LexicalCategoryEnum.ANY))
			{
				xml = xml + string.Format("  <category>%s</category>%n", Category.ToString().ToLower()); //$NON-NLS-1$
			}
			if (!ReferenceEquals(Id, null))
			{
				xml = xml + string.Format("  <id>%s</id>%n", Id); //$NON-NLS-1$
			}

			SortedSet<string> featureNames = new SortedSet<string>(AllFeatureNames); // list features in alpha order
			foreach (string feature in featureNames)
			{
				object value = getFeature(feature);
				if (value != null)
				{ // ignore null features
					if (value is bool?)
					{ // booleans ignored if false,

						bool bvalue = ((bool?) value).Value;
						if (bvalue)
						{
							xml = xml + string.Format("  <%s/>%n", feature); //$NON-NLS-1$
						}
					}
					else
					{ // otherwise include feature and value
						xml = xml + string.Format("  <%s>%s</%s>%n", feature, value.ToString(), feature); //$NON-NLS-1$
					}
				}

			}
			xml = xml + string.Format("</word>%n"); //$NON-NLS-1$
			return xml;
		}

	    /**
	     * This method returns an empty <code>List</code> as words do not have child
	     * elements.
	     */
		public override IList<NLGElement> Children
		{
			get
			{
				return new List<NLGElement>();
			}
		}

		public override string printTree(string indent)
		{
			StringBuilder print = new StringBuilder();
			print.Append("WordElement: base=").Append(BaseForm).Append(", category=").Append(Category.ToString()).Append(", ").Append(base.ToString()).Append('\n'); //$NON-NLS-1$ - $NON-NLS-1$ - $NON-NLS-1$
			return print.ToString();
		}

	    /**
	     * Check if this WordElement is equal to an object.
	     * 
	     * @param o
	     *            the object
	     * @return <code>true</code> iff the object is a word element with the same
	     *         id and the same baseform and the same features.
	     * 
	     */
		public override bool Equals(object o)
		{
			if (o is WordElement)
			{
				WordElement we = (WordElement) o;

				return (ReferenceEquals(baseForm, we.baseForm) || baseForm.Equals(we.baseForm)) && (ReferenceEquals(id, we.id) || id.Equals(we.id)) && we.features.Equals(features);
			}

			return false;
		}
	}

}