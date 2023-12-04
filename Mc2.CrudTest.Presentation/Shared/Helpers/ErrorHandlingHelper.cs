using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace Mc2.CrudTest.Shared.Helpers
{
    public class ErrorHandlingHelper
    {
        private static IDictionary<string, string> uniqueFieldList = new Dictionary<string, string>(
                new List<KeyValuePair<string, string>>
                {
                    new ("'IX_Customers_Email'",",Email Address")
                   , new ("'IX_Customers_Firstname_Lastname_DateOfBirth'",",Firstname,Lastname,DateOfBirth")
                     }
            );
        private static IDictionary<string, string> foreignkeyFieldList = new Dictionary<string, string>(
                new List<KeyValuePair<string, string>>
                {
                   // new ("'IX_'","")
                }
            );
        public static string ParseError(ILogger logger, string method, Exception e)
        {
            while (e.InnerException != null)
            {
                e = e.InnerException;
            }
            var sqlex = e as SqlException;
            if (sqlex?.Number == 2601 || sqlex?.Number == 2627)
            {
                foreach (var item in uniqueFieldList.Keys)
                {
                    if (e.Message.Contains(item))
                    {
                        //The value of the name fields should not be duplicate
                        var res = string.Format("The value of the {0} field(s) should not be duplicate", uniqueFieldList[item]);
                        logger.LogError($"{method}:{res}");
                        return res;
                    }
                }
            }
            logger.LogError($"{method}:{e.Message}");
            return e.Message;
        }
    }
}
