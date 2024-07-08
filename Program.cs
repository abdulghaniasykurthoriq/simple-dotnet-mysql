using MyRestfulApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Tambahkan layanan untuk controller
builder.Services.AddControllers();

// Tambahkan layanan DatabaseService
builder.Services.AddScoped<IDatabaseService, DatabaseService>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();
app.Run();
