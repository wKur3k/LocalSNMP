using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;
using LocalSNMP.Entities;
using Microsoft.Extensions.DependencyInjection;
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
        private readonly string[] _oids = 
            {
                "1.3.6.1.2.1.1.5.0", "1.3.6.1.2.1.1.1.0", "1.3.6.1.2.1.25.2.2.0", "1.3.6.1.2.1.4.21.1.7.0.0.0.0", "1.3.6.1.2.1.25.1.1.0",
                "1.3.6.1.2.1.4.20.1.3.",
                "1.3.6.1.2.1.4.20.1.2.", "1.3.6.1.2.1.2.2.1.6.",
                "1.3.6.1.2.1.25.2.3.1.4.1", "1.3.6.1.2.1.25.2.3.1.5.1", "1.3.6.1.2.1.25.2.3.1.6.1",
            };
        private readonly string _ipAddressNetwork = "192.168.1.0";
        private readonly int _ipRangeStart = 1;
        private readonly int _ipRangeClose = 254;
        private readonly IServiceScopeFactory _scopeFactory;

        public ScanService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            string[] newAddress = _ipAddressNetwork.Split('.');
            newAddress[3] = _ipRangeStart.ToString();
            string newIpAddress = string.Join('.', newAddress);
            IPAddress ipAddress = IPAddress.Parse(newIpAddress);
            Ping p = new Ping();
            PingReply rep;
            while (!stoppingToken.IsCancellationRequested)
            {
                for (int i = _ipRangeStart; i < _ipRangeClose; i++)
                {
                    rep = p.Send(ipAddress);
                    if (rep.Status == IPStatus.Success)
                    {
                        using (var scope = _scopeFactory.CreateScope())
                        {
                            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                            string helpOid;
                            Machine newMachine = new Machine();
                            for(int j = 0; j < 10; j++)
                            {
                                switch (j)
                                {
                                    case 1:

                                        break;
                                    case 2:
                                        break;
                                    case 3:
                                        break;
                                    case 4:
                                        break;
                                    case 5:
                                        helpOid = _oids[5] + ipAddress.ToString();
                                        break;
                                    case 6:
                                        helpOid = _oids[6] + ipAddress.ToString();
                                        break;
                                    case 7:
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine(rep.Status);
                    }
                    byte[] ipBytes = ipAddress.GetAddressBytes();
                    ipBytes[3] += 1;
                    ipAddress = IPAddress.Parse(string.Join('.', ipBytes));
                }
                await Task.Delay(10000, stoppingToken);
            }
        }
        private string sendSNMP(string oid, IPAddress host, AppDbContext dbContext)
        {
            try
            {
                var result = Messenger.Get(VersionCode.V1,
                    new IPEndPoint(host, 161),
                    new OctetString("public"),
                    new List<Variable> { new Variable(new ObjectIdentifier(oid))},
                    1000);
                string[] subs = result[0].ToString().Split("; ");
                return subs[1];
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
