using BlazorPracticeApp;
using BlazorPracticeApp.ApiRequest;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


builder.Services.AddScoped<ApiRequest>();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5086") });

await builder.Build().RunAsync();
    