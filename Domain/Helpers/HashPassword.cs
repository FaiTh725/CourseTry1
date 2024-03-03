using System.Security.Cryptography;
using System.Text;

namespace CourseTry1.Domain.Helpers
{
    public class HashPassword
    {
        public static string HashPas(string password)
        {
            MD5 md5 = MD5.Create();

            byte[] bytes = Encoding.ASCII.GetBytes(password);
            byte[] hash = md5.ComputeHash(bytes);

            return Convert.ToBase64String(hash);
        }
    }
}
