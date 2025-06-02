using System.Security.Cryptography.X509Certificates;

namespace WpfApp1.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string HashPassword { get; set; }
        public int Role { get; set; } = 1;
        public bool IsBlocked { get; set; } = false;
        public int FailAttempts { get; set; } = 0;
        public DateTime? LastLogin {  get; set; }
        public bool IsFirstPassword { get; set; } = true;
    }
}
