using Xunit;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Currencies.Db;

namespace Currencies.Tests;

public class BaseTestFixture : IDisposable
{
    private readonly SqliteConnection _connection;
    public TableContext _dbContext;

    public BaseTestFixture()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();
        _dbContext = CreateContext();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            if(_connection != null)
            {
                _connection.Close();
            }
        }
    }

    public TableContext CreateContext()
    {
        var table = new TableContext(new DbContextOptionsBuilder<TableContext>()
            .UseSqlite(_connection)
            .Options);
        table.Database.EnsureCreated();
        return table;
    }
}