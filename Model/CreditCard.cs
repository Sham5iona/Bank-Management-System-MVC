namespace Web_App_BMS.Model
{
    public class CreditCard
    {
        private int _card_id;
        public int CreditCardId
        {
            get { return _card_id; }
            private set { _card_id = value; }
        }

        //one-to-one relationship with Customer
        private int? _customerID;
        public int? CustomerID
        {
            get { return _customerID; }
            set { _customerID = value; }
        }
        public Customer Customer;

        //one-to-one relationship with Account
        private int? _accountID;
        public int? AccountID
        {
            get { return _accountID; }
            set { _accountID = value; }
        }
        public Account Account;

        private DateTime? _expiry_date;
        public DateTime? ExpiryDate
        {
            get { return _expiry_date; }
            set
            {
                _expiry_date = value;
            }
        }
        private int? _card_limit;
        public int? CardLimit
        {
            get { return _card_limit; }
            set { _card_limit = value; }
        }
        public CreditCard(int _customerID, int _accountID,
                          DateTime _expiry_date, int _card_limit)
        {
            this.CustomerID = _customerID;
            this.AccountID = _accountID;
            this.ExpiryDate = _expiry_date;
            this.CardLimit = _card_limit;
        }
        public CreditCard() //empty constructor for EFCore
        {
            
        }

    }
}
