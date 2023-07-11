using DAL.Models;
using DAL.Repositories.UnitOfWork;
using Domain.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class SpamService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IEmailSender emailSender;
        public SpamService(IUnitOfWork unitOfWork, IEmailSender emailSender)
        {
            this.unitOfWork = unitOfWork;   
            this.emailSender = emailSender;
        }
        public async Task<IReadOnlyCollection<SpamEmail>> GetSpamEmails() => await unitOfWork.SpamRepository.GetAllAsync();
        public async Task<IReadOnlyCollection<SpamEmail>> FindByConditionAsync(Expression<Func<SpamEmail, bool>> predicat) => await this.unitOfWork.SpamRepository.FindByConditionAsync(predicat);
        public async Task<OperationDetails> CreateEmail(SpamEmail email) => await unitOfWork.SpamRepository.CreateAsync(email);
        public async Task DeleteAsync(int id) => await unitOfWork.SpamRepository.Delete(id);
        public async Task EditAsync(string id, SpamEmail email) => await unitOfWork.SpamRepository.Update(email, id);
        public async void StartSpam(List<SpamEmail> spamEmails, string subject, string message)
        {
            foreach (var email in spamEmails)
            {
                for(int i = 0; i < email.CountOfCopies; i++)
                {
                    await emailSender.SendEmailAsync(email.Email, subject, message);
                }
            }
        }
    }
}
