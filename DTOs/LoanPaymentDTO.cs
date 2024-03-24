using System.ComponentModel.DataAnnotations;

namespace Web_App_BMS.DTOs
{
    public class LoanPaymentDTO
    {

        [Display(Name = "loan payment amount!")]
        [Required(ErrorMessage = "Invalid loan payment!")]
        [Range(1, int.MaxValue, ErrorMessage = "Value must be at least 1!")]
        public int? PaymentAmount { get; set; }

        [Display(Name = "issued ammount!")]
        [Required(ErrorMessage = "Invalid issued amount!")]
        [Range(1, int.MaxValue, ErrorMessage = "Value must be at least 1!")]
        public int? IssuedAmount { get; set; }

        [Display(Name = "remaining amount!")]
        [Required(ErrorMessage = "Invalid remaining amount!")]
        [Range(1, int.MaxValue, ErrorMessage = "Value must be at least 1!")]
        public int? RemainingAmount { get; set; }

        [Display(Name = "customer's account balance")]
        [Required(ErrorMessage = "Please, enter an account balance")]
        [Range(1, int.MaxValue, ErrorMessage = "Value must be at least 1!")]

        public int? AccountBalance { get; set; }

        [Display(Name = "customer's account type")]
        [Required(ErrorMessage = "Please, enter an account type!")]
        public string? AccountType { get; set; }

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

        public string? LoanPaymentAlreadyExists { get; set; }
    }
}
