using System.ComponentModel.DataAnnotations;

namespace Web_App_BMS.Model
{
    public class Account
    {
        private int _account_id;
        public int AccountId
        {
            get { return _account_id; }
            private set { _account_id = value; }
        }
        private int? _account_balance;
        public int? Account_Balance
        {
            get { return _account_balance; }
            set { _account_balance = value; }
        }
        private string? _account_type;
        public string? Account_Type
        {
            get { return _account_type; }
            set { _account_type = value; }
        }
        // one-to-many relationship with Branch
        private int? _branchID;
        public int? BranchID
        {
            get { return _branchID; }
            set { _branchID = value; }
        }
        public Branch Branch { get; set; }

        public Customer Customer { get; set; }
        public Loan Loan { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
        public CreditCard Card { get; set; }
        public Account(int _account_balance,
                                string _account_type, int _branchID)
        {
            this.Account_Balance = _account_balance;
            this.Account_Type = _account_type;
            this.BranchID = _branchID;
            this.Transactions = new List<Transaction>();
        }
        public Account() //empty constructor for EFCore
        {
            
        }
    }
}
