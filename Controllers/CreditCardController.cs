using Microsoft.AspNetCore.Mvc;
using Web_App_BMS.DTOs;
using Web_App_BMS.Services;

namespace Web_App_BMS.Controllers
{
    public class CreditCardController : Controller
    {
        readonly ICardService _cardService;
        public CreditCardController(ICardService CardService)
        {
            _cardService = CardService;
        }

        [HttpGet]
        public IActionResult AddCreditCard()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddCreditCard(CreditCardDTO creditCardDTO)
        {
            if(ModelState.IsValid)
            {
                if (_cardService.AddCreditCardToDB(creditCardDTO))
                {
                    return RedirectToAction("ReadCreditCards");
                }

                return View(creditCardDTO);
            }

            return View(creditCardDTO);
        }

        [HttpGet]
        public IActionResult EditCreditCard(int id)
        {
            var creditCardDTO = _cardService.GetCreditCardDTOById(id);

            return View(creditCardDTO);
        }

        [HttpPost]
        public IActionResult EditCreditCard(CreditCardDTO creditCardDTO)
        {
            if (ModelState.IsValid)
            {
                if(_cardService.EditCreditCardFromDB(creditCardDTO))
                {
                    return RedirectToAction("ReadCreditCards");
                }

                return View(creditCardDTO);
            }

            return View(creditCardDTO);
        }

        [HttpGet]
        public IActionResult DeleteCreditCard(int id)
        {
            _cardService.DeleteCreditCardFromDB(id);

            return RedirectToAction("ReadCreditCards");
        }

        [HttpGet]
        public IActionResult ReadCreditCards()
        {
            var creditCards = _cardService.GetCreditCards();

            return View(creditCards);
        }
    }
}
