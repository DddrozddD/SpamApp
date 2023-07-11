using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.UnitOfWork
{
    public class UnitOfWork:IUnitOfWork
    {
        public UnitOfWork(SpamRepository spamRepository) {
        this.SpamRepository = spamRepository;   
        }
        public SpamRepository SpamRepository { get; }

    }
}
