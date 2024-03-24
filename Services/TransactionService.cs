using Microsoft.EntityFrameworkCore;
using Web_App_BMS.Data;
using Web_App_BMS.DTOs;
using Web_App_BMS.Model;

namespace Web_App_BMS.Services
{
    public class TransactionService : ITransactionService
    {
        readonly BMS_DBContext _db;
        readonly ILogger<TransactionService> _logger;
        readonly ICustomerService _customerService;
        public TransactionService(ILogger<TransactionService> logger, BMS_DBContext dbContext,
            ICustomerService customerService)
        {
            _db = dbContext;
            _logger = logger;
            _customerService = customerService;
        }
        private static int _transaction_id = 0;
        public TransactionDTO? GetTransactionDTOById(int transaction_id)
        {
            _transaction_id = transaction_id;

            var transaction = _db.Transaction
                .Include(t => t.Customer)
                .ThenInclude(c => c.Account)
                .ThenInclude(a => a.Branch)
                .FirstOrDefault(t => t.TransactionId == _transaction_id);

            TransactionDTO TransactionDTO = new TransactionDTO();

            TransactionDTO.TransactionType = transaction.TransactionType;
            TransactionDTO.TransactionAmount = transaction.Amount;
            TransactionDTO.AccountBalance = transaction.Account.Account_Balance;
            TransactionDTO.AccountType = transaction.Account.Account_Type;
            TransactionDTO.BranchName = transaction.Account.Branch.BranchName;
            TransactionDTO.BranchAddress = transaction.Account.Branch.BranchAddress;
            TransactionDTO.BranchAssets = transaction.Account.Branch.BranchAssets;
            TransactionDTO.CustomerName = transaction.Customer.Customer_name;
            TransactionDTO.DateOfBirth = transaction.Customer.Date_of_birth;
            TransactionDTO.PhoneNumber = transaction.Customer.Phone_number;
            

            if (TransactionDTO is null)
            {
                _logger.LogCritical("Transaction is null !");
                return null;
            }

            return TransactionDTO;
        }

        public int GetCustomerID(string? customer_name, DateTime? date_of_birth,
            string? phone_number, int? accountId)
        {
            var customer = _db.Customer.FirstOrDefault(c =>
            c.Customer_name == customer_name && c.Date_of_birth == date_of_birth &&
            c.Phone_number == phone_number && c.AccountID == accountId);

            return customer != null ? customer.CustomerId : -1;
        }

        public bool BindToTransactionFromView(TransactionDTO transactionDTO,
                                           Transaction transaction)
        {
            int accountId = _customerService.GetAccountID(transactionDTO.AccountBalance,
                transactionDTO.AccountType, transactionDTO.BranchName,
                transactionDTO.BranchAddress, transactionDTO.BranchAssets);

            int customerId = GetCustomerID(transactionDTO.CustomerName,
                        transactionDTO.DateOfBirth, transactionDTO.PhoneNumber,
                        accountId);

            //Values of the TransactionDTO can't be null because of the validation
            //which won't allow them to pass to the services
            if (accountId != -1 && customerId != -1)
            {
                
                transaction.TransactionType = transactionDTO.TransactionType;
                transaction.Amount = transactionDTO.TransactionAmount;

                transaction.AccountID = accountId;
                transaction.CustomerID = customerId;

                return true;
            }

            return false;
        }


        public bool CheckIfAlreadyExists(TransactionDTO TransactionDTO)
        {
            int accountId = _customerService.GetAccountID(TransactionDTO.AccountBalance,
                TransactionDTO.AccountType, TransactionDTO.BranchName,
                TransactionDTO.BranchAddress, TransactionDTO.BranchAssets);

            var transaction = _db.Transaction.FirstOrDefault(t =>
            t.TransactionType == TransactionDTO.TransactionType &&
            t.Amount == TransactionDTO.TransactionAmount &&
            t.AccountID == accountId &&
            t.CustomerID == GetCustomerID(TransactionDTO.CustomerName,
            TransactionDTO.DateOfBirth, TransactionDTO.PhoneNumber, accountId));

            if (transaction == null)
            {
                return true;
            }

            return false;
        }

        public bool EditTransactionFromDB(TransactionDTO transactionDTO)
        {
            var transaction = _db.Transaction.Include(t => t.Customer).
                ThenInclude(c => c.Account)
                .ThenInclude(a => a.Branch)
                .FirstOrDefault(t => t.TransactionId == _transaction_id);

            if (transaction != null)
            {
                int accountId = _customerService.GetAccountID(
                    transactionDTO.AccountBalance,
                    transactionDTO.AccountType, transactionDTO.BranchName,
                    transactionDTO.BranchAddress, transactionDTO.BranchAssets);

                int customerId = GetCustomerID(transactionDTO.CustomerName,
                        transactionDTO.DateOfBirth, transactionDTO.PhoneNumber,
                        accountId);

                transaction.Amount = transactionDTO.TransactionAmount;


                if (accountId != -1 && customerId != -1)
                {
                    transaction.AccountID = accountId;
                    transaction.CustomerID = customerId;

                    _db.SaveChanges(); // Save changes to the database
                    _logger.LogCritical("Successfully updated a transaction" +
                        " from DB!!!");
                    return true;
                }
                else
                {
                    transactionDTO.NotFoundPropertyMessage = "There is" +
                        " no Customer, Account or Branch with this credentials.";
                    return false;
                }
            }
            else
            {
                // Transaction not found
                return false;
            }
        }

        public bool DeleteTransactionFromDB(int id)
        {
            var transaction = _db.Transaction.FirstOrDefault(t => t.TransactionId == id);
            
            if (transaction is not null)
            {
                _db.Transaction.Remove(transaction);
                _db.SaveChanges();
                _logger.LogInformation("Successfully deleted a Transaction" +
                    " from DB!!!");
                return true;
            }

            return false;
        }


        public bool AddTransactionToDB(TransactionDTO transactionDTO)
        {
            if (transactionDTO != null)
            {
                if (CheckIfAlreadyExists(transactionDTO))
                {
                    Transaction transaction= new Transaction();
                    if (BindToTransactionFromView(transactionDTO, transaction))
                    {
                        _db.Transaction.Add(transaction);
                        _db.SaveChanges();
                        _logger.LogInformation("Successfully added" +
                            " a Transaction to DB!!!");
                        return true;
                    }

                    transactionDTO.NotFoundPropertyMessage = "There is no data" +
                        " corresponding to the input one !";

                    return false;

                }
                transactionDTO.TransactionAlreadyExists = "Can't add to database" +
                    " because there is already a Transaction with this credentials!";
                return false;

            }

            return false;
        }

        public IEnumerable<Transaction> GetTransactions()
        {
            var transactions = _db.Transaction
                .Include(t => t.Customer)
                .ThenInclude(l => l.Account)
                .ThenInclude(a => a.Branch);

            return transactions;
        }
    }
}
