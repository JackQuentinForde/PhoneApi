namespace Domain.Entities
{
    public record PhoneNumber(
        int Id,
        string Number,
        int AccountId
    );
}
