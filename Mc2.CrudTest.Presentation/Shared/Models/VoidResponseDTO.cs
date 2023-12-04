﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mc2.CrudTest.Shared.Models
{
    public class VoidResponseDTO
    {
        public bool IsSuccessful { get; set; } = false;
        public List<string> Errors { get; set; } = new List<string>();
    }
}
