using Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DAL.Context
{
    public class SpamerContext:IdentityDbContext
    {
        public SpamerContext(DbContextOptions<SpamerContext> options) : base(options) 
        {
            Database.EnsureCreated();
        }
        public DbSet<SpamEmail> Emails { get; set; }
        public DbSet<Client> Clients { get; set; }
    }
}
