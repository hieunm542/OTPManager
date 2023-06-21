using JwtAuthDemo.Infrastructure;
using JwtAuthDemo.Models;
using JwtAuthDemo.Models.Reponses;
using JwtAuthDemo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    public class PhoneController : ControllerBase
    {

        private readonly ILogger<PhoneController> _logger;
        private readonly IUserService _userService;
        private readonly IJwtAuthManager _jwtAuthManager;
        private DBContext _dbContext;
        public PhoneController(ILogger<PhoneController> logger, IUserService userService, IJwtAuthManager jwtAuthManager, DBContext dBContext)
        {
            _logger = logger;
            _userService = userService;
            _jwtAuthManager = jwtAuthManager;
            _dbContext = dBContext;
        }
        [HttpGet("getPhones")]
        public async Task<PhoneResponse> getPhones(int pageIndex, int pageSize)
        {
            PhoneResponse phoneResponse = new PhoneResponse();
            phoneResponse.total = _dbContext.Phones.Count();
            //phoneResponse.items =
            Phone[] phoneData = _dbContext.Phones.Skip(pageIndex * pageSize).Take(pageSize).ToArray();
            foreach (Phone phone in phoneData)
            {
                phone.ConnectStatus = await SMSManager.getDeviceStatusAsync(phone.IP);
            }
            phoneResponse.items = phoneData;
            return phoneResponse;
        }
        [HttpPost("addPhone")]
        public async Task<string> addPhone()
        {
            string requestBody = string.Empty;

            // Read the request body
            using (var reader = new System.IO.StreamReader(Request.Body))
            {
                requestBody = await reader.ReadToEndAsync();
            }
            var body = JToken.Parse(requestBody);
            Phone phone = new Phone();
            phone.ID = Guid.NewGuid();
            phone.PhoneNumber = body["phoneNumber"].ToString();
            phone.IP = body["ip"].ToString();
            phone.Account = body["account"].ToString();
            phone.Password = body["password"].ToString();

            _dbContext.Phones.Add(phone);
            _dbContext.SaveChanges();
            return "";
        }
        [HttpPost("deletePhone")]
        public string deletePhone(string id)
        {
            var phone = _dbContext.Phones.FirstOrDefault(p => p.ID.ToString() == id);
            _dbContext.Phones.Remove(phone);
            _dbContext.SaveChanges();
            return "";
        }

        [HttpPut("updatePhone")]
        public Phone updatePhone(Phone phone)
        {
            _dbContext.Phones.Update(phone);
            _dbContext.SaveChanges();
            return _dbContext.Phones.Find(phone.ID);
        }
    }
}
