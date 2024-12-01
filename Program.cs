using OpenSoundMusicService.Contracts;
using OpenSoundMusicService.Models.Embeddings;
using OpenSoundMusicService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient<BPMAnalyzer>();
builder.Services.AddHttpClient<MusicClassifier>();

builder.Services.AddSingleton<EmbeddingConfigFactory>();

builder.Services.AddTransient<IBPMAnalyzer>(provider =>
    new BPMAnalyzer(
        provider.GetRequiredService<IHttpClientFactory>().CreateClient(nameof(BPMAnalyzer)),
        provider.GetRequiredService<IConfiguration>(),
        provider.GetRequiredService<EmbeddingConfigFactory>().CreateBPMConfig()
    ));

builder.Services.AddTransient<IMusicClassifier>(provider =>
    new MusicClassifier(
        provider.GetRequiredService<IHttpClientFactory>().CreateClient(nameof(MusicClassifier)),
        provider.GetRequiredService<IConfiguration>(),
        provider.GetRequiredService<EmbeddingConfigFactory>().CreateClassifierConfig()
    ));

builder.Services.AddTransient<IMusicService, MusicService>();

// You can also add other services here like authentication, CORS, etc.
builder.Services.AddAuthentication(); // Example: Adding authentication
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors(); // Enable CORS

app.MapControllers();

app.Run();
