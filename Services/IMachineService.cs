using LocalSNMP.Entities;
using System.Collections.Generic;

namespace LocalSNMP.Services
{
    public interface IMachineService
    {
        ICollection<Machine> GetAll();
        Machine GetMachine(int machineId);
    }
}