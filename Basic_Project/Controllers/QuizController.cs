using Microsoft.AspNetCore.Mvc;
using Basic_Project.Models;
using Basic_Project.ViewModels;
using Basic_Project.DAL;

namespace Basic_Project.Controllers
{
    public class QuizController : Controller
    {
        private readonly IQuizRepository _repository;
        private readonly ILogger<QuizController> _logger;

        public QuizController(IQuizRepository repository, ILogger<QuizController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        // GET: /Quiz/Create
        public IActionResult Create()
        {
            try
            {
                _logger.LogInformation("Opening create quiz page");
                return View(new QuizCreateViewModel());
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error opening create quiz page");
                return RedirectToAction("Error", "Home");
            }
        }

        // POST: /Quiz/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(QuizCreateViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid quiz data submitted");
                    return View(model);
                }

                _logger.LogInformation("Creating new quiz: {Title}", model.Title);

                var quiz = new Quiz
                {
                    Title = model.Title,
                    Description = model.Description,
                    CreatedDate = DateTime.Now,
                    Questions = model.Questions.Select(q => new Question
                    {
                        QuestionText = q.QuestionText,
                        OptionA = q.OptionA,
                        OptionB = q.OptionB,
                        OptionC = q.OptionC,
                        OptionD = q.OptionD,
                        CorrectAnswer = q.CorrectAnswer,
                        Points = q.Points
                    }).ToList()
                };
                
                await _repository.CreateQuizAsync(quiz);

                _logger.LogInformation("Quiz created successfully: {Title}", model.Title);
                return RedirectToAction("Index", "Home");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error creating quiz");
                ModelState.AddModelError("", "An error occurred while creating the quiz");
                return View(model);
            }
        }

        // GET: /Quiz/Update/5
        public async Task<IActionResult> Update(int id)
        {
            try
            {
                _logger.LogInformation("Opening update quiz page for ID: {Id}", id);

                var quiz = await _repository.GetQuizByIdAsync(id);
                if (quiz == null)
                {
                    _logger.LogWarning("Quiz not found with ID: {Id}", id);
                    return NotFound();
                }

                var model = new QuizCreateViewModel
                {
                    Title = quiz.Title,
                    Description = quiz.Description,
                    Questions = quiz.Questions.Select(q => new QuestionCreateViewModel
                    {
                        QuestionText = q.QuestionText,
                        OptionA = q.OptionA,
                        OptionB = q.OptionB,
                        OptionC = q.OptionC,
                        OptionD = q.OptionD,
                        CorrectAnswer = q.CorrectAnswer,
                        Points = q.Points
                    }).ToList()
                };

                return View(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error loading quiz for update, ID: {Id}", id);
                return RedirectToAction("Error", "Home");
            }
        }

        // POST: /Quiz/Update/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, QuizCreateViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid quiz data for update");
                    return View(model);
                }

                _logger.LogInformation("Updating quiz: {Id}", id);

                var quiz = new Quiz
                {
                    Id = id,
                    Title = model.Title,
                    Description = model.Description,
                    Questions = model.Questions.Select(q => new Question
                    {
                        QuestionText = q.QuestionText,
                        OptionA = q.OptionA,
                        OptionB = q.OptionB,
                        OptionC = q.OptionC,
                        OptionD = q.OptionD,
                        CorrectAnswer = q.CorrectAnswer,
                        Points = q.Points,
                        QuizId = id
                    }).ToList()
                };
                
                await _repository.UpdateQuizAsync(quiz);

                _logger.LogInformation("Quiz updated successfully: {Id}", id);
                TempData["Success"] = "Quiz updated successfully!";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error updating quiz, ID: {Id}", id);
                ModelState.AddModelError("", "An error occurred while updating the quiz");
                return View(model);
            }
        }

        // POST: /Quiz/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _logger.LogInformation("Deleting quiz: {Id}", id);

                await _repository.DeleteQuizAsync(id);

                _logger.LogInformation("Quiz deleted successfully: {Id}", id);
                TempData["Success"] = "Quiz deleted successfully!";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error deleting quiz, ID: {Id}", id);
                TempData["Error"] = "An error occurred while deleting the quiz";
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: /Quiz/Take/5
        public async Task<IActionResult> Take(int id)
        {
            try
            {
                _logger.LogInformation("Opening quiz taking page for ID: {Id}", id);

                var quiz = await _repository.GetQuizByIdAsync(id);
                if (quiz == null)
                {
                    _logger.LogWarning("Quiz not found with ID: {Id}", id);
                    return NotFound();
                }

                return View(quiz);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error loading quiz for taking, ID: {Id}", id);
                return RedirectToAction("Error", "Home");
            }
        }

        // POST: /Quiz/SubmitQuiz
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitQuiz(int quizId, Dictionary<int, string> answers)
        {
            try
            {
                _logger.LogInformation("Submitting quiz answers for ID: {QuizId}", quizId);

                var quiz = await _repository.GetQuizByIdAsync(quizId);
                if (quiz == null)
                {
                    _logger.LogWarning("Quiz not found with ID: {QuizId}", quizId);
                    return NotFound();
                }

                int totalScore = 0;
                int maxScore = 0;
                var questionResults = new List<QuestionResultViewModel>();

                foreach (var question in quiz.Questions)
                {
                    maxScore += question.Points;
                    
                    var userAnswer = answers.ContainsKey(question.Id) ? answers[question.Id] : "";
                    var isCorrect = userAnswer == question.CorrectAnswer;
                    
                    if (isCorrect)
                    {
                        totalScore += question.Points;
                    }

                    questionResults.Add(new QuestionResultViewModel
                    {
                        QuestionText = question.QuestionText,
                        UserAnswer = userAnswer,
                        CorrectAnswer = question.CorrectAnswer,
                        IsCorrect = isCorrect,
                        Points = question.Points
                    });
                }

                var resultViewModel = new QuizResultViewModel
                {
                    QuizId = quizId,
                    QuizTitle = quiz.Title,
                    TotalScore = totalScore,
                    MaxScore = maxScore,
                    Percentage = maxScore > 0 ? (double)totalScore / maxScore * 100 : 0,
                    QuestionResults = questionResults
                };

                _logger.LogInformation("Quiz submitted successfully. Score: {Score}/{MaxScore}", totalScore, maxScore);
                
                return View("Result", resultViewModel);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error submitting quiz answers, ID: {QuizId}", quizId);
                TempData["Error"] = "An error occurred while submitting your answers";
                return RedirectToAction("Take", new { id = quizId });
            }
        }
    }
}