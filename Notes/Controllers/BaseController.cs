using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Notes.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        protected string? GetUserId()
        {
            string? id = string.Empty;
            if (User != null)
            {
                id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            }
            return id;
        }

        protected string? GetEmail() 
        {
            string email = string.Empty;
            if (User != null)
            {
                email = User.FindFirstValue(ClaimTypes.Email);
            }
            return email;
        }
    }
}
