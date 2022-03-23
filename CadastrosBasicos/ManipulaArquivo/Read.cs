using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BancoDados;

namespace CadastrosBasicos.ManipulaArquivos
{
    public class Read
    {
        public string CaminhoFinal { get; set; }
        public string CaminhoCadastro { get; set; }
        public string ClienteInadimplente { get; set; }
        public string CaminhoFornecedor { get; set; }
        public string CaminhoBloqueado { get; set; }

        public Read() { }


        #region Cliente [Procura | Verifica Lista | Lista | Procura Inadimplente]
        public static Cliente ProcuraCliente(string cpf)
        {
            Cliente cliente = null;

            using (var conexao = Configuracao.Conexao())
            {
                string sql = $"SELECT CPF, Nome, Data_Nasc, Sexo, Ultima_Compra, Data_Cadastro, Situacao, Risco FROM dbo.Cliente WHERE CPF='{cpf}'";

                conexao.Open();

                using (SqlCommand cmd = new(sql, conexao))
                {
                    try
                    {
                        SqlDataReader dado = cmd.ExecuteReader();

                        while (dado.Read())
                        {
                            _ = DateTime.TryParse(DateTime.Parse(dado.GetValue(2).ToString()).ToString("dd/MM/yyyy"), out DateTime dataNasc);
                            _ = DateTime.TryParse(DateTime.Parse(dado.GetValue(4).ToString()).ToString("dd/MM/yyyy"), out DateTime ultimaCompra);
                            _ = DateTime.TryParse(DateTime.Parse(dado.GetValue(5).ToString()).ToString("dd/MM/yyyy"), out DateTime dataCadastro);

                            cliente = new Cliente((string)dado.GetValue(0), (string)dado.GetValue(1), dataNasc, char.Parse((string)dado.GetValue(3)), ultimaCompra, dataCadastro, char.Parse((string)dado.GetValue(6)), (bool)dado.GetValue(7));
                        }
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

            return cliente;
        }

        public static bool VerificaListaCliente()
        {
            string registros = null;

            using (var conexao = Configuracao.Conexao())
            {
                string sql = $"SELECT MAX(CPF) AS Registros FROM dbo.Cliente";

                conexao.Open();

                using (SqlCommand cmd = new(sql, conexao))
                {
                    try
                    {
                        cmd.ExecuteNonQuery();
                        registros = (string)cmd.ExecuteScalar();
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

            return registros == null;
        }

        public static List<Cliente> ListaArquivoCliente()
        {
            List<Cliente> clientes = new List<Cliente>();

            using (var conexao = Configuracao.Conexao())
            {
                string sql = $"SELECT CPF, Nome, Data_Nasc, Sexo, Ultima_Compra, Data_Cadastro, Situacao, Risco FROM dbo.Cliente";

                conexao.Open();

                using (SqlCommand cmd = new(sql, conexao))
                {
                    try
                    {
                        SqlDataReader dado = cmd.ExecuteReader();

                        while (dado.Read())
                        {
                            _ = DateTime.TryParse(DateTime.Parse(dado.GetValue(2).ToString()).ToString("dd/MM/yyyy"), out DateTime dataNasc);
                            _ = DateTime.TryParse(DateTime.Parse(dado.GetValue(4).ToString()).ToString("dd/MM/yyyy"), out DateTime ultimaCompra);
                            _ = DateTime.TryParse(DateTime.Parse(dado.GetValue(5).ToString()).ToString("dd/MM/yyyy"), out DateTime dataCadastro);

                            clientes.Add(new Cliente((string)dado.GetValue(0), (string)dado.GetValue(1), dataNasc, char.Parse((string)dado.GetValue(3)), ultimaCompra, dataCadastro, char.Parse((string)dado.GetValue(6)), (bool)dado.GetValue(7)));
                        }
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

            return clientes;
        }

        public static bool ProcurarCPFBloqueado(string cpf)
        {
            Cliente cliente = null;

            using (var conexao = Configuracao.Conexao())
            {
                string sql = $"SELECT CPF, Nome, Data_Nasc, Sexo, Ultima_Compra, Data_Cadastro, Situacao, Risco FROM dbo.Cliente WHERE CPF='{cpf}'";

                conexao.Open();

                using (SqlCommand cmd = new(sql, conexao))
                {
                    try
                    {
                        SqlDataReader dado = cmd.ExecuteReader();

                        while (dado.Read())
                        {
                            _ = DateTime.TryParse(DateTime.Parse(dado.GetValue(2).ToString()).ToString("dd/MM/yyyy"), out DateTime dataNasc);
                            _ = DateTime.TryParse(DateTime.Parse(dado.GetValue(4).ToString()).ToString("dd/MM/yyyy"), out DateTime ultimaCompra);
                            _ = DateTime.TryParse(DateTime.Parse(dado.GetValue(5).ToString()).ToString("dd/MM/yyyy"), out DateTime dataCadastro);

                            cliente = new Cliente((string)dado.GetValue(0), (string)dado.GetValue(1), dataNasc, char.Parse((string)dado.GetValue(3)), ultimaCompra, dataCadastro, char.Parse((string)dado.GetValue(6)), (bool)dado.GetValue(7));
                        }
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

            return cliente.Risco == true;
        }
        #endregion


        #region Fornecedor [Procura | Verifica Lista | Lista | Procura Bloqueado]
        public static Fornecedor ProcurarFornecedor(string cnpj)
        {
            Fornecedor fornecedor = null;

            using (var conexao = Configuracao.Conexao())
            {
                string sql = $"SELECT CNPJ, Razao_Social, Data_Abertura, Ultima_Compra, Data_Cadastro, Situacao, Bloqueio FROM dbo.Fornecedor WHERE CNPJ='{cnpj}'";

                conexao.Open();

                using (SqlCommand cmd = new(sql, conexao))
                {
                    try
                    {
                        SqlDataReader dado = cmd.ExecuteReader();

                        while (dado.Read())
                        {
                            _ = DateTime.TryParse(DateTime.Parse(dado.GetValue(2).ToString()).ToString("dd/MM/yyyy"), out DateTime dataAbertura);
                            _ = DateTime.TryParse(DateTime.Parse(dado.GetValue(3).ToString()).ToString("dd/MM/yyyy"), out DateTime ultimaCompra);
                            _ = DateTime.TryParse(DateTime.Parse(dado.GetValue(4).ToString()).ToString("dd/MM/yyyy"), out DateTime dataCadastro);

                            fornecedor = new Fornecedor((string)dado.GetValue(0), (string)dado.GetValue(1), dataAbertura, ultimaCompra, dataCadastro, char.Parse((string)dado.GetValue(5)), (bool) dado.GetValue(6));
                        }
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

            return fornecedor;
        }

        public static bool VerificaListaFornecedor()
        {
            string registros = null;

            using (var conexao = Configuracao.Conexao())
            {
                string sql = $"SELECT MAX(CNPJ) AS Registros FROM dbo.Fornecedor";

                conexao.Open();

                using (SqlCommand cmd = new(sql, conexao))
                {
                    try
                    {
                        cmd.ExecuteNonQuery();
                        registros = (string)cmd.ExecuteScalar();
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

            return registros != null;
        }

        public static List<Fornecedor> ListaArquivoFornecedor()
        {
            List<Fornecedor> fornecedores = new List<Fornecedor>();

            using (var conexao = Configuracao.Conexao())
            {
                string sql = $"SELECT CNPJ, Razao_Social, Data_Abertura, Ultima_Compra, Data_Cadastro, Situacao, Bloqueio FROM dbo.Fornecedor";

                conexao.Open();

                using (SqlCommand cmd = new(sql, conexao))
                {
                    try
                    {
                        SqlDataReader dado = cmd.ExecuteReader();

                        while (dado.Read())
                        {
                            _ = DateTime.TryParse(DateTime.Parse(dado.GetValue(2).ToString()).ToString("dd/MM/yyyy"), out DateTime dataAbertura);
                            _ = DateTime.TryParse(DateTime.Parse(dado.GetValue(3).ToString()).ToString("dd/MM/yyyy"), out DateTime ultimaCompra);
                            _ = DateTime.TryParse(DateTime.Parse(dado.GetValue(4).ToString()).ToString("dd/MM/yyyy"), out DateTime dataCadastro);

                            fornecedores.Add(new Fornecedor((string)dado.GetValue(0), (string)dado.GetValue(1), dataAbertura, ultimaCompra, dataCadastro, char.Parse((string)dado.GetValue(5)), (bool)dado.GetValue(6)));
                        }
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

            return fornecedores;
        }

        public static bool ProcurarCNPJBloqueado(string cnpj)
        {
            Fornecedor fornecedor = null;

            using (var conexao = Configuracao.Conexao())
            {
                string sql = $"SELECT CNPJ, Razao_Social, Data_Abertura, Ultima_Compra, Data_Cadastro, Situacao, Bloqueio FROM dbo.Fornecedor WHERE CNPJ='{cnpj}'";

                conexao.Open();

                using (SqlCommand cmd = new(sql, conexao))
                {
                    try
                    {
                        SqlDataReader dado = cmd.ExecuteReader();

                        while (dado.Read())
                        {
                            _ = DateTime.TryParse(DateTime.Parse(dado.GetValue(2).ToString()).ToString("dd/MM/yyyy"), out DateTime dataAbertura);
                            _ = DateTime.TryParse(DateTime.Parse(dado.GetValue(3).ToString()).ToString("dd/MM/yyyy"), out DateTime ultimaCompra);
                            _ = DateTime.TryParse(DateTime.Parse(dado.GetValue(4).ToString()).ToString("dd/MM/yyyy"), out DateTime dataCadastro);

                            fornecedor = new Fornecedor((string)dado.GetValue(0), (string)dado.GetValue(1), dataAbertura, ultimaCompra, dataCadastro, char.Parse((string)dado.GetValue(5)), (bool)dado.GetValue(6));
                        }
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

            return fornecedor.Bloqueio == true;
        }
        #endregion
    }
}
