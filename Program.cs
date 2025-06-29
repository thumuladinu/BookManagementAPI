var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();
app.UseDefaultFiles();
app.UseStaticFiles();
app.MapControllers();
app.Run();
