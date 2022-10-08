using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Musix.Api.Data;
using Musix.Api.Helper;
using Musix.Api.Models;

namespace Musix.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SongsController : ControllerBase
    {
        private readonly MusixDbContext _dbContext;
        private readonly IBlobHelper _blobHelper;
        private readonly IConfiguration _configuration;

        public SongsController(MusixDbContext dbContext
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
            var result = _dbContext.Songs.Select(x=> new { 
                Id= x.Id,
                Title= x.Title,
                CoverImageUrl=x.CoverImageUrl,
                Description = x.Description ,
                Language = x.Language,
                Duration = x.Duration,
                AudioUrl = x.AudioUrl,
                IsFeatured=x.IsFeatured,
                UploadedDate= x.UploadedDate
            }).AsAsyncEnumerable();

            return await Task.FromResult(Ok(result));
        }
        [HttpGet("{Id}")]
        public async Task<IActionResult> Get(int Id)
        {
            var result = await _dbContext.Songs.FindAsync(Id);
            if (result == null)
                return NotFound($"No song available with id :{Id}");

            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] Song song)
        {
            var coverUrlTask = _blobHelper.FileUpload(_configuration.GetConnectionString("BlobStoreConnection"), _configuration.GetValue<string>("BlobContainerName"), song.CoverImage);
            var audioTrackUrlTask = _blobHelper.FileUpload(_configuration.GetConnectionString("BlobStoreConnection"), _configuration.GetValue<string>("BlobContainerName"), song.AudioFile);
            await Task.WhenAll(coverUrlTask, audioTrackUrlTask);
            song.CoverImageUrl = coverUrlTask.Result;
            song.AudioUrl = audioTrackUrlTask.Result;
            song.UploadedDate = DateTime.UtcNow;
            await _dbContext.Songs.AddAsync(song);
            await _dbContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> Put(int Id, [FromBody] Song song)
        {
            var result = await _dbContext.Songs.FindAsync(Id);

            if (result == null)
                return NotFound($"No song available with id : {Id}");
            else
            {
                result.Duration = song.Duration;
                result.CoverImageUrl = song.CoverImageUrl;
                result.Description = song.Description;
                result.Title = song.Title;
                result.Language = song.Language;
                await _dbContext.SaveChangesAsync();
                return StatusCode(StatusCodes.Status200OK);
            }

        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(int Id)
        {
            var result = await _dbContext.Songs.FindAsync(Id);
            if (result == null)
                return NotFound($"No song available with id :{Id}");

            _dbContext.Songs.Remove(result);
            await _dbContext.SaveChangesAsync();

            return Ok($"Song with title : {result.Title} deleted");
        }

    }
}
