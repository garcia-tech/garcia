namespace Garcia.Application.Contracts.Infrastructure
{
    public interface IEncryption
    {
        /// <summary>
        /// Encrypts the plain text entered.
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns>Encrypted <see cref="string"/>.</returns>
        string Encrypt(string plainText);
        /// <summary>
        /// Decrypts the cipher text entered.
        /// </summary>
        /// <param name="cipherText"></param>
        /// <returns>Decrypted <see cref="string"/></returns>
        string Decrypt(string cipherText);
        /// <summary>
        /// Creates one way hash of the value entered.
        /// Created hash cannot be decrypted.
        /// </summary>
        /// <param name="inValue">The value to be hashed.</param>
        /// <param name="HashAlgorithm"></param>
        /// <returns>Hashed <see cref="string"/>.</returns>
        string CreateOneWayHash(string inValue, HashAlgorithm HashAlgorithm = HashAlgorithm.MD5);
    }
}