using OpenSoundMusicService.Contracts;
using OpenSoundMusicService.Models;

namespace OpenSoundMusicService.Services
{
    public class MusicService : IMusicService
    {
        private readonly IBPMAnalyzer _bpmAnalyzer;
        private readonly IMusicClassifier _classifier;

        public MusicService(IBPMAnalyzer bpmAnalyzer, IMusicClassifier classifier)
        {
            _bpmAnalyzer = bpmAnalyzer ?? throw new ArgumentNullException(nameof(bpmAnalyzer));
            _classifier = classifier ?? throw new ArgumentNullException(nameof(classifier));
        }

        public async Task<MusicAnalysisResult> AnalyzeAudio(IFormFile audioFile)
        {
            try
            {
                if (audioFile == null || audioFile.Length == 0)
                    throw new ArgumentException("An audio file must be provided.", nameof(audioFile));

                // For now, we're using a static URL. Replace with dynamic upload logic when implemented.
                string audioFileUrl = "https://www2.cs.uic.edu/~i101/SoundFiles/StarWars60.wav";

                var bpmTask = _bpmAnalyzer.AnalyzeBPM(audioFileUrl);
                var classificationTask = _classifier.ClassifyMusic(audioFileUrl);

                await Task.WhenAll(bpmTask, classificationTask);

                return new MusicAnalysisResult
                {
                    BPMResult = await bpmTask,
                    ClassifierResult = await classificationTask
                };
            }
            catch (Exception ex)
            {
                // Log the exception here if you want
                throw; // re-throw to let the controller handle this exception
            }
        }
    }
}
