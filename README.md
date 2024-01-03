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


![immagine](https://github.com/salisou/BlazorSignalRChatDotNet8/assets/61307355/2e1115a5-563c-4824-b758-2fd6d200d624)
