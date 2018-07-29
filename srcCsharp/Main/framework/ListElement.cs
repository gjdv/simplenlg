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

	using InternalFeature = features.InternalFeature;

    /**
     * <p>
     * <code>ListElement</code> is used to define elements that can be grouped
     * together and treated in a similar manner. The list element itself adds no
     * additional meaning to the realisation. For example, the syntax processor
     * takes a phrase element and produces a list element containing inflected word
     * elements. Phrase elements only have meaning within the syntax processing
     * while the morphology processor (the next in the sequence) needs to work with
     * inflected words. Using the list element helps to keep the inflected word
     * elements together.
     * </p>
     * 
     * <p>
     * There is no sorting within the list element and components are added in the
     * order they are given.
     * </p>
     * 
     * 
     * @author D. Westwater, University of Aberdeen.
     * @version 4.0
     */
	public class ListElement : NLGElement
	{

	    /**
	     * Creates a new list element with no components.
	     */
		public ListElement()
		{
    		// Do nothing
		}

	    /**
	     * Creates a new list element containing the given components.
	     * 
	     * @param components
	     *            the initial components for this list element.
	     */
		public ListElement(IList<NLGElement> components) : this()
		{
			addComponents(components);
		}

		public override IList<NLGElement> Children
		{
			get
			{
				return getFeatureAsElementList(InternalFeature.COMPONENTS);
			}
		}
	    /**
	     * Creates a new list element containing the given component.
	     * 
	     * @param newComponent
	     *            the initial component for this list element.
	     */
		public ListElement(NLGElement newComponent) : this()
		{
			addComponent(newComponent);
		}

	    /**
	     * Adds the given component to the list element.
	     * 
	     * @param newComponent
	     *            the <code>NLGElement</code> component to be added.
	     */
		public virtual void addComponent(NLGElement newComponent)
		{
			IList<NLGElement> components = getFeatureAsElementList(InternalFeature.COMPONENTS);
			if (components == null)
			{
				components = new List<NLGElement>();
			}
			setFeature(InternalFeature.COMPONENTS, components);
			components.Add(newComponent);
		}

	    /**
	     * Adds the given components to the list element.
	     * 
	     * @param newComponents
	     *            a <code>List</code> of <code>NLGElement</code>s to be added.
	     */
		public virtual void addComponents(IList<NLGElement> newComponents)
		{
			IList<NLGElement> components = getFeatureAsElementList(InternalFeature.COMPONENTS);
			if (components == null)
			{
				components = new List<NLGElement>();
			}
			setFeature(InternalFeature.COMPONENTS, components);
			((List<NLGElement>)components).AddRange(newComponents);
		}

	    /**
	     * Replaces the current components in the list element with the given list.
	     * 
	     * @param newComponents
	     *            a <code>List</code> of <code>NLGElement</code>s to be used as
	     *            the components.
	     */
		public virtual IList<NLGElement> Components
		{
			set
			{
				setFeature(InternalFeature.COMPONENTS, value);
			}
		}

		public override string ToString()
		{
			return Children.ToString();
		}

		public override string printTree(string indent)
		{
			string thisIndent = ReferenceEquals(indent, null) ? " |-" : indent + " |-"; //$NON-NLS-1$ //$NON-NLS-2$
			string childIndent = ReferenceEquals(indent, null) ? " | " : indent + " | "; //$NON-NLS-1$ //$NON-NLS-2$
			string lastIndent = ReferenceEquals(indent, null) ? " \\-" : indent + " \\-"; //$NON-NLS-1$ //$NON-NLS-2$
			string lastChildIndent = ReferenceEquals(indent, null) ? "   " : indent + "   "; //$NON-NLS-1$ //$NON-NLS-2$
			StringBuilder print = new StringBuilder();
			print.Append("ListElement: features="); //$NON-NLS-1$
            print.Append(AllFeatures.ToStringNLG()).Append("\n");

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
	     * Retrieves the number of components in this list element.
	     * @return the number of components.
	     */
	    public virtual int size()
		{
			return Children.Count;
		}

	    /**
	     * Retrieves the first component in the list.
	     * @return the <code>NLGElement</code> at the top of the list.
	     */
		public virtual NLGElement First
		{
			get
			{
				IList<NLGElement> children = Children;
				return children?[0];
			}
		}
	}

}