using Mc2.CrudTest.Presentation.Data.Context;
using Mc2.CrudTest.Presentation.Data.Entities;
using Mc2.CrudTest.Shared.Helpers;
using Mc2.CrudTest.Shared.Models;
using Mc2.CrudTest.Shared.Models.Args;
using Mc2.CrudTest.Shared.Models.Logs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Mc2.CrudTest.Presentation.Data.Repositories
{
    public interface ICustomersRepository
    {
        Task<RadzenDataGridDataModel<CustomerDto>> Get(RadzenLoadDataArgsModel args);
        Task<CustomerDto> GetByID(int ID);
        Task<LogDto> Insert(CustomerDto model);
        Task<LogDto> Update(CustomerDto model);
        Task<LogDto> Delete(int ID);
    }
    public class CustomersRepository : ICustomersRepository
    {
        private readonly PostgreSQLDbContext _context;
        public CustomersRepository(PostgreSQLDbContext context)
        {
            _context = context ?? throw new ArgumentException(nameof(context));
        }
        public async Task<LogDto> Delete(int ID)
        {
            var customer = await _context.Customers.FindAsync(ID);
            _context.Remove(customer);
            await _context.SaveChangesAsync();
            var model = Mapper.SimpleCopy<CustomerDto>(customer);
            LogDto log = new LogDto
            {
                LogDateTime = DateTime.UtcNow
                ,
                Type = LogType.Delete
                ,
                TableName = "Customers"
                ,
                RowId = ID
                ,
                OldData = JsonSerializer.Serialize(model)
                ,
                NewData = string.Empty
            };
            return log;
        }

        public async Task<RadzenDataGridDataModel<CustomerDto>> Get(RadzenLoadDataArgsModel args)
        {
            RadzenDataGridDataModel<CustomerDto> model = new RadzenDataGridDataModel<CustomerDto>();
            var query = _context.Customers.AsNoTracking();
            if (args != null)
            {
                if (!string.IsNullOrEmpty(args.Filter))
                {
                    query = query.Where(args.Filter);
                }
                if (!string.IsNullOrEmpty(args.OrderBy))
                {
                    query = query.OrderBy(args.OrderBy);
                }
                model.TotalRow = await query.CountAsync();
                query = query.Skip(args.Skip ?? 0).Take(args.Top ?? 50);
            }
            var result = await query.ToListAsync();
            model.List = result.Select(a => Mapper.SimpleCopy<CustomerDto>(a)) ?? new List<CustomerDto>();

            return model;
        }

        public async Task<CustomerDto> GetByID(int ID)
        {
            var customer = await _context.Customers.AsNoTracking().FirstOrDefaultAsync(a => a.Id == ID);
            var model = Mapper.SimpleCopy<CustomerDto>(customer);
            return model;
        }

        public async Task<LogDto> Insert(CustomerDto model)
        {
            var customer = Mapper.SimpleCopy<Customer>(model);
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            model.Id = customer.Id;
            LogDto log = new LogDto
            {
                LogDateTime = DateTime.UtcNow
                ,
                Type = LogType.Insert
                ,
                TableName = "Customers"
                ,
                RowId = model.Id
                ,
                NewData = JsonSerializer.Serialize(model)
                ,
                OldData = string.Empty
            };
            return log;
        }

        public async Task<LogDto> Update(CustomerDto model)
        {
            var customer = await _context.Customers.FindAsync(model.Id);
            var oldCustomer = Mapper.SimpleCopy<CustomerDto>(customer);
            customer = Mapper.SimpleCopy(model, customer);
            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();
            LogDto log = new LogDto
            {
                LogDateTime = DateTime.UtcNow
               ,
                Type = LogType.Update
               ,
                TableName = "Customers"
               ,
                RowId = model.Id
               ,
                NewData = JsonSerializer.Serialize(model)
               ,
                OldData = JsonSerializer.Serialize(oldCustomer)
            };
            return log;
        }
    }
}
