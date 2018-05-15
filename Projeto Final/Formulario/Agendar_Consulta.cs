using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.FormFlow;
using Projeto_Final.B.D;
using System.Collections.Generic;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Builder.Luis;

namespace Projeto_Final.Formularios
{

    [Serializable]
    //[Template(TemplateUsage.NotUnderstood, "Desculpe, não entendi o que foi dito!")]
    public class Agendar_Consulta
    {
        public string Medico { get; set; }
        public string CPF { get; set; }
        public string Nome { get; set; }
        public string DT_Nasc { get; set; }

    }
}