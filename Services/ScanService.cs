﻿using Lextm.SharpSnmpLib;
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

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Run(() => method());
                await Task.Delay(3600000, stoppingToken);
            }
        }
        private string sendSNMP(string oid, IPAddress host)
        {
            try
            {
                var result = Messenger.Get(VersionCode.V1,
                    new IPEndPoint(host, 161),
                    new OctetString("public"),
                    new List<Variable> { new Variable(new ObjectIdentifier(oid))},
                    1000);
                string[] subs = result[0].ToString().Split("; ");
                return subs[1].Remove(0, 6);

            }
            catch (Exception)
            {
                return "Exception occured";
            }
        }
        private void method()
        {
            string[] newAddress = _ipAddressNetwork.Split('.');
            newAddress[3] = _ipRangeStart.ToString();
            string newIpAddress = string.Join('.', newAddress);
            IPAddress ipAddress = IPAddress.Parse(newIpAddress);
            Ping p = new Ping();
            PingReply rep;
            for (int i = _ipRangeStart; i < _ipRangeClose; i++)
            {
                rep = p.Send(ipAddress);
                if (rep.Status == IPStatus.Success)
                {
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                        string value;
                        string newOid;
                        bool status = true;
                        Machine newMachine = new Machine();
                        if (dbContext.Machines.FirstOrDefault(m => m.IpAddress == ipAddress.ToString()) is not null)
                        {
                            newMachine = dbContext.Machines.FirstOrDefault(m => m.IpAddress == ipAddress.ToString());
                            status = false;
                        }
                        newMachine.IpAddress = ipAddress.ToString();
                        for (int j = 0; j < 8; j++)
                        {
                            switch (j)
                            {
                                case 0:
                                    value = sendSNMP(_oids[0], ipAddress);
                                    newMachine.Name = value;
                                    break;
                                case 1:
                                    value = sendSNMP(_oids[1], ipAddress);
                                    if (value.Contains("Windows"))
                                    {
                                        newMachine.SystemName = "Windows";
                                    }
                                    else if (value.Contains("Linux") || value.Contains("Ubuntu"))
                                    {
                                        newMachine.SystemName = "Linux";
                                    }
                                    else
                                    {
                                        newMachine.SystemName = "Not recognized";
                                    }
                                    break;
                                case 2:
                                    value = sendSNMP(_oids[2], ipAddress);
                                    newMachine.Ram = value;
                                    break;
                                case 3:
                                    value = sendSNMP(_oids[3], ipAddress);
                                    newMachine.IpGateway = value;
                                    break;
                                case 4:
                                    value = sendSNMP(_oids[4], ipAddress);
                                    newMachine.SystemUptime = value;
                                    break;
                                case 5:
                                    newOid = _oids[5] + ipAddress.ToString();
                                    value = sendSNMP(newOid, ipAddress);
                                    newMachine.IpMask = value;
                                    break;
                                case 6:
                                    newOid = _oids[6] + ipAddress.ToString();
                                    value = sendSNMP(newOid, ipAddress);
                                    newOid = _oids[7] + value;
                                    string x = sendSNMP(newOid, ipAddress);
                                    x = x.ToString();
                                    newMachine.Mac = "Encoding not available";
                                    break;
                                case 7:
                                    //todo fix values
                                    value = sendSNMP(_oids[8], ipAddress);
                                    string valueHelp = sendSNMP(_oids[9], ipAddress);
                                    long allocationUnits;
                                    long size;
                                    if (Int64.TryParse(value, out allocationUnits) && Int64.TryParse(valueHelp, out size))
                                    {
                                        allocationUnits = Int64.Parse(value);
                                        size = Int64.Parse(valueHelp);
                                        size = allocationUnits * size / 1000000000;
                                        newMachine.Storage = size.ToString();
                                        value = sendSNMP(_oids[10], ipAddress);
                                        if (Int64.TryParse(value, out size))
                                        {
                                            size = Int64.Parse(value);
                                            size = allocationUnits * size / 1000000000;;
                                            newMachine.StorageUsed = size.ToString();
                                            size = Int64.Parse(newMachine.Storage) - Int64.Parse(newMachine.StorageUsed);
                                            newMachine.StorageFree = size.ToString();
                                            newMachine.Storage += " GB";
                                            newMachine.StorageFree += " GB";
                                            newMachine.StorageUsed += " GB";
                                        }
                                        else
                                        {
                                            newMachine.StorageUsed = "Can't read storage";
                                            newMachine.StorageFree = "Can't read storage";
                                        }
                                    }
                                    else
                                    {
                                        newMachine.Storage = "Can't read storage";
                                        newMachine.StorageUsed = "Can't read storage";
                                        newMachine.StorageFree = "Can't read storage";
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                        if (status)
                        {
                            dbContext.Machines.Add(newMachine);
                        }
                        dbContext.SaveChanges();
                    }
                }
                else
                {
                }
                byte[] ipBytes = ipAddress.GetAddressBytes();
                ipBytes[3] += 1;
                ipAddress = IPAddress.Parse(string.Join('.', ipBytes));
            }
        }
    }
}
