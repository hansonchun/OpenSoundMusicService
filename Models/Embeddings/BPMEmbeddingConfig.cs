namespace OpenSoundMusicService.Models.Embeddings
{
    public class BPMEmbeddingConfig : IEmbeddingConfig
    {
        public string EmbeddingId { get; private set; }
        public string EmbeddingVersion { get; private set; }
        public string AlgoType { get; private set; }

        public BPMEmbeddingConfig(IConfiguration configuration)
        {
            EmbeddingId = configuration.GetValue<string>("Replicate:BPM:EmbeddingId") ?? throw new InvalidOperationException("BPM: EmbeddingId is required.");
            EmbeddingVersion = configuration.GetValue<string>("Replicate:BPM:EmbeddingVersion") ?? throw new InvalidOperationException("BPM: EmbeddingVersion is required.");
            AlgoType = configuration.GetValue<string>("Replicate:BPM:AlgoType") ?? throw new InvalidOperationException("BPM: AlgoType is required.");
        }
    }
}
