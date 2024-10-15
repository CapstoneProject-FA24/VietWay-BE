using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using VietWay.Repository.EntityModel.Base;

namespace VietWay.API.Customer.RequestModel
{
    public class CreateAccountRequest
    {
        public string Email { get; set; }
        [Required(ErrorMessage = "Phonenumber is required")]
        public required string PhoneNumber
        {
            get => phoneNumber;
            set
            {
                if (!IsValidPhoneNumber(value))
                {
                    throw new ValidationException("Phone number must be exactly 10 digits.");
                }
                phoneNumber = value;
            }
        }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        public required string Password
        {
            get => password;
            set
            {
                if (!IsValidPassword(value))
                {
                    throw new ValidationException("Password must be at least 8 characters long, and contain at least 1 letter, 1 number, 1 special character, 1 lowercase letter, and 1 uppercase letter.");
                }
                password = value;
            }
        }

        [Required(ErrorMessage = "Full name is required")]
        public required string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public string ProvinceId { get; set; }

        private string phoneNumber;
        private string password;
        private bool IsValidPassword(string password)
        {
            string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$";
            return Regex.IsMatch(password, pattern);
        }
        private bool IsValidPhoneNumber(string phoneNumber)
        {
            string pattern = @"^\d{10}$";
            return Regex.IsMatch(phoneNumber, pattern);
        }
    }
}
