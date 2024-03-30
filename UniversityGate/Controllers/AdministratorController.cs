using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UniversityGate.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdministratorController : Controller
    {
        // GET: Administrator
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            int hour = DateTime.Now.Hour;
            if (hour < 12)
            {
                ViewBag.message = "Good Morning";
            }
            else
            {
                ViewBag.message = "Good Evening";
            }
            return View();
        }
    }
}