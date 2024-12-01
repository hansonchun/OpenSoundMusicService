using OpenSoundMusicService.Contracts;
using OpenSoundMusicService.Models.Embeddings;
using OpenSoundMusicService.Models;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Text;

namespace OpenSoundMusicService.Services
{
    public class BPMAnalyzer : IBPMAnalyzer
    {
        private readonly HttpClient _httpClient;
        private readonly string _replicateApiToken;
        private readonly string _replicateBaseUrl;
        private readonly IEmbeddingConfig _EmbeddingConfig;

        public BPMAnalyzer(HttpClient httpClient, IConfiguration configuration, IEmbeddingConfig EmbeddingConfig)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _EmbeddingConfig = EmbeddingConfig ?? throw new ArgumentNullException(nameof(EmbeddingConfig));
            _replicateApiToken = configuration["Replicate:ApiToken"] ?? throw new InvalidOperationException("Replicate API Token is required.");
            _replicateBaseUrl = configuration["Replicate:BaseUrl"] ?? throw new InvalidOperationException("Replicate Base URL is required.");
        }

        public async Task<BPMResult> AnalyzeBPM(string audioFileUrl)
        {
            var requestBody = new
            {
                version = _EmbeddingConfig.EmbeddingVersion,
                input = new
                {
                    audio = audioFileUrl,
                    algo_type = _EmbeddingConfig.AlgoType
                }
            };

            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _replicateApiToken);
            _httpClient.DefaultRequestHeaders.Add("Prefer", "wait");

            var response = await _httpClient.PostAsync(_replicateBaseUrl + "predictions", content);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"API call failed with status code: {response.StatusCode}");
            }

            var predictionResult = await JsonSerializer.DeserializeAsync<EmbeddingResult>(
                await response.Content.ReadAsStreamAsync(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            if (predictionResult == null || predictionResult.Output == null)
            {
                throw new InvalidOperationException("Failed to retrieve BPM analysis result from the API.");
            }

            var outputResponse = await _httpClient.GetAsync(predictionResult.Output);

            if (!outputResponse.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Failed to fetch BPM output with status code: {outputResponse.StatusCode}");
            }

            var resultString = await outputResponse.Content.ReadAsStringAsync();
            var bpmValue = ParseBPMFromResponse(resultString);
            return new BPMResult { BPM = bpmValue };
        }

        private double ParseBPMFromResponse(string resultString)
        {
            const string pattern = @"Estimated BPM:\s*(\d+(?:\.\d+)?)";
            var match = Regex.Match(resultString, pattern, RegexOptions.IgnoreCase);

            if (match.Success && double.TryParse(match.Groups[1].Value, out var bpm))
            {
                return bpm;
            }
            throw new FormatException("The response does not contain a valid BPM value.");
        }
    }
}
