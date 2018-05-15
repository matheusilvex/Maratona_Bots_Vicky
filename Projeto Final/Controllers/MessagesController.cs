using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using Microsoft.Bot.Builder.Luis;
using System.Configuration;
using System.Linq;
using Projeto_Final.Dialogs;
using Microsoft.Bot.Builder.FormFlow;

namespace Projeto_Final
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        /// Inicio da Conversa com Usuário
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            var connector = new ConnectorClient(new Uri(activity.ServiceUrl));

            //Atributos de Configuração do LUIS
            var attributes = new LuisModelAttribute(
                ConfigurationManager.AppSettings["LuisId"],
                ConfigurationManager.AppSettings["LuisSubscriptionKey"]);
            var service = new LuisService(attributes);

            switch (activity.Type)
            {
                case ActivityTypes.Message:
                    await Conversation.SendAsync(activity, () => new Conversa(service));

                    break;
                    
                case ActivityTypes.ConversationUpdate:
                    if(activity.MembersAdded.Any(o => o.Id == activity.Recipient.Id)){
                        var reply = activity.CreateReply();
                        reply.Text = "Olá, eu sou a VICKY.\n" +
                            "Fui desenvolvida para auxiliar voce a agendar uma consulta!\n" +
                            "Minhas funções:\n" +
                            "* **Medicos Disponiveis** \n" +
                            "* **Especialidades** \n" +
                            "* **Fazer Agendamento** \n";
                        await connector.Conversations.ReplyToActivityAsync(reply);
                        
                    }
                    break;
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }
        
    }
}