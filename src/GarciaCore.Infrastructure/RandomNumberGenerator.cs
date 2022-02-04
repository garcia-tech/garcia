using System;
using System.Security.Cryptography;

namespace GarciaCore.Infrastructure
{
    public class RandomNumberGenerator : IRandomNumberGenerator
    {
        private readonly RNGCryptoServiceProvider _generator = new RNGCryptoServiceProvider();

        public int Generate(int minimumValue, int maximumValue)
        {
            var randomNumber = new byte[1];
            _generator.GetBytes(randomNumber);
            var asciiValueOfRandomCharacter = Convert.ToDouble(randomNumber[0]);
            var multiplier = Math.Max(0, (asciiValueOfRandomCharacter / 255d) - 0.00000000001d);
            var range = maximumValue - minimumValue + 1;
            var randomValueInRange = Math.Floor(multiplier * range);
            return (int)(minimumValue + randomValueInRange);
        }

        public string GenerateKey(int length)
        {
            string number = Generate(100000000, int.MaxValue).ToString();
            int numberLength = number.Length;
            return length >= numberLength ? number : number.Substring(0, length);
        }
    }
}