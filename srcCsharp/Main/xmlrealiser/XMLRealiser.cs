/**
 * Ported to C# by Gert-Jan de Vries
 */


using System;
using System.IO;
using SimpleNLG.Main.lexicon;

namespace SimpleNLG.Main.xmlrealiser
{

	using DocumentElement = framework.DocumentElement;
	using NLGElement = framework.NLGElement;
	using Lexicon = Lexicon;
	using NIHDBLexicon = NIHDBLexicon;
	using XMLLexicon = XMLLexicon;
	using Realiser = realiser.english.Realiser;

    /**
     * The Class XMLRealiser.
     * 
     * @author Christopher Howell Agfa Healthcare Corporation
     * @author Albert Gatt, University of Malta
     */
	public class XMLRealiser
	{

    	/** The lex db. */
		internal static string lexDB = null;

    	/** The lexicon. */
		internal static Lexicon lexicon = null;

    	/** The lexicon type. */
		internal static LexiconType? lexiconType = null;

    	/** The record. */
		internal static Recording record = null;

    	/**
	     * The Enum OpCode.
	     * 
	     */
	    /*
	     * The arg[0] is the op code. op codes are "realise", "setLexicon",
	     * "startRecording", "stopRecording" Usage is: realize <xml string> returns
	     * realised string. setLexicon (XML | NIHDB) <path to lexicon> returns "OK"
	     * or not. startRecording <path to recording directory> returns "OK" or not.
	     * stopRecording returns name of file which contains recording.
	     * Recordings can be used as regression tests. See simplenlg/test/xmlrealiser/Tester.java
	     */
		public enum OpCode
		{

		    /** The noop. */
			noop,
		    /** The realise. */
			realise,
		    /** The set lexicon. */
			setLexicon,
		    /** The start recording. */
			startRecording,
		    /** The stop recording. */
			stopRecording
		}

	    /**
	     * The Enum LexiconType.
	     */
		public enum LexiconType
		{

		    /** The DEFAULT. */
			DEFAULT,
		    /** The XML. */
			XML,
    		/** The NIHDB. */
			NIHDB
		}

	    /**
	     * The main method to perform realisation.
	     * 
	     * @param args
	     *            the args
	     * @return the string
	     * @throws XMLRealiserException
	     *             the xML realiser exception
	     */
		public static string main(object[] args)
		{

			if (args == null || args.Length == 0)
			{
				throw new XMLRealiserException("invalid args");
			}
			int argx = 0;
			string input = "";
			string output = "OK";
			string opCodeStr = (string) args[argx++];
			OpCode opCode;
			try
			{
				Enum.TryParse(opCodeStr, out opCode);
			}
			catch (ArgumentException)
			{
				throw new XMLRealiserException("invalid args");
			}
			switch (opCode)
			{
			case OpCode.realise:
				if (args.Length <= argx)
				{
					throw new XMLRealiserException("invalid args");
				}
				input = (string) args[argx++];
				StringReader reader = new StringReader(input);
				wrapper.RequestType request = getRequest(reader);
				output = realise(request.Document);

				break;
			case OpCode.setLexicon:
			{
				if (args.Length <= argx + 1)
				{
					throw new XMLRealiserException("invalid setLexicon args");
				}
				string lexTypeStr = (string) args[argx++];
				string lexFile = (string) args[argx++];
				LexiconType lexType;

				if (!Enum.TryParse(lexTypeStr, out lexType))
				{
					throw new XMLRealiserException("invalid args");
				}

				setLexicon(lexType, lexFile);
				break;
			}
			case OpCode.startRecording:
			{
				if (args.Length <= argx)
				{
					throw new XMLRealiserException("invalid args");
				}
				string path = (string) args[argx++];
				startRecording(path);
				break;
			}
			case OpCode.stopRecording:
				if (record != null)
				{
					output = record.GetRecordingFile();
					try
					{
						record.finish();
					}
					catch (Exception e)
					{
						throw new XMLRealiserException("xml writing error " + e.Message);
					}
				}
				break;
			case OpCode.noop:
				break;
			default:
				throw new XMLRealiserException("invalid op code " + opCodeStr);
			}

			if (opCode == OpCode.realise)
			{
			}

			return output;
		}

	    /**
	     * Sets the lexicon.
	     * 
	     * @param lexType
	     *            the lex type
	     * @param lexFile
	     *            the lex file
	     */
		public static void setLexicon(LexiconType lexType, string lexFile)
		{
			if (lexiconType != null && lexicon != null && lexType == lexiconType)
			{
				return; // done already
			}

			if (lexicon != null)
			{
				lexicon.close();
				lexicon = null;
				lexiconType = null;
			}

			if (lexType == LexiconType.XML)
			{
				lexicon = new XMLLexicon(lexFile);
			}
			else if (lexType == LexiconType.NIHDB)
			{
                lexicon = new NIHDBLexicon(lexFile);
			}
			else if (lexType == LexiconType.DEFAULT)
			{
				lexicon = Lexicon.DefaultLexicon;
			}

			lexiconType = lexType;
		}

	    /**
	     * Gets the request.
	     * 
	     * @param input
	     *            the input
	     * @return the request
	     * @throws XMLRealiserException
	     *             the xML realiser exception
	     */
		public static wrapper.RequestType getRequest(StringReader input)
		{
			wrapper.NLGSpec spec = UnWrapper.getNLGSpec(input);
			wrapper.RequestType request = spec.Request;
			if (request == null)
			{
				throw new XMLRealiserException("Must have Request element");
			}

			return request;
		}

	    /**
	     * Gets the recording.
	     * 
	     * @param input
	     *            the input
	     * @return the recording
	     * @throws XMLRealiserException
	     *             the xML realiser exception
	     */
		public static wrapper.RecordSet getRecording(StringReader input)
		{
			wrapper.NLGSpec spec = UnWrapper.getNLGSpec(input);
			wrapper.RecordSet recording = spec.Recording;
			if (recording == null)
			{
				throw new XMLRealiserException("Must have Recording element");
			}

			return recording;

		}

	    /**
	     * Realise.
	     * 
	     * @param wt
	     *            the wt
	     * @return the string
	     * @throws XMLRealiserException
	     *             the xML realiser exception
	     */
		public static string realise(wrapper.XmlDocumentElement wt)
		{
			string output = "";
			if (wt != null)
			{
				try
				{
					if (lexicon == null)
					{
						lexicon = Lexicon.DefaultLexicon;
					}
					UnWrapper w = new UnWrapper(lexicon);
					DocumentElement t = w.UnwrapDocumentElement(wt);
					if (t != null)
					{
						Realiser r = new Realiser(lexicon);
						r.initialise();

						NLGElement tr = r.realise(t);

						output = tr.Realisation;
					}

				}
				catch (Exception e)
				{
					throw new XMLRealiserException("NLG XMLRealiser Error", e);
				}
			}

			return output;
		}

	    /**
	     * Start recording.
	     * 
	     * @param path
	     *            the path
	     * @throws XMLRealiserException
	     *             the xML realiser exception
	     */
		public static void startRecording(string path)
		{
			if (record != null)
			{
				try
				{
					record.finish();
				}
				catch (Exception e)
				{
					throw new XMLRealiserException("NLG XMLRealiser Error", e);
				}
			}
			record = new Recording(path);
			try
			{
				record.start();
			}
			catch (IOException e)
			{
				throw new XMLRealiserException("NLG XMLRealiser Error", e);
			}
		}

	    /**
	     * Stop recording.
	     * 
	     * @return the string
	     * @throws XMLRealiserException
	     *             the xML realiser exception
	     */
		public static string stopRecording()
		{
			string file = "";
			if (record != null)
			{
				file = record.GetRecordingFile();
				try
				{
					record.finish();
				}
				catch (Exception e)
				{
					throw new XMLRealiserException("NLG XMLRealiser Error", e);
				}
			}

			return file;
		}
	}

}