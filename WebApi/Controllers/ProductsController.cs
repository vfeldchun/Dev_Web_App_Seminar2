using Microsoft.AspNetCore.Mvc;
using System.Text;
using WebApi.Abstraction;
using WebApi.Dto;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet(template: "get_cache_stats")]
        public ActionResult<string?> GetCacheStats()
        {
            var fileName = _productRepository.GetCacheStatistics("Product");
            var resultLink = "https://" + Request.Host.ToString() + "/cache_static/" + fileName;

            return Ok(resultLink);
        }

        [HttpPost(template: "add_product")]
        public ActionResult AddProduct([FromBody] ProductDto productDto)
        {            
            try
            {   
                _productRepository.AddProduct(productDto);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }               

        [HttpGet(template: "get_products")]
        public ActionResult<IEnumerable<ProductDto>> GetProducts()
        {
            try
            {    
                var list = _productRepository.GetProducts();
                return Ok(list);                
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet(template: "get_products_csv_file")]
        public FileContentResult GetProductsCsvFile()
        {
            var csvString = _productRepository.GetProductsCsvString();
            var contentFile = File(new UTF8Encoding().GetBytes(csvString), "text/csv", "product_list.csv");
            return contentFile;
        }

        //[HttpPost(template: "addprice")]
        //public ActionResult AddPrice(long price, int productId)
        //{
        //    if (price < 0) return StatusCode(500);
        //    try
        //    {
        //        using (var ctx = new ProductContext())
        //        {
        //            if (ctx.Products.Count(x => x.Id == productId) == 0)
        //            {
        //                return StatusCode(409);
        //            }
        //            else
        //            {
        //                var modifiedProduct = ctx.Products.FirstOrDefault(x => x.Id == productId);
        //                modifiedProduct!.Price = price;
        //                ctx.SaveChanges();
        //            }
        //        }

        //        return Ok();
        //    }
        //    catch
        //    {
        //        return StatusCode(500);
        //    }
        //}

        //[HttpDelete(template: "deleteproduct")]
        //public ActionResult DeleteProduct(int id)
        //{
        //    try
        //    {
        //        using (var ctx = new ProductContext())
        //        {
        //            if (ctx.Products.Count(x => x.Id == id) > 0)
        //            {
        //                var deleteRec = ctx.Products.FirstOrDefault(x => x.Id == id);
        //                ctx.Products.Remove(deleteRec!);
        //                ctx.SaveChanges();

        //                return Ok();
        //            }

        //            return StatusCode(404);
        //        }
        //    }
        //    catch
        //    {
        //        return StatusCode(500);
        //    }
        //}        
    }
}
