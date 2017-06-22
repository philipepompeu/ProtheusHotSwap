using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtheusHotSwap
{
    public class HotSwap
    {
        private Configuracao config;

        public HotSwap(Configuracao config)
        {

            this.config = config;
        }

        public void Execute()
        {
            this.transfereRpos();
            this.transfereInis();
        }

        private void transfereRpos()
        {
            string rpoCompilador = Path.Combine(config.CaminhoProtheus, config.RpoBase);
            string dirNewEnv = config.GetNextEnvDir();

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

        private void transfereInis()
        {
            string arquivoDestino = "";
            string arquivoOrigem = "";
            string temp = "";
            string dirNewEnv = config.GetNewInisDir();

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
