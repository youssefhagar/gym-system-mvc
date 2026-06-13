using GymSystem.BLL.Service.Interfaces;
using GymSystem.BLL.ViewModels.TrainerViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymSystem.PL.Controllers
{
    public class TrainerController : Controller
    {
        private readonly ITrainerService trainerService;

        public TrainerController(ITrainerService trainerService)
        {
            this.trainerService = trainerService;
        }

        public async Task<IActionResult> Index()
        {
            var trainers = await trainerService.GetAllAsync();

            return View(trainers);
        }

        public async Task<IActionResult> Details(int id)
        {
            var trainer = await trainerService.GetByIdAsync(id);

            if (trainer is null)
                return NotFound();

            return View(trainer);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTrainerViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await trainerService.CreateAsync(model);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Error!);
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {

            var trainer = await trainerService.GetTrainerToUpdateAsync(id); // UpdateTrainerViewModel

            if (trainer is null)
                return NotFound();

            return View(trainer);
            
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, UpdateTrainerViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await trainerService.UpdateAsync(id, model);

            if (!result.Success)
                return BadRequest(result.Error);

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var member = await trainerService.GetByIdAsync(id, ct);
            if (member == null)
            {
                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await trainerService.DeleteAsync(id);

            if (!result.Success)
                TempData["ErrorMessage"] = result.Error;
            else
                TempData["SuccessMessage"] = "Trainer deleted successfully";

            return RedirectToAction(nameof(Index));
        }
    }
}

