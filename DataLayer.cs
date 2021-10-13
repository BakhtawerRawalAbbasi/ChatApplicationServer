using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server_Socket
{
    class DataLayer
    {

        public void dataLayer()
        {
            SqlConnection sqlCon = new SqlConnection(@"Data Source=DEV-BAKHTAWAR; Initial Catalog= LoginDB;Integrated security=True;");
            try
            {
                if (sqlCon.State == ConnectionState.Closed)

                    sqlCon.Open();
                string query = "SELECT COUNT(1) FROM tblUser WHERE UserName=@UserName AND Password=@Password";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.CommandType = CommandType.Text;
                // sqlCmd.Parameters.AddWithValue("@UserName", UserName.Text);
                //   sqlCmd.Parameters.AddWithValue("@Password", Password.Text);
                int count = Convert.ToInt32(sqlCmd.ExecuteScalar());
                if (count == 1)
                {
                    //MainWindow dashboard = new MainWindow();
                    // this.Show();
                    // this.Close();
                }
                else
                {
                    // MessageBox.Show("Invalid name and password");
                }
            }
            catch (Exception ex)
            {

                //MessageBox.Show(ex.Message);
            }
            finally
            {
                sqlCon.Close();
            }
        }
    }
}
