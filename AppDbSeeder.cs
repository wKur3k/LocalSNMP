using LocalSNMP.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocalSNMP
{
    public class AppDbSeeder
    {
        private readonly AppDbContext _dbContext;

        public AppDbSeeder(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Seed()
        {
            if (_dbContext.Database.CanConnect())
            {
                if (!_dbContext.Users.Any())
                {
                    var users = GetUsers();
                    _dbContext.Users.AddRange(users);
                    _dbContext.SaveChanges();
                }
            }
            ICollection<User> GetUsers()
            {
                var users = new List<User>()
                {
                    new User()
                    {
                        Login = "admin",
                        HashedPassword = "AQAAAAEAACcQAAAAEEtdeMrrUoosdfIav0cWJp6j/vXcTQV7TY/FqPU5Oua29TqPtC+VTBPvk6Yfxb+y1Q==",
                        Role = "admin"
                    },
                    new User()
                    {
                        Login = "observer",
                        HashedPassword = "AQAAAAEAACcQAAAAEEtdeMrrUoosdfIav0cWJp6j/vXcTQV7TY/FqPU5Oua29TqPtC+VTBPvk6Yfxb+y1Q==",
                        Role = "observer"
                    }
                };
                return users;
            }
        }
    }
}
