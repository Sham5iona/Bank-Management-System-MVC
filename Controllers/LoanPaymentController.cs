using Microsoft.AspNetCore.Mvc;
using Web_App_BMS.DTOs;
using Web_App_BMS.Services;

namespace Web_App_BMS.Controllers
{
    public class LoanPaymentController : Controller
    {
        readonly ILoanPaymentService _loanPaymentService;
        public LoanPaymentController(ILoanPaymentService loanPaymentService)
        {
            _loanPaymentService = loanPaymentService;
        }

        [HttpGet]
        public IActionResult AddLoanPayment()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddLoanPayment(LoanPaymentDTO loanPaymentDTO)
        {
            if(ModelState.IsValid)
            {
                if (_loanPaymentService.AddLoanPaymentToDB(loanPaymentDTO))
                {
                    return RedirectToAction("ReadLoanPayments");
                }

                return View(loanPaymentDTO);
            }

            return View(loanPaymentDTO);
        }

        [HttpGet]
        public IActionResult EditLoanPayment(int id)
        {
            var loan_paymentDTO = _loanPaymentService.GetLoanPaymentDTOById(id);

            return View(loan_paymentDTO);
        }

        [HttpPost]
        public IActionResult EditLoanPayment(LoanPaymentDTO loanPaymentDTO)
        {
            if(ModelState.IsValid)
            {
                if (_loanPaymentService.EditLoanPaymentFromDB(loanPaymentDTO))
                {
                    return RedirectToAction("ReadLoanPayments");
                }

                return View(loanPaymentDTO);
            }

            return View(loanPaymentDTO);
        }

        [HttpGet]
        public IActionResult DeleteLoanPayment(int id)
        {
            _loanPaymentService.DeleteLoanPaymentFromDB(id);
            return RedirectToAction("ReadLoanPayments");
        }

        [HttpGet]
        public IActionResult ReadLoanPayments()
        {
            var loan_paymentDTO = _loanPaymentService.GetLoanPayments();
            return View(loan_paymentDTO);

        }

    }
}
