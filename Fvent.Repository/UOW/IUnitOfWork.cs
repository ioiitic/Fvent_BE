using Fvent.Repository.Repositories;

namespace Fvent.Repository.UOW;

public interface IUnitOfWork
{
    IEventRepo Events { get; }
    IReviewRepo Reviews { get; }
    IUserRepo Users { get; }
    IEventRegistrationRepo EventRegistration { get; }
    IEventTagRepo EventTag { get; }
    INotificationRepo Notification { get; }
    IVerificationTokenRepo VerificationToken { get; }
    IEventTypeRepo EventType { get; }
    IEventMediaRepo EventMedia { get; }
    ITagRepo Tag { get; }
    IRefreshTokenRepo RefreshToken { get; }
    IEventFileRepo EventFile { get; }
    IFormRepo Form { get; }
    IFormSubmitRepo FormSubmit { get; }

    bool IsUpdate<TEntity>(TEntity entity) where TEntity : class;
    Task SaveChangesAsync();
    void SaveChanges();
}
