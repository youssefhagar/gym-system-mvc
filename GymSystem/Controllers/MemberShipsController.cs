using GymSystem.BLL.Common;
using GymSystem.BLL.Service.Classes;
using GymSystem.BLL.Service.Interfaces;
using GymSystem.BLL.ViewModels.MembershipViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace GymSystem.PL.Controllers
{
    public class MemberShipsController : Controller
    {
        private readonly IMembershipService _membershipService;

        public MemberShipsController(IMembershipService membershipService)
        {
            _membershipService = membershipService;
        }

        
        
        public async Task<IActionResult> Index()
        {
            var memberships = await _membershipService.GetAllMembershipsAsync();
            if (memberships is null)
            {
                return BadRequest();
            }

            return View(memberships);
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await PopulateDropDown();
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateMembershipViewModel model,CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return View(nameof(Create), model);

            var result = await _membershipService.CreateMembershipAsync(model, ct);

            if (result.Success)
            {
                TempData["SuccessMessage"] = "Membership created successfully.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = result.Error;
            }
            await PopulateDropDown();
            return View(model);

        }

        
        public async Task<IActionResult> Cancel([FromRoute]int memberid, CancellationToken ct)
        {
            var result = await _membershipService.DeleteAsync(memberid, ct);
            if (!result.Success)
                TempData["ErrorMessage"] = result.Error;
            else
                TempData["SuccessMessage"] = "Plan deleted successfully";

            return RedirectToAction(nameof(Index));
        }


        private async Task PopulateDropDown()
        {
            ViewBag.Members = new SelectList(await _membershipService.GetMemberForDropDownAsync(), "MemberId", "MemberName");
            ViewBag.Plans = new SelectList(await _membershipService.GetPlanForDropDownAsync(), "PlanId", "PlanName");
        }
    }
}
