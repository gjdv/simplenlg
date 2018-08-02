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
 * Roman Kutlak, Margaret Mitchell, Saad Mahamood.
 *
 * Ported to C# by Gert-Jan de Vries
 */

using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using NUnit.Framework;
using SimpleNLG.Main.server;
using Assert = NUnit.Framework.Assert;

namespace SimpleNLG.Test.server
{
    /**
     * Tests for simplenlg server
     *
     * @author Roman Kutlak
     */
    [TestFixture]
    public class ServerTest
    {
        private SimpleServer serverapp;
        private TcpListener socket;

        [SetUp]
        public virtual void setUp()
        {
            try
            {
                IPAddress ipAddress = IPAddress.Parse("127.0.0.1"); //localhost
                socket = new TcpListener(ipAddress, 8888);
                serverapp = new SimpleServer(socket);
                Thread server = new Thread(new ThreadStart(serverapp.Run));
                server.IsBackground = true;
                server.Start();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
            }
        }

        [OneTimeTearDown]
        public virtual void tearDown()
        {
            serverapp.terminate();
        }

        [Test]
        public virtual void testServer()
        {
            Assert.IsNotNull(serverapp);

            string expected = "Put the piano and the drum into the truck.";

            SimpleClient clientApp = new SimpleClient();

            int port = ((IPEndPoint) socket.LocalEndpoint).Port;
            string result = clientApp.run("localhost", port);

            // Shutdown serverapp:
            serverapp.terminate();

            Assert.AreEqual(expected, result);
        }
    }
}