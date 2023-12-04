using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mc2.CrudTest.Shared.Models.Args
{
    public class RadzenDataGridDataModel<T>
    {
        public IEnumerable<T> List { get; set; }
        public int TotalRow { get; set; } = 0;
    }
}
