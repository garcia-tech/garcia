using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace GarciaCore.Infrastructure
{
    public class Encryption : IEncryption
    {
        private static string initVector = "E6C15F23B1E94008";
        private static string passPhrase = "3D4124B2A8748E4B55A997D1C2B4C40";
        private const int keysize = 256;

        public string Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(initVector)
                    || initVector.Length != 16)
            {
                throw new InvalidOperationException("Init vector should be 16 characters long");
            }

            byte[] initVectorBytes = Encoding.UTF8.GetBytes(initVector);
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
            byte[] keyBytes = password.GetBytes(keysize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cryptoStream.FlushFinalBlock();
            byte[] cipherTextBytes = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();
            return Convert.ToBase64String(cipherTextBytes);
        }

        public string Decrypt(string cipherText)
        {
            if (string.IsNullOrEmpty(initVector)
                    || initVector.Length != 16)
            {
                throw new InvalidOperationException("Init vector should be 16 characters long");
            }

            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
            byte[] keyBytes = password.GetBytes(keysize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = new MemoryStream(cipherTextBytes);
            CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];
            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
        }

        public string CreateOneWayHash(string inValue, HashAlgorithm HashAlgorithm = HashAlgorithm.MD5)
        {
            var result = new byte[inValue.Length];

            try
            {
                System.Security.Cryptography.HashAlgorithm hash = null;

                switch (HashAlgorithm)
                {
                    case HashAlgorithm.SHA1:
                        hash = new SHA1CryptoServiceProvider();
                        break;
                    case HashAlgorithm.MD5:
                        hash = new MD5CryptoServiceProvider();
                        break;
                }

                result = hash.ComputeHash(System.Text.Encoding.UTF8.GetBytes(inValue));
                return Convert.ToBase64String(result);
            }
            catch
            {
                throw;
            }
        }
    }
}
