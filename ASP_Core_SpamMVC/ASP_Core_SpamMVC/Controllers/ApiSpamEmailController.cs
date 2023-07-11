using BLL.Services;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace ASP_Core_SpamMVC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiSpamEmailController : Controller
    {
        private readonly SpamService _spamService;
        public ApiSpamEmailController(SpamService spamService)
        {
            _spamService = spamService;
        }
        [HttpGet("/api/emails")]
        public async Task<IReadOnlyCollection<SpamEmail>> GetEmails()
        {
            return await _spamService.GetSpamEmails();
        }
        [HttpPost("api/emails")]
        public async Task AddEmail(SpamEmail email)
        {
            await _spamService.CreateEmail(email);
        }
        [HttpPut("api/email/{id}")]
        public async Task EditEmail(string id, SpamEmail email)
        {
            await _spamService.EditAsync(id, email);
        }
        [HttpDelete("api/email/{id}")]
        public async Task DeleteEmail(int id)
        {
            await _spamService.DeleteAsync(id);
        }
    }
}
