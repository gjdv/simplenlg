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
using SimpleNLG.Main.format.english;
using SimpleNLG.Main.morphology.english;
using SimpleNLG.Main.orthography.english;
using SimpleNLG.Main.syntax.english;

namespace SimpleNLG.Main.realiser.english
{

	using TextFormatter = TextFormatter;
	using DocumentCategory = framework.DocumentCategory;
	using DocumentElement = framework.DocumentElement;
	using NLGElement = framework.NLGElement;
	using NLGModule = framework.NLGModule;
	using Lexicon = lexicon.Lexicon;
	using MorphologyProcessor = MorphologyProcessor;
	using OrthographyProcessor = OrthographyProcessor;
	using SyntaxProcessor = SyntaxProcessor;

    /**
     * @author D. Westwater, Data2Text Ltd
     * 
     */
	public class Realiser : NLGModule
	{

		private MorphologyProcessor morphology;
		private OrthographyProcessor orthography;
		private SyntaxProcessor syntax;
		private NLGModule formatter = null;
		private bool debug = false;

	    /**
	     * create a realiser (no lexicon)
	     */
		public Realiser() : base()
		{
			initialise();
		}

	    /**
	     * Create a realiser with a lexicon (should match lexicon used for
	     * NLGFactory)
	     * 
	     * @param lexicon
	     */
		public Realiser(Lexicon lexicon) : this()
		{
			Lexicon = lexicon;
		}

	    /**
	     * Set / get whether to separate premodifiers using a comma. If <code>true</code>,
	     * premodifiers will be comma-separated, as in <i>the long, dark road</i>.
	     * If <code>false</code>, they won't. <br/>
	     * <strong>Implementation note:</strong>: this method sets the relevant
	     * parameter in the
	     * {@link simplenlg.orthography.english.OrthographyProcessor}.
	     * 
	     * @param commaSepPremodifiers
	     *            the commaSepPremodifiers to set
	     */
		public virtual bool CommaSepPremodifiers
		{
			get
			{
				return orthography == null ? false : orthography.CommaSepPremodifiers;
			}
			set
			{
				if (orthography != null)
				{
					orthography.CommaSepPremodifiers = value;
				}
			}
		}

	    /**
	     * Set / get whether to separate cue phrases from the host phrase using a comma. If <code>true</code>,
	     * a comma will be inserted, as in <i>however, Bill arrived late</i>.
	     * If <code>false</code>, they won't. <br/>
	     * <strong>Implementation note:</strong>: this method sets the relevant
	     * parameter in the
	     * {@link simplenlg.orthography.english.OrthographyProcessor}.
	     * 
	     * @param commaSepcuephrase
	     */
		public virtual bool CommaSepCuephrase
		{
			get
			{
				return orthography == null ? false : orthography.CommaSepCuephrase;
			}
			set
			{
				if (orthography != null)
				{
					orthography.CommaSepCuephrase = value;
				}
			}
		}



		public override void initialise()
		{
			morphology = new MorphologyProcessor();
			morphology.initialise();
			orthography = new OrthographyProcessor();
			orthography.initialise();
			syntax = new SyntaxProcessor();
			syntax.initialise();
			formatter = new TextFormatter();
		    // AG: added call to initialise for formatter
			formatter.initialise();
		}

		public override NLGElement realise(NLGElement element)
		{

			StringBuilder debug = new StringBuilder();

			if (this.debug)
			{
				Console.WriteLine("INITIAL TREE\n"); //$NON-NLS-1$
				Console.WriteLine(element.printTree(null));
				debug.Append("INITIAL TREE<br/>");
				debug.Append(element.printTree("&nbsp;&nbsp;").Replace("\n", "<br/>"));
			}

			NLGElement postSyntax = syntax.realise(element);
			if (this.debug)
			{
				Console.WriteLine("<br/>POST-SYNTAX TREE<br/>"); //$NON-NLS-1$
				Console.WriteLine(postSyntax.printTree(null));
				debug.Append("<br/>POST-SYNTAX TREE<br/>");
				debug.Append(postSyntax.printTree("&nbsp;&nbsp;").Replace("\n", "<br/>"));
			}

			NLGElement postMorphology = morphology.realise(postSyntax);
			if (this.debug)
			{
				Console.WriteLine("\nPOST-MORPHOLOGY TREE\n"); //$NON-NLS-1$
				Console.WriteLine(postMorphology.printTree(null));
				debug.Append("<br/>POST-MORPHOLOGY TREE<br/>");
				debug.Append(postMorphology.printTree("&nbsp;&nbsp;").Replace("\n", "<br/>"));
			}

			NLGElement postOrthography = orthography.realise(postMorphology);
			if (this.debug)
			{
				Console.WriteLine("\nPOST-ORTHOGRAPHY TREE\n"); //$NON-NLS-1$
				Console.WriteLine(postOrthography.printTree(null));
				debug.Append("<br/>POST-ORTHOGRAPHY TREE<br/>");
				debug.Append(postOrthography.printTree("&nbsp;&nbsp;").Replace("\n", "<br/>"));
			}

			NLGElement postFormatter = null;
			if (formatter != null)
			{
				postFormatter = formatter.realise(postOrthography);
				if (this.debug)
				{
					Console.WriteLine("\nPOST-FORMATTER TREE\n"); //$NON-NLS-1$
					Console.WriteLine(postFormatter.printTree(null));
					debug.Append("<br/>POST-FORMATTER TREE<br/>");
					debug.Append(postFormatter.printTree("&nbsp;&nbsp;").Replace("\n", "<br/>"));
				}

			}
			else
			{
				postFormatter = postOrthography;
			}

			if (this.debug)
			{
				postFormatter.setFeature("debug", debug.ToString());
			}

			return postFormatter;
		}

	    /**
	     * Convenience class to realise any NLGElement as a sentence
	     * 
	     * @param element
	     * @return String realisation of the NLGElement
	     */
		public virtual string realiseSentence(NLGElement element)
		{
			NLGElement realised = null;
			if (element is DocumentElement)
			{
				realised = realise(element);
			}
			else
			{
				DocumentElement sentence = new DocumentElement(new DocumentCategory(DocumentCategory.DocumentCategoryEnum.SENTENCE), null);
				sentence.addComponent(element);
				realised = realise(sentence);
			}

			if (realised == null)
			{
				return null;
			}
			else
			{
				return realised.Realisation;
			}
		}

		public override IList<NLGElement> realise(IList<NLGElement> elements)
		{
			IList<NLGElement> realisedElements = new List<NLGElement>();
			if (null != elements)
			{
				foreach (NLGElement element in elements)
				{
					NLGElement realisedElement = realise(element);
					realisedElements.Add(realisedElement);
				}
			}
			return realisedElements;
		}

		public override Lexicon Lexicon
		{
			set
			{
				syntax.Lexicon = value;
				morphology.Lexicon = value;
				orthography.Lexicon = value;
			}
		}

		public virtual NLGModule Formatter
		{
			set
			{
				formatter = value;
			}
		}

		public virtual bool DebugMode
		{
			set
			{
				debug = value;
			}
		}
	}

}