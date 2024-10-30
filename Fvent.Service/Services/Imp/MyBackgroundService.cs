using Fvent.BO.Entities;
using Fvent.Repository.UOW;
using Microsoft.Extensions.Hosting;

namespace Fvent.Service.Services.Imp;

public class MyBackgroundService(UnitOfWork uOW) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var events = await uOW.Events.GetAllAsync();
            foreach (Event _event in events)
            {
                if (_event.EndTime <= DateTime.Now)
                    _event.Update(3);
            }
            await Task.Delay(30000);
        }
    }
}