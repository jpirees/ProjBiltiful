﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CadastrosBasicos;
using CadastrosBasicos.ManipulaArquivos;

namespace VendasProdutos
{
    public class MenuVendas
    {
        public static void SubMenu()
        {
            new Arquivos();

            string opcao;

            do
            {
                Console.Clear();

                Console.WriteLine("=============== VENDAS ===============");
                Console.WriteLine("1. Nova Venda");
                Console.WriteLine("2. Consultar Venda");
                Console.WriteLine("3. Imprimir Registros de Venda");
                Console.WriteLine("--------------------------------------");
                Console.WriteLine("0. Voltar");
                Console.Write("\nEscolha: ");

                switch (opcao = Console.ReadLine())
                {
                    case "1":
                        if (!Read.VerificaListaCliente())
                            NovaVenda();
                        else
                        {
                            Console.WriteLine("Para realizar uma venda sera necessario cadastrar um cliente!");
                            Console.ReadKey();
                        }
                        break;

                    case "2":
                        LocalizarVenda();
                        break;
                    case "3":
                        Venda.ImpressaoPorRegistro();
                        break;
                    case "0":
                        break;

                    default:
                        Console.Clear();
                        Console.WriteLine("Opção inválida");
                        Console.WriteLine("\nPressione ENTER para voltar ao menu");
                        break;
                }
            } while (opcao != "0");
        }


        public static void NovaVenda()
        {
            Console.Clear();

            Cliente cliente;

            Console.WriteLine("informe o CPF do cliente:");
            string cpf = Console.ReadLine();

            if (Read.ProcurarCPFBloqueado(cpf) == true)
            {
                Console.Clear();
                Console.WriteLine("\n Falha ao iniciar a venda. Procure pelo gerente do local.");
                Console.WriteLine("\n Pressione ENTER para voltar ao menu");
                Console.ReadKey();
                return;
            }
            else
            {
                cliente = Read.ProcuraCliente(cpf);

                if (cliente == null)
                {
                    Console.Clear();
                    Console.WriteLine("\nCliente não encontrado");
                    Console.WriteLine("\n Pressione ENTER para voltar ao menu");
                    Console.ReadKey();
                    return;
                }
            }

            Console.Clear();

            Venda venda = new Venda();

            venda.Cliente = cliente.CPF;
            venda.DataVenda = DateTime.Now.Date;

            Console.Write($"Data: {venda.DataVenda:dd/MM/yyyy}");
            Console.WriteLine();

            List<ItemVenda> itensVenda = new List<ItemVenda>();

            int itens = 1;
            string escolha;

            do
            {
                Produto produto;
                int qtd = 0;
                decimal totalItens = 0;

                do
                {

                    Console.WriteLine("\nDigite o Código do Produto:");
                    string codProduto = Console.ReadLine();

                    produto = Produto.RetornaProduto(codProduto);

                    if (produto == null)
                    {
                        Console.WriteLine("\nProduto não encontrado ou código inválido.");
                        Console.ReadKey();
                        Console.Clear();
                        continue;
                    }
                    else if (produto.Situacao.Equals('I'))
                    {
                        Console.WriteLine("\nProduto inativo ou código inválido.");
                        Console.ReadKey();
                        Console.Clear();
                        continue;
                    }

                    Console.WriteLine("\nInforme a quantidade:");
                    qtd = int.Parse(Console.ReadLine());


                    if (qtd <= 0 || qtd > 999)
                    {
                        Console.WriteLine("Informe uma quantidade entre 1 e 999");
                        Console.ReadKey();
                        Console.Clear();
                        continue;
                    }

                    totalItens = qtd * produto.ValorVenda;

                    if (totalItens > (decimal)9999.99)
                    {
                        Console.WriteLine("Valor total dos item passou o limite permitido de $ 9.999,99");
                        Console.ReadKey();
                        Console.Clear();
                        continue;
                    }

                } while ((qtd <= 0 || qtd > 999) || totalItens > (decimal)9999.99 || produto == null);

                Console.Clear();


                itensVenda.Add(new ItemVenda(produto.CodigoBarras, qtd, produto.ValorVenda));

                Console.WriteLine("Produto\t\t\tQtd\tV.Unitário\tT.Item");
                Console.WriteLine("------------------------------------------------------");

                decimal valorTotal = 0;

                itensVenda.ForEach(item =>
                {
                    produto = Produto.RetornaProduto(item.Produto);
                    ItemVenda.Impressao(item, produto);
                    valorTotal += item.TotalItem;
                    venda.ValorTotal = valorTotal;
                });

                Console.WriteLine("------------------------------------------------------");
                Console.WriteLine($"\t\t\t\t\t\t{venda.ValorTotal:#.00}");


                do
                {
                    Console.WriteLine("\nAdicionar novo produto?");
                    Console.WriteLine("[ S ] Sim\t[ N ] Não");
                    escolha = Console.ReadLine().ToUpper();

                    Console.Clear();
                } while (escolha != "S" && escolha != "N");


                if (escolha == "S")
                    itens++;
                else
                    break;

                if (itens == 4)
                {
                    Console.Clear();
                    Console.WriteLine("Seu carrinho está cheio!");
                    Console.ReadKey();
                    break;
                }

            } while (itens != 4);


            do
            {

                Produto produto;

                Console.Clear();
                Console.WriteLine("----------------------------------------------------------");
                Console.WriteLine("                           CLIENTE                        ");
                Console.WriteLine("----------------------------------------------------------");
                Console.WriteLine($"Nome:\t\t{cliente.Nome}");
                Console.WriteLine($"CPF:\t\t{cliente.CPF}");
                Console.WriteLine($"Ultima Compra:\t{cliente.UltimaVenda:dd/MM/yyyy}");
                Console.WriteLine("\n\n----------------------------------------------------------");
                Console.WriteLine($"Data: {venda.DataVenda:dd/MM/yyyy}");
                Console.WriteLine("----------------------------------------------------------");
                Console.WriteLine("\n\nProduto\t\t\tQtd\tV.Unitário\tT.Item");
                Console.WriteLine("----------------------------------------------------------");
                itensVenda.ForEach(item =>
                {
                    produto = Produto.RetornaProduto(item.Produto);
                    ItemVenda.Impressao(item, produto);
                });

                Console.WriteLine("----------------------------------------------------------");
                Console.WriteLine($"\t\t\t\t\t\t{venda.ValorTotal:#.00}");

                Console.WriteLine("\n\n");

                Console.WriteLine("[ F ] Finalizar venda\t[ C ] Cancelar venda");
                escolha = Console.ReadLine().ToUpper();

            } while (escolha != "F" && escolha != "C");

            if (escolha == "F")
            {
                ItemVenda itemVenda = new ItemVenda();

                itensVenda.ForEach(item =>
                {
                    Produto.Atualizar(item.Produto, venda.DataVenda.ToString("dd/MM/yyyy"));
                });

                int idVenda = venda.Cadastrar();

                ItemVenda.Cadastrar(idVenda, itensVenda);


                cliente.UltimaVenda = venda.DataVenda;

                Write.EditarCliente(cliente);

                Console.WriteLine("\n\nVenda cadastrada com sucesso!\nPressione ENTER para voltar ao Menu Vendas...");

                Console.ReadKey();
            }
        }

        public static void LocalizarVenda()
        {
            Console.Clear();

            Produto produto;
            Venda venda = new Venda();
            ItemVenda itemVenda = new ItemVenda();

            Console.WriteLine("Informe a venda que deseja buscar: ");
            _ = int.TryParse(Console.ReadLine(), out int id);
            Console.WriteLine();

            venda = Venda.Localizar(id);

            if (venda != null)
            {
                Cliente cliente = Read.ProcuraCliente(venda.Cliente);
                List<ItemVenda> itens = ItemVenda.Localizar(venda.Id);

                Console.WriteLine("----------------------------------------------------------");
                Console.WriteLine("                           CLIENTE                        ");
                Console.WriteLine("----------------------------------------------------------");
                Console.WriteLine($"Nome:\t\t{cliente.Nome}");
                Console.WriteLine($"CPF:\t\t{cliente.CPF}");
                Console.WriteLine($"Data Nasc.:\t{cliente.DataNascimento:dd/MM/yyyy}");
                Console.WriteLine($"Ultima Compra:\t{cliente.UltimaVenda:dd/MM/yyyy}");
                Console.WriteLine("\n\n----------------------------------------------------------");
                Console.WriteLine($"Venda Nº {venda.Id.ToString().PadLeft(5, '0')}\t\t\tData: {venda.DataVenda.ToString("dd/MM/yyyy")}");
                Console.WriteLine("----------------------------------------------------------");
                Console.WriteLine("\n\nProduto\t\t\tQtd\tV.Unitário\tT.Item");
                Console.WriteLine("----------------------------------------------------------");
                itens.ForEach(item =>
                {
                    produto = Produto.RetornaProduto(item.Produto);
                    ItemVenda.Impressao(item, produto);
                });
                Console.WriteLine("----------------------------------------------------------");
                Console.WriteLine($"\t\t\t\t\t\t{venda.ValorTotal.ToString("#.00")}");

                Console.WriteLine("\nPressione ENTER para voltar ao menu...\n");
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("venda não registrada!\nPressione ENTER para voltar ao menu...");
                Console.ReadLine();
            }
        }

    }
}