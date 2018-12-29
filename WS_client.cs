using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebSocketSharp;

namespace Epoc_harvister
{
    public class MyWebSocket
    {
        public WebSocket ws;
        List<WebSocket> allSockets;
        public MyWebSocket() //string[] args)
        {
            //string[] myStr = { Program.myForm.URLtoSend.Text };
            string myStr = My_Program.myForm.textBox10.Text;
            ws = new WebSocket(myStr);
            allSockets = new List<WebSocket>();
            ws.OnOpen += (sender, e) => {
                allSockets.Add(ws);
                My_Program.SRV = true;
                My_Program.myForm.textBox11.Text = "Open";
            };
            ws.OnClose += (sender, e) => {
                My_Program.SRV = false;
                My_Program.myForm.textBox11.Text = "Closed";
            };

            ws.OnMessage += (sender, e) =>
            {
                My_Program.UpdateMsgBox(e.Data);
            };
            ws.Connect();
        }

        public void SendWS(string myStr)
        {
            //wss://empathymtr.herokuapp.com:443/headset
            //string myStr = My_Program.myForm.DataToSend.Text;
            foreach (var socket in allSockets.ToList())
            {
            //ws.SendAsync(myStr, new Action<bool>((completed) =>
            //{
            //    if (completed)
            //    {
            //        My_Program.UpdateMsgBox("1");
            //    }
            //    else
            //    {
            //        My_Program.UpdateMsgBox("0");
            //    }

            //}));
            ws.Send(myStr);
            }
        }
     
        public void DisconnectWS()
        {
            ws.Close();
        }
    }

}