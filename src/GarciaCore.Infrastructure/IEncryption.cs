using System;
using System.Collections.Generic;
using System.Text;

namespace GarciaCore.Infrastructure
{
    public interface IEncryption
    {
        string Encrypt(string plainText);
        string Decrypt(string cipherText);
        string CreateOneWayHash(string inValue, HashAlgorithm HashAlgorithm = HashAlgorithm.MD5);
    }
}
