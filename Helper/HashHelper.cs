using System.Security.Cryptography;
using System.Text;

namespace Hotel_Management.Helper
{
    public static class HashHelper
    {
        public static string HashUserName(string username)
        {
            using var sha = SHA256.Create(); // This Line is used to create an object that performs SHA256 Hashing algorithm
            var bytes = Encoding.UTF8.GetBytes(username); // This is used to get bytes of the userName cuz Hashnig works with bytes not string
            var hashedBytes = sha.ComputeHash(bytes); // That Makes the hashing algorithm on those bytes and convert it into hashed bytes
            return Convert.ToBase64String(hashedBytes); //That converts those hashedbytes into readable characters that can be stored in db
        }
    }
}
