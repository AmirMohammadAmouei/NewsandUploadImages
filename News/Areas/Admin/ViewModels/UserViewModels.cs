﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace News.Areas.Admin.ViewModels
{
    public class UserViewModels
    {
        public long Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? UserName { get; set; }
        public string? PersonalId { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
