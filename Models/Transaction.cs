using System.ComponentModel.DataAnnotations;

namespace Models;

public class Transaction
{
    [Key]
    public int transaction_id { get; set; }
    public string transaction_type { get; set; } = string.Empty;
    public float transaction_nominal { get; set; } = 0.0f;
    public DateTime transaction_time { get; set; } = DateTime.UtcNow;
    public int user_id { get; set; }
    public User? user { get; set; }
}