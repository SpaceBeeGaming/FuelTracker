using JsonDeduplicator;
using JsonDeduplicator.Services;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton<Worker>();
        services.AddSingleton<JsonDataStore>();
    })
    .Build();

CancellationTokenSource cts = new();

Console.CancelKeyPress += (_, e) =>
{
    Console.WriteLine("Canceling!");
    cts.Cancel();
    e.Cancel = true;
};

try
{
    await host.Services.GetRequiredService<Worker>().ExecuteAsync(cts.Token);
}
catch (TaskCanceledException ex)
{

    throw;
}
