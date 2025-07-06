using Microsoft.AspNetCore.Mvc;
using System.Data;
using Dapper;
using System.Data.SqlClient;
using AnnouncementBoard.API.Models;
using AnnouncementBoard.API.Repository;

namespace AnnouncementBoard.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnnouncementsController : Controller
    {
        private readonly IAnnouncementRepository _repository;
        public AnnouncementsController(IAnnouncementRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? category, [FromQuery] string? subCategory)
        {
            IEnumerable<Announcement> announcements;

            if (!string.IsNullOrEmpty(category) && !string.IsNullOrEmpty(subCategory))
            {
                announcements = await _repository.GetByCategoryAndSubCategoryAsync(category, subCategory);
            }
            else if (!string.IsNullOrEmpty(category))
            {
                announcements = await _repository.GetByCategoryAsync(category);
            }
            else
            {
                announcements = await _repository.GetAllAsync();
            }

            return Ok(announcements);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var announcement = await _repository.GetByIdAsync(id);
            if (announcement == null)
                return NotFound();

            return Ok(announcement);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Announcement announcement)
        {
            await _repository.CreateAsync(announcement);
            return CreatedAtAction(nameof(GetAll), null);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Announcement announcement)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            await _repository.UpdateAsync(id, announcement);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}
