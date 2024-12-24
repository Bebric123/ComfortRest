using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<DatabaseContext>();
builder.Services.AddControllers();

/*builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();*/

var app = builder.Build();
app.UseDefaultFiles();
app.UseStaticFiles();

/*if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ComfortRest API v1");
        c.RoutePrefix = string.Empty;
    });
}*/

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
