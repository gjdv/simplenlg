﻿//
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
     * <p>Java class for anonymous complex type.
     * 
     * <p>The following schema fragment specifies the expected content contained within this class.
     * 
     * <pre>
     * &lt;complexType>
     *   &lt;complexContent>
     *     &lt;restriction base="{http://www.w3.org/2001/XMLSchema}anyType">
     *       &lt;choice>
     *         &lt;element name="Request" type="{http://simplenlg.googlecode.com/svn/trunk/res/xml}RequestType"/>
     *         &lt;element name="Recording" type="{http://simplenlg.googlecode.com/svn/trunk/res/xml}RecordSet"/>
     *       &lt;/choice>
     *     &lt;/restriction>
     *   &lt;/complexContent>
     * &lt;/complexType>
     * </pre>
     * 
     * 
     */
	public class NLGSpec
	{
		protected internal RequestType request;
		protected internal RecordSet recording;


	    /**
         * Set / get the value of the request property.
         * 
         * @param value
         *     allowed object is
         *     {@link RequestType }
         *     
         */
		public virtual RequestType Request
		{
			get
			{
				return request;
			}
			set
			{
				request = value;
			}
		}
        
        /**
         * Set / get the value of the recording property.
         * 
         * @param value
         *     allowed object is
         *     {@link RecordSet }
         *     
         */
		public virtual RecordSet Recording
		{
			get
			{
				return recording;
			}
			set
			{
				recording = value;
			}
		}
        
	}

}