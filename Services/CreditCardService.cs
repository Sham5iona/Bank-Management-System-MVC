using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Web_App_BMS.Data;
using Web_App_BMS.DTOs;
using Web_App_BMS.Model;

namespace Web_App_BMS.Services
{
    public class CreditCardService : ICardService
    {
        readonly BMS_DBContext _db;
        readonly ILogger<CreditCardService> _logger;
        readonly ICustomerService _customerService;
        readonly ITransactionService _transactionService;
        public CreditCardService(ILogger<CreditCardService> logger, BMS_DBContext dbContext,
            ICustomerService customerService, ITransactionService transactionService)
        {
            _db = dbContext;
            _logger = logger;
            _customerService = customerService;
            _transactionService = transactionService;
        }
        private static int _credit_card_id = 0;
        public CreditCardDTO? GetCreditCardDTOById(int CreditCard_id)
        {
            _credit_card_id = CreditCard_id;

            var CreditCard = _db.CreditCard
                .Include(cr => cr.Customer)
                .ThenInclude(c => c.Account)
                .ThenInclude(a => a.Branch)
                .FirstOrDefault(cr => cr.CreditCardId == _credit_card_id);

            CreditCardDTO CreditCardDTO = new CreditCardDTO();

            CreditCardDTO.ExpiryDate = CreditCard.ExpiryDate;
            CreditCardDTO.CardLimit = CreditCard.CardLimit;
            CreditCardDTO.AccountBalance = CreditCard.Account.Account_Balance;
            CreditCardDTO.AccountType = CreditCard.Account.Account_Type;
            CreditCardDTO.BranchName = CreditCard.Account.Branch.BranchName;
            CreditCardDTO.BranchAddress = CreditCard.Account.Branch.BranchAddress;
            CreditCardDTO.BranchAssets = CreditCard.Account.Branch.BranchAssets;
            CreditCardDTO.CustomerName = CreditCard.Customer.Customer_name;
            CreditCardDTO.DateOfBirth = CreditCard.Customer.Date_of_birth;
            CreditCardDTO.PhoneNumber = CreditCard.Customer.Phone_number;


            if (CreditCardDTO is null)
            {
                _logger.LogCritical("Credit Card is null !");
                return null;
            }

            return CreditCardDTO;
        }

        public bool BindToCreditCardFromView(CreditCardDTO CreditCardDTO,
                                           CreditCard CreditCard)
        {
            int accountId = _customerService.GetAccountID(CreditCardDTO.AccountBalance,
                CreditCardDTO.AccountType, CreditCardDTO.BranchName,
                CreditCardDTO.BranchAddress, CreditCardDTO.BranchAssets);

            int customerId = _transactionService.GetCustomerID(CreditCardDTO.CustomerName,
                        CreditCardDTO.DateOfBirth, CreditCardDTO.PhoneNumber,
                        accountId);

            //Values of the CreditCardDTO can't be null because of the validation
            //which won't allow them to pass to the services
            if (accountId != -1 && customerId != -1)
            {

                CreditCard.ExpiryDate = CreditCardDTO.ExpiryDate;
                CreditCard.CardLimit = CreditCardDTO.CardLimit;

                CreditCard.AccountID = accountId;
                CreditCard.CustomerID = customerId;

                return true;
            }

            return false;
        }


        public bool CheckIfAlreadyExists(CreditCardDTO CreditCardDTO)
        {
            int accountId = _customerService.GetAccountID(CreditCardDTO.AccountBalance,
                CreditCardDTO.AccountType, CreditCardDTO.BranchName,
                CreditCardDTO.BranchAddress, CreditCardDTO.BranchAssets);

            int customerId = _transactionService.GetCustomerID(CreditCardDTO.CustomerName,
                        CreditCardDTO.DateOfBirth, CreditCardDTO.PhoneNumber,
                        accountId);

            var CreditCard = _db.CreditCard.FirstOrDefault(cr =>
            cr.ExpiryDate == CreditCardDTO.ExpiryDate &&
            cr.CardLimit == CreditCardDTO.CardLimit &&
            cr.AccountID == accountId &&
            cr.CustomerID == customerId);

            if (CreditCard == null)
            {
                return true;
            }

            return false;
        }

        public bool EditCreditCardFromDB(CreditCardDTO CreditCardDTO)
        {
            var CreditCard = _db.CreditCard.Include(cr => cr.Customer).
                ThenInclude(c => c.Account)
                .ThenInclude(a => a.Branch)
                .FirstOrDefault(t => t.CreditCardId == _credit_card_id);

            if (CreditCard != null)
            {
                int accountId = _customerService.GetAccountID(
                    CreditCardDTO.AccountBalance,
                    CreditCardDTO.AccountType, CreditCardDTO.BranchName,
                    CreditCardDTO.BranchAddress, CreditCardDTO.BranchAssets);

                int customerId = _transactionService.GetCustomerID(CreditCardDTO.CustomerName,
                        CreditCardDTO.DateOfBirth, CreditCardDTO.PhoneNumber,
                        accountId);

                CreditCard.ExpiryDate = CreditCardDTO.ExpiryDate;
                CreditCard.CardLimit = CreditCardDTO.CardLimit;

                if (accountId != -1 && customerId != -1)
                {
                    CreditCard.AccountID = accountId;
                    CreditCard.CustomerID = customerId;

                    _db.SaveChanges(); // Save changes to the database
                    _logger.LogCritical("Successfully updated a Credit Card" +
                        " from DB!!!");
                    return true;
                }
                else
                {
                    CreditCardDTO.NotFoundPropertyMessage = "There is" +
                        " no Customer, Account or Branch with this credentials.";
                    return false;
                }
            }
            else
            {
                // CreditCard not found
                return false;
            }
        }

        public bool DeleteCreditCardFromDB(int id)
        {
            var CreditCard = _db.CreditCard.FirstOrDefault(t => t.CreditCardId == id);

            if (CreditCard is not null)
            {
                _db.CreditCard.Remove(CreditCard);
                _db.SaveChanges();
                _logger.LogInformation("Successfully deleted a Credit Card" +
                    " from DB!!!");
                return true;
            }

            return false;
        }


        public bool AddCreditCardToDB(CreditCardDTO CreditCardDTO)
        {
            if (CreditCardDTO != null)
            {
                if (CheckIfAlreadyExists(CreditCardDTO))
                {
                    CreditCard CreditCard = new CreditCard();
                    if (BindToCreditCardFromView(CreditCardDTO, CreditCard))
                    {
                        _db.CreditCard.Add(CreditCard);
                        _db.SaveChanges();
                        _logger.LogInformation("Successfully added" +
                            " a CreditCard to DB!!!");
                        return true;
                    }

                    CreditCardDTO.NotFoundPropertyMessage = "There is no data" +
                        " corresponding to the input one !";

                    return false;

                }
                CreditCardDTO.CreditCardAlreadyExists = "Can't add to database" +
                    " because there is already a CreditCard with this credentials!";
                return false;

            }

            return false;
        }

        public IEnumerable<CreditCard> GetCreditCards()
        {
            var CreditCards = _db.CreditCard
                .Include(t => t.Customer)
                .ThenInclude(l => l.Account)
                .ThenInclude(a => a.Branch);

            return CreditCards;
        }
    }
}
