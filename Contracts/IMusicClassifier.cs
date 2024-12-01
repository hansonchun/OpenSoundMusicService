using OpenSoundMusicService.Models;

namespace OpenSoundMusicService.Contracts
{
    public interface IMusicClassifier
    {
        Task<ClassifierResult> ClassifyMusic(string audioFileUrl);
    }
}
