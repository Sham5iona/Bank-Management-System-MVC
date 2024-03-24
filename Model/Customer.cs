using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;
namespace Web_App_BMS.Model
{
    public class Customer
    {

        private int _customerid;
        public int CustomerId 
        {
            get {return _customerid;}
            private set { _customerid = value; }
        }

        private string? _customer_name;
        public string? Customer_name
        {
            get { return _customer_name; }
            set { _customer_name = value;}
        }

        private DateTime? _date_of_birth;
        public DateTime? Date_of_birth
        {
            get { return _date_of_birth; }
            set { _date_of_birth = value;}
        }

        private string? _phone_number;
        public string? Phone_number
        {
            get { return _phone_number; }
            set { _phone_number = value;}
        }

        // one-to-one relationship with Account
        private int? _accountID;
        public int? AccountID
        {
            get { return _accountID; }
            set { _accountID = value; }
        }
        public Account Account { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
        public CreditCard Card { get; set; }
        public Customer(string _customer_name,
                      DateTime _date_of_birth, string _phone_number,
                      int _accountID)
        {
            this.Customer_name = _customer_name;
            this.Date_of_birth = _date_of_birth;
            this.Phone_number = _phone_number;
            this.AccountID = _accountID;
            this.Transactions = new List<Transaction>();

        }
        public Customer() // empty constructor for EFCore
        {
            
        }
    }
}
