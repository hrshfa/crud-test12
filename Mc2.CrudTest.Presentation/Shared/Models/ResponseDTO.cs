using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mc2.CrudTest.Shared.Models
{
    public class ResponseDTO<T> 
    {
        public bool IsSuccessful { get; set; }=false;
        public T Data { get; set; } = default!;
        public List<string> Errors { get; set; } = new List<string>();
    }
   
}
