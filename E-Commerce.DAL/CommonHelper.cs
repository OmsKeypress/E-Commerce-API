using E_Commerce.DAL;
using E_Commerce.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace EmployeeDirectory.DAL
{
    public static class CommonHelper
    {
        public static bool IsSaveInProgress = false;
        public static IList<FileLogData> fileLogDatas = new List<FileLogData>();
        public static IList<T> DataReaderMapToList<T>(this IDataReader dr)
        {
            List<T> list = new List<T>();
            T obj = default(T);
            while (dr.Read())
            {
                obj = Activator.CreateInstance<T>();
                foreach (PropertyInfo prop in obj.GetType().GetProperties())
                {
                    if (HasColumn(prop.Name) && !object.Equals(dr[prop.Name], DBNull.Value))
                    {
                        prop.SetValue(obj, dr[prop.Name] ?? default, null);
                    }
                }
                list.Add(obj);
            }
            return list;
            bool HasColumn(string columnName)
            {
                try
                {
                    return dr.GetOrdinal(columnName) >= 0;
                }
                catch (IndexOutOfRangeException)
                {
                    return false;
                }
            }
        }
        #region IPDetails
        public static async Task<IpInfo> GetIPDetailsByIp(string ip)
        {
            IpInfo ipInfo = new IpInfo();
            try

            {
                if (!string.IsNullOrWhiteSpace(Setting.ipDetail?.URL))
                {
                    string info = new WebClient().DownloadString(Setting.ipDetail.URL + ip + Setting.ipDetail.Key);
                    ipInfo = JsonConvert.DeserializeObject<IpInfo>(info);
                }
                else
                {
                    ipInfo.status = "Failure";
                }
            }
            catch (Exception)
            {

            }
            return ipInfo;
        }

        #endregion
        public static DataTable ToDataTable<T>(this IList<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties by using reflection
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }
        #region  Encrypt and Decrypt
        public static string Encrypt(string strText)
        {
            if (strText == null || strText == "")
            {
                return strText;
            }
            else
            {
                byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xab, 0xcd, 0xef };
                try
                {
                    string strEncrKey = "&%#@?,:*";
                    byte[] bykey = System.Text.Encoding.UTF8.GetBytes(strEncrKey.Substring(0, 8));
                    byte[] inputByteArray = System.Text.Encoding.UTF8.GetBytes(strText);
                    DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                    MemoryStream ms = new MemoryStream();
                    CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(bykey, IV), CryptoStreamMode.Write);
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    return Convert.ToBase64String(ms.ToArray());
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
        }
        public static string Decrypt(string strText)
        {
            if (strText == null || strText == "")
            {
                return strText;
            }
            else
            {
                byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xab, 0xcd, 0xef };
                byte[] inputByteArray = new byte[strText.Length + 1];
                try
                {
                    string strEncrKey = "&%#@?,:*";
                    byte[] byKey = System.Text.Encoding.UTF8.GetBytes(strEncrKey.Substring(0, 8));
                    DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                    inputByteArray = Convert.FromBase64String(strText);
                    MemoryStream ms = new MemoryStream();
                    CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(byKey, IV), CryptoStreamMode.Write);
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    System.Text.Encoding encoding = System.Text.Encoding.UTF8;
                    return encoding.GetString(ms.ToArray());
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
        }
        #endregion
        public static void WriteToFile(string Message, string path = "", string? filename = null, bool isDateTimeExtend = true)
        {
            string tmpFileName = filename == null || filename == "" ? DateTime.UtcNow.Date.ToShortDateString().Replace('/', '.')
                    : filename + (isDateTimeExtend ? "_" + DateTime.UtcNow.Date.ToShortDateString().Replace('/', '.') : "");
            string basePath = Path.Combine(Directory.GetCurrentDirectory() + "\\Logs" + path + "\\");

            string filepath = basePath + tmpFileName + ".txt";
            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }


            fileLogDatas.Add(new FileLogData()
            {
                Message = Message,
                FilePathWithName = filepath
            });
        }
        public static void SaveQueuedDataIntoFile()
        {

            IsSaveInProgress = true;
            try
            {
                var removeableIds = fileLogDatas.Select(x => x.Id);
                var datas = fileLogDatas.Where(x => removeableIds.Contains(x.Id)).GroupBy(x => x.FilePathWithName).Select(x => new
                {
                    FilePathWithName = x.Key,
                    Message = string.Join(Environment.NewLine, x.Select(y => y.Message)),
                })?.ToList();
                foreach (var logData in datas)
                {
                    if (!File.Exists(logData.FilePathWithName))
                    {
                        // Create a file to write to.
                        using (StreamWriter sw = File.CreateText(logData.FilePathWithName))
                        {
                            sw.WriteLine(logData.Message);
                        }
                    }
                    else
                    {
                        using (StreamWriter sw = File.AppendText(logData.FilePathWithName))
                        {
                            sw.WriteLine(logData.Message);
                        }
                    }
                }
                fileLogDatas = fileLogDatas.Where(x => !removeableIds.Contains(x.Id)).ToList();
            }
            catch (Exception ex)
            {
                string basePath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
                if (!Directory.Exists(basePath))
                {
                    Directory.CreateDirectory(basePath);
                }
                string filePath = basePath + DateTime.UtcNow.Date.ToShortDateString().Replace('/', '.') + ".txt";
                string message = "*******************************************************************" + Environment.NewLine
                            + "Date:" + DateTime.UtcNow + Environment.NewLine + Environment.NewLine
                            + "File Write Error:" + JsonConvert.SerializeObject(ex) + Environment.NewLine + Environment.NewLine;
                if (!File.Exists(filePath))
                {
                    // Create a file to write to.
                    using (StreamWriter sw = File.CreateText(filePath))
                    {
                        sw.WriteLine(message);
                    }
                }
                else
                {
                    using (StreamWriter sw = File.AppendText(filePath))
                    {
                        sw.WriteLine(message);
                    }
                }
            }
            finally
            {
                IsSaveInProgress = false;
            }
        }
        public static string btoa(this string stringToEncode)
        {
            byte[] bytes = Encoding.GetEncoding(28591).GetBytes(stringToEncode);
            string toReturn = System.Convert.ToBase64String(bytes);
            return toReturn;
        }
        //public static ValidateTokenRequest GetIdentityUserId(this HttpContext httpContext)
        //{
        //    StringValues token = "";
        //    var found = httpContext.Request.Headers.TryGetValue("AuthToken", out token);
        //    ValidateTokenRequest model = new ValidateTokenRequest();
        //    if (found && !string.IsNullOrWhiteSpace(token.ToString()))
        //    {
        //        try
        //        {
        //            token = HttpUtility.UrlDecode(token);
        //            string[] decrptedToken = CommonHelper.Decrypt(token.ToString()).Split(":|");
        //            model.Token = decrptedToken[0];
        //            model.UserId = Convert.ToInt32(decrptedToken[1]);
        //            model.UserName = decrptedToken[2];
        //        }
        //        catch
        //        {
        //            model = new ValidateTokenRequest();
        //        }
        //    }
        //    return model;
        //}
        public static ValidateTokenRequest GetIdentityUserId(this HttpContext httpContext)
        {
            ValidateTokenRequest model = new ValidateTokenRequest();

            // Get the Authorization header from the HTTP request.
            string authorizationHeader = httpContext.Request.Headers["Authorization"].FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(authorizationHeader) && authorizationHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    // Remove the "Bearer " prefix to get the token.
                    string token = authorizationHeader.Substring("Bearer ".Length).Trim();

                    // Parse the token using JwtSecurityTokenHandler.
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var jwtToken = tokenHandler.ReadJwtToken(token);

                    // Extract the claims from the JWT token.
                    model.Token = token;
                    model.UserId = Convert.ToInt32(jwtToken.Claims.FirstOrDefault(c => c.Type == "Id")?.Value);
                    model.UserName = jwtToken.Claims.FirstOrDefault(c => c.Type == "username")?.Value;
                }
                catch (Exception ex)
                {
                    // Handle token parsing errors here.
                    model = new ValidateTokenRequest();
                }
            }

            return model;
        }
    }
}

