using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using System.Text;
using WebApi.Abstraction;
using WebApi.Dto;
using WebApi.Models;

namespace WebApi.Repo
{
    public class ProductRepository : IProductRepository
    {
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private ProductContext _context;

        public ProductRepository(IMapper mapper, IMemoryCache cache, ProductContext context)
        {
            _mapper = mapper;
            _cache = cache;
            _context = context;

        }
        public void AddGroup(ProductGroupDto group)
        {            
            using (_context)
            {
                if (_context.ProductGroups.Any(x => x.Name.ToLower() == group.Name.ToLower()))
                    throw new Exception("Group with such name exists!");

                _context.ProductGroups.Add(_mapper.Map<ProductGroup>(group));
                _context.SaveChanges();
                _cache.Remove("groups");
            }
        }

        public void AddProduct(ProductDto product)
        {
            using (_context)
            {
                if (_context.Products.Any(x => x.Name.ToLower() == product.Name.ToLower() && x.Price == product.Price))
                    throw new Exception("Product with such name and price exists!");
                    
                _context.Products.Add(_mapper.Map<Product>(product));
                _context.SaveChanges();
                _cache.Remove("products");
            }
        }

        public IEnumerable<ProductGroupDto> GetGroups()
        {
            if (_cache.TryGetValue("groups", out List<ProductGroupDto> groupCacheList))
            {
                return groupCacheList;
            }

            using (_context)
            {        
                var groupList = _context.ProductGroups.Select(_mapper.Map<ProductGroupDto>).ToList();
                _cache.Set("groups", groupList, TimeSpan.FromMinutes(30));
                return groupList;
            }
                
        }

        public IEnumerable<ProductDto> GetProducts()
        {
            if (_cache.TryGetValue("products", out List<ProductDto> productCacheList))
            {
                return productCacheList;
            }

            using (_context)
            {
                var productList = _context.Products.Select(_mapper.Map<ProductDto>).ToList();
                _cache.Set("products", productList, TimeSpan.FromMinutes(30));
                return productList;
            }
        }

        // Создает статический файл со статистикой использования кеша и возвращаем имя созданного файла
        public string GetCacheStatistics(string fileTitle)
        {
            var currentStats = _cache.GetCurrentStatistics();
            
            var sb = new StringBuilder();
            sb.AppendLine("{");
            sb.AppendLine("CurrentEntryCount: " + currentStats.CurrentEntryCount);
            sb.AppendLine("CurrentEstimatedSize: " + currentStats.CurrentEstimatedSize);
            sb.AppendLine("TotalHits: " + currentStats.TotalHits);
            sb.AppendLine("TotalMisses: " + currentStats.TotalMisses);
            sb.AppendLine("}");            

            string fileName = null;

            fileName = fileTitle + DateTime.Now.ToBinary().ToString() + ".json";

            File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "CacheStaticFiles", fileName), sb.ToString());            

            return fileName;
            
        }

        // Возвращает строку в формате CSV со списком продуктов
        public string GetProductsCsvString()
        {
            var content = "";

            if (_cache.TryGetValue("products", out List<ProductDto> products))
            {
                content = GetCsvString(products);
            }
            else
            {
                content = GetCsvString(GetProducts());                
            }

            return content;
        }

        private string GetCsvString(IEnumerable<ProductDto> products)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Id;" + "Product Name;" + "Product Price;" + "Product Description");

            foreach (var product in products)
            {
                sb.AppendLine(product.Id + ";" + product.Name + ";" + product.Price + ";" + product.Description);
            }

            return sb.ToString();
        }
    }
}
