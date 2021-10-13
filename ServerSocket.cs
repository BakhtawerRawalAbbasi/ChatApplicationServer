using NetMQ;
using NetMQ.Sockets;
using System;

namespace Server_Socket
{
    class ServerSocket
    {
        static void Main(string[] args)
        {
            using (var server = new RouterSocket("@tcp://127.0.0.1:5556"))
            {
                //using (var poller = new NetMQPoller())
                //{

                // server loop
                while (true)
                {
                    var clientMessage = server.ReceiveMultipartMessage();
                    //PrintFrames("Server receiving", clientMessage);
                    if (clientMessage.FrameCount == 3)
                    {
                        var clientAddress = clientMessage[0];
                        var clientMessageType = clientMessage[1];
                        var clientOriginalMessage = clientMessage[2];
                        // string response = string.Format("Message type {1} Message back from server {1}", clientMessageType,
                        //clientOriginalMessage);
                        var messageToClient = new NetMQMessage();
                        messageToClient.Append(clientAddress);
                        messageToClient.Append(clientMessageType);
                        messageToClient.Append(clientOriginalMessage);

                        server.SendMultipartMessage(messageToClient);
                    }
                }
                //}
            }

        }
        static void PrintFrames(string operationType, NetMQMessage message)
        {
            for (int i = 0; i < message.FrameCount; i++)
            {
                Console.WriteLine("{0} Socket : Frame[{1}] = {2}", operationType, i,
                   message[i].ConvertToString());
            }
        }
    }
}
