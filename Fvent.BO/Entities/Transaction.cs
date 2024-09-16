namespace Fvent.BO.Entities;

public class Transaction
{
    public Guid TransactionId { get; set; }
    public Guid RegistrationId { get; set; }
    public decimal Amount { get; set; }
    public int PaymentMethodId { get; set; }
    public int PaymentStatus { get; set; }
    public DateTime PaymentTime { get; set; }

    public EventRegistration Registration { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
}
