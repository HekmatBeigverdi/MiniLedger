using Microsoft.EntityFrameworkCore;
using MiniLedger.Infrastructure.Data;

namespace MiniLedger.Tests.Helpers;

public static class TestDbContextFactory
{
    public static MiniLedgerDbContext Create()
    {
        var options = new DbContextOptionsBuilder<MiniLedgerDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new MiniLedgerDbContext(options);
    }
}