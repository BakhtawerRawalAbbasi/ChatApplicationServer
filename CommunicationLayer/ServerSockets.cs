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

        public IDictionary<string, string> ClientIDEmail=new Dictionary<string, string>(); 

        public delegate void MessageType(string mess,NetMQSocketEventArgs e);

        // Event of delegate 
        public event MessageType Mess_Res;
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
            Response response = new Response();
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
           
            //PrintFrames("Server receiving", clientMessage);
            //if (clientMessage.FrameCount == 3)
            //{

            //clientAddress = clientMessage[0];
            // var clientMessageType = clientMessage[1];
            //  var clientOriginalMessage = clientMessage[2];
            // string response = string.Format("Message type {1} Message back from server {1}", clientMessageType,
            //clientOriginalMessage);
            //  var messageToClient = new NetMQMessage();
            //  messageToClient.Append(clientAddress);
            //   messageToClient.Append(clientMessageType);
            //  messageToClient.Append(clientOriginalMessage);
            OnReceivedMess(clientMessage[0].ConvertToString() ,clientMessage[2].Buffer , clientMessage[1].ConvertToString());
              // OnReceivedMessType(clientMessage[1].ConvertToString(),e);
          //if (clientMessage.FrameCount == 3)
            //    {
              //     Server_Send("req", e);
              //  }

                // server.SendMultipartMessage(clientMessage);
           // }

            // Server_SendClient(response, e);

        }

        public void StoreClientIDEmail(string clientID,string EmailID)
        {
            ClientIDEmail.Add(clientID,EmailID);
        }
        public class Response
        {
            public string email ;
            public string password;

            public Response()
            {
                email = "bakhtawer@nu.edu.pk";
                password = "123";
        }
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
