using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WebApp.SharedKernel.Authorization.Abstractions;
using WebApp.SharedKernel.Models;
using WebApp.SharedKernel.Persistence;

namespace WebApp.SharedKernel.Authorization;

public sealed class Enforcer(AppDbContext dbContext) : IEnforcer
{
    public Task<bool> EnforceAsync(string subject, string obj, string action, string? domain = null)
    {
        return EnforceAsync(x =>
            x.Subject.Equals(subject)
            && x.Object.Equals(obj)
            && x.Action.Equals(action)
            && (x.Domain == null || x.Domain.Equals(domain))
        );
    }

    public Task<bool> EnforceAsync(Expression<Func<Policy, bool>> predicate)
    {
        return dbContext.UserPolicies.AnyAsync(predicate);
    }

    public Policy Add(string subject, string obj, string action, string? domain = null)
    {
        var policy = new Policy
        {
            Subject = subject,
            Object = obj,
            Action = action,
            Domain = domain
        };
        Add(policy);
        return policy;
    }

    public void Add(Policy policy)
    {
        dbContext.Add(policy);
    }

    public Task<int> SaveChangesAsync()
    {
        return dbContext.SaveChangesAsync();
    }
}
