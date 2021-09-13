using LocalSNMP.Entities;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;

namespace LocalSNMP
{
    public class ScanService : BackgroundService
    {
        private readonly string _ipAddressNetwork = "192.168.1.0";
        private readonly int _ipRangeStart = 1;
        private readonly int _ipRangeClose = 254;
        private readonly string[] _oids = { };

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            string[] newAddress = _ipAddressNetwork.Split('.');
            newAddress[3] = _ipRangeStart.ToString();
            string newIpAddress = string.Join('.', newAddress);
            while (!stoppingToken.IsCancellationRequested)
            {
                IPAddress ipAddress = IPAddress.Parse(newIpAddress);
                Ping p = new Ping();
                for (int i = _ipRangeStart; i < _ipRangeClose; i++)
                {
                    PingReply rep = p.Send(ipAddress);
                    if (rep.Status == IPStatus.Success)
                    {
                        Console.WriteLine(ipAddress.ToString());
                    }
                    else
                    {
                        Console.WriteLine(rep.Status);
                    }
                    byte[] ipBytes = ipAddress.GetAddressBytes();
                    ipBytes[3] += 1;
                    ipAddress = IPAddress.Parse(string.Join('.', ipBytes));
                    await Task.Delay(10000, stoppingToken);
                }
            }
        }
    }
}
