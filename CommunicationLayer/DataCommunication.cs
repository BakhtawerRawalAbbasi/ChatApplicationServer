using Models;
using NetMQ;
using NetMQ.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Communication
{
    public class DataCommunication
    {


        private ServerSockets obj;
        public  IDictionary<string, string> ClientEmailID = new Dictionary<string, string>();
        public delegate void MessageType(string ClientId, object mess, string messType);

        // Event of delegate 
        public event MessageType Mess_Res;


        public delegate void MessageCurrentUser(string ClientId,  string messType);

        // Event of delegate 
        public event MessageCurrentUser Mess_CurrentLoginUser;
       
        public DataCommunication()
        {
           /// LoginUserEmail = new UserStatus();

            obj = new ServerSockets();
            obj.Mess_Rec += Mess_Received;



        }

        public void DataSend<T>(T t, string ClientId,string messType)
        {
           
            byte[] jsonString = Serialization.JsonSerializer<T>(t);
            obj.Send(jsonString, ClientId, messType);

        }
        public void Run()

        {

            obj.Run();
        }

        public void Mess_Received(string ClientId, byte[] mess, string messType)
        {
            if (messType == "Login Request")
            {
                UserLoginRequest pp = Deserialization.JsonDeserialize<UserLoginRequest>(mess);
                bool keyExists = ClientEmailID.ContainsKey(pp.Email);
                if (!keyExists)
                {
                    ClientEmailID.Add(pp.Email, ClientId);
                }
                else
                {
                    ClientEmailID.Remove(pp.Email);
                    ClientEmailID.Add(pp.Email, ClientId);

                }
              
                OnReceivedMessType(ClientId, pp, messType);
            }
            else if (messType == "Registration")
            {
                RequesttoSignUP pp = Deserialization.JsonDeserialize<RequesttoSignUP>(mess);
                ClientEmailID.Add(pp.Email, ClientId);
                OnReceivedMessType(ClientId, pp, messType);
            }

            else if (messType == "Current User Login")
            {
                OnReceivedMessType(ClientId, messType);
            }
            else if (messType == "Send Message Request")
            {
                RequestToSendMess pp = Deserialization.JsonDeserialize<RequestToSendMess>(mess);
                OnReceivedMessType(ClientId, pp, messType);
            }

            else if (messType == "History of message")
            {
                SenderReceiverEmial pp = Deserialization.JsonDeserialize<SenderReceiverEmial>(mess);
                OnReceivedMessType(ClientId, pp, messType);
            }

            else if (messType == "Request for Logout")
            {
                OnReceivedMessType(ClientId, messType);
            }

        }


        protected virtual void OnReceivedMessType(string ClientId, object mess, string messType)
        {


            // this method notify all subscriber
            if (Mess_Res != null)
                // Raise an event
                Mess_Res(ClientId, mess, messType);



        }

        protected virtual void OnReceivedMessType(string ClientId, string messType)
        {


            // this method notify all subscriber
            if (Mess_CurrentLoginUser != null)
                // Raise an event
                Mess_CurrentLoginUser(ClientId,  messType);



        }

    }
}
