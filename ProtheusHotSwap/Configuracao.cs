using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtheusHotSwap
{
    public class Configuracao
    {
        public string AmbienteAtual { get; set; }
        public string CaminhoProtheus { get; set; }
        public string CaminhoInis { get; set; }
        public string RpoBase { get; set; }
        private string AmbienteOriginal { get; set; }

        public static Configuracao ObterConfiguracao(string configFile)
        {
            Configuracao config;
            if (!File.Exists(configFile))
            {
                config = new Configuracao()
                {
                    AmbienteAtual = "PRODUCAO1",
                    CaminhoInis = @"C:\quente\bin\",
                    CaminhoProtheus = @"C:\Protheus 11\Protheus",
                    RpoBase = @"apo\compilador\TTTP110.RPO"
                };
                
                string json = JsonConvert.SerializeObject(config,Formatting.Indented);
                File.WriteAllText(configFile, json);
            }
            else
            {
                using (StreamReader file = new StreamReader(configFile))
                {
                    string json = file.ReadToEnd();
                    config = JsonConvert.DeserializeObject<Configuracao>(json);
                }
            }
            return config;
        }

        private Configuracao() {


        }

        private string GetCurrentEnv() {            
            return this.AmbienteAtual.Trim() == "PRODUCAO2" ? "2" : "1";
        }

        private string GetNextEnv()
        {
            return this.AmbienteAtual.Trim() == "PRODUCAO2" ? "1" : "2";
        }

        public string GetNextEnvDir() {

            return Path.Combine(this.CaminhoProtheus, @"apo\prod" + this.GetNextEnv() + @"\");
        }

        public string GetNewInisDir() {
            return Path.Combine(this.CaminhoInis, @"prod" + this.GetNextEnv());
        }

        public void ReverseEnv()
        {
            this.AmbienteAtual = "PRODUCAO" + this.GetNextEnv();
        }
    }
}
