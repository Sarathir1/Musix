using System.ComponentModel.DataAnnotations.Schema;

namespace Musix.Api.Models
{
    public class Artist
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public string Gender { get; set; } = String.Empty;
        public string ImageUrl { get; set; } = String.Empty;
        [NotMapped]
        public IFormFile? Image { get; set; }    
        public ICollection<Album> Albums { get; set; }= new List<Album>();
        public ICollection<Song> Songs { get; set; } = new List<Song>();
    }
}
