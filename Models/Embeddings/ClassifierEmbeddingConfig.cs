namespace OpenSoundMusicService.Models.Embeddings
{
    public class ClassifierEmbeddingConfig : IEmbeddingConfig
    {
        public string EmbeddingId { get; private set; }
        public string EmbeddingVersion { get; private set; }
        public string AlgoType { get; private set; }

        public ClassifierEmbeddingConfig(IConfiguration configuration)
        {
            EmbeddingId = configuration.GetValue<string>("Replicate:Classifier:EmbeddingId") ?? throw new InvalidOperationException("Classifier: EmbeddingId is required.");
            EmbeddingVersion = configuration.GetValue<string>("Replicate:Classifier:EmbeddingVersion") ?? throw new InvalidOperationException("Classifier: EmbeddingId is required.");
            AlgoType = configuration.GetValue<string>("Replicate:Classifier:AlgoType") ?? throw new InvalidOperationException("Classifier: EmbeddingId is required.");
        }
    }
}

