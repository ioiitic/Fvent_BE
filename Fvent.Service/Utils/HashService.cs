using System;
using System.Security.Cryptography;
using System.Text;

namespace Fvent.Service.Utils;

public static class HashService
{
    public static string ToSHA256(string text)
    {
        var sb = new StringBuilder();
        var result = SHA256.HashData(Encoding.UTF8.GetBytes(text));

        for (int i = 0; i < result.Length; i++)
            sb.Append(result[i].ToString("x2"));

        return sb.ToString();
    }
}
