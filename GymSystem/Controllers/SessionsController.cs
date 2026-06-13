using GymSystem.BLL.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymSystem.PL.Controllers
{
    public class SessionsController : Controller
    {
        private readonly ISessionService sessionService;

        public SessionsController(ISessionService sessionService)
        {
            this.sessionService = sessionService;
        }

        public async Task<IActionResult> Index()
        {
            var sessions = await sessionService.GetSessions();

            if(sessions == null || !sessions.Any())
                TempData["ErrorMessage"] = "No Sessions Available yet";
            else
                TempData["SuccessMessage"] = "Sessions Available";

            return View(sessions);
        }
    }
}
