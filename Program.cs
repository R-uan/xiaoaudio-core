using AudioArchive.Extensions;
using AudioArchive.Shared;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddApplicationServices();
builder.Services.AddApplicationMiddlewares();
builder.Services.AddAuthentication(builder.Configuration);

builder.Services.AddRedis(builder.Configuration);
builder.Services.AddDatabase(builder.Configuration);

builder.Services.AddOpenApi();
builder.Services.AddCorsPolicy();
builder.Services.AddProblemDetails();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

var staticFilesPath = builder.Configuration.GetValue<string>("StaticFiles")
  ?? throw new Exception("No static files path found.");

app.UseAuthentication();
app.UseAuthorization();

app.MigrateDatabase();
await app.CreateAdminAccountAsync();
app.UseMiddleware<CachingMiddleware>();

app.UseAudioStaticFiles(staticFilesPath);
app.UseExceptionHandler();

app.UseCors("AllowAll");

app.UseHttpsRedirection();
app.MapControllers();

app.Run();