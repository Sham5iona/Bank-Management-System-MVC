using Web_App_BMS.DTOs;
using Web_App_BMS.Model;

namespace Web_App_BMS.Services
{
    public interface ILoanPaymentService
    {
        public bool AddLoanPaymentToDB(LoanPaymentDTO customer);
        public LoanPaymentDTO? GetLoanPaymentDTOById(int id);
        public bool CheckIfAlreadyExists(LoanPaymentDTO LoanPaymentDTO);
        public bool BindToLoanPaymentFromView(LoanPaymentDTO LoanPaymentDTO,
                                           Loan_Payment _LoanPayment);
        public bool EditLoanPaymentFromDB(LoanPaymentDTO LoanPaymentDTO);
        public bool DeleteLoanPaymentFromDB(int id);
        public IEnumerable<Loan_Payment> GetLoanPayments();
        public int GetLoanID(int? issued_amount, int? remaining_amount,
                              int? branchId, int? accountId);
    }
}
