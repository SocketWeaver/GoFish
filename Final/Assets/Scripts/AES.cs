using System;
using System.IO;
using System.Security.Cryptography;

namespace GoFish
{
    // https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.aes?view=netframework-4.8
    
    public static class AES
    {
        internal static byte[] EncryptAES128(byte[] input, byte[] key)
        {
            if (input == null || input.Length <= 0)
                throw new ArgumentNullException();
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException();

            byte[] encrypted;
            byte[] iv;
            // Create an Aes object
            // with the specified key
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                iv = aes.IV;

                ICryptoTransform encryptor = aes.CreateEncryptor();
                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        cs.Write(input, 0, input.Length);
                        cs.Close();
                    }

                    encrypted = msEncrypt.ToArray();
                }
            }
            // concate the iv and the encrypted input
            // we will read the iv in the decryption method.
            byte[] ret = new byte[encrypted.Length + iv.Length];
            Buffer.BlockCopy(iv, 0, ret, 0, iv.Length);
            Buffer.BlockCopy(encrypted, 0, ret, iv.Length, encrypted.Length);

            return ret;
        }

        internal static byte[] DecryptAES128(byte[] input, byte[] key)
        {
            if (input == null || input.Length <= 0)
                throw new ArgumentNullException();
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException();

            // read the iv from the input
            byte[] iv = new byte[16];
            Buffer.BlockCopy(input, 0, iv, 0, 16);

            // read the encrypted data
            byte[] encrypted = new byte[input.Length - 16];
            Buffer.BlockCopy(input, 16, encrypted, 0, encrypted.Length);

            byte[] decrypted;

            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                //ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                ICryptoTransform decryptor = aes.CreateDecryptor();
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Write))
                    {
                        cs.Write(encrypted, 0, encrypted.Length);
                        cs.Close();
                    }

                    decrypted = ms.ToArray();
                }
            }

            return decrypted;
        }
    }
}