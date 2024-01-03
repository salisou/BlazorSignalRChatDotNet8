using Microsoft.AspNetCore.SignalR;

namespace BlazorSignalRChatDotNet8.Hubs
{
    public class ChatHub : Hub
    {
        /// <summary>
        /// Questo metodo è chiamato dal server per inviare un messaggio a tutti i client connessi.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task InviaMessaggio(string user, string message)
        {
            // Invia il messaggio a tutti i client connessi
            await Clients.All.SendAsync("RiceviMessaggi", user, message);
        }
    }
}
