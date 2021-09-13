using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocalSNMP.Entities
{
    public class Machine
    {
        public int Id { get; set; }
        public string Name { get; set; } //
        public string SystemName { get; set; } //  
        public string Mac { get; set; } //
        public string Ram { get; set; } //
        public string Storage { get; set; }
        public string StorageUsed { get; set; }
        public string StorageFree { get; set; }
        public string IpAddress { get; set; } //
        public string IpMask { get; set; } //
        public string IpGateway { get; set; } //
        public string SystemUptime { get; set; } //
         
    }
}
