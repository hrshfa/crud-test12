using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mc2.CrudTest.Presentation.Data.Entities
{
    [Index(nameof(Firstname), nameof(Lastname), nameof(DateOfBirth), IsUnique = true)]
    public class Customer
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Firstname { get; set; }=string.Empty;
        public string Lastname { get; set; }=string.Empty;
        public DateTimeOffset DateOfBirth { get; set; }
        [StringLength(15)]
        public string PhoneNumber { get; set; }=string.Empty;
        [StringLength(320)]
        public string Email { get; set; }=string.Empty;
        public string BankAccountNumber { get; set; }=string.Empty;
    }
}
