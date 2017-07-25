using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NiCATPortal.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        [Route]
        [Route("Home")]
        public ActionResult Index()
        {
            return View();
        }
        [Route("Home/Onama")]
        public ActionResult About()
        {
            ViewBag.Message = "Opis NiCAT Portala.";

            return View();
        }
        [Route("Home/Kontakt")]
        public ActionResult Contact()
        {
            ViewBag.Message = "Kontakt stranica.";

            return View();
        }
    }
}