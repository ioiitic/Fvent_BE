using Fvent.BO.Entities;
using Fvent.BO.Enums;
using Fvent.Repository.Data;
using Fvent.Repository.UOW;
using Microsoft.Extensions.Hosting;

namespace Fvent.Service.Services.Imp;

public class MyBackgroundService : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            MyDbContext context = new MyDbContext();
            IUnitOfWork uOW = new UnitOfWork(context);
            var events = await uOW.Events.GetAllAsync();
            foreach (Event _event in events)
            {
                if(_event.Status == EventStatus.Upcoming || _event.Status == EventStatus.InProgress)
                {
                    if (_event.EndTime >= DateTime.Now && _event.StartTime <= DateTime.Now)
                        _event.Update(EventStatus.InProgress);

                    else if (_event.EndTime < DateTime.Now)
                    {
                        _event.Update(EventStatus.Completed);
                    }
                }
            }
            await uOW.SaveChangesAsync();

            var verificationToken = await uOW.VerificationToken.GetAllAsync();
            foreach(var verification in verificationToken)
            {
                if(verification.ExpiryDate < DateTime.Now) 
                    uOW.VerificationToken.Delete(verification);
            }
            await uOW.SaveChangesAsync();

            var refreshToken = await uOW.RefreshToken.GetAllAsync();
            foreach (var refresh in refreshToken)
            {
                if (refresh.Expires < DateTime.Now)
                    uOW.RefreshToken.Delete(refresh);
            }
            await uOW.SaveChangesAsync();

            await Task.Delay(30000);
        }
    }
}