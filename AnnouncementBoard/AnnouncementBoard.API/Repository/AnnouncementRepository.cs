using AnnouncementBoard.API.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace AnnouncementBoard.API.Repository
{
    public class AnnouncementRepository : IAnnouncementRepository
    {
        private readonly string _connectionString;

        public AnnouncementRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        private IDbConnection Connection => new SqlConnection(_connectionString);

        public async Task CreateAsync(Announcement announcement)
        {
            using var connection = Connection;
            var parameters = new DynamicParameters();
            parameters.Add("@Title", announcement.Title);
            parameters.Add("@Description", announcement.Description);
            parameters.Add("@Status", announcement.Status);
            parameters.Add("@Category", announcement.Category);
            parameters.Add("@SubCategory", announcement.SubCategory);

            await connection.ExecuteAsync(
                "sp_InsertAnnouncement",
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = Connection;
            await connection.ExecuteAsync(
                "sp_DeleteAnnouncement",
                new { Id = id },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Announcement>> GetAllAsync()
        {
            using var connection = Connection;
            var result = await connection.QueryAsync<Announcement>(
                "sp_GetAllAnnouncements",
                commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<IEnumerable<Announcement>> GetByCategoryAndSubCategoryAsync(string category, string subCategory)
        {
            using var connection = Connection;
            var parameters = new DynamicParameters();
            parameters.Add("@Category", category);
            parameters.Add("@SubCategory", subCategory);

            return await connection.QueryAsync<Announcement>(
                "sp_GetByCategoryAndSubCategory",
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Announcement>> GetByCategoryAsync(string category)
        {
            using var connection = Connection;
            var parameters = new DynamicParameters();
            parameters.Add("@Category", category);

            return await connection.QueryAsync<Announcement>(
                "sp_GetAnnouncementsByCategory",
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<Announcement?> GetByIdAsync(int id)
        {
            using var connection = Connection;
            var result = await connection.QueryFirstOrDefaultAsync<Announcement>(
                "sp_GetAnnouncementById",
                new { Id = id },
                commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task UpdateAsync(int id, Announcement announcement)
        {
            using var connection = Connection;
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);
            parameters.Add("@Title", announcement.Title);
            parameters.Add("@Description", announcement.Description);
            parameters.Add("@Status", announcement.Status);
            parameters.Add("@Category", announcement.Category);
            parameters.Add("@SubCategory", announcement.SubCategory);

            await connection.ExecuteAsync(
                "sp_UpdateAnnouncement",
                parameters,
                commandType: CommandType.StoredProcedure);
        }
    }
}
