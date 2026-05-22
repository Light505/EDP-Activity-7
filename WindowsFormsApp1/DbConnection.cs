using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace InformationSystem
{
    public class DbConnection
    {
        private readonly string connectionString =
            "server=localhost;port=3306;database=uni_db;uid=root;pwd=Password123456789;";

        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }
    }
}
