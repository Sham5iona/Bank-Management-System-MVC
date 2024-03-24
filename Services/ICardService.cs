using Web_App_BMS.DTOs;
using Web_App_BMS.Model;

namespace Web_App_BMS.Services
{
    public interface ICardService
    {
        public bool AddCreditCardToDB(CreditCardDTO CreditCardDTO);
        public CreditCardDTO? GetCreditCardDTOById(int id);
        public bool CheckIfAlreadyExists(CreditCardDTO CreditCardDTO);
        public bool BindToCreditCardFromView(CreditCardDTO CreditCardDTO,
                                           CreditCard CreditCard);
        public bool EditCreditCardFromDB(CreditCardDTO CreditCardDTO);
        public bool DeleteCreditCardFromDB(int id);
        public IEnumerable<CreditCard> GetCreditCards();

    }
}
