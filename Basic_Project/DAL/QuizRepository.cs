using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Basic_Project.Data;
using Basic_Project.Models;

namespace Basic_Project.DAL
{
    public class QuizRepository : IQuizRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<QuizRepository> _logger;

        public QuizRepository(ApplicationDbContext context, ILogger<QuizRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Quiz>> GetAllQuizzesAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all quizzes from database");
                return await _context.Quizzes
                    .Include(q => q.Questions)
                    .ToListAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error fetching all quizzes from database");
                throw;
            }
        }

        public async Task<Quiz?> GetQuizByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation("Fetching quiz with ID: {Id}", id);
                return await _context.Quizzes
                    .Include(q => q.Questions)
                    .FirstOrDefaultAsync(q => q.Id == id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error fetching quiz with ID: {Id}", id);
                throw;
            }
        }

        public async Task CreateQuizAsync(Quiz quiz)
        {
            try
            {
                _logger.LogInformation("Creating new quiz: {Title}", quiz.Title);
                _context.Quizzes.Add(quiz);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Quiz created successfully with ID: {Id}", quiz.Id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error creating quiz: {Title}", quiz.Title);
                throw;
            }
        }

        public async Task UpdateQuizAsync(Quiz quiz)
        {
            try
            {
                _logger.LogInformation("Updating quiz with ID: {Id}", quiz.Id);
                
                var existingQuiz = await _context.Quizzes
                    .Include(q => q.Questions)
                    .FirstOrDefaultAsync(q => q.Id == quiz.Id);

                if (existingQuiz == null)
                {
                    _logger.LogError("Quiz not found for update with ID: {Id}", quiz.Id);
                    throw new Exception($"Quiz with ID {quiz.Id} not found");
                }

                existingQuiz.Title = quiz.Title;
                existingQuiz.Description = quiz.Description;
                
                _context.Questions.RemoveRange(existingQuiz.Questions);
                existingQuiz.Questions = quiz.Questions;

                await _context.SaveChangesAsync();
                _logger.LogInformation("Quiz updated successfully: {Id}", quiz.Id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error updating quiz with ID: {Id}", quiz.Id);
                throw;
            }
        }

        public async Task DeleteQuizAsync(int id)
        {
            try
            {
                _logger.LogInformation("Deleting quiz with ID: {Id}", id);
                
                var quiz = await _context.Quizzes.FindAsync(id);
                if (quiz == null)
                {
                    _logger.LogError("Quiz not found for deletion with ID: {Id}", id);
                    throw new Exception($"Quiz with ID {id} not found");
                }

                _context.Quizzes.Remove(quiz);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Quiz deleted successfully: {Id}", id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error deleting quiz with ID: {Id}", id);
                throw;
            }
        }
    }
}