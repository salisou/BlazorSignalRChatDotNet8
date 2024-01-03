# BlazorSignalRChatDotNet8

## Installazione pacchetti 
    Microsoft.AspNetCore.SignalR.Client

## Configurazione di program.cs 
    using BlazorSignalRChatDotNet8.Client.Pages;
    using BlazorSignalRChatDotNet8.Components;
    using BlazorSignalRChatDotNet8.Hubs;
    
    var builder = WebApplication.CreateBuilder(args);
    
    // Add services to the container.
    builder.Services.AddRazorComponents()
        .AddInteractiveWebAssemblyComponents();
    
    
    // Chiamata del servizio SignalR
    builder.Services.AddSignalR();
    
    var app = builder.Build();
    
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseWebAssemblyDebugging();
    }
    else
    {
        app.UseExceptionHandler("/Error", createScopeForErrors: true);
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }
    
    app.UseHttpsRedirection();
    
    app.UseStaticFiles();
    app.UseAntiforgery();
    
    // Mappatura della classe ChtHub con SignalR
    app.MapHub<ChatHub>("/chatHub");
    
    app.MapRazorComponents<App>()
        .AddInteractiveWebAssemblyRenderMode()
        .AddAdditionalAssemblies(typeof(Counter).Assembly);
    
    app.Run();


## Crea la cartella Hubs in BlazorSignalRChatDotNet8/Hubs con la classe ChatHub.cs
    
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

## Aggiungi una pagina in lazorSignalRChatDotNet8.Client/Pages/Chat.razor e congiura cosi
    @page "/chat"
    @inject NavigationManager NavigationManager
    @rendermode InteractiveWebAssembly
    
    
    <h3>Chat</h3>
    
    <!-- Se la connessione al server SignalR è attiva, 
    visualizza i campi di input, 
    il pulsante di invio e la lista dei messaggi -->
    @if (IsConnected)
    {
        <input type="text" @bind="userInput"/>
        <input type="text" @bind="messageInput"/>
        <button @onclick="Send">Invia</button>
    
        <!-- Lista non ordinata per visualizzare i messaggi -->
        <ul id="messages">
            @foreach(var message in messages)
            {
                <li>@message</li>
            }
        </ul>
    }
    else
    {
        <!-- Se la connessione al server SignalR non è attiva, mostra un messaggio di "Connettiti..." -->
        <span>Connettiti...</span>
    }
    
    
    
    @code {
        private HubConnection hubConnection;
        private List<string> messages = new();
        private string userInput;
        private string messageInput;
    
        // Connessione al server SignalR quando la pagina viene inizializzata
        protected override async Task OnInitializedAsync()
        {
            hubConnection = new HubConnectionBuilder()
            .WithUrl(NavigationManager.ToAbsoluteUri("/chathub"))
            .Build();
    
            // Registra un gestore per l'evento "RiceviMessaggi" che aggiunge i messaggi alla lista
            hubConnection.On<string, string>("RiceviMessaggi", (user, message) =>
            {
                var encodedMsg = $"{user}: {message}";
                messages.Add(encodedMsg);
                StateHasChanged();
                // Aggiorna la visualizzazione per riflettere i nuovi messaggi
    
    
            });
    
            // Avvia la connessione al server SignalR
            await hubConnection.StartAsync();
        }
    
        // Metodo chiamato quando viene premuto il pulsante "Invia"
        Task Send() => hubConnection.SendAsync("InviaMessaggio", userInput, messageInput);
    
            // Proprietà che restituisce true se la connessione al server SignalR è attiva
        public bool IsConnected => hubConnection.State == HubConnectionState.Connected;
    }


![immagine](https://github.com/salisou/BlazorSignalRChatDotNet8/assets/61307355/2e1115a5-563c-4824-b758-2fd6d200d624)
