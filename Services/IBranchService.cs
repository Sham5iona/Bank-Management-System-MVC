using Web_App_BMS.DTOs;
using Web_App_BMS.Model;

namespace Web_App_BMS.Services
{
    public interface IBranchService
    {
        public bool AddBranchToDB(BranchDTO branchDTO);
        public BranchDTO? GetBranchDTOById(int id);
        public bool CheckIfAlreadyExists(BranchDTO branchDTO);
        public bool BindToBranchFromView(BranchDTO branchDTO,
                                          Branch branch);
        public bool EditBranchFromDB(BranchDTO branchDTO);
        public bool DeleteBranchFromDB(int id);
        public IEnumerable<Branch> GetBranches();
    }
}
