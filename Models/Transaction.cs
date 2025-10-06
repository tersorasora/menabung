namespace Models;

public class Transaction
{
    public int transaction_id { get; set; }
    public string transaction_type { get; set; }
    public float transaction_nominal { get; set; }
    public DateTime transaction_time { get; set; } = DateTime.UtcNow;
    public int user_id { get; set; }
    public User? user { get; set; }
}