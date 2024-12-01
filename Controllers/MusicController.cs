using Microsoft.AspNetCore.Mvc;
using OpenSoundMusicService.Contracts;

namespace OpenSoundMusicService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MusicController : ControllerBase
    {
        private readonly IMusicService _musicService;

        public MusicController(IMusicService musicService)
        {
            _musicService = musicService ?? throw new ArgumentNullException(nameof(musicService));
        }

        [HttpPost("analyze")]
        public async Task<IActionResult> AnalyzeAudio(IFormFile audioFile)
        {
            if (audioFile == null || audioFile.Length == 0)
            {
                return BadRequest("File not provided");
            }

            try
            {
                var result = await _musicService.AnalyzeAudio(audioFile);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the file.");
            }
        }

        // Other endpoints like upload, get, etc.
    }
}
