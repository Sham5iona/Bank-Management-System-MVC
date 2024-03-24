namespace Web_App_BMS.Model
{
    public class Banker
    {
        private int _banker_id;
        public int BankerID
        {
            get { return _banker_id; }
            private set { _banker_id = value; }
        }
        private string? _banker_name;
        public string? BankerName
        {
            get { return _banker_name; }
            set { _banker_name = value; }
        }
        // one-to-one relationship with Branch
        private int? _branchID;
        public int? BranchID
        {
            get { return _branchID; }
            set { _branchID = value; }
        }
        public Branch Branch { get; set; }
        public Banker(string _banker_name, int _branchID)
        {
            this.BankerName = _banker_name;
            this.BranchID = _branchID;
        }
        public Banker() // empty constructor for EFCore
        {

        }

    }
}
