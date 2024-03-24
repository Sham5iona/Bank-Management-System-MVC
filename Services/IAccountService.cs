using Web_App_BMS.DTOs;
using Web_App_BMS.Model;

namespace Web_App_BMS.Services
{
    public interface IAccountService
    {
        public bool AddAccountToDB(AccountDTO accountDTO);
        public AccountDTO? GetAccountDTOById(int id);
        public bool CheckIfAlreadyExists(AccountDTO accountDTO);
        public bool BindToAccountFromView(AccountDTO accountDTO,
                                          Account account);
        public bool EditAccountFromDB(AccountDTO accountDTO);
        public bool DeleteAccountFromDB(int id);
        public IEnumerable<Account> GetAccounts();
    }
}
