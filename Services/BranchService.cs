using Web_App_BMS.Data;
using Web_App_BMS.DTOs;
using Web_App_BMS.Model;

namespace Web_App_BMS.Services
{
    public class BranchService : IBranchService
    {
        readonly BMS_DBContext _db;
        readonly ILogger<BranchService> _logger;
        readonly ICustomerService _customerService;
        public BranchService(BMS_DBContext _db,
            ILogger<BranchService> _logger, ICustomerService _customerService)
        {
            this._db = _db;
            this._logger = _logger;
            this._customerService = _customerService;
        }
        private static int _branch_id = 0;
        public bool AddBranchToDB(BranchDTO branchDTO)
        {
            if (branchDTO != null)
            {
                if (CheckIfAlreadyExists(branchDTO))
                {
                    Branch _branch = new Branch();
                    if (BindToBranchFromView(branchDTO, _branch))
                    {
                        _db.Branch.Add(_branch);
                        _db.SaveChanges();
                        _logger.LogInformation("Successfully added" +
                            " a branch to DB!!!");
                        return true;
                    }

                    branchDTO.NotFoundPropertyMessage = "There is no data" +
                        " corresponding to the input one !";

                    return false;

                }

                branchDTO.BranchAlreadyExists = "Can't add to database" +
                    " because there is already a branch with this credentials!";
                return false;

            }

            return false;
        }

        public BranchDTO? GetBranchDTOById(int branch_id)
        {
            _branch_id = branch_id;
            BranchDTO branchDTO = new BranchDTO();
            var branch = _db.Branch
                .FirstOrDefault(br => br.BranchID == _branch_id);

            branchDTO.BranchName = branch.BranchName;
            branchDTO.BranchAddress = branch.BranchAddress;
            branchDTO.BranchAssets = branch.BranchAssets;

            if (branchDTO is null)
            {
                _logger.LogCritical("Branch is null !");
                return null;
            }

            return branchDTO;
        }

        public bool CheckIfAlreadyExists(BranchDTO branchDTO)
        {
            var branch = _db.Branch.FirstOrDefault(br =>
            br.BranchName == branchDTO.BranchName
            && br.BranchAddress == branchDTO.BranchAddress &&
            br.BranchAssets == branchDTO.BranchAssets);

            if (branch == null)
            {
                return true;
            }

            return false;
        }

        public bool BindToBranchFromView(BranchDTO branchDTO, Branch _branch)
        {
            //Values of the BranchDTO can't be null because of the validation
            //which won't allow them to pass to the services
                _branch.BranchName = branchDTO.BranchName;
                _branch.BranchAddress = branchDTO.BranchAddress;
                _branch.BranchAssets = branchDTO.BranchAssets;

                return true;
        }

        public bool EditBranchFromDB(BranchDTO branchDTO)
        {
            var branch = _db.Branch.FirstOrDefault(br => br.BranchID == _branch_id);

            if (branch != null)
            {

                branch.BranchName = branchDTO.BranchName;
                branch.BranchAddress = branchDTO.BranchAddress;
                branch.BranchAssets = branchDTO.BranchAssets;

                _db.SaveChanges(); // Save changes to the database
                _logger.LogInformation("Successfully updated" +
                        " a branch from DB!!!");
                return true;
            
            }
            else
            {
                // Branch not found
                return false;
            }
        }

        public bool DeleteBranchFromDB(int id)
        {
            var branch = _db.Branch.FirstOrDefault(a => a.BranchID == id);
            if (branch is not null)
            {
                _db.Branch.Remove(branch);
                _db.SaveChanges();
                _logger.LogInformation("Successfully deleted a" +
                    " branch from DB!!!");
                return true;
            }

            return false;
        }

        public IEnumerable<Branch> GetBranches()
        {
            var branchs = _db.Branch;

            return branchs;
        }
    }
}
