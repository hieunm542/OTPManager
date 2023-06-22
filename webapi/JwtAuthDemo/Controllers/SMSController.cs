using JwtAuthDemo.Infrastructure;
using JwtAuthDemo.Models;
using JwtAuthDemo.Models.Reponses;
using JwtAuthDemo.Models.Requests;
using JwtAuthDemo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JwtAuthDemo.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class SMSController : Controller
    {
        private readonly ILogger<SMSController> _logger;
        private readonly IUserService _userService;
        private readonly IJwtAuthManager _jwtAuthManager;
        private DBContext _dbContext;
        public SMSController(ILogger<SMSController> logger, IUserService userService, IJwtAuthManager jwtAuthManager, DBContext dBContext)
        {
            _logger = logger;
            _userService = userService;
            _jwtAuthManager = jwtAuthManager;
            _dbContext = dBContext;
        }
        [HttpGet("getMessage")]
        public void getMessage()
        {
            Phone[] phones = _dbContext.Phones.ToArray();
            foreach (Phone phone in phones)
            {
                SMSManager smsManager = new SMSManager(phone.IP, phone.Password);
                JArray smsData = smsManager.getMessages();
                foreach (JToken jProperty in smsData)
                {
                    SMS sms = new SMS();
                    sms.ID = Guid.NewGuid();
                    sms.Index = (int)jProperty["index"];
                    sms.From = jProperty["from"].ToString();
                    //2023-06-19 15:04:17
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

        [HttpPost("getSMS")]
        public SMSResponse getSMS(SMSRequest smsRequest)
        {
            var smsList = _dbContext.SMSs.OrderByDescending(p=>p.ReceivedTime).ToArray();
            if (smsRequest.index.HasValue)
            {
                smsList = smsList.Where(sms => sms.Index == smsRequest.index).ToArray();
            }
            if (!String.IsNullOrEmpty(smsRequest.from))
            {
                smsList = smsList.Where(sms => sms.From == smsRequest.from).ToArray();
            }
            if (!String.IsNullOrEmpty(smsRequest.content))
            {
                smsList = smsList.Where(sms => sms.Content.Contains(smsRequest.content)).ToArray();
            }
            if (smsRequest.phoneID.HasValue)
            {
                smsList = smsList.Where(sms => sms.PhoneID == smsRequest.phoneID).ToArray();
            }
            if (!String.IsNullOrEmpty(smsRequest.minReceivedTime.ToString()))
            {
                smsList = smsList.Where(sms => sms.ReceivedTime >= smsRequest.minReceivedTime).ToArray();
            }
            if (!String.IsNullOrEmpty(smsRequest.maxReceivedTime.ToString()))
            {
                smsList = smsList.Where(sms => sms.ReceivedTime <= smsRequest.maxReceivedTime).ToArray();
            }
            var joinResult = from sms in smsList
                             join phone in _dbContext.Phones.ToArray() on sms.PhoneID equals phone.ID
                             select new SMSMod
                             {
                                 ID = sms.ID,
                                 Index = sms.Index,
                                 From = sms.From,
                                 Content = sms.Content,
                                 ReceivedTime = sms.ReceivedTime,
                                 PhoneID = sms.PhoneID,
                                 CreatedBy = sms.CreatedBy,
                                 UpdatedBy = sms.UpdatedBy,
                                 CreatedAt = sms.CreatedAt,
                                 UpdatedAt = sms.UpdatedAt,
                                 PhoneNumber = phone.PhoneNumber
                             };
            if (!String.IsNullOrEmpty(smsRequest.phoneNumber))
            {
                joinResult = joinResult.Where(sms => sms.PhoneNumber == smsRequest.phoneNumber).ToArray();
            }
            joinResult = joinResult.Skip(smsRequest.pageIndex * smsRequest.pageSize).Take(smsRequest.pageSize);
            SMSResponse smsResponse = new SMSResponse()
            {
                total = joinResult.Count(),
                items = joinResult.ToArray()
            };

            return smsResponse;
        }
    }
}
