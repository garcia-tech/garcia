using System;
using System.Collections.Generic;
using System.Text;

namespace GarciaCore.Infrastructure
{
    public interface IRandomNumberGenerator
    {
        int Generate(int minimumValue, int maximumValue);
        string GenerateKey(int length);
    }
}
