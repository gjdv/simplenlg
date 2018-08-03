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
 * Contributor(s): Ehud Reiter, Albert Gatt, Dave Westwater,
 * Roman Kutlak, Margaret Mitchell, and Saad Mahamood.
 *
 * Ported to C# by Gert-Jan de Vries
 */

using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using SimpleNLG.Main.xmlrealiser;

namespace SimpleNLG.Main.server
{
    /**
     * SimpleServer is a program that realises xml requests.
     * 
     * @author Roman Kutlak.
     *
     * The program listens on a socket for client connections. 
     * When a client connects, the server creates an instance 
     * of RealisationRequest that serves the client.
     * 
     * The RealisationRequest parses the xml structure and
     * sends back corresponding surface string.
     * 
     * The server port can be specified as the first parameter
     * of the program; 50007 is used by default.
     * 
     * Typing exit will terminate the server.
     */
	public class SimpleServer
	{

		private TcpListener serverSocket;

        /**
         * Set to true to enable printing debug messages.
         */
		internal static bool DEBUG = false;

        /**
         * This path should be replaced by the path to the specialist lexicon.
         * If there is an entry for DB_FILENAME in lexicon.properties, that path
         * will be searched for the lexicon file. Otherwise, the path below will
         * be used.
         */
        static string lexiconPath = "Resources/NIHLexicon/lexAccess2013.sqlite";
	    static XMLRealiser.LexiconType lexiconType = XMLRealiser.LexiconType.NIHDB_SQLITE;

		// control the run loop
		private bool isActive = true;

        /**
         * Construct a new server.
         * 
         * @param port
         *      the port on which to listen
         *      
         * @throws IOException
         */
		public SimpleServer(int port)
		{
			startServer(new TcpListener(port));
		}

        /**
         * Construct a server with a pre-allocated socket.
         * @param socket
         * @throws IOException
         */
		public SimpleServer(TcpListener socket)
		{
			startServer(socket);
		}


	    /**
	     * startServer -- Start's the SimpleServer with a created ServerSocket.
	     * @param socket -- The socket for the server to use.
	     * @throws IOException
	     * @throws SocketException
	     */
		private void startServer(TcpListener socket)
		{
			serverSocket = socket;
		    serverSocket.Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, 1);
            serverSocket.Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 0);

		    int port = ((IPEndPoint) serverSocket.LocalEndpoint).Port;

            Console.WriteLine("Port Number used by Server is: " + port);

			// try to read the lexicon path from lexicon.properties file
			try
			{

				Properties prop = new Properties();
				prop.load("Resources/lexicon.properties");

				string dbFile = prop.getProperty("DB_FILENAME");

				if (null != dbFile)
				{
					lexiconPath = dbFile;
				}
				else
				{
					throw new Exception("No DB_FILENAME in lexicon.properties");
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
				Console.Write(e.StackTrace);
			}

			Console.WriteLine("Server is using the following lexicon: " + lexiconPath);

			XMLRealiser.setLexicon(lexiconType, lexiconPath);
		}

		internal static void print(object o)
		{
			Console.WriteLine(o);
		}

        /**
         * Terminate the server. The server can be started
         * again by invoking the <code>run()</code> method.
         */
		public virtual void terminate()
		{
			isActive = false;
		}


        /**
         * Start the server.
         * 
         * The server will listen on the port specified at construction
         * until terminated by calling the <code>terminate()</code> method.
         * 
         * Note that the <code>exit()</code> and <code>exit(int)</code>
         * methods will terminate the program by calling System.exit().
         */
		public virtual void Run()
		{
			try
			{
				while (isActive)
				{
					try
					{
						if (DEBUG)
						{
							Console.WriteLine("Waiting for client on  " + serverSocket.LocalEndpoint + "...");
						}

						Socket clientSocket = serverSocket.AcceptSocket();
						handleClient(clientSocket);
					}
					catch (TimeoutException)
					{
						Console.Error.WriteLine("Socket timed out!");
						break;
					}
					catch (IOException e)
					{
						Console.WriteLine(e.ToString());
						Console.Write(e.StackTrace);
						break;
					}
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
				Console.Write(e.StackTrace);
			}
			finally
			{
				try
				{
					serverSocket.Stop();
				}
				catch (Exception)
				{
					Console.Error.WriteLine("Could not close socket!");
				}
			}
		}

        /**
         * Handle the incoming client connection by constructing 
         * a <code>RealisationRequest</code> and starting it in a thread.
         * 
         * @param socket
         *          the socket on which the client connected
         */
		protected internal virtual void handleClient(Socket socket)
		{
			if (null == socket)
			{
				return;
			}

			Thread request = new Thread(new ThreadStart(new RealisationRequest(socket).Run));
			request.IsBackground = true;
			request.Start();
		}

        /**
         * Perform shutdown routines.
         */
		public virtual void shutdown()
		{
			lock (this)
			{
				Console.WriteLine("Server shutting down.");
        
				terminate();
				// cleanup...like close log, etc.
			}
		}
        /**
         * Exit the program without error.
         */
		public virtual void exit()
		{
			lock (this)
			{
				exit(0);
			}
		}
        /**
         * Exit the program signalling an error code.
         * 
         * @param code
         *          Error code; 0 means no error
         */
		public virtual void exit(int code)
		{
			lock (this)
			{
				Environment.Exit(code);
			}
		}
        /**
         * The main method that starts the server.
         * 
         * The program takes one optional parameter,
         * which is the port number on which to listen.
         * The default value is 50007.
         * 
         * Once the program starts, it can be terminated
         * by typing the command 'exit'
         * 
         * @param args
         *          Program arguments
         */
		public static void Main(string[] args)
		{
			int port;
			try
			{
				port = int.Parse(args[0]);
			}
			catch (Exception)
			{
				port = 50007;
			}

			try
			{
				SimpleServer serverapp = new SimpleServer(port);

				Thread server = new Thread(new ThreadStart(serverapp.Run));
				server.IsBackground = true;
				server.Start();

                // allow the user to terminate the server by typing "exit"
                StreamReader br = new StreamReader(Console.OpenStandardInput(), Console.InputEncoding);
				StreamWriter bw = new StreamWriter(Console.OpenStandardOutput(), Console.OutputEncoding);

				while (true)
				{
					try
					{
						bw.Write(":>");
						bw.Flush();
						string input = br.ReadLine();

						if (null != input && string.Compare(input, "exit", StringComparison.OrdinalIgnoreCase) == 0)
						{
							serverapp.shutdown();
							serverapp.exit();
						}
					}
					catch (IOException e)
					{
						serverapp.shutdown();
						Console.WriteLine(e.ToString());
						Console.Write(e.StackTrace);
						serverapp.exit(-1);
					}
				}
			}
			catch (IOException e)
			{
				Console.WriteLine(e.ToString());
				Console.Write(e.StackTrace);
			}
		}
	}

}