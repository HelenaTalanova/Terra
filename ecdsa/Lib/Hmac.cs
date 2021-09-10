using System.Security.Cryptography;

namespace TerraJigu.ecdsa.Lib
{
    /// <summary />
    public static class Hmac
    {
        /// <summary>
        /// Computes RFC 2104-compliant HMAC signature
        /// </summary>
        public static byte[] SHA256CompileHash(byte[] key, byte[] data)
        {
            using var hmSha1 = new HMACSHA256(key);
            return hmSha1.ComputeHash(data);
        }

        public static byte[] SHA512CompileHash(byte[] key, byte[] data)
        {
            using var hmSha1 = new HMACSHA512(key);
            return hmSha1.ComputeHash(data);
        }
    }
}
