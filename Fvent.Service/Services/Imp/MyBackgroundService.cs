using Microsoft.Extensions.Hosting;

namespace Fvent.Service.Services.Imp;

public class MyBackgroundService(IEventService eventService) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(3000);
        }
    }
}