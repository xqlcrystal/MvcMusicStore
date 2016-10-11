﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcMusicStore.Models;

namespace MvcMusicStore.Controllers
{
    public class StoreController : Controller
    {
        MusicStoreEntities storeDB = new MusicStoreEntities();
        //
        // GET: /Store/

        public ActionResult Index()
        {
            //var genres = new List<Genre>
            //{
            //    new Genre{Name="Disco"},
            //    new Genre{Name="Jazz"},
            //    new Genre{Name="Rock"}

            //};

            var genres = storeDB.Genres.ToList<Genre>();
            return View(genres);
        }


        public ActionResult Browse(string genre)
        {
            var genreModel = new Genre { Name = genre };
            return View(genreModel);
        }


        //Get:Store/Details/5
        public ActionResult Details(int id)
        {
            var album = new Album { Title = "Album" + id };
            return View(album);
        }

    }
}
