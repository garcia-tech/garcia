namespace GarciaCore.Application
{
    public interface IRandomNumberGenerator
    {
        int Generate(int minimumValue, int maximumValue);
        string GenerateKey(int length);
    }
}