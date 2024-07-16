namespace WebApp.Domain.Entities;

public interface IEntityId
{
    Guid Value { get; init; }
}
