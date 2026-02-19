using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartKazanlak.Core.Contract;
using System.Security.Claims;

namespace SmartKazanlak.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class EventRequestsController : Controller
    {
        private readonly IEventRequestService service;

        public EventRequestsController(IEventRequestService service)
        {
            this.service = service;
        }

        public async Task<IActionResult> Pending()
        {
            var data = await service.GetPendingAsync();
            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(Guid id, string? note)
        {
            var adminId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(adminId)) return Forbid();

            await service.ApproveAsync(id, adminId, note);
            return RedirectToAction(nameof(Pending));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(Guid id, string note)
        {
            var adminId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(adminId)) return Forbid();

            await service.RejectAsync(id, adminId, note);
            return RedirectToAction(nameof(Pending));
        }

        [HttpGet]
        public async Task<IActionResult> Approved()
        {
            var data = await service.GetApprovedAsync();
            return View(data);
        }

        [HttpGet]
        public async Task<IActionResult> Rejected()
        {
            var data = await service.GetRejectedAsync();
            return View(data);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var model = await service.GetDetailsAsync(id);
            if (model is null)
                return NotFound();

            return View(model);
        }

    }
}
