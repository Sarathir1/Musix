using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Musix.Api.Models
{
    public class Song
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; } = String.Empty;
        public string Language { get; set; } = String.Empty;
        public string Duration { get; set; } = String.Empty;
        [NotMapped]
        public IFormFile? CoverImage { get; set; }
        public string CoverImageUrl { get; set; } = String.Empty;
        public bool IsFeatured { get; set; }
        public string AudioUrl { get; set; } = String.Empty;
        [NotMapped]
        public IFormFile? AudioFile { get; set; }
        public DateTime UploadedDate { get; set; } = DateTime.UtcNow;
        public int ArtistId { get; set; }
        public int? AlbumId { get; set; }

    }
}
