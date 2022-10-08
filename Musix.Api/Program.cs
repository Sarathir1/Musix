using Microsoft.EntityFrameworkCore;
using Musix.Api.Data;
using Musix.Api.Helper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IBlobHelper, AzureBlobHelper>();
builder.Services.AddDbContext<MusixDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ApiExploreDbConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();    
}

app.MigrateDatabase();

app.MapGet("/healthcheck", async req => {
    await req.Response.WriteAsync("healthy");
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


public static class Bootup
{
    public static WebApplication MigrateDatabase(this WebApplication webApp)
    {
        using (var scope = webApp.Services.CreateScope())
        {
            using (var appContext = scope.ServiceProvider.GetRequiredService<MusixDbContext>())
            {
                try
                {
                    appContext.Database.MigrateAsync();
                }
                catch (Exception ex)
                {
                    //Log errors or do anything you think it's needed
                    throw;
                }
            }
        }
        return webApp;
    }
}