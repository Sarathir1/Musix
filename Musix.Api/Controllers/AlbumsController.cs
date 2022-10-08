using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Musix.Api.Data;
using Musix.Api.Helper;
using Musix.Api.Models;

namespace Musix.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlbumsController : ControllerBase
    {
        private readonly MusixDbContext _dbContext;
        private readonly IBlobHelper _blobHelper;
        private readonly IConfiguration _configuration;
        public AlbumsController(MusixDbContext dbContext
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
            return await Task.FromResult(Ok(_dbContext.Albums.Include(al=> al.Songs).AsAsyncEnumerable()));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAlbumDetails(int albumId)
        {
            var albumDetails = _dbContext.Albums.Where(al => al.Id == albumId)
                .Include(al => al.Songs).AsAsyncEnumerable();
            return await Task.FromResult(Ok(albumDetails));
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromForm] Album album)
        {
            album.ImageUrl = await _blobHelper.FileUpload(_configuration.GetConnectionString("BlobStoreConnection"), _configuration.GetValue<string>("BlobContainerName"), album.Image);
            await _dbContext.Albums.AddAsync(album);
            await _dbContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created);
        }
    }
}
