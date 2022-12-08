using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Garcia.Application.Contracts.Infrastructure;
using Microsoft.Extensions.Options;

namespace Garcia.Infrastructure
{
    public class Encryption : IEncryption
    {
        private readonly string initVector = "E6C15F23B1E94008";
        private readonly string passPhrase = "3D4124B2A8748E4B55A997D1C2B4C40";
        private const int keysize = 256;

        public Encryption(IOptions<EncryptionSettings> settings)
        {
            initVector = settings.Value?.InitVector ?? initVector;
            passPhrase = settings.Value?.PassPharse ?? passPhrase;
        }

        public Encryption()
        {
        }

        public string Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(initVector)
                    || initVector.Length != 16)
            {
                throw new InvalidOperationException("Init vector should be 16 characters long");
            }

            byte[] initVectorBytes = Encoding.UTF8.GetBytes(initVector);
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            var password = new PasswordDeriveBytes(passPhrase, null);
            byte[] keyBytes = password.GetBytes(keysize / 8);
            var symmetricKey = Aes.Create();
            symmetricKey.Mode = CipherMode.CBC;
            var encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
            using var memoryStream = new MemoryStream();
            using var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cryptoStream.FlushFinalBlock();
            byte[] cipherTextBytes = memoryStream.ToArray();
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
            var password = new PasswordDeriveBytes(passPhrase, null);
            byte[] keyBytes = password.GetBytes(keysize / 8);
            var symmetricKey = Aes.Create();
            symmetricKey.Mode = CipherMode.CBC;
            var decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
            using var memoryStream = new MemoryStream(cipherTextBytes);
            using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];
            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
        }

        public string CreateOneWayHash(string inValue, Application.HashAlgorithm HashAlgorithm = Application.HashAlgorithm.MD5)
        {
            try
            {
                HashAlgorithm hash = null;

                switch (HashAlgorithm)
                {
                    case Application.HashAlgorithm.SHA1:
                        hash = SHA1.Create();
                        break;
                    case Application.HashAlgorithm.MD5:
                        hash = MD5.Create();
                        break;
                }

                var result = hash.ComputeHash(Encoding.UTF8.GetBytes(inValue));
                return Convert.ToBase64String(result);
            }
            catch
            {
                throw;
            }
        }
    }
}