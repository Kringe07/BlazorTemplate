using Microsoft.EntityFrameworkCore;
using ProjectName.DataAccess.DatabaseContext;
using ProjectName.DataAccess.Entities;

namespace ProjectName.DataAccess.Repository;

public class AuditRepository(IDbContextFactory<ProjectNameContext> contextFactory)
{
    public async Task<List<Audit>> GetAudits()
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        return await context.Audits.ToListAsync();
    }

    public async Task<Audit> GetAudit(int id)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        var audit = await context.Audits.FindAsync(id);
        return audit ?? throw new Exception("Audit not found");
    }
}