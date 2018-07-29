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

    /**
     * <p>
     * <code>DocumentElement</code> is a convenient extension of the base
     * <code>NLGElement</code> class. It used to define elements that form part of
     * the textual structure (documents, sections, paragraphs, sentences, lists). It
     * essentially operates as the base class with the addition of some convenience
     * methods for getting and setting features specific to this type of element.
     * </p>
     * <p>
     * <code>TextElements</code> can be structured in a tree-like structure where
     * elements can contain child components. These child components can in turn
     * contain other child components and so on. There are restrictions on the type
     * of child components a particular element type can have. These are explained
     * under the <code>DocumentCategory</code> enumeration.
     * <p>
     * Instances of this class can be created through the static create methods in
     * the <code>OrthographyProcessor</code>.
     * </p>
     * 
     * 
     * @author D. Westwater, University of Aberdeen.
     * @version 4.0
     */
	public class DocumentElement : NLGElement
	{

    	/** The feature relating to the title or heading of this element. */
		private const string FEATURE_TITLE = "textTitle"; //$NON-NLS-1$

    	/** The feature relating to the components (or child nodes) of this element. */
		private const string FEATURE_COMPONENTS = "textComponents"; //$NON-NLS-1$

	    /**
	     * The blank constructor. Using this constructor will require manual setting
	     * of the element's category and title.
	     */
	    public DocumentElement()
		{
    		// Do nothing
		}

	    /**
	     * Creates a new DocumentElement with the given category and title.
	     * 
	     * @param category
	     *            the category for this element.
	     * @param textTitle
	     *            the title of this element, predominantly used with DOCUMENT
	     *            and SECTION types.
	     */
		public DocumentElement(DocumentCategory category, string textTitle)
		{
			Category = category;
			Title = textTitle;
		}

	    /**
	     * Set / get the title of this element. Titles are specifically used with
	     * documents (the document name) and sections (headings).
	     * 
	     * @param textTitle
	     *            the new title for this element.
	     */
		public virtual string Title
		{
			set
			{
				setFeature(FEATURE_TITLE, value);
			}
			get
			{
				return getFeatureAsString(FEATURE_TITLE);
			}
		}
	    /**
	     * Retrieves the child components of this element.
	     * 
	     * @return a <code>List</code> of <code>NLGElement</code>s representing the
	     *         child components.
	     */
		public virtual IList<NLGElement> Components
		{
			get
			{
				return getFeatureAsElementList(FEATURE_COMPONENTS);
			}
			set
			{
				setFeature(FEATURE_COMPONENTS, value);
			}
		}

	    /**
	     * <p>
	     * Add a single child component to the current list of child components. If
	     * there are no existing child components a new list is created.
	     * </p>
	     * <p>
	     * Note that there are restrictions on which child types can be added to
	     * which parent types.  Intermediate nodes are added if necessary; eg,
	     * if a sentence is added to a document, the sentence will be embedded
	     * in a paragraph before it is added
	     * See <code>
	     * DocumentCategory</code> for further information.
	     * </p>
	     * 
	     * @param element
	     *            the <code>NLGElement</code> to be added. If this is
	     *            <code>NULL</code> the method does nothing.
	     */
		public virtual void addComponent(NLGElement element)
		{
			if (element != null)
			{
				ElementCategory thisCategory = Category;
				ElementCategory category = element.Category;
				if (category != null && thisCategory is DocumentCategory)
				{
					if (((DocumentCategory) thisCategory).hasSubPart(category))
					{
						addElementToComponents(element);
					}
					else
					{
						NLGElement promotedElement = promote(element);
						if (promotedElement != null)
						{
							addElementToComponents(promotedElement);
						}
						else // error condition - add original element so something is visible
						{
							addElementToComponents(element);
						}
					}
				}
				else
				{
					addElementToComponents(element);
				}
			}
		}

	    /** add an element to a components list
	     * @param element
	     */
		private void addElementToComponents(NLGElement element)
		{
			IList<NLGElement> components = Components;
			components.Add(element);
			element.Parent = this;
			Components = components;
		}


	    /** promote an NLGElement so that it is at the right level to be added to a DocumentElement/
	     * Promotion means adding surrounding nodes at higher doc levels
	     * @param element
	     * @return
	     */
		private NLGElement promote(NLGElement element)
		{
		    // check if promotion needed
			if (((DocumentCategory) Category).hasSubPart(element.Category))
			{
				return element;
			}
		    // if element is not a DocumentElement, embed it in a sentence and recurse
			if (!(element is DocumentElement))
			{
				DocumentElement sentence = new DocumentElement(new DocumentCategory(DocumentCategory.DocumentCategoryEnum.SENTENCE), null);
				sentence.addElementToComponents(element);
				return promote(sentence);
			}

		    // if element is a Sentence, promote it to a paragraph
			if (element.Category.Equals(DocumentCategory.DocumentCategoryEnum.SENTENCE))
			{
				DocumentElement paragraph = new DocumentElement(new DocumentCategory(DocumentCategory.DocumentCategoryEnum.PARAGRAPH), null);
				paragraph.addElementToComponents(element);
				return promote(paragraph);
			}

		    // otherwise can't do anything
			return null;
		}

	    /**
	     * <p>
	     * Adds a collection of <code>NLGElements</code> to the list of child
	     * components. If there are no existing child components, then a new list is
	     * created.
	     * </p>
	     * <p>
	     * As with <code>addComponents(...)</code> only legitimate child types are
	     * added to the list.
	     * </p>
	     * 
	     * @param textComponents
	     *            the <code>List</code> of <code>NLGElement</code>s to be added.
	     *            If this is <code>NULL</code> the method does nothing.
	     */
		public virtual void addComponents<T1>(IList<T1> textComponents)
		{
			if (textComponents != null)
			{
				ElementCategory thisCategory = Category;
				List<NLGElement> elementsToAdd = new List<NLGElement>();
				ElementCategory category = null;

				foreach (object eachElement in textComponents)
				{
					if (eachElement is NLGElement)
					{
						category = ((NLGElement) eachElement).Category;
						if (category != null && thisCategory is DocumentCategory)
						{
							if (((DocumentCategory) thisCategory).hasSubPart(category))
							{
								elementsToAdd.Add((NLGElement) eachElement);
								((NLGElement) eachElement).Parent = this;
							}
						}
					}
				}
				if (elementsToAdd.Any())
				{
					IList<NLGElement> components = Components;
					if (components == null)
					{
						components = new List<NLGElement>();
					}
					((List<NLGElement>)components).AddRange(elementsToAdd);
					setFeature(FEATURE_COMPONENTS, components);
				}
			}
		}

	    /**
	     * Removes the specified component from the list of child components.
	     * 
	     * @param textComponent
	     *            the component to be removed.
	     * @return <code>true</code> if the element was removed, or
	     *         <code>false</code> if the element did not exist, there is no
	     *         component list or the the given component to remove is
	     *         <code>NULL</code>.
	     */
		public virtual bool removeComponent(NLGElement textComponent)
		{
			bool removed = false;

			if (textComponent != null)
			{
				IList<NLGElement> components = Components;
				if (components != null)
				{
					removed = components.Remove(textComponent);
				}
			}
			return removed;
		}

	    /**
	     * Removes all the child components from this element.
	     */
		public virtual void clearComponents()
		{
			IList<NLGElement> components = Components;
			if (components != null)
			{
				components.Clear();
			}
		}

	    /**
	     * Child elements of a <code>DocumentElement</code> are the components. This
	     * method is the same as calling <code>getComponents()</code>.
	     */
		public override IList<NLGElement> Children
		{
			get
			{
				return Components;
			}
	    /**
	     * Replaces the existing components with the supplied list of components.
	     * This is identical to calling:<br>
	     * <code><pre>
	     *     clearComponents();
	     *     addComponents(components);
	     * </pre></code>
	     * 
	     * @param components
	     */
		}



		public override string printTree(string indent)
		{
			string thisIndent = ReferenceEquals(indent, null) ? " |-" : indent + " |-"; //$NON-NLS-1$ //$NON-NLS-2$
			string childIndent = ReferenceEquals(indent, null) ? " | " : indent + " | "; //$NON-NLS-1$ //$NON-NLS-2$
			string lastIndent = ReferenceEquals(indent, null) ? " \\-" : indent + " \\-"; //$NON-NLS-1$ //$NON-NLS-2$
			string lastChildIndent = ReferenceEquals(indent, null) ? "   " : indent + "   "; //$NON-NLS-1$ //$NON-NLS-2$
			StringBuilder print = new StringBuilder();
			print.Append("DocumentElement: category=").Append(Category.ToString()); //$NON-NLS-1$

			string realisation = Realisation;
			if (!ReferenceEquals(realisation, null))
			{
				print.Append(" realisation=").Append(realisation); //$NON-NLS-1$
			}
			print.Append('\n');

			IList<NLGElement> children = Children;
			int length = children.Count - 1;
			int index = 0;

			if (children.Any())
			{
				for (index = 0; index < length; index++)
				{
					print.Append(thisIndent).Append(children[index].printTree(childIndent));
				}
				print.Append(lastIndent).Append(children[index].printTree(lastChildIndent));
			}
			return print.ToString();
		}
	}

}