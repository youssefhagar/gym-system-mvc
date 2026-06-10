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

            if (!result)
            {
                ModelState.AddModelError("", "Trainer Already Exists");
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var trainer = await trainerService.GetByIdAsync(id);

            if (trainer is null)
                return NotFound();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, UpdateTrainerViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await trainerService.UpdateAsync(id, model);

            if (!result)
                return BadRequest();

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
            await trainerService.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}

