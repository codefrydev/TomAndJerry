using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TomAndJerry;
using TomAndJerry.Services;
using TomAndJerry.Utils;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Configure HttpClient
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Register services with proper dependency injection
builder.Services.AddScoped<IVideoService, VideoService>();
builder.Services.AddScoped<ISearchService, SearchService>();
builder.Services.AddScoped<IStateService, StateService>();
builder.Services.AddScoped<IApplicationService, ApplicationService>();
builder.Services.AddScoped<IStickerService, StickerService>();
builder.Services.AddScoped<IRandomFactsService, RandomFactsService>();
builder.Services.AddScoped<IQuizService, QuizService>();

// Legacy Data class for backward compatibility (will be removed in future iterations)
builder.Services.AddSingleton<TomAndJerry.DataBase.Data>();

await builder.Build().RunAsync();