using mymusic_app.Models;
using System;
using System.Threading.Tasks;

namespace mymusic_app.Services
{
    public interface IAuthService
    {
        // ---------------- REGISTER ----------------
        Task<User> RegisterAsync(
            string email, string password, string firstName, string lastName,
            DateTime dob, string gender, bool isAdmin = false);

        // ---------------- LOGIN ----------------
        Task<User> LoginAsync(string email, string password);

        // ---------------- COOKIE SIGN-IN (WEB) ----------------
        Task SignInAsync(User user, HttpContext httpContext);

        // ---------------- LOGOUT ----------------
        Task SignOutAsync(HttpContext httpContext);

        // ---------------- JWT TOKEN (API / FLUTTER) ----------------
        string GenerateJwtToken(User user, int expireMinutes = 60);
    }
}

