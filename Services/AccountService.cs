using Microsoft.EntityFrameworkCore;
using Web_App_BMS.Data;
using Web_App_BMS.DTOs;
using Web_App_BMS.Model;

namespace Web_App_BMS.Services
{
    public class AccountService : AccountDTO, IAccountService
    {
        readonly BMS_DBContext _db;
        readonly ILogger<AccountService> _logger;
        readonly ICustomerService _customerService;
        public AccountService(BMS_DBContext _db,
            ILogger<AccountService> _logger, ICustomerService _customerService)
        {
            this._db = _db;
            this._logger = _logger;
            this._customerService = _customerService;
        }
        private static int _account_id = 0;
        public bool AddAccountToDB(AccountDTO accountDTO)
        {
            if (accountDTO != null)
            {
                if (CheckIfAlreadyExists(accountDTO))
                {
                    Account _account = new Account();
                    if (BindToAccountFromView(accountDTO, _account))
                    {
                        _db.Account.Add(_account);
                        _db.SaveChanges();
                        _logger.LogInformation("Successfully added" +
                            " an account to DB!!!");
                        return true;
                    }

                    accountDTO.NotFoundPropertyMessage = "There is no data" +
                        " corresponding to the input one !";

                    return false;

                }

                accountDTO.AccountAlreadyExists = "Can't add to database" +
                    " because there is already an account with this credentials!";
                return false;

            }

            return false;
        }

        public AccountDTO? GetAccountDTOById(int account_id)
        {
            _account_id = account_id;
            AccountDTO accountDTO = new AccountDTO();
            var account = _db.Account
                .Include(a => a.Branch)
                .FirstOrDefault(a => a.AccountId == _account_id);

            accountDTO.AccountBalance = account.Account_Balance;
            accountDTO.AccountType = account.Account_Type;
            accountDTO.BranchName = account.Branch.BranchName;
            accountDTO.BranchAddress = account.Branch.BranchAddress;
            accountDTO.BranchAssets = account.Branch.BranchAssets;

            if (accountDTO is null)
            {
                _logger.LogCritical("Account is null !");
                return null;
            }

            return accountDTO;
        }

        public bool CheckIfAlreadyExists(AccountDTO accountDTO)
        {
            var account = _db.Account.FirstOrDefault(a =>
            a.Account_Balance == accountDTO.AccountBalance &&
            a.Account_Type == accountDTO.AccountType &&
            a.Branch.BranchName == accountDTO.BranchName
            && a.Branch.BranchAddress == accountDTO.BranchAddress &&
            a.Branch.BranchAssets == accountDTO.BranchAssets);

            if (account == null)
            {
                return true;
            }

            return false;
        }

        public bool BindToAccountFromView(AccountDTO accountDTO, Account _account)
        {
            //Values of the AccountDTO can't be null because of the validation
            //which won't allow them to pass to the services

            if (_customerService.GetBranchID(accountDTO.BranchName,
                accountDTO.BranchAddress, accountDTO.BranchAssets) != -1)
            {
                _account.Account_Balance = accountDTO.AccountBalance;
                _account.Account_Type = accountDTO.AccountType;
                _account.BranchID = _customerService.GetBranchID(
                        accountDTO.BranchName,accountDTO.BranchAddress,
                        accountDTO.BranchAssets);

                return true;
            }

            return false;
        }

        public bool EditAccountFromDB(AccountDTO accountDTO)
        {
            var account = _db.Account.Include(a => a.Branch)
                .FirstOrDefault(a => a.AccountId == _account_id);

            if (account != null)
            {

                account.Account_Balance = accountDTO.AccountBalance;
                account.Account_Type = accountDTO.AccountType;
                account.Branch.BranchName = accountDTO.BranchName;
                account.Branch.BranchAddress = accountDTO.BranchAddress;
                account.Branch.BranchAssets = accountDTO.BranchAssets;

                int branchId = _customerService.
                    GetBranchID(accountDTO.BranchName,
                    accountDTO.BranchAddress, accountDTO.BranchAssets);

                if (branchId != -1)
                {
                    account.BranchID = branchId;
                    _db.SaveChanges(); // Save changes to the database
                    _logger.LogInformation("Successfully updated" +
                        " an account from DB!!!");
                    return true;
                }
                else
                {
                    accountDTO.NotFoundPropertyMessage = "There is no branch" +
                        " with this credentials.";
                    return false;
                }
            }
            else
            {
                // Account not found
                return false;
            }
        }

        public bool DeleteAccountFromDB(int id)
        {
            var account = _db.Account.FirstOrDefault(a => a.AccountId == id);
            if (account is not null)
            {
                _db.Account.Remove(account);
                _db.SaveChanges();
                _logger.LogInformation("Successfully deleted an" +
                    " account from DB!!!");
                return true;
            }

            return false;
        }

        public IEnumerable<Account> GetAccounts()
        {
            var accounts = _db.Account.Include(a => a.Branch);

            return accounts;
        }
    }

}
