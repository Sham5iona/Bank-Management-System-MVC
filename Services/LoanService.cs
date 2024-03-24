using Microsoft.EntityFrameworkCore;
using Web_App_BMS.Data;
using Web_App_BMS.DTOs;
using Web_App_BMS.Model;

namespace Web_App_BMS.Services
{
    public class LoanService : ILoanService
    {
        readonly BMS_DBContext _db;
        readonly ILogger<LoanService> _logger;
        readonly ICustomerService _customerService;
        public LoanService(ILogger<LoanService> logger, BMS_DBContext dbContext,
            ICustomerService customerService)
        {
            _db = dbContext;
            _logger = logger;
            _customerService = customerService;
        }
        private static int _loan_id = 0;
        public LoanDTO? GetLoanDTOById(int loan_id)
        {
            _loan_id = loan_id;
            LoanDTO loanDTO = new LoanDTO();
            var loan = _db.Loan
                .Include(l => l.Account)
                .ThenInclude(a => a.Branch)
                .FirstOrDefault(l => l.LoanID == _loan_id);

            loanDTO.IssuedAmount =  loan.Issued_amount;
            loanDTO.RemainingAmount = loan.Remaining_amount;
            loanDTO.AccountBalance = loan.Account.Account_Balance;
            loanDTO.AccountType = loan.Account.Account_Type;
            loanDTO.BranchName = loan.Account.Branch.BranchName;
            loanDTO.BranchAddress = loan.Account.Branch.BranchAddress;
            loanDTO.BranchAssets = loan.Account.Branch.BranchAssets;

            if (loanDTO is null)
            {
                _logger.LogCritical("Loan is null !");
                return null;
            }

            return loanDTO;
        }

        public bool BindToLoanFromView(LoanDTO loanDTO,
                                           Loan _loan)
        {
            //Values of the LoanDTO can't be null because of the validation
            //which won't allow them to pass to the services
            if (_customerService.GetAccountID(loanDTO.AccountBalance,
                loanDTO.AccountType, loanDTO.BranchName,
                loanDTO.BranchAddress, loanDTO.BranchAssets) != -1)
            {
                _loan.Issued_amount = loanDTO.IssuedAmount;
                _loan.Remaining_amount = loanDTO.RemainingAmount;
                _loan.AccountID = _customerService.GetAccountID(loanDTO.AccountBalance,
                    loanDTO.AccountType, loanDTO.BranchName,
                    loanDTO.BranchAddress, loanDTO.BranchAssets);
                _loan.BranchID = _customerService.GetBranchID(loanDTO.BranchName,
                    loanDTO.BranchAddress, loanDTO.BranchAssets);
                return true;
            }

            return false;
        }


        public bool CheckIfAlreadyExists(LoanDTO loanDTO)
        {
            var loan = _db.Loan.FirstOrDefault(l =>
            l.Issued_amount == loanDTO.IssuedAmount &&
            l.Remaining_amount == loanDTO.RemainingAmount &&
            l.Account.Account_Balance == loanDTO.AccountBalance &&
            l.Account.Account_Type == loanDTO.AccountType &&
            l.Account.Branch.BranchName == loanDTO.BranchName
            && l.Account.Branch.BranchAddress == loanDTO.BranchAddress &&
            l.Account.Branch.BranchAssets == loanDTO.BranchAssets);

            if (loan == null)
            {
                return true;
            }

            return false;
        }

        public bool EditLoanFromDB(LoanDTO loanDTO)
        {
            var loan = _db.Loan.Include(l => l.Account).
                ThenInclude(a => a.Branch)
                .FirstOrDefault(l => l.LoanID == _loan_id);

            if (loan != null)
            {
                loan.Issued_amount = loanDTO.IssuedAmount;
                loan.Remaining_amount = loanDTO.RemainingAmount;
                loan.Account.Account_Balance = loanDTO.AccountBalance;
                loan.Account.Account_Type = loanDTO.AccountType;
                loan.Account.Branch.BranchName = loanDTO.BranchName;
                loan.Account.Branch.BranchAddress = loanDTO.BranchAddress;
                loan.Account.Branch.BranchAssets = loanDTO.BranchAssets;

                int accountId = _customerService.GetAccountID(loanDTO.AccountBalance,
                    loanDTO.AccountType, loanDTO.BranchName,
                    loanDTO.BranchAddress, loanDTO.BranchAssets);
                int branchId = _customerService.GetBranchID(loanDTO.BranchName,
                    loanDTO.BranchAddress, loanDTO.BranchAssets);

                if (accountId != -1 && branchId != -1)
                {
                    loan.AccountID = accountId;
                    loan.BranchID = branchId;
                    _db.SaveChanges(); // Save changes to the database
                    _logger.LogCritical("Successfully updated a loan" +
                        " from DB!!!");
                    return true;
                }
                else
                {
                    loanDTO.NotFoundPropertyMessage = "There is no Account or Branch" +
                        " with this credentials.";
                    return false;
                }
            }
            else
            {
                // Loan not found
                return false;
            }
        }

        public bool DeleteLoanFromDB(int id)
        {
            var loan = _db.Loan.FirstOrDefault(l => l.LoanID == id);
            if (loan is not null)
            {
                _db.Loan.Remove(loan);
                _db.SaveChanges();
                _logger.LogInformation("Successfully deleted a loan" +
                    " from DB!!!");
                return true;
            }

            return false;
        }


        public bool AddLoanToDB(LoanDTO loanDTO)
        {
            if (loanDTO != null)
            {
                if (CheckIfAlreadyExists(loanDTO))
                {
                    Loan loan = new Loan();
                    if (BindToLoanFromView(loanDTO, loan))
                    {
                        _db.Loan.Add(loan);
                        _db.SaveChanges();
                        _logger.LogInformation("Successfully added" +
                            " a loan to DB!!!");
                        return true;
                    }

                    loanDTO.NotFoundPropertyMessage = "There is no data" +
                        " corresponding to the input one !";

                    return false;

                }
                loanDTO.LoanAlreadyExists = "Can't add to database" +
                    " because there is already a loan with this credentials!";
                return false;

            }

            return false;
        }

        public IEnumerable<Loan> GetLoans()
        {
            var loan = _db.Loan
                .Include(l => l.Account)
                .ThenInclude(a => a.Branch)
                .ThenInclude(b => b.Accounts)
                .ThenInclude(a => a.Customer);

            return loan;
        }
    }
}
