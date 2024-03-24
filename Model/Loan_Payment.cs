namespace Web_App_BMS.Model
{
    public class Loan_Payment
    {
        private int _loan_payment_id;
        public int Loan_PaymentId
        {
            get { return _loan_payment_id; }
            private set { _loan_payment_id = value; }
        }
        //one-to-one relationship with Loan
        private int? _loanID;
        public int? LoanID
        {
            get { return _loanID; }
            set { _loanID = value; }
        }
        public Loan Loan;

        private int? _amount;
        public int? Amount
        {
            get { return _amount; }
            set { _amount = value; }
        }
        public Loan_Payment(int _loanID, int _amount)
        {
            this.LoanID = _loanID;
            this.Amount = _amount;
        }
        public Loan_Payment() //empty constructor for EFCore
        {
            
        }
    }
}
