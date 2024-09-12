using E_Commerce.Model;
using EmployeeDirectory.DAL;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace E_Commerce.DAL
{
    public class EcommerceRepository : BaseRepository
    {
        public async Task<List<GetAllProducts>> GetAllProducts()
        {
            List<GetAllProducts> products = new List<GetAllProducts>();
            using (var connection = new SqlConnection(Connection.ConnectionString))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "dbo.GetAllProduct";
                await connection.OpenAsync();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    products = (reader.DataReaderMapToList<GetAllProducts>()).ToList();
                }
            }
            return products;
        }
        public async Task<Tuple<List<GetAllProducts>, Transtatus, int>> GetProductByCategory(Pagination model)
        {
            List<GetAllProducts> getAllProducts = new List<GetAllProducts>();
            Transtatus transtatus = new Transtatus();
            using (var connection = new SqlConnection(Connection.ConnectionString))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "dbo.GetCategory";
                await connection.OpenAsync();

                cmd.Parameters.AddWithValue("@SearchText", model.SearchText);
                cmd.Parameters.AddWithValue("@PageNo", model.PageNo);
                cmd.Parameters.AddWithValue("@PageSize", model.PageSize);
                cmd.Parameters.Add("@RowCount", SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@message", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@code", SqlDbType.Int).Direction = ParameterDirection.Output;

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    getAllProducts = (reader.DataReaderMapToList<GetAllProducts>()).ToList();
                }
                model.RowCount = (int)cmd.Parameters["@RowCount"].Value;
                transtatus.Message = (string)cmd.Parameters["@message"].Value;
                transtatus.Code = (int)cmd.Parameters["@code"].Value;
            }
            return new Tuple<List<GetAllProducts>, Transtatus, int>(getAllProducts, transtatus, model.RowCount);
        }
        public async Task<Transtatus> AddProduct(AddProduct model)
        {
            Transtatus transtatus = new Transtatus();
            using (var connection = new SqlConnection(Connection.ConnectionString))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "dbo.AddProducts";
                await connection.OpenAsync();
                cmd.Parameters.AddWithValue("@Product_Name", model.Product_Name);
                cmd.Parameters.AddWithValue("@Brand", model.Brand);
                cmd.Parameters.AddWithValue("@Price", model.Price);
                cmd.Parameters.AddWithValue("@DiscountedPrice", model.DiscountedPrice);
                cmd.Parameters.AddWithValue("@Category", model.Category);
                cmd.Parameters.AddWithValue("@Quentity", model.Quentity);
                cmd.Parameters.AddWithValue("@CreatedBy", model.CreatedBy);

                cmd.Parameters.Add("@message", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@code", SqlDbType.Int).Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                transtatus.Message = (string)cmd.Parameters["@message"].Value;
                transtatus.Code = (int)cmd.Parameters["@code"].Value;
            }
            return transtatus;
        }
        public async Task<Transtatus> UpdateProduct(UpdateProduct model)
        {
            Transtatus transtatus = new Transtatus();
            using (var connection = new SqlConnection(Connection.ConnectionString))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "dbo.UpdateProduct";
                await connection.OpenAsync();
                cmd.Parameters.AddWithValue("@PID", model.PID);
                cmd.Parameters.AddWithValue("@Product_Name", model.Product_Name);
                cmd.Parameters.AddWithValue("@Brand", model.Brand);
                cmd.Parameters.AddWithValue("@Price", model.Price);
                cmd.Parameters.AddWithValue("@DiscountedPrice", model.DiscountedPrice);
                cmd.Parameters.AddWithValue("@Category", model.Category);
                cmd.Parameters.AddWithValue("@Quentity", model.Quentity);
                cmd.Parameters.AddWithValue("@ModifiedBy", model.ModifiedBy);

                cmd.Parameters.Add("@message", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@code", SqlDbType.Int).Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                transtatus.Message = (string)cmd.Parameters["@message"].Value;
                transtatus.Code = (int)cmd.Parameters["@code"].Value;
            }
            return transtatus;
        }
        public async Task<Transtatus> DeleteProduct(DeleteProduct model)
        {
            model.Test = double.PositiveInfinity;
            Transtatus transtatus = new Transtatus();
            using (var connection = new SqlConnection(Connection.ConnectionString))
            {
                try
                {

                    double num = double.PositiveInfinity;
                    SqlCommand cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "dbo.DeleteProduct";
                    await connection.OpenAsync();
                    cmd.Parameters.AddWithValue("@PID", model.PID);
                    cmd.Parameters.AddWithValue("@Infinity", model.Test);

                    cmd.Parameters.Add("@message", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@code", SqlDbType.Int).Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();
                    transtatus.Message = (string)cmd.Parameters["@message"].Value;
                    transtatus.Code = (int)cmd.Parameters["@code"].Value;
                }
                catch (Exception ex)
                {
                    string error = ex.ToString();
                }
            }
            return transtatus;
        }
        public async Task<Transtatus> LoginCustomer(Authentication model)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                Transtatus transtatus = new Transtatus();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "dbo.Authentication";
                await connection.OpenAsync();

                model.Password = CommonHelper.Encrypt(model.Password);

                cmd.Parameters.AddWithValue("@Id", model.Id);
                cmd.Parameters.AddWithValue("@Password", model.Password);
                cmd.Parameters.Add("@message", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@code", SqlDbType.Int).Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                transtatus.Message = (string)cmd.Parameters["@message"].Value;
                transtatus.Code = (int)cmd.Parameters["@code"].Value;

                return transtatus;
            }
        }
        public async Task<Transtatus> CustomerRegester(CustomerRegesterReqModel model)
        {
            Transtatus tranStatus = new Transtatus();
            using (var connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.RegisterCustomer";
                await connection.OpenAsync();

                command.Parameters.AddWithValue("@Customername", model.CustomerName);
                command.Parameters.AddWithValue("@CustomerPassword", model.Password);
                command.Parameters.AddWithValue("@MobileNo", model.MobileNo);
                command.Parameters.AddWithValue("@Email", model.Email);
                command.Parameters.AddWithValue("@Info", model.Info);
                command.Parameters.AddWithValue("@Ip", model.Ip);

                command.Parameters.Add("@Code", SqlDbType.Int).Direction = ParameterDirection.Output;
                command.Parameters.Add("@Message", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                command.Parameters.Add("@Id", SqlDbType.Int).Direction = ParameterDirection.Output;
                foreach (SqlParameter Parameter in command.Parameters)
                {
                    Parameter.Value ??= DBNull.Value;
                }
                command.ExecuteNonQuery();

                tranStatus.Code = (int)command.Parameters["@Code"].Value;
                tranStatus.Message = (string)command.Parameters["@Message"].Value;
                int UserId = (int)command.Parameters["@Id"].Value;
            }
            return tranStatus;
        }
    }
}
