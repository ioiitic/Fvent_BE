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
    private readonly IEventRegistrationRepo _eventRegistrationRepo = new EventRegistrationRepo(context);
    private readonly IEventTagRepo _eventTagRepo = new EventTagRepo(context);
    private readonly ICommentRepo _commentRepo = new CommentRepo(context);
    private readonly INotificationRepo _notificationRepo = new NotificationRepo(context);   
    private readonly IVerificationTokenRepo _verificationTokenRepo = new VerificationTokenRepo(context);
    private readonly IEventTypeRepo _eventTypeRepo = new EventTypeRepo(context);
    private readonly IEventMediaRepo _eventMediaRepo = new EventMediaRepo(context);
    private readonly ITagRepo _tagRepo = new TagRepo(context);
    private readonly IRefreshTokenRepo _refreshTokenRepo = new RefreshTokenRepo(context);

    public IEventRepo Events => _eventRepo;
    public IEventFollowerRepo EventFollower => _eventFollowerRepo;

    public IReviewRepo Reviews => _reviewRepo;

    public IUserRepo Users => _userRepo;

    public IEventRegistrationRepo EventRegistration => _eventRegistrationRepo;
    public IEventTagRepo EventTag => _eventTagRepo;
    public ICommentRepo Comment => _commentRepo;
    public INotificationRepo Notification => _notificationRepo;
    public IVerificationTokenRepo VerificationToken => _verificationTokenRepo;
    public IEventTypeRepo EventType => _eventTypeRepo;
    public IEventMediaRepo EventMedia => _eventMediaRepo;
    public ITagRepo Tag => _tagRepo;
    public IRefreshTokenRepo RefreshToken => _refreshTokenRepo;

    public bool IsUpdate<TEntity>(TEntity entity) where TEntity : class
        => _context.Entry(entity).State == EntityState.Modified;

    public void SaveChanges()
        => _context.SaveChangesAsync();

    public async Task SaveChangesAsync()
        => await _context.SaveChangesAsync();
}
