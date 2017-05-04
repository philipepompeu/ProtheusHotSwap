using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json;
namespace ProtheusHotSwap
{
    class Program
    {
        static void Main(string[] args)
        {

            try
            {
                string envFile = @"rpo.json";

                Configuracao config = Configuracao.ObterConfiguracao(envFile);                

                string currentEnv = config.AmbienteAtual;
                Console.WriteLine("RPO atual : " + currentEnv);

                string newEnv = currentEnv.Trim() == "PRODUCAO2" ? "1" : "2";
                string oriEnv = currentEnv.Trim() == "PRODUCAO2" ? "2" : "1";                
                string dirNewEnv = Path.Combine(config.CaminhoProtheus, @"apo\prod" + newEnv + @"\");

                TransfereRpos(dirNewEnv, config);

                dirNewEnv = @"C:\quente\bin\prod" + newEnv;
                TransfereInis(dirNewEnv, config);

                config.AmbienteAtual = "PRODUCAO" + newEnv;
                File.WriteAllText(envFile, JsonConvert.SerializeObject(config));
                
                Console.WriteLine("Atualizando o rpo.json: " + newEnv + " substituir " + oriEnv);                

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

        public static void TransfereRpos(string dirNewEnv, Configuracao config)
        {
            string rpoCompilador = Path.Combine(config.CaminhoProtheus, config.RpoBase); 
            Console.WriteLine("RPO Compilador : " + rpoCompilador);            

            Console.WriteLine("Atualizar RPOs contidos em : " + dirNewEnv);

            string arquivoDestino = "";

            foreach (var diretorio in Directory.GetDirectories(dirNewEnv))
            {
                arquivoDestino = Path.Combine(diretorio, Path.GetFileName(rpoCompilador));

                Console.WriteLine("Copiando para : " + arquivoDestino);
                File.Copy(rpoCompilador, arquivoDestino, true);
            }

        }

        public static void TransfereInis(string dirNewEnv, Configuracao config)
        {
            string arquivoDestino = "";
            string arquivoOrigem = "";
            string temp = "";
            Console.WriteLine("Atualizar INIs contidos em : " + dirNewEnv);

            foreach (var diretorio in Directory.GetDirectories(dirNewEnv))
            {
                arquivoOrigem = Path.Combine(diretorio, "appserver.ini");
                temp = Path.GetDirectoryName(arquivoOrigem);
                temp = temp.Substring(temp.LastIndexOf(@"\") + 1);

                arquivoDestino = Path.Combine(config.CaminhoProtheus, @"bin\appserver");

                if (temp.ElementAt(0) == 's')
                {
                    temp = "S" + temp.Substring(1);
                    arquivoDestino += "_" + temp;
                }

                arquivoDestino = Path.Combine(arquivoDestino, Path.GetFileName(arquivoOrigem));

                Console.WriteLine("Copiando para : " + arquivoDestino);
                File.Copy(arquivoOrigem, arquivoDestino, true);

            }

        }
    }
}
