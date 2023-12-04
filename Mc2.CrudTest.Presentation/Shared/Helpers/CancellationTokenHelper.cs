using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mc2.CrudTest.Shared.Helpers
{
    public class CancellationTokenHelper
    {
        public static CancellationToken? GetCancellationToken(CancellationTokenSource cts)
        {
            try
            {
                if (cts is null || cts.IsCancellationRequested)
                {
                    return default(CancellationToken?);
                }
                return cts.Token;
            }
            catch (Exception)
            {
                return default(CancellationToken?);
            }

        }
    }
}
