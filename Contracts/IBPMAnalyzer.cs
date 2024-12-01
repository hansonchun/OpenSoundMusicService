using OpenSoundMusicService.Models;

namespace OpenSoundMusicService.Contracts
{
    public interface IBPMAnalyzer
    {
        Task<BPMResult> AnalyzeBPM(string audioFileUrl);
    }
}
