using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace WpfApp1
{
    class DatabseConn
    {
        public string MyConnection = "server=localhost;database=mailtable;uid=root;pwd=root;";
        public string Connection(string email, ref string result2 ,ref string dat)
        {
            
            MySqlConnection myCon = new MySqlConnection(MyConnection);
            string Query2 = "select * from tablemailer where mail = '"+ email + "'";

            MySqlCommand MyCommand2 = new MySqlCommand(Query2, myCon);

            myCon.Open();
            
            MySqlDataAdapter sda = new MySqlDataAdapter(MyCommand2);
            DataTable dt = new DataTable();
            sda.Fill(dt);

            string emailac = null;
            string date = null;
            string comment = null;
            string counterac = null;
            string spamcounterac = null;

            foreach (DataRow row in dt.Rows)
            {
                emailac = row["mail"].ToString();
                date = row["date"].ToString();
                comment = row["comment"].ToString();
                counterac = row["counter"].ToString();
                spamcounterac = row["spamcounter"].ToString();
            }

            DateTime pdate = DateTime.Today;
            if (counterac != null)
            {
                int count = int.Parse(counterac) + 1;

                string updatequr = "update tablemailer set counter = " + count + " where mail = '" + email + "'";

                MySqlCommand UpdateCommand = new MySqlCommand(updatequr, myCon);

                if (int.Parse(counterac) / 2 <= int.Parse(spamcounterac))
                {
                    result2 = "Spam mail found!";
                }

                UpdateCommand.ExecuteNonQuery();
            }
            else
            {
                string insertQuery = "insert into tablemailer values('" + email + "','" + pdate.ToString() + "','" + comment + "','" + 1 + "','" + 0 + "');";
                MySqlCommand MyCommand = new MySqlCommand(insertQuery, myCon);

                MyCommand.ExecuteNonQuery();
            }
            

            myCon.Close();
            dat = date;
            return comment;
        }

        public void Report(string email, string comment)
        {
            DateTime date = DateTime.Today;

            MySqlConnection myCon = new MySqlConnection(MyConnection);

            string Query = "select spamcounter from tablemailer where mail = '" + email + "'";
            string Query1 = "select counter from tablemailer where mail = '" + email + "'";
            string Query2 = "select comment from tablemailer where mail = '" + email + "'";
            string Query3 = "select date from tablemailer where mail = '" + email + "'";

            MySqlCommand MyCommand = new MySqlCommand(Query, myCon);
            MySqlCommand MyCommand1 = new MySqlCommand(Query1, myCon);
            MySqlCommand MyCommand2 = new MySqlCommand(Query2, myCon);
            MySqlCommand MyCommand3 = new MySqlCommand(Query3, myCon);

            myCon.Open();
            
            int spamcounter = int.Parse(MyCommand.ExecuteScalar().ToString());
            spamcounter = spamcounter + 1;

            int counter = int.Parse(MyCommand1.ExecuteScalar().ToString());
            counter = counter + 1;

            string dt = MyCommand3.ExecuteScalar().ToString();
            dt = dt + "^" + date.ToString();

            string comm = MyCommand2.ExecuteScalar().ToString();
            comm = comm + "^" + comment;
            
             string Queryx = "update tablemailer set counter = " + counter + ", " + "spamcounter = "+ spamcounter + ", " + "date = '" + dt + "' , " + "comment = '" + comm + "'" + " where mail = '" + email + "'";

             MySqlCommand MyCommandx = new MySqlCommand(Queryx, myCon);

            MyCommandx.ExecuteNonQuery();

            myCon.Close();
        }
    }
}
