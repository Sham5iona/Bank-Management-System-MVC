using Microsoft.EntityFrameworkCore;
using Web_App_BMS.Data;
using Web_App_BMS.DTOs;
using Web_App_BMS.Model;

namespace Web_App_BMS.Services
{
    public class LoanPaymentService : ILoanPaymentService
    {
        readonly BMS_DBContext _db;
        readonly ILogger<LoanPaymentService> _logger;
        readonly ICustomerService _customerService;
        public LoanPaymentService(ILogger<LoanPaymentService> logger, BMS_DBContext dbContext,
            ICustomerService customerService)
        {
            _db = dbContext;
            _logger = logger;
            _customerService = customerService;
        }
        private static int _loan_payment_id = 0;
        public LoanPaymentDTO? GetLoanPaymentDTOById(int loan_Payment_id)
        {
            _loan_payment_id = loan_Payment_id;
            
            var loan_payment = _db.Loan_Payment
                .Include(lp => lp.Loan)
                .ThenInclude(l => l.Account)
                .ThenInclude(a => a.Branch)
                .FirstOrDefault(lp => lp.Loan_PaymentId == _loan_payment_id);

            LoanPaymentDTO Loan_PaymentDTO = new LoanPaymentDTO();

            Loan_PaymentDTO.PaymentAmount = loan_payment.Amount;
            Loan_PaymentDTO.IssuedAmount = loan_payment.Loan.Issued_amount;
            Loan_PaymentDTO.RemainingAmount = loan_payment.Loan.Remaining_amount;
            Loan_PaymentDTO.AccountBalance = loan_payment.Loan.Account.Account_Balance;
            Loan_PaymentDTO.AccountType = loan_payment.Loan.Account.Account_Type;
            Loan_PaymentDTO.BranchName = loan_payment.Loan.Branch.BranchName;
            Loan_PaymentDTO.BranchAddress = loan_payment.Loan.Branch.BranchAddress;
            Loan_PaymentDTO.BranchAssets = loan_payment.Loan.Branch.BranchAssets;

            if (Loan_PaymentDTO is null)
            {
                _logger.LogCritical("Loan Payment is null !");
                return null;
            }

            return Loan_PaymentDTO;
        }

        public bool BindToLoanPaymentFromView(LoanPaymentDTO Loan_PaymentDTO,
                                           Loan_Payment Loan_Payment)
        {
            int accountId = _customerService.GetAccountID(Loan_PaymentDTO.AccountBalance,
                Loan_PaymentDTO.AccountType, Loan_PaymentDTO.BranchName,
                Loan_PaymentDTO.BranchAddress, Loan_PaymentDTO.BranchAssets);

            int branchId = _customerService.GetBranchID(Loan_PaymentDTO.BranchName,
                Loan_PaymentDTO.BranchAddress, Loan_PaymentDTO.BranchAssets);

            //Values of the LoanPaymentDTO can't be null because of the validation
            //which won't allow them to pass to the services
            if (GetLoanID(Loan_PaymentDTO.IssuedAmount,
                Loan_PaymentDTO.RemainingAmount, branchId, accountId)
                != -1)
            {
                Loan_Payment.Amount = Loan_PaymentDTO.PaymentAmount;

                Loan_Payment.LoanID = GetLoanID(Loan_PaymentDTO.IssuedAmount,
                    Loan_PaymentDTO.RemainingAmount,
                    _customerService.GetBranchID(
                    Loan_PaymentDTO.BranchName, Loan_PaymentDTO.BranchAddress,
                    Loan_PaymentDTO.BranchAssets),
                    _customerService.GetAccountID(
                    Loan_PaymentDTO.AccountBalance, Loan_PaymentDTO.AccountType,
                    Loan_PaymentDTO.BranchName, Loan_PaymentDTO.BranchAddress,
                    Loan_PaymentDTO.BranchAssets));

                return true;
            }

            return false;
        }


        public bool CheckIfAlreadyExists(LoanPaymentDTO Loan_PaymentDTO)
        {
            var Loan_Payment = _db.Loan_Payment.FirstOrDefault(lp =>
            lp.Loan.Issued_amount == Loan_PaymentDTO.IssuedAmount &&
            lp.Loan.Remaining_amount == Loan_PaymentDTO.RemainingAmount &&
            lp.Loan.Account.Account_Balance == Loan_PaymentDTO.AccountBalance &&
            lp.Loan.Account.Account_Type == Loan_PaymentDTO.AccountType &&
            lp.Loan.Branch.BranchName == Loan_PaymentDTO.BranchName
            && lp.Loan.Branch.BranchAddress == Loan_PaymentDTO.BranchAddress &&
            lp.Loan.Branch.BranchAssets == Loan_PaymentDTO.BranchAssets);

            if (Loan_Payment == null)
            {
                return true;
            }

            return false;
        }

        public int GetLoanID(int? issued_amount, int? remaining_amount,
                              int? branchId, int? accountId)
        {
            var loan = _db.Loan.FirstOrDefault(l => l.Issued_amount == issued_amount &&
            l.Remaining_amount == remaining_amount && l.BranchID ==  branchId &&
            l.AccountID == accountId);
            return loan != null ? loan.LoanID : -1;

        }

        public bool EditLoanPaymentFromDB(LoanPaymentDTO Loan_PaymentDTO)
        {
            var Loan_Payment = _db.Loan_Payment.Include(lp => lp.Loan).
                ThenInclude(l => l.Account)
                .ThenInclude(a => a.Branch)
                .FirstOrDefault(lp => lp.Loan_PaymentId == _loan_payment_id);

            if (Loan_Payment != null)
            {
                int accountId = _customerService.GetAccountID(Loan_PaymentDTO.AccountBalance,
                Loan_PaymentDTO.AccountType, Loan_PaymentDTO.BranchName,
                Loan_PaymentDTO.BranchAddress, Loan_PaymentDTO.BranchAssets);

                int branchId = _customerService.GetBranchID(Loan_PaymentDTO.BranchName,
                    Loan_PaymentDTO.BranchAddress, Loan_PaymentDTO.BranchAssets);

                Loan_Payment.Amount = Loan_PaymentDTO.PaymentAmount;
                

                if (accountId != -1 && branchId != -1)
                {
                    Loan_Payment.LoanID = GetLoanID(
                        Loan_PaymentDTO.IssuedAmount, Loan_PaymentDTO.RemainingAmount,
                        branchId, accountId);

                    _db.SaveChanges(); // Save changes to the database
                    _logger.LogCritical("Successfully updated a Loan Payment" +
                        " from DB!!!");
                    return true;
                }
                else
                {
                    Loan_PaymentDTO.NotFoundPropertyMessage = "There is" +
                        " no Loan, Account or Branch with this credentials.";
                    return false;
                }
            }
            else
            {
                // Loan Payment not found
                return false;
            }
        }

        public bool DeleteLoanPaymentFromDB(int id)
        {
            var Loan_Payment = _db.Loan_Payment.FirstOrDefault(l => l.Loan_PaymentId == id);
            if (Loan_Payment is not null)
            {
                _db.Loan_Payment.Remove(Loan_Payment);
                _db.SaveChanges();
                _logger.LogInformation("Successfully deleted a Loan Payment" +
                    " from DB!!!");
                return true;
            }

            return false;
        }


        public bool AddLoanPaymentToDB(LoanPaymentDTO Loan_PaymentDTO)
        {
            if (Loan_PaymentDTO != null)
            {
                if (CheckIfAlreadyExists(Loan_PaymentDTO))
                {
                    Loan_Payment Loan_Payment = new Loan_Payment();
                    if (BindToLoanPaymentFromView(Loan_PaymentDTO, Loan_Payment))
                    {
                        _db.Loan_Payment.Add(Loan_Payment);
                        _db.SaveChanges();
                        _logger.LogInformation("Successfully added" +
                            " a Loan Payment to DB!!!");
                        return true;
                    }

                    Loan_PaymentDTO.NotFoundPropertyMessage = "There is no data" +
                        " corresponding to the input one !";

                    return false;

                }
                Loan_PaymentDTO.LoanPaymentAlreadyExists = "Can't add to database" +
                    " because there is already a Loan Payment with this credentials!";
                return false;

            }

            return false;
        }

        public IEnumerable<Loan_Payment> GetLoanPayments()
        {
            var Loan_Payments = _db.Loan_Payment
                .Include(lp => lp.Loan)
                .ThenInclude(l => l.Account)
                .ThenInclude(a => a.Branch);

            return Loan_Payments;
        }
    }
}
