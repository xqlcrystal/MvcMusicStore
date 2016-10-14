using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcMusicStore.Models
{
    public partial class ShoppingCart
    {

        MusicStoreEntities storeDB = new MusicStoreEntities();

        string ShoppingCartId { get; set; }

        public const string CartSessionKey = "CartId";

        public static ShoppingCart GetCart(HttpContextBase context)
        {
            var cart = new ShoppingCart();
            cart.ShoppingCartId = cart.GetCartId(context);
            return cart;
        }

        public static ShoppingCart GetCart(Controller controller)
        {
            return GetCart(controller.HttpContext);
        }

        public void AddtoCart(Album album)
        {
            var cartItem = storeDB.Carts.SingleOrDefault(c => c.CartId == ShoppingCartId
                && c.AlbumId == album.AlbumId);
            if (cartItem == null)
            {
                cartItem = new Cart
                {
                    CartId = ShoppingCartId,
                    AlbumId = album.AlbumId,
                    Count = 1,
                    DateCreated = DateTime.Now

                };
                storeDB.Carts.Add(cartItem);
            }
            else
            {
                cartItem.Count++;
            }

            storeDB.SaveChanges();

        }

        public int ReomoveFromCart(int id)
        {
            var cartItem = storeDB.Carts.SingleOrDefault(c => c.CartId == ShoppingCartId && c.AlbumId == id);
            int itemCount = 0;
            if (cartItem != null)
            {
                if (cartItem.Count > 1)
                {
                    cartItem.Count--;
                    itemCount = cartItem.Count;
                }
                else
                {
                    storeDB.Carts.Remove(cartItem);
                }
                storeDB.SaveChanges();
            }

            return itemCount;
        }

        public void EmptyCart()
        {
            var cartitems = storeDB.Carts.Where(cart => cart.CartId == ShoppingCartId);
            foreach (var cartitem in cartitems)
            {
                storeDB.Carts.Remove(cartitem);
            }

            storeDB.SaveChanges();
        }

        public List<Cart> GetCartItems()
        {
            return storeDB.Carts.Where(cart => cart.CartId == ShoppingCartId).ToList();
        }

        public int GetCount()
        {
            int? count = (from cartitems in storeDB.Carts
                          where cartitems.CartId == ShoppingCartId
                          select (int?)cartitems.Count).Sum();

            return count ?? 0;
        }

        public decimal GetTotal()
        {
            decimal? total = (from cartitems in storeDB.Carts
                             where cartitems.CartId == ShoppingCartId
                             select (int?)cartitems.Count * cartitems.Album.Price).Sum();
            return total ?? decimal.Zero;
        }

        public int CreateOrder(Order order)
        {
            decimal orderTotal = 0;
            var cartItems = GetCartItems();
            foreach (var cartItem in cartItems)
            {
                var orderDetail = new OrderDetail
                {
                    AlbumId = cartItem.AlbumId,
                    OrderId = order.OrderId,
                    UnitPrice = cartItem.Album.Price,
                    Quantity = cartItem.Count

                };
                orderTotal += (cartItem.Count * cartItem.Album.Price);
                storeDB.OrderDetails.Add(orderDetail);
                    
            }

            order.Total = orderTotal;
            storeDB.SaveChanges();
            EmptyCart();
            return order.OrderId;
        }

        private string GetCartId(HttpContextBase context)
        {
            if (context.Session[CartSessionKey] == null)
            {
                if (string.IsNullOrWhiteSpace(context.User.Identity.Name))
                {
                    context.Session[CartSessionKey] = context.User.Identity.Name;
                }
                else
                {
                    Guid tempCartId = Guid.NewGuid();
                    context.Session[CartSessionKey] = tempCartId;
                }
            }
            return context.Session[CartSessionKey].ToString();
        }

        public void MigrateCart(string username)
        {
            var shoppingCart = storeDB.Carts.Where(cart => cart.CartId == ShoppingCartId);
            foreach (var cart in shoppingCart)
            {
                cart.CartId = username;
            }
            storeDB.SaveChanges();
        }
    }
}