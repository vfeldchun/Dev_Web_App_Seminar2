using WebApi.Models;

namespace WebApi.Dto
{
    public class StoreDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }        
        public int Quantity { get; set; }
    }
}
