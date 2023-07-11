using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public  class SpamEmail
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public int CountOfCopies { get; set; }
        public string clientId { get; set; }
        public Client client { get; set; }
    }
}
