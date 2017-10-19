using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InteticsTestApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }


        public ActionResult About()
        {
            ViewBag.Message = "ASP.NET MVC";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult GetName()
        {

            if (User.Identity.IsAuthenticated)
            {
           
                return ViewBag.name= User.Identity.Name;
            }
            else
            {
                return ViewBag.name = "";

            }
        }
    }
}