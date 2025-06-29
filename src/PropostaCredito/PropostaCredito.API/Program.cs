using PropostaCredito.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureServices();

var app = builder.Build();

app.MigrateDatabaseWithRetry();
app.ConfigurePipeline();
await app.InitializeMessagingAsync();

await app.RunAsync();
