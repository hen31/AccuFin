﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AccuFin.Api.Models
{
    public class ValidationError
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public string Field { get; set; }
    }
    

}
