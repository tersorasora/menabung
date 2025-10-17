using System.ComponentModel.DataAnnotations;

namespace Models;

public class User
{
    [Key]
    public int user_id { get; set; }
    public string username { get; set; } = string.Empty;
    public string nickname { get; set; } = string.Empty;
    public string password { get; set; } = string.Empty;
    public decimal balance { get; set; } = 0;
}