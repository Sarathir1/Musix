using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Musix.Api.Models;

namespace Musix.Api.Data
{
    public class MusixDbContext:DbContext
    {
        public MusixDbContext(DbContextOptions<MusixDbContext> options):base(options)
        {

        }
        public DbSet<Song> Songs { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Artist> Artists { get; set; }
    }
}
