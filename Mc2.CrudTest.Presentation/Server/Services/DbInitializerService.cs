using Mc2.CrudTest.Presentation.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Mc2.CrudTest.Presentation.Server.Services
{
    public interface IDbInitializerService
    {
        /// <summary>
        /// Applies any pending migrations for the context to the database.
        /// Will create the database if it does not already exist.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Adds some default values to the Db
        /// </summary>
        void SeedData();
    }

    public class DbInitializerService : IDbInitializerService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<DbInitializerService> _logger;

        public DbInitializerService(
            IServiceScopeFactory scopeFactory
            , ILogger<DbInitializerService> logger
            )
        {
            _scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Initialize()
        {
            using var serviceScope = _scopeFactory.CreateScope();
            using var context = serviceScope.ServiceProvider.GetService<PostgreSQLDbContext>() ?? throw new ArgumentNullException(nameof(PostgreSQLDbContext));
            var con = context.Database.GetConnectionString();
            _logger.LogInformation($"Connection strings is:{con}");
            context.Database.Migrate();
        }

        public void SeedData()
        {
            using (var serviceScope = _scopeFactory.CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<PostgreSQLDbContext>() ?? throw new ArgumentNullException(nameof(PostgreSQLDbContext)))
                {
                    // initialize data
                }
            }
        }
    }
}
