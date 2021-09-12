using LocalSNMP.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocalSNMP.Services
{
    public class MachineService : IMachineService
    {
        private readonly AppDbContext _dbContext;

        public MachineService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ICollection<Machine> GetAll()
        {
            var machines = _dbContext
                .Machines
                .ToList();
            if (!machines.Any())
            {
                return null;
            }
            return machines;
        }

        public Machine GetMachine(int machineId)
        {
            var machine = _dbContext
                .Machines
                .FirstOrDefault(m => m.Id == machineId);
            if (machine is null)
            {
                return null;
            }
            return machine;
        }
    }
}
