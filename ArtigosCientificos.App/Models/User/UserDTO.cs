﻿using System.ComponentModel.DataAnnotations;

namespace ArtigosCientificos.App.Models.User
{
    public class UserDTO
    {
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(50, ErrorMessage = "Username cannot exceed 50 characters.")]
        public string Username { get; set; } = string.Empty;

        
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string? Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        //[StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 100 characters.")]
        //[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
          //  ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }

}
