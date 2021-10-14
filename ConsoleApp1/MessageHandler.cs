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
        public UserAuthentication userNotification = new UserAuthentication();
        private UserLoginResponse resp;
        private ResponsetoSignUP signUpResp ;
        private ResponsetoListUser userLoginList;
        RequesttoSignUP userRegistation = new RequesttoSignUP();
        UserLoginRequest userLogin = new UserLoginRequest();
        public string response;
        DataTable dt = new DataTable();
        List<User> responseUserList;

        public ResponsetoSignUP RegistrationResponse
        {
            get { return signUpResp; }
            set { signUpResp = value; }
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
        public MessageHandler(DataCommunication obj)
        {
            obj1 = obj;
            obj.Mess_Res += Server_Send;
            obj.Mess_CurrentLoginUser += Server_SendLoginUser;


            //Response = new UserLoginResponse(response);
        }
       
        public string UserAuthentication(string email,string password)
        {
           string notifi=null;
           notifi= userNotification.UsersAuthentication(email, password);
           return notifi;
        }

        public string UserRegistration(string id,string email, string password,string userName)
        {
            string notifi = null;
            notifi = userNotification.UserRegistration(email, password,userName);
            return notifi;
        }
        public  DataTable UserList()
        {
            
          
            dt=userNotification.UserList();
            return dt;
        }
        public void Server_SendLoginUser(string ClientId, string request)
        {

            dt = UserList();

            List<User> UserLoginList = new List<User>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                User user = new User();
                user.EmailID = dt.Rows[i]["EmailID"].ToString();
                user.UserName = dt.Rows[i]["UserName"].ToString();
                UserLoginList.Add(user);
            }

            UserLogin = new ResponsetoListUser(UserLoginList);
            obj1.DataSend<ResponsetoListUser>(UserLogin, ClientId, request);


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

            else if (request == "Current User Login")
            {
                userRegistation = (RequesttoSignUP)pp;
                response = UserRegistration(ClientId, userRegistation.Email, userRegistation.Password, userRegistation.UserName);
                RegistrationResponse = new ResponsetoSignUP(response);
                obj1.DataSend<ResponsetoSignUP>(RegistrationResponse, ClientId, request);

            }


        }
    }
}
