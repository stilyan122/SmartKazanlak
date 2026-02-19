using Microsoft.AspNetCore.Mvc;
using SmartKazanlak.Core.Contract;

namespace SmartKazanlak.Controllers
{
    public class DebugController : Controller
    {
        private readonly IEventRequestService service;

        public DebugController(IEventRequestService service)
        {
            this.service = service;
        }

        public async Task<IActionResult> Pending()
        {
            var data = await service.GetPendingAsync();
            return Json(data);
        }
    }
}
