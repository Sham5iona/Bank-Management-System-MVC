using System.ComponentModel.DataAnnotations;

namespace Web_App_BMS.DTOs
{
    public class BranchDTO
    {
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
        public string? BranchAlreadyExists { get; set; }
    }
}
