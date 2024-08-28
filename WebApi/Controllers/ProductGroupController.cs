using Microsoft.AspNetCore.Mvc;
using WebApi.Abstraction;
using WebApi.Dto;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductGroupController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductGroupController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet(template: "get_cache_stats")]
        public ActionResult<string?> GetCacheStats()
        {
            var fileName = _productRepository.GetCacheStatistics("Group");
            var resultLink = "https://" + Request.Host.ToString() + "/cache_static/" + fileName;

            return Ok(resultLink);
        }

        [HttpPost(template: "add_group")]
        public ActionResult AddGroup([FromBody] ProductGroupDto productGroupDto)
        {
            try
            {                
                _productRepository.AddGroup(productGroupDto);
                return Ok();
            }
            catch (Exception ex) 
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet(template: "get_groups")]
        public ActionResult<IEnumerable<ProductGroupDto>> GetGroups()
        {
            try
            {                
                var list = _productRepository.GetGroups();
                return Ok(list);                
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        //[HttpDelete(template: "deletegroup")]
        //public ActionResult DeleteGroup(int id)
        //{
        //    try
        //    {
        //        using (var ctx = new ProductContext())
        //        {
        //            if (ctx.ProductGroups.Count(x => x.Id == id) > 0)
        //            {
        //                var deleteRec = ctx.ProductGroups.FirstOrDefault(x => x.Id == id);
        //                ctx.ProductGroups.Remove(deleteRec!);
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
