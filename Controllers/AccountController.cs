using Microsoft.AspNetCore.Mvc;
using Web_App_BMS.DTOs;
using Web_App_BMS.Services;

namespace Web_App_BMS.Controllers
{
    public class AccountController : Controller
    {
        readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            this._accountService = accountService;
        }
        [HttpGet]
        public IActionResult AddAccount()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddAccount(AccountDTO accountDTO)
        {
            if(ModelState.IsValid)
            {
                if (_accountService.AddAccountToDB(accountDTO))
                {
                    return RedirectToAction("ReadAccounts");
                }
                    return View(accountDTO);
            }

            return View(accountDTO);

        }

        [HttpGet]
        public IActionResult EditAccount(int id)
        {
            var accountDTO = _accountService.GetAccountDTOById(id);
            return View(accountDTO);
        }

        [HttpPost]
        public IActionResult EditAccount(AccountDTO accountDTO)
        {
            if(ModelState.IsValid)
            {
                if(_accountService.EditAccountFromDB(accountDTO))
                {
                    return RedirectToAction("ReadAccounts");
                }
                return View(accountDTO);
            }
            return View(accountDTO);
        }

        [HttpGet]
        public IActionResult DeleteAccount(int id)
        {
            _accountService.DeleteAccountFromDB(id);
            return RedirectToAction("ReadAccounts");
        }

        [HttpGet]
        public IActionResult ReadAccounts()
        {
            var accounts = _accountService.GetAccounts();
            return View(accounts);
        }
    }
}
