using Microsoft.AspNetCore.Mvc;
using Web_App_BMS.DTOs;
using Web_App_BMS.Services;

namespace Web_App_BMS.Controllers
{
    public class LoanController : Controller
    {
        readonly ILoanService _loanService;
        
        public LoanController(ILoanService loanService)
        {
            _loanService = loanService;
        }

        [HttpGet]
        public IActionResult AddLoan()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddLoan(LoanDTO loanDTO)
        {
            if(ModelState.IsValid)
            {
                if (_loanService.AddLoanToDB(loanDTO))
                {
                    return RedirectToAction("ReadLoans");
                }

                return View(loanDTO);
            }

            return View(loanDTO);
        }

        [HttpGet]
        public IActionResult EditLoan(int id)
        {
            var loanDTO = _loanService.GetLoanDTOById(id);
            return View(loanDTO);
        }

        [HttpPost]
        public IActionResult EditLoan(LoanDTO loanDTO)
        {
            if(ModelState.IsValid)
            {
                if (_loanService.EditLoanFromDB(loanDTO))
                {
                    return RedirectToAction("ReadLoans");
                }

                return View(loanDTO);

            }

            return View(loanDTO);
        }

        [HttpGet]
        public IActionResult DeleteLoan(int id)
        {
            _loanService.DeleteLoanFromDB(id);
            return RedirectToAction("ReadLoans");
        }

        [HttpGet]
        public IActionResult ReadLoans()
        {
            var loans = _loanService.GetLoans();
            return View(loans);
        }
    }
}
