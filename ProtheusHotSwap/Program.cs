using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtheusHotSwap
{
    class Program
    {
        static void Main(string[] args)
        {

            try
            {
                string currentEnv = File.ReadLines(@"C:\quente\rpo.txt").First();
                Console.WriteLine("RPO atual : " + currentEnv);

                string newEnv = currentEnv.Trim() == "PRODUCAO2" ? "1" : "2";
                string oriEnv = currentEnv.Trim() == "PRODUCAO2" ? "2" : "1";

                string rpoCompilador = @"C:\Protheus 11\Protheus\apo\compilador\TTTP110.RPO";

                Console.WriteLine("RPO Compilador : " + rpoCompilador);

                string dirNewEnv = @"C:\Protheus 11\Protheus\apo\prod" + newEnv + @"\";

                Console.WriteLine("Atualizar RPOs contidos em : " + dirNewEnv);

                string arquivoDestino = "";

                foreach (var diretorio in Directory.GetDirectories(dirNewEnv))
                {
                    arquivoDestino = Path.Combine(diretorio, Path.GetFileName(rpoCompilador));

                    Console.WriteLine("Copiando para : " + arquivoDestino);
                    File.Copy(rpoCompilador, arquivoDestino, true);
                }

                string arquivoOrigem = "";
                dirNewEnv = @"C:\quente\bin\prod" + newEnv;
                Console.WriteLine("Atualizar INIs contidos em : " + dirNewEnv);
                foreach (var diretorio in Directory.GetDirectories(dirNewEnv))
                {
                    arquivoOrigem = Path.Combine(diretorio, "appserver.ini");
                    string temp = Path.GetDirectoryName(arquivoOrigem);
                    temp = temp.Substring(temp.LastIndexOf(@"\") + 1);

                    arquivoDestino = @"C:\Protheus 11\Protheus\bin\appserver";

                    if (temp.ElementAt(0) == 's')
                    {
                        temp = "S" + temp.Substring(1);
                        arquivoDestino += "_" + temp;
                    }

                    arquivoDestino = Path.Combine(arquivoDestino, Path.GetFileName(arquivoOrigem));

                    Console.WriteLine("Copiando para : " + arquivoDestino);
                    File.Copy(arquivoOrigem, arquivoDestino, true);

                }

                Console.WriteLine("Ambiente foi alterado de: prod" + oriEnv + " para prod" + newEnv);
                Console.WriteLine("Troca quente executada com sucesso.");
            }
            catch (Exception e)
            {

                Console.WriteLine("Um erro inesperado ocorreu. Log abaixo:");
                Console.WriteLine(e.Message);
            }            
            
            Console.ReadKey();
        }
    }
}
