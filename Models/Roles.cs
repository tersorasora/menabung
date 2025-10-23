using System.ComponentModel.DataAnnotations;

namespace Models;

public class Roles
{
    [Key]
    public int role_id { get; set; }
    public string role_name { get; set; } = string.Empty;
}