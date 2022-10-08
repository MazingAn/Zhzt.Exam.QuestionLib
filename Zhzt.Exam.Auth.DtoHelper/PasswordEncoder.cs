using System.Security.Cryptography;
using System.Text;

namespace Zhzt.Exam.Auth.DomainDtoHelper
{
    public static class PasswordEncoder
    {
        public static string EncryptPassword(string password)
        {
            string ret;
            MD5 md5 = MD5.Create();
            byte[] palindata = Encoding.Default.GetBytes(password);
            byte[] encryptdata = md5.ComputeHash(palindata);
            ret = Convert.ToBase64String(encryptdata);
            return ret;
        }
    }
}
