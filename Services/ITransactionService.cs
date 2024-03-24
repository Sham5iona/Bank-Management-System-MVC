using Web_App_BMS.DTOs;
using Web_App_BMS.Model;

namespace Web_App_BMS.Services
{
    public interface ITransactionService
    {
        public bool AddTransactionToDB(TransactionDTO transactionDTO);
        public TransactionDTO? GetTransactionDTOById(int id);
        public bool CheckIfAlreadyExists(TransactionDTO TransactionDTO);
        public bool BindToTransactionFromView(TransactionDTO TransactionDTO,
                                           Transaction transaction);
        public bool EditTransactionFromDB(TransactionDTO TransactionDTO);
        public bool DeleteTransactionFromDB(int id);
        public IEnumerable<Transaction> GetTransactions();

        public int GetCustomerID(string? customer_name, DateTime? date_of_birth,
            string? phone_number, int? accountId);

    }
}
