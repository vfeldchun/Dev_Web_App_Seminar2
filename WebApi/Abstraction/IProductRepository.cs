using Microsoft.Extensions.Caching.Memory;
using WebApi.Dto;

namespace WebApi.Abstraction
{
    public interface IProductRepository
    {
        public void AddGroup(ProductGroupDto group);
        public IEnumerable<ProductGroupDto> GetGroups();
        public void AddProduct(ProductDto product);
        public IEnumerable<ProductDto> GetProducts();
        public string GetProductsCsvString();
        public string GetCacheStatistics(string fileTitle);
    }
}
