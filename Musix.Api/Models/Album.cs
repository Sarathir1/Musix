using System.ComponentModel.DataAnnotations.Schema;

namespace Musix.Api.Models
{
    public class Album
    { 
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public int ArtistId { get; set; }
        public string ImageUrl { get; set; } = String.Empty;
        [NotMapped]
        public IFormFile? Image { get; set; } = null;
        public ICollection<Song> Songs { get; set; } = new List<Song>();
    }
}
