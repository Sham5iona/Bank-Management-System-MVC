using Microsoft.AspNetCore.Mvc;
using Web_App_BMS.DTOs;
using Web_App_BMS.Services;

namespace Web_App_BMS.Controllers
{
    public class TransactionController : Controller
    {
        readonly ITransactionService _transactionService;
        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;  
        }

        [HttpGet]
        public IActionResult AddTransaction()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddTransaction(TransactionDTO transactionDTO)
        {
            if(ModelState.IsValid)
            {
                if (_transactionService.AddTransactionToDB(transactionDTO))
                {
                    return RedirectToAction("ReadTransactions");
                }
                
                return View(transactionDTO);
                
            }

            return View(transactionDTO);
        }

        [HttpGet]
        public IActionResult EditTransaction(int id)
        {
            var transactionDTO = _transactionService.GetTransactionDTOById(id);

            return View(transactionDTO);
        }

        [HttpPost]
        public IActionResult EditTransaction(TransactionDTO transactionDTO)
        {
            if (ModelState.IsValid)
            {
                if(_transactionService.EditTransactionFromDB(transactionDTO))
                {
                    return RedirectToAction("ReadTransactions");
                }

                return View(transactionDTO);
            }

            return View(transactionDTO);
        }

        [HttpGet]
        public IActionResult DeleteTransaction(int id)
        {
            _transactionService.DeleteTransactionFromDB(id);

            return RedirectToAction("ReadTransactions");
        }

        [HttpGet]
        public IActionResult ReadTransactions()
        {
            var transactions = _transactionService.GetTransactions();

            return View(transactions);
        }
    }
}
