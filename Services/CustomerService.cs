using Microsoft.EntityFrameworkCore;
using Web_App_BMS.Data;
using Web_App_BMS.DTOs;
using Web_App_BMS.Model;

namespace Web_App_BMS.Services
{
    public class CustomerService : CustomerDTO, ICustomerService
    {
        readonly BMS_DBContext _db;
        readonly ILogger<CustomerService> _logger;
        public CustomerService(ILogger<CustomerService> logger,BMS_DBContext dbContext)
        {
            _db = dbContext;
            _logger = logger;

        }
        private static int _customer_id = 0;
        public CustomerDTO? GetCustomerDTOById(int customer_id)
        {
            _customer_id = customer_id;
            CustomerDTO customerDTO = new CustomerDTO();
            var customer = _db.Customer
                .Include(c => c.Account)
                .ThenInclude(a => a.Branch)
                .FirstOrDefault(c => c.CustomerId == _customer_id);

            customerDTO.CustomerName = customer.Customer_name;
            customerDTO.DateOfBirth = customer.Date_of_birth;
            customerDTO.PhoneNumber = customer.Phone_number;
            customerDTO.AccountBalance = customer.Account.Account_Balance;
            customerDTO.AccountType = customer.Account.Account_Type;
            customerDTO.BranchName = customer.Account.Branch.BranchName;
            customerDTO.BranchAddress = customer.Account.Branch.BranchAddress;
            customerDTO.BranchAssets = customer.Account.Branch.BranchAssets;

            if (customerDTO is null)
            {
                _logger.LogCritical("Customer is null !");
                return null;
            }

            return customerDTO;
        }

        

        public int GetAccountID(int? AccountBalance,
            string? AccountType, string? BranchName, string? BranchAddress,
            int? BranchAssets)
        {

            var account = _db.Account.FirstOrDefault(a =>
            a.Account_Balance == AccountBalance &&
            a.Account_Type == AccountType &&
            a.BranchID == GetBranchID(BranchName,
            BranchAddress, BranchAssets));

            return account != null ? account.AccountId : -1;

        }

        public int GetBranchID(string? BranchName,
            string? BranchAddress, int? BranchAssets)
        {

            var branch = _db.Branch.FirstOrDefault(br => 
            br.BranchName ==  BranchName &&  br.BranchAddress == BranchAddress
            && br.BranchAssets == BranchAssets);

            return branch != null ? branch.BranchID : -1;
        }

        public bool BindToCustomerFromView(CustomerDTO customerDTO,
                                           Customer _customer)
        {
            //Values of the CustomerDTO can't be null because of the validation
            //which won't allow them to pass to the services
            if (GetAccountID(customerDTO.AccountBalance,
                customerDTO.AccountType, customerDTO.BranchName,
                customerDTO.BranchAddress, customerDTO.BranchAssets) != -1)
            {
                _customer.Customer_name = customerDTO.CustomerName;
                _customer.Date_of_birth = customerDTO.DateOfBirth;
                _customer.Phone_number = customerDTO.PhoneNumber;
                _customer.AccountID = GetAccountID(customerDTO.AccountBalance,
                    customerDTO.AccountType, customerDTO.BranchName,
                    customerDTO.BranchAddress, customerDTO.BranchAssets);
                return true;
            }

            return false;
        }
    

        public bool CheckIfAlreadyExists(CustomerDTO customerDTO)
        {
            var customer = _db.Customer.FirstOrDefault(c =>
            c.Account.Account_Balance == customerDTO.AccountBalance &&
            c.Account.Account_Type == customerDTO.AccountType &&
            c.Account.Branch.BranchName == customerDTO.BranchName
            && c.Account.Branch.BranchAddress == customerDTO.BranchAddress &&
            c.Account.Branch.BranchAssets == customerDTO.BranchAssets);

            if(customer == null)
            {
                return true;
            }

            return false;
        }

        public bool EditCustomerFromDB(CustomerDTO customerDTO)
        {
            var customer = _db.Customer.Include(c => c.Account).
                ThenInclude(a => a.Branch)
                .FirstOrDefault(c => c.CustomerId == _customer_id);

            if (customer != null)
            {
                customer.Customer_name = customerDTO.CustomerName;
                customer.Date_of_birth = customerDTO.DateOfBirth;
                customer.Phone_number = customerDTO.PhoneNumber;
                customer.Account.Account_Balance = customerDTO.AccountBalance;
                customer.Account.Account_Type = customerDTO.AccountType;
                customer.Account.Branch.BranchName = customerDTO.BranchName;
                customer.Account.Branch.BranchAddress = customerDTO.BranchAddress;
                customer.Account.Branch.BranchAssets = customerDTO.BranchAssets;

                int accountId = GetAccountID(customerDTO.AccountBalance,
                    customerDTO.AccountType, customerDTO.BranchName,
                    customerDTO.BranchAddress, customerDTO.BranchAssets);

                if (accountId != -1)
                {
                    customer.AccountID = accountId;
                    _db.SaveChanges(); // Save changes to the database
                    _logger.LogCritical("Successfully updated a customer" +
                        " from DB!!!");
                    return true;
                }
                else
                {
                    customerDTO.NotFoundPropertyMessage = "There is no account or branch" +
                        " with this credentials.";
                    return false;
                }
            }
            else
            {
                // Customer not found
                return false;
            }
        }

        public bool DeleteCustomerFromDB(int id)
        {
            var customer = _db.Customer.FirstOrDefault(c => c.CustomerId == id);
            if(customer is not null)
            {
                _db.Customer.Remove(customer);
                _db.SaveChanges();
                _logger.LogInformation("Successfully deleted a customer" +
                    " from DB!!!");
                return true;
            }

            return false;
        }

        
        public bool AddCustomerToDB(CustomerDTO customerDTO)
        {
            if (customerDTO != null)
            {
                if  (CheckIfAlreadyExists(customerDTO))
                {
                    Customer customer = new Customer();
                    if (BindToCustomerFromView(customerDTO, customer))
                    {
                        _db.Customer.Add(customer);
                        _db.SaveChanges();
                        _logger.LogInformation("Successfully added" +
                            " a customer to DB!!!");
                        return true;
                    }

                    customerDTO.NotFoundPropertyMessage = "There is no data" +
                        " corresponding to the input one !";

                    return false;

                }
                customerDTO.UserAlreadyExists = "Can't add to database" +
                    " because there is already a user with this credentials!";
                return false;

            }

            return false;
        }

        public IEnumerable<Customer> GetCustomers()
        {
            var customers = _db.Customer
                .Include(c => c.Account)
                .ThenInclude(a => a.Branch);

            return customers;
        }
    }
}
