using LocalSNMP.Entities;
using LocalSNMP.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocalSNMP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MachineController : ControllerBase
    {
        private readonly IMachineService _machineService;

        public MachineController(IMachineService machineService)
        {
            _machineService = machineService;
        }

        [HttpGet]
        public ActionResult<ICollection<Machine>> GetAll()
        {
            var machines = _machineService.GetAll();
            if(machines is null)
            {
                return NotFound("Machines not found");
            }
            return Ok(machines);
        }

        [HttpGet]
        [Route("{machineId}")]
        public ActionResult<Machine> GetMachine([FromRoute] int machineId)
        {
            var machine = _machineService.GetMachine(machineId);
            if(machine is null)
            {
                return NotFound("Machine not found");
            }
            return Ok(machine);
        }
    }
}
