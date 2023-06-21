using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace JwtAuthDemo.Infrastructure
{
    public class SMSManager
    {

        private string _ip = "";
        private string _password = "";
        public SMSManager(string ip, string password)
        {
            _ip = ip;
            _password = password;
        }
        public static string MD5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text  
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));


            //get hash result after compute it  
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits  
                //for each byte  
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }
        private static string getNonce(string _ip)
        {
            var client = new RestClient(_ip);
            var request = new RestRequest("cgi-bin/auth_cgi", Method.Post);
            request.Timeout = -1;
            request.AddHeader("Content-Type", "application/json");
            var body = @"{  ""module"": ""authenticator"",   ""action"": 0}";
            request.AddStringBody(body, DataFormat.Json);
            RestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);

            string nonce = JToken.Parse(response.Content)["nonce"].ToString();
            return nonce;
        }
        public static string authen(string _ip, string _password)
        {
            var nonce = _password + ":" + getNonce(_ip);
            var digget = MD5Hash(nonce);
            var client = new RestClient(_ip);
            var request = new RestRequest("cgi-bin/auth_cgi", Method.Post);
            request.Timeout = -1;
            request.AddHeader("Content-Type", "application/json");
            var body = @"{   ""module"": ""authenticator"",   ""action"": 1,  ""digest"": """ + digget + "\"}";
            request.AddStringBody(body, DataFormat.Json);
            RestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            string token = JToken.Parse(response.Content)["token"].ToString();
            return token;
        }
        private static int getTotalMessage(string _ip, string token)
        {
            JToken body = JToken.Parse("{}");
            body["token"] = token;
            body["module"] = "message";
            body["action"] = 2;
            body["pageNumber"] = 1;
            body["amountPerPage"] = 1;
            body["box"] = 0;
            var client = new RestClient(_ip);
            var request = new RestRequest("cgi-bin/web_cgi", Method.Post);
            request.Timeout = -1;
            request.AddHeader("Content-Type", "application/json");
            request.AddStringBody(body.ToString(), DataFormat.Json);
            RestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            return Convert.ToInt32(JToken.Parse(response.Content)["totalNumber"].ToString());
        }
        private static JToken getMessage(string _ip, string token, int page, int amount)
        {
            JToken body = JToken.Parse("{}");
            body["token"] = token;
            body["module"] = "message";
            body["action"] = 2;
            body["pageNumber"] = page;
            body["amountPerPage"] = amount;
            body["box"] = 0;
            var client = new RestClient(_ip);
            var request = new RestRequest("cgi-bin/web_cgi", Method.Post);
            request.Timeout = -1;
            request.AddHeader("Content-Type", "application/json");
            request.AddStringBody(body.ToString(), DataFormat.Json);
            RestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            return JToken.Parse(response.Content)["messageList"];
        }
        public JArray getMessages()
        {
            var token = authen(this._ip, this._password);
            var totalMessage = getTotalMessage(this._ip, token);
            var message = JArray.Parse("[]");
            var nguyen = Convert.ToInt32(totalMessage / 8);
            var le = Convert.ToInt32(totalMessage % 8) > 0 ? 1 : 0;
            var paging = nguyen + le;

            for (var i = 1; i <= paging; i++)
            {
                message.Merge(getMessage(this._ip, token, i, 8));
            }
            return message;
        }
        public static async Task<bool> getDeviceStatusAsync(string ip)
        {
            try
            {
                var pinger = new Ping();

                ip = ip.Replace("http://", "").Replace("https://", "").Replace("/", "");
                PingReply reply = await pinger.SendPingAsync(ip, 1);
                bool pingable = reply.Status == IPStatus.Success;
                return pingable;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        public static bool getDeviceStatus(string ip)
        {
            try
            {
                var pinger = new Ping();

                ip = ip.Replace("http://", "").Replace("https://", "").Replace("/", "");
                PingReply reply = pinger.Send(ip, 1);
                bool pingable = reply.Status == IPStatus.Success;
                return pingable;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
    }
}
