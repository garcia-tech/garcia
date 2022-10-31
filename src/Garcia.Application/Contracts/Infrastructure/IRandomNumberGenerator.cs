namespace Garcia.Application.Contracts.Infrastructure
{
    public interface IRandomNumberGenerator
    {
        /// <summary>
        /// Generates a random number between minimum and maximum value.
        /// </summary>
        /// <param name="minimumValue"></param>
        /// <param name="maximumValue"></param>
        /// <returns></returns>
        int Generate(int minimumValue, int maximumValue);
        /// <summary>
        /// Generates a key based on the given length.
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        string GenerateKey(int length);
    }
}