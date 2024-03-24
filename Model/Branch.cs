using System.ComponentModel.DataAnnotations;

namespace Web_App_BMS.Model
{
    public class Branch
    {
        private int _branchID;
        public int BranchID
        {
            get { return _branchID; }
            private set { _branchID = value; }
        }
        private string? _branch_name;
        public string? BranchName
        {
            get { return _branch_name; }
            set { _branch_name = value; }
        }
        private string? _branch_address;
        public string? BranchAddress
        {
            get { return _branch_address; }
           set { _branch_address = value; }
        }
        private int? _branch_assets;
        public int? BranchAssets
        {
            get { return _branch_assets; }
            set { _branch_assets = value; }
        }
        // one-to-many relationship with Loan
        public ICollection<Loan> Loans { get; set; }
        public ICollection<Account> Accounts { get; set; }
        public Banker Banker { get; set; }

        public Branch(string _branch_name, string _branch_address,
                      int _branch_assets)
        {
            this.BranchName = _branch_name;
            this.BranchAddress = _branch_address;
            this.BranchAssets = _branch_assets;
            this.Loans = new List<Loan>();
            this.Accounts = new List<Account>();
        }
        public Branch() // empty constructor for EFCore
        {
            
        }
    }
}
