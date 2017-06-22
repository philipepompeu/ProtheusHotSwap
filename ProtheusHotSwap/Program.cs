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

            bool shouldWait = false;
            shouldWait = !(args.Length > 0);

            try
            {
                string envFile = @"rpo.json";

                Configuracao config = Configuracao.ObterConfiguracao(envFile);

                Console.WriteLine("RPO atual : " + config.AmbienteAtual);

                HotSwap program = new HotSwap(config);

                program.Execute();

                string oriEnv = config.AmbienteAtual;

                config.ReverseEnv();

                string newEnv = config.AmbienteAtual;

                File.WriteAllText(envFile, JsonConvert.SerializeObject(config, Formatting.Indented));                            

                Console.WriteLine("Ambiente foi alterado de: " + oriEnv + " para " + newEnv);
                Console.WriteLine("Troca quente executada com sucesso.");
            }
            catch (Exception e)
            {
                Console.WriteLine("Um erro inesperado ocorreu. Log abaixo:");
                Console.WriteLine(e.Message);
            }

            if (shouldWait)
            {
                Console.ReadKey();
            }
        }
        
    }
}
