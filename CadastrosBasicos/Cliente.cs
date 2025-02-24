﻿using CadastrosBasicos.ManipulaArquivos;
using System;
using System.Collections.Generic;

namespace CadastrosBasicos
{
    public class Cliente
    {
        public Write write = new Write();
        public Read read = new Read();
        public string CPF { get; private set; }
        public string Nome { get; set; }
        public DateTime DataNascimento { get; set; }
        public char Sexo { get; set; }
        public DateTime UltimaVenda { get; set; }
        public DateTime DataCadastro { get; set; }
        public char Situacao { get; set; }
        public bool? Risco { get; set; }

        public Cliente()
        {

        }

        public Cliente(string cpf, string name, DateTime dataNascimento, char sexo, char situacao)
        {
            CPF = cpf;
            Nome = name;
            DataNascimento = dataNascimento;
            Sexo = sexo;
            UltimaVenda = DateTime.Now;
            DataCadastro = DateTime.Now;
            Situacao = situacao;
        }
        public Cliente(string cpf, string name, DateTime dataNascimento, char sexo, DateTime UltimaCompra, DateTime dataCadastro, char situacao, bool? risco)
        {
            CPF = cpf;
            Nome = name;
            DataNascimento = dataNascimento;
            Sexo = sexo;
            UltimaVenda = UltimaCompra;
            DataCadastro = dataCadastro;
            Situacao = situacao;
            Risco = risco;
        }


        public static void BloqueiaCadastro()
        {
            Console.Clear();

            Cliente cliente;

            Console.WriteLine("Insira o CPF para bloqueio: ");
            string cpf = Console.ReadLine();

            if (Read.ProcurarCPFBloqueado(cpf))
            {
                bool flag = false;
                int opcao;

                Console.WriteLine("\nCliente já está bloqueado.");
                Console.WriteLine("\nDeseja desbloqueá-lo ? [1 - Sim | 2 - Nao]");

                do
                {
                    flag = int.TryParse(Console.ReadLine(), out opcao);
                } while (flag != true);

                if (opcao == 1)
                {
                    Write.DesbloqueiaCliente(cpf);
                    Console.WriteLine("\n Cliente desbloqueado.");
                    Console.WriteLine("\n Pressione ENTER para continuar...");
                    Console.ReadKey();
                }
            }
            else
            {
                if (Validacoes.ValidarCpf(cpf))
                {
                    cliente = Read.ProcuraCliente(cpf);
                    if (cliente != null)
                    {
                        Write.BloqueiaCliente(cliente.CPF);
                        Console.WriteLine("\nCPF bloqueado!");
                        Console.ReadKey();
                    }
                }
                else
                {
                    Console.WriteLine("\nCPF incorreto!");
                }
            }
        }
        public static Cliente Editar()
        {
            Console.Clear();
            Cliente cliente;
            Console.WriteLine(" Somente algumas informações podem ser alteradas como: Nome | Data de Nascimento | Situação");
            Console.WriteLine(" caso não queira alterar alguma informação pressione ENTER!");
            Console.WriteLine();
            Console.WriteLine();
            Console.Write("CPF: ");
            string cpf = Console.ReadLine();

            cliente = Read.ProcuraCliente(cpf);

            if (cliente != null)
            {
                Console.WriteLine("Nome: ");
                string nome = Console.ReadLine().Trim();

                Console.WriteLine("Data de nascimento: ");
                bool flag = DateTime.TryParse(Console.ReadLine(), out DateTime dNascimento);

                Console.WriteLine("Situacao [A - Ativo/I - Inativo]: ");
                bool flagSituacao = char.TryParse(Console.ReadLine().Trim().ToUpper(), out char situacao);

                cliente.Nome = nome == "" ? cliente.Nome : nome;
                cliente.DataNascimento = !flag? cliente.DataCadastro : dNascimento;
                cliente.Situacao = !flagSituacao? cliente.Situacao : situacao;

                Write.EditarCliente(cliente);

                Console.WriteLine("\n Cliente atualizado com sucesso.");
                Console.WriteLine("\n Pressione ENTER para continuar...");
                Console.ReadKey();
            }

            return cliente;
        }

        public static void Navegar()
        {
            Console.WriteLine("============== Cliente ==============");

            bool verificaArquivo = Read.VerificaListaCliente();

            if (!verificaArquivo)
            {
                List<Cliente> lista = Read.ListaArquivoCliente();
                int opcao = 0, posicao = 0;
                bool flag = false;
                do
                {
                    Console.Clear();
                    Console.WriteLine("============== Cliente ==============");

                    if (opcao == 0)
                    {
                        Console.WriteLine(lista[posicao].ToString());
                    }
                    else if (opcao == 1)
                    {
                        if (posicao == lista.Count - 1)
                            posicao = lista.Count - 1;
                        else
                            posicao++;
                        Console.WriteLine(lista[posicao].ToString());
                    }
                    else if (opcao == 2)
                    {
                        if (posicao == 0)
                            posicao = 0;
                        else
                            posicao--;
                        Console.WriteLine(lista[posicao].ToString());
                    }
                    else if (opcao == 3)
                    {
                        posicao = 0;
                        Console.WriteLine(lista[posicao].ToString());
                    }
                    else if (opcao == 4)
                    {
                        posicao = lista.Count - 1;
                        Console.WriteLine(lista[posicao].ToString());
                    }


                    Console.WriteLine(@"
1. Proximo 
2. Anterior
3. Primeiro
4. Ultimo
0. Voltar para menu anterior.
");
                    do
                    {
                        flag = int.TryParse(Console.ReadLine(), out opcao);
                    } while (flag != true);

                } while (opcao != 0);

            }
            else
            {
                Console.Clear();
                Console.WriteLine("\n Ainda não há clientes cadastrados.");
                Console.WriteLine("\n Pressione ENTER para continuar...");
                Console.ReadKey();
            }
        }
        public static void Localizar()
        {
            Console.Clear();

            Console.WriteLine("Insira o cpf para localizar: ");
            string cpf = Console.ReadLine();

            Cliente cliente = Read.ProcuraCliente(cpf);

            if (cliente == null)
            {
                Console.WriteLine("\n Nenhum cadastrado encontrado!");
                Console.WriteLine("\n Pressione ENTER para continuar...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine(cliente.ToString());
            Console.ReadKey();
        }
        public static void ClientesBloqueados()
        {
            Console.Clear();

            Console.WriteLine("Insira o CPF para pesquisa: ");
            string cpf = Console.ReadLine();
            bool flag = Read.ProcurarCPFBloqueado(cpf);

            if (flag)
            {
                Cliente cliente = Read.ProcuraCliente(cpf);
                Console.WriteLine(cliente.ToString());
            }
            else
            {
                Console.WriteLine("\n Cliente bloqueado não encontrado");
            }

            Console.WriteLine("\n Pressione ENTER para continuar...");
            Console.ReadKey();
        }
        public override string ToString()
        {
            return $"CPF: {CPF}\nNome: {Nome.Trim()}\nData de nascimento: {DataNascimento.ToString("dd/MM/yyyy")}\nSexo: {Sexo}\nUltima Compra: {UltimaVenda.ToString("dd/MM/yyyy")}\nDia de Cadastro: {DataCadastro.ToString("dd/MM/yyyy")}\nSituacao: {Situacao}";
        }

    }
}
