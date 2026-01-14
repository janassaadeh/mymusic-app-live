using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace mymusic_app.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                // Check if user is admin
                var isAdminClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                if (!string.IsNullOrEmpty(isAdminClaim) && isAdminClaim == "Admin")
                {
                    return RedirectToAction("Index", "Admin");
                }

                // Non-admin user -> Account Dashboard
                return RedirectToAction("Dashboard", "Account");
            }

            // Not logged in -> redirect to login page
            return RedirectToAction("Login", "Auth");
        }
    }
}
