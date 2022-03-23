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
            DataSource = string.IsNullOrWhiteSpace(dataSource) ? "DESKTOP-1UCK7M9" : dataSource;
            Database = string.IsNullOrWhiteSpace(database) ? "Biltiful" : database;
            Username = string.IsNullOrWhiteSpace(username) ? "sa" : username;
            Password = string.IsNullOrWhiteSpace(password) ? "123456" : password;

            ConnString = @"Data Source=" + DataSource + ";Initial Catalog="
                            + Database + ";Persist Security Info=True;User ID=" + Username + ";Password=" + Password;
        }

        public static SqlConnection Conexao()
        {
            _ = new Configuracao();   

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
