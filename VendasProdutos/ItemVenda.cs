using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using BancoDados;

namespace VendasProdutos
{
    public class ItemVenda
    {
        private static Arquivos caminho = new Arquivos();

        public int Id { get; set; }
        public string Produto { get; set; }
        public int Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal TotalItem { get; set; }

        public ItemVenda() { }

        public ItemVenda(int id, string produto, int quantidade, decimal valorUnitario)
        {
            Id = id;
            Produto = produto;
            Quantidade = quantidade;
            ValorUnitario = valorUnitario;
            TotalItem = quantidade * valorUnitario;
        }

        public ItemVenda(string produto, int quantidade, decimal valorUnitario)
        {
            Produto = produto;
            Quantidade = quantidade;
            ValorUnitario = valorUnitario;
            TotalItem = quantidade * valorUnitario;
        }

        public ItemVenda(int id, string produto, int quantidade, decimal valorUnitario, decimal totalItem)
        {
            Id = id;
            Produto = produto;
            Quantidade = quantidade;
            ValorUnitario = valorUnitario;
            TotalItem = totalItem;
        }

        public override string ToString()
        {
            return $"{Produto,-20}\t{Quantidade.ToString().PadLeft(3, '0')}\t{ValorUnitario.ToString("000.00").TrimStart('0')}\t\t{TotalItem.ToString("0000.00").TrimStart('0')}";
        }

        public static void Impressao(ItemVenda item, CadastrosBasicos.Produto p)
        {
            Console.WriteLine($"{p.Nome,-20}\t{item.Quantidade.ToString().PadLeft(3, '0')}\t{item.ValorUnitario,-6}\t\t{item.TotalItem,-7}");
        }

        public static void Cadastrar(int idVenda, List<ItemVenda> itens)
        {
            string sql = $"INSERT INTO dbo.ItemVenda (ID_Venda, Codigo_Produto, Quantidade, Valor_Unitario, Total_Item) VALUES ";

            for (int i = 0; i < itens.Count; i++)
            {
                if (i != itens.Count - 1)
                {
                    sql += $"('{idVenda}', '{itens[i].Produto}', '{itens[i].Quantidade}', '{itens[i].ValorUnitario.ToString(new CultureInfo("en-US"))}', '{itens[i].TotalItem.ToString(new CultureInfo("en-US"))}'), ";
                    continue;
                }

                sql += $"('{idVenda}', '{itens[i].Produto}', '{itens[i].Quantidade}', '{itens[i].ValorUnitario.ToString(new CultureInfo("en-US"))}', '{itens[i].TotalItem.ToString(new CultureInfo("en-US"))}'); ";

            }

            using (var conexao = Configuracao.Conexao())
            {

                conexao.Open();

                using (SqlCommand cmd = new(sql, conexao))
                {
                    try
                    {
                        SqlDataReader dado = cmd.ExecuteReader();

                        while (dado.Read())
                        {
                            itens.Add(new ItemVenda((int)dado.GetValue(0), (string)dado.GetValue(1), (int)dado.GetValue(2), (decimal)dado.GetValue(3), (decimal)dado.GetValue(4)));
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
        }

        public static List<ItemVenda> Localizar(int idVenda)
        {
            List<ItemVenda> itens = new List<ItemVenda>();

            using (var conexao = Configuracao.Conexao())
            {
                string sql = $"SELECT ID_Venda, Codigo_Produto, Quantidade, Valor_Unitario, Total_Item FROM dbo.ItemVenda WHERE ID_Venda='{idVenda}'";

                conexao.Open();

                using (SqlCommand cmd = new(sql, conexao))
                {
                    try
                    {
                        SqlDataReader dado = cmd.ExecuteReader();

                        while (dado.Read())
                        {
                            itens.Add(new ItemVenda((int)dado.GetValue(0), (string)dado.GetValue(1), (int)dado.GetValue(2), (decimal)dado.GetValue(3), (decimal)dado.GetValue(4)));
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

            return itens;
        }

    }
}
