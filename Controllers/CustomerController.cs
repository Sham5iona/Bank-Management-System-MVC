using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_App_BMS.Data;
using Web_App_BMS.DTOs;
using Web_App_BMS.Model;
using Web_App_BMS.Services;

namespace Web_App_BMS
{
    public class CustomerController : Controller
    {
        readonly ICustomerService _customerService;
        readonly ILogger<CustomerController> _logger;
        public CustomerController(ICustomerService customerService,
                                  ILogger<CustomerController> logger)
        {
            _customerService = customerService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult AddCustomer()
        {
            
            return View();
        }

        [HttpPost]
        public IActionResult AddCustomer(CustomerDTO customerDTO)
        {
            
            if (ModelState.IsValid)
            {
                if (_customerService.AddCustomerToDB(customerDTO))
                {
                    //Redirect to GetCustomers view
                    return RedirectToAction("GetCustomers");
                }

                return View(customerDTO);
            }

            return View(customerDTO);
        }

        [HttpGet]
        public IActionResult GetCustomers()
        {
            var customers = _customerService.GetCustomers();

            return View(customers);
        }

        [HttpGet]
        public IActionResult EditCustomer(int id)
        {
            //bind from db to customerDTO
            var customerDTO = _customerService.GetCustomerDTOById(id);

            return View(customerDTO);
        }

        [HttpPost]
        public IActionResult EditCustomer(CustomerDTO customerDTO)
        {
            if (ModelState.IsValid)
            {
                if (_customerService.EditCustomerFromDB(customerDTO))
                {
                    //Redirect to GetCustomers view
                    return RedirectToAction("GetCustomers");
                }

                return View(customerDTO);

            }
            return View(customerDTO);
        }
        [HttpGet]
        public IActionResult DeleteCustomer(int id)
        {
            _customerService.DeleteCustomerFromDB(id);

            return RedirectToAction("GetCustomers");

        }
    }
}