using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UniversityGate.Models
{
    public class QuizModel
    {
        public bool isTaken = false;
        public int score { get; set; }
        public string QuizName { get; set; }
        public static int no { get; set; }
        public int ID { get; set; }
        public int NoOfQuestions { get; set; }
        public List<QuestionModel> Questions { get; set; }

        public QuizModel()
        {
            // Initialize the Questions list in the constructor
            Questions = new List<QuestionModel>();
        }
    }

    public class QuestionModel
    {
        public string QuestionText { get; set; }
        public List<string> Options { get; set; }
        public int CorrectOption { get; set; }

        public QuestionModel()
        {
            // Initialize the Options list in the constructor
            Options = new List<string> { "", "", "", "" };
        }
    }
}