using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mc2.CrudTest.Shared.Helpers
{
    public class PhoneNumberValidationAttribute : RequiredAttribute
    {
        public override bool IsValid(object value)
        {
            try
            {
                if (value == null)
                {
                    return false;
                }
                var phoneNumberUtil = PhoneNumbers.PhoneNumberUtil.GetInstance();
                var nationalPhoneNumber = value.ToString();
                var phoneNumber = phoneNumberUtil.Parse(nationalPhoneNumber, null);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            
        }
    }
}
