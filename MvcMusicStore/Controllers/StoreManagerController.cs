using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcMusicStore.Models;


namespace MvcMusicStore.Controllers
{
    public class StoreManagerController : Controller
    {
        MusicStoreEntities storeDB = new MusicStoreEntities();

        //
        // GET: /StoreManager/

        public ActionResult Index()
        {
            var albums = storeDB.Albums.Include("Genre").Include("Artist");
            return View(albums.ToList<Album>());
        }

        // GET: /StoreManager/Edit/5

        public ActionResult Edit(int id)
        {
            Album album = storeDB.Albums.Find(id);
            ViewBag.GenreId = new SelectList(storeDB.Genres, "GenreId", "Name", album.AlbumId);
            ViewBag.ArtistId = new SelectList(storeDB.Artist, "ArtistId", "Name", album.ArtistId);
            return View(album);

        }

        [HttpPost]
        public ActionResult Edit(Album album)
        {
            if(ModelState.IsValid)
            {
                storeDB.Entry(album).State = System.Data.Entity.EntityState.Modified;
                storeDB.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.GenreId = new SelectList(storeDB.Genres, "GenreId", "Name", album.AlbumId);
            ViewBag.ArtistId = new SelectList(storeDB.Artist, "ArtistId", "Name", album.ArtistId);
            return View(album);
        

        }

        // GET: /StoreManager/Details/5
        public ActionResult Details(int id)
        {
            Album album = storeDB.Albums.Find(id);
            return View(album);
        }

        public ActionResult Delete(int id)
        {
            Album album = storeDB.Albums.Find(id);
            return View(album);
        }


        [HttpPost,ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Album album = storeDB.Albums.Find(id);
            storeDB.Albums.Remove(album);
            storeDB.SaveChanges();
            return RedirectToAction("Index");
        }

        //[HttpPost]
        //public ActionResult Delete(int id, FormCollection collection)
        //{
        //    try
        //    {
        //          Album album = storeDB.Albums.Find(id);
        //          storeDB.Albums.Remove(album);
        //          return RedirectToAction("Index");
        //    }
        //    catch (Exception)
        //    {

        //        return View();
        //    }
        //}

        public ActionResult Create()
        {
            ViewBag.GenreId = new SelectList(storeDB.Genres, "GenreId", "Name");
            ViewBag.ArtistId = new SelectList(storeDB.Artist, "ArtistId", "Name");
            return View();
        }


        [HttpPost]
        public ActionResult Create(Album album)
        {
            if (ModelState.IsValid)
            {
                storeDB.Albums.Add(album);
                storeDB.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.GenreId = new SelectList(storeDB.Genres, "GenreId", "Name",album.AlbumId);
            ViewBag.ArtistId = new SelectList(storeDB.Artist, "ArtistId", "Name",album.ArtistId);
            return View();
        }
    }
}
