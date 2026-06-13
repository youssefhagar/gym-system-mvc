using GymSystem.BLL.Service.Interfaces;
using GymSystem.BLL.ViewModels.SessionViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await PopulateDropDown();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateSessionViewModel model,CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropDown();
                return View(model);
            }
                
            var result = await sessionService.CreateSessionAsync(model,ct);

            if(result.Success)
            {
                TempData["SuccessMessage"] = "Session Created Successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = result.Error;
            }
            await PopulateDropDown();
            return View(model);

        }

        [HttpGet]
        public async Task<IActionResult> Details(int id,CancellationToken ct)
        {
            var session = await sessionService.GetSessionByIdAsync(id,ct);
            if(session == null)
            {
                TempData["ErrorMessage"] = "Session Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(session.Value);  
        }



        private async Task PopulateDropDown()
        {
            ViewBag.Trainers = new SelectList(await sessionService.GetTrainerForropDownAsync(), "Id", "Name");
            ViewBag.Categories = new SelectList(await sessionService.GetCategoryForropDownAsync(), "Id", "CategoryName"); 
        }


    }
}
