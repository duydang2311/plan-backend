namespace WebApp.Domain.Entities;

public interface IEntityId<T>
{
    T Value { get; init; }
}
