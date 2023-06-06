using Microsoft.EntityFrameworkCore;

namespace Finance.Management.Api.Data;

public class StatementOfAccountContext:DbContext
{
    public StatementOfAccountContext(DbContextOptions<StatementOfAccountContext> options)
        :base(options)
    {
    }

    public DbSet<StatementOfAccount> StatementOfAccounts { get; set; }
}