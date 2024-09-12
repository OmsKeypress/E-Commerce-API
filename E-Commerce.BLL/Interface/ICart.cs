using E_Commerce.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.BLL.Interface
{
    public interface ICart
    {
        Task<Transtatus> AddProductInCart(AddProductInCartModel model, int CustomerId);
        Task<Tuple<List<UserCart>, decimal>> CustomerCart(int CustomerId);
        Task<Transtatus> DeleteProductInCart(int ProductId, int CustomerId);
    }
}
