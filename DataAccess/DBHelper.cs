using MySql.Data.MySqlClient;

namespace SweetBakery.DataAccess
{
    public static class DBHelper
    {
        private const string ConnString =
            "Server=localhost;Database=sweetbakery;Uid=root;Pwd=7fLGJ/NCoUTp#$#;";

        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnString);
        }
    }
}
