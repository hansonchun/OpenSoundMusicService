namespace OpenSoundMusicService.Models
{
    public class EmbeddingResult
    {
        public string Id { get; set; }
        public string Model { get; set; }
        public string Version { get; set; }
        public Dictionary<string, string> Input { get; set; }
        public string Logs { get; set; }
        public string Output { get; set; }
        public bool DataRemoved { get; set; }
        public string Error { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public Urls Urls { get; set; }
    }

    public class Urls
    {
        public string Cancel { get; set; }
        public string Get { get; set; }
        public string Stream { get; set; }
    }
}
