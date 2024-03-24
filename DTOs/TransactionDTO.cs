using System.ComponentModel.DataAnnotations;

namespace Web_App_BMS.DTOs
{
    public class TransactionDTO
    {
        [Display(Name = "transaction type!")]
        [Required(ErrorMessage = "Please, enter a transaction type!")]
        public string? TransactionType { get; set; }

        [Display(Name = "transaction amount!")]
        [Required(ErrorMessage = "Invalid transaction amount!")]
        [Range(1, int.MaxValue, ErrorMessage = "Value must be at least 1!")]
        public int? TransactionAmount { get; set; }

        [Display(Name = "customer's account balance")]
        [Required(ErrorMessage = "Please, enter an account balance")]
        [Range(1, int.MaxValue, ErrorMessage = "Value must be at least 1!")]
        public int? AccountBalance { get; set; }

        [Display(Name = "customer's account type")]
        [Required(ErrorMessage = "Please, enter an account type!")]
        public string? AccountType { get; set; }

        [Display(Name = "customer's name")]
        [Required(ErrorMessage = "Please, enter customer name!")]
        public string? CustomerName { get; set; }

        [Display(Name = "customer's date of birth")]
        [Required(ErrorMessage = "Enter valid datetime!")]
        public DateTime? DateOfBirth { get; set; }

        [Display(Name = "customer's phone number")]
        [Required(ErrorMessage = "Please, enter a phone number!")]
        [Phone(ErrorMessage = "Phone number must be numbers!")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be 10 digits.")]
        public string? PhoneNumber { get; set; }

        [Display(Name = "branch name")]
        [Required(ErrorMessage = "Please, enter a branch name!")]
        public string? BranchName { get; set; }

        [Display(Name = "branch address")]
        [Required(ErrorMessage = "Please, enter a branch address!")]
        public string? BranchAddress { get; set; }

        [Display(Name = "branch assets")]
        [Required(ErrorMessage = "Please, enter branch assets!")]
        [Range(1, int.MaxValue, ErrorMessage = "Value must be at least 1!")]
        public int? BranchAssets { get; set; }

        public string? NotFoundPropertyMessage { get; set; }

        public string? TransactionAlreadyExists { get; set; }

    }
}
