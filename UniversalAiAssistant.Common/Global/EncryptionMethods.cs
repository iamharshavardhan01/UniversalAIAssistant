using System.Security.Cryptography;
using System.Text;

namespace DigiSoft.Common.Global
{
    public class EncryptionMethods
    {
        protected static string Encipher(string Password)
        {
            string key = "abcdefghijklmnopqrstuvwxyz1234567890";
            byte[] bytesBuff = Encoding.Unicode.GetBytes(Password);
            using (System.Security.Cryptography.Aes aes = System.Security.Cryptography.Aes.Create())
            {
#pragma warning disable SYSLIB0041 // Type or member is obsolete
                Rfc2898DeriveBytes crypto = new(key,
                    "Ivan Mhdvedhx"u8.ToArray());
#pragma warning restore SYSLIB0041 // Type or member is obsolete
                aes.Key = crypto.GetBytes(32);
                aes.IV = crypto.GetBytes(16);
                using MemoryStream mStream = new();
                using (CryptoStream cStream = new(mStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cStream.Write(bytesBuff, 0, bytesBuff.Length);
                    cStream.Close();
                }
                Password = Convert.ToBase64String(mStream.ToArray());
            }
            return Password;
        }

        protected static string Decipher(string Password)
        {
            string key = "abcdefghijklmnopqrstuvwxyz1234567890";
            Password = Password.Replace(" ", "+");
            byte[] bytesBuff = Convert.FromBase64String(Password);
            using (Aes aes = Aes.Create())
            {
#pragma warning disable SYSLIB0041 // Type or member is obsolete
                Rfc2898DeriveBytes crypto = new(key,
                    "Ivan Mhdvedhx"u8.ToArray());
#pragma warning restore SYSLIB0041 // Type or member is obsolete
                aes.Key = crypto.GetBytes(32);
                aes.IV = crypto.GetBytes(16);
                using MemoryStream mStream = new();
                using (CryptoStream cStream = new(mStream, aes.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cStream.Write(bytesBuff, 0, bytesBuff.Length);
                    cStream.Close();
                }
                Password = Encoding.Unicode.GetString(mStream.ToArray());
            }
            return Password;
        }
    }
}
