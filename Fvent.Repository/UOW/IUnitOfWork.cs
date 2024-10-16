﻿using Fvent.Repository.Repositories;

namespace Fvent.Repository.UOW;

public interface IUnitOfWork
{
    IEventRepo Events { get; }
    IEventFollowerRepo EventFollower { get; }
    IReviewRepo Reviews { get; }
    IUserRepo Users { get; }
    IEventRegistrationRepo EventRegistration { get; }
    IEventTagRepo EventTag { get; }
    ICommentRepo Comment { get; }
    INotificationRepo Notification { get; }
    IVerificationTokenRepo VerificationToken { get; }

    bool IsUpdate<TEntity>(TEntity entity) where TEntity : class;
    Task SaveChangesAsync();
    void SaveChanges();
}
