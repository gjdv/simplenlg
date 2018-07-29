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

namespace SimpleNLG.Main.framework
{

	using Feature = features.Feature;
	using InternalFeature = features.InternalFeature;
	using NumberAgreement = features.NumberAgreement;

    /**
     * <p>
     * This class defines coordination between two or more phrases. Coordination
     * involves the linking of phrases together through the use of key words such as
     * <em>and</em> or <em>but</em>.
     * </p>
     * 
     * <p>
     * The class does not perform any ordering on the coordinates and when realised
     * they appear in the same order they were added to the coordination.
     * </p>
     * 
     * <p>
     * As this class appears similar to the <code>PhraseElement</code> class from an
     * API point of view, it could have extended from the <code>PhraseElement</code>
     * class. However, they are fundamentally different in their nature and thus
     * form two distinct classes with similar APIs.
     * </p>
     * @author D. Westwater, University of Aberdeen.
     * @version 4.0
     * 
     */
	public class CoordinatedPhraseElement : NLGElement
	{
	    /** Coordinators which make the coordinate plural (eg, "and" but not "or")*/


		private static readonly IList<string> PLURAL_COORDINATORS = new List<string>{"and"};

	    /**
	     * Creates a blank coordinated phrase ready for new coordinates to be added.
	     * The default conjunction used is <em>and</em>.
	     */
		public CoordinatedPhraseElement() : base()
		{
			setFeature(Feature.CONJUNCTION, "and"); //$NON-NLS-1$
		}

	    /**
	     * Creates a coordinated phrase linking the two phrase together. The default
	     * conjunction used is <em>and</em>.
	     * 
	     * @param coordinate1
	     *            the first coordinate.
	     * @param coordinate2
	     *            the second coordinate.
	     */
		public CoordinatedPhraseElement(object coordinate1, object coordinate2)
		{

			addCoordinate(coordinate1);
			addCoordinate(coordinate2);
			setFeature(Feature.CONJUNCTION, "and"); //$NON-NLS-1$
		}

	    /**
	     * Adds a new coordinate to this coordination. If the new coordinate is a
	     * <code>NLGElement</code> then it is added directly to the coordination. If
	     * the new coordinate is a <code>String</code> a <code>StringElement</code>
	     * is created and added to the coordination. <code>StringElement</code>s
	     * will have their complementisers suppressed by default. In the case of
	     * clauses, complementisers will be suppressed if the clause is not the
	     * first element in the coordination.
	     * 
	     * @param newCoordinate
	     *            the new coordinate to be added.
	     */
		public virtual void addCoordinate(object newCoordinate)
		{
			IList<NLGElement> coordinates = getFeatureAsElementList(InternalFeature.COORDINATES);
			if (coordinates == null)
			{
				coordinates = new List<NLGElement>();
				setFeature(InternalFeature.COORDINATES, coordinates);
			}
			else if (coordinates.Count == 0)
			{
				setFeature(InternalFeature.COORDINATES, coordinates);
			}
			if (newCoordinate is NLGElement)
			{
				if (((NLGElement) newCoordinate).isA(new PhraseCategory(PhraseCategory.PhraseCategoryEnum.CLAUSE)) && coordinates.Any())
				{

					((NLGElement) newCoordinate).setFeature(Feature.SUPRESSED_COMPLEMENTISER, true);
				}
				coordinates.Add((NLGElement) newCoordinate);
			}
			else if (newCoordinate is string)
			{
				NLGElement coordElement = new StringElement((string) newCoordinate);
				coordElement.setFeature(Feature.SUPRESSED_COMPLEMENTISER, true);
				coordinates.Add(coordElement);
			}
			setFeature(InternalFeature.COORDINATES, coordinates);
		}

		public override IList<NLGElement> Children
		{
			get
			{
				return getFeatureAsElementList(InternalFeature.COORDINATES);
			}
		}
	    /**
	     * Clears the existing coordinates in this coordination. It performs exactly
	     * the same as <code>removeFeature(Feature.COORDINATES)</code>.
	     */
		public virtual void clearCoordinates()
		{
			removeFeature(InternalFeature.COORDINATES);
		}

	    /**
	     * Adds a new pre-modifier to the phrase element. Pre-modifiers will be
	     * realised in the syntax before the coordinates.
	     * 
	     * @param newPreModifier
	     *            the new pre-modifier as an <code>NLGElement</code>.
	     */
		public virtual void addPreModifier(NLGElement newPreModifier)
		{
			IList<NLGElement> preModifiers = getFeatureAsElementList(InternalFeature.PREMODIFIERS);
			if (preModifiers == null)
			{
				preModifiers = new List<NLGElement>();
			}
			preModifiers.Add(newPreModifier);
			setFeature(InternalFeature.PREMODIFIERS, preModifiers);
		}

	    /**
	     * Adds a new pre-modifier to the phrase element. Pre-modifiers will be
	     * realised in the syntax before the coordinates.
	     * 
	     * @param newPreModifier
	     *            the new pre-modifier as a <code>String</code>. It is used to
	     *            create a <code>StringElement</code>.
	     */
		public virtual void addPreModifier(string newPreModifier)
		{
			IList<NLGElement> preModifiers = getFeatureAsElementList(InternalFeature.PREMODIFIERS);
			if (preModifiers == null)
			{
				preModifiers = new List<NLGElement>();
			}
			preModifiers.Add(new StringElement(newPreModifier));
			setFeature(InternalFeature.PREMODIFIERS, preModifiers);
		}

	    /**
	     * Retrieves the list of pre-modifiers currently associated with this
	     * coordination.
	     * 
	     * @return a <code>List</code> of <code>NLGElement</code>s.
	     */
		public virtual IList<NLGElement> PreModifiers
		{
			get
			{
				return getFeatureAsElementList(InternalFeature.PREMODIFIERS);
			}
		}
	    /**
	     * Retrieves the list of complements currently associated with this
	     * coordination.
	     * 
	     * @return a <code>List</code> of <code>NLGElement</code>s.
	     */
		public virtual IList<NLGElement> Complements
		{
			get
			{
				return getFeatureAsElementList(InternalFeature.COMPLEMENTS);
			}
		}
	    /**
	     * Adds a new post-modifier to the phrase element. Post-modifiers will be
	     * realised in the syntax after the coordinates.
	     * 
	     * @param newPostModifier
	     *            the new post-modifier as an <code>NLGElement</code>.
	     */
		public virtual void addPostModifier(NLGElement newPostModifier)
		{
			IList<NLGElement> postModifiers = getFeatureAsElementList(InternalFeature.POSTMODIFIERS);
			if (postModifiers == null)
			{
				postModifiers = new List<NLGElement>();
			}
			postModifiers.Add(newPostModifier);
			setFeature(InternalFeature.POSTMODIFIERS, postModifiers);
		}

	    /**
	     * Adds a new post-modifier to the phrase element. Post-modifiers will be
	     * realised in the syntax after the coordinates.
	     * 
	     * @param newPostModifier
	     *            the new post-modifier as a <code>String</code>. It is used to
	     *            create a <code>StringElement</code>.
	     */
		public virtual void addPostModifier(string newPostModifier)
		{
			IList<NLGElement> postModifiers = getFeatureAsElementList(InternalFeature.POSTMODIFIERS);
			if (postModifiers == null)
			{
				postModifiers = new List<NLGElement>();
			}
			postModifiers.Add(new StringElement(newPostModifier));
			setFeature(InternalFeature.POSTMODIFIERS, postModifiers);
		}

	    /**
	     * Retrieves the list of post-modifiers currently associated with this
	     * coordination.
	     * 
	     * @return a <code>List</code> of <code>NLGElement</code>s.
	     */
		public virtual IList<NLGElement> PostModifiers
		{
			get
			{
				return getFeatureAsElementList(InternalFeature.POSTMODIFIERS);
			}
		}

		public override string printTree(string indent)
		{
			string thisIndent = ReferenceEquals(indent, null) ? " |-" : indent + " |-"; //$NON-NLS-1$ //$NON-NLS-2$
			string childIndent = ReferenceEquals(indent, null) ? " | " : indent + " | "; //$NON-NLS-1$ //$NON-NLS-2$
			string lastIndent = ReferenceEquals(indent, null) ? " \\-" : indent + " \\-"; //$NON-NLS-1$ //$NON-NLS-2$
			string lastChildIndent = ReferenceEquals(indent, null) ? "   " : indent + "   "; //$NON-NLS-1$ //$NON-NLS-2$
			StringBuilder print = new StringBuilder();
			print.Append("CoordinatedPhraseElement:\n"); //$NON-NLS-1$

			IList<NLGElement> children = Children;
			int length = children.Count - 1;
			int index = 0;

			for (index = 0; index < length; index++)
			{
				print.Append(thisIndent).Append(children[index].printTree(childIndent));
			}
			if (length >= 0)
			{
				print.Append(lastIndent).Append(children[length].printTree(lastChildIndent));
			}
			return print.ToString();
		}

	    /**
	     * Adds a new complement to the phrase element. Complements will be realised
	     * in the syntax after the coordinates. Complements differ from
	     * post-modifiers in that complements are crucial to the understanding of a
	     * phrase whereas post-modifiers are optional.
	     * 
	     * @param newComplement
	     *            the new complement as an <code>NLGElement</code>.
	     */
		public virtual void addComplement(NLGElement newComplement)
		{
			IList<NLGElement> complements = getFeatureAsElementList(InternalFeature.COMPLEMENTS);
			if (complements == null)
			{
				complements = new List<NLGElement>();
			}
			complements.Add(newComplement);
			setFeature(InternalFeature.COMPLEMENTS, complements);
		}

	    /**
	     * Adds a new complement to the phrase element. Complements will be realised
	     * in the syntax after the coordinates. Complements differ from
	     * post-modifiers in that complements are crucial to the understanding of a
	     * phrase whereas post-modifiers are optional.
	     * 
	     * @param newComplement
	     *            the new complement as a <code>String</code>. It is used to
	     *            create a <code>StringElement</code>.
	     */
		public virtual void addComplement(string newComplement)
		{
			IList<NLGElement> complements = getFeatureAsElementList(InternalFeature.COMPLEMENTS);
			if (complements == null)
			{
				complements = new List<NLGElement>();
			}
			complements.Add(new StringElement(newComplement));
			setFeature(InternalFeature.COMPLEMENTS, complements);
		}

	    /**
	     * A convenience method for retrieving the last coordinate in this
	     * coordination.
	     * 
	     * @return the last coordinate as represented by a <code>NLGElement</code>
	     */
		public virtual NLGElement LastCoordinate
		{
			get
			{
				IList<NLGElement> children = Children;
				return children != null && children.Any() ? children[children.Count - 1] : null;
			}
		}
	    /** set / get the conjunction to be used in a coordinatedphraseelement
	     * @param conjunction
	     */
		public virtual string Conjunction
		{
			set
			{
				setFeature(Feature.CONJUNCTION, value);
			}
			get
			{
				return getFeatureAsString(Feature.CONJUNCTION);
			}
		}
	    
	    /**
	     * @return true if this coordinate is plural in a syntactic sense
	     */
		public virtual bool checkIfPlural()
		{
	    	// doing this right is quite complex, take simple approach for now
			int size = Children.Count;
			if (size == 1)
			{
				return (NumberAgreement.PLURAL.Equals(LastCoordinate.getFeature(Feature.NUMBER)));
			}
			else
			{
				return PLURAL_COORDINATORS.Contains(Conjunction);
			}
		}
	}

}