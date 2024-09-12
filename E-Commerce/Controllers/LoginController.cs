using E_Commerce.BLL.Interface;
using E_Commerce.Model;
using EmployeeDirectory.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using UAParser;
using ClientInfo = UAParser.ClientInfo;

namespace E_Commerce.Controllers
{

    //[ApiController]
    //[Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IEcommerce Iecommerce;
        IConfiguration iConfiguration;
        public LoginController(IEcommerce ecommerce, IConfiguration iConfiguration)
        {
            this.Iecommerce = ecommerce;
            this.iConfiguration = iConfiguration;
        }

        //[HttpPost]
        //[Route("LoginCustomer")]
        //public async Task<IActionResult> LoginCustomer([FromBody] Authentication model)
        //{
        //    Transtatus transtatus = new Transtatus();
        //    Dictionary<string, object> dctData = new Dictionary<string, object>();
        //    HttpStatusCode statusCode = HttpStatusCode.OK;
        //    string systemError = "NO SYSTEM ERROR";
        //    try
        //    {
        //        transtatus = await Iecommerce.LoginCustomer(model);
        //        if (transtatus.Code == 1)
        //        {
        //            transtatus.Message = "Invalid Credentials";
        //            transtatus.Code = 1;
        //            return BadRequest("Unauthorized");
        //        }
        //        else
        //        {
        //            transtatus.Message = "Login Success";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        transtatus.Message = "Something Went wrong.";
        //        transtatus.Code = 1;
        //        statusCode = HttpStatusCode.BadRequest;
        //        systemError = JsonConvert.SerializeObject(ex);

        //    }
        //    finally
        //    {
        //        string Token = GetToken(model);
        //        dctData.Add("status", transtatus);
        //        dctData.Add("Token", Token);
        //    }
        //    return this.StatusCode(Convert.ToInt32(statusCode), dctData);
        //}

        //[HttpPost]
        //[AllowAnonymous]
        //[Route("CustomerRegester")]
        //public async Task<IActionResult> CustomerRegistraion([FromBody] CustomerRegesterReqModel model)
        //{

        //    DateTime reqDateTime = DateTime.UtcNow;
        //    Transtatus transaction = new Transtatus();
        //    Transtatus DbTransaction = new Transtatus();
        //    string ip = string.Empty;
        //    UserDeviceDetail deviceDetail = new UserDeviceDetail();
        //    CustomerRegesterReqModel registerRequestModel = new CustomerRegesterReqModel();
        //    Dictionary<string, object> dctData = new Dictionary<string, object>();
        //    HttpStatusCode statusCode = HttpStatusCode.OK;
        //    string json = string.Empty,
        //            msg = "NO SYSTEM ERROR",
        //            strIpInfoErro = "NO IP INFO ERROR",
        //            signalRError = "NO SIGNALR ERROR";
        //    StringBuilder reqResLog = new StringBuilder();

        //    try
        //    {
        //        ip = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        //        if (Request.Headers.ContainsKey("X-Forwarded-For"))
        //            ip = Request.Headers["X-Forwarded-For"];
        //        if (string.IsNullOrEmpty(ip) && Request.Headers.ContainsKey("CF-CONNECTING-IP"))
        //            ip = Request.Headers["CF-CONNECTING-IP"].ToString();

        //        string uaString = HttpContext.Request.Headers["User-Agent"].ToString();
        //        Parser uaParser = Parser.GetDefault();
        //        ClientInfo clientInfo = uaParser.Parse(uaString);

        //        deviceDetail.browser = clientInfo.UA.Family;
        //        deviceDetail.version = clientInfo.UA.Major + "." + clientInfo.UA.Minor ?? "0" + "." + clientInfo.UA.Patch ?? "0";
        //        deviceDetail.os = clientInfo.OS.Family;
        //        deviceDetail.osVesrion = clientInfo.OS.Major + "." + clientInfo.OS.Minor ?? "0"
        //                + "." + clientInfo.OS.Patch ?? "0" + "." + clientInfo.OS.PatchMinor ?? "0";
        //        deviceDetail.device = clientInfo.Device.Family;
        //        deviceDetail.brand = clientInfo.Device.Brand;
        //        deviceDetail.isSpider = clientInfo.Device.IsSpider;
        //        deviceDetail.model = clientInfo.Device.Model;
        //        deviceDetail.ipv4 = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        //        deviceDetail.ipv6 = HttpContext.Connection.RemoteIpAddress.MapToIPv6().ToString();

        //        if (Request.Headers.ContainsKey("X-Forwarded-For"))
        //            deviceDetail.ipv4 = Request.Headers["X-Forwarded-For"];

        //        if (string.IsNullOrEmpty(deviceDetail.ipv4) && Request.Headers.ContainsKey("CF-CONNECTING-IP"))
        //            deviceDetail.ipv4 = Request.Headers["CF-CONNECTING-IP"].ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        strIpInfoErro = JsonConvert.SerializeObject(ex);
        //    }

        //    registerRequestModel.Ip = ip;
        //    registerRequestModel.Info = JsonConvert.SerializeObject(deviceDetail);
        //    registerRequestModel.CustomerName = model.CustomerName;
        //    registerRequestModel.MobileNo = model.MobileNo;
        //    registerRequestModel.Email = model.Email;
        //    registerRequestModel.Password = CommonHelper.Encrypt(model.Password);
        //    try
        //    {


        //        if (model.MobileNo.ToString().Length != 10)
        //        {
        //            transaction.Code = 2;
        //            transaction.Message = "Please enter a valid Phone number.";
        //        }
        //        else
        //        {
        //            if (!string.IsNullOrWhiteSpace(model.MobileNo))
        //            {
        //                transaction = await Iecommerce.CustomerRegester(registerRequestModel);
        //            }
        //            else
        //            {
        //                transaction.Code = 2;
        //                transaction.Message = "Contact number is required.";
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        transaction.Code = 2;
        //        transaction.Message = "Somthing went wrong";
        //        msg = JsonConvert.SerializeObject(e);
        //        statusCode = HttpStatusCode.BadRequest;
        //    }
        //    finally
        //    {
        //        dctData.Add("Status", transaction);
        //        string message = "*******************************************************************" + Environment.NewLine
        //                             + "Request UTC: " + reqDateTime.ToString() + Environment.NewLine
        //                             + "Response UTC: " + DateTime.UtcNow.ToString() + Environment.NewLine
        //                             + "HTTP Status Code: " + statusCode.ToString() + Environment.NewLine
        //                             + "Request: " + JsonConvert.SerializeObject(model) + Environment.NewLine
        //                             + "Response:" + JsonConvert.SerializeObject(dctData) + Environment.NewLine
        //                             + reqResLog.ToString()
        //                             + "OTPR"
        //                             + "Exception:" + msg + Environment.NewLine
        //                             + "Ip Info Exception:" + strIpInfoErro + Environment.NewLine
        //                             + "Hub Exception:" + signalRError + Environment.NewLine;

        //        string path = string.Format("\\Login\\CustomerRegester\\{0}", DateTime.UtcNow.ToString("yyyyMMdd"));
        //        CommonHelper.WriteToFile(message, path, DateTime.UtcNow.ToString("hh_00 tt"), false);
        //    }
        //    return this.StatusCode(Convert.ToInt32(statusCode), dctData);
        //}
        //public static async Task<GetCustomerIPAndDeviceDetail> CustomerIpDetail(string uaString, string Ip)
        //{
        //    GetCustomerIPAndDeviceDetail iPAndDeviceDetail = new GetCustomerIPAndDeviceDetail();
        //    try
        //    {
        //        Parser uaParser = Parser.GetDefault();
        //        UAParser.ClientInfo clientInfo = uaParser.Parse(uaString);
        //        var result = await CommonHelper.GetIPDetailsByIp(Ip);

        //        iPAndDeviceDetail.IP = Ip;
        //        iPAndDeviceDetail.CustomerName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
        //        iPAndDeviceDetail.City = result.status.ToLower() == "success" ? result.city : null;
        //        iPAndDeviceDetail.State = result.status.ToLower() == "success" ? result.status : null;
        //        iPAndDeviceDetail.Country = result.status.ToLower() == "success" ? result.country : null;
        //        iPAndDeviceDetail.TimeZone = result.status.ToLower() == "success" ? result.timezone : null;
        //        //  iPAndDeviceDetail.OsName = HttpContext.Current.Request.CustomerAgent;// Environment.OSVersion.ToString();
        //        iPAndDeviceDetail.OsVersion = clientInfo.UA.Major + "." + clientInfo.UA.Minor ?? "0" + "." + clientInfo.UA.Patch ?? "0";
        //        iPAndDeviceDetail.SystemName = Environment.MachineName;
        //        iPAndDeviceDetail.Browser = clientInfo.UA.Family;
        //        iPAndDeviceDetail.SystemManufacturer = clientInfo.Device.Brand;
        //        iPAndDeviceDetail.SystemModel = clientInfo.Device.Model;
        //        iPAndDeviceDetail.SystemCores = Environment.ProcessorCount;
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return iPAndDeviceDetail;
        //}
        //public string GetToken(Authentication authentication)
        //{

        //    var claims = new[]
        //    {
        //        new Claim(JwtRegisteredClaimNames.Sub,iConfiguration["Jwt:Subject"]),
        //        new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
        //        new Claim(JwtRegisteredClaimNames.Iat,DateTime.UtcNow.ToString()),
        //        new Claim("Id",authentication.Id.ToString()),
        //        new Claim("Password",authentication.Password)
        //    };
        //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(iConfiguration["Jwt:Key"]));
        //    var singin = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        //    var token = new JwtSecurityToken
        //    (
        //        iConfiguration["Jwt:Issuer"],
        //        iConfiguration["Jwt:Audience"],
        //        claims,
        //        expires: DateTime.UtcNow.AddMinutes(10),
        //        signingCredentials: singin);

        //    string Token = new JwtSecurityTokenHandler().WriteToken(token);
        //    return Token;
        //}
    }
} 