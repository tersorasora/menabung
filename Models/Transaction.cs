using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models;

public class Transaction
{
    [Key]
    public int transaction_id { get; set; }
    public string description { get; set; } = string.Empty;
    public string transaction_type { get; set; } = string.Empty;
    public decimal transaction_nominal { get; set; } = 0;
    public DateTime transaction_date { get; set; } = DateTime.UtcNow;
    public int user_id { get; set; }

    [ForeignKey("user_id")]
    public User? user { get; set; }
}