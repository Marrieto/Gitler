using Gitler;

IHost host = Host.CreateDefaultBuilder(args)
    .UseWindowsService()
    .ConfigureServices(services => { services.AddHostedService<GitCommitAndPushService>(); })
    .Build();

host.Run();