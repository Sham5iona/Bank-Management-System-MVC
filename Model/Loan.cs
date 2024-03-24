namespace Web_App_BMS.Model
{
    public class Loan
    {
        private int _loanId;
        public int LoanID
        {
            get { return _loanId; }
            private set { _loanId = value;}
        }
        private int? _issued_amount;
        public int? Issued_amount
        {
            get { return _issued_amount; }
            set { _issued_amount = value; }
        }
        private int? _remaining_amount;
        public int? Remaining_amount
        {
            get { return _remaining_amount; }
            set { _remaining_amount = value; }
        }
        // one-to-one relationship with Account
        private int? _accountID;
        public int? AccountID
        {
            get { return _accountID; }
            set { _accountID = value; }
        }
        public Account Account { get; set; }

        // many-to-one relationship with Branch
        private int? _branch_ID;
        public int? BranchID
        {
            get { return _branch_ID; }
            set { _branch_ID = value; }
        }
        public Branch Branch { get; set; }
        public Loan_Payment Loan_Payment { get; set; }
        public Loan(int _issued_amount, int _remaining_amount,
                    int _accountID, int _branchID)
        {
            this.Issued_amount = _issued_amount;
            this.Remaining_amount = _remaining_amount;
            this.AccountID = _accountID;
            this.BranchID = _branchID;
        }
        public Loan() //empty constructor for EFCore
        {
            
        }
    }
}
