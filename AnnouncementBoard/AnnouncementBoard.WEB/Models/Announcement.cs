using System.ComponentModel.DataAnnotations;

namespace AnnouncementBoard.WEB.Models
{
    public class Announcement
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Заголовок обов'язковий")]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required(ErrorMessage = "Статус обов'язковий")]
        public string Status { get; set; } = string.Empty;

        [Required(ErrorMessage = "Категорія обов'язкова")]
        public string Category { get; set; } = string.Empty;

        [Required(ErrorMessage = "Підкатегорія обов'язкова")]
        public string SubCategory { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; }
    }
}
