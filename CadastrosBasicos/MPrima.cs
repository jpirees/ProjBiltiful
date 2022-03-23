using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BancoDados;
using System.Data.SqlClient;

namespace CadastrosBasicos
{
    public class MPrima
    {
        public string Id { get; set; }
        public string Nome { get; set; }
        public DateTime UltimaCompra { get; set; }
        public DateTime DataCadastro { get; set; }
        public char Situacao { get; set; }

        public MPrima()
        {

        }

        public MPrima(string id, string nome, DateTime uCompra, DateTime dCadastro, char situacao)
        {
            Id = id;
            Nome = nome;
            UltimaCompra = uCompra;
            DataCadastro = dCadastro;
            Situacao = situacao;
        }

        public override string ToString()
        {
            return Id
                + Nome.PadLeft(20, ' ')
                + UltimaCompra.ToString("dd/MM/yyyy").Replace("/", "")
                + DataCadastro.ToString("dd/MM/yyyy").Replace("/", "")
                + Situacao;
        }

        public void Menu()
        {
            string escolha;

            do
            {
                Console.Clear();
                Console.WriteLine("\n=============== MATÉRIA-PRIMA ===============");
                Console.WriteLine("1. Cadastrar Matéria-Prima");
                Console.WriteLine("2. Localizar Matéria-Prima");
                Console.WriteLine("3. Imprimir Matérias-Primas");
                Console.WriteLine("4. Alterar Situação da Matéria-Prima");
                Console.WriteLine("---------------------------------------------");
                Console.WriteLine("0. Voltar ao menu anterior");
                Console.Write("\nEscolha: ");

                switch (escolha = Console.ReadLine())
                {
                    case "0":
                        break;
                    case "1":
                        Cadastrar();
                        break;
                    case "2":
                        Localizar();
                        break;
                    case "3":
                        ImprimirMPrimas();
                        break;
                    case "4":
                        AlterarSituacao();
                        break;

                    default:
                        Console.WriteLine("\n Opção inválida.");
                        Console.WriteLine("\n Pressione ENTER para voltar ao menu.");
                        Console.ReadKey();
                        break;
                }

            } while (escolha != "0");
        }

        public static void Cadastrar()
        {
            MPrima MPrima = new MPrima();

            char sit = 'A';
            string nomeTemp;
            bool flag = true;

            do
            {
                Console.Clear();
                Console.WriteLine("\n Cadastro de Materia-prima\n");
                Console.Write("Nome: ");
                nomeTemp = Console.ReadLine();
                Console.Write("Situacao (A / I): ");
                sit = char.Parse(Console.ReadLine().ToUpper());

                if (nomeTemp == null)
                {
                    Console.WriteLine("\n Nenhum campo podera ser vazio.");
                    Console.WriteLine("\n Pressione ENTER para voltar ao cadastro...");
                    Console.ReadKey();
                }
                else
                {
                    if (nomeTemp.Length > 20)
                    {
                        Console.WriteLine("\n Nome inválido. Digite apenas 20 caracteres.");
                        Console.WriteLine("\n Pressione ENTER para voltar ao cadastro...");
                        Console.ReadKey();
                    }
                    else if ((sit != 'A') && (sit != 'I'))
                    {
                        Console.WriteLine("\n Situação inválida.");
                        Console.WriteLine("\n Pressione ENTER para voltar ao cadastro...");
                        Console.ReadKey();
                    }
                    else
                    {
                        flag = false;

                        MPrima.Nome = nomeTemp;
                        MPrima.UltimaCompra = DateTime.Now.Date;
                        MPrima.DataCadastro = DateTime.Now.Date;
                        MPrima.Situacao = sit;

                        GravarMateriaPrima(MPrima);

                        Console.WriteLine("\n Cadastro de Materia-prima concluido com sucesso!\n");
                        Console.WriteLine("\n Pressione ENTER para voltar ao menu...");
                        Console.ReadKey();
                    }
                }

            } while (flag);
        }

        public static void GravarMateriaPrima(MPrima mprima)
        {
            _ = DateTime.TryParse(mprima.UltimaCompra.ToString("yyyy-MM-dd"), out DateTime UltimaCompra);
            _ = DateTime.TryParse(mprima.DataCadastro.ToString("yyyy-MM-dd"), out DateTime DataCadastro);

            _ = new Configuracao();

            using var conexao = Configuracao.Conexao();

            try
            {
                conexao.Open();
                string sql = $"INSERT INTO dbo.MateriaPrima (Nome, Ultima_Compra, Data_Cadastro, Situacao) VALUES  ('{mprima.Nome}', '{UltimaCompra}', '{DataCadastro}', '{mprima.Situacao}')";

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

        public static void Localizar()
        {
            MPrima mPrima;

            string cod;

            Console.Clear();
            Console.WriteLine("\n Localizar Materia-prima");
            Console.Write("\n Digite o codigo da materia-prima: ");
            cod = Console.ReadLine().ToUpper();

            if (cod.Length != 6)
            {
                Console.WriteLine("\n Código inválido. Código composto de 6 digitos");
                Console.WriteLine("\n Pressione ENTER para voltar...");
                Console.ReadKey();
                return;
            }
            else if (cod.Substring(0, 2) != "MP")
            {
                Console.WriteLine("\n Código inválido. Código inicia com MP");
                Console.WriteLine("\n Pressione ENTER para voltar...");
                Console.ReadKey();
                return;
            }

            string codN = cod.Substring(2, 4).ToString().TrimStart('0');

            mPrima = Buscar(codN);

            if (mPrima == null)
            {
                Console.WriteLine("\n A materia-prima nao existe.");
                Console.WriteLine("\n Pressione ENTER para voltar ao menu");
                Console.ReadKey();
            }
            else
            {
                string situacao = mPrima.Situacao.ToString();
                if (situacao == "A")
                    situacao = "Ativo";
                else if (situacao == "I")
                    situacao = "Inativo";

                Console.WriteLine("\n A materia-prima foi encontrada.\n");
                Console.WriteLine($" Codigo: MP{mPrima.Id}");
                Console.WriteLine($" Nome: {mPrima.Nome}");
                Console.WriteLine($" Data ultima compra: {mPrima.UltimaCompra:dd/MM/yyyy}");
                Console.WriteLine($" Data do cadastro: {mPrima.DataCadastro:dd/MM/yyyy}");
                Console.WriteLine($" Situacao: {situacao}");
                Console.WriteLine("\n Pressione ENTER para voltar ao menu...");
                Console.ReadKey();
            }
        }

        public static MPrima Buscar(string cod)
        {
            MPrima mprima = null;

            _ = new Configuracao();

            using (var conexao = Configuracao.Conexao())
            {
                string sql = $"SELECT Codigo, Nome, Ultima_Compra, Data_Cadastro, Situacao FROM dbo.MateriaPrima WHERE Codigo='{cod}'";

                conexao.Open();

                using (SqlCommand cmd = new(sql, conexao))
                {
                    try
                    {
                        SqlDataReader dado = cmd.ExecuteReader();

                        while (dado.Read())
                        {
                            _ = DateTime.TryParse(DateTime.Parse(dado.GetValue(2).ToString()).ToString("dd/MM/yyyy"), out DateTime ultimaCompra);
                            _ = DateTime.TryParse(DateTime.Parse(dado.GetValue(3).ToString()).ToString("dd/MM/yyyy"), out DateTime dataCadastro);



                            mprima = new MPrima(dado.GetValue(0).ToString().PadLeft(4, '0'), (string)dado.GetValue(1), ultimaCompra, dataCadastro, char.Parse((string)dado.GetValue(4)));
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

            return mprima;
        }

        public static void AlterarSituacao()
        {
            MPrima mPrima;
            string cod, situacao;
            bool flag = true;

            Console.Clear();
            Console.WriteLine("\n Alterar Materia-prima");
            Console.Write("\n Digite o codigo da materia-prima: ");
            cod = Console.ReadLine().ToUpper();

            if (cod.Length != 6)
            {
                Console.WriteLine("\n Código inválido. Código composto de 6 digitos");
                Console.WriteLine("\n Pressione ENTER para voltar...");
                Console.ReadKey();
                return;
            }
            else if (cod.Substring(0, 2) != "MP")
            {
                Console.WriteLine("\n Código inválido. Código inicia com MP");
                Console.WriteLine("\n Pressione ENTER para voltar...");
                Console.ReadKey();
                return;
            }

            string codN = cod.Substring(2, 4).ToString().TrimStart('0');

            mPrima = Buscar(codN);

            if (mPrima == null)
            {
                Console.WriteLine("\n A materia-prima nao existe.");
                Console.WriteLine("\n Pressione ENTER para voltar ao menu");
                Console.ReadKey();
            }
            else
            {
                situacao = mPrima.Situacao.ToString();
                if (situacao == "A")
                    situacao = "Ativo";
                else if (situacao == "I")
                    situacao = "Inativo";

                Console.WriteLine("\n A materia-prima foi encontrada.\n");
                Console.WriteLine($" Codigo: MP{mPrima.Id}");
                Console.WriteLine($" Nome: {mPrima.Nome}");
                Console.WriteLine($" Data ultima compra: {mPrima.UltimaCompra:dd/MM/yyyy}");
                Console.WriteLine($" Data do cadastro: {mPrima.DataCadastro:dd/MM/yyyy}");
                Console.WriteLine($" Situacao: {situacao}");

                do
                {
                    Console.Write("\n Qual a nova situacao da materia-prima (A / I): ");
                    situacao = Console.ReadLine().ToUpper();

                    if ((situacao != "A") && (situacao != "I"))
                    {
                        Console.WriteLine("\n Situacao invalida.");
                        Console.WriteLine("\n Pressione ENTER para voltar ao cadastro.");
                        Console.ReadKey();
                    }
                    else
                    {
                        flag = false;
                    }

                } while (flag);

                Atualizar(codN, null, situacao);
            }
        }

        public static void Atualizar(string cod, string dataUltimaCompra = null, string situacaoAtualizada = null)
        {
            MPrima mPrima;

            mPrima = Buscar(cod);

            if (mPrima == null)
            {
                Console.WriteLine("\n A materia-prima nao existe.");
                Console.WriteLine("\n Pressione ENTER para voltar");
                Console.ReadKey();
            }
            else
            {
                _ = new Configuracao();

                using var conexao = Configuracao.Conexao();

                string sql = $"UPDATE dbo.MateriaPrima SET ";

                if (situacaoAtualizada != null)
                    sql += $"Situacao = '{situacaoAtualizada}' ";

                if (dataUltimaCompra != null)
                {
                    _ = DateTime.TryParse(DateTime.Parse(dataUltimaCompra).ToString("yyyy-MM-dd"), out DateTime ultimaCompra);
                    sql += $"Ultima_Venda = CONVERT(DATE, '{ultimaCompra}') ";
                }

                sql += $"WHERE Codigo='{cod}'";

                try
                {
                    conexao.Open();

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
        }


        public static bool VerificaListaMPrima()
        {
            int registros = 0;

            _ = new Configuracao();

            using (var conexao = Configuracao.Conexao())
            {
                string sql = $"SELECT COUNT(Codigo) FROM dbo.MateriaPrima";

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

            return registros != 0;
        }

        public static void ImprimirMPrimas()
        {
            List<MPrima> mPrimas = new List<MPrima>();

            if (VerificaListaMPrima())
            {
                _ = new Configuracao();

                using (var conexao = Configuracao.Conexao())
                {
                    string sql = $"SELECT Codigo, Nome, Ultima_Compra, Data_Cadastro, Situacao FROM dbo.MateriaPrima";

                    conexao.Open();

                    using (SqlCommand cmd = new(sql, conexao))
                    {
                        try
                        {
                            SqlDataReader dado = cmd.ExecuteReader();

                            while (dado.Read())
                            {
                                _ = DateTime.TryParse(DateTime.Parse(dado.GetValue(2).ToString()).ToString("dd/MM/yyyy"), out DateTime ultimaCompra);
                                _ = DateTime.TryParse(DateTime.Parse(dado.GetValue(3).ToString()).ToString("dd/MM/yyyy"), out DateTime dataCadastro);

                                mPrimas.Add(new MPrima(dado.GetValue(0).ToString().PadLeft(4, '0'), (string)dado.GetValue(1), ultimaCompra, dataCadastro, char.Parse((string)dado.GetValue(4))));
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



                string escolha;
                int opcao = 1, posicao = 0;
                bool flag = true;

                do
                {
                    if ((opcao < 1) || (opcao > 5))
                    {
                        Console.WriteLine("\n Opção inválida.");
                        Console.WriteLine("\n Pressione ENTER para voltar.");
                        Console.ReadKey();
                        opcao = 1;
                    }
                    else
                    {
                        if (opcao == 5)
                        {
                            flag = false;
                            return;
                        }
                        else if (opcao == 1)
                        {
                            Console.Clear();
                            Console.WriteLine("\n Impressão de Materias-primas");
                            Console.WriteLine(" --------------------------- ");
                            posicao = mPrimas.IndexOf(mPrimas.First());
                            Console.WriteLine($"\n Materia-prima {posicao + 1}");
                            Console.WriteLine(Impressao(mPrimas.First()));
                        }
                        else if (opcao == 4)
                        {
                            Console.Clear();
                            Console.WriteLine("\n Impressão de Materias-primas");
                            Console.WriteLine(" --------------------------- ");
                            posicao = mPrimas.IndexOf(mPrimas.Last());
                            Console.WriteLine($"\n Materia-prima {posicao + 1}");
                            Console.WriteLine(Impressao(mPrimas.Last()));
                        }
                        else if (opcao == 2)
                        {
                            if (posicao == 0)
                            {
                                Console.Clear();
                                Console.WriteLine("\n Impressão de Materias-primas");
                                Console.WriteLine(" --------------------------- ");
                                Console.WriteLine("\n Não há materia-prima anterior.\n");
                                Console.WriteLine(" --------------------------- ");
                                posicao = mPrimas.IndexOf(mPrimas.First());
                                Console.WriteLine($"\n Materia-prima {posicao + 1}");
                                Console.WriteLine(Impressao(mPrimas.First()));
                            }
                            else
                            {
                                Console.Clear();
                                Console.WriteLine("\n Impressão de Materias-primas");
                                Console.WriteLine(" --------------------------- ");
                                posicao--;
                                Console.WriteLine($"\n Materia-prima {posicao + 1}");
                                Console.WriteLine(Impressao(mPrimas[posicao]));
                                posicao = mPrimas.IndexOf(mPrimas[posicao]);
                            }
                        }
                        else if (opcao == 3)
                        {
                            if (posicao == mPrimas.IndexOf(mPrimas.Last()))
                            {
                                Console.Clear();
                                Console.WriteLine("\n Impressão de Materias-primas");
                                Console.WriteLine(" --------------------------- ");
                                Console.WriteLine("\n Não há proxima materia-prima.\n");
                                Console.WriteLine(" --------------------------- ");
                                Console.WriteLine($"\n Materia-prima {posicao + 1}");
                                Console.WriteLine(Impressao(mPrimas.Last()));
                                posicao = mPrimas.IndexOf(mPrimas.Last());
                            }
                            else
                            {
                                Console.Clear();
                                Console.WriteLine("\n Impressão de Materias-primas");
                                Console.WriteLine(" --------------------------- ");
                                posicao++;
                                Console.WriteLine($"\n Materia-prima {posicao + 1}");
                                Console.WriteLine(Impressao(mPrimas[posicao]));
                                posicao = mPrimas.IndexOf(mPrimas[posicao]);
                            }
                        }

                        Console.WriteLine(" ------------------------------------------------------------------ ");
                        Console.WriteLine("\n Navegação\n");
                        Console.WriteLine(" 1 - Primeira / 2 - Anterior / 3 - Proxima / 4 - Ultima / 5 - Sair");
                        Console.Write("\n Escolha: ");
                        escolha = Console.ReadLine();
                        _ = int.TryParse(escolha, out opcao);
                    }

                } while (flag);
            }
            else
            {
                Console.WriteLine("\n Não há matérias-primas cadastradas.\n");
                Console.WriteLine("\n Pressione ENTER para voltar...");
                Console.ReadKey();
            }
        }

        public static string Impressao(MPrima mPrima)
        {
            string situacao = "";
            if (mPrima.Situacao == 'A')
                situacao = "Ativo";
            else if (mPrima.Situacao == 'I')
                situacao = "Inativo";

            return "\n"
                + "\n Codigo: \t" + mPrima.Id
                + "\n Nome: \t" + mPrima.Nome
                + "\n Ultima Venda: \t" + mPrima.UltimaCompra.ToString("dd/MM/yyyy")
                + "\n Data Cadastro: " + mPrima.DataCadastro.ToString("dd/MM/yyyy")
                + "\n Situacao: \t" + situacao
                + "\n";
        }

        public static MPrima RetornaMateriaPrima(string cod)
        {
            string caminhoFinal = Path.Combine(Directory.GetCurrentDirectory(), "DataBase");
            Directory.CreateDirectory(caminhoFinal);

            string arquivoFinal = Path.Combine(caminhoFinal, "Materia.dat");

            MPrima MPrima = null;

            if (File.Exists(arquivoFinal))
            {
                try
                {
                    using (StreamReader sr = new StreamReader(arquivoFinal))
                    {
                        string line = sr.ReadLine();
                        do
                        {
                            if (line.Substring(0, 6) == cod)
                                MPrima =
                                    new MPrima(
                                        line.Substring(0, 6),
                                        line.Substring(6, 20),
                                        Convert.ToDateTime(line.Substring(26, 8).Insert(2, "/").Insert(5, "/")).Date,
                                        Convert.ToDateTime(line.Substring(34, 8).Insert(2, "/").Insert(5, "/")).Date,
                                        Convert.ToChar(line.Substring(42, 1))
                                        );

                            line = sr.ReadLine();
                        } while (line != null);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ex ->" + ex.Message);
                }
            }
            return MPrima;
        }
    }
}

