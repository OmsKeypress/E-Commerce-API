using E_Commerce.BLL.Interface;
using E_Commerce.DAL;
using E_Commerce.Model;

namespace E_Commerce.BLL
{
    public class EcommerceService : IEcommerce
    {
        EcommerceRepository? ecommerceRepository;
        public async Task<List<GetAllProducts>> GetAllProducts()
        {
            using (ecommerceRepository = new EcommerceRepository())
            {
                return await ecommerceRepository.GetAllProducts();
            }
        }
        public async Task<Tuple<List<GetAllProducts>, Transtatus, int>> GetProductByCategory(Pagination model)
        {
            using (ecommerceRepository = new EcommerceRepository())
            {
                return await ecommerceRepository.GetProductByCategory(model);
            }
        }
        public async Task<Transtatus> AddProduct(AddProduct model)
        {
            using (ecommerceRepository = new EcommerceRepository())
            {
                return await ecommerceRepository.AddProduct(model);
            }
        }
        public async Task<Transtatus> UpdateProduct(UpdateProduct model)
        {
            using (ecommerceRepository = new EcommerceRepository())
            {
                return await ecommerceRepository.UpdateProduct(model);
            }
        }
        public async Task<Transtatus> DeleteProduct(DeleteProduct model)
        {
            using (ecommerceRepository = new EcommerceRepository())
            {
                return await ecommerceRepository.DeleteProduct(model);
            }
        }
        public async Task<Transtatus> LoginCustomer(Authentication model)
        {
            using (ecommerceRepository = new EcommerceRepository())
            {
                return await ecommerceRepository.LoginCustomer(model);
            }
        }
        public async Task<Transtatus> CustomerRegester(CustomerRegesterReqModel model)
        {
            using (ecommerceRepository = new EcommerceRepository())
            {
                return await ecommerceRepository.CustomerRegester(model);
            }
        }
    }
}