using BLL.Services;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ASP_Core_SpamMVC.Controllers
{
    [Authorize]
    public class SpamController : Controller
    {
        private readonly SpamService _spamService;
        private readonly UserManager<Client> userManager;
        public SpamController(SpamService spamService, UserManager<Client> userManager)
        {
            _spamService = spamService;
            this.userManager = userManager;
        }
        public async Task<ActionResult> Index()
        {
            return View(await _spamService.FindByConditionAsync(x=>x.clientId == userManager.GetUserId(User)));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(SpamEmail spamEmail)
        {
            try
            {
                spamEmail.clientId = userManager.GetUserId(User).ToString();
                await _spamService.CreateEmail(spamEmail);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }


        [HttpPost]
        
        public async Task<ActionResult> StartSpam(string message, string subject)
        {
            try
            {
                _spamService.StartSpam((List<SpamEmail>)await _spamService.FindByConditionAsync(x => x.clientId == userManager.GetUserId(User)), subject, message);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }
        // POST: SpamController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                
                await _spamService.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
