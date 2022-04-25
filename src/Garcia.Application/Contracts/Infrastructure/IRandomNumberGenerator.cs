namespace Garcia.Application.Contracts.Infrastructure
{
    public interface IRandomNumberGenerator
    {
        int Generate(int minimumValue, int maximumValue);
        string GenerateKey(int length);
    }
}