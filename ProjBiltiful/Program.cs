using System;
using System.IO;
using VendasProdutos;
using CadastrosBasicos;
using ProducaoCosmeticos;
using System.Globalization;

namespace ProjBiltiful
{
    internal class Program
    {
        static void Main(string[] args)
        {

            var cultureInformation = new CultureInfo("pt-BR");
            cultureInformation.NumberFormat.CurrencySymbol = "R$";
            CultureInfo.DefaultThreadCurrentCulture = cultureInformation;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInformation;

            int value;

            do
            {
                Console.Clear();
                Console.Write(@"=============== BITIFUL ===============
1. Cadastros
2. Vendas
3. Compra de Materia-Prima
4. Producao
0. Sair
:: ");

                switch (value = int.Parse(Console.ReadLine()))
                {
                    case 0:
                        Environment.Exit(0);
                        break;

                    case 1:
                        MenuCadastros.SubMenu();
                        break;

                    case 2:
                        MenuVendas.SubMenu();
                        break;

                    case 3:
                        break;

                    case 4:
                        break;

                    default:
                        Console.Clear();
                        Console.WriteLine("Opção inválida");
                        Console.ReadKey();
                        break;
                }
            } while (value != 0);
        }
    }
}