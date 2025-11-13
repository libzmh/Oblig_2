using QuizAPI.Models;

namespace QuizAPI.DAL
{
    public interface IQuizRepository
    {
        Task<List<Quiz>> GetAllQuizzesAsync();
        Task<Quiz?> GetQuizByIdAsync(int id);
        Task CreateQuizAsync(Quiz quiz);
        Task UpdateQuizAsync(Quiz quiz);
        Task DeleteQuizAsync(int id);
    }
}