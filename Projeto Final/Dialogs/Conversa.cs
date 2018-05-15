using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Projeto_Final.B.D;
using Microsoft.Bot.Builder.FormFlow;
using Projeto_Final.Formularios;

namespace Projeto_Final.Dialogs
{
    [Serializable]
    public class Conversa : LuisDialog<object>
    {
        public Conversa(ILuisService service) : base(service) { }

        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Desculpe, não entendi o que voce disse. Lembrese que sou apenas um BOT. Veja o que posso fazer: \n" +
                            "* **Medicos Disponiveis** \n" +
                            "* **Especialidades** \n" +
                            "* **Fazer Agendamento** \n");
        }

        [LuisIntent("Saudacao")]
        public async Task Saudar(IDialogContext context, LuisResult result)
        {
            var now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).TimeOfDay;
            string saudacao;

            if (now < TimeSpan.FromHours(12)) saudacao = "Bom Dia!";
            else if (now < TimeSpan.FromHours(18)) saudacao = "Boa Tarde!";
            else saudacao = "Boa Noite!";

            await context.PostAsync($"{saudacao}! Em que posso ajudar? \n" +
                            "Veja Algumas de minhas funções: \n"  +
                            "* **Medicos Disponiveis** \n" +
                            "* **Especialidades** \n" +
                            "* **Fazer Agendamento** \n");
            context.Done<string>(null);
        }

        internal IDialog<object> Agendar()
        {
            throw new NotImplementedException();
        }

        [LuisIntent("Conhecimento")]
        public async Task Conhecimento(IDialogContext context, LuisResult result)
        {
            if ((result.Entities.Count >= 1) && (result.Entities[0].Type.ToString() == "Criador"))
            {
                await context.PostAsync("Fui criada por Matheus William, ele ainda está me aperfeiçoando.");

            }
            else await context.PostAsync("Sou um bot de auxílio de agendamento. Ainda estou aprendendo muito! Fui desenvolvida para ajudar na hora de agendar uma consulta aqui, no Hospital Imesa.");
            context.Done<string>(null);
        }

        [LuisIntent("Ajuda")]
        public async Task Ajuda(IDialogContext context, LuisResult result)
        {
            var response = "Olha no que posso te ajudar: \n" +
                            "* **Medicos Disponiveis** \n" +
                            "* **Especialidades** \n" +
                            "* **Fazer Agendamento** \n";
            await context.PostAsync(response);
            context.Done<string>(null);
        }

        [LuisIntent("Consulta")]
        public async Task Consulta(IDialogContext context, LuisResult result)
        {
            Conexao dadosBD = new Conexao();

            if ((result.Entities.Count >= 1) && (result.Entities[0].Type.ToString() == "Especialidade"))
            {
                await context.PostAsync("Essas são as especialidades: \n" + dadosBD.ConsultaEspecialidade());
            }
            else
            {
                await context.PostAsync("Esses são os medicos que atendem aqui, no Hospital Imesa: \n" + dadosBD.ConsultaMedico());
            }
            context.Done<string>(null);
        }

        [LuisIntent("Agendar")]
        public async Task Agendar(IDialogContext context, LuisResult result)
        {
            var agendarConsulta = new Agendar_Consulta();
            await context.PostAsync("Iniciando Agendamento...");
            var teste = new FormDialog<Agendar_Consulta>(agendarConsulta, this.BuildForm, FormOptions.PromptInStart,result.Entities);
            context.Call(teste,this.Finalizar);
        }

        private async Task Finalizar(IDialogContext context, IAwaitable<Agendar_Consulta> result)
        {
            await context.PostAsync("Agendado com Sucesoo.");
        }

        public IForm<Agendar_Consulta> BuildForm()
        {
            var agendar = new Agendar_Consulta();
            var medicos = new Conexao();
            var mensagem = "";
            var form = new FormBuilder<Agendar_Consulta>();
            form.Configuration.DefaultPrompt.ChoiceStyle = ChoiceStyleOptions.Buttons;
            form.Configuration.Yes = new string[] { "yes", "sim", "claro", "frmz" };
            form.Configuration.No = new string[] { "nao", "no", "never" };
            form.Message("Olá, vou precisar de alguns dados para agendar sua consulta!");
            form.OnCompletion(async (context, agendar_ok) =>
            {
                await context.PostAsync("Voce foi agendado com sucesso." + agendar.Nome.ToString());
            });
            foreach (var item in medicos.Nome_Medico())
            {
                mensagem += $"\n{item}\n";
            }
            form.Message("\nEscolha o profissional da lista abaixo:\n"+ mensagem);
            return form.Build();
        }
    }

}