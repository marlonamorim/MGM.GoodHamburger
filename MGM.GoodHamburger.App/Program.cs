using MGM.GoodHamburger.App;
using MGM.GoodHamburger.App.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Configurar HttpClient com a URL base da API
builder.Services.AddScoped(sp => new HttpClient 
{ 
    BaseAddress = new Uri("https://localhost:7098") // URL da API
});

// Registrar o MenuService
builder.Services.AddScoped<MenuService>();

// Registrar o OrderService
builder.Services.AddScoped<OrderService>();

// Registrar o CartService como singleton para manter o estado do carrinho
builder.Services.AddSingleton<CartService>();

await builder.Build().RunAsync();
