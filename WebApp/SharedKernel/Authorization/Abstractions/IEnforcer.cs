using System.Linq.Expressions;
using WebApp.SharedKernel.Models;

namespace WebApp.SharedKernel.Authorization.Abstractions;

public interface IEnforcer
{
    Task<bool> EnforceAsync(string subject, string obj, string action, string? domain = null);
    Task<bool> EnforceAsync(Expression<Func<Policy, bool>> predicate);
    Policy Add(string subject, string obj, string action, string? domain = null);
    void Add(Policy policy);
    Task<int> SaveChangesAsync();
}
