using MGM.GoodHamburger.App;
using MGM.GoodHamburger.App.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Obter a URL da API do appsettings.json
var apiBaseUrl = builder.Configuration["ApiSettings:BaseUrl"]
    ?? throw new InvalidOperationException("ApiSettings:BaseUrl não está configurada no appsettings.json");

// Configurar HttpClient com a URL base da API
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(apiBaseUrl)
});

builder.Services.AddScoped<MenuService>();
builder.Services.AddScoped<OrderService>();

// Registrar o CartService como singleton para manter o estado do carrinho
builder.Services.AddSingleton<CartService>();

await builder.Build().RunAsync();
