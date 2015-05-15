using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSim
{
    public static class RandomMersene
    {
        static UInt32 N = 624;
        static int M = 394;
        static UInt32 MATRIX_A = (UInt32)0x9908b0dfUL;   /* vectorul a */

        static UInt32 UPPER_MASK = (UInt32)0x80000000UL;
        static UInt32 LOWER_MASK = (UInt32)0x7fffffffUL;

        static UInt32[] x = new UInt32[625];
        static UInt32 mti = (UInt32)N + 1; /* mti=N+1 inseamna ca  x[N] nu este initializat */
        public static void init_genrand(long s)
        {
            init_genrand((UInt32)s);
        }
        public static void init_genrand(UInt32 s)
        {  /* modul de initializare a fost sugerat de Donald Knuth*/
            x[0] = s & (UInt32)0xffffffffUL;
            for (mti = 1; mti < N; mti++)
            {
                x[mti] =
                ((UInt32)1812433253UL * (x[mti - 1] ^ (x[mti - 1] >> 30)) + mti);

                x[mti] &= (UInt32)0xffffffffUL;
            }
        }

        public static UInt32 genrand_int32()
        {
            UInt32 y;
            UInt32[] v = new UInt32[] { (UInt32)0x0UL, MATRIX_A };


            if (mti >= N)
            { /* generate N numere simultan */
                int j;

                if (mti == N + 1)   /* daca init_genrand() nu a fost apelat, */
                    init_genrand((UInt32)5489UL); /* un seed implicit */

                for (j = 0; j < N - M; j++)
                {
                    y = (x[j] & UPPER_MASK) | (x[j + 1] & LOWER_MASK);
                    x[j] = x[j + M] ^ (y >> 1) ^ v[y & 0x1UL];
                }
                for (; j < N - 1; j++)
                {
                    y = (x[j] & UPPER_MASK) | (x[j + 1] & LOWER_MASK);
                    x[j] = x[j + (M - N)] ^ (y >> 1) ^ x[y & 0x1UL];
                }
                y = (x[N - 1] & UPPER_MASK) | (x[0] & LOWER_MASK);
                x[N - 1] = x[M - 1] ^ (y >> 1) ^ v[y & 0x1UL];

                mti = 0;
            }

            y = x[mti++];

            /* Operatiile de temperare */
            y ^= (y >> 11);
            y ^= (y << 7) & (UInt32)0x9d2c5680UL;
            y ^= (y << 15) & (UInt32)0xefc60000UL;
            y ^= (y >> 18);

            return y;
        }
        /* generator de numere din intervalul inchis [0,1] */
        public static double genrand_real1()
        {
            return genrand_int32() * (1.0 / 4294967295.0);
            /* 4294967295= 2^32-1 */
        }
        public static bool genrand_bool()
        {
            return genrand_real1() > 0.5;
        }
    }
}
