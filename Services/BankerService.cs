using Microsoft.EntityFrameworkCore;
using Web_App_BMS.Data;
using Web_App_BMS.DTOs;
using Web_App_BMS.Model;

namespace Web_App_BMS.Services
{
    public class BankerService : IBankerService
    {
        readonly BMS_DBContext _db;
        readonly ILogger<AccountService> _logger;
        readonly ICustomerService _customerService;
        public BankerService(BMS_DBContext _db,
            ILogger<AccountService> _logger, ICustomerService _customerService)
        {
            this._db = _db;
            this._logger = _logger;
            this._customerService = _customerService;
        }
        private static int _banker_id = 0;
        public bool AddBankerToDB(BankerDTO bankerDTO)
        {
            if (bankerDTO != null)
            {
                if (CheckIfAlreadyExists(bankerDTO))
                {
                    Banker _banker = new Banker();
                    if (BindToBankerFromView(bankerDTO, _banker))
                    {
                        _db.Banker.Add(_banker);
                        _db.SaveChanges();
                        _logger.LogInformation("Successfully added" +
                            " a banker to DB!!!");
                        return true;
                    }

                    bankerDTO.NotFoundPropertyMessage = "There is no data" +
                        " corresponding to the input one!";

                    return false;

                }

                bankerDTO.BankerAlreadyExists = "Can't add to database" +
                    " because there is already a banker with this credentials!";
                return false;

            }

            return false;
        }

        public BankerDTO? GetBankerDTOById(int banker_id)
        {
            _banker_id = banker_id;
            BankerDTO bankerDTO = new BankerDTO();
            var banker = _db.Banker
                .Include(b => b.Branch)
                .FirstOrDefault(b => b.BankerID == _banker_id);

            bankerDTO.BankerName = banker.BankerName;
            bankerDTO.BranchName = banker.Branch.BranchName;
            bankerDTO.BranchAddress = banker.Branch.BranchAddress;
            bankerDTO.BranchAssets = banker.Branch.BranchAssets;

            if (bankerDTO is null)
            {
                _logger.LogCritical("Banker is null !");
                return null;
            }

            return bankerDTO;
        }

        
        public bool CheckIfAlreadyExists(BankerDTO bankerDTO)
        {
            var banker = _db.Banker.FirstOrDefault(b =>
            b.Branch.BranchName == bankerDTO.BranchName
            && b.Branch.BranchAddress == bankerDTO.BranchAddress &&
            b.Branch.BranchAssets == bankerDTO.BranchAssets);

            if (banker == null)
            {
                return true;
            }

            return false;
        }

        public bool BindToBankerFromView(BankerDTO bankerDTO, Banker _banker)
        {
            //Values of the BankerDTO can't be null because of the validation
            //which won't allow them to pass to the services

            if (_customerService.GetBranchID(bankerDTO.BranchName,
                bankerDTO.BranchAddress, bankerDTO.BranchAssets) != -1)
            {
                _banker.BankerName = bankerDTO.BankerName;
                _banker.BranchID = _customerService.GetBranchID(
                        bankerDTO.BranchName, bankerDTO.BranchAddress,
                        bankerDTO.BranchAssets);
                return true;
            }

            return false;
        }

        public bool EditBankerFromDB(BankerDTO bankerDTO)
        {
            var banker = _db.Banker.Include(b => b.Branch)
                .FirstOrDefault(b => b.BankerID == _banker_id);

            if (banker != null)
            {

                banker.BankerName = bankerDTO.BankerName;
                banker.Branch.BranchName = bankerDTO.BranchName;
                banker.Branch.BranchAddress = bankerDTO.BranchAddress;
                banker.Branch.BranchAssets = bankerDTO.BranchAssets;

                int branchId = _customerService.
                    GetBranchID(bankerDTO.BranchName,
                    bankerDTO.BranchAddress, bankerDTO.BranchAssets);

                if (branchId != -1)
                {
                    banker.BranchID = branchId;
                    _db.SaveChanges(); // Save changes to the database
                    _logger.LogInformation("Successfully updated" +
                        " a banker from DB!!!");
                    return true;
                }
                else
                {
                    bankerDTO.NotFoundPropertyMessage = "There is no branch" +
                        " with this credentials.";
                    return false;
                }
            }
            else
            {
                // Banker not found
                return false;
            }
        }

        public bool DeleteBankerFromDB(int id)
        {
            var banker = _db.Banker.FirstOrDefault(b => b.BankerID == id);
            if (banker is not null)
            {
                _db.Banker.Remove(banker);
                _db.SaveChanges();
                _logger.LogInformation("Successfully deleted a" +
                    " banker from DB!!!");
                return true;
            }

            return false;
        }

        public IEnumerable<Banker> GetBankers()
        {
            var bankers = _db.Banker.Include(b => b.Branch);

            return bankers;
        }
    }
}
