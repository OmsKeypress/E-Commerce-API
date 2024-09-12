using E_Commerce.BLL.Interface;
using E_Commerce.Model;
using EmployeeDirectory.DAL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;

namespace E_Commerce.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api/[controller]")]
    public class EcommerceController : ControllerBase
    {

        private readonly IEcommerce Iecommerce;
        private readonly IHttpContextAccessor _httpContextAccessor;
        IDistributedCache idistributedCache;
        public EcommerceController(IEcommerce ecommerce, IDistributedCache distributedCache, IHttpContextAccessor httpContextAccessor)
        {
            this.Iecommerce = ecommerce;
            this.idistributedCache = distributedCache;
            this._httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("GetAllProducts")]
        public async Task<IActionResult> GetAllProduct()
        {
            string reqDateTime = DateTime.UtcNow.ToString();
            string datakey = GetAllProductsKey.Datakey;
            string serializedfavoriteList;
            List<GetAllProducts> getAllProducts = new List<GetAllProducts>();
            Dictionary<string, object> dctData = new Dictionary<string, object>();
            HttpStatusCode statusCode = HttpStatusCode.OK;
            string systemError = "NO SYSTEM ERROR";

            try
            {
                var redisDb = await idistributedCache.GetAsync(datakey);
                if (redisDb != null)
                {
                    try
                    {
                        serializedfavoriteList = Encoding.UTF8.GetString(redisDb);
                        getAllProducts = JsonConvert.DeserializeObject<List<GetAllProducts>>(serializedfavoriteList) ?? new List<GetAllProducts>(new List<GetAllProducts>());
                    }
                    catch (Exception ex)
                    {
                        systemError = ex.Message;
                    }
                }
                if (getAllProducts?.Count == 0)
                {
                    getAllProducts = await Iecommerce.GetAllProducts();
                    serializedfavoriteList = JsonConvert.SerializeObject(getAllProducts);
                    redisDb = Encoding.UTF8.GetBytes(serializedfavoriteList);
                    var time = new DistributedCacheEntryOptions()
                                    .SetAbsoluteExpiration(DateTime.Now.AddHours(GetAllProductsKey.ExpiryTime));
                    await idistributedCache.SetAsync(datakey, redisDb, time);
                }
            }
            catch (Exception ex)
            {
                getAllProducts = await Iecommerce.GetAllProducts();
                statusCode = HttpStatusCode.BadRequest;
                systemError = JsonConvert.SerializeObject(ex);
            }
            finally
            {
                //Data adding into DctData 
                dctData.Add("Data", getAllProducts);
                Loggin("GetAllProducts", "", dctData, systemError, reqDateTime);
            }
            return this.StatusCode(Convert.ToInt32(statusCode), dctData);
        }

        [HttpPost]
        [Route("GetProductByCategory")]
        public async Task<IActionResult> GetProductByCategory([FromBody] Pagination model)
        {
            string reqDateTime = DateTime.UtcNow.ToString();
            Tuple<List<GetAllProducts>, Transtatus, int> getAllProducts = new Tuple<List<GetAllProducts>, Transtatus, int>(new List<GetAllProducts>(), new Transtatus(), 0);

            Dictionary<string, object> dctData = new Dictionary<string, object>();
            HttpStatusCode statusCode = HttpStatusCode.OK;
            string systemError = "NO SYSTEM ERROR";
            try
            {
                getAllProducts = await Iecommerce.GetProductByCategory(model);
            }
            catch (Exception ex)
            {
                statusCode = HttpStatusCode.BadRequest;
                systemError = JsonConvert.SerializeObject(ex);
            }
            finally
            {
                if (getAllProducts.Item2.Code == 1)
                {
                    dctData.Add("Status", getAllProducts.Item2);
                    dctData.Add("RowCount", getAllProducts.Item3);
                }
                else
                {
                    dctData.Add("Data", getAllProducts.Item1);
                    dctData.Add("RowCount", getAllProducts.Item3);
                }
                //Data adding into DctData 

                Loggin("GetProductByCategory", model, dctData, systemError, reqDateTime);
            }
            return this.StatusCode(Convert.ToInt32(statusCode), dctData);
        }

        [HttpPost]
        [Route("AddProduct")]
        public async Task<IActionResult> AddProduct([FromBody] AddProduct model)
        {
            string reqDateTime = DateTime.UtcNow.ToString();
            Dictionary<string, object> dctData = new Dictionary<string, object>();
            Transtatus transtatus = new Transtatus();
            HttpStatusCode statusCode = HttpStatusCode.OK;
            string systemError = "NO SYSTEM ERROR";
            try
            {
                transtatus = await Iecommerce.AddProduct(model);
            }
            catch (Exception ex)
            {
                statusCode = HttpStatusCode.BadRequest;
                systemError = JsonConvert.SerializeObject(ex);
            }
            finally
            {
                dctData.Add("Status", transtatus);
                //Data adding into DctData 

                Loggin("AddProduct", model, dctData, systemError, reqDateTime);
            }
            return this.StatusCode(Convert.ToInt32(statusCode), dctData);
        }

        [HttpPost]
        [Route("UpdateProduct")]
        public async Task<IActionResult> UpdateProduct([FromBody] UpdateProduct model)
        {
            string reqDateTime = DateTime.UtcNow.ToString();
            Dictionary<string, object> dctData = new Dictionary<string, object>();
            Transtatus transtatus = new Transtatus();
            HttpStatusCode statusCode = HttpStatusCode.OK;
            string systemError = "NO SYSTEM ERROR";
            try
            {
                transtatus = await Iecommerce.UpdateProduct(model);
            }
            catch (Exception ex)
            {
                statusCode = HttpStatusCode.BadRequest;
                systemError = JsonConvert.SerializeObject(ex);
            }
            finally
            {
                dctData.Add("Status", transtatus);
                //Data adding into DctData 

                Loggin("UpdateProduct", model, dctData, systemError, reqDateTime);
            }
            return this.StatusCode(Convert.ToInt32(statusCode), dctData);
        }
        [HttpPost]
        [Route("DeleteProduct")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteProduct([FromBody] DeleteProduct model)
        {
            string reqDateTime = DateTime.UtcNow.ToString();
            Dictionary<string, object> dctData = new Dictionary<string, object>();
            Transtatus transtatus = new Transtatus();
            HttpStatusCode statusCode = HttpStatusCode.OK;
            string systemError = "NO SYSTEM ERROR";
            try
            {
                transtatus = await Iecommerce.DeleteProduct(model);
            }
            catch (Exception ex)
            {
                statusCode = HttpStatusCode.BadRequest;
                systemError = JsonConvert.SerializeObject(ex);
            }
            finally
            {
                dctData.Add("Status", transtatus);
                //Data adding into DctData 

                Loggin("DeleteProduct", model, dctData, systemError, reqDateTime);
            }
            return this.StatusCode(Convert.ToInt32(statusCode), dctData);
        }

        //[HttpGet]
        //[AllowAnonymous]
        //[Route("GetMacaddress")]
        //public async Task<IActionResult> GetMacaddress()
        //{
        //    string MacAddress = string.Empty;
        //    NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
        //    NetworkInterface networkInterface = networkInterfaces[0];
        //    PhysicalAddress physicalAddress = networkInterface.GetPhysicalAddress();
        //    byte[] addressBytes = physicalAddress.GetAddressBytes();
        //    MacAddress = BitConverter.ToString(addressBytes).Replace("-", ":");

        //    return Ok(new { MacAddress = MacAddress });

        //}
        [HttpGet]
        [AllowAnonymous]
        [Route("GetIPAddress")]
        public async Task<IActionResult> GetIPAddress()
        {
            string IPAddress = string.Empty;

            string CreatedIpv4 = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            string CreatedIpv1 = JsonConvert.SerializeObject(_httpContextAccessor.HttpContext.Connection);

            string CreatedIpv3 = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString(); ;


            return Ok(new { CreatedIpv1 = CreatedIpv1 ,});

        }

        private string GetClientIPAddress(HttpContext context)
        {
            // Check for forwarded headers first (if behind a )
            var forwardedHeader = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();

            if (!string.IsNullOrEmpty(forwardedHeader))
            {
                var addresses = forwardedHeader.Split(',', StringSplitOptions.RemoveEmptyEntries);
                return addresses.FirstOrDefault();
            }

            var ipAddress = context.Connection.RemoteIpAddress;

            // If IPv6, convert to IPv4
            if (ipAddress != null)
            {
                if (ipAddress.IsIPv4MappedToIPv6)
                {
                    ipAddress = ipAddress.MapToIPv4();
                }
                else if (IPAddress.IsLoopback(ipAddress))
                {
                    ipAddress = IPAddress.Loopback;
                }
            }

            return ipAddress?.ToString();
        }



        //This method logs every request and response 
        public void Loggin<T>(string method, T request, Dictionary<string, object> Dctdata, string systemError, string reqDateTime)
        {
            string path = "";
            string message = "*******************************************************************" + Environment.NewLine
                                       + "Method :" + method + Environment.NewLine
                                       + "Request UTC :" + reqDateTime.ToString() + Environment.NewLine
                                       + "Response UTC :" + DateTime.UtcNow.ToString() + Environment.NewLine
                                       + "Request:" + JsonConvert.SerializeObject(request) + Environment.NewLine
                                       + "Response:" + JsonConvert.SerializeObject(Dctdata) + Environment.NewLine
                                       + "Exception:" + systemError + Environment.NewLine;
            path = string.Format("\\{0}\\{1}", path, DateTime.UtcNow.ToString("yyyyMMdd"));
            CommonHelper.WriteToFile(message, path, DateTime.UtcNow.ToString("hh_00 tt"), false);
        }
    }
}
