using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mc2.CrudTest.Shared.Models.Args
{
    public class RadzenLoadDataArgsModel
    {
        public int? Skip { get; set; }
        public int? Top { get; set; }
        public string OrderBy { get; set; }
        public string Filter { get; set; }
    }
}
