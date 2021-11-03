using Communication;
using DataAccessLayer;
using Models;
using NetMQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;


namespace ConsoleApp1
{
   public class MessageHandler
    {
        private DataCommunication obj1 ;
        public DataManipulation userNotification = new DataManipulation();
        private UserLoginResponse resp;
        LoginUser userLoginEmail;
        private ResponsetoSignUP signUpResp ;
        private ResponseSendMessage sendMessageResponse;
        private ResponsetoListUser userLoginList;
        private ResponseOfHistoryMessages userhistorymessage;
        private RequestToSendMess userMessage;
        public RequestToSendMess sendmessresponse { get; set; }
        private SenderReceiverEmial userHistoryMessage;
        RequesttoSignUP userRegistation = new RequesttoSignUP();
        UserLoginRequest userLogin = new UserLoginRequest();
        public List<string> clientEmail = new List<string> { };
        public string response;
        DataTable dt = new DataTable();
        string notifi = null;
        public ResponsetoSignUP RegistrationResponse
        {
            get { return signUpResp; }
            set { signUpResp = value; }
        }
        public ResponseSendMessage SendMessageResponse
        {
            get { return sendMessageResponse; }
            set { sendMessageResponse = value; }
        }
        public UserLoginResponse Response
        {
            get { return resp; }
            set { resp = value; }
        }
        
        public ResponsetoListUser UserLogin
        {
            get { return userLoginList; }
            set { userLoginList = value; }
        }

        public ResponseOfHistoryMessages UserHistoryMessage
        {
            get { return userhistorymessage; }
            set { userhistorymessage = value; }
        }
        public MessageHandler(DataCommunication obj)
        {
            obj1 = obj;
            obj.Mess_Res += Server_Send;
            LoginUserEmail = new UserStatus();
            obj.Mess_CurrentLoginUser += Server_SendLoginUser;
        

        }
        private UserStatus loginuser;

        public UserStatus LoginUserEmail
        {
            get { return loginuser; }
            set { loginuser = value; }
        }
        public string UserAuthentication(string email,string password)
        {
        
           notifi= userNotification.UsersAuthentication(email, password);
           return notifi;
        }

        public string UserRegistration(string id,string email, string password,string userName)
        {
           
            notifi = userNotification.UserRegistration(email, password,userName);
            return notifi;
        }

        public string UserMessage(string message, DateTime messagesendTime, string receiver_email, string sender_email)
        {
         
            notifi = userNotification.UserMessage(message, messagesendTime, receiver_email, sender_email);
            return notifi;
        }

        public DataTable HistoryMessages(string receiverEmailID,string sender_email)
        {
           
            dt = userNotification.UserHistoryMessages(receiverEmailID, sender_email);
            
            return dt;
        }
        public  DataTable UserList()
        {
            
          
            dt=userNotification.UserList();
            return dt;
        }
        public void Server_SendLoginUser(string ClientId, string request)
        {

            if(request== "Current User Login")
            {
                dt = UserList();

                List<User> UserLoginList = new List<User>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    User user = new User();
                    user.EmailID = dt.Rows[i]["EmailID"].ToString();
                    user.UserName = dt.Rows[i]["UserName"].ToString();
                    if (obj1.ClientEmailID.ContainsKey(user.EmailID))
                    {
                        user.Status = "online";
                    }
                    else
                        user.Status = "Ofline";
                    UserLoginList.Add(user);
                }


                UserLogin = new ResponsetoListUser(UserLoginList);
                clientEmail = obj1.ClientEmailID.Values.ToList();
                for (int i = 0; i < clientEmail.Count; i++)
                {

                    ClientId = obj1.ClientEmailID.ElementAt(i).Value;
                    obj1.DataSend<ResponsetoListUser>(UserLogin, ClientId, request);


                }
            }
           else if(request == "Request for Logout")
            {
                for (int i = 0; i < clientEmail.Count; i++)
                {

                 string clientIdKey = obj1.ClientEmailID.ElementAt(i).Key;
                 if(obj1.ClientEmailID.ElementAt(i).Value == ClientId)
                 {
                        obj1.ClientEmailID.Remove(clientIdKey);
                        break;
                 }
                    

                }

      

            }


        }
        public void Server_Send(string ClientId,object pp,string request)
        {
            
            if (request == "Login Request")
            {
                userLogin = (UserLoginRequest)pp;
                response = UserAuthentication(userLogin.Email, userLogin.Password);
               Response = new UserLoginResponse(response);
               obj1.DataSend<UserLoginResponse>(Response, ClientId,request);
               
            }

            else if (request == "Registration")
            {
                userRegistation = (RequesttoSignUP)pp;
                response = UserRegistration(ClientId,userRegistation.Email, userRegistation.Password, userRegistation.UserName);
                RegistrationResponse = new ResponsetoSignUP(response);
                obj1.DataSend<ResponsetoSignUP>(RegistrationResponse, ClientId,request);

            }

           
            else if(request == "Send Message Request")
            {
                userMessage = (RequestToSendMess)pp;
                UserMessage(userMessage.Message,userMessage.Messag_SendTime,userMessage.Receiver_Email_id,userMessage.Sender_Email_ID);
                SenderNewMessage.Instance.Message = userMessage.Message;
                SenderNewMessage.Instance.SenderEmailID = userMessage.Sender_Email_ID;
                SenderNewMessage.Instance.ReceiverEmailID = userMessage.Receiver_Email_id;
                SenderNewMessage.Instance.MessageSendTime = userMessage.Messag_SendTime;
                if (obj1.ClientEmailID.ContainsKey(userMessage.Receiver_Email_id))
                {
                    string value;
                    obj1.ClientEmailID.TryGetValue(userMessage.Receiver_Email_id,out value);
                     ClientId = value;

                    obj1.DataSend<SenderNewMessage>(SenderNewMessage.Instance, ClientId, "Message Receive Request");
                }
                

              
            }
            else if (request == "History of message")
            {
                userHistoryMessage = (SenderReceiverEmial)pp;
                dt = HistoryMessages(userHistoryMessage.ReceiverEmailID,userHistoryMessage.SenderEmailID);
                List<HistoryOfMessages> UserHistoryMessages = new List<HistoryOfMessages>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    HistoryOfMessages userMessages = new HistoryOfMessages();
                    userMessages.Messages = dt.Rows[i]["Message"].ToString();
                    userMessages.MessageSentTime = (DateTime)dt.Rows[i]["MessageSendTime"];
                    userMessages.SenderEmail = dt.Rows[i]["Sender_Email"].ToString();

                    UserHistoryMessages.Add(userMessages);
                }
                userhistorymessage = new ResponseOfHistoryMessages(UserHistoryMessages);
                obj1.DataSend<ResponseOfHistoryMessages>(userhistorymessage, ClientId, request);
     
            }
            




        }
    }
}
