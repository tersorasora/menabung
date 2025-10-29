using Models;

namespace Repositories;

public interface IToDoListRepository
{
    Task AddToDoList(ToDoList toDoList);
    Task<ToDoList?> GetToDoListByIdAsync(int id);
    Task<List<ToDoList>> GetAllToDoListsAsync();
    Task<List<ToDoList>> GetAllToDoListsByUserIdAsync(int? userId);
    Task EditToDoList(ToDoList toDoList);
    Task DeleteToDoListAsync(int id);
}