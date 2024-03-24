using Microsoft.AspNetCore.Mvc;
using Web_App_BMS.DTOs;
using Web_App_BMS.Services;

namespace Web_App_BMS.Controllers
{
    public class BankerController : Controller
    {
        readonly IBankerService _bankerService;
        public BankerController(IBankerService bankerService)
        {
            this._bankerService = bankerService;
        }

        [HttpGet]
        public IActionResult AddBanker()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddBanker(BankerDTO bankerDTO)
        {
            if(ModelState.IsValid)
            {
                if (_bankerService.AddBankerToDB(bankerDTO))
                {
                    return RedirectToAction("ReadBankers");
                }
                return View(bankerDTO);
            }
            return View(bankerDTO);
        }

        [HttpGet]
        public IActionResult EditBanker(int id)
        {
            var bankerDTO = _bankerService.GetBankerDTOById(id);
            return View(bankerDTO);
        }

        [HttpPost]
        public IActionResult EditBanker(BankerDTO bankerDTO)
        {
            if (ModelState.IsValid)
            {
                if (_bankerService.EditBankerFromDB(bankerDTO))
                {
                    return RedirectToAction("ReadBankers");
                }

                return View(bankerDTO);
            }
            return View(bankerDTO);
        }

        [HttpGet]
        public IActionResult DeleteBanker(int id)
        {
            _bankerService.DeleteBankerFromDB(id);
            
            return RedirectToAction("ReadBankers");
        }

        [HttpGet]
        public IActionResult ReadBankers()
        {
            var bankers = _bankerService.GetBankers();

            return View(bankers);
        }
    }
}
