using FirstTask.Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstTask.Entities.ViewModels
{
    public class RegisterVM
    {
        [Required]
        [MaxLength(50, ErrorMessage = "Maximum 50 characters are allowed")]
        public string FirstName { get; set; } = string.Empty;
        [MaxLength(50, ErrorMessage = "Maximum 50 characters are allowed")]
        public string? LastName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^[a-zA-Z0-9_\.-]+@([a-zA-Z0-9-]+\.)+[a-zA-Z]{2,3}$", ErrorMessage = "Please Enter valid email address")]
        public string Email { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Password, ErrorMessage = "Enter Valid Password")]
        [RegularExpression(@"^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z]).{6,20}$",ErrorMessage = "Enter Strong Password")]
        public string Password { get; set; } = string.Empty;
        [NotMapped]
        [DataType(DataType.Password, ErrorMessage = "Enter Valid Password")]
        [Compare("Password", ErrorMessage = "Password Doesn't Match")]
        public string ConfirmPassword { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Enter Valid Phone Number")]
        [MaxLength(10, ErrorMessage = "Phone Number can only be of 10 digits")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$",
                   ErrorMessage = "Entered phone format is not valid.")]
        public string PhoneNumber { get; set; } = string.Empty;
        [Required]
        public byte GenderId { get; set; }
        [Required]
        public long CountryId { get; set; }
        [Required]
        public long StateId { get; set; }
        [Required]
        public long CityId { get; set; }
        public IFormFile? Avtar { get; set; }
        public IEnumerable<SelectListItem> CountryList { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> StateListId { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> CityListById { get; set; } = new List<SelectListItem>();

        public User GetUserDetials(User model)
        {
            User user = new User()
            {
                FirstName = FirstName.Trim(),
                LastName = string.IsNullOrEmpty(LastName)?null:LastName.Trim(),
                Email = Email.Trim().ToLower(),
                Password = BCrypt.Net.BCrypt.HashPassword(Password.ToLower()),
                PhoneNumber = PhoneNumber.Trim(),
                GenderId = GenderId,
                CountryId = CountryId,
                StateId = StateId,
                CityId = CityId,
                CreatedAt = DateTime.UtcNow,
            };
            return user;
        }
    }
}
