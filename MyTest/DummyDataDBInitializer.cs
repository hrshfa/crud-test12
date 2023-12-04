using Mc2.CrudTest.Presentation.Data.Context;
using Mc2.CrudTest.Presentation.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTest
{
    public class DummyDataDBInitializer
    {
        public DummyDataDBInitializer()
        {
        }

        public void Seed(PostgreSQLDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.Customers.AddRange(
                new Customer() { Firstname = "hamid", Lastname = "shshh",DateOfBirth=DateTimeOffset.UtcNow,PhoneNumber="+989119660028",Email="hr.shahshahani@gmail.com",BankAccountNumber="1234561" },
                new Customer() { Firstname = "reza", Lastname = "shshh", DateOfBirth=DateTimeOffset.UtcNow,PhoneNumber="+989119660027",Email="hr.shahshahani1@gmail.com",BankAccountNumber="1234562" },
                new Customer() { Firstname = "fatemeh", Lastname = "shshh", DateOfBirth=DateTimeOffset.UtcNow,PhoneNumber="+989119660026",Email="hr.shahshahani2@gmail.com",BankAccountNumber="1234563" },
                new Customer() { Firstname = "zeynab", Lastname = "shshh", DateOfBirth=DateTimeOffset.UtcNow,PhoneNumber="+989119660025",Email="hr.shahshahani3@gmail.com",BankAccountNumber="1234564" },
                new Customer() { Firstname = "ali", Lastname = "shshh", DateOfBirth=DateTimeOffset.UtcNow,PhoneNumber="+989119660024",Email="hr.shahshahani4@gmail.com",BankAccountNumber="1234565" }
            );
            context.SaveChanges();
        }
    }
}
