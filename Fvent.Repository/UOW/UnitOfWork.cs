using Microsoft.EntityFrameworkCore;
using Fvent.Repository.Data;
using Fvent.Repository.Repositories;
using Fvent.Repository.Repositories.Imp;

namespace Fvent.Repository.UOW;

public class UnitOfWork(MyDbContext context) : IUnitOfWork
{
    private readonly MyDbContext _context = context;
    private readonly IEventRepo _eventRepo = new EventRepo(context);
    private readonly IEventFollowerRepo _eventFollowerRepo = new EventFollowerRepo(context);
    private readonly IReviewRepo _reviewRepo = new ReviewRepo(context);
    private readonly IUserRepo _userRepo = new UserRepo(context);

    public IEventRepo Events => _eventRepo;
    public IEventFollowerRepo EventFollower => _eventFollowerRepo;

    public IReviewRepo Reviews => _reviewRepo;

    public IUserRepo Users => _userRepo;

    public bool IsUpdate<TEntity>(TEntity entity) where TEntity : class
        => _context.Entry(entity).State == EntityState.Modified;

    public void SaveChanges()
        => _context.SaveChangesAsync();

    public async Task SaveChangesAsync()
        => await _context.SaveChangesAsync();
}
