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

using (var scope = app.Services.CreateScope())
{
    using (var appContext = scope.ServiceProvider.GetRequiredService<MusixDbContext>())
    {
        try
        {
           await appContext.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            //Log errors or do anything you think it's needed
            throw;
        }
    }
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();