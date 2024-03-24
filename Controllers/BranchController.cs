using Microsoft.AspNetCore.Mvc;
using Web_App_BMS.DTOs;
using Web_App_BMS.Services;

namespace Web_App_BMS.Controllers
{
    public class BranchController : Controller
    {
        readonly IBranchService _branchService;
        public BranchController(IBranchService branchService)
        {
            this._branchService = branchService;
        }
        [HttpGet]
        public IActionResult AddBranch()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddBranch(BranchDTO branchDTO)
        {
            if(ModelState.IsValid)
            {
                if (_branchService.AddBranchToDB(branchDTO))
                {
                    return RedirectToAction("ReadBranches");
                }
                return View(branchDTO);
            }
            return View(branchDTO);
        }
        [HttpGet]
        public IActionResult EditBranch(int id)
        {
            var branchDTO = _branchService.GetBranchDTOById(id);
            return View(branchDTO);
        }

        [HttpPost]
        public IActionResult EditBranch(BranchDTO branchDTO)
        {
            if (ModelState.IsValid)
            {
                if (_branchService.EditBranchFromDB(branchDTO))
                {
                    return RedirectToAction("ReadBranches");
                }
                return View(branchDTO);
            }
            return View(branchDTO);
        }

        [HttpGet]
        public IActionResult DeleteBranch(int id)
        {
            _branchService.DeleteBranchFromDB(id);
            return RedirectToAction("ReadBranches");
        }

        [HttpGet]
        public IActionResult ReadBranches()
        {
            var branches = _branchService.GetBranches();
            return View(branches);
        }
    }
}
