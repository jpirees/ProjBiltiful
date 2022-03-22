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

            _ = new Configuracao();

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

        public void EditarCliente(Cliente clienteAtualizado)
        {

            Read read = new Read();
            List<Cliente> clientes = Read.ListaArquivoCliente();
            int posicao = 0;
            try
            {

                while (clientes[posicao] != null)
                {
                    if (clienteAtualizado.CPF == clientes[posicao].CPF)
                    {
                        clientes[posicao] = clienteAtualizado;
                        break;
                    }
                    posicao++;
                }
                //File.Delete(CaminhoCadastro);
                //using (StreamWriter sw = new StreamWriter(CaminhoCadastro))
                //{
                //    posicao = 0;
                //    do
                //    {
                //        sw.WriteLine(clientes[posicao].RetornaArquivo());
                //        posicao++;
                //    } while (posicao < clientes.Count);
                //    Console.WriteLine("Registro atualizado");
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ocorreu um erro: " + ex.Message);
            }
        }

        public static void BloqueiaCliente(string cpf)
        {
            _ = new Configuracao();

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
            _ = new Configuracao();

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

            _ = new Configuracao();

            using var conexao = Configuracao.Conexao();

            conexao.Open();

            string sql = $"INSERT INTO dbo.Fornecedor (CNPJ, Razao_Social, Data_Abertura, Situacao, Bloqueio) VALUES ('{fornecedor.CNPJ}', '{fornecedor.RazaoSocial}', CONVERT(DATE,'{DataAbertura}'), '{fornecedor.Situacao}', '0')";
            SqlCommand cmd = new SqlCommand(sql, conexao);
            _ = cmd.ExecuteNonQuery();

            conexao.Close();
        }

        public void EditarFornecedor(Fornecedor fornecedorAtualizado)
        {
            List<Fornecedor> fornecedores = Read.ListaArquivoFornecedor();

            int posicao = 0;

            try
            {
                while (fornecedores[posicao] != null)
                {
                    if (fornecedorAtualizado.CNPJ == fornecedores[posicao].CNPJ)
                    {
                        fornecedores[posicao] = fornecedorAtualizado;
                        break;
                    }
                    posicao++;
                }
                //File.Delete(CaminhoFornecedor);
                //using (StreamWriter sw = new StreamWriter(CaminhoFornecedor))
                //{
                //    posicao = 0;
                //    do
                //    {
                //        sw.WriteLine(fornecedores[posicao].RetornaArquivo());
                //        posicao++;
                //    } while (posicao < fornecedores.Count);
                //    Console.WriteLine("Registro atualizado");
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ocorreu um erro: " + ex.Message);
            }
        }

        public static void BloquearFornecedor(string cnpj)
        {
            _ = new Configuracao();

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
            _ = new Configuracao();

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
