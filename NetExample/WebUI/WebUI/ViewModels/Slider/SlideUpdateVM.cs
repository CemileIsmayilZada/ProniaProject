using System.ComponentModel.DataAnnotations;

namespace WebUI.ViewModels.Slider
{
    public class SlideUpdateVM
    {
        public int Id { get; set; }
        [Required, MaxLength(150)]
        public string? Title { get; set; }
        [MaxLength(100)]
        public string? Offer { get; set; }
        public IFormFile? Photo { get; set; }
        [MaxLength(255)]
        public string? Description { get; set; }

        public string? ImagePath { get; set; }
    }
}
