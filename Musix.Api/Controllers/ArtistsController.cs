using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Musix.Api.Data;
using Musix.Api.Helper;
using Musix.Api.Models;

namespace Musix.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistsController : ControllerBase
    {
        private readonly MusixDbContext _dbContext;
        private readonly IBlobHelper _blobHelper;
        private readonly IConfiguration _configuration;
        public ArtistsController(MusixDbContext dbContext
            , IBlobHelper blobHelper
            , IConfiguration configuration)
        {
            _dbContext = dbContext;
            _blobHelper = blobHelper;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
              return await Task.FromResult(Ok(_dbContext.Artists.Include(art=>art.Songs).AsAsyncEnumerable()));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] Artist artist)
        {
            artist.ImageUrl = await _blobHelper.FileUpload(_configuration.GetConnectionString("BlobStoreConnection"), _configuration.GetValue<string>("BlobContainerName"), artist.Image);
            await _dbContext.Artists.AddAsync(artist);
            await _dbContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created);
        }
    }
}
