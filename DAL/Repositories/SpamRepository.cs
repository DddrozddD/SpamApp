using DAL.Context;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;

namespace DAL.Repositories
{
    public class SpamRepository : BaseRepository<SpamEmail>
    {
        public SpamRepository(SpamerContext context) : base(context) 
        {

        }

        public async Task Delete(int id)
        {
            var spam = await this.Entities.FirstOrDefaultAsync(s=>s.Id== id).ConfigureAwait(false);
            if(spam != null)
            {
                this.Entities.Remove(spam);
            }
            await _context.SaveChangesAsync();

        }
        public async Task<OperationDetails> Update(SpamEmail email, string Id)
        {
            var model = this.Entities.Where(s => s.Id == int.Parse(Id)).First();
            model.Email = email.Email;
            model.CountOfCopies = email.CountOfCopies;

            this._context.Entry(model).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            
            await _context.SaveChangesAsync();

            return new OperationDetails() { IsError = false };
        }
    }
}
