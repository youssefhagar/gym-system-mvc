
using GymSystem.BLL.Service.Interfaces;
using GymSystem.BLL.ViewModels.PlanViewModels;
using Microsoft.AspNetCore.Mvc;

public class PlansController : Controller
{
    private readonly IPlanService planService;

    public PlansController(IPlanService planService)
    {
        this.planService = planService;
    }

    public async Task<IActionResult> Index()
    {
        var plans = await planService.GetAllPlansAsync();
        return View(plans);
    }

    public async Task<IActionResult> Details(int id)
    {
        var plan = await planService.GetPlanByIdAsync(id);

        if (plan is null)
            return NotFound();

        return View(plan);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreatePlanViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var result = await planService.CreatePlanAsync(model);

        if (!result)
        {
            ModelState.AddModelError("", "Plan Already Exists");
            return View(model);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var plan = await planService.GetPlanByIdAsync(id);

        if (plan is null)
            return NotFound();

        return View(new UpdatePlanViewModel
        {
            Name = plan.Name,
            Description = plan.Description,
            DurationDays = plan.DurationDays,
            Price = plan.Price
        });
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id,
        UpdatePlanViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var result = await planService.UpdatePlanAsync(id, model);

        if (!result)
            return BadRequest();

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        await planService.DeletePlanAsync(id);

        return RedirectToAction(nameof(Index));
    }
}
