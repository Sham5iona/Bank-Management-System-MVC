using Web_App_BMS.DTOs;
using Web_App_BMS.Model;

namespace Web_App_BMS.Services
{
    public interface ICustomerService
    {
        public bool AddCustomerToDB(CustomerDTO customer);
        public CustomerDTO? GetCustomerDTOById(int id);
        public int GetAccountID(int? AccountBalance,
            string? AccountType, string? BranchName, string? BranchAddress,
            int? BranchAssets);
        public int GetBranchID(string? BranchName,
            string? BranchAddress, int? BranchAssets);
        public bool CheckIfAlreadyExists(CustomerDTO customerDTO);
        public bool BindToCustomerFromView(CustomerDTO customerDTO,
                                           Customer _customer);
        public bool EditCustomerFromDB(CustomerDTO customerDTO);
        public bool DeleteCustomerFromDB(int id);
        public IEnumerable<Customer> GetCustomers();
    }
}
