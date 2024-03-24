using Web_App_BMS.DTOs;
using Web_App_BMS.Model;

namespace Web_App_BMS.Services
{
    public interface ILoanService
    {
        public bool AddLoanToDB(LoanDTO customer);
        public LoanDTO? GetLoanDTOById(int id);
        public bool CheckIfAlreadyExists(LoanDTO loanDTO);
        public bool BindToLoanFromView(LoanDTO loanDTO,
                                           Loan _loan);
        public bool EditLoanFromDB(LoanDTO loanDTO);
        public bool DeleteLoanFromDB(int id);
        public IEnumerable<Loan> GetLoans();
    }
}
