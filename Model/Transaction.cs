namespace Web_App_BMS.Model
{
    public class Transaction
    {
        private int _transaction_id;
        public int TransactionId
        {
            get { return _transaction_id; }
            private set { _transaction_id = value; }
        }

        private string? _transaction_type;
        public string? TransactionType
        {
            get { return _transaction_type; }
            set { _transaction_type = value;}
        }

        private int? _amount;
        public int? Amount
        {
            get { return _amount; }
            set { _amount = value; }
        }

        //one-to-many relationship with Customer
        private int? _customerID;
        public int? CustomerID
        {
            get { return _customerID; }
            set { _customerID = value; }
        }
        
        public Customer Customer;

        // one-to-many relationship with Account
        private int? _accountID;
        public int? AccountID
        {
            get { return _accountID; }
            set { _accountID = value; }
        }

        public Account Account;
        public Transaction(string _transaction_type, int _amount,
                        int _customerID, int _accountID)
        {
            this.TransactionType = _transaction_type;
            this.Amount = _amount;
            this.CustomerID = _customerID;
            this.AccountID = _accountID;
        }
        public Transaction() //empty constructor for EFCore
        {
            
        }
    }
}
