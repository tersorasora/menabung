using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models;

public class User
{
    [Key]
    public int user_id { get; set; }
    public string username { get; set; } = string.Empty;
    public string nickname { get; set; } = string.Empty;
    public string password { get; set; } = string.Empty;
    public bool banned { get; set; } = false;
    public decimal balance { get; set; } = 0;
    public int role_id { get; set; }

    [ForeignKey("role_id")]
    public Roles? role { get; set; }
}