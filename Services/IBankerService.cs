using Web_App_BMS.DTOs;
using Web_App_BMS.Model;

namespace Web_App_BMS.Services
{
    public interface IBankerService
    {
        public bool AddBankerToDB(BankerDTO bankerDTO);
        public BankerDTO? GetBankerDTOById(int id);
        public bool CheckIfAlreadyExists(BankerDTO bankerDTO);
        public bool BindToBankerFromView(BankerDTO bankerDTO,
                                          Banker banker);
        public bool EditBankerFromDB(BankerDTO bankerDTO);
        public bool DeleteBankerFromDB(int id);
        public IEnumerable<Banker> GetBankers();
    }
}
