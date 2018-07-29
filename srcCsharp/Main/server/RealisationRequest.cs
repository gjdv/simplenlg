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
 * Contributor(s): Ehud Reiter, Albert Gatt, Dave Wewstwater,
 * Roman Kutlak, Margaret Mitchell.
 *
 * Ported to C# by Gert-Jan de Vries
 */


using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace SimpleNLG.Main.server
{

	using XMLRealiser = xmlrealiser.XMLRealiser;
	using XMLRealiserException = xmlrealiser.XMLRealiserException;

    /**
     * This class handles one realisation request.
     * @author Roman Kutlak
     *
     * The object will parse the xml from the client socket and return 
     * a surface realisation of the xml structure.
     * 
     * If an exception occurs, the request attempts to inform the client
     * by sending a string that starts with "Exception: " and continues
     * with the message of the exception. Sending the error might fail
     * (for example, if the client disconnected).
     * 
     * The program implements the "standard" socket protocol:
     * each message is preceded with an integer indicating the
     * length of the message (int is 4 bytes).
     */
	public class RealisationRequest
	{
	    private Socket socket;

	    private static bool DEBUG = SimpleServer.DEBUG;

		public RealisationRequest(Socket s)
		{
			socket = s;
		}

		public void Run()
		{
			if (null == socket)
			{
				return;
			}

			if (DEBUG)
			{
				Console.WriteLine("Client connected from " + socket.RemoteEndPoint);
			}

			try
			{

				// read the message length
				int msgLen = socket.ReceiveBufferSize;

				// create a buffer
				byte[] data = new byte[msgLen];
				// read the entire message (blocks until complete)
				socket.Receive(data, 0, data.Length, SocketFlags.None);

				if (data.Length < 1)
				{
					throw new Exception("Client did not send data.");
				}

				// now convert the raw bytes to utf-8
			    string tmp = Encoding.UTF8.GetString(data);
				StringReader reader = new StringReader(tmp);

				// get the realisation
				string result = doRealisation(reader).Trim();

				// convert the string to raw bytes
				sbyte[] tmp2 = result.GetBytes(Encoding.UTF8);

                // write the length
                socket.SendBufferSize = tmp2.Length;
				// write the data
				socket.Send((byte[])(Array) tmp2);

				if (DEBUG)
				{
					string text = "The following realisation was sent to client:";
					Console.WriteLine(text + "\n\t" + result);
				}

			}
			catch (XMLRealiserException e)
			{
				Console.WriteLine(e.ToString());
				Console.Write(e.StackTrace);
				try
				{
					// attempt to send the error message to the client
					sbyte[] tmp = ("Exception: " + e.Message).GetBytes(Encoding.UTF8);
				    socket.SendBufferSize = tmp.Length;
				    socket.Send((byte[])(Array)tmp);
                }
				catch (IOException)
				{
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
				Console.Write(e.StackTrace);
				try
				{
					// attempt to send the error message to the client
					sbyte[] tmp = ("Exception: " + e.Message).GetBytes(Encoding.UTF8);
				    socket.SendBufferSize = tmp.Length;
				    socket.Send((byte[])(Array)tmp);
                }
				catch (IOException)
				{
				}
			}
			finally
			{
				try
				{
					socket.Close();
					socket = null;
				}
				catch (IOException)
				{
					Console.Error.WriteLine("Could not close client socket!");
				}
			}
		}

		protected virtual string doRealisation(StringReader inputReader)
		{
			xmlrealiser.wrapper.RequestType request = XMLRealiser.getRequest(inputReader);
			string output = XMLRealiser.realise(request.Document);
			return output;
		}
	}

}