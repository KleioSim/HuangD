using System;
using System.Security.Cryptography;
using System.Text;

namespace HuangD.Sessions.Utilties;

public static class RandomBuilder
{
    public static Random Build(string seed)
    {
        var algo = SHA1.Create();
        var hash = BitConverter.ToInt32(algo.ComputeHash(Encoding.UTF8.GetBytes(seed)), 0);

        var random = new Random(hash);
        return random;
    }
}
