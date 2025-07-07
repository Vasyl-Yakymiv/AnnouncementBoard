using AnnouncementBoard.WEB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnnouncementBoard.WEB.Controllers
{
    public class AnnouncementController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AnnouncementController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index(string? category, string? subCategory)
        {
            var query = string.Empty;

            if (!string.IsNullOrEmpty(category) && !string.IsNullOrEmpty(subCategory))
                query = $"?category={category}&subCategory={subCategory}";
            else if (!string.IsNullOrEmpty(category))
                query = $"?category={category}";

            var client = _httpClientFactory.CreateClient("ApiClient");
            var response = await client.GetAsync($"announcements{query}");

            if (!response.IsSuccessStatusCode)
                return View(Enumerable.Empty<Announcement>());

            var announcements = await response.Content.ReadFromJsonAsync<IEnumerable<Announcement>>();
            return View(announcements);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Announcement announcement)
        {
            if (!ModelState.IsValid)
                return View(announcement);

            var client = _httpClientFactory.CreateClient("ApiClient");
            var response = await client.PostAsJsonAsync("announcements", announcement);

            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            ModelState.AddModelError(string.Empty, "Помилка при створенні оголошення");
            return View(announcement);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var client = _httpClientFactory.CreateClient("ApiClient");

            var response = await client.GetAsync($"announcements/{id}");

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                return Content($"Помилка API: {errorContent}");
            }

            var announcement = await response.Content.ReadFromJsonAsync<Announcement>();
            return View(announcement);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Announcement announcement)
        {
            if (id != announcement.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(announcement);

            var client = _httpClientFactory.CreateClient("ApiClient");

            var response = await client.PutAsJsonAsync($"announcements/{id}", announcement);

            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            ModelState.AddModelError(string.Empty, "Помилка при оновленні оголошення");
            return View(announcement);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            var response = await client.GetAsync($"announcements/{id}");

            if (!response.IsSuccessStatusCode)
                return NotFound();

            var announcement = await response.Content.ReadFromJsonAsync<Announcement>();
            return View(announcement);
        }

        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            var response = await client.DeleteAsync($"announcements/{id}");

            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            return Problem("Не вдалося видалити запис.");
        }
    }
}
