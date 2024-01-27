using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace babyFoot2
{
    public class Connexion
    {
        public static SqlConnection connexionMysql()
        {
            // string connectionString = "Server=ETU1883-THONY;Database=canal;User Id=sa;Pwd=0000;";
            string connectionString = "Server=ETU1883-THONY;Database=babyFoot;User Id=sa;Pwd=0000;";
            SqlConnection connection = new SqlConnection(connectionString);
            return connection;
        }
    }
}
