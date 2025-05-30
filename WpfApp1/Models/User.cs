using System.Security.Cryptography.X509Certificates;

namespace WpfApp1.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string HashPassword { get; set; } = string.Empty;
        public string Role { get; set; } = "User";
        public bool IsBlocked { get; set; } = false;
        public int FailAttempts { get; set; } = 0;
        public DateTime? LastLogin {  get; set; }
        public bool IsFirstPassword { get; set; } = true;
    }
}
