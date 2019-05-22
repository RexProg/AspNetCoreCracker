using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace AspNetCoreCracker
{
    public static class Crypto
    {
        public static bool VerifyHashedPassword(string hashedPassword, string password)
        {
            if (hashedPassword == null) return false;
            if (password == null) throw new ArgumentNullException("password");
            var array = Convert.FromBase64String(hashedPassword);
            if (array.Length != 49 || array[0] != 0) return false;
            var array2 = new byte[16];
            Buffer.BlockCopy(array, 1, array2, 0, 16);
            var array3 = new byte[32];
            Buffer.BlockCopy(array, 17, array3, 0, 32);
            byte[] bytes;
            using (var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, array2, 1000))
            {
                bytes = rfc2898DeriveBytes.GetBytes(32);
            }

            return ByteArraysEqual(array3, bytes);
        }

        [MethodImpl(MethodImplOptions.NoOptimization)]
        private static bool ByteArraysEqual(byte[] a, byte[] b)
        {
            if (ReferenceEquals(a, b)) return true;
            if (a == null || b == null || a.Length != b.Length) return false;
            var flag = true;
            for (var i = 0; i < a.Length; i++) flag &= a[i] == b[i];
            return flag;
        }
    }
}