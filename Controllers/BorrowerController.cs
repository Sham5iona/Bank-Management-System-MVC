using Microsoft.AspNetCore.Mvc;
using Web_App_BMS.Services;

namespace Web_App_BMS.Controllers
{
    public class BorrowerController : Controller
    {
        readonly ILoanService _loanService;
        public BorrowerController(ILoanService loanService)
        {
            _loanService = loanService;
        }
        public IActionResult ReadBorrowers()
        {
            var borrowers = _loanService.GetLoans();
            return View(borrowers);
        }
    }
}
