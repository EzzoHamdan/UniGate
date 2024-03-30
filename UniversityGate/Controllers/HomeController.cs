using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using UniversityGate.Models;

namespace UniversityGate.Controllers
{
    [Authorize]

    public class HomeController : Controller
    {
        private UniGateDatabaseEntities db = new UniGateDatabaseEntities();
        [AllowAnonymous]

        public ActionResult Home()
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
        [AllowAnonymous]

        public ActionResult Login()
        {
            return View();

        }

        [AllowAnonymous]
        [HttpPost]

        public ActionResult Login(User user, string ReturnUrl)
        {
            bool isValidId = true;
            bool isValidPassword = true;

            if (user.Type.Equals("Student"))
            {
                Student student = db.Students.FirstOrDefault(m => m.studentID == user.Id);

                if (student == null)
                {
                    isValidId = false;
                    ModelState.AddModelError("Id", "Invalid Student ID");
                }
                else if (!student.studentPassword.Equals(user.Password))
                {
                    isValidPassword = false;
                    ModelState.AddModelError("Password", "Invalid Password");
                }
            }
            else if (user.Type.Equals("Professor"))
            {
                Professor professor = db.Professors.FirstOrDefault(m => m.professorID == user.Id);

                if (professor == null)
                {
                    ModelState.AddModelError("Id", "Invalid Professor ID");
                    isValidId = false;

                }
                else if (!professor.professorPassword.Equals(user.Password))
                {
                    ModelState.AddModelError("Password", "Invalid Password");
                    isValidPassword = false;
                }
            }
            else if (user.Type.Equals("Administrator"))
            {
                if (user.Id != "admin")
                {
                    ModelState.AddModelError("Id", "Invalid Admin ID");
                    isValidId = false;
                    
                }
                else if(user.Password != "admin") {
                    ModelState.AddModelError("Password", "Invalid Password");
                    isValidPassword = false;
                }
            }
            else
            {
                isValidId = false;
                isValidPassword = false;
                ModelState.AddModelError("Password", "Invalid Password");
                ModelState.AddModelError("Id", "Invalid ID");
            }

            if (ModelState.IsValid && isValidId && isValidPassword)
            {
                FormsAuthentication.SetAuthCookie(user.Id, false);

                if (Url.IsLocalUrl(ReturnUrl))
                {
                    return Redirect(ReturnUrl);
                }
                else
                {
                    if (user.Type.Equals("Student"))
                        return RedirectToAction("Index", "Student");
                    else if (user.Type.Equals("Professor"))
                        return RedirectToAction("Index", "Professor");
                    else if (user.Type.Equals("Administrator"))
                        return RedirectToAction("Index", "Administrator");
                }
            }

            ModelState.AddModelError("LoginError", "Invalid login attempt");
            return View();
        }




        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Home", "Home");
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}