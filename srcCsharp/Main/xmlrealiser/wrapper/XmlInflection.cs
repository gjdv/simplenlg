﻿

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
     * <p>Java class for inflection.
     * 
     * <p>The following schema fragment specifies the expected content contained within this class.
     * <p>
     * <pre>
     * &lt;simpleType name="inflection">
     *   &lt;restriction base="{http://www.w3.org/2001/XMLSchema}string">
     *     &lt;enumeration value="GRECO_LATIN_REGULAR"/>
     *     &lt;enumeration value="IRREGULAR"/>
     *     &lt;enumeration value="REGULAR"/>
     *     &lt;enumeration value="REGULAR_DOUBLE"/>
     *     &lt;enumeration value="UNCOUNT"/>
     *     &lt;enumeration value="INVARIANT"/>
     *   &lt;/restriction>
     * &lt;/simpleType>
     * </pre>
     * 
     */
	public enum XmlInflection
    {
		GRECO_LATIN_REGULAR,
		IRREGULAR,
		REGULAR,
		REGULAR_DOUBLE,
		UNCOUNT,
		INVARIANT
	}

}