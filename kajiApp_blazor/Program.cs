using Blazored.Toast;
using kajiApp_blazor.Components;
using kajiApp_blazor.Components.ViewModel;
using kajiApp_blazor.Components.ViewModel.HomeDBC;
using kajiApp_blazor.Components.Entity;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<ProtectedSessionStorage>();
builder.Services.AddHttpClient();
builder.Services.AddSingleton<AppState>();
builder.Services.AddBlazoredToast();
builder.Services.AddMudServices();

builder.Services.AddDbContext<kajiappDBContext>();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();
// データベースの初期化(今月分の明細追加)
InitDBC.InitializeDatabase();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
