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

        public IDictionary<string, string> ClientIDEmail = new Dictionary<string, string>();
        public List<string> clientEmail = new List<string> { };
        LoginUser userLogin;
        public delegate void MessageType(string ClientId, object mess, string messType);

        // Event of delegate 
        public event MessageType Mess_Res;


        public delegate void MessageCurrentUser(string ClientId,  string messType);

        // Event of delegate 
        public event MessageCurrentUser Mess_CurrentLoginUser;
        private UserStatus loginuser;
      
        public UserStatus LoginUserEmail
        {
            get { return loginuser; }
            set { loginuser = value; }
        }
        public DataCommunication()
        {
            LoginUserEmail = new UserStatus();

            obj = new ServerSockets();
            obj.Mess_Rec += Mess_Received;



        }

        public void DataSend<T>(T t, string ClientId,string messType)
        {
            //Serialization serialize = new Serialization();
            byte[] jsonString = Serialization.JsonSerializer<T>(t);
            //Client_socket client = new Client_socket("2");
            obj.Send(jsonString, ClientId, messType);
            // client.Mess_Received += Mess_Received;

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
             
                //obj.StoreClientIDEmail(ClientId, pp.Email, messType);
                ClientIDEmail.Add(ClientId, pp.Email);
                clientEmail = ClientIDEmail.Values.ToList();

                List<UserStatus> UserLoginList = new List<UserStatus>();
                for (int i = 0; i < clientEmail.Count; i++)
                {
                    UserStatus userEmail = new UserStatus();
                    userEmail.EmailID = clientEmail[i];

                    UserLoginList.Add(userEmail);
                }

                userLogin = new LoginUser(UserLoginList);
                //byte[] jsonString = Serialization.JsonSerializer(userLogin);

                //Send(jsonString, clientID, "Login User");


                DataSend<LoginUser>(userLogin, ClientId, "Login User");
                OnReceivedMessType(ClientId, pp, messType);
            }
            else if (messType == "Registration")
            {
                RequesttoSignUP pp = Deserialization.JsonDeserialize<RequesttoSignUP>(mess);
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

            //  byte[] jsonString = Serialization.JsonSerializer<T>(t);
        }


        //protected virtual void OnReceivedMessType(string mess, NetMQSocketEventArgs e)
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











        //static void Main(string[] args)
        // {


        // }

    }
}
