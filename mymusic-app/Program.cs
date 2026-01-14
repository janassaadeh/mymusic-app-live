using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using mymusic_app.Controllers.Data;
using mymusic_app.Repositories;
using mymusic_app.Services;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

//
// ---------------- DATABASE ----------------
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);


//
// ---------------- REPOSITORIES ----------------
// EF Core
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IArtistRepository, ArtistRepository>();
builder.Services.AddScoped<ISongRepository, SongRepository>();
builder.Services.AddScoped<IPlaylistRepository, PlaylistRepository>();
builder.Services.AddScoped<IAlbumRepository, AlbumRepository>();
builder.Services.AddScoped<IGenreRepository, GenreRepository>();

// ADO.NET
builder.Services.AddScoped<IPlaybackRepository, PlaybackRepository>();

//
// ---------------- SERVICES ----------------
builder.Services.AddScoped<IAlbumService, AlbumService>();
builder.Services.AddScoped<IArtistService, ArtistService>();
builder.Services.AddScoped<IGenreService, GenreService>();
builder.Services.AddScoped<IPlaylistService, PlaylistService>();
builder.Services.AddScoped<ISongService, SongService>();
builder.Services.AddScoped<IUserService, UserService>();

//
// ---------------- AUTH SERVICE ----------------
var jwtSettings = builder.Configuration.GetSection("Jwt");
string jwtKey = jwtSettings.GetValue<string>("Key");
string jwtIssuer = jwtSettings.GetValue<string>("Issuer");
string jwtAudience = jwtSettings.GetValue<string>("Audience");

builder.Services.AddScoped<IAuthService>(sp =>
{
    var userRepo = sp.GetRequiredService<IUserRepository>();
    return new AuthService(userRepo, jwtKey, jwtIssuer, jwtAudience);
});

//
// ---------------- MVC + JSON OPTIONS ----------------
builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        // Handle cycles in object graphs (Artist ↔ Genre)
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
        options.JsonSerializerOptions.WriteIndented = true;
        options.JsonSerializerOptions.MaxDepth = 64; // optional, default is 32
    });

//
// ---------------- AUTHENTICATION ----------------

// Cookie-based auth for web
builder.Services.AddAuthentication(options =>
{
    // Default for web pages (MVC)
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.LoginPath = "/Auth/Login";
    options.AccessDeniedPath = "/Auth/AccessDenied"; // redirect non-admins
})
.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    // JWT Bearer auth for API
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var userRepo = scope.ServiceProvider.GetRequiredService<IUserRepository>();
    var authService = scope.ServiceProvider.GetRequiredService<IAuthService>();

    string adminEmail = "admin@musicapp.com";
    var existingAdmin = await userRepo.GetByEmailAsync(adminEmail);
    if (existingAdmin == null)
    {
        Console.WriteLine("Seeding initial admin...");

        await authService.RegisterAsync(
            email: adminEmail,
            password: "StrongPassword123!", // CHANGE TO SECURE PASSWORD
            firstName: "Super",
            lastName: "Admin",
            dob: DateTime.UtcNow.AddYears(-30),
            gender: "Other",
            isAdmin: true
        );

        Console.WriteLine("Admin created successfully.");
    }
}
//
// ---------------- MIDDLEWARE ----------------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Authentication must come before authorization
app.UseAuthentication();
app.UseAuthorization();

//
// ---------------- ROUTING ----------------
// Default route goes directly to Admin/Index
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
