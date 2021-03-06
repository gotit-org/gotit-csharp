﻿using GotIt.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GotIt.BLL.ViewModels
{
    public class RegisterationViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        public string Password { get; set; }
        public string RepeatPassword { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Picture { get; set; }
        public EUserType Type { get; set; }
        public EGender Gender { get; set; }
    }
}
