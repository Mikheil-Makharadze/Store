using Microsoft.AspNetCore.Http;
using Web.Models.DTO;
using Web.Models.ShopingCart;

namespace Web.Cart
{
    public class ShoppingCart
    {
        private ISession _session;

        public string ShoppingCartId { get; set; }
        public List<ShoppingCartItem> ShoppingCartItems { get; set; }

        private ShoppingCart(ISession session)
        {
            _session = session;
            ShoppingCartItems = new List<ShoppingCartItem>();
        }

        //public static ShoppingCart GetShoppingCart(IServiceProvider services)
        //{
        //    var session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;

        //    var cart = session.Get<ShoppingCart>("Cart");

        //    if (cart == null)
        //    {
        //        cart = new ShoppingCart(session);
        //        session.Set("Cart", cart);
        //    }

        //    return cart;
        //}

        //public void AddItemToCart(ProductDTO Product)
        //{
        //    var cartItem = ShoppingCartItems.FirstOrDefault(n => n.product.Id == Product.Id);

        //    if (cartItem == null)
        //    {
        //        cartItem = new ShoppingCartItem()
        //        {
        //            product = Product,
        //            Amount = 1
        //        };

        //        ShoppingCartItems.Add(cartItem);
        //    }
        //    else
        //    {
        //        cartItem.Amount++;
        //    }

        //    _session.Set("Cart", this);
        //}

        //public void RemoveItemFromCart(ProductDTO product)
        //{
        //    var cartItem = ShoppingCartItems.FirstOrDefault(n => n.product.Id == product.Id);

        //    if (cartItem != null)
        //    {
        //        if (cartItem.Amount > 1)
        //        {
        //            cartItem.Amount--;
        //        }
        //        else
        //        {
        //            ShoppingCartItems.Remove(cartItem);
        //        }
        //    }

        //    _session.Set("Cart", this);
        //}

        public List<ShoppingCartItem> GetShoppingCartItems()
        {
            return ShoppingCartItems;
        }

        public double GetShoppingCartTotal()
        {
            return ShoppingCartItems.Sum(n => n.product.Price * n.Amount);
        }

        public void ClearShoppingCart()
        {
            ShoppingCartItems.Clear();
            _session.Remove("Cart");
        }
    }
}
