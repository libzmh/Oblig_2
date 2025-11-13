using Microsoft.AspNetCore.Mvc;
using QuizAPI.Models;
using QuizAPI.DAL;

namespace QuizAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuizController : ControllerBase
    {
        private readonly IQuizRepository _repository;
        private readonly ILogger<QuizController> _logger;

        public QuizController(IQuizRepository repository, ILogger<QuizController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        // GET: api/Quiz
        [HttpGet]
        public async Task<IActionResult> GetAllQuizzes()
        {
            try
            {
                _logger.LogInformation("Fetching all quizzes");
                var quizzes = await _repository.GetAllQuizzesAsync();
                return Ok(quizzes);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error fetching all quizzes");
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/Quiz/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetQuizById(int id)
        {
            try
            {
                _logger.LogInformation("Fetching quiz with ID: {Id}", id);
                var quiz = await _repository.GetQuizByIdAsync(id);

                if (quiz == null)
                {
                    _logger.LogWarning("Quiz not found with ID: {Id}", id);
                    return NotFound($"Quiz with ID {id} not found");
                }

                return Ok(quiz);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error fetching quiz with ID: {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        // POST: api/Quiz
        [HttpPost]
        public async Task<IActionResult> CreateQuiz([FromBody] Quiz quiz)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid quiz data submitted");
                    return BadRequest(ModelState);
                }

                _logger.LogInformation("Creating new quiz: {Title}", quiz.Title);
                await _repository.CreateQuizAsync(quiz);
                _logger.LogInformation("Quiz created successfully with ID: {Id}", quiz.Id);

                return CreatedAtAction(nameof(GetQuizById), new { id = quiz.Id }, quiz);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error creating quiz");
                return StatusCode(500, "Internal server error");
            }
        }

        // PUT: api/Quiz/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateQuiz(int id, [FromBody] Quiz quiz)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid quiz data for update");
                    return BadRequest(ModelState);
                }

                if (id != quiz.Id)
                {
                    return BadRequest("ID mismatch");
                }

                _logger.LogInformation("Updating quiz: {Id}", id);

                foreach (var question in quiz.Questions)
                {
                    question.QuizId = id;
                }

                await _repository.UpdateQuizAsync(quiz);
                _logger.LogInformation("Quiz updated successfully: {Id}", id);

                return Ok(quiz);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error updating quiz, ID: {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        // DELETE: api/Quiz/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuiz(int id)
        {
            try
            {
                _logger.LogInformation("Deleting quiz: {Id}", id);
                await _repository.DeleteQuizAsync(id);
                _logger.LogInformation("Quiz deleted successfully: {Id}", id);

                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error deleting quiz, ID: {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        // POST: api/Quiz/5/submit
        [HttpPost("{id}/submit")]
        public async Task<IActionResult> SubmitQuiz(int id, [FromBody] Dictionary<int, List<string>> answers)
        {
            try
            {
                _logger.LogInformation("Submitting quiz answers for ID: {QuizId}", id);

                var quiz = await _repository.GetQuizByIdAsync(id);
                if (quiz == null)
                {
                    _logger.LogWarning("Quiz not found with ID: {QuizId}", id);
                    return NotFound($"Quiz with ID {id} not found");
                }

                int totalScore = 0;
                int maxScore = 0;
                var questionResults = new List<object>();

                foreach (var question in quiz.Questions)
                {
                    maxScore += question.Points;

                    var userAnswers = answers.ContainsKey(question.Id) ? answers[question.Id] : new List<string>();
                    var correctAnswers = System.Text.Json.JsonSerializer.Deserialize<List<string>>(question.CorrectAnswersJson) ?? new List<string>();

                    bool isCorrect = question.Type switch
                    {
                        QuestionType.Checkbox => userAnswers.OrderBy(a => a).SequenceEqual(correctAnswers.OrderBy(a => a)),
                        QuestionType.ShortAnswer => userAnswers.Count > 0 && correctAnswers.Any(ca => ca.Equals(userAnswers[0], StringComparison.OrdinalIgnoreCase)),
                        _ => userAnswers.Count > 0 && userAnswers[0] == correctAnswers.FirstOrDefault()
                    };

                    if (isCorrect)
                    {
                        totalScore += question.Points;
                    }

                    questionResults.Add(new
                    {
                        questionText = question.QuestionText,
                        userAnswers = userAnswers,
                        correctAnswers = correctAnswers,
                        isCorrect = isCorrect,
                        points = question.Points
                    });
                }

                var result = new
                {
                    quizId = id,
                    quizTitle = quiz.Title,
                    totalScore = totalScore,
                    maxScore = maxScore,
                    percentage = maxScore > 0 ? (double)totalScore / maxScore * 100 : 0,
                    questionResults = questionResults
                };

                _logger.LogInformation("Quiz submitted successfully. Score: {Score}/{MaxScore}", totalScore, maxScore);

                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error submitting quiz answers, ID: {QuizId}", id);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}