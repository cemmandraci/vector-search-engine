using MediatR;
using VectorSearch.API.Middleware;
using VectorSearch.Application.Commands.IndexDocument;
using VectorSearch.Domain.Interfaces;
using VectorSearch.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "Vector Search API",
        Version = "v1",
        Description = "Semantic search powered by Qdrant and Gemini Embeddings"
    });
});

builder.Services.AddMediatR(typeof(IndexDocumentCommand).Assembly);

builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var vectorRepository = scope.ServiceProvider
        .GetRequiredService<IVectorRepository>();
    await vectorRepository.EnsureCollectionExistsAsync();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
