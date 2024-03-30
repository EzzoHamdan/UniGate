using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using UniversityGate.Models;
using System.Web.UI;
using System.Web.Services.Description;

namespace UniversityGate.Controllers
{
    [Authorize(Roles = "Student")]

    public class StudentController : Controller
    {
        private UniGateDatabaseEntities db = new UniGateDatabaseEntities();
        // GET: Student
        public ActionResult Index()
        {
            Student student = db.Students.Where(m=>m.studentID == User.Identity.Name).FirstOrDefault();
            int hour = DateTime.Now.Hour;
            if (hour < 12)
            {
                ViewBag.message = "Good Morning";
            }
            else
            {
                ViewBag.message = "Good Evening";
            }

            return View(student);
        }
        
        public ActionResult Details()
        {
            Student student = db.Students.Find(db.Students.Where(m=> m.studentID == User.Identity.Name).FirstOrDefault().student_id);
            if (student != null)
            {
                return View(student);
            }
            else {
                return View("Error");
            }
            
        }
        public ActionResult Edit()
        {
            Student student = db.Students.Where(m => m.studentID == User.Identity.Name).FirstOrDefault();
           
            return View(student);
        }


        [HttpPost]
        public ActionResult Edit(Student student)
        {
            if (ModelState.IsValid)
            {
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                
                return RedirectToAction("Details");
            }
            return View();
        }

        public ActionResult Enroll(int? id, string message)
        {
            var enrolledCourses = db.EnrolledCourses.Where(m => m.student_id == id).ToList();
            ViewBag.studentId = id;

            if (enrolledCourses.Any())
            {
                int? creditHours = enrolledCourses
                    .Where(m => m.Cours != null)
                    .Sum(m => m.Cours.credit_hours);

                ViewBag.creditHours = creditHours ?? 0;
            }
            else
            {
                ViewBag.creditHours = 0;
            }

            ViewBag.errorMessage = message;
            ViewBag.registeredCourses = enrolledCourses;

            return View(db.Courses.ToList());
        }

        public ActionResult AddCourse(int? id, int c_id, int credit)
        {
            EnrolledCours exist = db.EnrolledCourses.Where(m => m.course_id == c_id && m.student_id == id).FirstOrDefault();
            if (exist == null && credit < 18)
            {
                EnrolledCours x = new EnrolledCours();
                x.student_id = (int)id;
                x.course_id = c_id;
                db.EnrolledCourses.Add(x);
                Grade addToGradeTable = new Grade();
                addToGradeTable.student_id = (int)id;
                addToGradeTable.course_id = c_id;
                addToGradeTable.grade_value = 0;
                addToGradeTable.professor_id = 1;
                db.Grades.Add(addToGradeTable);
                db.SaveChanges();
                return RedirectToAction("Enroll", new { id = id });
            }
            else
            {
                if (credit >= 18)
                {
                    return RedirectToAction("Enroll", new { id = id, message = "You are limited to enrolling in a maximum of 18 credit hours!" });
                }
                else
                {
                    return RedirectToAction("Enroll", new { id = id, message = "You have successfully enrolled in this course!" });
                }


            }

        }
        public ActionResult DeleteCourse(int? id, int enrollId)
        {
            if (id != null)
            {
                db.EnrolledCourses.Remove(db.EnrolledCourses.FirstOrDefault(c => c.student_id == id && c.enrollment_id == enrollId));
                db.SaveChanges();
                return RedirectToAction("Enroll", new { id = id });
            }
            else
            {
                return View("Error");
            }

        }
        public ActionResult cDetails(int? id, int sid)
        {
            ViewBag.sid = sid;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cours cours = db.Courses.Find(id);
            if (cours == null)
            {
                return HttpNotFound();
            }
            return View("coursDetails", cours);
        }




        List<QuizModel> quizModels = ProfessorController.GetQuizModels();
        // GET: Student
        public ActionResult IndexPortal()
        {
            if (quizModels != null && quizModels.Count > 0)
            {
                return View(quizModels);
            }
            else
            {
                return View();
            }
        }
        public ActionResult Take(int id)
        {
            QuizModel quizModel = quizModels.Find(q => q.ID == id);
            if (quizModel.isTaken == false) { return View(quizModel); }
            else { return RedirectToAction("Taken"); }


        }
        [HttpPost]
        public ActionResult Submit(int id, int[] answers)
        {
            QuizModel quizModel = quizModels.Find(q => q.ID == id);
            quizModel.isTaken = true;

            if (quizModel != null && answers != null)
            {
                quizModel.score = CalculateScore(quizModel, answers);

            }

            return RedirectToAction("Result", new { id = quizModel.ID });
        }

        private int CalculateScore(QuizModel quizModel, int[] answers)
        {
            int score = 0;

            for (int i = 0; i < quizModel.Questions.Count; i++)
            {
                // Compare the submitted answer with the correct answer
                if (answers[i] == quizModel.Questions[i].CorrectOption)
                {
                    // Increase the score if the answer is correct
                    score++;
                }
            }


            return score;
        }
        public ActionResult Result(int id)
        {
            QuizModel quizModel = quizModels.Find(q => q.ID == id);

            if (quizModel != null)
            {
                double? score = quizModel.score;

                return View(score);
            }

            // Handle the case where the quiz model is not found
            return HttpNotFound(); // You can customize this to handle the scenario appropriately
        }


    }
}