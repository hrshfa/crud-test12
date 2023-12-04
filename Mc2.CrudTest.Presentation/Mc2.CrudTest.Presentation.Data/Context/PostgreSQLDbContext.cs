using Mc2.CrudTest.Presentation.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mc2.CrudTest.Presentation.Data.Context
{
    public partial class PostgreSQLDbContext : DbContext
    {

        public PostgreSQLDbContext(DbContextOptions options) : base(options)
        {

        }
        public virtual DbSet<Customer> Customers { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder builder)
        {

            // it should be placed here, otherwise it will rewrite the following settings!
            base.OnModelCreating(builder);

            builder.Entity<Customer>(entity =>
                  entity.HasIndex(e => e.Email).IsUnique()
            );
        }
    }
}
