using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtheusHotSwap
{
    class Configuracao
    {
        public string AmbienteAtual { get; set; }
        public string CaminhoProtheus { get; set; }
        public string RpoBase { get; set; }

        public static Configuracao ObterConfiguracao(string configFile)
        {
            Configuracao config;
            if (!File.Exists(configFile))
            {
                config = new Configuracao()
                {
                    AmbienteAtual = "PRODUCAO1",
                    CaminhoProtheus = @"C:\Protheus 11\Protheus",
                    RpoBase = @"apo\compilador\TTTP110.RPO"
                };

                string json = JsonConvert.SerializeObject(config);
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
    }
}
