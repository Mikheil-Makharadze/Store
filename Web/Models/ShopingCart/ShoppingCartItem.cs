using Web.Models.DTO;

namespace Web.Models.ShopingCart
{
    public class ShoppingCartItem
    {
        public int Id { get; set; }

        public ProductDTO product { get; set; }
        public int Amount { get; set; }


        public string ShoppingCartId { get; set; }
    }
}
