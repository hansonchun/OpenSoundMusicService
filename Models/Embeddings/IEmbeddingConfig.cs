namespace OpenSoundMusicService.Models.Embeddings
{
    public interface IEmbeddingConfig
    {
        string EmbeddingId { get; }
        string EmbeddingVersion { get; }
        string AlgoType { get; }
    }
}
