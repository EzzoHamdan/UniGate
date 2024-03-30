using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using UniversityGate.Models;
using PagedList;
using PagedList.Mvc;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Configuration;

namespace UniversityGate.Controllers
{
    [Authorize(Roles = "Admin")]

    public class StudentsController : Controller
    {
        public static int number = 1;
        private UniGateDatabaseEntities db = new UniGateDatabaseEntities();


        public ActionResult Index(int? page, string name, string option)
        {

            if (name != null && name.Equals("Student Name", StringComparison.OrdinalIgnoreCase))
            {

                return View(db.Students.OrderBy(m => m.first_name).ToPagedList(page ?? 1, 8));
            }
            else if (name != null && name.Equals("Stuednt ID", StringComparison.OrdinalIgnoreCase))
            {
                return View(db.Students.OrderBy(m => m.studentID).ToPagedList(page ?? 1, 8));
            }
            else if (name != null && name.Equals("Cumulative GPA", StringComparison.OrdinalIgnoreCase))
            {
                return View(db.Students.OrderBy(m => m.Grades.Average(grade => grade.grade_value)).ToPagedList(page ?? 1, 8));
            }
            else if (name != null && option == "ID")
            {
                return View(db.Students.Where(t => t.studentID.Contains(name)).ToList().ToPagedList(page ?? 1, 8));
            }
            else if (name != null && option == "First Name")
            {
                return View(db.Students.Where(t => t.first_name.Contains(name)).ToList().ToPagedList(page ?? 1, 8));
            }
            else if (name != null && option == "Last Name")
            {
                return View(db.Students.Where(t => t.last_name.Contains(name)).ToList().ToPagedList(page ?? 1, 8));
            }
            else if (name != null && option == "Email")
            {
                return View(db.Students.Where(t => t.email.Contains(name)).ToList().ToPagedList(page ?? 1, 8));
            }
            else if (option == "Cancel")
            {
                return View(db.Students.ToList().ToPagedList(page ?? 1, 8));
            }
            else
            {
                return View(db.Students.ToList().ToPagedList(page ?? 1, 8));
            }
        }


        // GET:
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return View("Error");
            }
            return View(student);
        }

        // GET: Students/Create
        public ActionResult Add()
        {
            return View();
        }
        


        [HttpPost]
        public ActionResult Add(Student student, HttpPostedFileBase file)
        {
            //string fileName = $"{student.studentID}.jpg";
            student.studentID = System.DateTime.Now.Year+""+System.DateTime.Now.Month+""+ (++number) + ""+number;
            if (student.email == null) { 
                student.email = student.studentID+"@universitygate.edu";
            }
            student.UserRole = 3;
            if (file != null)
            {
                file.SaveAs(Server.MapPath("~/Images/"+file.FileName));
                student.ImagePath = file.FileName;
            }
            if (ModelState.IsValid)
            {
                db.Students.Add(student);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(student);
        }

        // GET: Students/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return View("Error");
            }
            return View(student);
        }


        [HttpPost]
        public ActionResult Edit(Student student, HttpPostedFileBase file)
        {
            UniGateDatabaseEntities tempDb = new UniGateDatabaseEntities();
            Student oldImage = tempDb.Students.Find(student.student_id);
            if (file != null)
            {
                file.SaveAs(Server.MapPath("~/Images/"+file.FileName));
                student.ImagePath = file.FileName;
                if (student.ImagePath != null)
                {
                    System.IO.File.Delete(Server.MapPath("~/Images/" + oldImage.ImagePath));
                }
            }
            else
            {
                student.ImagePath = oldImage.ImagePath;
            }

            if (ModelState.IsValid)
            {
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(student);
        }

        // GET: Students/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return View("Error");
            }
            return View(student);
        }


        [HttpPost]
        public ActionResult Delete(int id)
        {
            Student student = db.Students.Find(id);
            var enrolledCoursesToDelete = db.EnrolledCourses.Where(ec => ec.student_id == id);
            db.EnrolledCourses.RemoveRange(enrolledCoursesToDelete);
            var gradesToDelete = db.Grades.Where(g => g.student_id == id);
            db.Grades.RemoveRange(gradesToDelete);
            db.Students.Remove(student);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}