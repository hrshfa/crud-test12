using Mc2.CrudTest.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mc2.CrudTest.Shared.Models
{
    public class CustomerDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Firstname is required.")]
        public string Firstname { get; set; } = string.Empty;
        [Required(ErrorMessage = "Lastname is required.")]
        public string Lastname { get; set; } = string.Empty;
        [Required(ErrorMessage = "DateOfBirth is required.")]
        public DateTimeOffset DateOfBirth { get; set; } = new DateTimeOffset(DateTime.UtcNow);
        [Required(ErrorMessage = "PhoneNumber is required.")]
        [StringLength(15,ErrorMessage = "Maximum length of PhoneNumber is 15 character.")]
        [PhoneNumberValidation(ErrorMessage = "PhoneNumber is invalid.")]
        public string PhoneNumber { get; set; } = string.Empty;
        [Required(ErrorMessage = "Email is required.")]
        [StringLength(320, ErrorMessage = "Maximum length of Email Adress is 320 character.")]
        [RegularExpression("^[a-zA-Z0-9_.-]+@[a-zA-Z0-9-]+.[a-zA-Z0-9-.]+$",ErrorMessage = "Invalid Email Adress")]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = "BankAccountNumber is required.")]
        public string BankAccountNumber { get; set; } = string.Empty;
    }
}
