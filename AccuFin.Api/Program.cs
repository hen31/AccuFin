using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AccuFin.Api.Data;
using AccuFin.Api.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
using AccuFin.Api.Areas.Identity.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using System;
using Microsoft.Extensions.DependencyInjection;
using AccuFin.Data;
using AccuFin.Api.Controllers.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("IdentityContextConnection") ?? throw new InvalidOperationException("Connection string 'IdentityContextConnection' not found.");

builder.Services.AddDbContext<IdentityContext>(options =>
    options.UseSqlServer(connectionString));

var connectionStringAccuFin = builder.Configuration.GetConnectionString("AccuFinContextConnection") ?? throw new InvalidOperationException("Connection string 'IdentityContextConnection' not found.");
builder.Services.AddDbContext<AccuFinDatabaseContext>(options =>
    options.UseSqlServer(connectionStringAccuFin));
builder.Services.AddCors(options =>
            options.AddPolicy("AllowAllOrigins", builder1 => builder1.WithOrigins("https://localhost:7266").AllowAnyHeader().AllowAnyMethod().AllowCredentials()));

builder.Services.AddDefaultIdentity<AccuFinUser>(options =>
{
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Tokens.EmailConfirmationTokenProvider = "Email";
    options.User.RequireUniqueEmail = true;
    options.Lockout.AllowedForNewUsers = true;
    options.Lockout.DefaultLockoutTimeSpan = new TimeSpan(0, 5, 0);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.SignIn.RequireConfirmedEmail = true;
})
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<IdentityContext>()
    .AddErrorDescriber<CustomIdentityErrorDescriber>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(options =>
{

    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"])),
        ClockSkew = TimeSpan.Zero
    };
});
builder.Services.AddTransient<IEmailSender, EmailSender>();

var app = builder.Build();
app.MapControllers();
app.UseCors("AllowAllOrigins");
app.UseAuthentication(); ;
app.UseAuthorization(); ;

using (var scope = app.Services.CreateScope())
{
    // Just fix with pull and re-comment when pushing :)
    var databaseContext = scope.ServiceProvider.GetService<IdentityContext>();

    //databaseContext?.Database.EnsureDeleted();
    databaseContext?.Database.EnsureCreated();
    databaseContext?.Database.Migrate();

    /*    var databaseContextImkery = scope.ServiceProvider.GetService<AccuFinDatabaseContext>();
        //databaseContextImkery?.Database.EnsureDeleted();
        databaseContextImkery?.Database.EnsureCreated();
        databaseContextImkery?.Database.Migrate();*/
}

app.Run();


