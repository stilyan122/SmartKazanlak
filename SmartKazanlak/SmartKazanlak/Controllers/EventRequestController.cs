using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SmartKazanlak.Core.Contract;
using SmartKazanlak.Core.Domain.Entities;
using SmartKazanlak.Core.Models.EventRequest;
using SmartKazanlak.Models.EventRequest;
using System.Security.Claims;

namespace SmartKazanlak.Controllers
{
    [Authorize]
    public class EventRequestsController : Controller
    {
        private readonly IEventRequestService service;
        private readonly UserManager<ApplicationUser> userManager;

        public EventRequestsController(IEventRequestService service, UserManager<ApplicationUser> userManager)
        {
            this.service = service;
            this.userManager = userManager;
        }

        [HttpGet]
        public IActionResult Create() => View(new CreateEventRequestInputModel());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEventRequestInputModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var email = User.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(email))
                return Forbid();

            var dto = new CreateEventRequestDto
            {
                Title = model.Title,
                StartDateTime = model.StartDateTime,
                Location = model.Location,
                Description = model.Description,
                Phone = model.Phone
            };

            var user = await userManager.GetUserAsync(User);

            await service.CreateAsync(dto, userId, email, user.FullName ?? "-");
            return RedirectToAction(nameof(My));
        }

        [HttpGet]
        public async Task<IActionResult> My()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId)) return Forbid();

            var data = await service.GetMyAsync(userId);
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
