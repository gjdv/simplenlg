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

namespace SimpleNLG.Main.format.english
{

	using CoordinatedPhraseElement = framework.CoordinatedPhraseElement;
	using DocumentCategory = framework.DocumentCategory;
	using DocumentElement = framework.DocumentElement;
	using ElementCategory = framework.ElementCategory;
	using ListElement = framework.ListElement;
	using NLGElement = framework.NLGElement;
	using NLGModule = framework.NLGModule;
	using StringElement = framework.StringElement;

    /**
     * <p>
     * This processing module adds some simple plain text formatting to the
     * SimpleNLG output. This includes the following:
     * <ul>
     * <li>Adding the document title to the beginning of the text.</li>
     * <li>Adding section titles in the relevant places.</li>
     * <li>Adding appropriate new line breaks for ease-of-reading.</li>
     * <li>Adding list items with ' * '.</li>
     * <li>Adding numbers for enumerated lists (e.g., "1.1 - ", "1.2 - ", etc.)</li>
     * </ul>
     * </p>
     * 
     * @author D. Westwater, University of Aberdeen.
     * @version 4.0
     * 
     */
	public class TextFormatter : NLGModule
	{

		private static NumberedPrefix numberedPrefix = new NumberedPrefix();

		public override void initialise()
		{
    		// Do nothing
		}

		public override NLGElement realise(NLGElement element)
		{
			NLGElement realisedComponent = null;
			StringBuilder realisation = new StringBuilder();

			if (element != null)
			{
				ElementCategory category = element.Category;
				IList<NLGElement> components = element.Children;

			    //NB: The order of the if-statements below is important!
			    
			    // check if this is a canned text first
				if (element is StringElement)
				{
					realisation.Append(element.Realisation);

				}
				else if (category is DocumentCategory)
				{ // && element is DocumentElement
					string title = element is DocumentElement ? ((DocumentElement) element).Title : null;


    				// String title = ((DocumentElement) element).getTitle();
					switch (((DocumentCategory) category).GetDocumentCategory())
					{

					case DocumentCategory.DocumentCategoryEnum.DOCUMENT:
						appendTitle(realisation, title, 2);
						realiseSubComponents(realisation, components);
						break;
					case DocumentCategory.DocumentCategoryEnum.SECTION:
						appendTitle(realisation, title, 1);
						realiseSubComponents(realisation, components);
						break;
					case DocumentCategory.DocumentCategoryEnum.LIST:
						realiseSubComponents(realisation, components);
						break;

					case DocumentCategory.DocumentCategoryEnum.ENUMERATED_LIST:
						numberedPrefix.upALevel();
						if (!ReferenceEquals(title, null))
						{
							realisation.Append(title).Append('\n');
						}

						if (null != components && 0 < components.Count)
						{

							realisedComponent = realise(components[0]);
							if (realisedComponent != null)
							{
								realisation.Append(realisedComponent.Realisation);
							}
							for (int i = 1; i < components.Count; i++)
							{
								if (realisedComponent != null && !realisedComponent.Realisation.EndsWith("\n"))
								{
									realisation.Append(' ');
								}
								if (components[i].Parent.Category == DocumentCategory.DocumentCategoryEnum.ENUMERATED_LIST)
								{
									numberedPrefix.increment();
								}
								realisedComponent = realise(components[i]);
								if (realisedComponent != null)
								{
									realisation.Append(realisedComponent.Realisation);
								}
							}
						}

						numberedPrefix.downALevel();
						break;

					case DocumentCategory.DocumentCategoryEnum.PARAGRAPH:
						if (null != components && 0 < components.Count)
						{
							realisedComponent = realise(components[0]);
							if (realisedComponent != null)
							{
								realisation.Append(realisedComponent.Realisation);
							}
							for (int i = 1; i < components.Count; i++)
							{
								if (realisedComponent != null)
								{
									realisation.Append(' ');
								}
								realisedComponent = realise(components[i]);
								if (realisedComponent != null)
								{
									realisation.Append(realisedComponent.Realisation);
								}
							}
						}
						realisation.Append("\n\n");
						break;

					case DocumentCategory.DocumentCategoryEnum.SENTENCE:
						realisation.Append(element.Realisation);
						break;

					case DocumentCategory.DocumentCategoryEnum.LIST_ITEM:
						if (element.Parent != null)
						{
							if (element.Parent.Category == DocumentCategory.DocumentCategoryEnum.LIST)
							{
								realisation.Append(" * ");
							}
							else if (element.Parent.Category == DocumentCategory.DocumentCategoryEnum.ENUMERATED_LIST)
							{
								realisation.Append(numberedPrefix.Prefix + " - ");
							}
						}

					    for (int index = 0; index < components.Count; index++)
					    {
					        NLGElement eachComponent = components[index];
					        realisedComponent = realise(eachComponent);

					        if (realisedComponent != null)
					        {
					            realisation.Append(realisedComponent.Realisation);

					            if (index < components.Count - 1)
					            {
					                realisation.Append(' ');
					            }
					        }
					    }

					    //finally, append newline
						realisation.Append("\n");
						break;
					}

		    		// also need to check if element is a ListElement (items can
		    		// have embedded lists post-orthography) or a coordinate
				}
				else if (element is ListElement || element is CoordinatedPhraseElement)
				{
					foreach (NLGElement eachComponent in components)
					{
						realisedComponent = realise(eachComponent);
						if (realisedComponent != null)
						{
							realisation.Append(realisedComponent.Realisation).Append(' ');
						}
					}
				}
			}

			return new StringElement(realisation.ToString());
		}

	    /**
	     * realiseSubComponents -- Realises subcomponents iteratively.
	     * @param realisation -- The current realisation StringBuffer.
	     * @param components -- The components to realise.
	     */
		private void realiseSubComponents(StringBuilder realisation, IList<NLGElement> components)
		{
			NLGElement realisedComponent;
			foreach (NLGElement eachComponent in components)
			{
				realisedComponent = realise(eachComponent);
				if (realisedComponent != null)
				{
					realisation.Append(realisedComponent.Realisation);
				}
			}
		}

	    /**
	     * appendTitle -- Appends document or section title to the realised document.
	     * @param realisation -- The current realisation StringBuffer.
	     * @param title -- The title to append.
	     * @param numberOfLineBreaksAfterTitle -- Number of line breaks to append.
	     */
		private void appendTitle(StringBuilder realisation, string title, int numberOfLineBreaksAfterTitle)
		{
			if (!ReferenceEquals(title, null) && title.Length > 0)
			{
				realisation.Append(title);
				for (int i = 0; i < numberOfLineBreaksAfterTitle; i++)
				{
					realisation.Append("\n");
				}
			}
		}

		public override IList<NLGElement> realise(IList<NLGElement> elements)
		{
			IList<NLGElement> realisedList = new List<NLGElement>();

			if (elements != null)
			{
				foreach (NLGElement eachElement in elements)
				{
					realisedList.Add(realise(eachElement));
				}
			}
			return realisedList;
		}
	}

}