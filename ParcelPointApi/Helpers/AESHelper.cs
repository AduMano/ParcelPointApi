using System.Security.Cryptography;
using System.Text;

namespace ParcelPointApi.Helpers
{
    public class AESHelper
    {
        // AES Encryption (Optional for security)
        public static byte[] EncryptAES(byte[] data, string key)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key);
                aesAlg.IV = new byte[16]; // Use a secure IV in production
                aesAlg.Mode = CipherMode.CBC;

                using (var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV))
                {
                    return encryptor.TransformFinalBlock(data, 0, data.Length);
                }
            }
        }


        // AES Decryption (If encryption was used)
        public static byte[] DecryptAES(byte[] data, string key)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key);
                aesAlg.IV = new byte[16];
                aesAlg.Mode = CipherMode.CBC;

                using (var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV))
                {
                    return decryptor.TransformFinalBlock(data, 0, data.Length);
                }
            }
        }
    }
}
