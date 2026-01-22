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
// ---------------- PORT BINDING (RENDER FIX) ----------------
// Render injects PORT env var — MUST bind to it
//
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

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
string jwtKey = jwtSettings.GetValue<string>("Key")!;
string jwtIssuer = jwtSettings.GetValue<string>("Issuer")!;
string jwtAudience = jwtSettings.GetValue<string>("Audience")!;

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
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
        options.JsonSerializerOptions.WriteIndented = true;
        options.JsonSerializerOptions.MaxDepth = 64;
    });

//
// ---------------- AUTHENTICATION ----------------

// Cookie auth (MVC)
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.LoginPath = "/Auth/Login";
    options.AccessDeniedPath = "/Auth/AccessDenied";
})
// JWT auth (API)
.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtKey))
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

//
// ---------------- SEED ADMIN ----------------
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
            password: "StrongPassword123!", // CHANGE IN PROD
            firstName: "Super",
            lastName: "Admin",
            dob: DateTime.UtcNow.AddYears(-30),
            gender: "Other",
            isAdmin: true
        );

        Console.WriteLine("Admin created.");
    }
}

//
// ---------------- MIDDLEWARE ----------------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// ❌ DO NOT redirect HTTPS in Render / Docker
// app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseRouting();

// Authentication BEFORE Authorization
app.UseAuthentication();
app.UseAuthorization();

//
// ---------------- ROUTES ----------------
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

//
// ---------------- HEALTH CHECK (RENDER) ----------------
app.MapGet("/health", () => Results.Ok("Healthy"));

app.Run();
