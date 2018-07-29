﻿using System.Collections.Generic;

//
// This file was generated by the JavaTM Architecture for XML Binding(JAXB) Reference Implementation, v2.2.4-2 
// See <a href="http://java.sun.com/xml/jaxb">http://java.sun.com/xml/jaxb</a> 
// Any modifications to this file will be lost upon recompilation of the source schema. 
// Generated on: 2014.02.04 at 09:14:07 AM EST 
//

/*
 * Ported to C# by Gert-Jan de Vries
 */

namespace SimpleNLG.Main.xmlrealiser.wrapper
    {

    /**
     * <p>Java class for SPhraseSpec complex type.
     * 
     * <p>The following schema fragment specifies the expected content contained within this class.
     * 
     * <pre>
     * &lt;complexType name="SPhraseSpec">
     *   &lt;complexContent>
     *     &lt;extension base="{http://simplenlg.googlecode.com/svn/trunk/res/xml}PhraseElement">
     *       &lt;sequence>
     *         &lt;element name="cuePhrase" type="{http://simplenlg.googlecode.com/svn/trunk/res/xml}NLGElement" minOccurs="0"/>
     *         &lt;element name="subj" type="{http://simplenlg.googlecode.com/svn/trunk/res/xml}NLGElement" maxOccurs="unbounded" minOccurs="0"/>
     *         &lt;element name="vp" type="{http://simplenlg.googlecode.com/svn/trunk/res/xml}NLGElement"/>
     *       &lt;/sequence>
     *       &lt;attGroup ref="{http://simplenlg.googlecode.com/svn/trunk/res/xml}sPhraseAtts"/>
     *     &lt;/extension>
     *   &lt;/complexContent>
     * &lt;/complexType>
     * </pre>
     * 
     * 
     */
	public class XmlSPhraseSpec : XmlPhraseElement
	{

		protected internal XmlNLGElement cuePhrase;
		protected internal IList<XmlNLGElement> subj;
		protected internal XmlNLGElement vp;
		protected internal bool? aggregateauxiliary;
		protected internal XmlClauseStatus? clausestatus;
		protected internal string complementiser;
		protected internal XmlForm? form;
		protected internal XmlInterrogativeType? interrogativetype;
		protected internal string modal;
		protected internal bool? negated;
		protected internal bool? passive;
		protected internal bool? perfect;
		protected internal XmlPerson? person;
		protected internal bool? progressive;
		protected internal bool? suppressgenitiveingerund;
		protected internal bool? supressedcomplementiser;
		protected internal XmlTense? tense;


        /**
         * Set / get the value of the cuePhrase property.
         * 
         * @param value
         *     allowed object is
         *     {@link XmlNLGElement }
         *     
         */
		public virtual XmlNLGElement CuePhrase
		{
			get
			{
				return cuePhrase;
			}
			set
			{
				cuePhrase = value;
			}
		}
        /**
         * Gets the value of the subj property.
         * 
         * <p>
         * This accessor method returns a reference to the live list,
         * not a snapshot. Therefore any modification you make to the
         * returned list will be present inside the JAXB object.
         * This is why there is not a <CODE>set</CODE> method for the subj property.
         * 
         * <p>
         * For example, to add a new item, do as follows:
         * <pre>
         *    getSubj().add(newItem);
         * </pre>
         * 
         * 
         * <p>
         * Objects of the following type(s) are allowed in the list
         * {@link XmlNLGElement }
         * 
         * 
         */
		public virtual IList<XmlNLGElement> Subj
		{
			get
			{
				if (subj == null)
				{
					subj = new List<XmlNLGElement>();
				}
				return subj;
			}
		}


        /**
         * Set / get the value of the vp property.
         * 
         * @param value
         *     allowed object is
         *     {@link XmlNLGElement }
         *     
         */
		public virtual XmlNLGElement Vp
		{
			get
			{
				return vp;
			}
			set
			{
				vp = value;
			}
		}

        /**
         * Set / get the value of the aggregateauxiliary property.
         * 
         * @param value
         *     allowed object is
         *     {@link Boolean }
         *     
         */
		public virtual bool? AGGREGATEAUXILIARY
		{
			get
			{
				return aggregateauxiliary;
			}
			set
			{
				aggregateauxiliary = value;
			}
		}

        /**
         * Set / get the value of the clausestatus property.
         * 
         * @param value
         *     allowed object is
         *     {@link XmlClauseStatus }
         *     
         */
		public virtual XmlClauseStatus? CLAUSESTATUS
		{
			get
			{
				return clausestatus;
			}
			set
			{
				clausestatus = value;
			}
		}

        /**
         * Set / get the value of the complementiser property.
         * 
         * @param value
         *     allowed object is
         *     {@link String }
         *     
         */
		public virtual string COMPLEMENTISER
		{
			get
			{
				return complementiser;
			}
			set
			{
				complementiser = value;
			}
		}

        /**
         * Set / get the value of the form property.
         * 
         * @param value
         *     allowed object is
         *     {@link XmlForm }
         *     
         */
		public virtual XmlForm? FORM
		{
			get
			{
				return form;
			}
			set
			{
				form = value;
			}
		}

        /**
         * Set / get the value of the interrogativetype property.
         * 
         * @param value
         *     allowed object is
         *     {@link XmlInterrogativeType }
         *     
         */
		public virtual XmlInterrogativeType? INTERROGATIVETYPE
		{
			get
			{
				return interrogativetype;
			}
			set
			{
				interrogativetype = value;
			}
		}
        
        /**
         * Set / get the value of the modal property.
         * 
         * @param value
         *     allowed object is
         *     {@link String }
         *     
         */
		public virtual string MODAL
		{
			get
			{
				return modal;
			}
			set
			{
				modal = value;
			}
		}
            
        /**
         * Set / get the value of the negated property.
         * 
         * @param value
         *     allowed object is
         *     {@link Boolean }
         *     
         */
		public virtual bool? NEGATED
		{
			get
			{
				return negated;
			}
			set
			{
				negated = value;
			}
		}
            
        /**
         * Set / get the value of the passive property.
         * 
         * @param value
         *     allowed object is
         *     {@link Boolean }
         *     
         */
		public virtual bool? PASSIVE
		{
			get
			{
				return passive;
			}
			set
			{
				passive = value;
			}
		}
            
        /**
         * Set / get the value of the perfect property.
         * 
         * @param value
         *     allowed object is
         *     {@link Boolean }
         *     
         */
		public virtual bool? PERFECT
		{
			get
			{
				return perfect;
			}
			set
			{
				perfect = value;
			}
		}
            
        /**
         * Set / get the value of the person property.
         * 
         * @param value
         *     allowed object is
         *     {@link XmlPerson }
         *     
         */
		public virtual XmlPerson? PERSON
		{
			get
			{
				return person;
			}
			set
			{
				person = value;
			}
		}

        /**
         * Set / get the value of the progressive property.
         * 
         * @param value
         *     allowed object is
         *     {@link Boolean }
         *     
         */
		public virtual bool? PROGRESSIVE
		{
			get
			{
				return progressive;
			}
			set
			{
				progressive = value;
			}
		}
            
        /**
         * Set / get the value of the suppressgenitiveingerund property.
         * 
         * @param value
         *     allowed object is
         *     {@link Boolean }
         *     
         */
		public virtual bool? SUPPRESSGENITIVEINGERUND
		{
			get
			{
				return suppressgenitiveingerund;
			}
			set
			{
				suppressgenitiveingerund = value;
			}
		}

        /**
         * Set / get the value of the supressedcomplementiser property.
         * 
         * @param value
         *     allowed object is
         *     {@link Boolean }
         *     
         */
		public virtual bool? SUPRESSEDCOMPLEMENTISER
		{
			get
			{
				return supressedcomplementiser;
			}
			set
			{
				supressedcomplementiser = value;
			}
		}

        /**
         * Set / get the value of the tense property.
         * 
         * @param value
         *     allowed object is
         *     {@link XmlTense }
         *     
         */
		public virtual XmlTense? TENSE
		{
			get
			{
				return tense;
			}
			set
			{
				tense = value;
			}
		}
        
	}

}