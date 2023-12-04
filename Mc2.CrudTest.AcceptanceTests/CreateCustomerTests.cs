using Mc2.CrudTest.Presentation.Data.Context;
using Mc2.CrudTest.Presentation.Data.Repositories;
using Mc2.CrudTest.Presentation.Server.Controllers;
using Mc2.CrudTest.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xunit;

namespace Mc2.CrudTest.AcceptanceTests
{
    public class BddTddTests
    {

        ILogger<CustomersController> _logger;
        private CustomersRepository repository;
        public static DbContextOptions<PostgreSQLDbContext> dbContextOptions { get; }
        public static string connectionString = "Host=127.0.0.1; Database=CrudTest1Db; Username=postgres; Password=123456";

        static BddTddTests()
        {
            dbContextOptions = new DbContextOptionsBuilder<PostgreSQLDbContext>()
                .UseNpgsql(connectionString)
                .Options;
        }
        public BddTddTests()
        {
            var context = new PostgreSQLDbContext(dbContextOptions);
            DummyDataDBInitializer db = new DummyDataDBInitializer();
            db.Seed(context);
            repository = new CustomersRepository(context);
            using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            _logger = loggerFactory.CreateLogger<CustomersController>();
        }


        [Fact]
        public void CreateCustomerValid_ReturnsSuccess()
        {
            // Todo: Refer to readme.md 
        }

        // Please create more tests based on project requirements as per in readme.md

       


    }
}
