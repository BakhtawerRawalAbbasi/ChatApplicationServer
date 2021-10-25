using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class UserAuthentication
    {
        SqlConnection sqlCon = new SqlConnection(@"Data Source=DEV-BAKHTAWAR; Initial Catalog=LoginDB;Integrated security=True;");


        public string UsersAuthentication(string email,string password)
        {
              try
            {
                if (sqlCon.State == ConnectionState.Closed)

                sqlCon.Open();
                string query = $"SELECT COUNT(1) FROM tbl_User WHERE EmailID= '{email}' AND Password= '{password}'";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.Parameters.AddWithValue("@EmailID", email);
                sqlCmd.Parameters.AddWithValue("@Password", password);
                int count = Convert.ToInt32(sqlCmd.ExecuteScalar());
                if (count == 1)
                {
                    return "succefull";
                   
                }
                else
                {
                    return "Invalid";
                    // MessageBox.Show("Invalid name and password");
                }
            }
         
            finally
            {
                sqlCon.Close();
            }
        }


        public string UserRegistration(string email, string password,string userName)
        {
            try
            {
                if (sqlCon.State == ConnectionState.Closed)
                sqlCon.Open();

                string query = $"SELECT COUNT(1) FROM tbl_User WHERE EmailID= '{email}'";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.CommandType = CommandType.Text;
                int count = Convert.ToInt32(sqlCmd.ExecuteScalar());
                if (count == 1)
                {
                    return "Email ID already Exit";

                }
                else
                {
                    query = $"INSERT INTO tbl_User(UserName,EmailID,Password) VALUES('{userName}','{email}','{password}' )";
                    sqlCmd.Parameters.AddWithValue("@UserName", userName);
                    sqlCmd.Parameters.AddWithValue("@Password", password);
                    sqlCmd.Parameters.AddWithValue("@EmailID", email);
                    sqlCmd.ExecuteScalar();
                    return "succefull";
                }
               
            }

            finally
            {
                sqlCon.Close();
            }
        }


        public string UserMessage(string message, DateTime messageSendTime, string receiver_Email,string sender_Email)
        {
            try
            {
                if (sqlCon.State == ConnectionState.Closed)
                sqlCon.Open();

                string query = $"INSERT INTO tbl_Message(Message,MessageSendTime,Receiver_Email,Sender_Email) VALUES('{message}','{messageSendTime}','{receiver_Email}','{sender_Email}' )";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.Parameters.AddWithValue("@Message", message);
                sqlCmd.Parameters.AddWithValue("@MessageSendTime", messageSendTime);
                sqlCmd.Parameters.AddWithValue("@Receiver_Email", receiver_Email);
                sqlCmd.Parameters.AddWithValue("@Sender_Email", sender_Email);
                sqlCmd.ExecuteScalar();

                return "succefull";


            }

            finally
            {
                sqlCon.Close();
            }
        }

        public DataTable UserHistoryMessages(String receiverEmail,string senderEmail)
        {
            try
            {
                if (sqlCon.State == ConnectionState.Closed)
                sqlCon.Open();
                string query = $"select Message,MessageSendTime,Sender_Email from tbl_Message where Sender_Email='{senderEmail}' And Receiver_Email='{receiverEmail}' UNION select Message,MessageSendTime,Sender_Email from tbl_Message where Sender_Email = '{receiverEmail}' And Receiver_Email = '{senderEmail}'order by MessageSendTime ASC;";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                DataTable dt = new DataTable();
                //  dt = json
                sqlCmd.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(sqlCmd);
                da.Fill(dt);

                //sqlCon.Close();
                //dt= (DataTable)sqlCmd.ExecuteScalar();
                return dt;


            }

            finally
            {
                sqlCon.Close();
            }
        }
        public DataTable UserList()
        {
            try
            {
                if (sqlCon.State == ConnectionState.Closed)

                sqlCon.Open();
                string query = $"Select UserName,EmailID FROM tbl_User";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                DataTable dt = new DataTable();
              //  dt = json
               sqlCmd.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(sqlCmd);
                da.Fill(dt);
               
                //sqlCon.Close();
               //dt= (DataTable)sqlCmd.ExecuteScalar();
                return dt;


            }

            finally
            {
                sqlCon.Close();
            }
        }
    }


}
