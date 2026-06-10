using GymSystem.BLL.Service.Interfaces;
using GymSystem.BLL.ViewModels.MemberViewModels;
using GymSystem.DAL.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GymSystem.PL.Controllers
{
    public class MembersController : Controller
    {
        private readonly IMemberService memberServic;

        public MembersController(IMemberService memberServic)
        {
            this.memberServic = memberServic;
        }

        public async Task<IActionResult> Index()
        {
            var members = await memberServic.GetAllMemberAsync();
            return View(members);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateMember(CreateMemberViewModel model,CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return View(nameof(Create) ,model);

            // Save
            var res = await memberServic.CreateMemberAsync(model, ct);

            if (res)
                TempData["SuccessMessage"] = "Memder Created Successfully";
            else
                TempData["ErrorMessage"] = "Failed To Create Member" +
                    "";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> MemberDetails(int id,CancellationToken ct)
        {
            var member = await memberServic.GetMemberDetailsByIdAsync(id,ct);
            if (member == null)
            {
                TempData["ErrorMessage"] = "Member not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(member);

        }

        public async Task<IActionResult> HealthRecordDetails(int id, CancellationToken ct)
        {
            var item = await memberServic.GetHealthRecordDetailsByIdAsync(id,ct);
            if (item == null)
            {
                TempData["ErrorMessage"] = "Health Record not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(item);
        }

        [HttpGet]
        public async Task<IActionResult> EditMember(int id, CancellationToken ct)
        {
            var member = await memberServic.GetMemberToUpdateAsync(id,ct);
            if (member == null)
            {
                TempData["ErrorMessage"] = "Member  not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        [HttpPost]
        public async Task<IActionResult> EditMember(int id,MemberToUpdateViewModel member, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(member);
            var res = await memberServic.UpdateMemberDetailsAsync(id,member,ct);
            if (res)
                TempData["SuccessMessage"] = "Member updated successfully";
            else
                TempData["ErrorMessage"] = "Member update Failed";

            return RedirectToAction(nameof(Index));

        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id,CancellationToken ct)
        {
            var member = await memberServic.GetMemberDetailsByIdAsync(id,ct);
            if (member == null)
            {
                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed([FromRoute]int id ,CancellationToken ct)
        {
            var res = await memberServic.DeleteMemberAsync(id,ct);
            if (res)
            {
                TempData["SuccessMessage"] = "Member Deleted successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Member Deleted Failed";
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
