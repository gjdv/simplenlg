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
 * Contributor(s): Ehud Reiter, Albert Gatt, Dave Wewstwater, Roman Kutlak, Margaret Mitchell, Saad Mahamood.
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
     * This processing module adds some simple plain HTML formatting to the
     * SimpleNLG output. This includes the following:
     * <ul>
     * <li>Adding the document title to the beginning of the text.</li>
     * <li>Adding section titles in the relevant places.</li>
     * <li>Adding appropriate new line breaks for ease-of-reading.</li>
     * <li>Indenting list items with ' * '.</li>
     * </ul>
     * </p>
     * 
     * @author D. Westwater, University of Aberdeen ~ for the TextFormatter; 
     * 		   <br />J Christie, University of Aberdeen ~ for HTMLFormatter 
     * @version 4.0 original TextFormatter Version; <br />version 0.0 HTMLFormatter
     * 
     */
	//public class TextFormatter extends NLGModule {
	public class HTMLFormatter : NLGModule
	{
    	// Modifications by James Christie to convert TextFormatter into a HTML Formatter
 

		public override void initialise()
		{
    		// Do nothing
		} // constructor

		public override NLGElement realise(NLGElement element)
		{ // realise a single element
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

					switch (((DocumentCategory) category).GetDocumentCategory())
					{

					case DocumentCategory.DocumentCategoryEnum.DOCUMENT:
						string title = element is DocumentElement ? ((DocumentElement) element).Title : null;
						realisation.Append("<h1>" + title + "</h1>");

						foreach (NLGElement eachComponent in components)
						{
							realisedComponent = realise(eachComponent);
							if (realisedComponent != null)
							{
								realisation.Append(realisedComponent.Realisation);
							}
						}

						break;

					case DocumentCategory.DocumentCategoryEnum.SECTION:
						title = element is DocumentElement ? ((DocumentElement) element).Title : null;

						if (!ReferenceEquals(title, null))
						{
							string sectionTitle = ((DocumentElement) element).Title;
							realisation.Append("<h2>" + sectionTitle + "</h2>");
						}

						foreach (NLGElement eachComponent in components)
						{
							realisedComponent = realise(eachComponent);
							if (realisedComponent != null)
							{
								realisation.Append(realisedComponent.Realisation);
							}
						}
						break;

					case DocumentCategory.DocumentCategoryEnum.LIST :
						realisation.Append("<ul>");
						foreach (NLGElement eachComponent in components)
						{
							realisedComponent = realise(eachComponent);
							if (realisedComponent != null)
							{
								realisation.Append(realisedComponent.Realisation);
							}
						}
						realisation.Append("</ul>");
						break;

					case DocumentCategory.DocumentCategoryEnum.ENUMERATED_LIST :
						realisation.Append("<ol>");
						foreach (NLGElement eachComponent in components)
						{
							realisedComponent = realise(eachComponent);
							if (realisedComponent != null)
							{
								realisation.Append(realisedComponent.Realisation);
							}
						}
						realisation.Append("</ol>");
						break;

					case DocumentCategory.DocumentCategoryEnum.PARAGRAPH :
						if (null != components && 0 < components.Count)
						{
							realisedComponent = realise(components[0]);
							if (realisedComponent != null)
							{
								realisation.Append("<p>");
								realisation.Append(realisedComponent.Realisation);
							}
							for (int i = 1; i < components.Count; i++)
							{
								if (realisedComponent != null)
								{
									realisation.Append(" ");
								}
								realisedComponent = realise(components[i]);
								if (realisedComponent != null)
								{
									realisation.Append(realisedComponent.Realisation);
								}
							}
							realisation.Append("</p>");
						}

						break;

					case DocumentCategory.DocumentCategoryEnum.SENTENCE :
						realisation.Append(element.Realisation);
						break;

					case DocumentCategory.DocumentCategoryEnum.LIST_ITEM :
						realisation.Append("<li>");

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

					    realisation.Append("</li>");

						break;

					}

				    // also need to check if element is a listelement (items can
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
		} // realise ~ single element

		public override IList<NLGElement> realise(IList<NLGElement> elements)
		{ // realise a list of elements
			IList<NLGElement> realisedList = new List<NLGElement>();

			if (elements != null)
			{
				foreach (NLGElement eachElement in elements)
				{
					realisedList.Add(realise(eachElement));
				}
			}
			return realisedList;
		} // realise ~ list of elements

	} // class

}