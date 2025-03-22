using Currencies.Abstractions.Contracts.Exceptions;
using Currencies.Db;
using Microsoft.EntityFrameworkCore;

namespace Currencies.WebApi;

public class DatabaseManager
{
    private readonly WebApplicationBuilder _builder;

    public DatabaseManager(WebApplicationBuilder builder)
    {
        _builder = builder;
        ConnectToMsSQL();
    }

    private void ConnectToMsSQL()
    {
        string connectionString = _builder.Configuration.GetConnectionString("Default");

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("Connection string 'Default' nie została skonfigurowana.");
        }

        _builder.Services.AddDbContext<TableContext>((DbContextOptionsBuilder options) =>
        {
            options.UseSqlServer(
                connectionString,
                sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly("Currencies.Migrations");
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5, // Maksymalna liczba prób
                        maxRetryDelay: TimeSpan.FromSeconds(10), // Maksymalne opóźnienie między próbami
                        errorNumbersToAdd: null // Możesz dodać dodatkowe numery błędów
                    );
                });
        });
    }

    public void ApplyMigrations(IHost app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        var dbContext = services.GetRequiredService<TableContext>();

        try
        {
            // Sprawdzanie połączenia z bazą danych
            Console.WriteLine("Nawiązywanie połączenia z bazą danych...");
            dbContext.Database.OpenConnection();
            Console.WriteLine("Połączenie udane.");

            if (!dbContext.Database.CanConnect())
            {
                Console.WriteLine("Nie można nawiązać połączenia z bazą danych.");
                return;
            }

            // Sprawdzanie i stosowanie migracji
            var pendingMigrations = dbContext.Database.GetPendingMigrations();
            if (pendingMigrations.Any())
            {
                Console.WriteLine($"Znaleziono {pendingMigrations.Count()} oczekujących migracji. Stosowanie migracji...");
                dbContext.Database.Migrate();
                Console.WriteLine("Migracje zostały zastosowane pomyślnie.");
            }
            else
            {
                Console.WriteLine("Brak oczekujących migracji.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Wystąpił błąd podczas stosowania migracji: {ex.Message}");
            throw new BadRequestException($"Błąd aplikacji migracji: {ex.Message}");
        }
    }
}
