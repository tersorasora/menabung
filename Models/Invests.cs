using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models;

public class Invests
{
    [Key]
    public int invest_id { get; set; }
    public string invest_type { get; set; } = string.Empty;
    public decimal quantity { get; set; } = 0;
    public string quantity_type { get; set; } = string.Empty;
    public decimal price { get; set; } = 0;
    public DateTime date { get; set; } = DateTime.UtcNow;
    public bool is_sell { get; set; } = false;
    public int user_id { get; set; }

    [ForeignKey("user_id")]
    public User? user { get; set; }
}