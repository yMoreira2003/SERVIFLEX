﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Entidades
{
    public class ResBase
    {
        public bool resultado { get; set; }
        public List<Error> error { get; set; }
    }
}
