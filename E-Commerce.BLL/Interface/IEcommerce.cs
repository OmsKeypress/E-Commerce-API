using E_Commerce.Model;

namespace E_Commerce.BLL.Interface
{
    public interface IEcommerce
    {
        Task<List<GetAllProducts>> GetAllProducts();
        Task<Tuple<List<GetAllProducts>, Transtatus, int>> GetProductByCategory(Pagination model);
        Task<Transtatus> AddProduct(AddProduct model);
        Task<Transtatus> UpdateProduct(UpdateProduct model);
        Task<Transtatus> DeleteProduct(DeleteProduct model);
        Task<Transtatus> LoginCustomer(Authentication model);
        Task<Transtatus> CustomerRegester(CustomerRegesterReqModel model);
    }
}
