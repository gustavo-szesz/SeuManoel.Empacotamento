using br.seumanoel.empacotamento.api.Data;
using br.seumanoel.empacotamento.api.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace br.seumanoel.empacotamento.api.Services
{
    public class AuthService
    {
        #region Constructor
        private readonly AppDbContext _context;

        public AuthService(AppDbContext context)
        {
            _context = context;
        }
        #endregion

        #region AuthMethod
        public async Task<User?> AuthenticateAsync(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
                return null;

            using var sha = SHA256.Create();
            var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            var hashBase64 = Convert.ToBase64String(hash);

            if (user.Password == hashBase64)
                return user;

            return null;
        }
        #endregion
    }
}
