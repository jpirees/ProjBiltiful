using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BancoDados;
using System.Data.SqlClient;

namespace CadastrosBasicos.ManipulaArquivos
{
    public class Write
    {
        public Write() { }

        public static void GravarNovoCliente(Cliente cliente)
        {
            _ = DateTime.TryParse(cliente.DataNascimento.ToString("yyyy-MM-dd"), out DateTime DataNascimento);

            using var conexao = Configuracao.Conexao();

            try
            {
                conexao.Open();

                string sql = $"INSERT INTO dbo.Cliente (CPF, Nome, Data_Nasc, Sexo, Situacao, Risco) VALUES ('{cliente.CPF}', '{cliente.Nome}', CONVERT(DATE,'{DataNascimento}'),'{cliente.Sexo}', '{cliente.Situacao}', '0')";
                SqlCommand cmd = new SqlCommand(sql, conexao);
                _ = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                Console.ReadKey();
            }
            finally
            {
                conexao.Close();
            }
        }

        public static void EditarCliente(Cliente clienteAtualizado)
        {
            _ = DateTime.TryParse(clienteAtualizado.DataNascimento.ToString("yyyy-MM-dd"), out DateTime DataNascimento);
            
            using var conexao = Configuracao.Conexao();

            try
            {
                conexao.Open();

                string sql = $"UPDATE dbo.Cliente SET Nome='{clienteAtualizado.Nome}', Data_Nasc=CONVERT(DATE,'{DataNascimento}'), '{clienteAtualizado.Situacao}') WHERE CPF='{clienteAtualizado.CPF}'";
                SqlCommand cmd = new SqlCommand(sql, conexao);
                _ = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                Console.ReadKey();
            }
            finally
            {
                conexao.Close();
            }
        }

        public static void BloqueiaCliente(string cpf)
        {
            using var conexao = Configuracao.Conexao();

            conexao.Open();

            try
            {
                string sql = $"UPDATE dbo.Cliente SET Risco = 1 WHERE CPF='{cpf}'";

                SqlCommand cmd = new SqlCommand(sql, conexao);
                _ = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
            finally
            {
                conexao.Close();
            }
        }

        public static void DesbloqueiaCliente(string cpf)
        {
            

            using var conexao = Configuracao.Conexao();

            conexao.Open();

            try
            {
                string sql = $"UPDATE dbo.Cliente SET Risco = 0 WHERE CPF='{cpf}'";

                SqlCommand cmd = new SqlCommand(sql, conexao);
                _ = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
            finally
            {
                conexao.Close();
            }
        }

        public static void GravarNovoFornecedor(Fornecedor fornecedor)
        {
            _ = DateTime.TryParse(fornecedor.DataAbertura.ToString("yyyy-MM-dd"), out DateTime DataAbertura);

            using var conexao = Configuracao.Conexao();

            conexao.Open();

            string sql = $"INSERT INTO dbo.Fornecedor (CNPJ, Razao_Social, Data_Abertura, Situacao, Bloqueio) VALUES ('{fornecedor.CNPJ}', '{fornecedor.RazaoSocial}', CONVERT(DATE,'{DataAbertura}'), '{fornecedor.Situacao}', '0')";
            SqlCommand cmd = new SqlCommand(sql, conexao);
            _ = cmd.ExecuteNonQuery();

            conexao.Close();
        }

        public static void EditarFornecedor(Fornecedor fornecedorAtualizado)
        {
            using var conexao = Configuracao.Conexao();

            try
            {
                conexao.Open();

                string sql = $"UPDATE dbo.Fornecedor SET Razao_Social='{fornecedorAtualizado.RazaoSocial}', '{fornecedorAtualizado.Situacao}') WHERE CNPJ='{fornecedorAtualizado.CNPJ}'";
                SqlCommand cmd = new SqlCommand(sql, conexao);
                _ = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                Console.ReadKey();
            }
            finally
            {
                conexao.Close();
            }
        }

        public static void BloquearFornecedor(string cnpj)
        {
            using var conexao = Configuracao.Conexao();

            conexao.Open();

            try
            {
                string sql = $"UPDATE dbo.Fornecedor SET Bloqueio = 1 WHERE CNPJ='{cnpj}'";

                SqlCommand cmd = new SqlCommand(sql, conexao);
                _ = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
            finally
            {
                conexao.Close();
            }
        }

        public static void DesbloqueiaFornecedor(string cnpj)
        {
            using var conexao = Configuracao.Conexao();

            conexao.Open();

            try
            {
                string sql = $"UPDATE dbo.Fornecedor SET Bloqueio = 0 WHERE CNPJ='{cnpj}'";

                SqlCommand cmd = new SqlCommand(sql, conexao);
                _ = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
            finally
            {
                conexao.Close();
            }
        }
    }
}
