﻿using System.ComponentModel.DataAnnotations;

namespace TodoApp.ViewModels
{
    public class LoginViewModel

    {

   

        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
