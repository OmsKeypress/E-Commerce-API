namespace E_Commerce.Model
{
    public class GetAllProducts
    {
        public int PID { get; set; }
        public string? Product_Name { get; set; }
        public string? Brand { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountedPrice { get; set; }
        public string? Category { get; set; }
        public string? Quentity { get; set; }
    }
    public class Pagination
    {
        public int RowCount { get; set; } = 0;
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public string? SearchText { get; set; }
    }

    public class AddProduct
    {
        public string? Product_Name { get; set; }
        public string? Brand { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountedPrice { get; set; }
        public string? Category { get; set; }
        public string? Quentity { get; set; }
        public int CreatedBy { get; set; }
    }
    public class DeleteProduct
    {
        public int? PID { get; set; }
        public double? Test { get; set; }
    }
    public class UpdateProduct
    {
        public int PID { get; set; }
        public string? Product_Name { get; set; }
        public string? Brand { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountedPrice { get; set; }
        public string? Category { get; set; }
        public string? Quentity { get; set; }
        public int ModifiedBy { get; set; }
    }

    public class GetCustomerIPAndDeviceDetail
    {
        public string? IP { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? TimeZone { get; set; }
        public string? OsName { get; set; }
        public string? OsVersion { get; set; }
        public string? CustomerName { get; set; }
        public string? SystemName { get; set; }
        public string? SystemManufacturer { get; set; }
        public string? SystemModel { get; set; }
        public int SystemCores { get; set; }
        public string? Browser { get; set; }
    }
    public class IpInfo
    {
        public string? status { get; set; }
        public string? country { get; set; }
        public string? regionName { get; set; }
        public string? city { get; set; }
        public string? zip { get; set; }
        public decimal lat { get; set; }
        public decimal lon { get; set; }
        public string? timezone { get; set; }
        public string? isp { get; set; }
        public bool mobile { get; set; }
    }
    public class Authentication
    {
        public int Id { get; set; }
        public string? Password { get; set; }
    }
    public class CustomerRegesterReqModel
    {
        public string? CustomerName { get; set; }
        public string? Password { get; set; }
        public string? MobileNo { get; set; }
        public string? Email { get; set; }
        public string? Info { get; set; }
        public string? Ip { get; set; }
    }
    public class UserDeviceDetail
    {
        public string? browser { get; set; }
        public string? version { get; set; }
        public string? os { get; set; }
        public string? osVesrion { get; set; }
        public string? device { get; set; }
        public string? brand { get; set; }
        public bool isSpider { get; set; }
        public string? model { get; set; }
        public string? ipv4 { get; set; }
        public string? ipv6 { get; set; }
    }
    public class ValidateTokenRequest
    {
        public string? Token { get; set; }
        public int UserId { get; set; }
        public string? UserName { get; set; }
    }
    public class AddProductInCartModel
    {
        public int ProductId { get; set;}
        public int Quentity { get; set; }
    }
    public class UserCart
    {
        public string Product_Name{ get; set;}
        public decimal DiscountedPrice { get; set; }
        public int Quentity { get; set; }
        public decimal Amount { get; set; }
    }
}