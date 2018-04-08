using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emotiv;
using System.Threading;
using System.Web.Script.Serialization;
using Fleck;



namespace Epoc_harvister
{
    public class WSServer
    {
        public WebSocketServer ws;
        List<IWebSocketConnection> allSockets;
        public WSServer(string arg)
        {
            string host = arg;
            Console.WriteLine("creating server with host: " + host);
            ws = new WebSocketServer(host);
            allSockets = new List<IWebSocketConnection>();
            
            ws.Start(socket =>
            {
                socket.OnOpen = () =>
                {
                    Console.WriteLine("Open!");
                    allSockets.Add(socket);
                };
                socket.OnClose = () =>
                {
                    Console.WriteLine("Close!");
                    allSockets.Remove(socket);
                };
                socket.OnMessage = message =>
                {
                    Console.WriteLine(message);
                    allSockets.ToList().ForEach(s => s.Send("Echo: " + message));
                };
            });
        }

        public void Broadcast(string msg)
        {
            // Console.WriteLine("broadcasting frame json with length " + msg.Length);
            // Console.WriteLine(msg);
            foreach (var socket in allSockets.ToList())
            {
                //Console.WriteLine(msg);
                socket.Send(msg);
            }
            msg = "";
        }
    }








}