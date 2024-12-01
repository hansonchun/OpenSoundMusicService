using OpenSoundMusicService.Contracts;
using OpenSoundMusicService.Models;
using OpenSoundMusicService.Models.Embeddings;
using System.Text.Json;

namespace OpenSoundMusicService.Services
{
    public class MusicClassifier : IMusicClassifier
    {
        private readonly HttpClient _httpClient;
        private readonly string _replicateApiToken;
        private readonly string _replicateBaseUrl;
        private readonly IEmbeddingConfig _EmbeddingConfig;

        public MusicClassifier(HttpClient httpClient, IConfiguration configuration, IEmbeddingConfig EmbeddingConfig)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _EmbeddingConfig = EmbeddingConfig ?? throw new ArgumentNullException(nameof(EmbeddingConfig));
            _replicateApiToken = configuration["Replicate:ApiToken"] ?? throw new InvalidOperationException("API Token is required.");
            _replicateBaseUrl = configuration["Replicate:BaseUrl"] ?? throw new InvalidOperationException("API Base URL is required.");
        }

        public async Task<ClassifierResult> ClassifyMusic(string audioFileUrl)
        {
            var requestBody = new
            {
                version = _EmbeddingConfig.EmbeddingVersion,
                input = new
                {
                    url = audioFileUrl,
                    algo_type = _EmbeddingConfig.AlgoType
                }
            };

            var content = new StringContent(JsonSerializer.Serialize(requestBody), System.Text.Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _replicateApiToken);
            _httpClient.DefaultRequestHeaders.Add("Prefer", "wait");

            var response = await _httpClient.PostAsync(_replicateBaseUrl + "predictions", content);

            if (response.IsSuccessStatusCode)
            {
                var resultString = await response.Content.ReadAsStringAsync();
                var classifierResult = JsonSerializer.Deserialize<ClassifierResult>(resultString);
                if (classifierResult == null)
                    throw new Exception("Failed to deserialize Music Classification result.");

                return classifierResult;
            }
            else
            {
                throw new Exception($"API call failed with status code: {response.StatusCode}");
            }
        }

        private async Task<string> UploadFileAndGetUrl(IFormFile file)
        {
            // Similar to BPMAnalyzer, this should return a URL after uploading the file
            return "some-storage-service-url/" + file.FileName;
        }
    }
}
