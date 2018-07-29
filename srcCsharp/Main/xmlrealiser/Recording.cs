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
using System.IO;
using System.Xml.Serialization;

namespace SimpleNLG.Main.xmlrealiser
{


	using DocumentRealisation = wrapper.DocumentRealisation;
	using NLGSpec = wrapper.NLGSpec;
	using RecordSet = wrapper.RecordSet;

    /**
     * A recording is a utility class that holds xml objects for testing the
     * xmlrealiser.
     * 
     * @author Christopher Howell Agfa Healthcare Corporation
     * @author Albert Gatt, University of Malta
     * 
     */
	public class Recording
	{

	    /** The recording on. */
		internal bool recordingOn = false;

	    /** The recording folder. */
		internal string recordingFolder;

	    /** The record. */
		internal RecordSet record = null;

	    /** The recording file. */
		internal string recordingFileName;

	    /**
	     * Instantiate a Recording from an XML file. Recordings can contain multiple
	     * records, each of which represents a single element to be realised.
	     * 
	     * @param directoryPath
	     *            the path to the file
	     */
		public Recording(string directoryPath)
		{
			recordingFolder = directoryPath;
		}

	    /**
	     * Recording on.
	     * 
	     * @return true, if successful
	     */
		public virtual bool RecordingOn()
		{
			return recordingOn;
		}

	    /**
	     * Gets the recording file.
	     * 
	     * @return the string
	     */
		public virtual string GetRecordingFile()
		{
			if (recordingOn)
			{
			    return recordingFileName;
			}
			else
			{
				return "";
			}
		}
	    /**
	     * Start.
	     * 
	     * @throws IOException
	     *             Signals that an I/O exception has occurred.
	     */
		public virtual void start()
		{

			if (recordingFolder.Length == 0 || recordingOn)
			{
				return;
			}

		    if (!Directory.Exists(recordingFolder))
		    {
		        Directory.CreateDirectory(recordingFolder);
		    }

		    recordingFileName = recordingFolder + '/' + "xmlrealiser" + ".xml";
			recordingOn = true;
			record = new RecordSet();
		}


	    /**
	     * Adds a record to this recording.
	     * 
	     * @param input
	     *            the DocumentElement in this record
	     * @param output
	     *            the realisation
	     */
		public virtual void addRecord(wrapper.XmlDocumentElement input, string output)
		{
			if (!recordingOn)
			{
				return;
			}
			DocumentRealisation t = new DocumentRealisation();
			int? testNumber = record.Record.Count + 1;
			string testName = "TEST_" + testNumber.ToString();
			t.Name = testName;
			t.Document = input;
			t.Realisation = output;
			record.Record.Add(t);
		}

	    /**
	     * Ends processing for this recording and writes it to an XML file.
	     * 
	     * @throws JAXBException
	     *             the jAXB exception
	     * @throws IOException
	     *             Signals that an I/O exception has occurred.
	     * @throws TransformerException
	     *             the transformer exception
	     */
		public virtual void finish()
		{
			if (!recordingOn)
			{
				return;
			}

			recordingOn = false;
			FileStream os = new FileStream(recordingFileName, FileMode.Create, FileAccess.Write);
//			os.Channel.truncate(0);
			writeRecording(record, os);
		}

	    /**
	     * Write recording.
	     * 
	     * @param record
	     *            the record
	     * @param os
	     *            the os
	     * @throws JAXBException
	     *             the jAXB exception
	     * @throws IOException
	     *             Signals that an I/O exception has occurred.
	     * @throws TransformerException
	     *             the transformer exception
	     */
		public static void writeRecording(RecordSet record, Stream os)
		{

			XmlSerializer serializer = new XmlSerializer(typeof(NLGSpec));
			
			NLGSpec nlg = new NLGSpec();
			nlg.Recording = record;

			StringWriter osTemp = new StringWriter();
			serializer.Serialize(osTemp,nlg);

throw new NotImplementedException("Not yet ported");
/*
		    // Prettify it.
			Source xmlInput = new StreamSource(new StringReader(osTemp.ToString()));
			StreamResult xmlOutput = new StreamResult(new System.IO.StreamWriter(os, Encoding.UTF8));
			Transformer transformer = TransformerFactory.newInstance().newTransformer();
			if (transformer != null)
			{
				transformer.setOutputProperty(OutputKeys.INDENT, "yes");
				transformer.setOutputProperty("{http://xml.apache.org/xslt}indent-amount", "2");
				transformer.transform(xmlInput, xmlOutput);
			}
*/
		}
	}

}