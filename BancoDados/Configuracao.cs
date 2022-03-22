using System;
using System.Data.SqlClient;

namespace BancoDados
{
    public class Configuracao
    {
        protected string DataSource { get; set; }
        protected string Database { get; set; }
        protected string Username { get; set; }
        protected string Password { get; set; }

        public static string ConnString { get; set; }

        public Configuracao(string dataSource = null, string database = null, string username = null, string password = null)
        {
            DataSource  = string.IsNullOrWhiteSpace(dataSource)?    "src"    : dataSource;
            Database    = string.IsNullOrWhiteSpace(database)?      "base"   : database;
            Username    = string.IsNullOrWhiteSpace(username)?      "user"   : username;
            Password    = string.IsNullOrWhiteSpace(password)?      "pass"   : password;

            ConnString = @"Data Source=" + DataSource + ";Initial Catalog="
                            + Database + ";Persist Security Info=True;User ID=" + Username + ";Password=" + Password;
        }

        public static SqlConnection Conexao()
        {
            try
            {
                SqlConnection conexao = new(ConnString);
                return conexao;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }

            return null;
        }
    }
}
