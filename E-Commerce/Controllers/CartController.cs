using E_Commerce.BLL.Interface;
using E_Commerce.Model;
using EmployeeDirectory.DAL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Net;

namespace E_Commerce.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICart _icart;
        IDistributedCache idistributedCache;
        public CartController(ICart icart, IDistributedCache distributedCache)
        {
            this._icart = icart;
            this.idistributedCache = distributedCache;
        }

        [HttpPost]
        [Route("AddProductInCart")]
        public async Task<IActionResult> AddProductInCart([FromBody] AddProductInCartModel model)
        {
            int CustomerId = 0;
            string reqDateTime = DateTime.UtcNow.ToString();
            Dictionary<string, object> dctData = new Dictionary<string, object>();
            Transtatus transtatus = new Transtatus();
            HttpStatusCode statusCode = HttpStatusCode.OK;
            string systemError = "NO SYSTEM ERROR";
            try
            {
                CustomerId = (int)HttpContext.GetIdentityUserId().UserId;
                transtatus = await _icart.AddProductInCart(model, CustomerId);
            }
            catch (Exception ex)
            {
                transtatus.Message = "Something went wrong.";
                transtatus.Code = 1;
                statusCode = HttpStatusCode.BadRequest;
                systemError = JsonConvert.SerializeObject(ex);
            }
            finally
            {
                dctData.Add("Status", transtatus);
                //Data adding into DctData 

                Loggin("AddProductInCart", model, dctData, systemError, reqDateTime);
            }
            return this.StatusCode(Convert.ToInt32(statusCode), dctData);
        }

        [HttpGet]
        [Route("CustomerCart")]
        public async Task<IActionResult> CustomerCart()
        {
            int CustomerId = 0;
            string reqDateTime = DateTime.UtcNow.ToString();
            Dictionary<string, object> dctData = new Dictionary<string, object>();
            Transtatus transtatus = new Transtatus();
            Tuple<List<UserCart>, decimal> UsarCart = new Tuple<List<UserCart>, decimal>(new List<UserCart>(), 0);

            HttpStatusCode statusCode = HttpStatusCode.OK;
            string systemError = "NO SYSTEM ERROR";
            try
            {
                CustomerId = (int)HttpContext.GetIdentityUserId().UserId;
                UsarCart = await _icart.CustomerCart(CustomerId);
            }
            catch (Exception ex)
            {
                transtatus.Message = "Something went wrong.";
                transtatus.Code = 1;
                statusCode = HttpStatusCode.BadRequest;
                systemError = JsonConvert.SerializeObject(ex);
            }
            finally
            {
                dctData.Add("Products", UsarCart.Item1);
                dctData.Add("Total Amount", UsarCart.Item2);
                //Data adding into DctData 

                Loggin("CustomerCart", CustomerId, dctData, systemError, reqDateTime);
            }
            return this.StatusCode(Convert.ToInt32(statusCode), dctData);
        }

        [HttpPost]
        [Route("DeleteProductInCart")]
        public async Task<IActionResult> DeleteProductInCart([FromQuery] int ProductId)
        {
            int CustomerId = 0;
            string reqDateTime = DateTime.UtcNow.ToString();
            Dictionary<string, object> dctData = new Dictionary<string, object>();
            Transtatus transtatus = new Transtatus();
            HttpStatusCode statusCode = HttpStatusCode.OK;
            string systemError = "NO SYSTEM ERROR";
            try
            {
                CustomerId = (int)HttpContext.GetIdentityUserId().UserId;
                transtatus = await _icart.DeleteProductInCart(ProductId, CustomerId);
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

                Loggin("DeleteProduct", (ProductId, CustomerId), dctData, systemError, reqDateTime);
            }
            return this.StatusCode(Convert.ToInt32(statusCode), dctData);
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
