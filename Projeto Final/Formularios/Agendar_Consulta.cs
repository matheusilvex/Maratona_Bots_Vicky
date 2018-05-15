using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.FormFlow;
using Projeto_Final.B.D;

namespace Projeto_Final.Formularios
{
    [Serializable]
    [Template(TemplateUsage.NotUnderstood, "Desculpe, não entendi o que foi dito!")]
    public class Agendar_Consulta
    {
        Conexao dadosBD = new Conexao();
        
        public static IForm<Agendar_Consulta> BuildForm()
        {
            var form = new FormBuilder<Agendar_Consulta>();




            return form.Build();
        }
    }

    public struct Medico
    {   
        

    }

    
}