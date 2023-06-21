using JwtAuthDemo.Infrastructure;
using JwtAuthDemo.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace JwtAuthDemo.Services
{

    public class CronJobService
    {
        private DBContext _dbContext;
        private readonly ILogger<CronJobService> _logger;
        private readonly IConfiguration _configuration;
        public CronJobService(ILogger<CronJobService> logger, DBContext dBContext, IConfiguration configuration)
        {
            _logger = logger;
            _dbContext = dBContext;
            _configuration = configuration;
        }
        public async Task ExecuteAsync()
        {

            while (true)
            {
                try
                {
                    _logger.LogInformation($"Start at ${DateTime.Now.ToString()}");
                    Phone[] phones = _dbContext.Phones.ToArray();
                    foreach (Phone phone in phones)
                    {
                        bool isConnect = SMSManager.getDeviceStatus(phone.IP);
                        if (isConnect)
                        {
                            SMSManager smsManager = new SMSManager(phone.IP, phone.Password);
                            JArray smsData = smsManager.getMessages();
                            foreach (JToken jProperty in smsData)
                            {
                                SMS sms = new SMS();
                                sms.ID = Guid.NewGuid();
                                sms.Index = (int)jProperty["index"];
                                sms.From = jProperty["from"].ToString();
                                sms.ReceivedTime = DateTime.ParseExact(jProperty["receivedTime"].ToString(), "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                                sms.Content = jProperty["content"].ToString();
                                sms.PhoneID = phone.ID;

                                var existMessage = _dbContext.SMSs.FirstOrDefault(s => s.Index == (int)jProperty["index"] && s.PhoneID == phone.ID);
                                if (existMessage == null)
                                {
                                    _dbContext.SMSs.Add(sms);
                                    _dbContext.SaveChanges();
                                }

                            }
                        }

                    }
                    // Write your task logic here
                    _logger.LogInformation($"Runn at ${DateTime.Now.ToString()}");
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex.Source + "--" + ex.Message);
                };
                Thread.Sleep(_configuration.GetValue<int>("DelayFecthSMS"));
            }





        }
    }

}
