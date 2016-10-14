using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcMusicStore.Models;
using MvcMusicStore.ViewModels;
namespace MvcMusicStore.Controllers
{
    public class ShoppingCartController : Controller
    {
        MusicStoreEntities storeDB = new MusicStoreEntities();
        //
        // GET: /ShoppingCart/

        public ActionResult Index()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()

            };

            return View(viewModel);
        }

        public ActionResult AddToCart(int id)
        {
            var addalbum = storeDB.Albums.Single(album => album.AlbumId == id);

            var cart = ShoppingCart.GetCart(this.HttpContext);

            cart.AddtoCart(addalbum);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult RemoveFromCart(int id)
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            
            Album album=storeDB.Carts.Single(cartitem => cartitem.RecordId == id).Album;
            string albumName = album.Title;
            int itemcount = cart.ReomoveFromCart(id);
            var result = new ShoppingCartRemoveViewModel
            {
                Message = Server.HtmlEncode(albumName) + " has been removed from your shopping cart.",
                CartTotal = cart.GetTotal(),
                CartCount = cart.GetCount(),
                ItemCount = itemcount,
                DeleteId = id

            };
            return Json(result);

        }

        [ChildActionOnly]
        public ActionResult CartSummary()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            ViewData["CartCount"] = cart.GetCount();
            return PartialView("CartSummary");
        }

    }
}
