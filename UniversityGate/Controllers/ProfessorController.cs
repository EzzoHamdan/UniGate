using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UniversityGate.Models;
using PagedList;
using PagedList.Mvc;
using System.Web.UI;

namespace UniversityGate.Controllers
{
    [Authorize(Roles ="Professor")]
    public class ProfessorController : Controller
    {
        private UniGateDatabaseEntities db = new UniGateDatabaseEntities();
        // GET: Professor

        public ActionResult Index()

        {
            Professor professor = db.Professors.Where(m=>m.professorID == User.Identity.Name).FirstOrDefault();
            ViewBag.professorName = professor.first_name+" "+professor.last_name;
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
        public ActionResult ShowGrades(string chooes, int? page)
        {
            int id = db.Professors.Where(m => m.professorID == User.Identity.Name).FirstOrDefault().professor_id;
            var list = db.Grades.Where(m => m.professor_id == id);
            List<Cours> courses = db.Courses.Where(m => m.professor_id == id).ToList();
            //ViewBag.professorId = id;
            ////ViewBag.chooes = chooes;
            ViewBag.professorCourses = new SelectList(courses, "course_name", "course_name");
            if (chooes == null)
            {
                return View(list.ToList().ToPagedList(page ?? 1, 5));
            }
            else
            {
                return View(list.Where(m => m.Cours.course_name == chooes).ToList().ToPagedList(page ?? 1, 5));
            }

        }

        [HttpPost]
        public ActionResult EditGrade(int gradeId, string editedGrade, int page,string chooes)
        {
            var gradeToUpdate = db.Grades.Find(gradeId);
            if (gradeToUpdate != null)
            {
                gradeToUpdate.grade_value = Convert.ToDecimal(editedGrade);
                db.SaveChanges();
            }
            return RedirectToAction("ShowGrades", new { page = page,chooes = chooes });
        }
        [HttpPost]
        public ActionResult Delete(int? gradeId, string chooes, int page)
        {
            Grade grade = db.Grades.Find(gradeId);

            if (grade != null)
            {
                db.Grades.Remove(grade);
                db.SaveChanges();
            }

            return RedirectToAction("ShowGrades", new { chooes = chooes, page = page });
        }
        [HttpPost]
        public ActionResult Cancel(int page)
        {
            return RedirectToAction("ShowGrades", new {  page = page });
        }





        static List<QuizModel> quizModels = new List<QuizModel>();

        // GET: Professor
        public ActionResult IndexPortal()
        {
            if (quizModels.Count == 0)
            {
                return View();
            }
            else
            {
                return View(quizModels);
            }
        }


        [HttpGet]
        public ActionResult CreateQuiz(int noOfQuestions)
        {

            QuizModel quizModel = new QuizModel();
            quizModel.NoOfQuestions = noOfQuestions;
            quizModel.Questions = Enumerable.Range(1, noOfQuestions).Select(i => new QuestionModel()).ToList();

            return View(quizModel);
        }

        [HttpPost]
        public ActionResult CreateQuiz(QuizModel quizModel)
        {
            quizModel.ID = ++QuizModel.no;
            quizModels.Add(quizModel);
            return RedirectToAction("IndexPortal");
        }

        public ActionResult Details(int id)
        {
            QuizModel quizModel = quizModels.Find((i => i.ID == id));
            return View(quizModel);
        }
        public static List<QuizModel> GetQuizModels()
        {
            return quizModels;
        }

    }

}