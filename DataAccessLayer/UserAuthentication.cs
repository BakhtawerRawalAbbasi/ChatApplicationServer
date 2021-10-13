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
                string query = $"SELECT COUNT(1) FROM tbl_User WHERE UserName= '{email}' AND Password= '{password}'";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.Parameters.AddWithValue("@UserName", email);
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
                string query = $"INSERT INTO tbl_User(UserName, Password,EmailID) VALUES('{email}', '{password}', '{userName}')";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.Parameters.AddWithValue("@UserName", email);
                sqlCmd.Parameters.AddWithValue("@Password", password);
                sqlCmd.Parameters.AddWithValue("@EmailID", userName);
                sqlCmd.ExecuteScalar();
                return "succefull";

               
            }

            finally
            {
                sqlCon.Close();
            }
        }

        public string UserList()
        {
            try
            {
                if (sqlCon.State == ConnectionState.Closed)

                sqlCon.Open();
                string query = $"Select UserName,EmailID FROM tbl_User";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.ExecuteScalar();
                return "succefull";


            }

            finally
            {
                sqlCon.Close();
            }
        }
    }


}
