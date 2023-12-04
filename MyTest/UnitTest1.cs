using FluentAssertions;
using Mc2.CrudTest.Presentation.Data.Context;
using Mc2.CrudTest.Presentation.Data.Repositories;
using Mc2.CrudTest.Presentation.Server.Controllers;
using Mc2.CrudTest.Shared.Models;
using Mc2.CrudTest.Shared.Models.Args;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace MyTest
{
    public class UnitTest1
    {
        ILogger<CustomersController> _logger;
        private CustomersRepository repository;
        public static DbContextOptions<PostgreSQLDbContext> dbContextOptions { get; }
        public static string connectionString = "Host=127.0.0.1; Database=CrudTest1Db; Username=postgres; Password=123456";
        private RadzenLoadDataArgsModel args=new RadzenLoadDataArgsModel { Skip=0,Top=50,Filter="",OrderBy=""};
        static UnitTest1()
        {
            dbContextOptions = new DbContextOptionsBuilder<PostgreSQLDbContext>()
                .UseNpgsql(connectionString)
                .Options;
        }
        public UnitTest1()
        {
            var context = new PostgreSQLDbContext(dbContextOptions);
            DummyDataDBInitializer db = new DummyDataDBInitializer();
            db.Seed(context);
            repository = new CustomersRepository(context);
            using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            _logger = loggerFactory.CreateLogger<CustomersController>();
        }


        

        #region Get By Id  

        [Fact]
        public async void Task_GetCustomerById_Return_OkResult()
        {
            //Arrange  
            var controller = new CustomersController(_logger, repository);
            var CustomerId = 2;

            //Act  
            var data = await controller.Get(CustomerId);

            //Assert  
            Assert.IsType<OkObjectResult>(data);
        }

        [Fact]
        public async void Task_GetCustomerById_Return_NotFoundResult()
        {
            //Arrange  
            var controller = new CustomersController(_logger, repository);
            var CustomerId = 3;

            //Act  
            var data = await controller.Get(CustomerId);

            //Assert  
            Assert.IsType<NotFoundResult>(data);
        }

        [Fact]
        public async void Task_GetCustomerById_Return_BadRequestResult()
        {
            //Arrange  
            var controller = new CustomersController(_logger, repository);
            int? CustomerId = null;

            //Act  
            var data = await controller.Get(CustomerId);

            //Assert  
            Assert.IsType<BadRequestResult>(data);
        }

        [Fact]
        public async void Task_GetCustomerById_MatchResult()
        {
            //Arrange  
            var controller = new CustomersController(_logger, repository);
            int CustomerId = 1;

            //Act  
            var data = await controller.Get(CustomerId);

            //Assert  
            Assert.IsType<OkObjectResult>(data);

            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;
            var Customer = okResult.Value.Should().BeAssignableTo<ResponseDTO<CustomerDto>>().Subject;

            Assert.Equal("+989119660028", Customer.Data.PhoneNumber);
            Assert.Equal("hr.shahshahani@gmail.com", Customer.Data.Email);
        }

        #endregion

        #region Get All  

        [Fact]
        public async void Task_GetPosts_Return_OkResult()
        {
            //Arrange  
            var controller = new CustomersController(_logger, repository);

            //Act  
            var data = await controller.Get(args);

            //Assert  
            Assert.IsType<OkObjectResult>(data);
        }

        [Fact]
        public void Task_GetPosts_Return_BadRequestResult()
        {
            //Arrange  
            var controller = new CustomersController(_logger, repository);

            //Act  
            var data = controller.Get(args);
            data = null;

            if (data != null)
                //Assert  
                Assert.IsType<BadRequestResult>(data);
        }

        [Fact]
        public async void Task_GetPosts_MatchResult()
        {
            //Arrange  
            var controller = new CustomersController(_logger, repository);

            //Act  
            var data = await controller.Get(args);

            //Assert  
            Assert.IsType<OkObjectResult>(data);

            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;
            var customer = okResult.Value.Should().BeAssignableTo<RadzenDataGridDataModel<CustomerDto>>().Subject;

            Assert.Equal("+989119660028", customer.List.ElementAt(0).PhoneNumber);
            Assert.Equal("hr.shahshahani@gmail.com", customer.List.ElementAt(0).Email);

            Assert.Equal("+989119660027", customer.List.ElementAt(0).PhoneNumber);
            Assert.Equal("hr.shahshahani1@gmail.com", customer.List.ElementAt(0).Email);
        }

        #endregion

        #region Add New Blog  

        [Fact]
        public async void Task_Add_ValidData_Return_OkResult()
        {
            //Arrange  
            var controller = new CustomersController(_logger, repository);
            var customer = new CustomerDto() { Firstname = "mohsen", Lastname = "shshh", DateOfBirth = DateTimeOffset.UtcNow, PhoneNumber = "+989119660023", Email = "hr.shahshahani5@gmail.com", BankAccountNumber = "1234561" };

            //Act  
            var data = await controller.Post(customer);

            //Assert  
            Assert.IsType<OkObjectResult>(data);
        }

        [Fact]
        public async void Task_Add_InvalidData_Return_BadRequest()
        {
            //Arrange  
            var controller = new CustomersController(_logger, repository);
            var customer = new CustomerDto() { Firstname = "mohsen", Lastname = "shshh", DateOfBirth = DateTimeOffset.UtcNow, PhoneNumber = "+ir9119660023", Email = "hr.shahshahani5@gmail.com", BankAccountNumber = "1234561" };

            //Act              
            var data = await controller.Post(customer);// invalid phone number

            //Assert  
            Assert.IsType<BadRequestResult>(data);
        }

        [Fact]
        public async void Task_Add_ValidData_MatchResult()
        {
            //Arrange  
            var controller = new CustomersController(_logger, repository);
            var customer = new CustomerDto() { Firstname = "mohsen", Lastname = "shshh", DateOfBirth = DateTimeOffset.UtcNow, PhoneNumber = "+989119660022", Email = "hr.shahshahani5@gmail.com", BankAccountNumber = "1234561" };

            //Act  
            var data = await controller.Post(customer);

            //Assert  
            Assert.IsType<OkObjectResult>(data);

            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;
            var result = okResult.Value.Should().BeAssignableTo<CustomerDto>().Subject;

            Assert.Equal(8, result.Id);
        }

        #endregion

        #region Update Existing Blog  

        [Fact]
        public async void Task_Update_ValidData_Return_OkResult()
        {
            //Arrange  
            var controller = new CustomersController(_logger, repository);
            var customerId = 2;

            //Act  
            var existingPost = await controller.Get(customerId);
            var okResult = existingPost.Should().BeOfType<OkObjectResult>().Subject;
            var result = okResult.Value.Should().BeAssignableTo<CustomerDto>().Subject;

            var customer = new CustomerDto();
            customer.Firstname = result.PhoneNumber;
            customer.Lastname = result.Lastname;
            customer.PhoneNumber = "+989119660021";
            customer.Email = result.Email;
            customer.BankAccountNumber = result.BankAccountNumber;

            var updatedData = await controller.Put(customer);

            //Assert  
            Assert.IsType<OkResult>(updatedData);
        }

        [Fact]
        public async void Task_Update_InvalidData_Return_BadRequest()
        {
            //Arrange  
            var controller = new CustomersController(_logger, repository);
            var customerId = 2;

            //Act  
            var existingPost = await controller.Get(customerId);
            var okResult = existingPost.Should().BeOfType<OkObjectResult>().Subject;
            var result = okResult.Value.Should().BeAssignableTo<CustomerDto>().Subject;

            var customer = new CustomerDto();
            customer.Firstname = result.PhoneNumber;
            customer.Lastname = result.Lastname;
            customer.PhoneNumber = "+ir9119660021";
            customer.Email = result.Email;
            customer.BankAccountNumber = result.BankAccountNumber;

            var updatedData = await controller.Put(customer);

            //Assert  
            Assert.IsType<BadRequestResult>(updatedData);
        }

        #endregion

        #region Delete Post  

        [Fact]
        public async void Task_Delete_Post_Return_OkResult()
        {
            //Arrange  
            var controller = new CustomersController(_logger, repository);
            var CustomerId = 2;

            //Act  
            var data = await controller.Delete(CustomerId);

            //Assert  
            Assert.IsType<OkResult>(data);
        }

        [Fact]
        public async void Task_Delete_Post_Return_NotFoundResult()
        {
            //Arrange  
            var controller = new CustomersController(_logger, repository);
            var CustomerId = 5;

            //Act  
            var data = await controller.Delete(CustomerId);

            //Assert  
            Assert.IsType<NotFoundResult>(data);
        }

        [Fact]
        public async void Task_Delete_Return_BadRequestResult()
        {
            //Arrange  
            var controller = new CustomersController(_logger, repository);
            int? CustomerId = null;

            //Act  
            var data = await controller.Delete(CustomerId);

            //Assert  
            Assert.IsType<BadRequestResult>(data);
        }

        #endregion
    }
}
