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

        [HttpGet]
        public async Task<IActionResult> Edit(int Id,CancellationToken ct)
        { 
            var session = await sessionService.GetUpdateSessionsAsync(Id,ct);
            if(session.Success)
            {
                await PopulateDropDown();
                return View(session.Value);
            }

            TempData["ErrorMessage"] = session.Error;
            return RedirectToAction(nameof(Index));

        }


        [HttpPost]
        public async Task<IActionResult> Edit(int id, UpdateSessionViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropDown();
                return View(model);
            }

            var result = await sessionService.UpdateSessionAsync(id, model, ct);
            if (result.Success)
            {
                TempData["SuccessMessage"] = "Session Updated Successfully";
                return RedirectToAction(nameof(Index));
            }


            TempData["ErrorMessage"] = result.Error;
            await PopulateDropDown();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id ,CancellationToken ct)
        {
            var session = await sessionService.GetSessionByIdAsync(id,ct);
            if(session is null)
            {
                TempData["ErrorMessage"] = "Session Not Found";
                return RedirectToAction(nameof(Index));
            }
            
            return View(session);
        }


        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id ,CancellationToken ct)
        {
            var session = await sessionService.DeleteSessionAsync(id,ct);

            TempData[session.Success ? "SuccessMessage" : "ErrorMessage"] =
                session.Success ? "Session Deleted Successfully" : session.Error;
            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateDropDown()
        {
            ViewBag.Trainers = new SelectList(await sessionService.GetTrainerForropDownAsync(), "Id", "Name");
            ViewBag.Categories = new SelectList(await sessionService.GetCategoryForropDownAsync(), "Id", "CategoryName"); 
        }


    }
}
