﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AccuFin.Api.Models.Authentication
{
    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ClientId { get; set; }
    }
}
