using AnnouncementBoard.API.Models;

namespace AnnouncementBoard.API.Repository
{
    public interface IAnnouncementRepository
    {
        Task<IEnumerable<Announcement>> GetAllAsync();
        Task<Announcement?> GetByIdAsync(int id);
        Task<IEnumerable<Announcement>> GetByCategoryAndSubCategoryAsync(string category, string subCategory);
        Task<IEnumerable<Announcement>> GetByCategoryAsync(string category);
        Task CreateAsync(Announcement announcement);
        Task UpdateAsync(int id, Announcement announcement);
        Task DeleteAsync(int id);
    }
}
