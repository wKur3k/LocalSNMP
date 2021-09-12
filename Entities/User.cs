using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocalSNMP.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Role { get; set; }
        public string HashedPassword { get; set; }

    }
}
