using JsonDeduplicator;
using JsonDeduplicator.Services;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services
            .AddSingleton<JsonService>()
            .AddSingleton<Executor>();
    })
    .Build();

host.Services.GetRequiredService<Executor>().Execute();
