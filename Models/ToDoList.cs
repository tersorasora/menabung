using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models;

public class ToDoList
{
    [Key]
    public int todolist_id { get; set; }
    public string objective { get; set; } = string.Empty;
    public bool todolist_status { get; set; } = false;
    public DateTime todolist_date { get; set; } = DateTime.UtcNow;
    public int user_id { get; set; }
    
    [ForeignKey("user_id")]
    public User? user { get; set; }
}