using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagmentSystem.Models;
using TaskManagmentSystem.ViewModels;

namespace TaskManagmentSystem.Controllers
{
    [Authorize]
    public class NotificationController : Controller
    {
        private readonly AppDbContext _context;

        public NotificationController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Show()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var notifications = _context.Notifications
                .Where(n=>n.RecipientId==userId && n.DateToSend<=DateTime.Now)
                .Select(n=>new NotificationViewModel
                {
                    Id = n.Id,
                    IsRead = n.IsRead,
                    Details = n.Details,
                    DateToSend = n.DateToSend
                })
                .ToList();

            return View("Show",notifications);
        }

        public async Task<IActionResult> MakeRead(int Id)
        {
            var notification = await _context.Notifications
                .FirstOrDefaultAsync(n => n.Id == Id);
            if (notification is null)
                return NotFound();
            else
            {
                notification.IsRead = true;
                await _context.SaveChangesAsync();
                return RedirectToAction("Show");
            }
        }

        public async Task<IActionResult> MakeAllRead()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var unreadNotifications = _context.Notifications
                .Where(n=>n.RecipientId == userId)
                .Where(n => n.IsRead == false);
            await unreadNotifications.ForEachAsync(n => n.IsRead = true);
            await _context.SaveChangesAsync();
            return RedirectToAction("Show");
        }
    }
}
