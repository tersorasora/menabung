using Models;
using System.Text;

namespace Services;

public interface IToDoListService
{
    Task<bool> CreateToDoListAsync(string objective, int userId);
    Task<ToDoList?> GetToDoListByIdAsync(int id);
    Task<List<ToDoList>> GetAllToDoListsAsync(int? userId);
    Task<bool> UpdateToDoListAsync(int tdlID, string objective, bool isCompleted);
    Task<bool> DeleteToDoListAsync(int id);
}