using CadastrosBasicos.ManipulaArquivos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadastrosBasicos
{

    public class MenuCadastros
    {
        public static Write write = new Write();
        public static Read read = new Read();

        public static void SubMenu()
        {
            string escolha;

            do
            {
                Console.Clear();

                Console.WriteLine("=============== CADASTROS ===============");
                Console.WriteLine("1. Clientes / Fornecedores");
                Console.WriteLine("2. Produtos");
                Console.WriteLine("3. Matérias-Primas");
                Console.WriteLine("-----------------------------------------");
                Console.WriteLine("0. Voltar ao menu anterior");
                Console.Write("\nEscolha: ");

                switch(escolha = Console.ReadLine())
                {
                    case "0":
                        break;

                    case "1":
                        SubMenuClientesFornecedores();
                        break;

                    case "2":
                        new Produto().Menu();
                        break;

                    case "3":
                        new MPrima().Menu();
                        break;

                    default:
                        Console.Clear();
                        Console.WriteLine("Opção inválida");
                        Console.WriteLine("\nPressione ENTER para voltar ao menu");
                        break;
                }

            }while(escolha != "0");

        }

        public static void SubMenuClientesFornecedores()
        {
            string escolha;

            do
            {
                Console.Clear();

                Console.WriteLine("=============== CLIENTES / FORNECEDORES ===============");
                Console.WriteLine("1. Cadastar cliente");
                Console.WriteLine("2. Listar clientes");
                Console.WriteLine("3. Editar registro de cliente");
                Console.WriteLine("4. Bloquear/Desbloqueia cliente (Inadimplente)");
                Console.WriteLine("5. Localizar cliente");
                Console.WriteLine("6. Localizar cliente bloqueado");
                Console.WriteLine("-------------------------------------------------------");
                Console.WriteLine("7. Cadastar fornecedor");
                Console.WriteLine("8. Listar fornecedores");
                Console.WriteLine("9. Editar registro de fornecedor");
                Console.WriteLine("10. Bloquear/Desbloqueia fornecedor");
                Console.WriteLine("11. Localizar fornecedor");
                Console.WriteLine("12. Localizar fornecedor bloqueado");
                Console.WriteLine("-------------------------------------------------------");
                Console.WriteLine("0. Voltar ao menu anterior");
                Console.Write("\nEscolha: ");

                switch (escolha = Console.ReadLine())
                {
                    case "0":
                        break;

                    case "1":
                        NovoCliente();
                        break;

                    case "2":
                        Cliente.Navegar();
                        break;

                    case "3":
                        Cliente.Editar();
                        break;

                    case "4":
                        Cliente.BloqueiaCadastro();
                        
                        break;

                    case "5":
                        Cliente.Localizar();
                        break;

                    case "6":
                        Cliente.ClientesBloqueados();
                        break;

                    case "7":
                        NovoFornecedor();
                        break;

                    case "8":
                        Fornecedor.Navegar();
                        break;
                    case "9":
                        Fornecedor.Editar();
                        break;
                    case "10":
                        Fornecedor.BloqueiaFornecedor();
                        break;
                    case "11":
                        Fornecedor.Localizar();
                        break;
                    case "12":
                        Fornecedor.FornecedorBloqueado();
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Opção inválida");
                        Console.WriteLine("\n Pressione ENTER para voltar ao menu");
                        break;
                }

            } while (escolha != "0");
        }

        public static void NovoCliente()
        {
            Console.Clear();

            bool flag;

            DateTime dNascimento;

            do
            {
                Console.Write("Data de nascimento: ");
                flag = DateTime.TryParse(Console.ReadLine(), out dNascimento);
            } while (flag != true);
            if (Validacoes.CalculaData(dNascimento))
            {
                RegistrarCliente(dNascimento);
            }
            else
            {
                Console.WriteLine("Menor de 18 anos nao pode ser cadastrado");
                Console.ReadKey();
            }
                
        }

        public static void NovoFornecedor()
        {
            Console.Clear();

            DateTime dCriacao;
            bool flag;

            do
            {
                Console.Write("Data de criacao da empresa: ");
                _ = DateTime.TryParse(Console.ReadLine(), out dCriacao);

                if (flag = !Validacoes.CalculaCriacao(dCriacao))
                {
                    Console.WriteLine("\n Empresa com menos de 6 meses não deve ser cadastrada.");
                    Console.WriteLine("\n Pressione ENTER para continuar...");
                    _ = Console.ReadKey();
                    return;
                }

            } while (flag == true);

            Fornecedor fornecedor;

            if ((fornecedor = RegistrarFornecedor(dCriacao)) != null)
                Write.GravarNovoFornecedor(fornecedor);
        }

        public static Fornecedor RegistrarFornecedor(DateTime dFundacao)
        {
            string cnpj, rSocial;
            char situacao;

            do
            {
                Console.Write("CNPJ: ");
                cnpj = Console.ReadLine();
            } while (Validacoes.ValidarCnpj(cnpj) == false);

            Fornecedor fornecedor = Read.ProcurarFornecedor(cnpj);

            if (fornecedor != null)
            {
                Console.WriteLine("\n Fornecedor já cadastrado.");
                Console.WriteLine("\n Pressione ENTER para continuar...");
                Console.ReadKey();
                return null;
            }

            Console.Write("Razao social: ");
            rSocial = Console.ReadLine().Trim();

            Console.Write("Situacao (A - Ativo/ I - Inativo): ");
            situacao = char.Parse(Console.ReadLine().ToString().ToUpper().Trim());

            return new Fornecedor(cnpj, rSocial, dFundacao, situacao);
        }

        public static Cliente RegistrarCliente(DateTime dNascimento)
        {
            string cpf;

            char situacao, sexo;
            
            do
            {
                Console.Write("CPF: ");
                cpf = Console.ReadLine().Trim();

            } while (Validacoes.ValidarCpf(cpf) == false);

            Cliente c = Read.ProcuraCliente(cpf);

            if (c == null)
            {
                Console.Write("Nome: ");
                var nome = Console.ReadLine().Trim();

                Console.Write("Genero (M - Masculino/ F - Feminino): ");
                sexo = char.Parse(Console.ReadLine().ToUpper());

                Console.Write("Situacao (A - Ativo/ I - Inativo): ");
                situacao = char.Parse(Console.ReadLine().ToUpper());

                Write.GravarNovoCliente(new Cliente(cpf, nome, dNascimento, sexo, situacao));
            }
            else
            {
                Console.WriteLine("Cliente ja cadastrado!!");
                Console.ReadKey();
                return c;
            }

            return null;
        }
    }
}