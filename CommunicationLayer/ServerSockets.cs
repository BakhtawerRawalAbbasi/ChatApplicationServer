using Models;
using NetMQ;
using NetMQ.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communication
{
   public class ServerSockets
    {
        public delegate void MessageReceived(string ClientId, byte[] mess, string messType);

        // Event of delegate 
        public event MessageReceived Mess_Rec;
        public delegate void MessageType(string mess,NetMQSocketEventArgs e);
        NetMQPoller poller;
        RouterSocket server;
        public string response;
        public string clientAddress;
        public ServerSockets()
        {
            initialize();
        }
        ~ServerSockets()
        {

        }

        public void initialize()
        {
            //There is one server. It binds a RouterSocket which stores the inbound request's connection identity to work out
            //how to route back the response message to the correct client socket.

            // poller = new NetMQPoller();
            server = new RouterSocket("@tcp://127.0.0.1:5556");
            // raise event by method
            server.ReceiveReady += Server_ReceiveReady;
            poller = new NetMQPoller { server };
            //poller.Run();
      

        }
        public void Run()

        {
            poller.Run();
        }
        public  void Server_ReceiveReady(object sender, NetMQSocketEventArgs e)
        {

            var clientMessage = e.Socket.ReceiveMultipartMessage(); 
            OnReceivedMess(clientMessage[0].ConvertToString() ,clientMessage[2].Buffer , clientMessage[1].ConvertToString());
              

        }

        public void Send(byte[] mess,string ClientId,string messType)
        {
           
            var clientMessage = new NetMQMessage();
            clientMessage.Append(ClientId);
            clientMessage.Append(mess);
            clientMessage.Append(messType);
           // clientMessage.Append(mess);
           server.SendMultipartMessage(clientMessage);
       }

   
        protected virtual void OnReceivedMess(string ClientId,byte[] mess, string messType)
        {

            
            // this method notify all subscriber
            if (Mess_Rec != null)
                // Raise an event
                Mess_Rec(ClientId,mess, messType);

        }
        
    }
}
