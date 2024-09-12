using E_Commerce.Model;
using EmployeeDirectory.DAL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.DAL
{
    public class CartRepository :BaseRepository
    {
        public async Task<Transtatus> AddProductInCart(AddProductInCartModel model, int CustomerId)
        {
            Transtatus tranStatus = new Transtatus();
            using (var connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.AddProductInCart";
                await connection.OpenAsync();

                command.Parameters.AddWithValue("@CustomerId", CustomerId);
                command.Parameters.AddWithValue("@productId", model.ProductId);
                command.Parameters.AddWithValue("@Quentity", model.Quentity);
                command.Parameters.Add("@Code", SqlDbType.Int).Direction = ParameterDirection.Output;
                command.Parameters.Add("@Message", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                foreach (SqlParameter Parameter in command.Parameters)
                {
                    Parameter.Value ??= DBNull.Value;
                }
                command.ExecuteNonQuery();
                tranStatus.Code = (int)command.Parameters["@Code"].Value;
                tranStatus.Message = (string)command.Parameters["@Message"].Value;
            }
            return tranStatus;
        }
        public async Task<Tuple<List<UserCart>, decimal>> CustomerCart(int CustomerId)
        {
            decimal TotalAmount = 0;
            List<UserCart> products = new List<UserCart>();
            using (var connection = new SqlConnection(Connection.ConnectionString))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "dbo.ViewCustomerCart";
                await connection.OpenAsync();
                cmd.Parameters.AddWithValue("@CustomerId", CustomerId);
                cmd.Parameters.Add("@TotalAmount", SqlDbType.Decimal).Direction = ParameterDirection.Output;

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    products = (reader.DataReaderMapToList<UserCart>()).ToList();
                }

                TotalAmount = (decimal)cmd.Parameters["@TotalAmount"].Value;

            }
            return new Tuple<List<UserCart>, decimal>(products, TotalAmount);
        }
        public async Task<Transtatus> DeleteProductInCart(int ProductId, int CustomerId)
        {
            Transtatus transtatus = new Transtatus();
            using (var connection = new SqlConnection(Connection.ConnectionString))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "dbo.DeleteProductInCart";
                await connection.OpenAsync();

                cmd.Parameters.AddWithValue("@CustomerId", CustomerId);
                cmd.Parameters.AddWithValue("@productId", ProductId);
                cmd.Parameters.Add("@message", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@code", SqlDbType.Int).Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                transtatus.Message = (string)cmd.Parameters["@message"].Value;
                transtatus.Code = (int)cmd.Parameters["@code"].Value;
            }
            return transtatus;
        }
    }
}
