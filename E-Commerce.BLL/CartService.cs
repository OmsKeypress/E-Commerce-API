using E_Commerce.BLL.Interface;
using E_Commerce.DAL;
using E_Commerce.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.BLL
{
    public class CartService :ICart 
    {
        CartRepository? cartrepository;
        public async Task<Transtatus> AddProductInCart(AddProductInCartModel model, int CustomerId)
        {
            using (cartrepository = new CartRepository())
            {
                return await cartrepository.AddProductInCart(model, CustomerId);
            }
        }
        public async Task<Tuple<List<UserCart>, decimal>> CustomerCart(int CustomerId)
        {
            using (cartrepository = new CartRepository())
            {
                return await cartrepository.CustomerCart(CustomerId);
            }
        }
        public async Task<Transtatus> DeleteProductInCart(int ProductId, int CustomerId)
        {
            using (cartrepository = new CartRepository())
            {
                return await cartrepository.DeleteProductInCart(ProductId, CustomerId);
            }
        }
    }
}
