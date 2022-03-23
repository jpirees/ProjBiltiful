using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using BancoDados;
using CadastrosBasicos;
using CadastrosBasicos.ManipulaArquivos;

namespace VendasProdutos
{
    public class Venda
    {
        public int Id { get; set; }
        public string Cliente { get; set; }
        public DateTime DataVenda { get; set; }
        public decimal ValorTotal { get; set; }

        public Venda()
        {
            ValorTotal = 0;
        }

        public Venda(int id, string cliente, DateTime dataVenda, decimal vTotal)
        {
            Id = id;
            Cliente = cliente;
            DataVenda = dataVenda;
            ValorTotal = vTotal;
        }

        public int Cadastrar()
        {
            int idVenda = 0;

            using var conexao = Configuracao.Conexao();

            try
            {
                conexao.Open();
                string sql = $"INSERT INTO dbo.Venda (CPF_Cliente, Valor_Total) " +
                    $"OUTPUT INSERTED.ID " +
                    $"VALUES  ('{Cliente}', '{ValorTotal.ToString(new CultureInfo("en-US"))}')";

                SqlCommand cmd = new SqlCommand(sql, conexao);
                idVenda = (int) cmd.ExecuteScalar();
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

            return idVenda;
        }

        public static Venda Localizar(int id)
        {
            Venda venda = null;

            using (var conexao = Configuracao.Conexao())
            {
                string sql = $"SELECT ID, CPF_Cliente, Data_Venda, Valor_Total FROM dbo.Venda WHERE ID='{id}'";

                conexao.Open();

                using (SqlCommand cmd = new(sql, conexao))
                {
                    try
                    {
                        SqlDataReader dado = cmd.ExecuteReader();

                        while (dado.Read())
                        {
                            _ = DateTime.TryParse(DateTime.Parse(dado.GetValue(2).ToString()).ToString("dd/MM/yyyy"), out DateTime dataVenda);

                            venda = new Venda((int)dado.GetValue(0), (string)dado.GetValue(1), dataVenda, (decimal)dado.GetValue(3));
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

            return venda;
        }

        public static bool VerificaListaVenda()
        {
            int? registros = null;

            using (var conexao = Configuracao.Conexao())
            {
                string sql = $"SELECT MAX(ID) FROM dbo.Venda";

                conexao.Open();

                using (SqlCommand cmd = new(sql, conexao))
                {
                    try
                    {
                        cmd.ExecuteNonQuery();
                        registros = (int)cmd.ExecuteScalar();
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

        public static void ImpressaoPorRegistro()
        {
            Console.Clear();

            if (!VerificaListaVenda())
            {
                Console.Clear();
                Console.WriteLine("\nNão há vendas para exibir.");
                Console.WriteLine("\nPressione ENTER para voltar...");
                Console.ReadLine();
                return;
            }

            List<Venda> dados = new List<Venda>();

            using (var conexao = Configuracao.Conexao())
            {
                string sql = $"SELECT ID, CPF_Cliente, Data_Venda, Valor_Total FROM dbo.Venda";

                conexao.Open();

                using (SqlCommand cmd = new(sql, conexao))
                {
                    try
                    {
                        SqlDataReader dado = cmd.ExecuteReader();

                        while (dado.Read())
                        {
                            _ = DateTime.TryParse(DateTime.Parse(dado.GetValue(2).ToString()).ToString("dd/MM/yyyy"), out DateTime dataVenda);

                            dados.Add(new Venda((int)dado.GetValue(0), (string)dado.GetValue(1), dataVenda, (decimal)dado.GetValue(3)));
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

            var i = 0;
            string choice;
            ItemVenda itemVenda = new ItemVenda();

            do
            {
                Produto produto;
                Venda venda = new Venda(dados[i].Id, dados[i].Cliente, dados[i].DataVenda, dados[i].ValorTotal);

                Cliente cliente = Read.ProcuraCliente(venda.Cliente);

                List<ItemVenda> itens = ItemVenda.Localizar(venda.Id);

                Console.Clear();
                Console.WriteLine("----------------------------------------------------------");
                Console.WriteLine("                           CLIENTE                        ");
                Console.WriteLine("----------------------------------------------------------");
                Console.WriteLine($"Nome:\t\t{cliente.Nome}");
                Console.WriteLine($"CPF:\t\t{cliente.CPF}");
                Console.WriteLine($"Ultima Compra:\t{cliente.UltimaVenda:dd/MM/yyyy}");
                Console.WriteLine("\n\n----------------------------------------------------------");
                Console.WriteLine($"Venda Nº {venda.Id.ToString().PadLeft(5, '0')}\t\t\tData: {venda.DataVenda:dd/MM/yyyy}");
                Console.WriteLine("----------------------------------------------------------");
                Console.WriteLine("\n\nProduto\t\t\tQtd\tV.Unitário\tT.Item");
                Console.WriteLine("----------------------------------------------------------");
                itens.ForEach(item =>
                {
                    produto = Produto.RetornaProduto(item.Produto);
                    ItemVenda.Impressao(item, produto);
                });
                Console.WriteLine("----------------------------------------------------------");
                Console.WriteLine($"\t\t\t\t\t\t{venda.ValorTotal}");

                Console.WriteLine("\n\n");
                Console.WriteLine("1 - Proximo\t2 - Anterior\t3 - Primeiro\t4 - Ultimo\t0 - Cancelar");
                choice = Console.ReadLine();
                Console.Clear();
                switch (choice)
                {
                    case "1":
                        if (i == dados.Count - 1)
                            i = dados.Count - 1;
                        else
                            i++;
                        break;

                    case "2":
                        if (i == 0)
                            i = 0;
                        else
                            i--;
                        break;

                    case "3":
                        i = 0;
                        break;

                    case "4":
                        i = dados.Count - 1;
                        break;
                    case "0":
                        break;
                    default:
                        Console.WriteLine("Opção invalida. Tente novamente.");
                        break;
                }
            } while (choice != "0");
        }
    }
}
