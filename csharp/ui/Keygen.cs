// Shared key generation core. Compiled into both the CLI and the GUI build.
// Algorithm is a 1:1 port of Pro_Keygen.py.

using System;
using System.Text;

public static class Keygen
{
    // Base32 alphabet. Note the tail is ...W X Z Y, so index 35 is 'Y',
    // which the third-character picker deliberately excludes (0..34).
    const string Table = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXZY";

    // One shared instance: a fresh Random() per click can repeat the seed
    // within the same millisecond.
    static readonly Random Rng = new Random();

    public static string Generate(int product)
    {
        var sb4 = new StringBuilder(5);
        for (int i = 0; i < 5; i++) sb4.Append(Table[Rng.Next(0, Table.Length)]);
        string key4th = sb4.ToString();

        // low 16 bits random, product goes into bit 21-22
        ulong low4B = (ulong)Rng.Next(0x2580, 0x10000) + ((ulong)product << 21);
        ulong mid4B = (ulong)Rng.Next(0, 0x10000);
        ulong high4B = Handle(key4th);

        ulong v17 = Crc32Like(low4B, mid4B, high4B) & 0x1FFFFFF;
        ulong v18 = v17 ^ (v17 << 7);
        string key5th = HandleRe(v17, 5);
        string k07 = HandleRe(low4B ^ 0x12345678 ^ v18, 7);
        string k714 = HandleRe(mid4B ^ 0x87654321 ^ v18, 7);

        // 3rd char is a filler that the checker drops; Next(0,35) keeps 'Y' out
        return k07.Substring(0, 2) + Table[Rng.Next(0, 35)] + k07.Substring(3, 2)
             + "-" + k07.Substring(5, 2) + k714.Substring(0, 3)
             + "-" + k714.Substring(3, 4) + k07[2]
             + "-" + key4th
             + "-" + key5th;
    }

    static ulong Handle(string s)
    {
        ulong res = 0;
        for (int i = s.Length - 1; i >= 0; i--)
        {
            res *= 32;
            char c = s[i];
            if (c == 'W') continue;
            else if (c == 'X') res += 24;
            else if (c == 'Y') res += 1;
            else if (c == 'Z') res += 18;
            else if (c >= '0' && c <= '9') res += (ulong)(c - '0');
            else res += (ulong)(c - 'A' + 10);
        }
        return res;
    }

    static string HandleRe(ulong s, int len)
    {
        var sb = new StringBuilder(len);
        for (int i = 0; i < len; i++)
        {
            ulong t = s % 32;
            s /= 32;
            if (t == 0) sb.Append('W');
            else if (t == 24) sb.Append('X');
            else if (t == 1) sb.Append('Y');
            else if (t == 18) sb.Append('Z');
            else if (t <= 9) sb.Append((char)('0' + t));
            else sb.Append((char)('A' + t - 10));
        }
        return sb.ToString();
    }

    // Python builds n = (high << 64) + (mid << 32) + low and walks its 12 low
    // bytes. A ulong would overflow, so the bytes are fed directly.
    static uint Crc32Like(ulong low, ulong mid, ulong high)
    {
        byte[] b = new byte[12];
        for (int i = 0; i < 4; i++)
        {
            b[i] = (byte)(low >> (8 * i));
            b[4 + i] = (byte)(mid >> (8 * i));
            b[8 + i] = (byte)(high >> (8 * i));
        }

        uint res = 0;
        for (int i = 0; i < 12; i++)
        {
            uint v2 = (uint)b[i] << 24;
            if (i != 0) res ^= v2;
            else res = ~v2;
            for (int j = 0; j < 8; j++)
            {
                // Python: res *= 2; if res >= 0xffffffff { res &= 0xffffffff; res ^= poly; }
                // Carry out of bit 31 is exactly the overflow condition.
                bool carry = (res & 0x80000000u) != 0;
                res <<= 1;
                if (carry) res ^= 0x4C11DB7u;
            }
        }
        return res;
    }
}
