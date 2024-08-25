

using System.Text;
using DotnetAPI.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//Allows to runswagger UI to explore API
builder.Services.AddSwaggerGen();

builder.Services.AddCors((options) =>
    {
        options.AddPolicy("DevCors", (corsBuilder) =>
            {
                corsBuilder.WithOrigins("http://localhost:4200", "https://localhost:3000", "http://localhost:8000")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
            });

        options.AddPolicy("ProdCors", (corsBuilder) =>
            {
                corsBuilder.WithOrigins("https://myProductionSite.com")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
            });

    });

builder.Services.AddScoped<IUserRepository, UserRepository>();



string? tokenKeyString = builder.Configuration.GetSection("AppSettings:TokenKey").Value;
SymmetricSecurityKey tokenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKeyString != null ? tokenKeyString : throw new Exception("Token key not found")));


//Take token key and create token validation parameters. Decides how to handle token when its passed in.
TokenValidationParameters tokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuerSigningKey = false,
    IssuerSigningKey = tokenKey, //key used for validation
    ValidateIssuer = false,
    ValidateAudience = false
};


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = tokenValidationParameters;
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
// IsDevelopment is local machine.
if (app.Environment.IsDevelopment())
{
    //define cors policy
    app.UseCors("DevCors");
    app.UseSwagger();
    app.UseSwaggerUI(); // we can look at swagger UI page
}
else
{

    app.UseCors("ProdCors");
    app.UseHttpsRedirection();
}

app.UseAuthentication(); //this NEEDS to come before UseAuthorization

app.UseAuthorization();

app.MapControllers();

// app.MapGet("/weatherforecast", () =>
// {

// })
// .WithName("GetWeatherForecast")
// .WithOpenApi(); //will be shown in swagger UI

app.Run();


