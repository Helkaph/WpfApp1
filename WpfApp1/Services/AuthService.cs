using WpfApp1.Services;
using System.Threading.Tasks;
using WpfApp1.Models;
using WpfApp1.Data;
namespace WpfApp1.Services
{
    public class AuthService
    {
        private AppDbContext _context = new AppDbContext();
        
        public string Authenticate(string username, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == username);

            if (user == null)
                return "Введены неправильные данные. Проверьте соответствие введённых данных";

            if (user.IsBlocked || (user.LastLogin.HasValue && user.LastLogin.Value < DateTime.Now.AddMonths(-1)))
            {
                user.IsBlocked = true;
                _context.SaveChanges();
                return "Вы заблокированы. Обратитесь к администратору";
            }

            if (!PasswordService.VerifyPassword(password, user.HashPassword))
            {
                user.FailAttempts++;
                if (user.FailAttempts >= 3)
                    user.IsBlocked = true;
                _context.SaveChanges();
                return "Введены неправильные данные. Проверьте соответствие введённых данных";
            }

            if (!_context.Users.Any(u => u.UserName == username && PasswordService.VerifyPassword(password, u.HashPassword)))
            {
                user.FailAttempts++;
                if (user.FailAttempts >= 3)
                    user.IsBlocked = true;
                _context.SaveChanges();
                return "Введены неправильные данные. Проверьте соответствие введённых данных";
            }

            user.FailAttempts = 0;
            user.LastLogin = DateTime.Now;
            _context.SaveChanges();

            return user.IsFirstPassword ? "CHANGE" : user.Role;
        }

        public string ChangePassword(string username, string currentPassword, string newPassword, string confirmPassword)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == username);
            if (user == null || !PasswordService.VerifyPassword(currentPassword, user.HashPassword))
                return "Неверные текущий пароль";

            if (newPassword != confirmPassword)
                return "Новый пароль и пароль для подтверждения не совпадают";

            if (!PasswordService.IsValidPassword(newPassword))
                return "Пароль должен содержать заглавные и строчные буквы, цифры, спецсимволы. \nДлина пароля:  16-32 символа";

            user.HashPassword = PasswordService.HashPassword(newPassword);
            user.IsFirstPassword = false;
            _context.SaveChanges();
            return "Пароль успешно сохранён";
        }

        public string RegisterUser(string username, string password, string role = "User")
        {
            if (_context.Users.Any(u => u.UserName == username))
                return "Данный пользователь уже существует";

            if (PasswordService.IsValidPassword(password))
                return "Пароль должен содержать заглавные и строчные буквы, цифры, спецсимволы. \nДлина пароля:  16-32 символа";

            var user = new User
            {
                UserName = username,
                HashPassword = PasswordService.HashPassword(password),
                Role = role,
                IsFirstPassword = true
            };

            _context.Users.Add(user);
            _context.SaveChanges();
            return "Пользователь успешно добавлен";
        }

        public string UnblockUser(string username)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == username);
            if (user == null) return "Пользователь не найден";

            user.IsBlocked = false;
            user.FailAttempts = 0;
            _context.SaveChanges();
            return "Пользователь разблокирован";
        }

        public List<User> GetAllUsers() => _context.Users.ToList();
    }
}
