namespace OpenSoundMusicService.Models.Embeddings
{
    public class EmbeddingConfigFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public EmbeddingConfigFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IEmbeddingConfig CreateBPMConfig()
        {
            return new BPMEmbeddingConfig(_serviceProvider.GetRequiredService<IConfiguration>());
        }

        public IEmbeddingConfig CreateClassifierConfig()
        {
            return new ClassifierEmbeddingConfig(_serviceProvider.GetRequiredService<IConfiguration>());
        }
    }
}
