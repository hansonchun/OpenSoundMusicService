using OpenSoundMusicService.Models;

namespace OpenSoundMusicService.Contracts
{
    public interface IMusicService
    {
        Task<MusicAnalysisResult> AnalyzeAudio(IFormFile audioFile);
    }
}